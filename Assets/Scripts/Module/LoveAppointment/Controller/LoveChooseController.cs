using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Core.Events;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module.NetWork;
using Com.Proto;
using Common;
using Componets;
using DataModel;
using game.main;
using UnityEngine;
using UnityEngine.Networking;

public class LoveAppointmentlController : Controller
{

	public LoveChooseView View;
	//private List<AppointmentRuleVo> _appointmentRuleVos;
	//public AppointmentModel AppointmentModel;
	
	
	public override void Start()
	{
		//获取恋爱约会的规则，参考CMD的引用
		
		//OnShow的时候要重新调用
//		AppointmentModel = GetData<AppointmentModel>();
//		NetWorkManager.Instance.Send<AppointmentRuleRes>(CMD.APPOINTMENT_RULES,null,OnGetAppointmentRules,null,true, GlobalData.VersionData.VersionDic[CMD.APPOINTMENT_RULES]);
//        Util.SetIsRedPoint(Constants.REDPOINT_LOVE_BTN_LOVEAPPOINT, false);
		GetMyAppointmentRes();

    }                                                                                             

//	private void OnGetAppointmentRules(AppointmentRuleRes res)
//	{
//		if (AppointmentModel._appointmentRuleVos == null)
//		{
//			AppointmentModel.GetAppointmentRules(res);
//		}
//
//		GetMyAppointmentRes();
//	}
//
	public void GetMyAppointmentRes()
	{
		LoadingOverlay.Instance.Show();
		var buffer = NetWorkManager.GetByteData(new MyAppointmentReq() {IsOpenModule = 1});
//		Debug.LogError(buffer);
		NetWorkManager.Instance.Send<MyAppointmentRes>(CMD.APPOINTMENT_USERAPPOINTMENTS,buffer,OnGetMyAppointment);//这些数据要一开始就拉下来的
	}
	
	private void OnGetMyAppointment(MyAppointmentRes res)
	{
		LoadingOverlay.Instance.Hide();
		GlobalData.AppointmentData.InitMyAppointmentData(res);
		View.SetData(GlobalData.AppointmentData._appointmentRuleVos,GlobalData.AppointmentData);//这个生命周期要变，必须要等myappointment读取完数据后才能执行。
	}
//
	public void BackToLoveChoose()
	{
		//GetMyAppointmentRes();
		View.SetData(GlobalData.AppointmentData._appointmentRuleVos,GlobalData.AppointmentData);//这个生命周期要变，必须要等myappointment读取完数据后才能执行。
	}
	/// <summary>
	/// 处理View消息
	/// </summary>
	/// <param name="message"></param>
	public override void OnMessage(Message message)
	{
		string name = message.Name;
		object[] body = message.Params;
		switch (name)
		{

		}
	}


}