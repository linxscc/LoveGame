using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Core.Events;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module.Framework.Utils;
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

public class ActivityMonthCardController : Controller
{
    public ActivityMonthCardView View;
    private ShopModel _shopModel;
//    private Dictionary<string, string> _pushDic;
//    private string refreshPoint = "6:00:00";
//    private DateTime refreshTime;
    private float _lastClickTime=0;
    private List<int> _tempmallidlist;
    private GiftPackWindow _giftPackWindow;
    
    public override void Init()
    {
       // _pushDic=new Dictionary<string, string>();
        _tempmallidlist=new List<int>();
//        EventDispatcher.AddEventListener<RepeatedField<UserBuyRmbMallPB>>(EventConst.GetPayInfoSuccess,UpdateMonthCard);
        EventDispatcher.AddEventListener<UserBuyRmbMallVo>(EventConst.PayforSpecial,BuySpcialGift);
        EventDispatcher.AddEventListener<int>(EventConst.PayforSpecialGift,OnPayGiftClick);
    }

    private void OnPayGiftClick(int mallid)
    {
        if (Time.realtimeSinceStartup - _lastClickTime < 10f && _tempmallidlist.Contains(mallid))
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
        _giftPackWindow?.Close();
    }

    private void BuySpcialGift(UserBuyRmbMallVo userBuyRmbMallVo)
    {      
        Debug.LogError("ComeHere!");
        if (_giftPackWindow==null)
        {
            _giftPackWindow=PopupManager.ShowWindow<GiftPackWindow>("Shop/Prefab/MallWindow/GiftPackWindow");
        }
        _giftPackWindow.SetData(userBuyRmbMallVo,_shopModel.RmbMallDic[userBuyRmbMallVo.MallId]);
    }

//    private void UpdateMonthCard(RepeatedField<UserBuyRmbMallPB> userBuyRmbMallPbs)
//    {
//        //ClientTimer.Instance.DelayCall(GetUserInfo,0.1f);
//        //todo 支付成功后，要刷新一次活动数据！
//        LoadingOverlay.Instance.Show();
//        NetWorkManager.Instance.Send<ActivityRes>(CMD.ACTIVITY_ACTIVITYLIST,null, res =>
//        {
//            GlobalData.ActivityModel.GetAllActivityRes(res);
//            LoadingOverlay.Instance.Hide();
//            _shopModel.UpdateUserRmbMallVo(userBuyRmbMallPbs);
//            View.SetData(_shopModel);
//        });
//        
//
//    }


    public override void Start()
    {
        _shopModel=new ShopModel();
        //GetRule();
        GetService<ShopService>().SetCallback(InitMonthCardView).Execute();
    }

    private void InitMonthCardView(ShopModel shopModel)
    {
        _shopModel = shopModel;
        View.SetData(_shopModel);
    }


//    private void MallRuleCallBacK(MallRuleRes res)
//    {
//        _shopModel.InitRule(res);		
//        GetUserInfo();
//    }
	
//    private void GetUserInfo()
//    {
//        NetWorkManager.Instance.Send<MallInfoRes>(CMD.MALL_USERINFO, null, MallInfoCallBack);
//    }
//
//    private void MallInfoCallBack(MallInfoRes res)
//    {
//        LoadingOverlay.Instance.Hide();
//        _shopModel.InitUserMallInfo(res);
//        View.SetData(_shopModel);
//
//    }

    
    public override void OnMessage(Message message)
    {
        string name = message.Name;
        object[] body = message.Params;
        switch (name)
        {
            case MessageConst.CMD_USETASTECARD:
                //要先用复选框！
                var tasteNum = GlobalData.PropModel.GetUserProp(PropConst.TasteCardId).Num;

                if (tasteNum==0)
                {
                    FlowText.ShowMessage(I18NManager.Get("Shop_NoVipTaste"));
                    return;
                }
	            

                PopupManager.ShowConfirmWindow(I18NManager.Get("Shop_UseTasteCard",tasteNum,tasteNum)).WindowActionCallback = evt =>
                {
                    if (evt==WindowEvent.Ok)
                    {
                        UseTasteCard();
                    }
                };

                break;
            case MessageConst.CMD_BUYMONTHCARD:
                SdkHelper.PayAgent.PayMonthCard();
                break;
            case MessageConst.CMD_MALL_DAILYGEMREWARD:
                GetMonthCardReward();
                break;
            case MessageConst.CMD_ACTIVITY_REFRESHACTIVITYDA:
                var userBuyRmbMallPbs = (RepeatedField<UserBuyRmbMallPB>) body[0];
                if (userBuyRmbMallPbs!=null)
                {
                    _shopModel.UpdateUserRmbMallVo(userBuyRmbMallPbs);
                    View.SetData(_shopModel); 
                }
                else
                {
                    Debug.LogError("message.Body is null"+body.Length);
                }

                break;
        }
    }
    
    private void UseTasteCard()
    {
        LoadingOverlay.Instance.Show();
        var buffer = NetWorkManager.GetByteData(new UseVipExperienceReq{ItemId = PropConst.TasteCardId});	
        NetWorkManager.Instance.Send<UseVipExperienceRes>(CMD.MonthCard_UseTasteCard, buffer,GetTasteCardCallBack);
    }

    private void GetTasteCardCallBack(UseVipExperienceRes res)
    {
        FlowText.ShowMessage(I18NManager.Get("Activity_UseVipCardSuccess"));
        GlobalData.PropModel.UpdateProps(new []{res.UserItem});
        GlobalData.PlayerModel.PlayerVo.UserMonthCard = res.UserMonthCard;
        EventDispatcher.TriggerEvent(EventConst.RefreshActivityImageAndActivityPage);
        //刷新UI！
        View.SetData(_shopModel);
        LoadingOverlay.Instance.Hide();
    }
    
    private void GetMonthCardReward()
    {
        LoadingOverlay.Instance.Show();
        NetWorkManager.Instance.Send<ReceiveMonthCardGemRes>(CMD.MonthCard_ReveiveDailyGem, null,GetMonthCardRewardCallBack);
    }

    private void GetMonthCardRewardCallBack(ReceiveMonthCardGemRes res)
    {
        //更新各种状态啊！
        FlowText.ShowMessage(I18NManager.Get("Shop_GetDailyGem"));
        GlobalData.PlayerModel.PlayerVo.UserMonthCard = res.UserMonthCard;
        RewardUtil.AddReward(res.Award);
        View.SetData(_shopModel);
        SendMessage(new Message(MessageConst.CMD_ACTIVITY_REFRESH_ACTIVITYTOGGLE_REDDOT));
        LoadingOverlay.Instance.Hide();
    }

    public override void Destroy()
    {
        base.Destroy();
        ClientData.Clear();
//        EventDispatcher.RemoveEventListener<RepeatedField<UserBuyRmbMallPB>>(EventConst.GetPayInfoSuccess,UpdateMonthCard);
        EventDispatcher.RemoveEventListener<UserBuyRmbMallVo>(EventConst.PayforSpecial,BuySpcialGift);
        EventDispatcher.RemoveEventListener<int>(EventConst.PayforSpecialGift,OnPayGiftClick);
    }
}
