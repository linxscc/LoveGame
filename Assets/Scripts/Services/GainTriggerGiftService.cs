using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module.NetWork;
using Com.Proto;

namespace Assets.Scripts.Services
{
    public class GainTriggerGiftService : RemoteService<ReceiveFreeTriggerGiftRes>
    {
        public RemoteService<ReceiveFreeTriggerGiftRes> Request(int giftId)
        {
            ReceiveFreeTriggerGiftReq req = new ReceiveFreeTriggerGiftReq()
            {
                Id = giftId
            };
            requstBytes = NetWorkManager.GetByteData(req);
            return this;
        }   
        
        protected override void OnExecute()
        {
            InitData(CMD.MALLC_RECEIVEFREETRIGGERGIFT);
        }

        protected override void ProcessData()
        {
            
        }
    }
}