using Assets.Scripts.Framework.GalaSports.Core.Events;
using Assets.Scripts.Module.NetWork;
using Com.Proto;
using Common;
using DataModel;
using Framework.GalaSports.Core;

namespace Assets.Scripts.Module.Task.Service
{
	public class MissionService : ComboService<MissionModel, MissionRuleRes, UserMissionsRes>
	{
		protected override void OnExecute()
		{
			httpTimeoutU = 100;
			AddServiceData(CMD.MISSION_RULES, null, true);
			AddServiceData(CMD.MISSION_MYMISSION);
		}

		protected override void ProcessData(MissionRuleRes resU, UserMissionsRes resV)
		{
			_data=GlobalData.MissionModel;
			_data.InitBaseData(resU);
			_data.InitUserData(resV);
			SetModel(_data);

		}
	}
}