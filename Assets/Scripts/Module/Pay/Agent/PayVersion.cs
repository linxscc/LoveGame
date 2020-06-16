using System;
using GalaSDKBase;

namespace Assets.Scripts.Module.Pay.Agent
{
    /// <summary>
    /// 支付宝SDK版本：101，版本：100
    /// 微信SDK版本：100，版本100
    /// </summary>
    public class PayVersion
    {
        public static string GetPayVersion(PayAgent.PayType payType)
        {
            switch (payType)
            {
                case PayAgent.PayType.NoDefined:
                    break;
                case PayAgent.PayType.None:
                    return "100";
                case PayAgent.PayType.AliPay:
                    return "100";
                case PayAgent.PayType.WechatPay:
                    return "100";
                case PayAgent.PayType.MyCard:
                    break;
                case PayAgent.PayType.GooglePlay:
                    break;
                case PayAgent.PayType.ApplePay:
                    break;
            }

            return "100";
        }
        
        public static string GetPaySdkVersion(PayAgent.PayType payType)
        {
            switch (payType)
            {
                case PayAgent.PayType.NoDefined:
                    break;
                case PayAgent.PayType.None:
                    return "100";
                case PayAgent.PayType.AliPay:
                    return "101";
                case PayAgent.PayType.WechatPay:
                    return "100";
                case PayAgent.PayType.MyCard:
                    break;
                case PayAgent.PayType.GooglePlay:
                    break;
                case PayAgent.PayType.ApplePay:
                    break;
            }

            return "100";
        }

        public static GalaSDKBaseFunction.GalaSDKType GetGalaSdkPayType(PayAgent.PayType payType)
        {
            switch (payType)
            {
                case PayAgent.PayType.NoDefined:
                    break;
                case PayAgent.PayType.None:
                    break;
                case PayAgent.PayType.AliPay:
                    return GalaSDKBaseFunction.GalaSDKType.Alipay;
                case PayAgent.PayType.WechatPay:
                    return GalaSDKBaseFunction.GalaSDKType.WeChat;
                case PayAgent.PayType.MyCard:
                    break;
                case PayAgent.PayType.GooglePlay:
                    return GalaSDKBaseFunction.GalaSDKType.Google;
                case PayAgent.PayType.ApplePay:
                    return GalaSDKBaseFunction.GalaSDKType.Apple;  
            }

            return GalaSDKBaseFunction.GalaSDKType.Gala;
        }
    }
}