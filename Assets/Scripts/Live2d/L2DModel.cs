using live2d;
using live2d.framework;
using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Service;
using game.main;
using UnityEngine;
using DataModel;

public class L2DModel : L2DBaseModel
{
    private ModelSetting modelSetting = null;

    private L2DModelMatrix matrix;
    private string _baseDir;

    private List<string> _expressionList;

    public List<string> ExpressionList
    {
        get { return _expressionList; }
        set { _expressionList = value; }
    }

    private List<string> _motionList;
    private bool _loop;
    private int _currentMontionIndex;
    private string _modelId;

    private float _interval = -1;
    private float _setExpressionTime = 0;

    public static bool HasResource(string modelId)
    {
        string data = Live2DFramework.GetPlatformManager().loadString(AssetLoader.GetLive2dModelJsonById(modelId));
        return !string.IsNullOrEmpty(data);
    }

    public List<string> MotionList
    {
        get { return _motionList; }
        set { _motionList = value; }
    }

    bool _isIdle = true;
    private List<string> _donotUnloadIds;

    public bool IsIdle
    {
        get { return Time.realtimeSinceStartup > _setExpressionTime + _interval; }
        // set { _isIdle = value; }
    }

    public void LoadAssets(string modelId, List<string> donotUnloadIds)
    {
        _donotUnloadIds = donotUnloadIds;
        if (_modelId != null)
            UnloadAsset();

        _modelId = modelId;
        _expressionList = new List<string>();
        _motionList = new List<string>();

        var data = Live2DFramework.GetPlatformManager().loadString(AssetLoader.GetLive2dModelJsonById(modelId));
        Init(data, modelId);
    }

    public void Reload()
    {
        var len = modelSetting.GetTextureNum();
        for (int i = 0; i < len; i++)
        {
            loadTexture(i, _baseDir + modelSetting.GetTextureFile(i));
        }
    }

    private void Init(string modelJson, string modelId)
    {
        modelSetting = new ModelSettingJson(modelJson);

        _baseDir = AssetLoader.GetLive2dDirById(modelId) + "/";

        // Live2D Model
        if (modelSetting.GetModelFile() != null)
        {
            loadModelData(_baseDir + modelSetting.GetModelFile());

            var len = modelSetting.GetTextureNum();
            for (int i = 0; i < len; i++)
            {
                loadTexture(i, _baseDir + modelSetting.GetTextureFile(i));
            }
        }

        string[] motionGroup = modelSetting.GetMotionGroupNames();

        for (int i = 0; i < motionGroup.Length; i++)
        {
            int num = modelSetting.GetMotionNum(motionGroup[i]);
            if (num != 0)
            {
                for (int j = 0; j < num; j++)
                {
                    _motionList.Add(modelSetting.GetMotionFile(motionGroup[i], j));
                }
            }
        }

        // Expression
        if (modelSetting.GetExpressionNum() != 0)
        {
            var len = modelSetting.GetExpressionNum();
            for (int i = 0; i < len; i++)
            {
                string name = modelSetting.GetExpressionName(i);
                loadExpression(name, _baseDir + modelSetting.GetExpressionFile(i));
                _expressionList.Add(name);
            }
        }

        // Physics
        if (modelSetting.GetPhysicsFile() != null)
        {
            loadPhysics(_baseDir + modelSetting.GetPhysicsFile());
        }

        // Pose
        if (modelSetting.GetPoseFile() != null)
        {
            loadPose(_baseDir + modelSetting.GetPoseFile());
        }

        for (int i = 0; i < modelSetting.GetInitParamNum(); i++)
        {
            string id = modelSetting.GetInitParamID(i);
            float value = modelSetting.GetInitParamValue(i);
            live2DModel.setParamFloat(id, value);
        }

        for (int i = 0; i < modelSetting.GetInitPartsVisibleNum(); i++)
        {
            string id = modelSetting.GetInitPartsVisibleID(i);
            float value = modelSetting.GetInitPartsVisibleValue(i);
            live2DModel.setPartsOpacity(id, value);
        }
    }

