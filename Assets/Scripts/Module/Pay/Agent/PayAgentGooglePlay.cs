using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core.Events;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module.NetWork;
using Assets.Scripts.Module.Pay.Agent;
using Com.Proto;
using Common;
using Componets;
using DataModel;
using Framework.GalaSports.Service;
using game.main;
using GalaSDKBase;
using UnityEngine;
using Utils;

public class PayAgentGooglePlay : IPayAgent
{
    private PayAgent.PayType payType;

    private ProductVo _product;

    public PayAgentGooglePlay()
    {
        PayAgent.PayChannel = "GOOGLE";

        GalaSDKBaseCallBack.Instance.GALASDKPayInitSuccessEvent += OnPayInit;
        GalaSDKBaseCallBack.Instance.GALASDKPaySuccessEvent += OnPaySuccess;
        GalaSDKBaseCallBack.Instance.GALASDKPayFailEvent += OnPayFail;
    }

    private void OnPayInit(string json)
    {
        Debug.LogError("SDK_OnPayInit===>" + json);

        CheckPayWhenLogin();

        JSONObject jsonObject = new JSONObject(json);
        GlobalData.PayModel.SetAreaPrice(jsonObject);
    }

    public void InitPay()
    {
        string[] productIds = GlobalData.PayModel.GetProductIds();
        GalaSDKBaseFunction.InitPay(GalaSDKBaseFunction.GalaSDKType.Google, productIds);
    }

    private void OnPayFail(string data)
    {
        Debug.LogError("SDK_支付失败===>" + data);
    }

    private void OnPaySuccess(string data)
    {
        Debug.LogWarning("===SDK===OnPaySuccess=======" + data);

        CheckOrderReqs reqList = new CheckOrderReqs();

        JSONObject jsonObject = new JSONObject(data);

        List<JSONObject> list = jsonObject.list;

        foreach (var obj in list)
        {
            string str = obj["signedData"].str;
            Debug.LogError("signedData====>" + str);
            JSONObject json = new JSONObject(str.Replace("\\", ""));

            string payload = json["developerPayload"].str;
            if (Payload.IsCurrentUser(payload) == false)
            {
                Debug.Log("===SDK===OnPaySuccess===== IsCurrentUser false ===== " + payload);
                continue;
            }

            string orderId = Payload.GetGooglePlayOrderId(payload);
            long extInt = Payload.GetGooglePlayExtInt(payload);

            Debug.LogWarning("===SDK===OnPaySuccess orderId===>" + orderId);

            JSONObject paramsJson = new JSONObject();
            paramsJson.AddField("signedData", obj["signedData"].str);
            paramsJson.AddField("signature", obj["signature"].str);

            Debug.LogError("paramsJson==>" + paramsJson.ToString());

            CheckOrderReq req = new CheckOrderReq()
            {
                Channel = PayAgent.PayChannel,
                Environment = 0,
                ExtInfo = "",
                Origin = PayAgent.PayChannel,
                Params = paramsJson.ToString(),
                OrderId = orderId,
                PayType = (int) PayAgent.PayType.GooglePlay,
                Tag = AppConfig.Instance.payKey,
                SdkVersion = "100",
                Version = "100",
                TriggerGiftId = new TriggerGiftIdPb()
                {
                    Id = extInt
                }
            };
            reqList.CheckOrderReqs_.Add(req);
            Debug.LogWarning("===Pay CheckOrderReq==>" + ObjectDumper.Dump(req));
        }

        Debug.LogWarning("===SDK===OnPaySuccess==>" + ObjectDumper.Dump(reqList));

        byte[] bytes = NetWorkManager.GetByteData(reqList);

        NetWorkManager.Instance.Send<CheckOrderRess>(CMD.RECHARGEC_CHECKORDER, bytes, OnCheckOrdersSuccess,
            OnCheckOrdersFail);
    }

    private void OnCheckOrdersFail(HttpErrorVo errorVo)
    {
        Debug.LogError("OnCheckOrdersFail===>" + errorVo.ToString());
        ShowWallowPayView(errorVo);
    }

    private void ShowWallowPayView(HttpErrorVo vo)
    {
        var  code = vo.ErrorCode;
        float money = (float) vo.CustomData;
        switch (code)
        {
            case 100013:
            case 100014:
            case 100015:
                GalaAccountManager.Instance.ShowWallowPayView(code, money);  
                break;
        }
    }
    
