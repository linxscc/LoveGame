using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Framework.GalaSports.Core.Events;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module.NetWork;
using Assets.Scripts.Module.Pay.Agent;
using Com.Proto;
using Common;
using Componets;
using DataModel;
using Framework.GalaSports.Service;
using GalaSDKBase;
using game.main;
using UnityEngine;
using Utils;

public class PayAgentIOS : IPayAgent
{
    public PayAgentIOS()
    {
        PayAgent.PayChannel = "APPLE";

        GalaSDKBaseCallBack.Instance.GALASDKPayInitSuccessEvent += OnPayInit;
        GalaSDKBaseCallBack.Instance.GALASDKPaySuccessEvent += OnPaySuccess;
        GalaSDKBaseCallBack.Instance.GALASDKPayFailEvent += OnPayFail;
    }

    public void InitPay()
    {
        string[] productIds = GlobalData.PayModel.GetProductIds();
        GalaSDKBaseFunction.InitPay(GalaSDKBaseFunction.GalaSDKType.Apple, productIds);

//        OnPayInit(
//            "{\"CN_gift_25\":\"88.98\",\"CN_gift_09\":\"148.98\",\"storeCountry\":\"SG\",\"currency\":\"SGD\",\"CN_gift_22\":\"16.98\",\"CN_gift_03\":\"1.48\",\"CN_gift_16\":\"44.98\",\"CN_gift_05\":\"6.98\",\"CN_gift_11\":\"8.98\",\"CN_gift_13\":\"17.98\",\"CN_gift_07\":\"28.98\",\"CN_gift_21\":\"10.98\",\"CN_gift_02\":\"1.48\",\"CN_gift_15\":\"36.98\",\"CN_gift_23\":\"22.98\",\"CN_gift_04\":\"4.48\",\"CN_gift_10\":\"2.98\",\"CN_gift_19\":\"5.98\",\"CN_gift_06\":\"14.98\",\"CN_gift_12\":\"1.48\",\"CN_gift_17\":\"60.98\",\"CN_gift_20\":\"5.98\",\"CN_gift_01\":\"1.48\",\"CN_gift_14\":\"21.98\",\"CN_gift_24\":\"64.98\",\"CN_gift_18\":\"65.98\",\"CN_gift_08\":\"68.98\"}");
        
    }

    private void OnPayInit(string json)
    {
        Debug.Log("OnPayInit===>" + json);
        CheckPayWhenLogin();
        
        JSONObject jsonObject = new JSONObject(json);
        GlobalData.PayModel.SetAreaPrice(jsonObject);
    }

    private void OnPayFail(string json)
    {
        Debug.Log("OnPayFail===>\n" + json);
    }

    private void OnPaySuccess(string json)
    {
        Debug.Log("OnPaySuccess===>\n" + json);

        CheckOrderReq req = new CheckOrderReq
        {
            OrderId = "",
            Origin = PayAgent.PayChannel,
            PayType = -1,
            SdkVersion = "100",
            Version = "100",
            Environment = 0, //TODO 0沙箱，1正式
            Type = CommodityTypePB.Recharge,
            Channel = PayAgent.PayChannel,
            Tag = AppConfig.Instance.payKey
        };

        JSONObject jsonObject = new JSONObject(json);
        List<JSONObject> arr = jsonObject["dataInfos"].list;

        for (int i = 0; i < arr.Count; i++)
        {
            JSONObject obj = arr[i];

            string productId = obj["productId"].str;

            string str = obj["dataInfo"].str;

            Debug.Log("commodityId0===>" + str);

            CheckTransactionIdPB pb;
            if (string.IsNullOrEmpty(str))
            {
                pb = new CheckTransactionIdPB()
                {
                    CommodityId = 0,
                    Extra = "",
                    OrderCreateTime = obj["beginTime"].i,
                    PayStatus = obj["payState"].str,
                    ProductId = productId,
                    TransactionId = obj["transactionId"].str,
                    Type = CommodityTypePB.Recharge,
                };
            }
            else
            {
                string commodityId = Payload.GetAppleCommodityIdByPayLoad(str);
                long extInt = Payload.GetAppleExtIntByPayLoad(str);

                ProductVo productVo = GlobalData.PayModel.GetProduct(Convert.ToInt32(commodityId));

                pb = new CheckTransactionIdPB()
                {
                    CommodityId = productVo.CommodityId,
                    Extra = str,
                    OrderCreateTime = obj["beginTime"].i,
                    PayStatus = obj["payState"].str,
                    ProductId = productId,
                    TransactionId = obj["transactionId"].str,
                    Type = productVo.ProductType,
                    TriggerGiftId = new TriggerGiftIdPb()
                    {
                        Id = extInt
                    }
                };
            }

            req.CheckTransactions.Add(pb);
        }

        JSONObject jo = new JSONObject();
        jo.AddField("receiptData", jsonObject["receptdata"].str);
        req.Params = jo.ToString();

        byte[] buff = NetWorkManager.GetByteData(req);
        NetWorkManager.Instance.Send<CheckOrderRess>(CMD.RECHARGEC_CHECKAPPLEORDER, buff, OnCheckPayListSuccess,
            OnCheckPayListFail);
    }

