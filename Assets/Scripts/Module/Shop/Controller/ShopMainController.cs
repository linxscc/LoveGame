using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Core.Events;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module.NetWork;
using Assets.Scripts.Services;
using Com.Proto;
using Common;
using Componets;
using DataModel;
using game.main;
using Google.Protobuf.Collections;
using UnityEngine;
using Utils;

public class ShopMainController : Controller
{

	public ShopMainView View;
	public ShopModel ShopModel;
	private MallItemWindow _mallItemWindow;
	private GiftPackWindow _giftPackWindow;
	private AwardWindow _awardWindow;
	private float _lastClickTime=0;
	private List<int> _tempmallidlist;
	public int TargetPage=0;

	public override void Start()
	{
		_tempmallidlist=new List<int>();
		ShopModel = GetData<ShopModel>();
		ClientData.LoadItemDescData(null);
		ClientData.LoadSpecialItemDescData(null);
		EventDispatcher.AddEventListener<UserBuyRmbMallVo>(EventConst.BuyRmbMallGift,OnRmbGiftClick);
//		EventDispatcher.AddEventListener<UserBuyRmbMallVo>(EventConst.BuyMallGift,OnMallGiftClick);
		EventDispatcher.AddEventListener<GameMallVo,UserBuyGameMallVo>(EventConst.BuyMallItem,OnMallItemClick);
		EventDispatcher.AddEventListener<GameMallVo,UserBuyGameMallVo>(EventConst.BuyMallBatchItem,OnMallBatchItemClick);
		EventDispatcher.AddEventListener<GameMallVo,int>(EventConst.BuyGoldMallItem,OnBuyGoldMallItemClick);
		EventDispatcher.AddEventListener<RepeatedField<UserBuyRmbMallPB>>(EventConst.GetPayInfoSuccess,GetPayInfoSuccess);
		EventDispatcher.AddEventListener(EventConst.RefreshGoodsItem,GetRule);
		EventDispatcher.AddEventListener<int>(EventConst.PayforGift,OnPayGiftClick);
		EventDispatcher.AddEventListener<int>(EventConst.PayforSpecialGift,OnPayGiftClick);
		EventDispatcher.AddEventListener(EventConst.UpdateShopTopBar, UpdateTopBar);
		EventDispatcher.AddEventListener<RmbMallVo>(EventConst.BuyGemMall,OnGemClick);
        GetRule();
	}

	private void OnGemClick(RmbMallVo vo)
	{
		if (Time.realtimeSinceStartup- _lastClickTime <10f&&_tempmallidlist.Contains(vo.MallId))
		{
			FlowText.ShowMessage(I18NManager.Get("Shop_DontRepeatBuy"));
			return;
		}

		if (!_tempmallidlist.Contains(vo.MallId))
		{
			_tempmallidlist.Add(vo.MallId);	
		}
		SdkHelper.PayAgent.Pay(GlobalData.PayModel.GetProduct(vo.MallId));	
		_lastClickTime = Time.realtimeSinceStartup;
	}

	private void OnPayGiftClick(int mallid)
	{
	
		if (Time.realtimeSinceStartup- _lastClickTime <10f&&_tempmallidlist.Contains(mallid))
		{
			FlowText.ShowMessage(I18NManager.Get("Shop_DontRepeatBuy"));
			return;
		}

		if (!_tempmallidlist.Contains(mallid))
		{
			_tempmallidlist.Add(mallid);	
		}
		SdkHelper.PayAgent.Pay(GlobalData.PayModel.GetProduct(mallid));
		_lastClickTime = Time.realtimeSinceStartup;
		_giftPackWindow?.Close();


	}


	public void GetRule()
	{
		//商城不出缓存
		//NetWorkManager.Instance.Send<MallRuleRes>(CMD.MALL_RULE,null,MallRuleCallBacK);
		UpdateTopBar();
		GetUserInfo();
	}
	
	private void UpdateTopBar()
	{
		View.SetTopbarData(GlobalData.PlayerModel.PlayerVo);
	}

//	private void MallRuleCallBacK(MallRuleRes res)
//	{
//		ShopModel.InitRule(res);		
//		GetUserInfo();
//	}
	
	private void GetUserInfo()
	{
		LoadingOverlay.Instance.Show();
		//NetWorkManager.Instance.Send<MallInfoRes>(CMD.MALL_USERINFO, null, MallInfoCallBack);
		GetService<ShopService>().SetCallback(MallInfoCallBack).Execute();
	}

	private void MallInfoCallBack(ShopModel shopModel)
	{
		LoadingOverlay.Instance.Hide();
		ShopModel = shopModel;
		//ShopModel.InitUserMallInfo(res);
		View.SetData(ShopModel,TargetPage);
		TargetPage = 0;
	}

