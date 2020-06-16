using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Module.NetWork;
using Com.Proto;
using DataModel;

namespace Assets.Scripts.Services
{
    public class CardBaseDataService : RemoteService<CardRes>
    {
        
        protected override void OnExecute()
        {
            InitData(CMD.CARDC_CARDS, null, true);
        }

        protected override void ProcessData()
        {
            GlobalData.CardModel.InitBaseData(_data);
        }
    }
}