using System.Collections;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Core.Events;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module;
using Com.Proto;
using Common;
using DataModel;
using game.main;
using game.tools;
using Google.Protobuf.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Module.Battle.Data;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class FinalEstimateFailWindow : Window
{



	private bool _oneNum;
	private void Start()
	{
		_oneNum = true;
		Button gotoCardCollentionBtn = transform.Find("Bg/Buttons/GotoCardCollention").GetComponent<Button>();
		Button gotoSupporter = transform.Find("Bg/Buttons/GotoSupporter").GetComponent<Button>();
		Button gotoGiftPack = transform.Find("Bg/Buttons/GotoGiftPack").GetComponent<Button>();
		Button explainBtn = transform.GetButton("Bg/ExplainBtn");
		
//		if (GuideManager.IsMainGuidePass(GuideConst.MainStep_MainStory1_6_Fail) == false)
//		{
//			gotoSupporter.gameObject.Hide();
//			gotoGiftPack.gameObject.Hide();
//			SendMessage(new Message(MessageConst.TO_GUIDE_BATTLE_FAIL));
//		}
		
		//1-6之后如果用R卡打失败。应援会和甜蜜加速站会显示出。策划要改成1-12后再出现

		if (!GuideManager.IsPass1_9())
		{
			gotoSupporter.gameObject.Hide();
			gotoGiftPack.gameObject.Hide();	
		}
		
		
		
		
		gotoCardCollentionBtn.onClick.AddListener(()=>
		{
			EnterModule(ModuleConfig.MODULE_CARD);
		});
		gotoSupporter.onClick.AddListener(()=>
		{
			EnterModule(ModuleConfig.MODULE_SUPPORTER);
		});
		gotoGiftPack.onClick.AddListener(()=>
		{
			EnterModule(ModuleConfig.MODULE_SHOP);
		});

		string title = I18NManager.Get("Common_HelpExplain");
		string ruleDesc = I18NManager.Get("Battle_FailExplain");
		explainBtn.onClick.AddListener(() =>
		{
			PopupManager.ShowCommonRuleWindow(ruleDesc,title );
			//PopupManager.ShowRuleExplainWindow(title,ruleDesc,new Vector2(800,1000));
		});
	}

	
	
	private void EnterModule(string moduleName)
	{
		SendMessage(new Message(MessageConst.CMD_BATTLE_FINISH, Message.MessageReciverType.DEFAULT, moduleName));
	}
	
	protected override void OnClickOutside(GameObject go)
	{
		if (_oneNum)
		{
			SendMessage(new Message(MessageConst.CMD_BATTLE_FINISH));
			_oneNum = false;
		}
	}
	
	public void SetData(int cap)
	{
		transform.Find("Bg/StarAndGrade/Text/Text").GetComponent<Text>().text =  cap.ToString();
	
	}
}