    private void OnCheckPayListFail(HttpErrorVo errorVo)
    {
        Debug.Log("OnCheckPayListFail======>" + errorVo.ToString());
        FlowText.ShowMessage("支付失败：" + errorVo.ErrorString + " code:" + errorVo.ErrorCode);
   
    }
    
    

    private void OnCheckPayListSuccess(CheckOrderRess resList)
    {
        PayHelper.SetGlobal(resList);

        if (resList.Awards?.Count > 0)
        {
            RewardUtil.AddReward(resList.Awards);
            
            AwardWindow awardWindow = PopupManager.ShowWindow<AwardWindow>("GameMain/Prefabs/AwardWindow/AwardWindow");
            awardWindow.SetData(resList.Awards);
        }
        
        EventDispatcher.TriggerEvent(EventConst.GetPayInfoSuccess,resList.UserBuyRmbMall);

        List<string> list = new List<string>();
        foreach (var res in resList.CheckOrderRess_)
        {
            list.Add(res.OrderId);
            if (res.Status == OrderStatusPB.StatusFiish)
            {
                Debug.Log("充值成功---TransactionId：" + res.TransactionId);
            }
            else
            {
                Debug.Log("充值状态：" + res.Status + " ---TransactionId：" + res.TransactionId);
            }

            ProductVo vo = GlobalData.PayModel.GetProduct(res.CommodityId);
            SdkHelper.StatisticsAgent.IOSPay(res, vo);
        }

        Debug.Log("OnCheckPayListSuccess resList.TransactionId=======>" + ObjectDumper.Dump(resList.TransactionId));

        Debug.Log("first"+GlobalData.PlayerModel.PlayerVo.FirstRecharges.Count);

        GalaSDKBaseFunction.EndPay(GalaSDKBaseFunction.GalaSDKType.Apple, resList.TransactionId.ToArray());
        
        if (resList.TriggerId != null && resList.TriggerId.Count > 0)
        {
            Debug.LogWarning("resList.TriggerId-------->" + resList.TriggerId[0]);
            EventDispatcher.TriggerEvent(EventConst.TriggerGiftPaySuccess, resList.TriggerId);
        }
        
        if (resList.UserBuyRmbMall == null)
        {
            string orderId = "none";
            if (list.Count > 0)
            {
                orderId = list[0];
            }
            BuglyAgent.ReportException("checkOrderSuccess UserBuyRmbMall", "resList.UserBuyRmbMall为空 orderId:" + orderId,"");
        }
    }

    public void Pay(ProductVo product, PayAgent.PayType payType = PayAgent.PayType.None)
    {
        //CreateOrder(product,
        //    () => { GalaSDKBaseFunction.Pay(GalaSDKBaseFunction.GalaSDKType.Apple, GetPayInfo(product)); });      
        CreateOrder(product,
            () => { GalaSDKBaseFunction.Pay(GalaSDKBaseFunction.GalaSDKType.Channel, GetPayInfo(product)); });
    }

    private Dictionary<string, string> GetPayInfo(ProductVo product)
    {
        PlayerVo playerVo = GlobalData.PlayerModel.PlayerVo;

        Dictionary<string, string> payInfo = new Dictionary<string, string>();
        payInfo.Add("productId", product?.ProductId);
        payInfo.Add("dataInfo", Payload.GetApplePayLoad(product));

        Debug.Log("Product======>\n" + product);

        return payInfo;
    }

