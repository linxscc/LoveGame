using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core.Events;
using Assets.Scripts.Framework.GalaSports.Service;
using Common;
using Componets;
using DataModel;
using game.main;
using UnityEngine;
using UnityEngine.UI;

public class ActivityMusicExchangeWindow : Window
{

   private Text _title;  
   private Text _desc;
   private Frame _frame;
   private Text _num;
   private LongPressButton _reduceBtn;
   private LongPressButton _addBtn;   
   private Text _curNum;
   private Text _price;
   private RawImage _exchangeIcon;
   private Text _remainBuyNum;
   private Button _buyBtn;

   private int _curBuyNum;

   private ActivityExchangeShopVo _data;

   private Action<ActivityExchangeShopVo, int> _onClickBuyCallBack;
   
   private void Awake()
   {
      _title = transform.GetText("window/TitleText");
      _desc = transform.GetText("window/Desc");
      _frame = transform.Find("window/ItemContent/SmallFrame").GetComponent<Frame>();
      _num = transform.GetText("window/ItemContent/ItemNum");
      _reduceBtn = transform.Find("window/ReduceBtn").GetComponent<LongPressButton>();
      _addBtn = transform.Find("window/AddBtn").GetComponent<LongPressButton>();
      _curNum = transform.GetText("window/CurNum");
      _price = transform.GetText("window/Content");
      _exchangeIcon = transform.GetRawImage("window/Content/Image");
      _remainBuyNum = transform.GetText("window/BuyCount");
      _buyBtn = transform.GetButton("window/BuyBtn");
      _buyBtn.onClick.AddListener(OnClickBuyBtn);
      SetAddBtn();
      SetReduceBtn();
   }

   private void OnClickBuyBtn()
   {
      _onClickBuyCallBack(_data, _curBuyNum);
   }

   public void CloseWindow()
   {
      base.Close();
   }
   
   
   private void SetAddBtn()
   {
      _addBtn.OnDown = () =>
      {
         if (_curBuyNum>=_data.RemainBuyNum)
         {
            return;
         }

         _curBuyNum++;
         _num.text = _curBuyNum.ToString();
         _price.text = I18NManager.Get("Shop_CostToBuy",_curBuyNum*_data.Price);
      };
   }

   private void SetReduceBtn()
   {
      _reduceBtn.OnDown = () =>
      {
         if (_curBuyNum <= 1)
         {
            return;
         }
         _curBuyNum--;
         _num.text = _curBuyNum.ToString();
         _price.text = I18NManager.Get("Shop_CostToBuy",_curBuyNum*_data.Price); 
      };
   }
  
   public void SetData(ActivityExchangeShopVo vo,Action<ActivityExchangeShopVo,int> callBack)
   {
      _onClickBuyCallBack = null;
      _onClickBuyCallBack = callBack;
      _data = vo;
      _curBuyNum = 1;
      _title.text = I18NManager.Get("Common_Buy") + vo.Rewards[0].Name;
     
      _frame.SetData(vo.Rewards[0]);
      _num.text = _curBuyNum.ToString();
      SetCurNum(vo.Rewards);         
      _price.text = I18NManager.Get("Shop_CostToBuy",_curBuyNum*vo.Price);
      _exchangeIcon.texture = ResourceManager.Load<Texture>(vo.ExchangeIconPath);
      _remainBuyNum.text = I18NManager.Get("Shop_LeftBuyCounts2", vo.RemainBuyNum, vo.BuyMax);     
      _desc.text = I18NManager.Get("Shop_GoodsDesc")+ClientData.GetItemDescById(vo.Rewards[0].Id,vo.Rewards[0].Resource).ItemDesc;
   }

   

   private void SetCurNum(List<RewardVo> list)
   {    
      foreach (var t in list)
      {
         switch (t.Resource)
         {          
            case ResourcePB.Item:
               _curNum.text =I18NManager.Get("Shop_CurrentOwn",GlobalData.PropModel.GetUserProp(t.Id).Num);
               break;         
            case ResourcePB.Power:
               _curNum.text =I18NManager.Get("Shop_CurrentOwn",GlobalData.PlayerModel.PlayerVo.Energy);
               break;
            case ResourcePB.Gem:
               _curNum.text =I18NManager.Get("Shop_CurrentOwn",GlobalData.PlayerModel.PlayerVo.Gem);
               break;
            case ResourcePB.Gold:
               _curNum.text =I18NManager.Get("Shop_CurrentOwn",GlobalData.PlayerModel.PlayerVo.Gold);
               break;  
            case ResourcePB.Memories:
               _curNum.text =I18NManager.Get("Shop_CurrentOwn",GlobalData.PlayerModel.PlayerVo.RecollectionEnergy);
               break;
            case ResourcePB.Puzzle:
               var isNull = GlobalData.CardModel.GetUserPuzzleVo(t.Id) == null;
               var curNum = 0;
               if (!isNull)
                  curNum = GlobalData.CardModel.GetUserPuzzleVo(t.Id).Num;
               
               _curNum.text =I18NManager.Get("Shop_CurrentOwn",curNum);  
               break;

         }
      }
   }
}