    public void Draw(LIVE2DVIEWTYPE viewType = LIVE2DVIEWTYPE.STORY)
    {
        if (live2DModel == null)
            return;

        Update(viewType);
        live2DModel.draw();
    }

    public void Update(LIVE2DVIEWTYPE viewType)
    {
        if (live2DModel == null)
        {
            Debug.Log("Can not update there is no model data");
            return;
        }

        if (mainMotionManager.isFinished())
        {
            if (viewType == LIVE2DVIEWTYPE.MAINPANLE)
            {
                EXPRESSIONTRIGERTYPE eType = EXPRESSIONTRIGERTYPE.NORMAL;
                ExpressionInfo expressionInfo = ClientData.GetRandomExpression(_modelId, eType);
                if (expressionInfo == null)
                    return;
                string name = ExpressionList[expressionInfo.Id];
                AMotion motion = expressions[name];

                int no = UnityEngine.Random.Range(0, MotionList.Count);
                StartMotion(L2DConst.MOTION_GROUP_IDLE, no, L2DConst.PRIORITY_IDLE, _loop);
                if (Time.realtimeSinceStartup > _setExpressionTime + _interval)
                    SetExpression(name);
            }
            else
            {
                StartMotion(L2DConst.MOTION_GROUP_IDLE, _currentMontionIndex, L2DConst.PRIORITY_IDLE, _loop);
            }
        }

        //-----------------------------------------------------------------
        live2DModel.loadParam();
        mainMotionManager.updateParam(live2DModel);


        //眨眼睛
        eyeBlink?.updateParam(live2DModel);
        live2DModel.saveParam();

        expressionManager.updateParam(live2DModel);

        if (physics != null)
            physics.updateParam(live2DModel);


        // live2DModel.update();
    }

    /// <summary>
    /// 眨眼
    /// </summary>
    /// <param name="interval">单位：毫秒</param>
    public void StartEyeBlink(int interval = 5000)
    {
        eyeBlink = new L2DEyeBlink();
        eyeBlink.setInterval(interval);
    }

    public void StopEyeBlink()
    {
        eyeBlink = null;
    }

    public void StartMotion(string group, int no, int priority, bool loop)
    {
        _loop = loop;
        _currentMontionIndex = no;

        string motionName = modelSetting.GetMotionFile(group, no);

        if (motionName == null || motionName.Equals(""))
        {
            if (L2DConst.DEBUG_LOG) Debug.Log("Motion name is invalid");
            return;
        }


        if (priority == L2DConst.PRIORITY_FORCE)
        {
            mainMotionManager.setReservePriority(priority);
        }
        else if (!mainMotionManager.reserveMotion(priority))
        {
            if (L2DConst.DEBUG_LOG)
            {
                Debug.Log("Do not play because book already playing, or playing a motion already." + motionName);
            }

            return;
        }

        AMotion motion = null;
        string name = group + "_" + no;

        if (motions.ContainsKey(name))
        {
            motion = motions[name];
        }

        if (motion == null)
        {
            motion = loadMotion(name, _baseDir + motionName);
        }

        if (motion == null)
        {
            Debug.Log("Failed to read the motion." + motionName);
            mainMotionManager.setReservePriority(0);
            return;
        }

        motion.setFadeIn(modelSetting.GetMotionFadeIn(group, no));
        motion.setFadeOut(modelSetting.GetMotionFadeOut(group, no));
        mainMotionManager.startMotionPrio(motion, priority);
    }