    private void OnCheckOrdersSuccess(CheckOrderRess resList)
    {
        Debug.Log("OnCheckOrdersSuccess==>ObjectDumper Length:" + resList.CheckOrderRess_.Count + "\n" +
                  ObjectDumper.Dump(resList));

        PayHelper.SetGlobal(resList);
        
        List<string> list = new List<string>();
        foreach (var res in resList.CheckOrderRess_)
        {
            JSONObject json = new JSONObject(res.Extra);
            Debug.Log("OnCheckOrdersSuccess==>res.Extra " + res.Extra);

            string data = json["signedData"].str;
            data = data.Replace("\\", "");

            JSONObject signedData = new JSONObject(data);

            JSONObject purchaseToken = signedData["purchaseToken"];

            Debug.Log("OnCheckOrdersSuccess==>purchaseToken " + purchaseToken.str);

            list.Add(purchaseToken.str);

            ProductVo vo = GlobalData.PayModel.GetProduct(res.CommodityId);
            SdkHelper.StatisticsAgent.OnPayEnd(res.OrderId, PayAgent.PayChannel, vo);
            
            if (res.Amount <= 0 || string.IsNullOrEmpty(res.OrderId))
            {
                BuglyAgent.ReportException("GooglePlay_Pay", "Amount:" + res.Amount + "  OrderId:" + res.OrderId, "none");
            }
        }

        GalaSDKBaseFunction.EndPay(GalaSDKBaseFunction.GalaSDKType.Google, list.ToArray());

        if (resList.Awards?.Count > 0)
        {
            RewardUtil.AddReward(resList.Awards);
            AwardWindow awardWindow = PopupManager.ShowWindow<AwardWindow>("GameMain/Prefabs/AwardWindow/AwardWindow");
            awardWindow.SetData(resList.Awards);
        }
        
        Debug.Log("GlobalData.PlayerModel.PlayerVo.ExtInfo" + resList.UserExtraInfo);
        EventDispatcher.TriggerEvent(EventConst.GetPayInfoSuccess,resList.UserBuyRmbMall);

        if (resList.TriggerId != null && resList.TriggerId.Count > 0)
        {
            Debug.LogWarning("resList.TrigerId-------->" + resList.TriggerId[0]);
            EventDispatcher.TriggerEvent(EventConst.TriggerGiftPaySuccess, resList.TriggerId);
        }
    }

    public void Pay(ProductVo product, PayAgent.PayType payType = PayAgent.PayType.None)
    {
        this.payType = payType;
        CreateOrder(product);
    }

    /// <summary>
    /// 购买月卡
    /// </summary>
    /// <param name="index"></param>
    /// <param name="payType"></param>
    public virtual void PayMonthCard(PayAgent.PayType payType = PayAgent.PayType.None)
    {
        this.payType = payType;
        ProductVo product = GlobalData.PayModel.GetMonthCardProduct();
        CreateOrder(product);
    }

    public void PayGrowthFund(PayAgent.PayType payType = PayAgent.PayType.None)
    {
        this.payType = payType;
        ProductVo product = GlobalData.PayModel.GetGrowthCapitalProduct();
        CreateOrder(product);
    }

    /// <summary>
    /// 购买礼包
    /// </summary>
    /// <param name="product"></param>
    /// <param name="payType"></param>
    public void PayGift(ProductVo product, PayAgent.PayType payType = PayAgent.PayType.None)
    {
        this.payType = payType;

        if (product.ProductType != CommodityTypePB.Gift)
        {
            FlowText.ShowMessage("商品类型不是礼包！");
            return;
        }

        CreateOrder(product);
    }

    /// <summary>
    /// 创建支付订单
    /// </summary>
    protected virtual void CreateOrder(ProductVo product)
    {
        _product = product;

        Debug.LogError("CreateOrder==>" + product.ToString());

        CreateOrderReq req = new CreateOrderReq
        {
            Channel = PayAgent.PayChannel,
            Origin = PayAgent.PayChannel,
            CommodityId = product.CommodityId,
            Type = product.ProductType,
            Version = "100",
            Environment = 0, //TODO 0沙箱，1正式
            PayType = (int) PayAgent.PayType.GooglePlay,
            SdkVersion = "100",
            Tag = AppConfig.Instance.payKey,
            TriggerGiftId = new TriggerGiftIdPb()
            {
                Id = _product.ExtInt
            }
        };

        byte[] buffer = NetWorkManager.GetByteData(req);

        LoadingOverlay.Instance.Show();
        NetWorkManager.Instance.Send<CreateOrderRes>(CMD.RECHARGEC_CREATEORDER, buffer, OnGetOrderSuccess,
            OnGetOrderError);
    }

    private void OnGetOrderSuccess(CreateOrderRes res)
    {
        LoadingOverlay.Instance.Hide();

        _product.OrderId = res.OrderId;

        Debug.LogError("OnGetOrderSuccesso===>OrderId:" + res.OrderId + " ExtraInfo:" + res.ExtraInfo);

        Dictionary<string, string> payinfo = new Dictionary<string, string>();
        payinfo.Add("productId", _product?.ProductId);
        payinfo.Add("dataInfo", Payload.GetGooglePlayPayLoad(_product));

        GalaSDKBaseFunction.Pay(GalaSDKBaseFunction.GalaSDKType.Google, payinfo);

        SdkHelper.StatisticsAgent.OnStartPay(res.OrderId, PayAgent.PayChannel, _product);
    }

    private void OnGetOrderError(HttpErrorVo vo)
    {
        FlowText.ShowMessage(I18NManager.Get("Shop_Hint2", vo.ErrorCode)); // ("创建订单失败" + "->" + vo.ErrorCode);
    }

    public void CheckPayWhenLogin()
    {
        Debug.LogWarning("CheckPayWhenLogin");
        GalaSDKBaseFunction.CheckPayWhenLogin();
    }
}