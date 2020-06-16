using System;
using System.Collections.Generic;
//using System.Linq;
using Assets.Scripts.Framework.GalaSports.Core;
//using Assets.Scripts.Module.Phone.Data;
using Com.Proto;
using DataModel;
using game.main;
using Google.Protobuf.Collections;
using Newtonsoft.Json;
using UnityEngine;

public enum PhoneModeType
{
    Sms,
    Call,
    Friend,
    Weibo
}

public class PhoneData : Model
{
    // private RepeatedField<MicroBlogRulePB> _microWeiboRules;

    private List<FriendCircleVo> _userFriendCircleList;
    private List<WeiboVo> _userWeiboList;

    private Dictionary<int, List<MySmsOrCallVo>> _npcSmsDic;
    private Dictionary<int, List<MySmsOrCallVo>> _npcCallDic;

    public Dictionary<int, List<MySmsOrCallVo>> NpcSmsDic => _npcSmsDic;
    public Dictionary<int, List<MySmsOrCallVo>> NpcCallDic => _npcCallDic;
    public List<FriendCircleVo> UserFriendCircleList => _userFriendCircleList;
    public List<WeiboVo> UserWeiboList => _userWeiboList;

    List<NpcInfo> _npcInfos;//Npc信息

    public List<NpcInfo> npcInfos
    {
        get
        {
            return _npcInfos;
        }
    }

    public NpcInfo GetNpcInfoByNpcId(int id)
    {
        return _npcInfos.Find((m) => { return m.NpcId == id; });
    }
    /// <summary>
    /// 初始化数据
    /// </summary>
    /// <param name="res"></param>
    public void InitData(UserPhoneDataRes res)
    {
        LoadNpcInfo();
        SetMyData(res.MsgOrCalls);
        SetFriendCircleData(res.FriendCircles);
        SetWeiboData(res.MicroBlogs);
    }

    public bool IsSmsOrCallTipsShow(MySmsOrCallVo vo)
    {
        Dictionary<int, List<MySmsOrCallVo>> dirc = vo.SceneId < 10000 ? _npcSmsDic : _npcCallDic;
        if (!dirc.ContainsKey(vo.NpcId))
            return true;
        //检查是否有未读完的消息
        List<MySmsOrCallVo> list = dirc[vo.NpcId];
        MySmsOrCallVo t_vo;
        for (int i = 0; i < list.Count; i++)
        {
            t_vo = list[i];
            if (!t_vo.IsReaded)
                return false;
        }
        return true;
    }


    public void AddSmsOrCallData(MySmsOrCallVo vo)
    {
        Dictionary<int, List<MySmsOrCallVo>> dirc = vo.SceneId < 10000 ? _npcSmsDic : _npcCallDic;
        if (!dirc.ContainsKey(vo.NpcId))
        {
            dirc[vo.NpcId] = new List<MySmsOrCallVo>();
        }
        if (dirc[vo.NpcId].Find((m) => { return m.SceneId == vo.SceneId; }) != null)
        {
            return;
        }
        if (!dirc[vo.NpcId].Contains(vo))
        {
            dirc[vo.NpcId].Add(vo);
        }
    }

    public void AddFriendCircleData(FriendCircleVo vo)
    {
        if (_userFriendCircleList.Contains(vo))
            return;
        if (_userFriendCircleList.Find((m) => { return m.SceneId == vo.SceneId; }) != null)
        {
            return;
        }
        _userFriendCircleList.Add(vo);
    }

    public void AddWeiboData(WeiboVo vo)
    {
        if (_userWeiboList.Contains(vo))
            return;
        if (_userWeiboList.Find((m) => { return m.SceneId == vo.SceneId; }) != null)
        {
            return;
        }
        _userWeiboList.Add(vo);
    }

