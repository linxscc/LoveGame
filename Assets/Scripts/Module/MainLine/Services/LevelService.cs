using Assets.Scripts.Framework.GalaSports.Core.Events;
using Assets.Scripts.Module.NetWork;
using Com.Proto;
using Common;
using DataModel;
using Framework.GalaSports.Core;

namespace Assets.Scripts.Module.MainLine.Services
{
    public class LevelService : ComboService<LevelModel, LevelRes, MyLevelRes>
    {
        private int _lastPassLevelNum = -1;

        protected override void OnExecute()
        {
            AddServiceData(CMD.CAREERC_LEVELS, null, true);
            AddServiceData(CMD.CAREERC_MYLEVELS);
        }

        protected override void ProcessData(LevelRes resU, MyLevelRes resV)
        {
            _data = GlobalData.LevelModel;
            _data.SetData(resU);
            _data.SetMyLevelData(resV);

            SetModel(_data);

            if (_lastPassLevelNum != -1 && _lastPassLevelNum < _data.PassLevelCount)
            {
                EventDispatcher.TriggerEvent(EventConst.MainLineLevelUpdate);
            }

            _lastPassLevelNum = _data.PassLevelCount;
        }
    }
}