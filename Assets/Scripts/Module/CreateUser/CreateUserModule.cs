using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Core.Events;
using Assets.Scripts.Module;
using Assets.Scripts.Module.CreateUser;
using Com.Proto;
using Common;
using game.main;
using GalaSDKBase;
using UnityEngine;

public class CreateUserModule : ModuleBase
{
    private CreateUserPanel _createUserPanel;

    private bool _fromStory = false;
    private int _month;
    private int _day;

    private const string ShowCgFunc = "showCG";

    public override void LoadAssets()
    {
        //没有图集，不加载图集
    }

    public override void Init()
    {
        GuideManager.RegisterModule(this);
        EventDispatcher.AddEventListener<bool>(EventConst.OnCreateUser, OnCreateUser);

        Main.EnableBackKey = false;

        Main.UiCamera.backgroundColor = Color.black;


#if UNITY_EDITOR
        ClientTimer.Instance.DelayCall(() => { OnCgEnd(ShowCgFunc, "0"); }, 1f);
        FlowText.ShowMessage(I18NManager.Get("Guide_EditorShow"));
#else
            string tip = I18NManager.Get("CreateUser_CgTips");

            GalaSDKBaseFunction.CustomFunction(ShowCgFunc, tip);
            GalaSDKBaseCallBack.Instance.GALASDKCustomCallBackEvent += OnCgEnd;
            AudioManager.Instance.StopBackgroundMusic();
#endif
    }

    public override void SetData(params object[] paramsObjects)
    {
        if (paramsObjects != null && paramsObjects.Length > 0)
        {
            _fromStory = true;
        }
    }

    private void OnCreateUser(bool isSuccess)
    {
        if (isSuccess)
        {
            EventDispatcher.TriggerEvent(EventConst.CreateUserEnd);

            GuideManager.SetStatisticsRemoteGuideStep(GuideConst.MainLineStep_CreateUserName);
            
            new UpdateBirthdayService().SetBirthday(_month,  _day).Execute();
        }
    }

    private void OnCgEnd(string funcName, string time)
    {
        Debug.Log("CG Over");
        ModuleManager.Instance.EnterModule(ModuleConfig.MODULE_STORY, false, false, "0-1");
    }

    public override void OnMessage(Message message)
    {
        string name = message.Name;
        object[] body = message.Params;
        switch (name)
        {
            case MessageConst.MODULE_CREATE_USER_CONFIRM:
                EventDispatcher.TriggerEvent(EventConst.CreateRoleSubmit, (string) body[0]);
                break;
            case MessageConst.MODULE_CREATE_USER_END:
                EventDispatcher.TriggerEvent(EventConst.CreateUserEnd);
                break;
            case MessageConst.MODULE_CREATE_BIRTHDAY:
                _month = int.Parse(body[0] as string);
                _day = int.Parse(body[1] as string);
                break;
            case MessageConst.MOUDLE_GUIDE_RECEIVE_REMOTE_STEP:
                EventDispatcher.TriggerEvent(EventConst.CreateUserEnd);
                break;
        }
    }


    public override void OnShow(float delay)
    {
        base.OnShow(delay);

        if (!_fromStory) return;
        
        _createUserPanel = new CreateUserPanel();
        _createUserPanel.Init(this);
        _createUserPanel.Show(0);
    }

    public override void Remove(float delay)
    {
        base.Remove(delay);

        Main.UiCamera.backgroundColor = Color.white;

        EventDispatcher.RemoveEvent(EventConst.OnCreateUser);

        GalaSDKBaseCallBack.Instance.GALASDKCustomCallBackEvent -= OnCgEnd;

        Main.EnableBackKey = true;

        AudioManager.Instance.PlayDefaultBgMusic();
    }
}