using System.Collections.Generic;
using Com.Proto;
using DataModel;
using GalaSDKBase;
using UnityEngine;

namespace Assets.Scripts.Module.Sdk
{
    public class StatisticsAgent
    {
        public void OnLogin()
        {
            PlayerVo vo = GlobalData.PlayerModel.PlayerVo;
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("roleId", vo.UserId.ToString());
            dict.Add("roleName", vo.UserName);
            dict.Add("zoneId", AppConfig.Instance.serverId);
            dict.Add("zoneName", AppConfig.Instance.serverName);

            GalaSDKBaseFunction.Trace(GalaSDKBaseFunction.GalaTraceSDKType.ALL,
                GalaSDKBaseFunction.GalaTraceType.OnLogin, dict);
            
            BuglyAgent.SetUserId(vo.UserId +"_"+ GlobalData.PlayerModel.PlayerVo.UserName);
        }

        public void OnRegister()
        {
            PlayerVo vo = GlobalData.PlayerModel.PlayerVo;
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("roleId", vo.UserId.ToString());
            dict.Add("roleName", vo.UserName);
            dict.Add("zoneId", AppConfig.Instance.serverId);
            dict.Add("zoneName", AppConfig.Instance.serverName);
                    
            GalaSDKBaseFunction.Trace(GalaSDKBaseFunction.GalaTraceSDKType.ALL,
                GalaSDKBaseFunction.GalaTraceType.OnRegister, dict);
        }

        public void SetLevel(int level)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("level", level.ToString());

            GalaSDKBaseFunction.Trace(GalaSDKBaseFunction.GalaTraceSDKType.ALL,
                GalaSDKBaseFunction.GalaTraceType.OnSetLevel, dict);
        }

        /// <summary>
        /// 开始支付
        /// </summary>
        /// <param name="orderId">计费点</param>
        /// <param name="iapId">支付内容</param>
        /// <param name="amount">价格（RMB）</param>
        /// <param name="payType">支付类型</param>
        public void OnStartPay(string orderId, string payType, ProductVo product)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("orderID", orderId);
            dict.Add("iapID", product.CommodityId.ToString());
            dict.Add("productName", product.Name);
            dict.Add("currencyAmount", (product.AmountRmb * 100).ToString());
            dict.Add("currencyType", "CNY");
            dict.Add("virtualCurrencyAmount", (product.AmountRmb * 10).ToString());
            dict.Add("paymentType", payType);

            GalaSDKBaseFunction.Trace(GalaSDKBaseFunction.GalaTraceSDKType.ALL,
                GalaSDKBaseFunction.GalaTraceType.OnStartPay, dict);
        }

        public void OnPayEnd(string orderId, string payType, ProductVo product)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("orderID", orderId);
            dict.Add("iapID", product.CommodityId.ToString());
            dict.Add("productName", product.Name);
            dict.Add("currencyAmount", (product.AmountRmb * 100).ToString());
            dict.Add("currencyType", "CNY");
            dict.Add("virtualCurrencyAmount", (product.AmountRmb * 10).ToString());
            dict.Add("paymentType", payType);

            GalaSDKBaseFunction.Trace(GalaSDKBaseFunction.GalaTraceSDKType.ALL,
                GalaSDKBaseFunction.GalaTraceType.OnPay, dict);
        }

        public void IOSPay(CheckOrderRes res, ProductVo vo)
        {
            if (res.Amount <= 0 || string.IsNullOrEmpty(res.OrderId))
            {
                BuglyAgent.ReportException("iOS_Pay", "Amount:" + res.Amount + "  OrderId:" + res.OrderId, "none");
            }
            OnStartPay(res.OrderId, PayAgent.PayChannel, vo);

            ClientTimer.Instance.DelayCall(() =>
            {
                OnPayEnd(res.OrderId, PayAgent.PayChannel, vo);
            }, Random.Range(5f, 10.0f));
        }

        /// <summary>
        /// 消耗虚拟币
        /// </summary>
        /// <param name="name">消费点编号（道具名、服务名）</param>
        /// <param name="num">购买数量</param>
        /// <param name="price">单价</param>
        public void OnPurchase(string name, int num, float price = 1)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("item", name);
            dict.Add("num", num.ToString());
            dict.Add("priceInVirtualCurrency", price.ToString());

            GalaSDKBaseFunction.Trace(GalaSDKBaseFunction.GalaTraceSDKType.ALL,
                GalaSDKBaseFunction.GalaTraceType.OnPurchase, dict);
        }

        /// <summary>
        /// 获得虚拟币
        /// </summary>
        /// <param name="num">数量</param>
        /// <param name="reason">赠与原因/类型(格式：32 个字符内的中文、空格、英文、数字。不要带有任何开发中的转义字符，如斜杠。)”</param>
        public void OnReward(int num, string reason)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("num", num.ToString());
            dict.Add("reason", reason);

            GalaSDKBaseFunction.Trace(GalaSDKBaseFunction.GalaTraceSDKType.ALL,
                GalaSDKBaseFunction.GalaTraceType.OnReward, dict);
        }


        /// <summary>
        /// 消耗道具
        /// </summary>
        /// <param name="name">消费点编号（道具名、服务名）</param>
        /// <param name="num">消耗数量</param>
        public void OnUse(string name, int num)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("item", name);
            dict.Add("num", num.ToString());

            GalaSDKBaseFunction.Trace(GalaSDKBaseFunction.GalaTraceSDKType.ALL,
                GalaSDKBaseFunction.GalaTraceType.OnUse, dict);
        }
        
        /// <summary>
        /// 开始任务
        /// </summary>
        /// <param name="name">任务编号</param>
        public void OnMissionBegin(string name)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("missionId", name);

            GalaSDKBaseFunction.Trace(GalaSDKBaseFunction.GalaTraceSDKType.ALL,
                GalaSDKBaseFunction.GalaTraceType.OnMissionBegin, dict);
        }
        
        /// <summary>
        /// 完成任务
        /// </summary>
        /// <param name="name">任务编号</param>
        public void OnMissionCompleted(string name)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("missionId", name);

            GalaSDKBaseFunction.Trace(GalaSDKBaseFunction.GalaTraceSDKType.ALL,
                GalaSDKBaseFunction.GalaTraceType.OnMissionCompleted, dict);
        }
        
        /// <summary>
        /// 任务失败
        /// </summary>
        /// <param name="name">任务编号</param>
        /// <param name="reason">失败原因</param>
        public void OnMissionFail(string name, string reason = "")
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("missionId", name);
            dict.Add("reason", reason);

            GalaSDKBaseFunction.Trace(GalaSDKBaseFunction.GalaTraceSDKType.ALL,
                GalaSDKBaseFunction.GalaTraceType.OnMissionFail, dict);
        }
        
        /// <summary>
        /// 事件
        /// </summary>
        /// <param name="eventId">事件ID</param>
        /// <param name="count">事件次数</param>
        public void OnEvent(string eventId, int count = 1)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("eventID", eventId);
            dict.Add("count", count.ToString());

            GalaSDKBaseFunction.Trace(GalaSDKBaseFunction.GalaTraceSDKType.ALL,
                GalaSDKBaseFunction.GalaTraceType.OnEvent, dict);
        }

    }
}