    public void UpdateFriendCircleData(UserFriendCirclePB friendCirclePb)
    {
        //todo 更新数据需要优化
        var smsItem = _userFriendCircleList.Find((item) => { return item.SceneId == friendCirclePb.SceneId; });
        smsItem.PublishState = friendCirclePb.PubState == 1;
        smsItem.PublishTime = friendCirclePb.PubTime;
        smsItem.CreateTime = friendCirclePb.CreateTime;
        foreach (var v in friendCirclePb.SelectIds)
        {
            if (!smsItem.SelectIds.Contains(v))
            {
                smsItem.curOperateSelectID = v;
            }
        }
        if (smsItem.curOperateSelectID == -1)
        {
            smsItem.curOperateSelectID = 0;
        }
        smsItem.curOperateTime = ClientTimer.Instance.GetCurrentTimeStamp();
        smsItem.SelectIds.Clear();
        smsItem.SelectIds.AddRange(friendCirclePb.SelectIds);
    }

    public void UpdateWeiboData(UserMicroBlogPB MicroBlog)
    {
        var smsItem = _userWeiboList.Find((item) => { return item.SceneId == MicroBlog.SceneId; });
        smsItem.IsLike = MicroBlog.Like == 1;
        smsItem.IsPublish = smsItem.WeiboRuleInfo.weiboSceneInfo.NpcId == 0 ? MicroBlog.PubState == 1 : true;
    }

    public void UpdateSmsData(UserMsgOrCallPB MsgOrCall)
    {
        if (!_npcSmsDic.ContainsKey(MsgOrCall.NpcId))
        {
            Debug.LogError("UpdateSmsData   UnContainsKey " + MsgOrCall.NpcId);
        }
        var smsList = _npcSmsDic[MsgOrCall.NpcId];

        var smsItem = smsList.Find((item) => { return item.SceneId == MsgOrCall.SceneId; });

        smsItem.IsReaded = MsgOrCall.ReadState == 1;
        smsItem.selectIds = new List<int>();
        smsItem.FinishTime = MsgOrCall.FinishTime;
        smsItem.selectIds.AddRange(MsgOrCall.SelectIds);
        smsItem.listenIds = new List<int>();
        smsItem.listenIds.AddRange(MsgOrCall.ListenIds);
    }

    private void UpdateCallDataByLocal(int sceneId, List<int> selects,bool isFinished=false)
    {
        MySmsOrCallVo vo = null;
        foreach (var v in _npcSmsDic)
        {
            foreach(var v1 in v.Value)
            {
                if(v1.SceneId==sceneId)
                {
                    v1.selectIds = selects;
                    v1.IsReaded = isFinished;
                    return;
                }
            }
        }

    }

    public void UpdateCallData(UserMsgOrCallPB userMsgOrCallPB)
    {
        UserMsgOrCallPB MsgOrCall = userMsgOrCallPB;
        if (!_npcCallDic.ContainsKey(MsgOrCall.NpcId))
        {
            Debug.LogError("UpdateSmsData   UnContainsKey " + MsgOrCall.NpcId);
        }

        var callList = _npcCallDic[MsgOrCall.NpcId];
        var callItem = callList.Find((item) => { return item.SceneId == MsgOrCall.SceneId; });
        //var smsItem = _userCallList.Find((item) => { return item.SceneId == readMsgOrCallRes.MsgOrCall.SceneId; });
        callItem.IsReaded = MsgOrCall.ReadState == 1;
        callItem.selectIds = new List<int>();
        callItem.FinishTime = MsgOrCall.FinishTime;
        callItem.selectIds.AddRange(MsgOrCall.SelectIds);
    }



    public static FriendCircleVo TransFriendCircleData(UserFriendCirclePB data)
    {
        var userMsg = data;
        var friendCircleInfo = new FriendCircleVo()
        {
            UserId = data.UserId,
            SceneId = data.SceneId,
            PublishState = data.PubState == 1,
            CreateTime = data.CreateTime,
            PublishTime = data.PubTime,
            curOperateSelectID = -1,
            curOperateTime = 0,
            SelectIds = new List<int>(),
        };

        if (data.SelectIds.Count > 0)
        {
            friendCircleInfo.SelectIds.AddRange(data.SelectIds);
        }
        string text = new AssetLoader().LoadTextSync(AssetLoader.GetPhoneDataPath(data.SceneId.ToString()));
        if (text == "")
            return null;
        FriendCircleInfo info = JsonConvert.DeserializeObject<FriendCircleInfo>(text);
        friendCircleInfo.FriendCircleRuleInfo = info;
        return friendCircleInfo;
    }

