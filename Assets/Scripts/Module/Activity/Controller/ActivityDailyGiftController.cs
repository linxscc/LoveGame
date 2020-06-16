using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Core.Events;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module.Framework.Utils;
using Assets.Scripts.Module.NetWork;
using Com.Proto;
using Common;
using Componets;
using DataModel;
using game.main;
using Google.Protobuf.Collections;
using UnityEngine;
using Utils;

public class ActivityDailyGiftController : Controller
{
    public ActivityDailyGiftView View;
    private ShopModel _shopModel;
    private AwardWindow _awardWindow;
    private List<int> _tempmallidlist;
    private float _lastClickTime=0;
//    private DateTime refreshTime;
//    private long _refreshTime;
//    private TimerHandler _handler;
    private int _freeGift=0;
    
    public override void Init()
    {
       // _pushDic=new Dictionary<string, string>();
        //EventDispatcher.AddEventListener<RepeatedField<UserBuyRmbMallPB>>(EventConst.GetPayInfoSuccess,UpdateDailyGift);
        EventDispatcher.AddEventListener<int>(EventConst.PayforDaily,OnPayforDaily);
//        var serverDateTime = DateUtil.GetDataTime(ClientTimer.Instance.GetCurrentTimeStamp());
//        refreshTime = new DateTime(serverDateTime.Year,serverDateTime.Month,serverDateTime.Day,6,0,0);
//        _refreshTime = DateUtil.GetTimeStamp(refreshTime);
        //CheckNeedToRefresh();
        _tempmallidlist=new List<int>();
        EventDispatcher.AddEventListener(EventConst.DailyRefresh6,UpdateDailyGiftTime);
    }

//    private void CheckNeedToRefresh()
//    {
//        _handler = ClientTimer.Instance.AddCountDown("UpdateDailyGiftTime", Int64.MaxValue, 5f, UpdateDailyGiftTime, null);   
//    }
//
//    private void UpdateDailyGiftTime(int obj)
//    {
//        //一个是加dateTime，一个是加时间戳，所以不冲突！！
//        var serverDateTime = DateUtil.GetDataTime(ClientTimer.Instance.GetCurrentTimeStamp());
//        if (DateTime.Compare(serverDateTime, refreshTime) > 0)
//        {
//            //已经过了整点
////            Debug.LogError(serverDateTime+" "+refreshTime);
//
//            refreshTime=new DateTime(serverDateTime.Year,serverDateTime.Month,serverDateTime.Day,6,0,0);//DateTime.Parse($"{DateTime.Today.Year}-{DateTime.Today.Month}-{DateTime.Today.Day+1} {refreshPoint}");
//            //_refreshTime = DateUtil.GetTimeStamp(refreshTime)+86400000;
//            refreshTime=refreshTime.AddDays(1);
//        }
//        else
//        {
//            if (ClientTimer.Instance.GetCurrentTimeStamp()>_refreshTime)
//            {
//                Debug.LogError(DateUtil.GetDataTime(ClientTimer.Instance.GetCurrentTimeStamp())+" "+refreshTime);
//                UpdateDailyGiftTime();
////                var serverDateTime = DateUtil.GetDataTime(ClientTimer.Instance.GetCurrentTimeStamp());
//                //refreshTime=new DateTime(serverDateTime.Year,serverDateTime.Month,serverDateTime.Day,6,0,0);//DateTime.Parse($"{serverDateTime.Year}-{serverDateTime.Month}-{serverDateTime.Day} {refreshPoint}");
//                _refreshTime = DateUtil.GetTimeStamp(new DateTime(serverDateTime.Year,serverDateTime.Month,serverDateTime.Day,6,0,0))+86400000;
////                refreshTime=refreshTime.AddDays(1);
//            }
//        }
//        
//    }

    private void OnPayforDaily(int mallid)
    {
        var freeGift = _shopModel.GetFreeGift;
        if (mallid==freeGift.MallId&&freeGift.BuyNum==0)
        {
            OnBuyGoldMallItemClick(_shopModel.GameMallDic[mallid], 1);
            return;
        }
        
        
        if (Time.realtimeSinceStartup- _lastClickTime <10f&&_tempmallidlist.Contains(mallid))
        {
            FlowText.ShowMessage(I18NManager.Get("Shop_DontRepeatBuy"));
            return;
        }

        if (!_tempmallidlist.Contains(mallid))
        {
            _tempmallidlist.Add(mallid);	
        }
        SdkHelper.PayAgent.Pay(GlobalData.PayModel.GetProduct(mallid));
        _lastClickTime = Time.realtimeSinceStartup;
    }

