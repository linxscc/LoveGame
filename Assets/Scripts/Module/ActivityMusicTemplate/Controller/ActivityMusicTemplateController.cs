using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Core.Events;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module;
using Assets.Scripts.Module.NetWork;
using Com.Proto;
using Common;
using Componets;
using DataModel;
using game.main;
using Google.Protobuf.Collections;
using UnityEngine;
using Utils;

public class ActivityMusicTemplateController : Controller
{
    public ActivityMusicTemplateView View;
    private ActivityMusicTaskWindow _taskWindow;
    private ActivityMusicStoryWindow _storyWindow;
    
    private ActivityMusicTemplateModel _model;  
    private ActivityStoryModel _storyModel;
    private ActivityExchangeShopModel _exchangeShopModel;
    private ActivityMissionModel _activityMissionModel;  
    
    private CapsuleLevelVo _lastPlayLevel;
    private ActivityVo _curActivity;

    public override void Init()
    {
        ClientData.LoadItemDescData(null);
        ClientData.LoadSpecialItemDescData(null);
        EventDispatcher.AddEventListener<ActivityMissionVo>(EventConst.GetActivityMusicTaskAward, GetActivityMusicTaskAward);          
        EventDispatcher.AddEventListener<string>(EventConst.ActivityMusicTaskGoto, ActivityMusicTaskGoto);
        EventDispatcher.AddEventListener(EventConst.DailyRefresh6, DailyRefresh6);
        EventDispatcher.AddEventListener<CapsuleLevelVo>(EventConst.OnClickMusicCapsuleBattleEntrance,  OnClickMusicCapsuleBattleEntrance);                
        EventDispatcher.AddEventListener<string, System.Action>(EventConst.WatchActivityStoryOver, WatchActivityStoryOver); 
    }

    public override void Destroy()
    {
        EventDispatcher.RemoveEvent(EventConst.ShowLastCapsuleBattleWindow);
        EventDispatcher.RemoveEvent(EventConst.GetActivityMusicTaskAward);
        EventDispatcher.RemoveEvent(EventConst.ActivityMusicTaskGoto);
        EventDispatcher.RemoveEvent(EventConst.DailyRefresh6);
        EventDispatcher.RemoveEvent(EventConst.OnClickMusicCapsuleBattleEntrance);       
        EventDispatcher.RemoveEvent(EventConst.WatchActivityStoryOver);
        View.DestroyCountDown();
    }
    
    public override void Start()
    {
        InitCurActivityData();
        InitGetData();
    }

    private void InitCurActivityData()
    {
        _curActivity = GlobalData.ActivityModel.GetCurActivityTemplate(ActivityTypePB.ActivityMusicGame)[0];
        Debug.LogError(_curActivity.Name+_curActivity.ActivityId);
    }

    private void InitGetData()
    {
        ActivityListReq req = new ActivityListReq
        {
            ActivityId = _curActivity.ActivityId,
        };

        byte[] data = NetWorkManager.GetByteData(req);

        NetWorkManager.Instance.Send<ActivityListRes>(CMD.ACTIVITY_ACTIVITYLISTS2, data, res =>
        {        
            GlobalData.ActivityModel.AddDataToActivityListDic(_curActivity.ActivityType, res);
            _model = new ActivityMusicTemplateModel(_curActivity);
            View.SetEntranceShow(_model.IsOpenMusicTemplateStoryEntrance, _model.IsOpenMusicExchangeShopEntrance, _model.IsOpenMusicTemplateTaskEntrance);
            GlobalData.CapsuleLevelModel.SetCapsuleBattleData(_curActivity);
            GlobalData.CapsuleLevelModel.SetMyCapsuleBattleLevelData();
            var list = GlobalData.CapsuleLevelModel.GetLevelRule(_curActivity.ActivityId);
            
            View.SetData(_curActivity, list);
            View.FirstShowRuleWindow(_model.GetPlayRule());
            InitModel();
            View.SetRedDot(_storyModel, _activityMissionModel);

            NetWorkManager.Instance.Send<Rules>(CMD.MUSICGAMEC_RULES, null, OnGetMusicGameRule, null, true, GlobalData.VersionData.VersionDic[CMD.MUSICGAMEC_RULES]);

        });
    }

