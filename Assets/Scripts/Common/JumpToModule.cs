using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Module;
using Com.Proto;
using Common;
using DataModel;
using game.main;
using UnityEngine;

public static class  JumpToModule
{


	public static void JumpTo(string jumpTarget, Action callBack=null,PlayerPB pb = PlayerPB.None)
	{
		//Debug.LogError("跳转字段===>"+jumpTarget);

		if (jumpTarget.Contains("music"))
		{
			  callBack?.Invoke();
			  return;
		}
		
		if (jumpTarget.Contains("Activity") && jumpTarget != "SupporterActivity")
		{
			//int activityId = int.Parse(jumpTarget.Replace("Activity", null));
			ModuleManager.Instance.EnterModule(ModuleConfig.MODULE_ACTIVITY, false, false, jumpTarget);
			callBack?.Invoke();
			return;
		}

		switch (jumpTarget)
		{
			case "BuyEnergy":
			case "BuyGold":
			case "BuyEncouragePower":
				OnJumpToBuyWindow(jumpTarget);
				break;
			case "SupporterActivity":
				var openLevel = GuideManager.GetOpenUserLevel(ModulePB.EncourageAct, FunctionIDPB.EncourageActEntry);
				if (GlobalData.PlayerModel.PlayerVo.Level < openLevel)
				{
					FlowText.ShowMessage(I18NManager.Get("Task_Levellimit",openLevel));
					return;
				}
				ModuleManager.Instance.EnterModule(jumpTarget,false,true);
				break;
			case "GameMain":
				ModuleManager.Instance.DestroyAllModuleBackToCommon();
				break;
			case "CardResolve":
				ModuleManager.Instance.EnterModule(ModuleConfig.MODULE_CARD,false,false,jumpTarget);
				break;
			case "DrawCard_Gold" :
				ModuleManager.Instance.EnterModule(ModuleConfig.MODULE_DRAWCARD,false,false,jumpTarget);
				break;
			case "DrawCard_Gem":
				ModuleManager.Instance.EnterModule(ModuleConfig.MODULE_DRAWCARD);
				break;
			case "SendGift" :
				var openmainline = GlobalData.LevelModel.FindLevel("1-9").IsPass;
				string levelmark = GuideManager.GetOpenMainStory(ModulePB.Favorability,FunctionIDPB.FavorabilityGifts);
				if (!openmainline)
				{
					FlowText.ShowMessage(I18NManager.Get("Guide_Battle6",levelmark));
					return;   
				}
				ModuleManager.Instance.EnterModule(ModuleConfig.MODULE_FAVORABILITYMAIN,false,false,jumpTarget,pb);
				break;
//			case "FavorabilityPhoneEvent":
//				ModuleManager.Instance.EnterModule(ModuleConfig.MODULE_FAVORABILITYMAIN,false,false,jumpTarget,pb);
//				break;
			case "Reloading":
				GlobalData.FavorabilityMainModel.CurrentRoleVo =GlobalData.FavorabilityMainModel.GetUserFavorabilityVo((int)pb);
				ModuleManager.Instance.EnterModule(jumpTarget,false,true);
				break;
			case "Recollection":
				openLevel=GuideManager.GetOpenUserLevel(ModulePB.CardMemories, FunctionIDPB.CardMemoriesEntry);
				if (GlobalData.PlayerModel.PlayerVo.Level < openLevel)
				{
					FlowText.ShowMessage(I18NManager.Get("Task_Levellimit",openLevel));
					return; 
				}
				ModuleManager.Instance.EnterModule(jumpTarget,false,true);
				break;
			case ModuleConfig.MODULE_VISIT:
				openLevel=GuideManager.GetOpenUserLevel(ModulePB.Visiting, FunctionIDPB.VisitingEntry);
				if (GlobalData.PlayerModel.PlayerVo.Level < openLevel)
				{
					FlowText.ShowMessage(I18NManager.Get("Task_Levellimit",openLevel));
					return; 
				}
				ModuleManager.Instance.EnterModule(jumpTarget,false,true);
				break;
			case "Shop":
				ModuleManager.Instance.EnterModule(jumpTarget,false,true);
				break;
			case "Shopping":
				ModuleManager.Instance.EnterModule(ModuleConfig.MODULE_GAME_PLAY,false,true,"Shopping");
				break;
			default:
				ModuleManager.Instance.EnterModule(jumpTarget,false,true);
				break;
		}

		callBack?.Invoke();
	}

	private static void OnJumpToBuyWindow(string str)
	{
		int temp = 0;

		switch (str)
		{
			case "BuyEnergy":
				temp = PropConst.PowerIconId;
				QuickBuy.BuyGlodOrPorwer(temp, PropConst.GemIconId);
				break;
			case "BuyGold":
				temp = PropConst.GoldIconId;
				QuickBuy.BuyGlodOrPorwer(temp, PropConst.GemIconId);
				break;
			case "BuyEncouragePower":
				temp = PropConst.EncouragePowerId;
				QuickBuy.BuyGlodOrPorwer(temp, PropConst.GemIconId);
				break;
		}
	}
	
}