    public static WeiboVo TransWeiboData(UserMicroBlogPB data)
    {
        var weiboInfo = new WeiboVo()
        {
            UserId = data.UserId,
            SceneId = data.SceneId,
            IsLike = data.Like == 1,
            IsComment = data.SelectState == 1,
            IsPublish = data.PubState == 1,
            CommentIndex = data.SelectIndex,
            CreateTime = data.CreateTime,
            //CommentContents = new List<string>(),
        };
        // weiboInfo.IsPublish = weiboInfo.WeiboRuleInfo.weiboSceneInfo.NpcId == 0 ? v.PubState == 1 : true;
        string text = new AssetLoader().LoadTextSync(AssetLoader.GetPhoneDataPath(data.SceneId.ToString()));
        if (text == "")
            return null;
        WeiboInfo info = JsonConvert.DeserializeObject<WeiboInfo>(text);
        weiboInfo.WeiboRuleInfo = info;
        return weiboInfo;
    }
    public static MySmsOrCallVo TransSmsOrCallData(UserMsgOrCallPB data)
    {
        var userMsg = data;
        var vo = new MySmsOrCallVo();
        vo.SceneId = userMsg.SceneId;
        vo.NpcId = userMsg.NpcId;
        vo.CreateTime = userMsg.CreateTime;
        vo.FinishTime = userMsg.FinishTime;
        vo.IsReaded = userMsg.ReadState == 1;
        vo.selectIds = new List<int>();
        vo.listenIds = new List<int>();
        vo.selectIds.AddRange(userMsg.SelectIds);
        vo.listenIds.AddRange(userMsg.ListenIds);
        return vo;
    }
    /// <summary>
    /// 获取用户已触发的短信/电话信息
    /// </summary>
    /// <param name="res"></param>
    private void SetMyData(RepeatedField<UserMsgOrCallPB> res)
    {
        if (_npcSmsDic == null)
        {
            _npcSmsDic = new Dictionary<int, List<MySmsOrCallVo>>();
        }else
        {
            _npcSmsDic.Clear();
        }

        if (_npcCallDic == null)
        {
            _npcCallDic = new Dictionary<int, List<MySmsOrCallVo>>();
        }
        else
        {
            _npcCallDic.Clear();
        }
        RepeatedField<UserMsgOrCallPB> userData = res;
        for (int i = 0; i < userData.Count; i++)
        {
            var vo = TransSmsOrCallData(userData[i]);
            vo.IsLocal = false;
            AddMySmsOrCallVo(vo);
        }

        //Do添加新手引导
        // UserMsgOrCallPB guideMsg = new UserMsgOrCallPB();
        // guideMsg.SceneId = 101;
        // guideMsg.NpcId = 1000;
        // guideMsg.CreateTime = 0;
        // guideMsg.FinishTime = 0;
        // guideMsg.SelectIds.AddRange(GetSelectsLocal(guideMsg.SceneId));
        // guideMsg.ReadState = GetIsFinishedLocal(guideMsg.SceneId);
        // var lvo = TransSmsOrCallData(guideMsg);
        // lvo.IsLocal = true;
        // AddMySmsOrCallVo(lvo);
        return;
    }

    public void ReCordSelect(int sceneId, List<int> selects,bool isFinish)
    {
        string strKey = GlobalData.PlayerModel.PlayerVo.UserId.ToString() + AppConfig.Instance.serverId + "PhoneReadRecord" + sceneId;
        string selectIds = "";

        for (int i = 0; i < selects.Count; i++)
        {
            selectIds = selectIds + selects[i].ToString() + ",";
        }
        if (selects.Count > 0)
        {
            selectIds = selectIds.Substring(0, selectIds.Length - 1);
        }
        Debug.LogError(selectIds);
        PlayerPrefs.SetString(strKey, selectIds);
        if (isFinish)
        {
            string strKey2 = GlobalData.PlayerModel.PlayerVo.UserId.ToString() + AppConfig.Instance.serverId + "PhoneReadFnished" + sceneId;
            PlayerPrefs.SetInt(strKey2, 1);
        }

        UpdateCallDataByLocal(sceneId, selects, isFinish);
    }

