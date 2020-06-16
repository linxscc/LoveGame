using System;
using System.Linq;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Core.Events;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module.NetWork;
using Com.Proto;
using Common;
using Componets;
using DataModel;
using Framework.GalaSports.Service;
using game.main;
using Google.Protobuf.Collections;
using UnityEngine;
using Utils;

public class ActivityChooseController : Controller
{

	public ActivityChooseView View;
	//private List<AppointmentRuleVo> _appointmentRuleVos;
	private AcitivityRewardTips _rewardTips;
	private SupporterRewardWindow _getRewardWindow;
	private int _replaceActId;
	private int _replaceActImmediateId;
	
	public SupporterActivityModel SupporterActivityModel;

//	private ConfirmWindow _confirmWindow;
	
	public override void Start()
	{
		//OnShow的时候要重新调用
		GetUserData();
//		EventDispatcher.AddEventListener(EventConst.UpdateEnergy, UpdateSupporterPower);
		EventDispatcher.TriggerEvent<bool>(EventConst.ChangeTopPower,true);
	}


	private void GetUserData()
	{
		LoadingOverlay.Instance.Show();
		SupporterActivityModel = GetData<SupporterActivityModel>();
		NetWorkManager.Instance.Send<EncourageActRulesRes>(CMD.SUPPORTERACTIVITY_ENCOURAGEACTRULES,null,OnGetEncourageActRules,null,true, GlobalData.VersionData.VersionDic[CMD.SUPPORTERACTIVITY_ENCOURAGEACTRULES]);

	}
	
	private void OnGetEncourageActRules(EncourageActRulesRes res)
	{
		if (SupporterActivityModel.EncourageActRuleVos==null)
		{
			SupporterActivityModel.InitEncourageActRule(res.EncourageActivityRules.ToList());
			SupporterActivityModel.InitEncourageRefresh(res.EncourageActivityRefreshRules.ToList());
			SupporterActivityModel.InitEncourageDone(res.EncourageActivityDoneRules.ToList());
			SupporterActivityModel.InitActBuyRule(res.EncourageActivityBuyRules);
		}

		GetMyEncourageActData();
	}

	public void GetMyEncourageActData()
	{
		LoadingOverlay.Instance.Show();
		NetWorkManager.Instance.Send<MyEncourageActsRes>(CMD.SUPPORTERACTIVITY_MYENCOURAGEACTS,null,OnMyEncourageActData,GetUserActDataErrorCallBack);
		//View
	}

	private void GetUserActDataErrorCallBack(HttpErrorVo vo)
	{
		//收不到回包的情况下也会进行错误处理刷新了。
		UpgrageUserData();

	}

	private void OnMyEncourageActData(MyEncourageActsRes res)
	{
		LoadingOverlay.Instance.Hide();
//		Debug.LogError(res.UserEncourageActivitys[2]);
		SupporterActivityModel.GetMyEncourageActs(res.UserEncourageActivitys.ToList());
//		Debug.LogError("当天手动刷新次数"+res.RefreshCount);
		//根据刷新次数话获得金币消费
		SupporterActivityModel.RefreshCount = res.RefreshCount;
		SortTheUserVo();
		View.SetData(SupporterActivityModel.UserEncourageActVos,SupporterActivityModel,res.RefreshCount,res.NextRefreshTime);
		View.SetLeftTimes(res.LeftInterCount);
		SupporterActivityModel.hasBuyTime = res.BuyInterCount;
		SupporterActivityModel.leftBuyTime = res.LeftInterCount;
		SupporterActivityModel.PushActFinishTime();
		//Debug.LogError("当天刷新时间"+res.NextRefreshTime);
	}

	public void UpgrageUserData(bool isStart=false)
	{
		
		//解决办法：如果是有进行中的任务，那么该任务的序号不能变，保留该任务所在的序号，其他任务按依旧按Id排序。
		SortTheUserVo();
		View.SetData(SupporterActivityModel.UserEncourageActVos,SupporterActivityModel,SupporterActivityModel.RefreshCount);
	}

	public void SortTheUserVo()
	{
		SupporterActivityModel.UserEncourageActVos.Sort();
		for (int i = 0; i < SupporterActivityModel.UserEncourageActVos.Count; i++)
		{
			SupporterActivityModel.UserEncourageActVos[i].PosIndex = i;
		}
	}