    public void PayMonthCard(PayAgent.PayType payType = PayAgent.PayType.None)
    {
        ProductVo product = GlobalData.PayModel.GetMonthCardProduct();
        CreateOrder(product,
            () => {GalaSDKBaseFunction.Pay(GalaSDKBaseFunction.GalaSDKType.Apple, GetPayInfo(product)); });             
    }

    public void PayGrowthFund(PayAgent.PayType payType = PayAgent.PayType.None)
    {
        ProductVo product = GlobalData.PayModel.GetGrowthCapitalProduct();      
        CreateOrder(product,
            () => {  GalaSDKBaseFunction.Pay(GalaSDKBaseFunction.GalaSDKType.Apple, GetPayInfo(product)); });
       
    }

    public void PayGift(ProductVo product, PayAgent.PayType payType = PayAgent.PayType.None)
    {
        CreateOrder(product,
            () => { GalaSDKBaseFunction.Pay(GalaSDKBaseFunction.GalaSDKType.Apple, GetPayInfo(product)); });      
    }


    /// <summary>
    /// 创建订单
    /// </summary>
    /// <param name="product"></param>
    /// <param name="succCallBack"></param>
    private void CreateOrder(ProductVo product,Action succCallBack)
    {
        LoadingOverlay.Instance.Show();
        
        CreateOrderReq req =new CreateOrderReq
        {
            Origin = PayAgent.PayChannel,
            Channel = PayAgent.PayChannel,
            CommodityId = product.CommodityId,
            Type = product.ProductType,
            PayType = -1,
            Environment = 0,
            SdkVersion = "100",
            Version = "100",
            Tag = AppConfig.Instance.payKey,
            IsClose =int.Parse(AppConfig.Instance.SwitchControl.CheckAdultPay),
            TriggerGiftId = new TriggerGiftIdPb()
            {
                Id = product.ExtInt
            },
                
        };
        byte[] buffer = NetWorkManager.GetByteData(req);
        NetWorkManager.Instance.Send<CreateOrderRes>(CMD.RECHARGEC_CREATEORDER, buffer, (res =>
        {    
            LoadingOverlay.Instance.Hide();
            Debug.LogError("IOS创建订单成功回调--->"+"响应码："+res.Ret+";充值总金额："+res.TotalRecharge);
            if (res.Ret==ErrorCode.SERVER_TOURIST_NOT_RECHARGE || 
                res.Ret == ErrorCode.SERVER_RECHARGE_UPPERLIMIT ||
                res.Ret ==ErrorCode.SERVER_NOT_OPPEN_RECHARGE)
            {
                ShowWallowPayView(res.Ret, res.TotalRecharge);
                return;
            }
            
            succCallBack?.Invoke();
            
        }),OnGetOrderError);
    }
  
    /// <summary>
    ///显示支付限制View
    /// </summary>
    /// <param name="code">后端响应码</param>
    /// <param name="money">充值金额</param>
    private void ShowWallowPayView(int code ,long money)
    {
        var platformCode = 0;   //平台错误码
        switch (code)
        {
            case ErrorCode.SERVER_TOURIST_NOT_RECHARGE:
                platformCode = ErrorCode.PLATFORMCODE_TOURIST_NOT_RECHARGE;
                break;
            case ErrorCode.SERVER_RECHARGE_UPPERLIMIT:
                platformCode = ErrorCode.PLATFORMCODE_RECHARGE_UPPERLIMIT;
                break;
            case ErrorCode.SERVER_NOT_OPPEN_RECHARGE:
                platformCode =ErrorCode.PLATFORMCODE_NOT_OPPEN_RECHARGE;
                break;
        }
        Debug.LogError("调平台充值提示窗口1");
        GalaAccountManager.Instance.ShowWallowPayView(platformCode, money);    
        Debug.LogError("调平台充值提示窗口2");
    }
    
    private void OnGetOrderError(HttpErrorVo vo)
    {        
        Debug.LogError("IOS创建订单失败回调---》"+vo.ErrorCode);
        LoadingOverlay.Instance.Hide();
        FlowText.ShowMessage(I18NManager.Get("Shop_Hint2", vo.ErrorCode)); // ("创建订单失败" + "->" + vo.ErrorCode);       
    }
    
    public void CheckPayWhenLogin()
    {
        Debug.Log("======CheckPayWhenLogin========");
        GalaSDKBaseFunction.CheckPayWhenLogin();
    }
}