    private int GetIsFinishedLocal(int sceneId)
    {
        string strKey2 = GlobalData.PlayerModel.PlayerVo.UserId.ToString() + AppConfig.Instance.serverId + "PhoneReadFnished" + sceneId;
        return PlayerPrefs.GetInt(strKey2, 0);
    }

    public List<int> GetSelectsLocal(int sceneId)
    {
        string strKey = GlobalData.PlayerModel.PlayerVo.UserId.ToString() + AppConfig.Instance.serverId + "PhoneReadRecord" + sceneId;
        string selectIds = PlayerPrefs.GetString(strKey, "");
        List<int> selects = new List<int>();
        string[] selectIds2 = selectIds.Split(',');

        for (int i = 0; i < selectIds2.Length; i++) 
        {
            if(selectIds2[i]=="")
            {
                continue;
            }
            int seId = 0;
            if (!int.TryParse(selectIds2[i],out seId))
            {
                continue;
            }

            selects.Add(seId);
        }
        return selects;
    }



    private void AddMySmsOrCallVo(MySmsOrCallVo vo)
    {
        if(_npcSmsDic == null)
        {
            _npcSmsDic = new Dictionary<int, List<MySmsOrCallVo>>();
        }

        if (_npcCallDic==null)
        {
            _npcCallDic = new Dictionary<int, List<MySmsOrCallVo>>();
        }


        if (vo.SceneId < 10000)
        {
            if (!_npcSmsDic.ContainsKey(vo.NpcId))
            {
                var list = new List<MySmsOrCallVo>();
                _npcSmsDic.Add(vo.NpcId, list);
            }
            _npcSmsDic[vo.NpcId].Add(vo);
        }
        else if (vo.SceneId < 20000)
        {
            if (!_npcCallDic.ContainsKey(vo.NpcId))
            {
                var list = new List<MySmsOrCallVo>();
                _npcCallDic.Add(vo.NpcId, list);

            }
            _npcCallDic[vo.NpcId].Add(vo);
        }
    }



    private void SetFriendCircleData(RepeatedField<UserFriendCirclePB> res)
    {
        if (_userFriendCircleList == null)
        {
            _userFriendCircleList = new List<FriendCircleVo>();
        }
        else
        {
            _userFriendCircleList.Clear();
        }
        List<int> ids = new List<int>();
        foreach (var v in res)
        {
            var friendCircleInfo = new FriendCircleVo()
            {
                UserId = v.UserId,
                SceneId = v.SceneId,
                PublishState = v.PubState == 1,
                CreateTime = v.CreateTime,
                PublishTime = v.PubTime,
                curOperateSelectID = -1,
                curOperateTime = 0,
                SelectIds = new List<int>(),
            };

            if (v.SelectIds.Count > 0)
            {
                friendCircleInfo.SelectIds.AddRange(v.SelectIds);
            }
            _userFriendCircleList.Add(friendCircleInfo);
            ids.Add(v.SceneId);
        }
        LoadFcRulesByIds(ids);
        //LoadFcFinishedCallback();
    }

    private void SetWeiboData(RepeatedField<UserMicroBlogPB> res)
    {

        if (_userWeiboList == null)
        {
            _userWeiboList = new List<WeiboVo>();
        }
        else
        {
            _userWeiboList.Clear();
        }

        List<int> ids = new List<int>();
        foreach (var v in res)
        {
            var weiboInfo = new WeiboVo()
            {
                UserId = v.UserId,
                SceneId = v.SceneId,
                IsLike = v.Like == 1,
                IsComment = v.SelectState == 1,
                IsPublish = v.PubState == 1,
                CommentIndex = v.SelectIndex,
                CreateTime = v.CreateTime,
                //CommentContents = new List<string>(),
            };

            // weiboInfo.IsPublish = weiboInfo.WeiboRuleInfo.weiboSceneInfo.NpcId == 0 ? v.PubState == 1 : true;
            _userWeiboList.Add(weiboInfo);
            ids.Add(v.SceneId);
        }
        LoadWeiboRulesByIds(ids);
    }