    public void SetExpression(string name, float interval = -1)
    {
        if (!expressions.ContainsKey(name)) return;
        if (L2DConst.DEBUG_LOG) Debug.Log("Setting expression : " + name);
        AMotion motion = expressions[name];

        if (Time.realtimeSinceStartup < _setExpressionTime + _interval)
        {
            return;
        }

        if (L2DConst.DEBUG_LOG) Debug.Log("Start expression : " + name);
        _interval = interval;
        _setExpressionTime = Time.realtimeSinceStartup;
        expressionManager.startMotionPrio(motion, L2DConst.PRIORITY_IDLE);
    }

    public void StopAll()
    {
        expressionManager.stopAllMotions();
        mainMotionManager.stopAllMotions();
        eyeBlink = null;
    }

    private bool CanUnload()
    {
        if (_donotUnloadIds != null && _donotUnloadIds.Contains(_modelId))
            return false;
        
        //当和主界面live2d id相同的时候 && 没有NetWorkManager.CookieStr，不卸载 
        if (GlobalData.PlayerModel.PlayerVo.Apparel != null &&
            _modelId == GlobalData.PlayerModel.PlayerVo.Apparel[0].ToString() &&
            SdkHelper.AccountAgent.IsLogin)
            return false;

        return true;
    }

    public void UnloadAsset()
    {
        if (CanUnload() == false)
            return;

        _baseDir = AssetLoader.GetLive2dDirById(_modelId) + "/";

        IPlatformManager pm = Live2DFramework.GetPlatformManager();

        pm.UnloadBytes(AssetLoader.GetLive2dModelJsonById(_modelId));

        // Live2D Model
        if (modelSetting.GetModelFile() != null)
        {
            pm.UnloadBytes(_baseDir + modelSetting.GetModelFile());

            var len = modelSetting.GetTextureNum();
            for (int i = 0; i < len; i++)
            {
                pm.UnloadTexture(_baseDir + modelSetting.GetTextureFile(i));
            }
        }

        // Expression
        if (modelSetting.GetExpressionNum() != 0)
        {
            var len = modelSetting.GetExpressionNum();
            for (int i = 0; i < len; i++)
            {
                pm.UnloadBytes(_baseDir + modelSetting.GetExpressionFile(i));
            }
        }

        // Physics
        if (modelSetting.GetPhysicsFile() != null)
        {
            pm.UnloadBytes(_baseDir + modelSetting.GetPhysicsFile());
        }

        // Pose
        if (modelSetting.GetPoseFile() != null)
        {
            pm.UnloadBytes(_baseDir + modelSetting.GetPoseFile());
        }


        string[] motionGroup = modelSetting.GetMotionGroupNames();

        for (int i = 0; i < motionGroup.Length; i++)
        {
            int num = modelSetting.GetMotionNum(motionGroup[i]);
            if (num != 0)
            {
                for (int j = 0; j < num; j++)
                {
                    pm.UnloadBytes(_baseDir + modelSetting.GetMotionFile(motionGroup[i], j));
                }
            }
        }

        _modelId = null;
    }

    public EXPRESSIONTRIGERTYPE TapEvent(float x, float y)
    {
        Debug.Log("tapEvent view x:" + x + " y:" + y);

        if (HitTest(L2DConst.HIT_AREA_HEAD, x, y))
        {
            Debug.LogError("Tapped face");
            return EXPRESSIONTRIGERTYPE.HEAD;
        }
        else if (HitTest(L2DConst.HIT_AREA_BODY, x, y))
        {
            Debug.LogError("Tapped body");
            return EXPRESSIONTRIGERTYPE.BODY;
        }

        return EXPRESSIONTRIGERTYPE.NORMAL;
    }

    public bool HitTest(string id, float testX, float testY)
    {
        if (modelSetting == null) return false;
        int len = modelSetting.GetHitAreasNum();
        for (int i = 0; i < len; i++)
        {
            if (id.Equals(modelSetting.GetHitAreaName(i)))
            {
                string drawID = modelSetting.GetHitAreaID(i);
                return hitTestSimple(drawID, testX, testY);
            }
        }

        return false;
    }
}