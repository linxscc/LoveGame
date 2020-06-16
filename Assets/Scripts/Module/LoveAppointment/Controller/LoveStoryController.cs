using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Core.Events;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module;
using Assets.Scripts.Module.NetWork;
using Com.Proto;
using Common;
using Componets;
using DataModel;
using game.main;
using UnityEngine;

public class LoveStoryController : Controller
{
	public LoveStoryView View;
	private AppointmentRuleVo _appointmentRuleVo;
	private EnsureOpenGateWindow _ensureOpenGateWindow;
	private AppointmentGateRuleVo _usergate;
	private AppointmentRuleVo _appointmentrule;
//	public AppointmentModel AppointmentModel;
	private int _curId;
    
	public override void Start()
	{
		//获取用户任务的数据源
		//View.SetMissionItemData(GlobalData.MissionModel.UserMissionList);
		EventDispatcher.AddEventListener<int[]>(EventConst.LoveStoryEnd,UpdateStoryEndData);
		EventDispatcher.AddEventListener(EventConst.LoveAppointmentGotoCardResolve,GotoCardResolve);
	}

	private void GotoCardResolve()
	{
		_ensureOpenGateWindow?.Close();
		ModuleManager.Instance.EnterModule(ModuleConfig.MODULE_CARD,false,false,"CardResolve");
		
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
			case MessageConst.CMD_APPOINTMENT_ENSUREOPENGATE:
				_usergate = (AppointmentGateRuleVo) body[0];
				_appointmentrule = (AppointmentRuleVo) body[1];
				
				//这里要判断卡牌和星级啊。从appointment中获取到。
				var cardvo = GlobalData.CardModel.GetUserCardById(_appointmentrule.ActiveCards[0]);
				if (cardvo.Star<_usergate.Star)
				{
                    //Debug.LogError("cstar"+cardvo.Star+"  userstar"+_usergate.Star);
                    FlowText.ShowMessage(I18NManager.Get("LoveAppointment_Hint3"));//("星缘心级不足");
					return;
				}

				if ((int)cardvo.Evolution<_usergate.Evo)
				{
//					Debug.LogError("(int)cardvo.Evolution"+(int)cardvo.Evolution+" _usergate.Evo"+_usergate.Evo);
					FlowText.ShowMessage(I18NManager.Get("LoveAppointment_Hint4"));//("星缘未进化");
                    return;
				}
				
				
				
				if (_ensureOpenGateWindow==null)
				{
					_ensureOpenGateWindow=PopupManager.ShowWindow<EnsureOpenGateWindow>("LoveAppointment/Prefabs/EnsureOpenGateWindow");
				}
				_ensureOpenGateWindow.SetData(_usergate,_appointmentrule.Id);
				
				//bug 这个事件监听在Start的时候无法执行。
				EventDispatcher.AddEventListener(EventConst.OpenGate,OpenGate);

				break;
//			case MessageConst.CMD_APPOINTMENT_UPDATE_LOVESTORY:
//				
//				UpdateAppointmentData();
//				break;
			case MessageConst.CMD_APPOINTMENT_ACTIVE_PHOTOCLEARUP:
				PhotoNickUpReq((int)body[0],(int)body[1]);
				break;
			case MessageConst.CMD_APPOINTMENT_JUMPTODAILY:

				if (!GuideManager.IsOpen(ModulePB.Love, FunctionIDPB.LoveDiary))
				{
					string desc = GuideManager.GetOpenConditionDesc(ModulePB.Love, FunctionIDPB.LoveDiary);
					FlowText.ShowMessage(desc);
					return;
				}	           
				ModuleManager.Instance.EnterModule(ModuleConfig.MODULE_LOVEDIARY,false);
				break;
		}
	}

	public void SetData(AppointmentRuleVo vo)
	{
		_appointmentRuleVo = vo;
		LoadingOverlay.Instance.Show();
		var buff=NetWorkManager.GetByteData(new ActiveReq() {AppointmentId = vo.Id});
		NetWorkManager.Instance.Send<ActiveRes>(CMD.APPOINTMENT_ACTIVE,buff,ActiveAppointment);
	}

	public void UpdateAppointmentData()
	{
		View.SetData(GlobalData.AppointmentData._curAppointmentRuleVo,GlobalData.AppointmentData);
	}
	
	private void ActiveAppointment(ActiveRes res)
	{
		LoadingOverlay.Instance.Hide();
		GlobalData.AppointmentData.UpdateUserAppointment(res.UserAppointments);
		View.SetData(_appointmentRuleVo,GlobalData.AppointmentData);
		Show();
	}

	private void OpenGate()
	{	
		foreach (var v in _usergate.CosumesDic)
		{
			UserPropVo item = GlobalData.PropModel.GetUserProp(v.Key);
			int propnum = item.Num;
			if (propnum<v.Value)
			{
				FlowText.ShowMessage(I18NManager.Get("LoveAppointment_Hint5"));//("道具不足！");
                return;
			}
		}
				
		_ensureOpenGateWindow.Close();
		LoadingOverlay.Instance.Show();
		
		var buffer = NetWorkManager.GetByteData(new OpenGateReq() {AppointmentId = _appointmentrule.Id, Gate = _usergate.Gate});
		NetWorkManager.Instance.Send<OpenGateRes>(CMD.APPOINTMENT_OPENGATE,buffer,OpenGateSuc);
	}

	public void UpdateStoryEndData(int[] array)
	{
		var appointmentgate = array[0];
		var appointmentid = array[1];
//		Debug.LogError("Appointmentdata"+array[0]);
		var userapoointment = GlobalData.AppointmentData.GetUserAppointment(appointmentid);
		var state = GlobalData.AppointmentData.IsGateActive(userapoointment.AppointmentId,
			appointmentgate);
		if (state != GateState.Finish)
		{
			var buffer = NetWorkManager.GetByteData(new PassGateReq()
			{
				AppointmentId = appointmentid,
				Gate = appointmentgate
			});
                        
			LoadingOverlay.Instance.Show();
			NetWorkManager.Instance.Send<PassGateRes>(CMD.APPOINTMENT_PASSGATE, buffer,
				UpdateAppointmentInfo);
		}
		else
		{
			Debug.LogError("Has pass");
			//这里有BUG了！
			//ModuleManager.Instance.GoBack();
		}
		
	}
	
	private void UpdateAppointmentInfo(PassGateRes res)
	{
		LoadingOverlay.Instance.Hide();
		GlobalData.AppointmentData.UpdateUserAppointment(res.UserAppointments);
		UpdateAppointmentData();
		
	}
	
	
	public void Show()
	{
		View.Show();
	}
	
	private void OpenGateSuc(OpenGateRes res)
	{
		//Debug.LogError(res.UserAppointments);
		LoadingOverlay.Instance.Hide();
		GlobalData.AppointmentData.UpdateUserAppointment(res.UserAppointments);
		GlobalData.PropModel.UpdateProps(res.UserItems);
		if (GlobalData.MissionModel.IsHaveStarActivityMission)
		{
			GlobalData.MissionModel.MissionAttainmentModel.AddLoveDramaNum(res.UserAppointments.ActiveGates.Count);
		}
		ModuleManager.Instance.EnterModule(ModuleConfig.MODULE_STORY,false, false, _usergate,_appointmentrule.Id);
	}