    private void OnGetMusicGameRule(Rules res)
    {
        NetWorkManager.Instance.Send<ActivityMusicInfoRes>(CMD.ACTIVITY_ACTIVITYMUSICINFO, null, res2 =>
        {
            _model.InitActivityMusicInfo(GlobalData.ActivityModel.GetActivityMusicPoolRules(_curActivity.ActivityId), res2, res);
            View.SetMusicData(_model.ActivityMusicVos);
        });
    }

    private void OnShowGetData(Action callBack)
    {
        ActivityListReq req = new ActivityListReq
        {
            ActivityId = _curActivity.ActivityId,
        };
        byte[] data = NetWorkManager.GetByteData(req);
        
             
        NetWorkManager.Instance.Send<ActivityListRes>(CMD.ACTIVITY_ACTIVITYLISTS2, data, res =>
        {
            GlobalData.ActivityModel.AddDataToActivityListDic(_curActivity.ActivityType,res);
            callBack?.Invoke();   
        });
    }

    private void InitModel()
    {
        if ( _model.IsOpenMusicExchangeShopEntrance)
        {
            _exchangeShopModel=new ActivityExchangeShopModel(_curActivity);
        }

        if (_model.IsOpenMusicTemplateStoryEntrance)
        {
            _storyModel= new ActivityStoryModel(_curActivity);
        }

        if (_model.IsOpenMusicTemplateTaskEntrance)
        {
            _activityMissionModel =new ActivityMissionModel(_curActivity);
        } 
    }

    /// <summary>
    /// OnShow刷新音游关卡入口
    /// </summary>
    private void OnShowRefreshMusicLevelEntrance()
    {
        NetWorkManager.Instance.Send<ActivityMusicInfoRes>(CMD.ACTIVITY_ACTIVITYMUSICINFO, null, res2 =>
        {
            _model.InitActivityMusicInfo(GlobalData.ActivityModel.GetActivityMusicPoolRules(_curActivity.ActivityId), res2);
            View.RefreshMusicLevelItem(_model.ActivityMusicVos);
        }); 
    }
  

    public override void OnMessage(Message message)
    {
        string name = message.Name;
        object[] body = message.Params;
        switch (name)
        {
            case MessageConst.CMD_ACTIVITY_MUSIC_TEMPLATE_ON_SHOW_REFRESH: //OnShow在刷新               
                View.RefreshNum(); 
                CapsuleBattleOver();
                OnShowRefreshMusicLevelEntrance();
                OnShowGetData(() =>
                {
                    if (_model.IsOpenMusicTemplateTaskEntrance)
                    {
                        _activityMissionModel=new ActivityMissionModel(_curActivity);
                    }      
                    View.SetRedDot(_storyModel,_activityMissionModel);  
                });                          
                break;
            case MessageConst.CMD_OPEN_ACTIVITYMUSIC_TASK_WINDOW:
                OpenTaskWindow();
                break;
            case MessageConst.CMD_OPEN_ACTIVITYMUSIC_STORY_WINDOW:
                OpenStoryWindow();
                break;
            case MessageConst.CMD_ACTIVITY_MUSIC_OVER:
                ModuleManager.Instance.GoBack();
                break;
            case MessageConst.CMD_OPEN_ACTIVITYMUSIC_EXCHANGESHOP:               
                SendMessage(new Message(MessageConst.CMD_SHOW_ACTIVITYMUSIC_EXCHANGESHOP,_exchangeShopModel));
                break;
        }
    }

    #region 6点刷新相关

    /// <summary>
    /// 每日6点刷新
    /// </summary>
    private void DailyRefresh6()
    {       
        OnShowGetData(() =>
        {
            SixOClockRefreshTaskWindow();
            SixOClockRefreshStoryWindow();
            View.SetRedDot(_storyModel,_activityMissionModel);
        });                 
    }

    /// <summary>
    /// 6点刷新任务窗口
    /// </summary>
    private void SixOClockRefreshTaskWindow()
    {
        if (_taskWindow!=null&&_model.IsOpenMusicTemplateTaskEntrance)
        {                     
            _activityMissionModel =new ActivityMissionModel(_curActivity);
            _taskWindow.SetData(_activityMissionModel.MusicUserMissionInfo);                        
        } 
    }

