using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core;
using Common;
using DataModel;
using game.main;
using UnityEngine;
using UnityEngine.UI;

public class ActivityGrowthCapitalView : View
{

 
    private LoopVerticalScrollRect _growthItemList;
    private List<GrowthFunVo> _growthFunVos;


    private Button _buy;   //购买按钮
 //   private Text _des;     //文本描述  Activity_GrowthHint1
    private Text _price;   //价格     Activity_GrowthHint2
    
    private float _lastClickTime=0;
    private int hasGrowthfund = 0;
    
    private Button _payProblemBtn;
    
    private void Awake()
    {
        _payProblemBtn = transform.GetButton("Bg/PayProblemBtn");
        _payProblemBtn.gameObject.SetActive(Channel.IsTencent);
        _payProblemBtn.onClick.AddListener(() =>
        {
            PopupManager.ShowWindow<TencentBalanceWindow>("Shop/Prefab/TencentBalanceWindow");
        });
        
        _growthItemList = transform.Find("Bg/GrowthFundList/GrowthFundList").GetComponent<LoopVerticalScrollRect>();
        _growthItemList.prefabName = "Activity/Prefabs/GrowthFundItem";
        _growthItemList.poolSize = 8;


        _buy = transform.GetButton("Bg/GemBg/PriceBg");
        _buy.onClick.AddListener(Buy);
        
       // _des = transform.GetText("Bg/Top/DesBg/Text");
        _price = transform.GetText("Bg/GemBg/PriceBg/Price");


        var productVo = GlobalData.PayModel.GetGrowthCapitalProduct();
        var price = productVo?.Curreny+productVo?.AreaPrice;
        Debug.LogError(price);
        //_des.text = I18NManager.Get("Activity_GrowthHint1",price);  
        _price.text = price; // I18NManager.Get("Activity_GrowthHint2", );//价格是写死的68。

    }

    private void Buy()
    {
        Debug.LogError(Time.realtimeSinceStartup);
        if (hasGrowthfund==1&&Time.realtimeSinceStartup- _lastClickTime <10f)
        {
            FlowText.ShowMessage(I18NManager.Get("Shop_DontRepeatBuy"));
            return;
        }

        
        if (GlobalData.PlayerModel.PlayerVo.ExtInfo.GrowthFund==0)
        {
            SdkHelper.PayAgent.Pay(GlobalData.PayModel.GetGrowthCapitalProduct());
            hasGrowthfund = 1;
            _lastClickTime = Time.realtimeSinceStartup;
                 
        }
        else
        {
            FlowText.ShowMessage(I18NManager.Get("Activity_HasBuy"));
        }
        
    }


    public void SetData(List<GrowthFunVo> vo)
    {
        _growthFunVos = vo;
        Debug.LogError(vo.Count);
        SetGrowthList(vo);
        _buy.gameObject.SetActive(GlobalData.PlayerModel.PlayerVo.ExtInfo.GrowthFund==0);

    }

    private void SetGrowthList(List<GrowthFunVo> vo)
    {
//        _growthItemList.RefillCells();
        _growthItemList.UpdateCallback = GrowthFundItemCallBack;
        _growthItemList.totalCount = vo.Count;
        _growthItemList.RefreshCells();

    }

    private void GrowthFundItemCallBack(GameObject gameObject, int index)
    {
        gameObject.GetComponent<GrowthFundItem>().SetData(_growthFunVos[index]);
        
        
    }
}
