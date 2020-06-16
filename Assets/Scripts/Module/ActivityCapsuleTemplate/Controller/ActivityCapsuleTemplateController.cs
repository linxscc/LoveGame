using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Core.Events;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module;
using Assets.Scripts.Module.NetWork;
using Assets.Scripts.Services;
using Com.Proto;
using Common;
using Componets;
using DataModel;
using game.main;
using Google.Protobuf.Collections;
using UnityEngine;
using Utils;

public class ActivityCapsuleTemplateController : Controller
{
    public ActivityCapsuleTemplateView View;

    private ActivityCapsuleTemplateStoryWindow _storyWindow;

    private CapsuleLevelVo _lastPlayLevel;
    
    public override void Init()
    {

        EventDispatcher.AddEventListener<string, System.Action>(EventConst.ActivityCapsuleTemplateWatchStory, SendWatchStoryReq);
        EventDispatcher.AddEventListener<string, System.Action>(EventConst.ActivityCapsuleTemplateWatchOverStory, SendWatchOverStoryReq);       
        //EventDispatcher.AddEventListener<int,CapsuleLevelVo>(EventConst.EnterCapsuleBattle,EnterCapsuleBattle);  
        EventDispatcher.AddEventListener(EventConst.CapsuleBattleOver,CapsuleBattleOver);
        EventDispatcher.AddEventListener<CapsuleLevelVo>(EventConst.OnClickCapsuleBattleEntrance,OnClickCapsuleBattleEntrance);
        EventDispatcher.AddEventListener(EventConst.DailyRefresh6,SixPointRefreshActivity);
    }

    private void SixPointRefreshActivity()
    {
        SendUserInfoReq();
    }

    private void OnClickCapsuleBattleEntrance(CapsuleLevelVo vo)
    {
        OpenBattleWindow(vo);
    }


    public override void Start()
    {
        //View.SetUiData(GetData<ActivityTemplateModel>().TemplateUiVo);
        SendRuleReq();
                                   
    }

    public override void OnMessage(Message message)
    {
        string name = message.Name;
        object[] body = message.Params;
        switch (name)
        {
            case MessageConst.CMD_ACTIVITY_CAPSULE_TEMPLATE_OPEN_STORY_WINDOW:
                OpenStoryWindow();
                break;
            case MessageConst.MODULE_ACTIVITY_CAPSULE_TEMPLATE_ACTIVITY_OVER:
                ModuleManager.Instance.GoBack();
                break;
            
        }
    }

    /// <summary>
    /// 进入扭蛋战斗
    /// </summary>
    /// <param name="num"></param>
    /// <param name="arg2"></param>
    private void EnterCapsuleBattle(int num, CapsuleLevelVo level)
    {
        _lastPlayLevel = level;
        if (num==0)
        {
            Main.ChangeMenu(MainMenuDisplayState.HideAll);    
            ModuleManager.Instance.EnterModule(ModuleConfig.MODULE_ACTIVITYCAPSULEBATTLE, false, true, level,
              GlobalData.LevelModel.CommentRule, GlobalData.LevelModel.CardNumRules);  
        }
        else
        {
            //发送扫荡
            byte[] data = NetWorkManager.GetByteData(new ActivityLevelSweepReq()
            {
                ActivityId = level.ActivityId,
                LevelId = level.LevelId,
                Num = num
            });
            LoadingOverlay.Instance.Show();
            NetWorkManager.Instance.Send<ActivityLevelSweepRes>(CMD.ACTIVITYCAPSULEC_SWEEP, data, OnSweep);
        } 
    }