    /// <summary>
    /// 6点刷新剧情窗口
    /// </summary>
    private void SixOClockRefreshStoryWindow()
    {
        if (_storyWindow != null&&_model.IsOpenMusicTemplateStoryEntrance)
        {
            _storyModel =new ActivityStoryModel(_curActivity);
            _storyWindow.SetData(_storyModel.GetUserStoryInfo);         
        }
    }

    #endregion

    #region 音游任务相关

    /// <summary>
    ///  打开音游任务窗口
    /// </summary>
    private void OpenTaskWindow()
    {
        _taskWindow =PopupManager.ShowWindow<ActivityMusicTaskWindow>("ActivityMusicTemplate/Prefabs/ActivityMusicTaskWindow");           
        _taskWindow.SetData(_activityMissionModel.MusicUserMissionInfo);
    }

    /// <summary>
    /// 音游任务前往
    /// </summary>
    private void ActivityMusicTaskGoto(string jumpTo)
    {       
        JumpToModule.JumpTo(jumpTo, () =>
        {
           View.JumpToMusicLevel(jumpTo);
        });
    }

    /// <summary>
    /// 领取音游活动任务奖励
    /// </summary>
    private void GetActivityMusicTaskAward(ActivityMissionVo vo)
    {
        GainActivityMissionAwardsReq req =new GainActivityMissionAwardsReq
        {
            ActivityId = vo.ActivityId,
            ActivityMissionId = vo.ActivityMissionId,
        };
        byte[] data = NetWorkManager.GetByteData(req);
        NetWorkManager.Instance.Send<GainActivityMissionAwardsRes>(CMD.ACTIVITYC_GET_AWARDS, data, (res)=>
        {        
            RewardUtil.AddReward(res.Awards);
            
         
            RewardVo rewardVo = null;
            foreach (var t in res.Awards)
            {      
                rewardVo=new RewardVo(t);                         
            }

            if (rewardVo != null) FlowText.ShowMessage(I18NManager.Get("Activity_Get", rewardVo.Name, rewardVo.Num));          
            _activityMissionModel.UpdateActivityMissionData(res.UserActivityMission);
            _taskWindow.SetData(_activityMissionModel.MusicUserMissionInfo);
            View.SetRedDot(_storyModel,_activityMissionModel);  
            View.RefreshNum(); 
        });
    }

    #endregion

    #region 音游剧情相关

    /// <summary>
    /// 打开剧情窗口
    /// </summary>
    private void OpenStoryWindow()
    {
        _storyWindow = PopupManager.ShowWindow<ActivityMusicStoryWindow>("ActivityMusicTemplate/Prefabs/ActivityMusicStoryWindow");          
        _storyWindow.SetData(_storyModel.GetUserStoryInfo);     
    }
    
    //看完剧情
    private void WatchActivityStoryOver(string id, System.Action finishCallback = null)
    {             
        WatchActivityPlotReq req = new WatchActivityPlotReq
        {
            ActivityId = _curActivity.ActivityId,
            PlotId = id
        };
        byte[] data = NetWorkManager.GetByteData(req);
        NetWorkManager.Instance.Send<WatchActivityPlotRes>(CMD.ACTIVITYC_WATCH_STORY, data, (res)=>
        {        
            _storyModel.UpdateUserData(res.UserActivityPlotInfo);
            _storyModel.SetCanEnterStory();
            View.SetRedDot(_storyModel,_activityMissionModel);
            finishCallback?.Invoke();
        });
    }


#endregion

    #region 音游主线战斗相关
    private void OnClickMusicCapsuleBattleEntrance(CapsuleLevelVo vo)
    {
        OpenBattleWindow(vo);
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
        capsuleBattleIntroductionPopup.Init(level,_curActivity,EnterCapsuleBattle);
        capsuleBattleIntroductionPopup.MaskColor = new Color(0,0,0,0.5f);
        ClientData.CustomerSelectedCapsuleLevel = level;
        
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
    
    //扫荡回包
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

        View.RefreshNum();
    }
    
    
    private void OnSweepEnd()
    {
        if (_lastPlayLevel != null)
        {
            OpenBattleWindow(_lastPlayLevel);
        }
    }

 

    private void CapsuleBattleOver()
    {       
        View.RefreshLevelItem(GlobalData.CapsuleLevelModel.GetLevelRule(_curActivity.ActivityId));
    
    }
    #endregion
    

}
