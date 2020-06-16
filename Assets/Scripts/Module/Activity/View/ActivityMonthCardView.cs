using System;
using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Module;
using Assets.Scripts.Module.Framework.Utils;
using Common;
using DataModel;
using game.main;
using UnityEngine;
using UnityEngine.UI;

public class ActivityMonthCardView : View
{

   // private Button goToBuy;

    
    //阿晨下面是新的 加上注释，写完删掉注释
    private Button _get;   //领取按钮
    private Button _experienceCard;//体验卡按钮
    private Button _buy;   //购买按钮

    private Text _des; //描述     对应→Activity_MonthCardHint1 = 购买首日立即获得{0}星钻\r\n持续30日\r\n每日获得{1}星钻\r\n体力上限增加{2}\r\n签到双倍奖励\r\n可购买特权礼包
    private Text _experienceText;//体验卡文本描述 对应→Activity_MonthCardHint3 = 体验卡（{0}）
    private Text _price; //价格  对应→ Activity_MonthCardHint4 = ¥ {0}
    private Text _time; //时间   对应→ Activity_MonthCardHint6 = 还剩{0}到期

    private Transform _timeTran;
    
    private bool canReceive=true;


    private Text _hint1;  //这个是30天上面那个描述 立即获得 300 星钻
    private Text _hint2;  //这个是额外再送 1200 星钻
    private Text _num;    //这个是领取按钮上面的数量X40 
    private Transform _content; // 这个是月卡特权礼包生成的父物体
    private ShopModel _shopModel;

    private Text _buywelfare;//买一送一

    private Button _welfareBtn;
    private Button _payProblemBtn;
    private void Awake()
    {
       // goToBuy = transform.GetButton("GoToBuyBtn");
      //  goToBuy.onClick.AddListener(GoToBuyEvent);

      _payProblemBtn = transform.GetButton("Bg/PayProblemBtn");
      _payProblemBtn.gameObject.SetActive(Channel.IsTencent);
      _payProblemBtn.onClick.AddListener(() =>
      {
          PopupManager.ShowWindow<TencentBalanceWindow>("Shop/Prefab/TencentBalanceWindow");
      });

      _welfareBtn = transform.GetButton("Bg/Welfare/Button");
      _welfareBtn.onClick.AddListener(() =>
      {
          MonthCardPresentedActivityWindow window = PopupManager.ShowWindow<MonthCardPresentedActivityWindow>("Activity/Prefabs/MonthCardPresentedActivityWindow");
      });
        //新的
        _get = transform.GetButton("Bg/Get");
        _experienceCard = transform.GetButton("Bg/ExperienceCard");
        _buy = transform.GetButton("Bg/Buy");

        
        _get.onClick.AddListener(Get);
        _experienceCard.onClick.AddListener(ExperienceCard);
        _buy.onClick.AddListener(Buy);
        
        _des = transform.GetText("Bg/Des/Text");
        _experienceText = transform.GetText("Bg/ExperienceCard/Text");
        _price = transform.GetText("Bg/Buy/Price");
        _timeTran = transform.Find("Bg/Time");
        _time = transform.GetText("Bg/Time/Text");

        _hint1 = transform.GetText("Bg/30Days/bg/Hint1");
        _hint2 = transform.GetText("Bg/30Days/bg/Hint2");
        _num = transform.GetText("Bg/Get/Num");
        _content = transform.Find("Bg/GiftbagBg/bg/Content");
        
        
        var prefab = GetPrefab("Activity/Prefabs/MonthCardGiftbagItem");
        for (int i = 0; i < 3; i++)
        {
            var go = Instantiate(prefab,_content,false);
        }

        _buywelfare = transform.Find("Bg/Welfare").GetText();


    }

    private int GetPhoneTickNum()
    {
        return 60;
    }
    
