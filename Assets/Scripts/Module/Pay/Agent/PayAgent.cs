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
using game.main;
using GalaSDKBase;
using QFramework;
using UnityEngine;
using Utils;

public class PayAgent : IPayAgent
{
    public enum PayType
    {
        NoDefined = 999999,
        None = 0,
        AliPay = 1004,
        WechatPay = 1009,
        MyCard = 1010,
        GooglePlay = 888,
        ApplePay = 889
    }

    public static string PayChannel;

    protected ProductVo _product;
    private PayType payType;

    private PayChecker payChecker;

    public static string GetPaySdk(PayType payType)
    {
        switch (payType)
        {
            case PayType.NoDefined:
                break;
            case PayType.None:
                return AppConfig.Instance.payChannel;
            case PayType.AliPay:
                return "ALIPAY";
            case PayType.WechatPay:
                return "WX";
            case PayType.MyCard:
                break;
            case PayType.GooglePlay:
                break;
            case PayType.ApplePay:
                break;
        }

        return AppConfig.Instance.payChannel;
    }

    public PayAgent()
    {
        payChecker = new PayChecker();

        PayChannel = AppConfig.Instance.payChannel;

        GalaSDKBaseCallBack.Instance.GALASDKPaySuccessEvent += OnPaySuccess;
        GalaSDKBaseCallBack.Instance.GALASDKPayFailEvent += OnPayFail;
    }

    public void CheckPayWhenLogin()
    {
        payChecker.CheckPayWhenLogin();
    }

    public void InitPay()
    {
    }

    private bool ShowAlipayWechatChooser(ProductVo productVo, PayType payType)
    {
        if (payType == PayType.None && Channel.CheckShowPayChooser())
        {
            IconSelectWindow win = PopupManager.ShowWindow<IconSelectWindow>(Constants.IconSelectWindowPath);
            win.SetData("",IconType.Alipay, IconType.WeChatFriend);
            win.clickCallback = (m) =>
            {
                if (m == IconType.Alipay)
                {
                    this.payType = PayType.AliPay;
                }
                else if (m == IconType.WeChatFriend)
                {
                    this.payType = PayType.WechatPay;
                }

                CreateOrder(productVo);
                win.Close();
            };
            return true;
        }

        return false;
    }

    public void Pay(ProductVo product, PayType payType = PayType.None)
    {
        if (ShowAlipayWechatChooser(product,payType))
            return;

        this.payType = payType;
        CreateOrder(product);
    }

    public void PayMonthCard(PayType payType = PayType.None)
    {
        ProductVo product = GlobalData.PayModel.GetMonthCardProduct();
        if (ShowAlipayWechatChooser(product,payType))
            return;

        this.payType = payType;
        CreateOrder(product);
    }

    public void PayGrowthFund(PayType payType = PayType.None)
    {
        ProductVo product = GlobalData.PayModel.GetGrowthCapitalProduct();
        if (ShowAlipayWechatChooser(product,payType))
            return;

        this.payType = payType;
        CreateOrder(product);
    }

    public void PayGift(ProductVo product, PayType payType = PayType.None)
    {
        if (product.ProductType != CommodityTypePB.Gift)
        {
            FlowText.ShowMessage("商品类型不是礼包！");
            return;
        }

        if (ShowAlipayWechatChooser(product,payType))
            return;

        this.payType = payType;
        CreateOrder(product);
    }

    private void OnPayFail(string data)
    {
        Debug.LogError("SDK_支付失败===>" + data);
        if(Channel.IsTencent)
            return;
        
        if (string.IsNullOrEmpty(data))
        {
            BuglyAgent.ReportException("PayAgent.OnPayFail","SDK_支付失败", "");
            return;
        }

        OnPaySuccess(data);
    }

    protected void OnPaySuccess(string data)
    {
        Debug.LogWarning("===SDK===OnPaySuccess=======" + data);

        CheckOrderReqs reqList = new CheckOrderReqs();

        CheckOrderReq req = new CheckOrderReq()
        {
            Channel = GetPaySdk(payType),
            Origin = PayChannel,
            Environment = 0,
            Params = "",
            OrderId = data,
            PayType = (int) payType,
            Tag = AppConfig.Instance.payKey,
            SdkVersion = PayVersion.GetPayVersion(payType),
            Version = PayVersion.GetPaySdkVersion(payType),
            TriggerGiftId = new TriggerGiftIdPb()
            {
                Id = _product.ExtInt
            }
        };

        req.ExtInfo = GalaSDKBaseFunction.SdkFetchTokenWithType("");
        
        reqList.CheckOrderReqs_.Add(req);
        payChecker.AddToCheckList(req.OrderId);

        Debug.LogWarning("===SDK===OnPaySuccess==>" + ObjectDumper.Dump(reqList));

        byte[] bytes = NetWorkManager.GetByteData(reqList);

        NetWorkManager.Instance.Send<CheckOrderRess>(CMD.RECHARGEC_CHECKORDER, bytes, OnCheckOrdersSuccess,
            OnCheckOrdersFail);
    }

    public void OnCheckOrdersFail(HttpErrorVo errorVo)
    {
        Debug.LogError("OnCheckOrdersFail===>" + errorVo.ToString());
    }

