using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Core.Events;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module;
using Assets.Scripts.Module.NetWork;
using Com.Proto;
using Common;
using DataModel;
using game.main;
using Google.Protobuf.Collections;
using UnityEngine;
using Utils;

public class ActivityTemplateController : Controller
{

	public ActivityTemplateView View;


	private ActivityTemplateTaskWindow _taskWindow;
	
	public override void Init()
	{
		ClientData.LoadItemDescData(null);
		ClientData.LoadSpecialItemDescData(null);
		EventDispatcher.AddEventListener<int>(EventConst.ActivityTemplateOnPlay,SendPlayReq);
		EventDispatcher.AddEventListener<RepeatedField<AwardPB>>(EventConst.ActivityTemplateCardAniOver,OpenGetAwardsWindow);
		EventDispatcher.AddEventListener<string>(EventConst.ActivityTemplateJumpTo,JumpTo);
    }

	private void JumpTo(string jumpTarget)
	{						
		JumpToModule.JumpTo(jumpTarget, () =>
		{
			if (_taskWindow!=null)
			{
				_taskWindow.JumpToCloseWindow();
			}
		});		
	}


	public override void Start()
	{
        //View.SetUiData(GetData<ActivityTemplateModel>().TemplateUiVo);
        View.SetUIData(GetData<ActivityTemplateModel>().playerList, GetData<ActivityTemplateModel>().curPlayerId);

        InitUserActivityInfo();		
	}

    private void OnCurPlayerChange()
    {
        View.SetCurPlayer(GetData<ActivityTemplateModel>().curPlayerId);
    }


	/// <summary>
	/// 初始化假日活动用户信息
	/// </summary>
	private void InitUserActivityInfo()
	{
		
		GetUserActivityHolidayInfoReq req =new GetUserActivityHolidayInfoReq
		{
			ActivityId = GetData<ActivityTemplateModel>().CurActivityId
		};
		byte[] data = NetWorkManager.GetByteData(req);
		NetWorkManager.Instance.Send<GetUserActivityHolidayInfoRes>(CMD.ACTIVITYC_GETHOLIDAYINFO, data,UserActivityInfoCallBack);
	}

	private void UserActivityInfoCallBack(GetUserActivityHolidayInfoRes res)
	{			
		GetData<ActivityTemplateModel>().InitUserActivityHolidayInfo(res);		
		View.SetData(GetData<ActivityTemplateModel>());
	}



	
	
	public override void OnMessage(Message message)
	{
		string name = message.Name;
		object[] body = message.Params;
		switch (name)
		{
			case MessageConst.CMD_ACTIVITYTEMPLATE1_ON_LOTTERBTN:
				OpenLotteryWindow();
				break;
			case MessageConst.CMD_ACTIVITYTEMPLATE1_ON_GETBTN:
				OpenTaskWindow();
				break;
			case MessageConst.CMD_ACTIVITYTEMPLATE1_ON_PREVIEWBTN:
				OpenPreviewWindow();
				break;
			case MessageConst.CMD_ACTIVITYTEMPLATE1_ACTIVITY_OVER:			
				ModuleManager.Instance.GoBack();
				break;
			case MessageConst.CMD_ACTIVITYTEMPLATE1_GET_ACTIVE_AWARD:
				
				var weight = (int) body[0];
				
				SendGetActiveAward(weight );
				break;
			case MessageConst.CMD_ACTIVITYTEMPLATE1_ON_SHOW_REFRESH:
			
				InitUserActivityInfo();
				break;
            case MessageConst.CMD_ACTIVITYTEMPLATE1_ON_LEFT_BTN:
                GetData<ActivityTemplateModel>().SetCurPrePlayer();
                OnCurPlayerChange();
                break;
            case MessageConst.CMD_ACTIVITYTEMPLATE1_ON_RIGHT_BTN:
                GetData<ActivityTemplateModel>().SetCurNextPlayer();
                OnCurPlayerChange();
                break;
        }
	}

//	private void SendActivityEnd()
//	{
//		ActivityEndMailReq req =new ActivityEndMailReq();
//		byte[] data = NetWorkManager.GetByteData(req);
//		NetWorkManager.Instance.Send<ActivityEndMailRes>(CMD.ACTIVITYC_ACTIVITYENDMAIL,data, (c) =>
//		{
//			ModuleManager.Instance.GoBack();
//		});
//	}

	//发送活跃奖励请求
	private void SendGetActiveAward(int weight)
	{
		GainActiveHolidayAwardReq req =new GainActiveHolidayAwardReq
		{
			Weight = weight,
			ActivityId = GetData<ActivityTemplateModel>().CurActivityId
		};
		byte[] data = NetWorkManager.GetByteData(req);
		NetWorkManager.Instance.Send<GainActiveHolidayAwardRes>(CMD.ACTIVITYC_GAINACTIVEHOLIDAYAWARD,data ,ActiveHolidayAwardCallBack);
	}

