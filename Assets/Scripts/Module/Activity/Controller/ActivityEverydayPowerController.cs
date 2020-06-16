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
using UnityEngine;
using Utils;

public class ActivityEverydayPowerController : Controller
{
    public ActivityEverydayPowerSigninView View;

    private NormalAwardWindow _normalAwardWindow;

    private EverydayPowerModel everydayPowerModel;

    public override void Init()
    {
        EventDispatcher.AddEventListener<EveryDayPowerVO>(EventConst.GetEverydayPowerAward, GetEverydayPowerAward);
        EventDispatcher.AddEventListener<EveryDayPowerVO>(EventConst.OpenEverydayPowerAwardRetroactiveWindow, OpenEverydayPowerAwardRetroactiveWindow);
        EventDispatcher.AddEventListener<EveryDayPowerVO>(EventConst.SendRetroactiveEverydayPowerAwardReq, SendRetroactiveEverydayPowerAwardReq);
       
    }


    /// <summary>
    /// 每日体力6点刷新
    /// </summary>
    private void EverydayPowerSixPointRefresh()
    {
       
        if (View.gameObject!=null)
        {
            Debug.LogError("刷新每日体力");
            everydayPowerModel.Init();
            View.RefreshData(everydayPowerModel.GetEveryDayPowerList());  //生成  
        }        
    }

    public override void Start()
    {
        everydayPowerModel = new EverydayPowerModel();
        View.CreateEverydayPowerData(everydayPowerModel.GetEveryDayPowerList());  //生成
        CheckRefresh();

    }


    private void CheckRefresh()
    {
        ClientTimer.Instance.AddCountDown("EverydayPowerUpdateRefresh", Int64.MaxValue, 600f, EverydayPowerUpdateRefresh,
            null); 
    }

    private void EverydayPowerUpdateRefresh(int obj)
    {
        everydayPowerModel.Init();
        View.RefreshData(everydayPowerModel.GetEveryDayPowerList());  //生成
    }

    public override void OnMessage(Message message)
    {
        string name = message.Name;
        object[] body = message.Params;
        switch (name)
        {
              case MessageConst.CMD_ACTIVITY_SIXREFRESHACTIVITY:
                  EverydayPowerSixPointRefresh();
                  break;
        }
    }

    public override void Destroy()
    {
        ClientTimer.Instance.RemoveCountDown("EverydayPowerUpdateRefresh");
        EventDispatcher.RemoveEvent(EventConst.GetEverydayPowerAward);
        EventDispatcher.RemoveEvent(EventConst.OpenEverydayPowerAwardRetroactiveWindow);
        EventDispatcher.RemoveEvent(EventConst.SendRetroactiveEverydayPowerAwardReq);
      
    }



    //打开每日体力窗口
    private void GetEverydayPowerAward(EveryDayPowerVO vO)
    {           
        
        OpenEverydayPowerAwarWindow(vO);
       
    }

    //发送每天体力签到请求
    private void SendPowerGetRewardReq(EveryDayPowerVO vO)
    {
        
       
        PowerGetRewardReq req = new PowerGetRewardReq
        {
            Id = vO.Id,
        };
        byte[] data = NetWorkManager.GetByteData(req);
        NetWorkManager.Instance.Send<PowerGetRewardRes>(CMD.ACTIVITY_GET_POWER, data, res =>
        {
            for (int i = 0; i < res.Awards.Count; i++) { RewardUtil.AddReward(res.Awards[i]); }  //增加奖励数据
            GlobalData.ActivityModel.UpdataUserPowerGottenIds(res.GottenIds);                    //更新ActivityModel用户每日体力签到次数集合数据
            for (int i = 0; i < res.GottenIds.Count; i++) { everydayPowerModel.UpdataEveryDayPowerList(res.GottenIds[i]); }  //更新用户每日体力集合
            View.RefreshData(everydayPowerModel.GetEveryDayPowerList());             //重新生成
           
            SendMessage(new Message(MessageConst.CMD_ACTIVITY_REFRESH_ACTIVITYTOGGLE_REDDOT));

            AudioManager.Instance.PlayEffect("buypower"); 
        },errCallback =>{
            
            everydayPowerModel.Init();
            View.RefreshData(everydayPowerModel.GetEveryDayPowerList()); 
        });
       

    }

    private void OpenEverydayPowerAwarWindow(EveryDayPowerVO vO)
    {
        _normalAwardWindow = null;
        if (_normalAwardWindow == null)
        {
            _normalAwardWindow = PopupManager.ShowWindow<NormalAwardWindow>("Activity/Prefabs/NormalAwardWindow");
            _normalAwardWindow.SetData(vO);
            _normalAwardWindow.WindowActionCallback = evt =>
            {
                 Debug.LogError("发送每日体力领取请求");               
                SendPowerGetRewardReq(vO);                 
            };
            
        }

       
                  
    }

    //打开每日体力补签窗口
    private void OpenEverydayPowerAwardRetroactiveWindow(EveryDayPowerVO vO)
    {
        _normalAwardWindow = null;
        if (_normalAwardWindow == null)
        {
            _normalAwardWindow = PopupManager.ShowWindow<NormalAwardWindow>("Activity/Prefabs/NormalAwardWindow");
            _normalAwardWindow.SetData(vO, true);
        }
    }



    //发送每日体力补签请求
    private void SendRetroactiveEverydayPowerAwardReq(EveryDayPowerVO vO)
    {
        PowerBuyGetReq req = new PowerBuyGetReq
        {
            Id = vO.Id,
        };
        byte[] data = NetWorkManager.GetByteData(req);
        NetWorkManager.Instance.Send<PowerBuyGetRes>(CMD.ACTIVITY_BUY_GET_POWER, data, res =>
        {

            for (int i = 0; i < res.Awards.Count; i++)
            {
                RewardUtil.AddReward(res.Awards[i]);
                FlowText.ShowMessage(I18NManager.Get("Activity_RetroactiveSucceed"));
            }  //增加奖励数据         
            GlobalData.ActivityModel.UpdataUserPowerGottenIds(res.GottenIds);                    //更新ActivityModel用户每日体力签到次数集合数据
            for (int i = 0; i < res.GottenIds.Count; i++) { everydayPowerModel.UpdataEveryDayPowerList(res.GottenIds[i]); }  //更新用户每日体力集合
            View.RefreshData(everydayPowerModel.GetEveryDayPowerList());             //重新生成
            GlobalData.PlayerModel.UpdateUserMoney(res.UserMoney);                               //更新用户的钱
          
            SendMessage(new Message(MessageConst.CMD_ACTIVITY_REFRESH_ACTIVITYTOGGLE_REDDOT));

        });

        AudioManager.Instance.PlayEffect("buypower"); 


    }

}
