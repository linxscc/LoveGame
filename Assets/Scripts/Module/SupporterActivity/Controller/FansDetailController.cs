using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Core.Events;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module;
using Assets.Scripts.Module.MainLine.Services;
using Assets.Scripts.Module.NetWork;
using Com.Proto;
using Common;
using Componets;
using DataModel;
using game.main;
using UnityEngine;

public class FansDetailController : Controller
{
	private GainPropWindow _gainPropWindow;
	public FansDetailView View;
	//private List<AppointmentRuleVo> _appointmentRuleVos;
	public SupporterActivityModel SupporterActivityModel;
	private UserEncourageActVo _curUserEncourageActVo;
	
	public override void Start()
	{
		//获取恋爱约会的规则，参考CMD的引用
		//cityLevelModel=
		ClientData.LoadJumpData(null);
	}

	public void SetData(UserEncourageActVo vo)
	{
		_curUserEncourageActVo = vo;
		View.SetData(vo,SupporterActivityModel);
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
			case MessageConst.CMD_SUPPORTERACTIVITY_STARTACTIVITY:
				var vo = (UserEncourageActVo)body[0];
				StartActivityReq(vo);
				break;
			case MessageConst.CMD_SUPPORTERACTIVITY_OPENPROPORG:
				//Debug.LogError((KeyValuePair<int, int>)body[0]);
				var keyvaluevo = (KeyValuePair<int, int>)body[0];
				ShowObtainWindow(keyvaluevo);
				break;
			case MessageConst.CMD_SUPPORTERACTIVITY_GUIDETODOINGACT:
				StartActivityReq(_curUserEncourageActVo);
				break;
			case MessageConst.MODULE_SUPPORTERACTIVITY_JUMPTOOTHER:
				ModuleManager.Instance.EnterModule(ModuleConfig.MODULE_DRAWCARD,false,false,"DrawCard_Gold");
				break;
		}
	}

	private void StartActivityReq(UserEncourageActVo vo)
	{
//		Debug.LogError("活动Id为"+vo.Id);
		LoadingOverlay.Instance.Show();
		var buffer = NetWorkManager.GetByteData(new StartReq() {Id =vo.Id ,ActId = vo.ActId});
		NetWorkManager.Instance.Send<StartRes>(CMD.SUPPORTERACTIVITY_START,buffer,OnStartActivityRes);
	}

	private void OnStartActivityRes(StartRes res)
	{
		//先更新GlobaData的数据，然后回到应援活动界面！
		LoadingOverlay.Instance.Hide();
		FlowText.ShowMessage(I18NManager.Get("SupporterActivity_StartSuccess")+SupporterActivityModel.EncourageRuleDic[res.UserEncourageActivity.ActId].Title);
		//GlobalData.PlayerModel.UpdateUserPower(res.UserPower);
//		Debug.LogError(res.UserEncourageActivity);
		GlobalData.PropModel.UpdateProps(res.UserItems);
		
		//就这里可能出现问题！
		foreach (var v in res.UserFans)
		{
			GlobalData.DepartmentData.UpdateFansWithNum(v.FansType,v.Num);
		}
		SupporterActivityModel.UpdateEncourageActs(res.UserEncourageActivity);
		SupporterActivityModel.GetUserActVo(res.UserEncourageActivity.Id).NeedToChangeAni=true;
		SendMessage(new Message(MessageConst.CMD_SUPPORTERACTIVITY_STARTSUCCESS,res.LeftInterCount));

		EventDispatcher.TriggerEvent(EventConst.UpdateEnergy);
        SendMessage(new Message(MessageConst.MOUDLE_GUIDE_SUPPORTERACT_STARTSUCCESS));
	}
	
	private void ShowObtainWindow(KeyValuePair<int,int> vo)
	{
		if (_gainPropWindow == null)
		{
			_gainPropWindow = PopupManager.ShowWindow<GainPropWindow>("Card/Prefabs/CardDetail/GainPropWindow");
		}

		if (GlobalData.LevelModel==null)
		{
			Debug.LogError("Model is null");
		}
		
		_gainPropWindow.SetData(vo,GlobalData.CardModel,GlobalData.LevelModel);
	}                                                                                                
	
}