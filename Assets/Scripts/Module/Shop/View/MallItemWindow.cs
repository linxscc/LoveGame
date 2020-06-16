using Assets.Scripts.Framework.GalaSports.Core.Events;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module;
using Common;
using Componets;
using DataModel;
using UnityEngine;
using UnityEngine.UI;

namespace game.main
{
	public class MallItemWindow : Window {

		private Text _haveNumText;                     //当前拥有数量
		private Text _itemNumText;                    //道具数量
		private Text itemName;
		private Text _costNum;    //花费数量
		private Text _malldesc;
		private Text _buyCountDesc;    //购买次数描述
		private RawImage _itemRawIamge;           //道具图片
		private RawImage _costIconRawIamge;           //道具图片
		private LongPressButton _addLongPressBtn;
		private LongPressButton _reduceLongPressBtn;
		private Button _buyBtn;
		private Text _buytext;
		private int _curnum=1;
		private GameMallVo _curMallVo;
		private int realprice;
		private int leftTime;

		private void Awake()
		{
			itemName = transform.Find("window/TitleText").GetText();
			_haveNumText = transform.Find("window/CurNum").GetText();
			_itemNumText = transform.Find("window/FrameImage/ItemNum").GetText();
			_costNum = transform.Find("window/Content").GetText();
			_itemRawIamge = transform.Find("window/FrameImage/ItemRawImage").GetRawImage();
			_costIconRawIamge = transform.Find("window/Content/Image").GetRawImage();
			_buyBtn = transform.Find("window/BuyBtn").GetButton();
			_buytext = transform.Find("window/BuyBtn/Text").GetText();
			_addLongPressBtn = transform.Find("window/AddBtn").GetComponent<LongPressButton>();
			_reduceLongPressBtn = transform.Find("window/ReduceBtn").GetComponent<LongPressButton>();
			_malldesc = transform.Find("window/Desc").GetText();
			_buyBtn.onClick.AddListener(BuyGoodClick);
			_buyCountDesc = transform.Find("window/BuyCount").GetText();
			_addLongPressBtn.OnDown = () =>
			{
				if (_curnum>=leftTime)//要减去已经购买的次数
				{
					return;
				}
				_curnum++; 
//				Debug.LogError(_curnum);
				_itemNumText.text =_curnum.ToString(); //到底要更新谁的显示？
				_costNum.text = I18NManager.Get("Shop_CostToBuy",_curnum*realprice); //$"是否花费  {_curnum*realprice}        购买";
			};
			_reduceLongPressBtn.OnDown = () =>
			{
				if (_curnum<=1)
				{
					return;
				}
				_curnum--;
				_itemNumText.text =_curnum.ToString(); //到底要更新谁的显示？
//				Debug.LogError(_curnum);
				_costNum.text = I18NManager.Get("Shop_CostToBuy",_curnum*realprice);
			};

		}

		private void BuyGoodClick()
		{
			//发送购买的协议。要校验数量是否正确的！长按的东西需要谨慎！
			if (leftTime>0)
			{
				EventDispatcher.TriggerEvent<GameMallVo,int>(EventConst.BuyGoldMallItem,_curMallVo,_curnum);
			}
			else
			{
				FlowText.ShowMessage(I18NManager.Get("Shop_BuyMax"));
				Close();
			}
			

		}

		public void SetData(GameMallVo vo,UserBuyGameMallVo userBuyGameMallVo)
		{
			_curMallVo = vo;
			itemName.text =vo.RealPrice > 0 ? I18NManager.Get("Shop_BuyMall",vo.MallName):  I18NManager.Get("Common_GetReward")+vo.MallName;
			var enableLongPress = vo.MallSortPb == MallSortPB.MallItem && vo.MallType == MallTypePB.MallGem;
			SetLongBtnActive(enableLongPress);//malllBatchItem来判断是否为批量道具显影长按
			leftTime = userBuyGameMallVo==null?vo.BuyMax:vo.BuyMax - userBuyGameMallVo.BuyNum;
			_buyCountDesc.text = I18NManager.Get("Shop_LeftBuyCounts2", (vo.BuyMax - userBuyGameMallVo?.BuyNum), vo.BuyMax);
			//"剩余购买次数:" + (vo.BuyMax - userBuyGameMallVo?.BuyNum) + "/" + vo.BuyMax;//
			foreach (var v in vo.Award)
			{
				if (v.Num!=0)
				{
					_malldesc.text = I18NManager.Get("Shop_GoodsDesc")+ClientData.GetItemDescById(v.ResourceId,v.Resource).ItemDesc;
					_itemNumText.text = v.Num.ToString();
					_itemRawIamge.texture=ResourceManager.Load<Texture>(GlobalData.PropModel.GetPropPath(v.ResourceId),ModuleConfig.MODULE_SHOP,true);
					switch (v.Resource)
					{
						case ResourcePB.Power:
							_haveNumText.text =
								I18NManager.Get("Shop_CurrentOwn",GlobalData.PlayerModel.PlayerVo.Energy); 
							break;
						case ResourcePB.Gold:
							_haveNumText.text =
								I18NManager.Get("Shop_CurrentOwn",GlobalData.PlayerModel.PlayerVo.Gold); 
							break;
						case ResourcePB.Fans:
							var fansvo = GlobalData.DepartmentData.GetFans(v.ResourceId);
								_haveNumText.text =I18NManager.Get("Shop_CurrentOwn",fansvo.Num); 
							_itemRawIamge.texture=ResourceManager.Load<Texture>(fansvo.FansHeadPath,ModuleConfig.MODULE_SHOP,true);
							break;
						
						default:
							_haveNumText.text = I18NManager.Get("Shop_CurrentOwn",GlobalData.PropModel.GetUserProp(v.ResourceId).Num);
							break;
						
					}
					


				}
			}
			if (enableLongPress)
			{
				_itemNumText.text =leftTime>0?1.ToString():0.ToString();
			}
//			Debug.LogError(vo.MoneyTypePb);
			_costIconRawIamge.texture=ResourceManager.Load<Texture>("Prop/particular/"+(vo.MoneyTypePb==MoneyTypePB.MoGem?PropConst.GemIconId:PropConst.GoldIconId),ModuleConfig.MODULE_SHOP,true);
			realprice = vo.RealPrice;
			_buytext.text = vo.RealPrice > 0 ? I18NManager.Get("Shop_BuyTxt") : I18NManager.Get("RandowEventWindow_Free");
			//这个应该要在长按的逻辑里面去！
			_costNum.text =I18NManager.Get("Shop_CostToBuy",_curnum*realprice);
			_costNum.gameObject.SetActive(vo.RealPrice>0);
			//还有剩余购买数量和商品描述
			
		}

		private void SetLongBtnActive(bool enable)
		{
			_addLongPressBtn.gameObject.SetActive(enable);
			_reduceLongPressBtn.gameObject.SetActive(enable);
			
		}
		
	}
	

}

