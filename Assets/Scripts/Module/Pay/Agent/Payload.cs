using System;
using System.Globalization;
using DataModel;
using UnityEngine;

namespace Assets.Scripts.Module.Pay.Agent
{
    public class Payload
    {
        /// <summary>
        /// ServerId UserId CommodityId
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public static string GetApplePayLoad(ProductVo product)
        {
            PlayerVo playerVo = GlobalData.PlayerModel.PlayerVo;
            string str = AppConfig.Instance.serverId + "@_" + playerVo.UserId + "_" + product.CommodityId;
            if (product.ExtInt != 0)
            {
                str += "&" + product.ExtInt;
            }
            return str;
        }
        
        public static string GetAppleCommodityIdByPayLoad(string payload)
        {
            string str = payload.Substring(payload.IndexOf("@_", StringComparison.Ordinal) + 1);

            Debug.Log("commodityId1===>" + str);
            
            string[] arr1 = str.Split('_');
            string commodityId = arr1[arr1.Length - 1];

            int index = commodityId.IndexOf("&", StringComparison.Ordinal);
            if (index != -1)
            {
                commodityId = commodityId.Substring(0, index);
            }
            
            Debug.Log("commodityId2===>" + commodityId);

            return commodityId;
        }
        
        public static long GetAppleExtIntByPayLoad(string payload)
        {
            int index = payload.IndexOf("&", StringComparison.Ordinal);
            if (index == -1)
                return 0;
            
            string str = payload.Substring(index + 1);
            Debug.Log("GetAppleExtIntByPayLoad===>" + str);

            long result;
            if (long.TryParse(str, out result))
            {
                return result;
            }

            return 0;
        }
        
        
        
        public static string GetGooglePlayPayLoad(ProductVo product)
        {
            PlayerVo playerVo = GlobalData.PlayerModel.PlayerVo;

            string str = AppConfig.Instance.serverId + "&" + playerVo.UserId + "&" + product.CommodityId + "&" +
                         product.OrderId;
            if (product.ExtInt != 0)
            {
                str += "&" + product.ExtInt;
            }
            return str;
        }

        public static bool IsCurrentUser(string payload)
        {
            PlayerVo playerVo = GlobalData.PlayerModel.PlayerVo;
            return payload.StartsWith(AppConfig.Instance.serverId + "&" + playerVo.UserId);
        }

        public static string GetGooglePlayOrderId(string payload)
        {
            var strings = payload.Split('&');
            return strings[3];
        }
        
        public static long GetGooglePlayExtInt(string payload)
        {
            var strings = payload.Split('&');
            if (strings.Length < 5)
                return 0;
            
            long result;
            if (long.TryParse(strings[4], out result))
            {
                return result;
            }
            return 0;
        }
        
    }
}