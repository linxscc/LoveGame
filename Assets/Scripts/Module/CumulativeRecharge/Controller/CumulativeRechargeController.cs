using System;
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

public class CumulativeRechargeController : Controller
{

    public CumulativeRechargeView View;
    private AccumulativeRechargeModel _accumulativeRechargeModel;
    private AwardWindow _awardWindow;
    
    public override void Start()
    {
        ClientData.LoadItemDescData(null);
        ClientData.LoadSpecialItemDescData(null);
        EventDispatcher.AddEventListener<AccumulativeRechargeVO>(EventConst.ReceiveCumulativeAward,ReveiveRechargeAwardReq);
        _accumulativeRechargeModel=new AccumulativeRechargeModel();
        View.SetData(_accumulativeRechargeModel.GetLongLastingVo());
        
        //这个之后要重构，会统一通知刷新ActivityModel,然后再通知各自模块刷新UI！
//        EventDispatcher.AddEventListener<RepeatedField<UserBuyRmbMallPB>>(EventConst.GetPayInfoSuccess,UpdateAccumulativeData);
    }

//    private void UpdateAccumulativeData(RepeatedField<UserBuyRmbMallPB> obj)
//    {
//        LoadingOverlay.Instance.Show();
//        ClientTimer.Instance.DelayCall(() =>
//        {
//            NetWorkManager.Instance.Send<ActivityRes>(CMD.ACTIVITY_ACTIVITYLIST,null, res =>
//            {
//                GlobalData.ActivityModel.GetAllActivityRes(res);
//                LoadingOverlay.Instance.Hide();
//                View.SetData(_accumulativeRechargeModel.GetLongLastingVo());
//            }); 
//        }, 0.5f);
//    }
    

    private void ReveiveRechargeAwardReq(AccumulativeRechargeVO vo)
    {
        //Debug.LogError(vo.GearId+" "+vo.ActivityId);
        var buffer = NetWorkManager.GetByteData(new ReceiveActivityAccumulativeRechargeAwardReq { Id = vo.GearId,ActivityId = vo.ActivityId});
        LoadingOverlay.Instance.Show();
        NetWorkManager.Instance.Send<ReceiveActivityAccumulativeRechargeAwardRes>(CMD.ACTIVITY_GETACCUMULATIVERECHARGE,
            buffer, res =>
            {
                LoadingOverlay.Instance.Hide();
                GlobalData.ActivityModel.UpdateLongLastRechargeInfoBb(res.UserActivityAccumulativeRechargeInfo);
                RewardUtil.AddReward(res.Awards);
                if (_awardWindow==null)
                {
                    _awardWindow=PopupManager.ShowWindow<AwardWindow>("GameMain/Prefabs/AwardWindow/AwardWindow");
                }
                _awardWindow.SetData(res.Awards);
                View.SetData(_accumulativeRechargeModel.GetLongLastingVo());
                SendMessage(new Message(MessageConst.CMD_ACTIVITY_REFRESH_ACTIVITYTOGGLE_REDDOT));
            });


    }

    public override void OnMessage(Message message)
    {
        string name = message.Name;
        object[] body = message.Params;
        switch (name)
        {
            case MessageConst.CMD_GOTORECHARGE:
                ModuleManager.Instance.EnterModule(ModuleConfig.MODULE_SHOP,false,true,0);
                break;
            case MessageConst.CMD_ACTIVITY_REFRESHACTIVITYDA:
                View.SetData(_accumulativeRechargeModel.GetLongLastingVo());
                SendMessage(new Message(MessageConst.CMD_ACTIVITY_REFRESH_ACTIVITYTOGGLE_REDDOT));
                break;
            
        }
    }

    public override void Destroy()
    {
        //要加这个东西啊！！
        base.Destroy();
        EventDispatcher.RemoveEventListener<AccumulativeRechargeVO>(EventConst.ReceiveCumulativeAward,ReveiveRechargeAwardReq);
//        EventDispatcher.RemoveEventListener<RepeatedField<UserBuyRmbMallPB>>(EventConst.GetPayInfoSuccess,UpdateAccumulativeData);

    }
}