    private void OnSweep(ActivityLevelSweepRes res)
    {
      GlobalData.CapsuleLevelModel.UpdateUserActivityLevelInfo(res.UserActivityLevelInfo);
      LoadingOverlay.Instance.Hide();
      EventDispatcher.RemoveEvent(EventConst.ShowLastCapsuleBattleWindow);
      EventDispatcher.AddEventListener(EventConst.ShowLastCapsuleBattleWindow, OnSweepEnd); 
      
      RepeatedField<GameResultPB> result = res.GameResult;
      CapsuleBattleSweepWindow win = PopupManager.ShowWindow<CapsuleBattleSweepWindow>("ActivityCapsuleBattle/Prefabs/CapsuleBattleSweepWindow");
      win.SetData(res.GameResult,ClientData.CustomerSelectedCapsuleLevel,result[0].Exp);
      win.MaskColor = new Color(0,0,0,0.5f);
      if (GlobalData.CapsuleLevelModel.JumpData != null &&
          GlobalData.PropModel.GetUserProp(GlobalData.CapsuleLevelModel.JumpData.RequireId).Num >=
          GlobalData.CapsuleLevelModel.JumpData.RequireNum)
      {
          FlowText.ShowMessage(I18NManager.Get("MainLine_Hint1"));
      }
      
      for (int i = 0; i < result.Count; i++)
      {
          GameResultPB pb = result[i];
          GlobalData.PlayerModel.AddExp(pb.Exp);
      }

      View.RefreshNum(GetData<ActivityCapsuleTemplateModel>());
    }

    private void OnSweepEnd()
    {
        if (_lastPlayLevel != null)
        {
            OpenBattleWindow(_lastPlayLevel);
        }
    }


    private void OpenBattleWindow(CapsuleLevelVo vo)
    {
       
        var level = vo;
           
        if (level.IsPass &&  !GuideManager.IsPass1_9())
        {
            FlowText.ShowMessage(I18NManager.Get("Guide_Battle6", "1-9"));
            return;  
        }

        var capsuleBattleIntroductionPopup = PopupManager.ShowWindow<CapsuleBattleIntroductionPopup>("ActivityCapsuleBattle/Prefabs/CapsuleBattleIntroductionPopup");
        capsuleBattleIntroductionPopup.Init(level,GetData<ActivityCapsuleTemplateModel>().CurActivity,EnterCapsuleBattle);
        capsuleBattleIntroductionPopup.MaskColor = new Color(0,0,0,0.5f);
        ClientData.CustomerSelectedCapsuleLevel = level;
    }
    
    
    ///打开活动任务窗口
	private void OpenStoryWindow()
    {
        _storyWindow = PopupManager.ShowWindow<ActivityCapsuleTemplateStoryWindow>("ActivityCapsuleTemplate/Prefabs/ActivityCapsuleTemplateStoryWindow");
        _storyWindow.SetData(GetData<ActivityCapsuleTemplateModel>());
    }

    public void Refresh()
    {
        View.SetData(GetData<ActivityCapsuleTemplateModel>());
    }
    

    #region 网络协议
    /// <summary>
	/// 规则
	/// </summary>
	private void SendRuleReq()
    {
        NetWorkManager.Instance.Send<ActivityCapsuleRules>(CMD.ACTIVITYC_CAPSULE_RULE, null, GetRuleRep);
    }

    private void GetRuleRep(ActivityCapsuleRules res)
    {      
        GetData<ActivityCapsuleTemplateModel>().InitRule(res);
      
       
        SendUserInfoReq();
       

    }

