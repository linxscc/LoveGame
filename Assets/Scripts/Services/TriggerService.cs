using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module.NetWork;
using Com.Proto;
using DataModel;
using UnityEngine;

namespace Assets.Scripts.Services
{
    public class TriggerService : RemoteService<TriggerGiftRes>
    {
        private bool _showNewGiftWindow = false;

        public TriggerService ShowNewGiftWindow()
        {
            _showNewGiftWindow = true;
            return this;
        }

        /// <summary>
        /// 需要具体触发的礼包
        /// </summary>
        /// <param name="mallIds"></param>
        public TriggerService Request(params int[] mallIds)
        {
            TriggerGiftReq req = new TriggerGiftReq();
            req.MaillId.AddRange(mallIds);

            requstBytes = NetWorkManager.GetByteData(req);

            Debug.LogError("TriggerService Mallids" + mallIds[0]);
            return this;
        }

        protected override void OnExecute()
        {
            InitData(CMD.MALLC_TRIGGERGIFT, requstBytes);
        }

        protected override void ProcessData()
        {
            GlobalData.RandomEventModel.InitData(_data.UserTriggerGiftData, _data.UserTriggerGiftPB, _showNewGiftWindow);
        }
    }
}