    private void UpdateDailyGift(RepeatedField<UserBuyRmbMallPB> userBuyRmbMallPbs)
    {
        LoadingOverlay.Instance.Hide();
        if (userBuyRmbMallPbs?.Count==0)
        {
            return;
        }
        
        
        _shopModel.UpdateUserRmbMallVo(userBuyRmbMallPbs);
        View.SetData(_shopModel);

    }

    private void UpdateDailyGiftTime()
    {
        EventDispatcher.TriggerEvent(EventConst.RefreshPoint);
        ClientTimer.Instance.DelayCall(GetUserInfo,2f);
    }


    public override void Start()
    {
        ClientData.LoadItemDescData(null);
        ClientData.LoadSpecialItemDescData(null);
        _shopModel=new ShopModel();
        GetRule();
    }

    private void GetRule()
    {
        LoadingOverlay.Instance.Show();
        NetWorkManager.Instance.Send<MallRuleRes>(CMD.MALL_RULE,null,MallRuleCallBacK);
    }
    
    private void MallRuleCallBacK(MallRuleRes res)
    {
        _shopModel.InitRule(res);		
        GetUserInfo();
    }
	
    private void GetUserInfo()
    {
        LoadingOverlay.Instance.Show();
        NetWorkManager.Instance.Send<MallInfoRes>(CMD.MALL_USERINFO, null, MallInfoCallBack);
    }

    private void MallInfoCallBack(MallInfoRes res)
    {
        LoadingOverlay.Instance.Hide();
        _shopModel.InitUserMallInfo(res);
        View.SetData(_shopModel);

    }

    
    public override void OnMessage(Message message)
    {
        string name = message.Name;
        object[] body = message.Params;
        switch (name)
        {
//            case MessageConst.CMD_MALL_BUYFREEGIFT:
//                OnBuyGoldMallItemClick((GameMallVo)body[0],1);
//                break;
               case  MessageConst.CMD_ACTIVITY_REFRESHACTIVITYDA:
                   var userBuyRmbMallPbs = (RepeatedField<UserBuyRmbMallPB>) body[0];
                   if (userBuyRmbMallPbs!=null)
                   {
                       UpdateDailyGift(userBuyRmbMallPbs);
                   }
                   break;
        }
    }
    
    private void OnBuyGoldMallItemClick(GameMallVo vo, int num)
    {
        LoadingOverlay.Instance.Show();
//        Debug.LogError(vo.MallId+" "+vo.MallType+" "+num);
        _freeGift = vo.MallId;
        var buffer=NetWorkManager.GetByteData(new GotDailyPackageReq(){MallId = vo.MallId,MallType = (int)vo.MallType});
        NetWorkManager.Instance.Send<GotDailyPackageRes>(CMD.USER_GOTDAILYPACKAGE,buffer, BuyGoldSuccessCallBack);
    }

    private void BuyGoldSuccessCallBack(GotDailyPackageRes res)
    {
        LoadingOverlay.Instance.Hide();
        GlobalData.PlayerModel.PlayerVo.ExtInfo = res.UserExtraInfo;
        _shopModel.UpdateUserBuyGameMallVo(new UserBuyGameMallPB(){BuyNum = 1,MallId = _freeGift,MallType = MallTypePB.MallGem});
        //GlobalData.PlayerModel.UpdateUserMoney(res.UserMoney);
//		Debug.LogError(res.Award);
        RewardUtil.AddReward(res.Award);
		
        if (_awardWindow==null)
        {
            _awardWindow=PopupManager.ShowWindow<AwardWindow>("GameMain/Prefabs/AwardWindow/AwardWindow");
        }
        _awardWindow.SetData(res.Award);
		
        View.SetData(_shopModel);
		
        //统计
        GameMallVo mallVo = _shopModel.GameMallDic[_freeGift];
        if (mallVo.MoneyTypePb == MoneyTypePB.MoGem)
        {
            SdkHelper.StatisticsAgent.OnPurchase(mallVo.MallName, res.Award.Count, mallVo.RealPrice);
        }
        else
        {
            SdkHelper.StatisticsAgent.OnEvent(mallVo.MallName,res.Award.Count);
        }
        
        SendMessage(new Message(MessageConst.CMD_ACTIVITY_REFRESH_ACTIVITYTOGGLE_REDDOT));
    }
    
    

    public override void Destroy()
    {
        base.Destroy();
        //EventDispatcher.RemoveEventListener<RepeatedField<UserBuyRmbMallPB>>(EventConst.GetPayInfoSuccess,UpdateDailyGift);
        EventDispatcher.RemoveEventListener<int>(EventConst.PayforDaily,OnPayforDaily);
        EventDispatcher.RemoveEventListener(EventConst.DailyRefresh6,UpdateDailyGiftTime);
        //ClientTimer.Instance.RemoveCountDown(_handler);
    }
}