//	private void PhotoClearOnReq(int id)
//	{
//		LoadingOverlay.Instance.Show();
//		var buffer = NetWorkManager.GetByteData(new PhotoClearReq() {AppointmentId = id});
//		NetWorkManager.Instance.Send<PhotoClearRes>(CMD.APPOINTMENT_PHOTOCLEAR, buffer, OnPhotoClearCallBack);
//	}
//
//	private void OnPhotoClearCallBack(PhotoClearRes res)
//	{
//		LoadingOverlay.Instance.Hide();
//		AppointmentModel.UpdateUserAppointment(res.UserAppointments);
//		UpdateAppointmentData();
//		FlowText.ShowMessage("成功获得拍立得");
//	}

	
	//钉起来的功能被去掉了
	private void PhotoNickUpReq(int id,int gate)
	{
		LoadingOverlay.Instance.Show();
		Debug.LogError(id+" gate"+gate);
		_curId = id;
		var buffer = NetWorkManager.GetByteData(new PhotoNickUpReq() {AppointmentId = id,Gate = gate});
		NetWorkManager.Instance.Send<PhotoNickUpRes>(CMD.APPOINTMENT_PHOTONICKUP, buffer, OnPhotoNickUpCallBack);
	}

	private void OnPhotoNickUpCallBack(PhotoNickUpRes res)
	{
		LoadingOverlay.Instance.Hide();
		//AppointmentModel.UpdateUserAppointment(res.UserAppointments);
		Debug.LogError("Success PhotoNickUp");
		//返回成功才在缓存里添加奖励。
		GlobalData.AppointmentData.UpdatePassGateAwardToProp(GlobalData.AppointmentData.GetUserAppointment(_curId));
		//View.EndPicSuccess();
		//在这里做拍立得动画
		//UpdateAppointmentData();
		View.SetData(GlobalData.AppointmentData._curAppointmentRuleVo,GlobalData.AppointmentData,true);
	}


	public override void Destroy()
	{
		base.Destroy();
		EventDispatcher.RemoveEventListener<int[]>(EventConst.LoveStoryEnd,UpdateStoryEndData);
		EventDispatcher.RemoveEventListener(EventConst.LoveAppointmentGotoCardResolve,GotoCardResolve);
		EventDispatcher.RemoveEventListener(EventConst.OpenGate,OpenGate);
	}

}