	private void GetPayInfoSuccess(RepeatedField<UserBuyRmbMallPB> userBuyRmbMallPbs)
	{
		if (userBuyRmbMallPbs?.Count==0)
		{
			return;
		}
		ShopModel.UpdateUserRmbMallVo(userBuyRmbMallPbs);
		View.SetData(ShopModel,TargetPage);
		TargetPage = 0;
	}
	
	
	private void OnRmbGiftClick(UserBuyRmbMallVo vo)
	{
		if (_giftPackWindow==null)
		{
			_giftPackWindow=PopupManager.ShowWindow<GiftPackWindow>("Shop/Prefab/MallWindow/GiftPackWindow");
		}
		_giftPackWindow.SetData(vo,ShopModel.RmbMallDic[vo.MallId]);
	}
	
//	private void OnMallGiftClick(UserBuyRmbMallVo vo)
//	{
//		if (_giftPackWindow==null)
//		{
//			_giftPackWindow=PopupManager.ShowWindow<GiftPackWindow>("Shop/Prefab/MallWindow/GiftPackWindow");
//		}
//		_giftPackWindow.SetData(vo,ShopModel.RmbMallDic[vo.MallId]);
//	}

	private void OnMallItemClick(GameMallVo vo,UserBuyGameMallVo userBuyGameMallVo)
	{		
		if (_mallItemWindow==null)
		{
			_mallItemWindow=PopupManager.ShowWindow<MallItemWindow>("Shop/Prefab/MallWindow/MallItemWindow");
		}
		_mallItemWindow.SetData(vo,userBuyGameMallVo);
	}

	private void OnMallBatchItemClick(GameMallVo vo,UserBuyGameMallVo userBuyGameMallVo)
	{
		if (_mallItemWindow==null)
		{
			_mallItemWindow=PopupManager.ShowWindow<MallItemWindow>("Shop/Prefab/MallWindow/MallItemWindow");
		}
		_mallItemWindow.SetData(vo,userBuyGameMallVo);
	}


	private void OnBuyGoldMallItemClick(GameMallVo vo, int num)
	{
		LoadingOverlay.Instance.Show();
		_mallItemWindow.Close();
//		Debug.LogError(vo.MallId+" "+vo.MallType+" "+num);
		var buffer=NetWorkManager.GetByteData(new BuyGameGoodsReq(){MallId = vo.MallId,MallType = vo.MallType,Num = num});
		NetWorkManager.Instance.Send<BuyGameGoodsRes>(CMD.MALL_BUYGAMEGOODS,buffer, BuyGoldSuccessCallBack);
	}

