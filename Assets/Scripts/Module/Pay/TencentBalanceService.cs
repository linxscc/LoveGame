using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module.NetWork;
using Assets.Scripts.Module.Pay.Agent;
using Com.Proto;
using Com.Proto.Server;
using DataModel;
using GalaSDKBase;

namespace Assets.Scripts.Module.Pay
{
    public class TencentBalanceService : RemoteService<TxBalanceRes>
    {
        protected override void OnExecute()
        {
            TxBalanceReq req = new TxBalanceReq()
            {
                GameId = 3,
                SdkVersion = PayVersion.GetPaySdkVersion(PayAgent.PayType.None),
                Version = PayVersion.GetPayVersion(PayAgent.PayType.None),
                Params = GalaSDKBaseFunction.SdkFetchTokenWithType("")
            };
            byte[] reqBytes = NetWorkManager.GetByteData(req);
            InitData(CMD.RECHARGEC_CHECKTXBALANCES, reqBytes);
        }

        protected override void ProcessData()
        {
            PayAgentTencent payAgent = SdkHelper.PayAgent as PayAgentTencent;
            if (payAgent != null)
            {
                payAgent.Balance = _data.Balance;
            }
        }
    }
}