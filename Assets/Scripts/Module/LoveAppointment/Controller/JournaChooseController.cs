using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module;
using Assets.Scripts.Module.NetWork;
using Com.Proto;
using Common;
using DataModel;
using game.main;
using UnityEngine;

public class JournalChooseController : Controller
{

	public JournalChooseView View;
	private AppointmentRuleVo _appointmentRuleVo;
//	public AppointmentModel AppointmentModel;
    
	public override void Start()
	{
		
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

	public void SetData(int vo)
	{
//		_appointmentRuleVo = vo;
		//Debug.LogError(vo);
		List<AppointmentRuleVo> data= GlobalData.AppointmentData.GetTargetData(vo);
		View.SetData(data,vo,GlobalData.AppointmentData);
	}
	
	public void GuideData(int vo)
	{
//		_appointmentRuleVo = vo;
		Debug.Log("guide role"+vo);
		List<AppointmentRuleVo> data= GlobalData.AppointmentData.GetTargetData(vo);
		View.SetData(data,vo,GlobalData.AppointmentData);
		//这里要返回数据回去！
		SendMessage(new Message(MessageConst.MOUDLE_GUIDE_SUPPORTERACT_STARTSUCCESS,data));
		
	}


}