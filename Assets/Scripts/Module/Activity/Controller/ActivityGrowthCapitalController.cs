using System.Collections;
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

public class ActivityGrowthCapitalController : Controller
{

    public ActivityGrowthCapitalView View;
    private GrowthCapitalModel _growthCapitalModel;


    public override void Init()
    {
        EventDispatcher.AddEventListener<int>(EventConst.GetGrowthFundAward,SendGrowthFundReq);
        //EventDispatcher.AddEventListener<RepeatedField<UserBuyRmbMallPB>>(EventConst.GetPayInfoSuccess,RefreshView);
    }

    private void SendGrowthFundReq(int id)
    {
        Debug.LogError(id);
        LoadingOverlay.Instance.Show();
        var buffer = NetWorkManager.GetByteData(new GrowthFundAwardReq {Id = id});
        NetWorkManager.Instance.Send<GrowthFundAwardRes>(CMD.ACTIVITY_GETGROWTHFUND,buffer, GetGrowthFund);
    }

    private void GetGrowthFund(GrowthFundAwardRes res)
    {
        FlowText.ShowMessage(I18NManager.Get("Task_ReceiveRewardSuccess"));
        RewardUtil.AddReward(res.Awards);
        //GlobalData.ActivityModel.UpdateGrowthData(res.UserGrowthFund);
        _growthCapitalModel.UpdateGrowthData(res.UserGrowthFund);
        _growthCapitalModel.GrowthFunVos.Sort();
        View.SetData(_growthCapitalModel.GrowthFunVos);
        LoadingOverlay.Instance.Hide();
          SendMessage(new Message(MessageConst.CMD_ACTIVITY_REFRESH_ACTIVITYTOGGLE_REDDOT));
    }

    public override void Start()
    {
        _growthCapitalModel=new GrowthCapitalModel(); 
        _growthCapitalModel.GrowthFunVos.Sort();
        View.SetData(_growthCapitalModel.GrowthFunVos);
    }

    private void RefreshView(RepeatedField<UserBuyRmbMallPB> userBuyRmbMallPbs)
    {
        SendMessage(new Message(MessageConst.CMD_ACTIVITY_REFRESH_ACTIVITYTOGGLE_REDDOT));//购买完后也要刷下红点，如果玩家是5级在这个界面购买是要刷新下红点
        View.SetData(_growthCapitalModel.GrowthFunVos); 
    }

    public override void OnMessage(Message message)
    {
        string name = message.Name;
        object[] body = message.Params;
        switch (name)
        {
           case MessageConst.CMD_ACTIVITY_REFRESHACTIVITYDA:
               var userBuyRmbMallPbs = (RepeatedField<UserBuyRmbMallPB>) body[0];
               if (userBuyRmbMallPbs!=null)
               {
                   RefreshView(userBuyRmbMallPbs);
               }               
               break;
        }
    }
    
    

    public override void Destroy()
    {
        base.Destroy();
        EventDispatcher.RemoveEventListener<int>(EventConst.GetGrowthFundAward,SendGrowthFundReq);
       // EventDispatcher.RemoveEventListener<RepeatedField<UserBuyRmbMallPB>>(EventConst.GetPayInfoSuccess,RefreshView);
    }
}