    public virtual void OnCheckOrdersSuccess(CheckOrderRess resList)
    {
        Debug.Log("OnCheckOrdersSuccess==>ObjectDumper Length:" + resList.CheckOrderRess_.Count + "\n" +
                  ObjectDumper.Dump(resList));

        PayHelper.SetGlobal(resList);
        
        List<string> list = new List<string>();
        foreach (var res in resList.CheckOrderRess_)
        {
            list.Add(res.OrderId);
            
            JSONObject json = new JSONObject(res.Extra);
            Debug.Log("OnCheckOrdersSuccess==>res.Extra " + res.Extra);

            ProductVo vo = GlobalData.PayModel.GetProduct(res.CommodityId);
            SdkHelper.StatisticsAgent.OnPayEnd(res.OrderId, PayChannel, vo);

            if (res.Amount <= 0 || string.IsNullOrEmpty(res.OrderId))
            {
                BuglyAgent.ReportException("PayAgent", "Amount:" + res.Amount + "  OrderId:" + res.OrderId, "none");
            }

            payChecker.RemoveOrder(res.OrderId);
        }

        if (resList.Awards?.Count > 0)
        {
            RewardUtil.AddReward(resList.Awards);
            AwardWindow awardWindow = PopupManager.ShowWindow<AwardWindow>("GameMain/Prefabs/AwardWindow/AwardWindow");
            awardWindow.SetData(resList.Awards,I18NManager.Get("Common_RechargeSuccess"));
        }

        Debug.Log("GlobalData.PlayerModel.PlayerVo.ExtInfo" + resList.UserExtraInfo);
       
        EventDispatcher.TriggerEvent(EventConst.GetPayInfoSuccess, resList.UserBuyRmbMall);

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

    /// <summary>
    /// 创建支付订单
    /// </summary>
    protected virtual void CreateOrder(ProductVo product)
    {
        LoadingOverlay.Instance.Show();
        
        _product = product;
        string param = GalaSDKBaseFunction.PayCreateOrderParameter();
        Debug.LogError("CreateOrder==>" + product.ToString() + "\n param:" + param);

        JSONObject extJson = new JSONObject();
        extJson.AddField("productName", _product.Name);
        extJson.AddField("productDesc", _product.Name);

        if (!param.IsNullOrEmpty() && !param.Equals("false"))
        {
            JSONObject paramJson = new JSONObject(param);
            if (paramJson != null && paramJson.keys != null)
            {
                for (int i = 0; i < paramJson.keys.Count; i++)
                {
                    string key = paramJson.keys[i];
                    extJson.AddField(key, paramJson[key].str);
                }
            }
        }

        CreateOrderReq req = new CreateOrderReq
        {
            Channel = GetPaySdk(payType),
            Origin = PayChannel,
            CommodityId = product.CommodityId,
            Type = product.ProductType,
            Version = PayVersion.GetPayVersion(payType),
            SdkVersion = PayVersion.GetPaySdkVersion(payType),
            Environment = 0, //TODO 0沙箱，1正式
            PayType = (int) payType,
            Tag = AppConfig.Instance.payKey,
            IsClose =int.Parse(AppConfig.Instance.SwitchControl.CheckAdultPay),
            TriggerGiftId = new TriggerGiftIdPb()
            {
                Id = _product.ExtInt
            },
            Extra = extJson.ToString()
        };

        byte[] buffer = NetWorkManager.GetByteData(req);

        LoadingOverlay.Instance.Show();
       NetWorkManager.Instance.Send<CreateOrderRes>(CMD.RECHARGEC_CREATEORDER, buffer, OnGetOrderSuccess, OnGetOrderError);
      
    }

    protected virtual void OnGetOrderSuccess(CreateOrderRes res)
    {
        LoadingOverlay.Instance.Hide();

        Debug.LogError("Android创建订单成功回调--->"+"响应码："+res.Ret+";充值总金额："+res.TotalRecharge);
        if (res.Ret==ErrorCode.SERVER_TOURIST_NOT_RECHARGE || 
            res.Ret == ErrorCode.SERVER_RECHARGE_UPPERLIMIT ||
            res.Ret ==ErrorCode.SERVER_NOT_OPPEN_RECHARGE)
        {             
            ShowWallowPayView(res.Ret, res.TotalRecharge);
            return;
        }
          
        _product.OrderId = res.OrderId;

        payChecker.AddOrder(_product, payType);

        Debug.LogError("OnGetOrderSuccess===>OrderId:" + res.OrderId + " ExtraInfo:" + res.ExtraInfo);

        Dictionary<string, string> payinfo = new Dictionary<string, string>();
        payinfo.Add("orderId", _product.OrderId);
        payinfo.Add("paySandbox", AppConfig.Instance.paySandbox ? "true" : "false");
//        payinfo.Add("price", _product.Price + "");
        payinfo.Add("currencyAmount", _product.Price*100 + "");
        payinfo.Add("productName", _product.Name);
        payinfo.Add("productDesc", _product.Name);
        payinfo.Add("productId", _product.ProductId);
        
        foreach (var data in res.ExtraInfo)
        {
            payinfo.Add(data.Key, data.Value);
        }

        GalaSDKBaseFunction.Pay(PayVersion.GetGalaSdkPayType(payType), payinfo);

        SdkHelper.StatisticsAgent.OnStartPay(res.OrderId, PayChannel, _product);
    }

    private void OnGetOrderError(HttpErrorVo vo)
    {
        Debug.LogError("Android创建订单失败回调---》"+vo.ErrorCode);
        LoadingOverlay.Instance.Hide();
        FlowText.ShowMessage(I18NManager.Get("Shop_Hint2", vo.ErrorCode)); // ("创建订单失败" + "->" + vo.ErrorCode);
       
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
}