	private void BuyGoldSuccessCallBack(BuyGameGoodsRes res)
	{
		LoadingOverlay.Instance.Hide();
		ShopModel.UpdateUserBuyGameMallVo(res.UserBuyGameMall);
		GlobalData.PlayerModel.UpdateUserMoney(res.UserMoney);
//		Debug.LogError(res.Award);
		RewardUtil.AddReward(res.Award);
		
		if (_awardWindow==null)
		{
			_awardWindow=PopupManager.ShowWindow<AwardWindow>("GameMain/Prefabs/AwardWindow/AwardWindow");
		}
		_awardWindow.SetData(res.Award);
		

		
		//统计
		GameMallVo mallVo = ShopModel.GameMallDic[res.UserBuyGameMall.MallId];
		View.SetData(ShopModel);
		GlobalData.PlayerModel.PlayerVo.HasGetFreeGemGift = ShopModel.HasFreeGemMall();
		if (mallVo.MoneyTypePb == MoneyTypePB.MoGem)
		{
			SdkHelper.StatisticsAgent.OnPurchase(mallVo.MallName, res.Award.Count, mallVo.RealPrice);
		}
		else
		{
			SdkHelper.StatisticsAgent.OnEvent(mallVo.MallName,res.Award.Count);
		}
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
			case MessageConst.CMD_REFRESHGOLDMALLITEM:
				PopupManager.ShowConfirmWindow(I18NManager.Get("Shop_NeedToRefresh")).WindowActionCallback = evt =>
				{
					if (evt==WindowEvent.Ok)
					{
						RefreshGoldMallClick();
					}
				};

				break;
			case MessageConst.CMD_REGETSHOPRULE:
				GetUserInfo();
				break;
			case MessageConst.CMD_USETASTECARD:
				//要先用复选框！
				var tasteNum = GlobalData.PropModel.GetUserProp(PropConst.TasteCardId).Num;

				if (tasteNum==0)
				{
					FlowText.ShowMessage(I18NManager.Get("Shop_NoVipTaste"));
					return;
				}
	            

				PopupManager.ShowConfirmWindow(I18NManager.Get("Shop_UseTasteCard",tasteNum,tasteNum)).WindowActionCallback = evt =>
				{
					if (evt==WindowEvent.Ok)
					{
						UseTasteCard();
					}
				};

				break;
			case MessageConst.CMD_BUYMONTHCARD:
				SdkHelper.PayAgent.PayMonthCard();
				break;
			case MessageConst.CMD_MALL_DAILYGEMREWARD:
				GetMonthCardReward();
				break;
			case MessageConst.CMD_MAIN_SHOW_BUY_POWER:     //买体力
				var _buyPowerUpperlimit = GlobalData.PlayerModel.BuyPowerUpperlimit; //10
				if (GlobalData.PlayerModel.PlayerVo.PowerNum >= _buyPowerUpperlimit)
				{
					FlowText.ShowMessage(I18NManager.Get("Common_TodaysBuyUpperlimit"));// ("今日兑换次数已达上限");
				}
				else
				{
					ShowBuyPowerWindow();
				}
				break;
                
			case MessageConst.CMD_MAIN_SHOW_BUY_GOLD:    //买金币
				var _buyGlodUpperlimit = GlobalData.PlayerModel.BuyGoldUpperlimit;  //10
				if (GlobalData.PlayerModel.PlayerVo.GoldNum >= _buyGlodUpperlimit)
				{
					FlowText.ShowMessage(I18NManager.Get("Common_TodaysBuyUpperlimit"));
					return;
				}
				else {
					ShowBuyGlodWindow();
				}
				break;
			
			
        }
	}
	
	/// <summary>
	/// 显示兑换金币窗口
	/// </summary>
	private void ShowBuyGlodWindow()
	{
		QuickBuy.BuyGlodOrPorwer(PropConst.GoldIconId, PropConst.GemIconId);
	}


	/// <summary>
	/// 显示兑换体力窗口
	/// </summary>
	private void ShowBuyPowerWindow()
	{
		QuickBuy.BuyGlodOrPorwer(PropConst.PowerIconId, PropConst.GemIconId);
	}

	private void UseTasteCard()
	{
		var buffer = NetWorkManager.GetByteData(new UseVipExperienceReq{ItemId = PropConst.TasteCardId});	
		NetWorkManager.Instance.Send<UseVipExperienceRes>(CMD.MonthCard_UseTasteCard, buffer,GetTasteCardCallBack);
	}

	private void GetTasteCardCallBack(UseVipExperienceRes res)
	{
		FlowText.ShowMessage(I18NManager.Get("Shop_UseOneTasteCard"));
		GlobalData.PropModel.UpdateProps(new []{res.UserItem});
		GlobalData.PlayerModel.PlayerVo.UserMonthCard = res.UserMonthCard;
        EventDispatcher.TriggerEvent(EventConst.RefreshActivityImageAndActivityPage);
        //刷新UI！

    }
	
	private void RefreshGoldMallClick()
	{
		NetWorkManager.Instance.Send<RefreshGoldMallRes>(CMD.MALL_REFRESHGOLDMALL,null,RefreshGoldMallReq);
	}

	private void RefreshGoldMallReq(RefreshGoldMallRes res)
	{
		ShopModel.UpdateUserBuyGameMallVo(res.UserBuyGameMall);
		ShopModel.UserBuyMallInfoPb = res.UserBuyMallInfo;
		GlobalData.PlayerModel.UpdateUserMoney(res.UserMoney);
		FlowText.ShowMessage(I18NManager.Get("Shop_RefreshTips"));
        //刷新UI！
       
        View.SetData(ShopModel);
	}
	
	private void GetMonthCardReward()
	{
		
		NetWorkManager.Instance.Send<ReceiveMonthCardGemRes>(CMD.MonthCard_ReveiveDailyGem, null,GetMonthCardRewardCallBack);
	}

	private void GetMonthCardRewardCallBack(ReceiveMonthCardGemRes res)
	{
		//更新各种状态啊！
		FlowText.ShowMessage(I18NManager.Get("Shop_GetDailyGem"));
		GlobalData.PlayerModel.PlayerVo.UserMonthCard = res.UserMonthCard;
		RewardUtil.AddReward(res.Award);
		View.SetData(ShopModel);
	}


	public override void Destroy()
	{
		base.Destroy();
		//ClientData.Clear();
		EventDispatcher.RemoveEventListener<RmbMallVo>(EventConst.BuyGemMall,OnGemClick);
		EventDispatcher.RemoveEventListener<UserBuyRmbMallVo>(EventConst.BuyRmbMallGift,OnRmbGiftClick);
//		EventDispatcher.RemoveEventListener<UserBuyRmbMallVo>(EventConst.BuyMallGift,OnMallGiftClick);
		EventDispatcher.RemoveEventListener<int>(EventConst.PayforSpecialGift,OnPayGiftClick);
		EventDispatcher.RemoveEventListener<GameMallVo,UserBuyGameMallVo>(EventConst.BuyMallItem,OnMallItemClick);
		EventDispatcher.RemoveEventListener<GameMallVo,UserBuyGameMallVo>(EventConst.BuyMallBatchItem,OnMallBatchItemClick);
		EventDispatcher.RemoveEventListener<GameMallVo,int>(EventConst.BuyGoldMallItem,OnBuyGoldMallItemClick);
		EventDispatcher.RemoveEventListener<RepeatedField<UserBuyRmbMallPB>>(EventConst.GetPayInfoSuccess,GetPayInfoSuccess);
		EventDispatcher.RemoveEventListener(EventConst.RefreshGoodsItem,GetRule);
		EventDispatcher.RemoveEventListener<int>(EventConst.PayforGift,OnPayGiftClick);
		EventDispatcher.RemoveEventListener(EventConst.UpdateShopTopBar, UpdateTopBar);
	}
}