	//不需要排序的更新数据
	public void UpdateUserData()
	{
		View.SetData(SupporterActivityModel.UserEncourageActVos,SupporterActivityModel,SupporterActivityModel.RefreshCount);
	}

	public void RefreshStartItem(bool isReceiveAwrad=false)
	{
		//Debug.LogError("RefreshStart!");
		View.RefreshStartItemData(SupporterActivityModel.RefreshUserStartActItem,isReceiveAwrad);
		SupporterActivityModel.PushActFinishTime();
	}

	public void RefreshBuyTime(int lefttime)
	{
				
		View.SetLeftTimes(lefttime);
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
			case MessageConst.CMD_SUPPORTERACTIVITY_REFRESH:
					//复选框！
				OnRefeshReq((UserEncourageActVo) body[0]);
				break;
			case MessageConst.CMD_SUPPORTERACTIVITY_GETREWARD:
				var rewardvo = (UserEncourageActVo) body[0];
				OnGetRewardReq(rewardvo);
				break;
			case MessageConst.CMD_SUPPORTERACTIVITY_DONEIMMEDIATE:

				
//				Debug.LogError("Call");
				PopupManager.ShowConfirmWindow(I18NManager.Get("SupporterActivity_NeedToDoImmediate"), I18NManager.Get("Common_Hint"), I18NManager.Get("Common_OK1"), 
					I18NManager.Get("Common_Cancel1")).WindowActionCallback = evt =>
				{
					if (evt==WindowEvent.Ok)
					{
						OnDoneimmediate((UserEncourageActVo) body[0]);
					}

				};
				break;
			case MessageConst.CMD_SUPPORTERACTIVITY_REWARDTIPS:
				var rulevo = (EncourageActRuleVo) body[0];
				OpenRuleTips(rulevo);
				break;
			case MessageConst.CMD_SUPPORTERACTIVITY_GETMYENCOURAGE:
				GetMyEncourageActData();
				break;
            case MessageConst.CMD_SUPPORTERACTIVITY_BUYENCOURAGEPOWER:

                var _buyEncouragePowerUpperlimit = GlobalData.PlayerModel.BuyEncouragePowerUpperlimit;
                if (GlobalData.PlayerModel.PlayerVo.EncourageNum>= _buyEncouragePowerUpperlimit)
                {
                    FlowText.ShowMessage(I18NManager.Get("SupporterActivity_BuyTimeMax"));
                    return;
                }
                else
                {
                    BuyEncouragePower();
                }
	            
                break;
			case MessageConst.CMD_SUPPORTERACTIVITY_BuyInner:
				if (SupporterActivityModel.EncourageActBuyRulePbs.Count-SupporterActivityModel.hasBuyTime<=0)
				{					
					FlowText.ShowMessage(I18NManager.Get("SupporterActivity_NoBuyTime"));
					return;
				}
				
				PopupManager.ShowConfirmWindow(I18NManager.Get("SupporterActivity_CostToBuy",
					SupporterActivityModel.GetBuyRulePb(SupporterActivityModel.hasBuyTime+1).Gem,SupporterActivityModel.EncourageActBuyRulePbs.Count-SupporterActivityModel.hasBuyTime) , I18NManager.Get("Common_Hint"),I18NManager.Get("Common_OK1"), I18NManager.Get("Common_Cancel1")).WindowActionCallback = evt =>
				{
					if (evt==WindowEvent.Ok)
					{
						BuyTimeReq();
					}
				};
				

				break;

        }
	}

    private void BuyEncouragePower()
    {
        QuickBuy.BuyGlodOrPorwer(PropConst.EncouragePowerId, PropConst.GemIconId);
    }


    private void OpenRuleTips(EncourageActRuleVo rulevo)
	{
		if (_rewardTips==null)
		{
			_rewardTips=PopupManager.ShowWindow<AcitivityRewardTips>("SupporterActivity/Prefabs/ActivityRewardTips");
		}
		_rewardTips.SetData(rulevo);

	}

	
	private void OnDoneimmediate(UserEncourageActVo vo)
	{
		LoadingOverlay.Instance.Show();
//		Debug.LogError("加速"+vo.Id+" "+vo.ActId);
		_replaceActImmediateId = vo.ActId;
		var buffer = NetWorkManager.GetByteData(new DoneImmediateReq(){Id = vo.Id,ActId = vo.ActId});
		NetWorkManager.Instance.Send<DoneImmediateRes>(CMD.SUPPORTERACTIVITY_DONEIMMEDIATE,buffer,DoneImmeediateOnReq);
	}

	private void DoneImmeediateOnReq(DoneImmediateRes res)
	{
		LoadingOverlay.Instance.Hide();
		FlowText.ShowMessage(I18NManager.Get("SupporterActivity_SpeedUpSuccess"));
		
//		if (res.FansType!=0)
//		{
//			GlobalData.DepartmentData.UpdateFans(res.FansType,1);		
//		}
		SupporterActivityModel.UpdateEncourageActs(res.UserEncourageActivity,_replaceActImmediateId);
		var backFansRule = SupporterActivityModel.EncourageRuleDic[_replaceActImmediateId];

		foreach (var v in backFansRule.Fans)
		{
			GlobalData.DepartmentData.UpdateFans(v.Key,v.Value);
		}

		RewardUtil.AddReward(res.Awards);
		RewardUtil.AddReward(res.ExtraAwards);
		if (res.DroppingItem.Count>0)
		{
			RepeatedField<AwardPB> awardPbs = new RepeatedField<AwardPB>();
			for (int i = 0; i < res.DroppingItem.Count; i++)
			{
				AwardPB awardPb=new AwardPB(){Num =res.DroppingItem[i].Num,ResourceId = res.DroppingItem[i].ResourceId,Resource = res.DroppingItem[i].Resource};
				awardPbs.Add(awardPb);
			}

			RewardUtil.AddReward(awardPbs);
			
			
		}
		GlobalData.PlayerModel.UpdateUserMoney(res.UserMoney);//你又发送金币过来，又不发送奖励过来，到底啥意思？
		
		if (_getRewardWindow==null)
		{
			_getRewardWindow=PopupManager.ShowWindow<SupporterRewardWindow>("SupporterActivity/Prefabs/GetActivityRewarWindow");
		}
		Debug.LogError(res.ExtraAwards.Count);
		_getRewardWindow.SetData(res.Awards,res.ExtraAwards,res.DroppingItem);

//		UpgrageUserData();
		RefreshStartItem(true);
		if (!SupporterActivityModel.GetRewardActNum())
		{
			//又要发消息给主界面！！
			EventDispatcher.TriggerEvent(EventConst.RefreshPoint);
		}
		foreach (var award in res.Awards)
		{
			if (award.Resource==ResourcePB.Gem)
			{
				SdkHelper.StatisticsAgent.OnReward(award.Num,"应援活动");   
			}
		}


	}


	private void OnGetRewardReq(UserEncourageActVo vo)
	{
		LoadingOverlay.Instance.Show();
//		Debug.LogError(vo.Id+" "+vo.ActId);
		_replaceActId = vo.ActId;
		var buffer = NetWorkManager.GetByteData(new GetAwardReq(){Id = vo.Id,ActId = vo.ActId});
		NetWorkManager.Instance.Send<GetAwardRes>(CMD.SUPPORTERACTIVITY_GETAWARD,buffer,GetRewardOnReq);
	}
	
	private void GetRewardOnReq(GetAwardRes res)
	{
		LoadingOverlay.Instance.Hide();
		
		//应援活动刷新BUG，没有使用_replaceActId来作为返回的领奖问题！
		
		SupporterActivityModel.UpdateEncourageActs(res.UserEncourageActivity,_replaceActId);
		//还是需要刷新次数的
		RewardUtil.AddReward(res.Awards);
		RewardUtil.AddReward(res.ExtraAwards);
		//活动掉落，前端要自己把它加到道具缓存中？！
		if (res.DroppingItem.Count>0)
		{
			RepeatedField<AwardPB> awardPbs = new RepeatedField<AwardPB>();
			for (int i = 0; i < res.DroppingItem.Count; i++)
			{
				AwardPB awardPb=new AwardPB(){Num =res.DroppingItem[i].Num,ResourceId = res.DroppingItem[i].ResourceId,Resource = res.DroppingItem[i].Resource};
				awardPbs.Add(awardPb);
			}

			RewardUtil.AddReward(awardPbs);
			
			
		}
		

		
		
		if (_getRewardWindow==null)
		{
			_getRewardWindow=PopupManager.ShowWindow<SupporterRewardWindow>("SupporterActivity/Prefabs/GetActivityRewarWindow");
		}
		
		_getRewardWindow.SetData(res.Awards,res.ExtraAwards,res.DroppingItem);
		var backFansRule = SupporterActivityModel.EncourageRuleDic[_replaceActId];

		foreach (var v in backFansRule.Fans)
		{
			GlobalData.DepartmentData.UpdateFans(v.Key,v.Value);
		}
		
//		UpgrageUserData();
		RefreshStartItem(true);
		if (!SupporterActivityModel.GetRewardActNum())
		{
			//又要发消息给主界面！！
			EventDispatcher.TriggerEvent(EventConst.RefreshPoint);
		}
		foreach (var award in res.Awards)
		{
			if (award.Resource==ResourcePB.Gem)
			{
				SdkHelper.StatisticsAgent.OnReward(award.Num,"应援活动");                                                 
			}
		}

	}

	public void OnRefeshReq(UserEncourageActVo vo)
	{
		LoadingOverlay.Instance.Show();
		var buffer = NetWorkManager.GetByteData(new RefreshReq(){Id = vo.Id});

		_replaceActId = vo.ActId;
		Debug.LogError(_replaceActId);
		NetWorkManager.Instance.Send<RefreshRes>(CMD.SUPPORTERACTIVITY_REFRESH,buffer,RefreshOnReq);
	}

	private void RefreshOnReq(RefreshRes res)
	{
		LoadingOverlay.Instance.Hide();
		//Debug.LogError("refresh");
		FlowText.ShowMessage(I18NManager.Get("SupporterActivity_ChangeSuccess"));
		SupporterActivityModel.RefreshCount++;
		Debug.LogError(res.UserEncourageActivity.ActId+" "+_replaceActId);
		SupporterActivityModel.UpdateEncourageActs(res.UserEncourageActivity,_replaceActId);
		GlobalData.PlayerModel.UpdateUserMoney(res.UserMoney);
		
		//刷新的Item需要的数据
		View.RefreshOneActivity(SupporterActivityModel.GetUserActVo(res.UserEncourageActivity.Id));
		View.refreshCostGoldNum(SupporterActivityModel.RefreshCount);
		SdkHelper.StatisticsAgent.OnEvent("应援活动刷新", SupporterActivityModel.RefreshCount);
		//刷新
		//UpgrageUserData();
	}

	private void BuyTimeReq()
	{
		//剩余次数为0的时候不可以点击！
		LoadingOverlay.Instance.Show();
		NetWorkManager.Instance.Send<BuyInterCountRes>(CMD.SUPPORTERACTIVITY_BUYINNER,null,BuyTimeOnReq);

	}

//	private void BuyErrorCallBack(HttpErrorVo res)
//	{
//		if (ClientData.ErrorCodeDict.ContainsKey(res.ErrorCode))
//		{
//			FlowText.ShowMessage(ClientData.ErrorCodeDict[res.ErrorCode]);
//		}
//		else
//		{
//			FlowText.ShowMessage("error:"+res.ErrorCode);
//		}
//	}

	private void BuyTimeOnReq(BuyInterCountRes res)
	{
		LoadingOverlay.Instance.Hide();
		SupporterActivityModel.hasBuyTime = res.BuyInterCount;
		SupporterActivityModel.leftBuyTime = res.LeftInterCount;
		GlobalData.PlayerModel.UpdateUserMoney(res.UserMoney);
		RefreshBuyTime(res.LeftInterCount);
		SdkHelper.StatisticsAgent.OnPurchase("应援活动次数购买", res.BuyInterCount,res.UserMoney.Gem);
	}
	
	

	public override void Destroy()
	{
		base.Destroy();
//		EventDispatcher.RemoveEvent(EventConst.UpdateEnergy);
		EventDispatcher.TriggerEvent<bool>(EventConst.ChangeTopPower,false);
	}
}