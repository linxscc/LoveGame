using Assets.Scripts.Framework.GalaSports.Core.Events;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module.NetWork;
using Com.Proto;
using Common;
using DataModel;
using Framework.GalaSports.Core;

namespace Assets.Scripts.Services
{
	public class LoveAppointmentService : ComboService<AppointmentModel, AppointmentRuleRes, MyAppointmentRes>
	{
		protected override void OnExecute()
		{
			httpTimeoutU = 100;
			AddServiceData(CMD.APPOINTMENT_RULES, null, true);
			//var buffer = NetWorkManager.GetByteData(new MyAppointmentReq() {IsOpenModule = 1});
			AddServiceData(CMD.APPOINTMENT_USERAPPOINTMENTS);
		}

		protected override void ProcessData(AppointmentRuleRes resU, MyAppointmentRes resV)
		{
			_data=GlobalData.AppointmentData;
			_data.GetAppointmentRules(resU);
			_data.InitMyAppointmentData(resV);
//			_data=GlobalData.AppointmentData;
//			_data.InitBaseData(resU);
//			_data.InitUserData(resV);
//			SetModel(_data);

		}
	}
}