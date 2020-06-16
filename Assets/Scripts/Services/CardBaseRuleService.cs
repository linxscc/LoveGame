using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Module.NetWork;
using Com.Proto;
using DataModel;

namespace Assets.Scripts.Services
{
    public class CardBaseRuleService : RemoteService<CardRuleRes>
    {
        protected override void OnExecute()
        {
            InitData(CMD.CARDC_CARDRULE, null, true);
        }

        protected override void ProcessData()
        {
            GlobalData.CardModel.InitBaseRule(_data);
        }
    }
}