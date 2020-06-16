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
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utils;

public class ActivityDrawCardController : Controller
{
    public ActivityDrawCardView View;
    private ActivityDrawCardModel _model;

    private ActivityVo _curActivity;
    public override void Init()
    {
        base.Init();

        _curActivity = GlobalData.ActivityModel.GetActivityVo(ActivityType.ActivityTenDrawCard);
        if (_curActivity==null)
        {
             return;
        }
        
        _model = new ActivityDrawCardModel();
        _model.activitId = _curActivity.ActivityId;      
        GetData();
        
//        if (GlobalData.ActivityModel.GetActivityVo(ActivityType.ActivityTenDrawCard)
//            == null)
//        {
//            _model.activitId = 0;
//        }
//        _model = new ActivityDrawCardModel();
//        _model.activitId = GlobalData.ActivityModel.GetActivityVo(ActivityType.ActivityTenDrawCard).ActivityId;
//        _model.Init(GlobalData.ActivityModel.BaseTemplateActivityRule, GlobalData.ActivityModel.ActivityListRes);


  
        EventDispatcher.AddEventListener<int,int>(EventConst.ActivityGetDrawCardAward, OnGetMissionAward);

    }


    private void GetData()
    {
        ActivityListReq req = new ActivityListReq
        {
            ActivityId = _model.activitId,
        };
        byte[] data = NetWorkManager.GetByteData(req);

        NetWorkManager.Instance.Send<ActivityListRes>(CMD.ACTIVITY_ACTIVITYLISTS2, data, res =>
        {
            GlobalData.ActivityModel.AddDataToActivityListDic(_curActivity.ActivityType, res);
            var activityListRes = GlobalData.ActivityModel.GetActivityTemplateListRes(_curActivity.ActivityType);
            _model.Init(GlobalData.ActivityModel.BaseTemplateActivityRule,activityListRes);
            View.SetData(_model.GetActivityVo(), _model.LeftTime(), _model.EndTime(), _model.CurDrawTime);
        });
    }
    
    
    public override void OnMessage(Message message)
    {
        base.OnMessage(message);
        string name = message.Name;
        object[] body = message.Params;
        switch (name)
        {
            case MessageConst.CMD_ACTIVITY_ONSHOW:
                ActivityUserDataRefresh();
          
                Debug.LogError("CMD_ACTIVITY_ONSHOW ..............");
                break;
        }
    }

    void ActivityUserDataRefresh()
    {
//        NetWorkManager.Instance.Send<ActivityRes>(CMD.ACTIVITY_ACTIVITYLIST, null, res =>
//        {
//            GlobalData.ActivityModel.GetAllActivityRes(res);
//            _model.Init(GlobalData.ActivityModel.BaseTemplateActivityRule, GlobalData.ActivityModel.ActivityListRes);
//            View.SetData(_model.GetActivityVo(), _model.LeftTime(), _model.EndTime(), _model.CurDrawTime);
//            SendMessage(new Message(MessageConst.CMD_ACTIVITY_REFRESH_ACTIVITYTOGGLE_REDDOT));
//        });

        ActivityListReq req = new ActivityListReq
        {
            ActivityId = _model.activitId,
        };
        byte[] data = NetWorkManager.GetByteData(req);

        NetWorkManager.Instance.Send<ActivityListRes>(CMD.ACTIVITY_ACTIVITYLISTS2, data, res =>
        {
            GlobalData.ActivityModel.AddDataToActivityListDic(_curActivity.ActivityType, res);
            var activityListRes = GlobalData.ActivityModel.GetActivityTemplateListRes(_curActivity.ActivityType);
            _model.Init(GlobalData.ActivityModel.BaseTemplateActivityRule, activityListRes);

            View.SetData(_model.GetActivityVo(), _model.LeftTime(), _model.EndTime(), _model.CurDrawTime);
            SendMessage(new Message(MessageConst.CMD_ACTIVITY_REFRESH_ACTIVITYTOGGLE_REDDOT));
        });
    }
    public override void Start()
    {
        base.Start();
      
    }

    void OnGetMissionAward(int activityId,int missionId)
    {
        LoadingOverlay.Instance.Show();
        Debug.LogError("OnGetMissionAward activityId " + activityId+ "  missionId   "+ missionId);
        var buffer = NetWorkManager.GetByteData(new GainActivityMissionAwardsReq
        {
            ActivityId = activityId,
            ActivityMissionId = missionId
        });
        NetWorkManager.Instance.Send<GainActivityMissionAwardsRes>(CMD.ACTIVITY_GAINACTIVITYMISSIONAWARDS, buffer, res =>
        {
            LoadingOverlay.Instance.Hide();
            GlobalData.ActivityModel.UpDataActivityMission(_curActivity.ActivityType, res.UserActivityMission);
            var activityListRes = GlobalData.ActivityModel.GetActivityTemplateListRes(_curActivity.ActivityType);
            _model.Init(GlobalData.ActivityModel.BaseTemplateActivityRule,activityListRes);
            View.SetData(_model.GetActivityVo(), _model.LeftTime(),_model.EndTime(), _model.CurDrawTime);
//            GlobalData.ActivityModel.UpdateActivityMission(res);
//            _model.Init(GlobalData.ActivityModel.BaseTemplateActivityRule, GlobalData.ActivityModel.ActivityListRes);
//            View.SetData(_model.GetActivityVo(), _model.LeftTime(),_model.EndTime(), _model.CurDrawTime);
            GlobalData.ActivityModel.RemoveActivityMissionRedDot(_curActivity.ActivityId);
            SendMessage(new Message(MessageConst.CMD_ACTIVITY_REFRESH_ACTIVITYTOGGLE_REDDOT));
            RewardUtil.AddReward(res.Awards);     //增加道具
            var isCard = false;
            foreach (var t in res.Awards)
            {
                if (t.Resource == ResourcePB.Card)
                {
                    isCard = true;
                    break;
                }
            }

            if (isCard)
            {
                List<AwardPB> award = new List<AwardPB>();
                foreach (var t in res.Awards)
                {
                    if (t.Resource == ResourcePB.Card)
                    {
                        award.Add(t);
                        break;
                    }
                }
                Action finish = () =>
                {
                    SendMessage(new Message(MessageConst.CMD_ACTIVITYTEMPLATE1_ACTIVITY_SHOW_TOPBAR_AND_BACKBTN, Message.MessageReciverType.UnvarnishedTransmission, true));
                };
                ModuleManager.Instance.EnterModule(ModuleConfig.MODULE_DRAWCARD,
                    false, false, "DrawCard_CardShow", award, finish, false);
                ClientTimer.Instance.DelayCall(() =>
                {
                    SendMessage(new Message(MessageConst.CMD_ACTIVITYTEMPLATE1_ACTIVITY_SHOW_TOPBAR_AND_BACKBTN, Message.MessageReciverType.UnvarnishedTransmission, false));
                }, 0.1f);
       
            }
            else
            {
                var window = PopupManager.ShowWindow<CommonAwardWindow>("GameMain/Prefabs/AwardWindow/CommonAwardWindow");
                window.SetData(res.Awards.ToList(), false, ModuleConfig.MODULE_ACTIVITYTEMPLATE);
            }
        },(error)=>{
            LoadingOverlay.Instance.Hide();
        });
    }
   
    public override void Destroy()
    {
        EventDispatcher.RemoveEvent(EventConst.ActivityGetDrawCardAward);


        View.DestroyCountDown();
        base.Destroy();
    }
}