    public void SetData(ShopModel shopModel)
    {
//        _des.text = I18NManager.Get("Activity_MonthCardHint1",shopModel.GetMonthCardMall?.Award[0].Num,GlobalData.ConfigModel.GetConfigByKey(GameConfigKey.MONTH_CARD_GEM_NUM)
//        ,GlobalData.ConfigModel.GetConfigByKey(GameConfigKey.MONTH_CARD_POWER_NUM),GlobalData.ConfigModel.GetConfigByKey(GameConfigKey.MONTH_CARD_LEVEL_EXP_NUM));
//        
        _shopModel = shopModel;
        _hint1.text = shopModel.GetMonthCardMall?.Award[0].Num.ToString();//"300";
        _hint2.text = (Convert.ToInt32(GlobalData.ConfigModel.GetConfigByKey(GameConfigKey.MONTH_CARD_GEM_NUM))*30).ToString();
        _num.text = "x"+GlobalData.ConfigModel.GetConfigByKey(GameConfigKey.MONTH_CARD_GEM_NUM);//"x40";
        _des.text = I18NManager.Get("Activity_MonthCardHint11",GlobalData.ConfigModel.GetConfigByKey(GameConfigKey.MONTH_CARD_LEVEL_EXP_NUM),GetPhoneTickNum(),GlobalData.ConfigModel.GetConfigByKey(GameConfigKey.MONTH_CARD_POWER_NUM));

        
        var monthcardRechargePb = GlobalData.ActivityModel
            .GetUserActivityMonthCardRechargeInfo();
//        Debug.LogError(monthcardRechargePb);
        if (monthcardRechargePb!=null)
        {

            if (monthcardRechargePb.BuyNum>0)
            {
                _buywelfare.gameObject.Hide();
                _buywelfare.text = I18NManager.Get("Activity_BuyOneAndOne",3-monthcardRechargePb.BuyNum);
            }
            
        }
        else
        {
            _buywelfare.gameObject.Hide();
            _buywelfare.text = I18NManager.Get("Activity_BuyOneAndOne",3);
        }


        
        _experienceText.text= I18NManager.Get("Activity_MonthCardHint3",GlobalData.PropModel.GetUserProp(PropConst.TasteCardId).Num);
        var monthCardProduct = GlobalData.PayModel.GetMonthCardProduct();
        _price.text = monthCardProduct.Curreny+" "+monthCardProduct.AreaPrice;
        CreateGiftbag(shopModel.GetSpecialGift);
        SetVIPDailyGemState();
    }

    private void SetVIPDailyGemState()
    {
        if (GlobalData.PlayerModel.PlayerVo.UserMonthCard != null)
        {

            TimeSpan sp = DateUtil.GetDataTime(GlobalData.PlayerModel.PlayerVo.UserMonthCard.EndTime)
                .Subtract(DateUtil.GetDataTime(ClientTimer.Instance.GetCurrentTimeStamp()));
            var day = sp.Days;
                                          
            _time.text =I18NManager.Get("Activity_MonthCardHint6",day);
            if (day > 0)
            {
                _timeTran.gameObject.SetActive(true);
                //_des.gameObject.SetActive(false);
                _get.gameObject.SetActive(true);
            }
            else
            {
                _timeTran.gameObject.SetActive(false);
               // _des.gameObject.SetActive(true);
                _get.gameObject.SetActive(false);
            }


            if (ClientTimer.Instance.GetCurrentTimeStamp()> GlobalData.PlayerModel.PlayerVo.UserMonthCard.EndTime)
            {
                //月卡过期
                _get.gameObject.SetActive(false);
            }
            else
            {
                bool viprewardshow = (ClientTimer.Instance.GetCurrentTimeStamp()+60000 -
                                      GlobalData.PlayerModel.PlayerVo.UserMonthCard.PrizeTime) >= 0&&GlobalData.PlayerModel.PlayerVo.UserMonthCard.EndTime>ClientTimer.Instance.GetCurrentTimeStamp();
                
                Debug.LogError(DateUtil.GetDataTime(ClientTimer.Instance.GetCurrentTimeStamp()+60000)+" priceTime:"+DateUtil.GetDataTime( GlobalData.PlayerModel.PlayerVo.UserMonthCard.PrizeTime));
                _get.gameObject.SetActive(viprewardshow);  
            }
        }
        else
        {
            //为空的时候！
            _timeTran.gameObject.SetActive(false);
            _des.gameObject.SetActive(true);
//            _hasreceive.SetActive(false); 
            _get.gameObject.SetActive(false);
            
        }
    }


    /// <summary>
    /// 生成月卡下面的特权礼拜
    /// </summary>
    /// <param name="list">参数随便填的，阿晨到时候你把生成数据的集合穿过来就好了</param>
    public void CreateGiftbag(List<UserBuyRmbMallVo> list)
    {



        for (int i = 0; i < list.Count; i++)
        {
            if (i>=3)
            {
                break;
            }
            _content.GetChild(i).localScale=Vector3.one;
            _content.GetChild(i).GetComponent<MonthCardGiftbagItem>().SetData(list[i],_shopModel.RmbMallDic[list[i].MallId]);
        }
        
    }
    
    
    //领取按钮事件
    private void Get()
    {
        SendMessage(new Message(MessageConst.CMD_MALL_DAILYGEMREWARD)); 
    }
    
    //体验卡事件
    private void ExperienceCard()
    {
        SendMessage(new Message(MessageConst.CMD_USETASTECARD));
    }

    //购买按钮事件
    private void Buy()
    {
        SendMessage(new Message(MessageConst.CMD_BUYMONTHCARD));
    }
    

}
