using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core.Events;
using Assets.Scripts.Framework.GalaSports.Service;
using Common;
using DataModel;
using game.main;
using UnityEngine;
using UnityEngine.UI;

public class MonthCardGiftbagItem : MonoBehaviour
{
    private RawImage _iconImg;
    private Button _iconOnClick;
    private Text _name;
    private Text _priceTxt;
    private Button _priceBtn;
    private int _resourceid;
    private ResourcePB _resourcePb;
    private UserBuyRmbMallVo _rmbvo;
    private GameObject _tips;
    private string desc;

    private void Awake()
    {
        _iconImg = transform.GetRawImage("Icon");
        _name = transform.GetText("Name");
        _priceTxt = transform.GetText("PriceBg/Price");
        _tips = transform.Find("Icon/Tips").gameObject;

        _iconOnClick = transform.GetButton("Icon");
        _priceBtn = transform.GetButton("PriceBg");

        _iconOnClick.onClick.AddListener(IconOnClick);
        _priceBtn.onClick.AddListener(PriceBtn);
    }

    //图标点击事件
    private void IconOnClick()
    {
        //策划说礼包描述
//        var desc = ClientData.GetItemDescById(_resourceid, _resourcePb);
        EventDispatcher.TriggerEvent(EventConst.PayforSpecial, _rmbvo); 
        //月卡用户才能购买特权礼包
//        if (GlobalData.PlayerModel.PlayerVo.UserMonthCard!=null&&
//            ClientTimer.Instance.GetCurrentTimeStamp()< GlobalData.PlayerModel.PlayerVo.UserMonthCard.EndTime)
//        {
//            EventDispatcher.TriggerEvent(EventConst.PayforSpecial, _rmbvo); 
//        }
//        else
//        {
//            FlowText.ShowMessage(I18NManager.Get("Pay_MonthCardCanPay"));
//        }
        

    }

    //购买点击事件
    private void PriceBtn()
    {
        EventDispatcher.TriggerEvent(EventConst.PayforSpecial, _rmbvo); 
//        if (GlobalData.PlayerModel.PlayerVo.UserMonthCard!=null&&
//            ClientTimer.Instance.GetCurrentTimeStamp()< GlobalData.PlayerModel.PlayerVo.UserMonthCard.EndTime)
//        {
//            EventDispatcher.TriggerEvent(EventConst.PayforSpecial, _rmbvo); 
//        }
//        else
//        {
//            FlowText.ShowMessage(I18NManager.Get("Pay_MonthCardCanPay"));
//        }

    }


    public void SetData(UserBuyRmbMallVo rmbvo, RmbMallVo mallVo)
    {
        _iconImg.texture = ResourceManager.Load<Texture>(GlobalData.PropModel.GetGiftPropPath(mallVo.GiftImage));
        _name.text = mallVo.MallName;
        var payData = GlobalData.PayModel.GetProduct(rmbvo.MallId);
        _priceTxt.text = payData != null ?payData.Curreny+ payData.AreaPrice : "";
        _rmbvo = rmbvo;
        desc = mallVo.MallDesc;
        foreach (var v in mallVo.Award)
        {
            _resourceid = v.ResourceId;
            _resourcePb = v.Resource;
        }

        _iconOnClick.enabled = rmbvo.BuyNum == 0;
        _tips.SetActive(rmbvo.BuyNum > 0);
        //要加一个已售罄！
    }
}