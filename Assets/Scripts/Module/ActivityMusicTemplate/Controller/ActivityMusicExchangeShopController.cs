using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Core.Events;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module.NetWork;
using Com.Proto;
using Common;
using DataModel;
using game.main;
using UnityEngine;
using Utils;

public class ActivityMusicExchangeShopController : Controller
{
 
    public ActivityMusicExchangeShopView View;
    public ActivityExchangeShopModel ShopModel;
    private ActivityMusicExchangeWindow _window;
   
    public override void Init()
    {     
       EventDispatcher.AddEventListener<ActivityExchangeShopVo>(EventConst.OnClickExchangeItem,OnClickExchangeItem);            
    }

    private void OnClickExchangeBuyBtn(ActivityExchangeShopVo vo, int num)
    {
        var mallItemNum = GlobalData.PropModel.GetUserProp(vo.MallItemId).Num;  
        var price = num * vo.Price;
    
      if (price > mallItemNum)
      {
         FlowText.ShowMessage("兑换道具数量不足");
         return;
      }
  
      _window.CloseWindow(); 
        
        ActivityExchangeMallReq req =new ActivityExchangeMallReq
        {
            ActivityId = vo.ActivityId,
            MallId = vo.MallId,
            Num = num,
        };
        byte[] data = NetWorkManager.GetByteData(req);
        NetWorkManager.Instance.Send<ActivityExchangeMallRes>(CMD.ACTIVITY_ACTIVITYEXCHANGEMALL, data, ExchangeSucceed);
    }

    private void ExchangeSucceed(ActivityExchangeMallRes res)
    {           
        RewardUtil.AddReward(res.Awards);
    
        int num = 0;
        RewardVo vo = null;
        foreach (var t in res.Awards)
        {      
            vo=new RewardVo(t);
            num++;            
        }

        if (vo != null) FlowText.ShowMessage(I18NManager.Get("Activity_Get", vo.Name, num));

        GlobalData.PropModel.UpdateProps(new []{res.UserItem});
      
        ShopModel.UpdateUserData(res.UserBuyActivityMall.ActivityId, res.UserBuyActivityMall);
        View.RefreshExchangeItemNum();
        View.RefreshExchangeShopItem(ShopModel.GetUserData);
       
    }


    public override void Start()
    {        
        View.SetData(ShopModel.GetUserData,ShopModel.ExchangeItemId,ShopModel.ExchangeItemImgPath);   
    }
   
    public override void Destroy()
    {
        base.Destroy();
        EventDispatcher.RemoveEvent(EventConst.OnClickExchangeItem);      
    }


 
    
    //点击兑换商店Item打开兑换窗口
    private void OnClickExchangeItem(ActivityExchangeShopVo vo)
    {     
        if (_window==null)
        {
           _window = PopupManager.ShowWindow<ActivityMusicExchangeWindow>("ActivityMusicTemplate/Prefabs/ActivityMusicExchangeWindow");
           _window.SetData(vo,OnClickExchangeBuyBtn);
        }
       
    }

  
  
}
