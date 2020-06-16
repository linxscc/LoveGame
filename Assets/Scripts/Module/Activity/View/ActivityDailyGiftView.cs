using System;
using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module;
using Assets.Scripts.Module.Framework.Utils;
using Com.Proto;
using Common;
using DataModel;
using game.main;
using Google.Protobuf.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ActivityDailyGiftView : View
{



    private Text _des; //描述     对应→Activity_MonthCardHint1 = 购买首日立即获得{0}星钻\r\n持续30日\r\n每日获得{1}星钻\r\n体力上限增加{2}\r\n签到双倍奖励\r\n可购买特权礼包
    private Text _price; //价格  对应→ Activity_MonthCardHint4 = ¥ {0}
    private Text _time; //时间   对应→ Activity_MonthCardHint6 = 还剩{0}到期

    private Transform _timeTran;
    
    private bool canReceive=true;

    private Transform _freeItem;
//    private Transform _propContainer;
    private Button _get;   //领取按钮
    private GameMallVo _gameMallVo;
    private UserBuyGameMallVo _userBuyGameMallVo;
    private LoopVerticalScrollRect _loopVerticalScroll;
    private List<UserBuyRmbMallVo> _rmbMallVos;
    private ShopModel _shopModel;

    private Button _payProblemBtn;
    
    private void Awake()
    {      
        _time = transform.GetText("Bg/Time/Text");
        _loopVerticalScroll = transform.Find("Bg/ListContent/DailyGift/DailyGiftList").GetComponent<LoopVerticalScrollRect>();
        _loopVerticalScroll.prefabName = "Activity/Prefabs/DailyGiftItem";
        _loopVerticalScroll.poolSize = 6;
        _loopVerticalScroll.UpdateCallback = ListUpdateCallback;

        _payProblemBtn = transform.GetButton("Bg/PayProblemBtn");
        _payProblemBtn.gameObject.SetActive(Channel.IsTencent);
        _payProblemBtn.onClick.AddListener(() =>
        {
            PopupManager.ShowWindow<TencentBalanceWindow>("Shop/Prefab/TencentBalanceWindow");
        });
    }

    private void ListUpdateCallback(GameObject go, int index)
    {
        if (index==0)
        {
            go.GetComponent<DailyGiftItem>().SetFreeAward(_gameMallVo,_userBuyGameMallVo);
        }
        else
        {
            go.GetComponent<DailyGiftItem>().SetData(_shopModel.RmbMallDic[_rmbMallVos[index-1].MallId],_rmbMallVos[index-1]);
        }
        
    }

    public void SetData(ShopModel shopModel)
    {
        //逻辑:先设置免费的，然后设置RMBmallvo哪些。
        _shopModel = shopModel;
        _userBuyGameMallVo = shopModel.GetFreeGift;
        if (_userBuyGameMallVo!=null)
        {
            _gameMallVo = shopModel.GameMallDic[_userBuyGameMallVo.MallId];
//            SetFreeAward(_gameMallVo.Award);
//            _des.text = _userBuyGameMallVo.BuyNum > 0 ? I18NManager.Get("Shop_TodayHasBug") : I18NManager.Get("Shop_DailyBuyLimit");
//            _get.image.color=_userBuyGameMallVo.BuyNum > 0 ? Color.grey : Color.white;
//            _price.text=_userBuyGameMallVo.BuyNum > 0 ? I18NManager.Get("Common_AlreadyGet") : I18NManager.Get("Common_Free");
            _rmbMallVos = shopModel.GetTargetRmbMallList(MallLabelPB.LabelDailyGift);
 
            SetRmbDailyGift(_rmbMallVos);
        }
        else
        {
            _get.gameObject.SetActive(false);
            _des.gameObject.SetActive(false);
        }



    }

    public void SetRmbDailyGift(List<UserBuyRmbMallVo> dailyMallVos)
    {
        //_loopVerticalScroll.RefillCells();
        _loopVerticalScroll.totalCount = dailyMallVos.Count+1;
        _loopVerticalScroll.RefreshCells();
    }
    


    //领取按钮事件
//    private void Get()
//    {
//        if (_userBuyGameMallVo.BuyNum==0)
//        {
//            SendMessage(new Message(MessageConst.CMD_MALL_BUYFREEGIFT,Message.MessageReciverType.CONTROLLER,_gameMallVo));
//        }
//        
//    }
    



}