    //设置关卡信息
    private void InitLevelInfo()
    {
        var id = GetData<ActivityCapsuleTemplateModel>().CurActivityId;
        GlobalData.CapsuleLevelModel.SetCapsuleBattleData(GetData<ActivityCapsuleTemplateModel>().CurActivity);
      
        ActivityListReq req = new ActivityListReq
        {
            ActivityId = id,
        };
        byte[] data = NetWorkManager.GetByteData(req);
        NetWorkManager.Instance.Send<ActivityListRes>(CMD.ACTIVITY_ACTIVITYLISTS2, data, res =>
        {
            GlobalData.ActivityModel.AddDataToActivityListDic(GetData<ActivityCapsuleTemplateModel>().CurActivity.ActivityType, res);
            GlobalData.CapsuleLevelModel.SetMyCapsuleBattleLevelData();
            View.CreateLevelEntranceItem(GlobalData.CapsuleLevelModel.GetLevelRule(GetData<ActivityCapsuleTemplateModel>().CurActivityId));
        });
    }
    
    
    //用户信息
    private void SendUserInfoReq()
    {
        GetUserActivityCapsuleReq req = new GetUserActivityCapsuleReq
        {
            ActivityId = GetData<ActivityCapsuleTemplateModel>().CurActivityId
        };
        byte[] data = NetWorkManager.GetByteData(req);
        NetWorkManager.Instance.Send<GetUserActivityCapsuleRes>(CMD.ACTIVITYC_CAPSULE_INFO, data, GetUserInfoRep);
    }

    private void GetUserInfoRep(GetUserActivityCapsuleRes res)
    {     
        if (res.UserActivityCapsuleInfoPB != null)
        {
            GetData<ActivityCapsuleTemplateModel>().SetReadStoryIds(res.UserActivityCapsuleInfoPB.PlotIds.ToList<string>());
            GetData<ActivityCapsuleTemplateModel>().SetGainCapsuleItemIds(res.UserActivityCapsuleInfoPB.DrawAwardProgress.ToList<int>());
        }
        else {
            GetData<ActivityCapsuleTemplateModel>().SetReadStoryIds(null);
            GetData<ActivityCapsuleTemplateModel>().SetGainCapsuleItemIds(null);
        }
        View.SetData(GetData<ActivityCapsuleTemplateModel>());
        InitLevelInfo();
    }

    
    private void CapsuleBattleOver()
    {      
        View.CreateLevelEntranceItem(GlobalData.CapsuleLevelModel.GetLevelRule(GetData<ActivityCapsuleTemplateModel>().CurActivityId));
        View.RefreshNum(GetData<ActivityCapsuleTemplateModel>());
    }
    //看剧情
    private void SendWatchStoryReq(string id, System.Action finishCallback = null)
    {
        WatchOverDramaReq req = new WatchOverDramaReq
        {
            ActivityId = GetData<ActivityCapsuleTemplateModel>().CurActivityId,
            PlotId = id
        };
        byte[] data = NetWorkManager.GetByteData(req);
        NetWorkManager.Instance.Send<WatchOverDramaRes>(CMD.ACTIVITYC_CAPSULE_WATCH_DRAMA, data, (res) =>
        {
            if (finishCallback != null)
                finishCallback();
        });
    }

    //看完剧情
    private void SendWatchOverStoryReq(string id, System.Action finishCallback = null)
    {
        Debug.Log("SendWatchOverStoryReq:"+id);
        WatchOverDramaReq req = new WatchOverDramaReq
        {
            ActivityId = GetData<ActivityCapsuleTemplateModel>().CurActivityId,
            PlotId = id
        };
        byte[] data = NetWorkManager.GetByteData(req);
        NetWorkManager.Instance.Send<WatchOverDramaRes>(CMD.ACTIVITYC_CAPSULE_WATCH_OVER_DRAMA, data, (res)=>
        {
            SendUserInfoReq();
            if (finishCallback != null)
                finishCallback();
        });
    }


    #endregion


    public override void Destroy()
    {
        EventDispatcher.RemoveEvent(EventConst.ShowLastCapsuleBattleWindow);
        EventDispatcher.RemoveEvent(EventConst.ActivityCapsuleTemplateWatchStory);
        EventDispatcher.RemoveEvent(EventConst.ActivityCapsuleTemplateWatchOverStory);
       // EventDispatcher.RemoveEvent(EventConst.EnterCapsuleBattle);
        EventDispatcher.RemoveEvent(EventConst.CapsuleBattleOver);
        EventDispatcher.RemoveEvent(EventConst.OnClickCapsuleBattleEntrance);
        EventDispatcher.RemoveEvent(EventConst.DailyRefresh6);
    }
}
