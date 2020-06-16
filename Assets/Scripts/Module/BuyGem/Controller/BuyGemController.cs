using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Core.Events;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module.NetWork;
using Com.Proto;
using Common;
using Componets;
using DataModel;
using game.main;
using Google.Protobuf.Collections;
using UnityEngine;
using Utils;

public class BuyGemController : Controller
{

	public BuyGemView View;
	public BuyGemModel BuyGemModel;
	private GiftPackWindow _giftPackWindow;

	

	public override void Start()
	{
		BuyGemModel = GetData<BuyGemModel>();
		//这个也要加监听器！
		EventDispatcher.AddEventListener<RmbMallVo>(EventConst.BuyGemMall,OnRmbGiftClick);
		EventDispatcher.AddEventListener<RepeatedField<UserBuyRmbMallPB>>(EventConst.GetPayInfoSuccess,RefreshView);
        GetRule();
	}

	private void RefreshView(RepeatedField<UserBuyRmbMallPB> userBuyRmbMallPbs)
	{
		Debug.LogError("refresh"+GlobalData.PlayerModel.PlayerVo.FirstRecharges.Count);
		AudioManager.Instance.PlayEffect("buyGem"); 
		ClientTimer.Instance.DelayCall(GetUserInfo, 0.3f);
	}
	

	public void GetRule()
	{
		NetWorkManager.Instance.Send<MallRuleRes>(CMD.MALL_RULE,null,MallRuleCallBacK);
	}

	private void MallRuleCallBacK(MallRuleRes res)
	{
		BuyGemModel.InitRule(res);		
		GetUserInfo();
	}
	
	private void GetUserInfo()
	{
		LoadingOverlay.Instance.Show();
		NetWorkManager.Instance.Send<MallInfoRes>(CMD.MALL_USERINFO, null, MallInfoCallBack);
	}

	private void MallInfoCallBack(MallInfoRes res)
	{
		LoadingOverlay.Instance.Hide();
		BuyGemModel.InitUserMallInfo(res);
		View.SetData(BuyGemModel);
	}
	
	private void OnRmbGiftClick(RmbMallVo vo)
	{
        //可能是直接支付了，不打开这个窗口！
        //FlowText.ShowMessage(I18NManager.Get("Common_Underdevelopment"));// ("暂无支付通道");
		SdkHelper.PayAgent.Pay(GlobalData.PayModel.GetProduct(vo.MallId));	

//		if (_giftPackWindow==null)
//		{
//			_giftPackWindow=PopupManager.ShowWindow<GiftPackWindow>("Shop/Prefab/MallWindow/GiftPackWindow");
//		}
//		_giftPackWindow.SetData(vo);
	}

	private void GetMonthCardReward()
	{
		
		NetWorkManager.Instance.Send<ReceiveMonthCardGemRes>(CMD.MonthCard_ReveiveDailyGem, null,GetMonthCardRewardCallBack);
	}

	private void GetMonthCardRewardCallBack(ReceiveMonthCardGemRes res)
	{
		//更新各种状态啊！
		GlobalData.PlayerModel.PlayerVo.UserMonthCard = res.UserMonthCard;
		RewardUtil.AddReward(res.Award);
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
	            //FlowText.ShowMessage("暂无月卡购买渠道");
	            SdkHelper.PayAgent.Pay(GlobalData.PayModel.GetMonthCardProduct());
	            break;
        }
	}

	private void UseTasteCard()
	{
		var buffer = NetWorkManager.GetByteData(new UseVipExperienceReq{ItemId = PropConst.TasteCardId});		
		Debug.LogError("402");
		NetWorkManager.Instance.Send<UseVipExperienceRes>(CMD.MonthCard_UseTasteCard, buffer,GetTasteCardCallBack);
	}

	private void GetTasteCardCallBack(UseVipExperienceRes res)
	{
		FlowText.ShowMessage(I18NManager.Get("Activity_UseVipCardSuccess"));
		GlobalData.PropModel.UpdateProps(new []{res.UserItem});
		GlobalData.PlayerModel.PlayerVo.UserMonthCard = res.UserMonthCard;
	}


	public override void Destroy()
	{
		base.Destroy();
		EventDispatcher.RemoveEventListener<RmbMallVo>(EventConst.BuyGemMall,OnRmbGiftClick);
		EventDispatcher.RemoveEventListener<RepeatedField<UserBuyRmbMallPB>>(EventConst.GetPayInfoSuccess,RefreshView);

	}
}