	//活跃奖励回包
	private void ActiveHolidayAwardCallBack(GainActiveHolidayAwardRes res)
	{
		RewardUtil.AddReward(res.Awards);     //增加道具
		GetData<ActivityTemplateModel>().UpdateUserActivityHolidayInfo(res.UserActivityHolidayInfoPB); //更新用户信息
		
		View.SetData(GetData<ActivityTemplateModel>());  //刷新UI
		
		var isCard = false;
		foreach (var t in res.Awards)
		{
			if (t.Resource== ResourcePB.Card)
			{
				isCard = true;
				break;                   
			}  
		}

		if (isCard)
		{
			List<AwardPB> award =new  List<AwardPB>();
			foreach (var t in res.Awards)
			{
				if (t.Resource == ResourcePB.Card)
				{
					award.Add(t); 
					break;
				}                 
			}
			Action finish = () =>
			{
				SendMessage(new Message(MessageConst.CMD_ACTIVITYTEMPLATE1_ACTIVITY_SHOW_TOPBAR_AND_BACKBTN, Message.MessageReciverType.UnvarnishedTransmission,true));    
			};
			ModuleManager.Instance.EnterModule(ModuleConfig.MODULE_DRAWCARD,
				false,false,"DrawCard_CardShow",award,finish,false);
			ClientTimer.Instance.DelayCall(() =>
			{
				SendMessage(new Message(MessageConst.CMD_ACTIVITYTEMPLATE1_ACTIVITY_SHOW_TOPBAR_AND_BACKBTN, Message.MessageReciverType.UnvarnishedTransmission,false)); 
			}, 0.1f);
			
		}
		else
		{
			var  window = PopupManager.ShowWindow<CommonAwardWindow>("GameMain/Prefabs/AwardWindow/CommonAwardWindow");
			window.SetData(res.Awards.ToList(),false,ModuleConfig.MODULE_ACTIVITYTEMPLATE);
		}
		
		
		

		
	}


	//发送抽卡请求
	private void SendPlayReq(int playNum)
	{
        Debug.LogWarning("SendPlayReq player:"+ GetData<ActivityTemplateModel>().curPlayerId);
        DrawingRewardsReq req = new DrawingRewardsReq
        {
            Type = playNum,
            ActivityId = GetData<ActivityTemplateModel>().CurActivityId,
            Player = GetData<ActivityTemplateModel>().curPlayerId
        };		
		byte[] data = NetWorkManager.GetByteData(req);
		NetWorkManager.Instance.Send<DrawingRewardsRes>(CMD.ACTIVITYC_DRAWAWARDS,data ,DrawingRewardsCallBack);
	}

	//抽卡回包
	private void DrawingRewardsCallBack(DrawingRewardsRes res)
	{	
		RewardUtil.AddReward(res.Awards);//增加道具
		GetData<ActivityTemplateModel>().UpdateUserActivityHolidayInfo(res.UserActivityHolidayInfoPB);
		GlobalData.PropModel.UpdateProps(new[] {res.UserItem});
		
		
		View.SetData(GetData<ActivityTemplateModel>());  //刷新UI
		
	
		OpenCardAniWindow(res.Awards);
	}


	///打开翻牌窗口
	private void OpenCardAniWindow(RepeatedField<AwardPB> awardPbs)
	{
		var window = PopupManager.ShowWindow<ActivityTemplateCardAniWindow>("ActivityTemplate/Prefabs/ActivityTemplateCardAniWindow");	
		window.SetData(awardPbs);
	}

	///打开抽奖窗口
	private void OpenLotteryWindow()
	{
		var window = PopupManager.ShowWindow<ActivityTemplateLotteryWindow>("ActivityTemplate/Prefabs/ActivityTemplateLotteryWindow");
		var id = GetData<ActivityTemplateModel>().ConsumeItemId;
		var haveNum = GetData<ActivityTemplateModel>().ActivityItemNum();
		var price =  GetData<ActivityTemplateModel>().Price;
		var playMoreNum =  GetData<ActivityTemplateModel>().PlayMoreNum;
		window.SetData(id,haveNum,price,playMoreNum);
	}
	

	///打开活动任务窗口
	private void OpenTaskWindow()
	{
		_taskWindow = PopupManager.ShowWindow<ActivityTemplateTaskWindow>("ActivityTemplate/Prefabs/ActivityTemplateTaskWindow");
		_taskWindow.SetData(GetData<ActivityTemplateModel>().GetActivityTaskVo());
	}
	

	///打开预览窗口
	private void OpenPreviewWindow()
	{
		var window = PopupManager.ShowWindow<ActivityTemplateAwardWinodw>("ActivityTemplate/Prefabs/ActivityTemplateAwardWinodw");
		window.SetParentAwardsData(GetData<ActivityTemplateModel>().GetCurAwardData());
	}

	///打开获得奖励窗口
	private void OpenGetAwardsWindow(RepeatedField<AwardPB> list)
	{
		var window = PopupManager.ShowWindow<ActivityTemplateAwardWinodw>("ActivityTemplate/Prefabs/ActivityTemplateAwardWinodw");
		window.SetGetAwardsData(list);
	}
	
	
	
	public override void Destroy()
	{
		EventDispatcher.RemoveEvent(EventConst.ActivityTemplateOnPlay);
		EventDispatcher.RemoveEvent(EventConst.ActivityTemplateCardAniOver);
		EventDispatcher.RemoveEvent(EventConst.ActivityTemplateJumpTo);

    }
}