    private void LoadNpcInfo()
    {
        string text = new AssetLoader().LoadTextSync(AssetLoader.GetPhoneDataPath("NpcInfo"));
        _npcInfos = JsonConvert.DeserializeObject<List<NpcInfo>>(text);
    }
    public static SmsInfo LoadPhoneSmsRuleById(int id)
    {
        string text = new AssetLoader().LoadTextSync(AssetLoader.GetPhoneDataPath(id.ToString()));
        SmsInfo info = JsonConvert.DeserializeObject<SmsInfo>(text);  
        return info;
    }

    List<int> _fcIds;
    List<FriendCircleInfo> _fcRuleInfos;
    private void LoadFcRulesByIds(List<int> ids)
    {
        _fcIds = ids;
        _fcRuleInfos = new List<FriendCircleInfo>();
        foreach (var v in ids)
        {
            LoadPhoneFcRuleById(v);
        }
    }

    private void LoadFcRuleFinishs()
    {
        foreach (var v in _userFriendCircleList)
        {
            // v.friendCircleRuleInfo = _fcRuleInfos.Find((m) => { return v.SceneId == int.Parse(m.friendCircleSceneInfo.SceneId); });
        }
    }
    private void LoadPhoneFcRuleById(int id)
    {
        string text = new AssetLoader().LoadTextSync(AssetLoader.GetPhoneDataPath(id.ToString()));
        FriendCircleInfo info = JsonConvert.DeserializeObject<FriendCircleInfo>(text);

        _fcRuleInfos.Add(info);
        if (_fcRuleInfos.Count == _fcIds.Count)
        {
            LoadFcRuleFinishs();
        }
    }

    List<int> _weiboIds;
    List<WeiboInfo> _weiboRuleInfos;
    private void LoadWeiboRulesByIds(List<int> ids)
    {
        _weiboIds = ids;
        _weiboRuleInfos = new List<WeiboInfo>();
        foreach (var v in ids)
        {
            LoadPhoneWeiboRuleById(v);
        }
    }


    private void LoadWeiboRuleFinishs()
    {
        foreach (var v in _userWeiboList)
        {
            v.WeiboRuleInfo = _weiboRuleInfos.Find((m) => { return v.SceneId == int.Parse(m.weiboSceneInfo.SceneId); });
        }
    }
    private void LoadPhoneWeiboRuleById(int id)
    {
        string text = new AssetLoader().LoadTextSync(AssetLoader.GetPhoneDataPath(id.ToString()));
        WeiboInfo info = JsonConvert.DeserializeObject<WeiboInfo>(text);
        _weiboRuleInfos.Add(info);
        if (_weiboRuleInfos.Count == _weiboIds.Count)
        {
            LoadWeiboRuleFinishs();
        }
    }

    //get Npc name by ID
    public static string GetNpcNameById(int roleId)
    {
        string cardName = "";
        switch (roleId)
        {
            case 0:
                cardName = GlobalData.PlayerModel.PlayerVo.UserName;
                break;
            case 1:
                cardName = I18NManager.Get("Common_Role1");
                break;
            case 2:
                cardName = I18NManager.Get("Common_Role2");
                break;
            case 3:
                cardName = I18NManager.Get("Common_Role3");
                break;
            case 4:
                cardName = I18NManager.Get("Common_Role4");
                break;
        }
        return cardName;
    }

    public static string GetHeadPath(int NpcId)
    {
        string path = "";
        switch (NpcId)
        {
            case 0:
                path = "Head/PlayerHead/PlayerHead";
                break;
            case 1:
                path = "Head/1108";
                break;
            case 2:
                path = "Head/2008";
                break;
            case 3:
                path = "Head/3108";
                break;
            case 4:
                path = "Head/EvolutionHead/4112";
                break;
            default:
                path = "Head/OtherHead/"+NpcId;
                break;
        }
        return path;
    }


}