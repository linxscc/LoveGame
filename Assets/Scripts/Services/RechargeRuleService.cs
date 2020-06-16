using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module.NetWork;
using Com.Proto;
using DataModel;
using UnityEngine;

namespace Assets.Scripts.Services
{
    public class RechargeRuleService : RemoteService<CommodityInfoRes>
    {
        protected override void OnExecute()
        {
            CommodityInfoReq req = new CommodityInfoReq()
            {
                Tag = AppConfig.Instance.payKey
            };
            byte[] reqBytes = NetWorkManager.GetByteData(req);
            InitData(CMD.RECHARGEC_RECHARGERULE, reqBytes, true);
        }

        protected override void ProcessData()
        {
            GlobalData.PayModel.InitProductInfo(_data);
        }
    }
}