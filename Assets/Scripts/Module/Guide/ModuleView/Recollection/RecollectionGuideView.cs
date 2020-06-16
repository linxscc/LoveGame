using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module;
using Assets.Scripts.Module.Guide;
using Assets.Scripts.Module.NetWork;
using Com.Proto;
using Common;
using DataModel;
using DG.Tweening;
using game.main;
using game.tools;
using GalaAccount.Scripts.Framework.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Module.Guide.ModuleView.Recollection
{

    public class RecollectionGuideView : View
    {
	    private readonly float _samllScale = 0f;

	    private void Awake()
	    {
		    transform.Find("Step4/GetAward").localScale =new Vector3(_samllScale,_samllScale,1f);
		    ClientData.LoadItemDescData(null);
		    ClientData.LoadSpecialItemDescData(null);
	    }

	    public void Step1()
		{									
			Transform step1 = transform.Find("Step1");
			step1.gameObject.Show();

			step1.GetText("GuideView/DialogFrame/Text").text = I18NManager.Get("Guide_RecollectionGuideStep1");
			PointerClickListener.Get(step1.gameObject).onClick = go =>
			{
				step1.gameObject.Hide();
				Step2();
			};
//
//			var arrowsRect = step1.GetRectTransform("WhiteBottomBg/Panel/Arrows");
//			var localPosition = arrowsRect.localPosition;
//			arrowsRect.DOLocalMove(new Vector2(localPosition.x+30f,localPosition.y-30f),0.5f).SetLoops(-1, LoopType.Yoyo);
//
//			var onClickArea = step1.Find("WhiteBottomBg/Panel/OnClickArea").gameObject;
//			PointerClickListener.Get(onClickArea).onClick = go =>
//			{				
//				SendMessage(new Message(MessageConst.MODULE_RECOLLECTION_SHOW_CHOOSE_CARD, Message.MessageReciverType.UnvarnishedTransmission));				
//				step1.gameObject.Hide();				
//				Step2();
//			};

		}



		private void Step2()
		{
			Transform step2 = transform.Find("Step2");
			step2.gameObject.Show();
			step2.GetText("GuideView/DialogFrame/Text").text = I18NManager.Get("Guide_RecollectionGuideStep2");

			var choosePhotoOnClick = step2.Find("PhotoBg/ChoosePhotoOnClick");
			GuideArrow.DoAnimation(choosePhotoOnClick);
			
			PointerClickListener.Get(choosePhotoOnClick.gameObject).onClick = go =>
			{
				SendMessage(new Message(MessageConst.MODULE_RECOLLECTION_SHOW_CHOOSE_CARD, Message.MessageReciverType.UnvarnishedTransmission));		
				step2.gameObject.Hide();
				Step3();
			};

//			Transform step2 = transform.Find("Step2");
//			step2.gameObject.Show();
//						
//			var arrowsRect = step2.GetRectTransform("Arrows");
//			var localPosition = arrowsRect.localPosition;
//			arrowsRect.DOLocalMoveX(localPosition.x + 30.0f, 0.5f).SetLoops(-1, LoopType.Yoyo);
//					
//			var onClickArea = step2.Find("OnClickArea").gameObject;
//			PointerClickListener.Get(onClickArea).onClick = go =>
//			{				
//				UserCardVo vo = GlobalData.CardModel.UserCardList[0];							
//				SendMessage(new Message(MessageConst.MODULE_CARD_SHOW_SELECTED_CARD, Message.MessageReciverType.UnvarnishedTransmission, vo));				
//				step2.gameObject.Hide();
//				Step3();
//
//			};
		}


		private void Step3()
		{
			Transform step3 = transform.Find("Step3");
			step3.gameObject.Show();

			var onClickArea = step3.Find("OnClickArea");
			GuideArrow.DoAnimation(onClickArea);
			PointerClickListener.Get(onClickArea.gameObject).onClick = go =>
			{
				
				UserCardVo vo = GlobalData.CardModel.UserCardList[0];
				SendMessage(new Message(MessageConst.MODULE_CARD_SHOW_SELECTED_CARD, Message.MessageReciverType.UnvarnishedTransmission, vo));
				//打星缘回忆的点
				GuideManager.SetRemoteGuideStep(GuideTypePB.CardMemoriesGuide,1020);
				step3.gameObject.Hide();
				Step5();
			};

//			
//			var arrowsRect = step3.GetRectTransform("WhiteBottomBg/Arrows");
//			var localPosition = arrowsRect.localPosition;
//			arrowsRect.DOLocalMove(new Vector2(localPosition.x - 30.0f, localPosition.y + 30.0f), 0.5f).SetLoops(-1, LoopType.Yoyo);
//			
//			var onClickArea = step3.Find("WhiteBottomBg/OnClickArea").gameObject;
//			PointerClickListener.Get(onClickArea).onClick = go =>
//			{
//				
//				//打星缘回忆的点
//				
//				GuideManager.SetRemoteGuideStep(GuideTypePB.CardMemoriesGuide,1020);
//				
//				//发送完三次的消息
//				UserCardVo vo = GlobalData.CardModel.UserCardList[0]; 		
//				SendMessage(new Message(MessageConst.MODULE_RECOLLECTION_PLAY, Message.MessageReciverType.UnvarnishedTransmission, 3, vo));								
//				step3.gameObject.Hide();		
//
//			};

		}

		private void Step4()
		{
			SetShowAward();
			Transform step4 = transform.Find("Step4");
			step4.gameObject.Show();
			var okBtn = step4.GetButton("GetAward/OkBtn");			
			var getAward = step4.Find("GetAward");
			getAward.DOScale(Vector3.one, 0.2f).SetEase(Ease.OutExpo).OnComplete(() =>
			{
				okBtn.onClick.AddListener(() =>
					{
						getAward.DOScale(new Vector3(_samllScale, _samllScale, 1), 0.2F).SetEase(Ease.OutExpo).onComplete =
							Step5;
					});
			});
		}


		private void SetShowAward()
		{
			var parent = transform.Find("Step4/GetAward/Awards");		
			var list = GuideManager.GetGuideAwardsToRule(GuideTypePB.CardMemoriesGuide);
			for (int i = 0; i < list.Count; i++)
			{								
				RewardVo vo = new RewardVo(list[i]);
				var item= parent.GetChild(i);
				item.transform.GetText("PropNameText").text = vo.Name;
				item.transform.GetText("Num").text = vo.Num.ToString();
				item.transform.GetRawImage("PropImage").texture = ResourceManager.Load<Texture>(vo.IconPath,ModuleName);
			
				PointerClickListener.Get(item.gameObject).onClick = go =>
				{
					
					var desc = ClientData.GetItemDescById(vo.Id,vo.Resource);
					if (desc!=null)
					{
						FlowText.ShowMessage(desc.ItemDesc); 			
					}
				};
			}
		}
		
		private void Step5()
		{
			
			Transform step4 = transform.Find("Step4");
			step4.gameObject.Hide();
			
			Transform step5 = transform.Find("Step5");
			step5.gameObject.Show();
			step5.GetText("GuideView/DialogFrame/Text").text = I18NManager.Get("Guide_RecollectionGuideStep4");
			var rightBtn = step5.Find("BtnContent/RightBtn");
			GuideArrow.DoAnimation(rightBtn);
			PointerClickListener.Get(rightBtn.gameObject).onClick = go =>
			{
				
				UserCardVo vo = GlobalData.CardModel.UserCardList[0];
				SendMessage(new Message(MessageConst.MODULE_RECOLLECTION_PLAY, Message.MessageReciverType.UnvarnishedTransmission, 3, vo));		
				step5.gameObject.Hide();
				gameObject.Hide();
			};


//			
//			var arrowsRect = step4.GetRectTransform("WhiteBottomBg/Panel/Arrows");
//			var localPosition = arrowsRect.localPosition;
//			arrowsRect.DOLocalMove(new Vector2(localPosition.x + 30.0f, localPosition.y -30.0f), 0.5f).SetLoops(-1, LoopType.Yoyo);
//			
//			var onClickArea = step4.Find("WhiteBottomBg/Panel/OnClickArea").gameObject;
//			PointerClickListener.Get(onClickArea).onClick = go =>
//			{
//				UserCardVo vo = GlobalData.CardModel.UserCardList[0];  
//				SendMessage(new Message(MessageConst.MODULE_RECOLLECTION_SHOW_CARD_DROP_PROP, Message.MessageReciverType.UnvarnishedTransmission,
//					vo.CardVo.RecollectionDropItemId));
//
//				step4.gameObject.Hide();
//			};

		}


		public void Step6()
		{	
			gameObject.Show();
			Transform step6 = transform.Find("Step6");
			step6.gameObject.Show();
			step6.GetText("GuideView/DialogFrame/Text").text = I18NManager.Get("Guide_RecollectionGuideStep5");

			var addBtn = step6.Find("PhotoBg/CardContent/Num/AddIcon/AddBtn");
			GuideArrow.DoAnimation(addBtn);

			PointerClickListener.Get(addBtn.gameObject).onClick = go =>
			{
				
				GuideManager.SetRemoteGuideStep(GuideTypePB.CardMemoriesGuide,1030);
				 //防止网络异常先模拟数据
                UserGuidePB userGuide = new UserGuidePB()
                {
                    GuideId = 1030,
                    GuideType = GuideTypePB.CardMemoriesGuide
                };
                GuideManager.UpdateRemoteGuide(userGuide);
				
				
				//发消息去点那个增加的窗口
				UserCardVo vo = GlobalData.CardModel.UserCardList[0];
				SendMessage(new Message(MessageConst.CMD_RECOLLECTION_BUY_COUNT, Message.MessageReciverType.UnvarnishedTransmission,vo.CardId));
			
				step6.gameObject.Hide();
				gameObject.Hide();
			};
//			
//			var arrowsRect = step5.GetRectTransform("WhiteBottomBg/Panel/Arrows");
//			var localPosition = arrowsRect.localPosition;
//			arrowsRect.DOLocalMove(new Vector2(localPosition.x + 30.0f, localPosition.y -30.0f), 0.5f).SetLoops(-1, LoopType.Yoyo);
//			
//			var onClickArea = step5.Find("WhiteBottomBg/Panel/OnClickArea").gameObject;
//			PointerClickListener.Get(onClickArea).onClick = go =>
//			{
//				SendMessage(new Message(MessageConst.CMD_RECOLLECTION_SHOW_REWARD, Message.MessageReciverType.UnvarnishedTransmission));
//				gameObject.Hide();
//			};
		}




		public void Step7()
		{
			gameObject.Show();
			Transform step7 = transform.Find("Step7");
			step7.gameObject.Show();			
			step7.GetText("GuideView/DialogFrame/Text").text = I18NManager.Get("Guide_RecollectionGuideStep6");

			var propIcon = step7.Find("PhotoBg/CardContent/CardHint/PropIcon");
			GuideArrow.DoAnimation(propIcon);

			PointerClickListener.Get(propIcon.gameObject).onClick = go =>
			{
				GuideManager.SetRemoteGuideStep(GuideTypePB.CardMemoriesGuide,1040);
				
			
				//防止网络异常先模拟数据
				UserGuidePB userGuide = new UserGuidePB()
				{
					GuideId = 1040,
					GuideType = GuideTypePB.CardMemoriesGuide
				};
				GuideManager.UpdateRemoteGuide(userGuide);
				
				//发消息去点那个其他弹窗
				UserCardVo vo = GlobalData.CardModel.UserCardList[0];
				SendMessage(new Message(MessageConst.MODULE_RECOLLECTION_SHOW_CARD_DROP_PROP,
					Message.MessageReciverType.UnvarnishedTransmission, vo.CardVo.RecollectionDropItemId));
				step7.gameObject.Hide();
				gameObject.Hide();
			};
		}
		
    }
}


