using System.Collections;
using System.Collections.Generic;
using DataModel;
using game.main;
using UnityEngine;
using UnityEngine.UI;

public class CapsuleBattleBuyPlayNumWindow : Window
{
   private Transform _freeContent;
   private Transform _costGemContent;

   private Text _freeDescTxt;
   private Text _costDestTxt;
   private Text _costOncePriceTxt;
   private Text _costMorePriceTxt;
   private Text _numTxt;
   
   
   private Button _freeBtn;
   private Button _onceBtn;
   private Button _moreBtn;


  private int _price;    //单价
  private int _freeNum;  //免费的次数
  private int _buyMax;   //上限说
   private int _moreNum;  //更多次数
   
  
  
  

   private int _buyCount;

  
   
   private void Awake()
   {
       _freeContent = transform.Find("FreeContent");
       _costGemContent = transform.Find("CostGemContent");

       _freeDescTxt = _freeContent.GetText("Text");       
       _costDestTxt = _costGemContent.GetText("Text");
       _costOncePriceTxt = _costGemContent.GetText("OncePrice/Text");
       _costMorePriceTxt = _costGemContent.GetText("MorePrice/Text");
       _numTxt = _costGemContent.GetText("NumText");
       
       _freeBtn = _freeContent.GetButton("FreeBuyBtn");
       _onceBtn = _costGemContent.GetButton("OnceBtn");
       _moreBtn = _costGemContent.GetButton("MoreBtn");
       
       _freeBtn.onClick.AddListener(FreeBtn);
       _onceBtn.onClick.AddListener(OnceBtn); 
       _moreBtn.onClick.AddListener(MoreBtn);

      // _moreNum = GlobalData.ConfigModel.GetConfigByKey(GameConfigKey.ACTIVITY_CAPSULE_COPY_SECOND_BUY_COUNT);
      
   }
   private void FreeBtn()
   {
     
       WindowEvent = WindowEvent.Ok;
       Close();
   }
   
   private void OnceBtn()
   {
       var isCanBuy = IsCanBuy(_price,false);
       if (isCanBuy)
       {
          
           WindowEvent = WindowEvent.Cancel;
           Close();
       }
       else
       {
           SetFlowText(_price,false);
       }
   }
   
   private void MoreBtn()
   {
       var isCanBuy = IsCanBuy(_price*_moreNum,true);
       if (isCanBuy)
       {
          
           WindowEvent = WindowEvent.Yes;
           Close();
       }
       else
       {
           SetFlowText(_price * _moreNum,true);
           
       }
   }

   private void SetFlowText(int costGemNum,bool isMore)
   {
       var haveGemNum = GlobalData.PlayerModel.PlayerVo.Gem;
       var residueBuyNum = _buyMax+_freeNum - _buyCount;
       if (haveGemNum<costGemNum)
       {
           FlowText.ShowMessage(I18NManager.Get("Shop_NotEnoughGem"));
           return;
       }

       if (isMore)
       {
           if (residueBuyNum <= 0)
           {
               FlowText.ShowMessage(I18NManager.Get("ActivityCapsuleBattle_TodayNoBuy"));         
           }
           else if (residueBuyNum<_moreNum && residueBuyNum>0)
           {
               FlowText.ShowMessage(I18NManager.Get("ActivityCapsuleBattle_TodayNoBuyTenNum",_moreNum));
           }
       }
       else
       {
           if (residueBuyNum<=0)
           {
               FlowText.ShowMessage(I18NManager.Get("ActivityCapsuleBattle_TodayNoBuy"));             
           }
       }
      
   }

   private bool IsCanBuy(int costGemNum,bool isMore)
   {
       var haveGemNum = GlobalData.PlayerModel.PlayerVo.Gem;
       var residueBuyNum =_buyMax +_freeNum- _buyCount;     
       if (haveGemNum>=costGemNum)
       {
           if (isMore)
           {
               return residueBuyNum>=_moreNum;
           }
           else
           {
               return residueBuyNum > 0;
           }
           
       }
       else
       {
           return false;
       }

   }

   /// <summary>
   /// 设置购买扭蛋可玩次数窗口数据
   /// </summary>
   /// <param name="isFree">是否可以免费</param>
   /// <param name="freeNum">免费购买送的次数</param>
   /// <param name="price">单价</param>
   /// <param name="moreNum">购买更多次数</param>
   public void SetData(CapsuleLevelVo vo,ActivityVo curActivity)
   {
       _price = curActivity.ActivityExtra.LevelEachNumGem;
       Debug.LogError("单价---》"+_price);
       _buyMax = curActivity.ActivityExtra.LevelBuyMax;
       _freeNum = curActivity.ActivityExtra.LevelReplyNum;
       _moreNum = curActivity.ActivityExtra.SecondBuyCount;
       _buyCount = vo.CapsuleBattleBuyCount;             
       var isFree = vo.IsFree;
          
       if (isFree)
       {
           _freeContent.gameObject.Show();
           _costGemContent.gameObject.Hide();
           _freeDescTxt.text = I18NManager.Get("ActivityCapsuleBattle_FreeBuyDesc",_freeNum);
       }
       else
       {
           _freeContent.gameObject.Hide();
           _costGemContent.gameObject.Show();
           _costDestTxt.text = I18NManager.Get("ActivityCapsuleBattle_CostBuyDesc");
           _numTxt.text = I18NManager.Get("ActivityCapsuleBattle_TodayResidueBuyNum",_buyMax+_freeNum-_buyCount);
           SetCostPrice(_price, _moreNum);           
       }     
   }

   /// <summary>
   /// 设置花费星钻的Txt
   /// </summary>
   /// <param name="price">单价</param>
   /// <param name="moreNum">更多的次数</param>
   private void SetCostPrice(int price,int moreNum)
   {
       _onceBtn.transform.GetText("Text").text = I18NManager.Get("ActivityCapsuleBattle_CostNumBtn", 1);
       _moreBtn.transform.GetText("Text").text =  I18NManager.Get("ActivityCapsuleBattle_CostNumBtn", moreNum);;

       _costOncePriceTxt.text = "x " + price;
       _costMorePriceTxt.text = "x " + (price * moreNum);
   }
        

}
