using System;
using Assets.Scripts.Framework.GalaSports.Core.Events;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module.Framework.Utils;
using Common;
using DataModel;
using UnityEngine;
using UnityEngine.UI;

namespace game.main
{
	public class GiftPackWindow : Window
	{
		private Text _giftname;
		private Text _giftDesc;
		private LoopHorizontalScrollRect _awardList;
		private Text _realPrice;
		private Text _oldPrice;
		private Button _buyBtn;
		private GameMallVo _curMallVo;
		private RmbMallVo _curRmbMallvo;
		private int malltype = 0;

		private Text _buyCount;
		private Text _endtimetxt;
		private GameObject _line;
		private int _leftbuyTime;
	
		private void Awake()
		{
			_giftname = transform.Find("Title/Text").GetText();
			_giftDesc = transform.Find("DescBg/DescText").GetText();
			_awardList = transform.Find("AwardItemList").GetComponent<LoopHorizontalScrollRect>();
			_realPrice = transform.Find("Price/PriceText").GetText();
			_oldPrice = transform.Find("Price/OriText").GetText();
			_buyBtn = transform.Find("BuyBtn").GetButton();
            _buyBtn.onClick.AddListener(BuyGift);
			_buyCount = transform.Find("BuyCounts").GetText();
			_endtimetxt = transform.Find("EndTime").GetText();
		
			_line=transform.Find("Price/Line").gameObject;

			_awardList.prefabName = "Shop/Prefab/MallWindow/GiftAwardItem";
			_awardList.poolSize = 2;
		}

		private void BuyGift()
		{
			//FlowText.ShowMessage(I18NManager.Get("Common_Underdevelopment"));
			if (malltype==0)
			{
				
			}
			else
			{
				//FlowText.ShowMessage("测试支付");//I18NManager.Get("Common_Underdevelopment")
				if (_leftbuyTime>0)
				{
					//var productvo = GlobalData.PayModel.GetProduct(_curRmbMallvo.MallId);
					if (_curRmbMallvo.MallLabelPb==MallLabelPB.LabelVip)
					{
						if (GlobalData.PlayerModel.PlayerVo.UserMonthCard==null)
						{
							FlowText.ShowMessage(I18NManager.Get("Pay_MonthCardCanPay"));
							return;
						}
						
						
						if (GlobalData.PlayerModel.PlayerVo.UserMonthCard != null &&
						    ClientTimer.Instance.GetCurrentTimeStamp() >
						    GlobalData.PlayerModel.PlayerVo.UserMonthCard.EndTime)
						{
							FlowText.ShowMessage(I18NManager.Get("Pay_MonthCardCanPay"));
							return;	
						}
					}
					
					
					//要做时间限制			
					if (_curRmbMallvo.Special)
					{
						EventDispatcher.TriggerEvent(EventConst.PayforSpecialGift,_curRmbMallvo.MallId);
					}
					else
					{
						EventDispatcher.TriggerEvent(EventConst.PayforGift,_curRmbMallvo.MallId);
					}

				}
				else
				{
					FlowText.ShowMessage(I18NManager.Get("Shop_BuyMax"));
				}

			}
			

		}

		public void SetData(UserBuyGameMallVo uservo,GameMallVo vo)
		{
			malltype = 0;
			_curMallVo = vo;
			_giftname.text = vo.MallName;
			_giftDesc.text = vo.MallDesc;
			_realPrice.text = vo.RealPrice.ToString();
			_oldPrice.text = I18NManager.Get("Shop_RealPrice",vo.OriginalPrice);

			_awardList.UpdateCallback = UpdateAwardItem;
			_awardList.totalCount = vo.Award.Count;
            _awardList.RefreshCells();
			_awardList.RefillCells();
		}

		public void SetData(UserBuyRmbMallVo uservo,RmbMallVo vo)
		{
			malltype = 1;
			_curRmbMallvo = vo;
			_giftname.text = vo.MallName;
			_giftDesc.text = vo.MallDesc;

			var npcId = GlobalData.PlayerModel.PlayerVo.NpcId; 
			var roleImg = transform.GetRawImage("RoleImage"+npcId);
			roleImg.texture =ResourceManager.Load<Texture>("Background/PersonIcon/Npc"+npcId);
			roleImg.gameObject.Show();
			
			var payvo = GlobalData.PayModel.GetProduct(vo.MallId);
			_realPrice.text = AppConfig.Instance.isChinese=="true"||payvo?.Curreny==Constants.CHINACURRENCY?payvo?.AreaPrice:(payvo?.Curreny+payvo?.AreaPrice);//I18NManager.Get("Shop_RealPrice",);


		
			_oldPrice.gameObject.SetActive(vo.OriginalPrice>0);
			if (vo.OriginalPrice>0)
			{
				//var realrmbpoint = payvo != null ? payvo.Price: vo.RealPrice;
////				Debug.LogError("realprice:"+realrmbpoint);
//				var originalPrice=Math.Ceiling(realrmbpoint/(vo.OriginalPrice*0.01)) ;
				_oldPrice.text =AppConfig.Instance.isChinese=="true"||payvo?.Curreny==Constants.CHINACURRENCY? I18NManager.Get("RandowEventWindow_OriginalPrice")+vo.OriginalPrice+"元":I18NManager.Get("RandowEventWindow_OriginalPrice")+payvo?.GetOriginalPrice(vo.OriginalPrice);//I18NManager.Get("Shop_RealPrice",);
			}

			if (AppConfig.Instance.isChinese=="true"||payvo?.Curreny==Constants.CHINACURRENCY)
			{
				_line.gameObject.SetActive(vo.OriginalPrice>vo.RealPrice);
			}
			else
			{
				_line.gameObject.SetActive(vo.OriginalPrice>0);
			}
			

			_leftbuyTime = vo.BuyMax - uservo.BuyNum;
			_buyCount.text = I18NManager.Get("Shop_LeftBuyCounts", _leftbuyTime, vo.BuyMax); //"剩余购买次数:" + _leftbuyTime+"/"+vo.BuyMax;
			var entime = DateUtil.GetDataTime(vo.EndTime);
			_endtimetxt.text = I18NManager.Get("Shop_GiftTimeEnd") + $"<color=#9769ac>{entime}</color>";
			_endtimetxt.gameObject.SetActive(entime.Subtract(DateUtil.GetDataTime(ClientTimer.Instance.GetCurrentTimeStamp())).Days<365);
			//_awardList.RefillCells();
			_awardList.UpdateCallback = UpdateAwardItem;
			_awardList.totalCount = vo.Award.Count;
			_awardList.RefreshCells();
		}

		private void UpdateAwardItem(GameObject go, int index)
		{
			go.GetComponent<GiftAwardItem>()
				.SetData(_curRmbMallvo != null ? _curRmbMallvo.Award[index] : _curMallVo.Award[index]);
		}
	}
	

}

