using System;
using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module.NetWork;
using Assets.Scripts.Module.Pay.Agent;
using Com.Proto;
using Common;
using DataModel;
using Framework.GalaSports.Service;
using Framework.Utils;
using UnityEngine;

public class PayChecker
{
    private readonly string dataPath = Application.persistentDataPath + "/order";
    private string dataName;

    private readonly int[] CHECK_INTERVAL = {10, 10, 15, 30, 60, 300, 600, 1200, 2400};
    
    public int leftTime;
    
    public int timeIndex;

    private Dictionary<string, OrderData> _orderDict;

    private float timeInterval = 2.0f;
    private bool _isFinish = true;
    private bool _hasInit;
    private const string CountDownName = "paychecker";

    private void OnTimeTick(int time)
    {
        float delay = 0.0F;

//        Debug.Log("orderDict len->" + _orderDict.Count);

        leftTime -= (int) timeInterval;
        if (_orderDict.Count > 0)
        {
            if (leftTime <= 0)
            {
                CheckPayList();

                timeIndex++;

                if (timeIndex < CHECK_INTERVAL.Length)
                {
                    leftTime = CHECK_INTERVAL[timeIndex];
                }
                else
                {
                    _isFinish = true;
                    ClientTimer.Instance.RemoveCountDown(CountDownName);
                }

                Debug.LogWarning("OnTimeTick->" + " ---- leftTime->" + leftTime);

            }
        }
        else
        {
            timeIndex = CHECK_INTERVAL.Length;
            _isFinish = true;
        }
    }

    private void CheckPayList()
    {
        CheckOrderReqs checkOrderReqs = new CheckOrderReqs();
        
        foreach (KeyValuePair<string,OrderData> orderData in _orderDict)
        {
            Debug.LogWarning("检查订单="+orderData.Value.orderId);

            PayAgent.PayType payType = orderData.Value.payType;
            CheckOrderReq req = new CheckOrderReq()
            {
                Channel = PayAgent.GetPaySdk(payType),
                Origin = PayAgent.PayChannel,
                Environment = 0,
                Params = "",
                OrderId = orderData.Value.orderId,
                PayType = (int) payType,
                Tag = AppConfig.Instance.payKey,
                SdkVersion = PayVersion.GetPayVersion(payType),
                Version = PayVersion.GetPaySdkVersion(payType),
                TriggerGiftId = new TriggerGiftIdPb()
                {
                    Id = orderData.Value.productVo.ExtInt
                }
            };
            
            if (orderData.Value.payType == PayAgent.PayType.WechatPay)
            {
                req.Channel = "WX";
            }
            else if (orderData.Value.payType == PayAgent.PayType.AliPay)
            {
                req.Channel = "ALIPAY";
            }
            
            checkOrderReqs.CheckOrderReqs_.Add(req);
        }

        byte[] buff = NetWorkManager.GetByteData(checkOrderReqs);
        NetWorkManager.Instance.Send<CheckOrderRess>(CMD.RECHARGEC_CHECKORDER, buff, OnCheckPayListSuccess, OnCheckPayListFail);
    }

    private void OnCheckPayListFail(HttpErrorVo vo)
    {
        if (SdkHelper.PayAgent is PayAgent)
        {
            PayAgent payAgent = SdkHelper.PayAgent as PayAgent;
            payAgent.OnCheckOrdersFail(vo);
        }
        if (SdkHelper.PayAgent is PayAgentMyIOS)
        {
            PayAgentMyIOS payAgent = SdkHelper.PayAgent as PayAgentMyIOS;
            payAgent.OnCheckOrdersFail(vo);
        }
    }

    private void OnCheckPayListSuccess(CheckOrderRess res)
    {
        if (SdkHelper.PayAgent is PayAgent)
        {
            PayAgent payAgent = SdkHelper.PayAgent as PayAgent;
            payAgent.OnCheckOrdersSuccess(res);
        }
        if (SdkHelper.PayAgent is PayAgentMyIOS)
        {
            PayAgentMyIOS payAgent = SdkHelper.PayAgent as PayAgentMyIOS;
            payAgent.OnCheckOrdersSuccess(res);
        }
    }

    public void AddOrder(ProductVo product, PayAgent.PayType payType)
    {
        if(IgnoreCheck)
            return;
        
        OrderData od = new OrderData()
        {
            orderId = product.OrderId,
            serverId = AppConfig.Instance.serverId,
            userId = GlobalData.PlayerModel.PlayerVo.UserId + "",
            payType = payType,
            CreatOrderTime = DateTime.Now,
            productVo = product
        };

        _orderDict.Add(product.OrderId, od);

        SaveData();
    }

    public bool IgnoreCheck
    {
        get { return Channel.IsTencent; }
    }

    public void RemoveOrder(string orderId)
    {
        Debug.LogWarning("RemoveOrder->"+orderId);

        if (_orderDict != null && _orderDict.ContainsKey(orderId))
        {
            _orderDict.Remove(orderId);
            SaveData();
        }
    }
    
    public void AddToCheckList(string orderId)
    {
        if(IgnoreCheck)
            return;
        
        OrderData od = null;
        if(_orderDict.TryGetValue(orderId, out od) == false)
        {
            Debug.LogError("orderDict no orderId->"+orderId);
            return;
        }

        timeIndex = 0;
        leftTime = CHECK_INTERVAL[timeIndex];
        if(_isFinish)
            ClientTimer.Instance.AddCountDown(CountDownName, long.MaxValue, timeInterval, OnTimeTick, null);
    }

    public void CheckPayWhenLogin()
    {
        if(IgnoreCheck)
            return;
        
        Init();
        if (_orderDict.Count > 0)
        {
            Debug.Log("==批量验单=="+_orderDict.Count);
            foreach (KeyValuePair<string,OrderData> orderData in _orderDict)
            {
//                ULog.I("==订单号=="+orderData.Value.+"\n");
            }

            _isFinish = false;
            ClientTimer.Instance.AddCountDown(CountDownName, long.MaxValue, 2f, OnTimeTick, null);
        }
    }

    private void Init()
    {
        _hasInit = true;
        ClientTimer.Instance.RemoveCountDown(CountDownName);
        string userId = GlobalData.PlayerModel.PlayerVo.AccountId;
        string serverId = AppConfig.Instance.serverId;
        dataName = serverId + "_" + userId;

        byte[] bytes = FileUtil.GetBytesFile(dataPath, dataName);
        if (bytes != null)
        {
            _orderDict = (Dictionary<string, OrderData>) FileUtil.Bytes2Object(bytes);
        }
        else
        {
            _orderDict = new Dictionary<string, OrderData>();
        }
        
    }

    private void SaveData()
    {
        byte[] bytes = FileUtil.Object2Bytes(_orderDict);
        FileUtil.SaveBytesFile(dataPath, dataName, bytes);
    }

    
}