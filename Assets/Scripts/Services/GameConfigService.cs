using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Module.NetWork;
using Com.Proto;
using Common;
using DataModel;

namespace Assets.Scripts.Services
{
    /// <summary>
    /// 游戏配置信息
    /// </summary>
    public class GameConfigService : RemoteService<GameConfigRes>
    {
        protected override void OnExecute()
        {
            InitData(CMD.GAME_CONFIG, null, true);
        }

        protected override void ProcessData()
        {
            GlobalData.ConfigModel.InitBaseData(_data);
            GuideManager.SetData(_data.FunctionEntrys);
        }
    }
}