using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Core.Events;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module.NetWork;
using Com.Proto;
using Common;
using Componets;
using DataModel;
using game.main;
using Google.Protobuf.Collections;
using UnityEngine;
using Utils;

public class ActivityController : Controller
{
    public ActivityView  ActivityView; 
    
   
    public override void Init()
    {                  
        EventDispatcher.AddEventListener<RepeatedField<UserBuyRmbMallPB>>(EventConst.GetPayInfoSuccess,UpdateActivityData);
        EventDispatcher.AddEventListener(EventConst.DailyRefresh6,SixPointRefreshActivity);
 
    }


    private void SixPointRefreshActivity()
    {
        NetWorkManager.Instance.Send<ActivityRes>(CMD.ACTIVITY_ACTIVITYLIST, null, res =>
        {
            GlobalData.ActivityModel.GetAllActivityRes(res);
            
            //发生消息
            SendMessage(new Message(MessageConst.CMD_ACTIVITY_SIXREFRESHACTIVITY));
        });
    }

    private void UpdateActivityData(RepeatedField<UserBuyRmbMallPB> res)
    {            
        LoadingOverlay.Instance.Show();
        NetWorkManager.Instance.Send<ActivityRes>(CMD.ACTIVITY_ACTIVITYLIST,null, actRes =>
        {
            GlobalData.ActivityModel.GetAllActivityRes(actRes);
            LoadingOverlay.Instance.Hide();
            if (res.Count>0)
            {
                SendMessage(new Message(MessageConst.CMD_ACTIVITY_REFRESHACTIVITYDA,Message.MessageReciverType.CONTROLLER,res)); 
            }
        }); 
        
    }

    public override void Start()
    {
        ClientData.LoadItemDescData(null);
        ClientData.LoadSpecialItemDescData(null);  
        GetActivityData();
        
//        if (GlobalData.ActivityModel.ActivityTypeId!=-1)    
//        ActivityView.CreateActivityToggleAndActivityContent(GlobalData.ActivityModel.GetActivityList(),GlobalData.ActivityModel.ActivityTypeId);       
    }

    
    
    
    private void GetActivityData()
    {
        if (GuideManager.CurStage()== GuideStage.SevenDaySigninActivityStage)
        {
            if (GlobalData.ActivityModel.ActivitySoleId!="-1")    
                ActivityView.CreateActivityToggleAndActivityContent(GlobalData.ActivityModel.GetActivityVoList(),GlobalData.ActivityModel.ActivitySoleId);    
        }
        else
        {
            LoadingOverlay.Instance.Show();
            NetWorkManager.Instance.Send<ActivityRes>(CMD.ACTIVITY_ACTIVITYLIST,null, res =>
            {
                GlobalData.ActivityModel.GetAllActivityRes(res);
                LoadingOverlay.Instance.Hide();
                if (GlobalData.ActivityModel.ActivitySoleId != "-1")
                {
                    ActivityView.CreateActivityToggleAndActivityContent(GlobalData.ActivityModel.GetActivityVoList(),GlobalData.ActivityModel.ActivitySoleId);      
                }  

            });  
        }
      
    }

   

    public override void OnMessage(Message message)
    {
        string name = message.Name;
        object[] body = message.Params;
        switch (name)
        {
            case MessageConst.CMD_ACTIVITY_REFRESH_ACTIVITYTOGGLE_REDDOT:
                ActivityView.RefreshActivityToggleRedDot(GlobalData.ActivityModel.GetActivityVoList());
                break;
   
        }
    }

    public override void Destroy()
    {
        base.Destroy();
        ClientData.Clear();
        EventDispatcher.RemoveEventListener<RepeatedField<UserBuyRmbMallPB>>(EventConst.GetPayInfoSuccess,UpdateActivityData);
        EventDispatcher.RemoveEventListener(EventConst.DailyRefresh6,SixPointRefreshActivity);
    }

 
   
}
