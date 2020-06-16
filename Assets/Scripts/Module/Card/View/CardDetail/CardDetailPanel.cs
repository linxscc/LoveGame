using Assets.Scripts.Common;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Interfaces;
using Assets.Scripts.Module;
using DataModel;
using game.main;
using UnityEngine;

public class CardDetailPanel : ReturnablePanel {
	
	private CardDetailController controller;
	//public CityLevelModel cityLevelModel;
	public bool EnterFromOther=false;
	
	
	public override void Init(IModule module)
	{
		SetComplexPanel();
	    base.Init(module);

		IView viewScript = InstantiateView<CardDetailView>("Card/Prefabs/CardDetail/CardDetailView");

		controller = new CardDetailController();
		controller.view = (CardDetailView)viewScript;
		RegisterController(controller);
		//controller.cityLevelModel = cityLevelModel;
		controller.Start();
	}
	
    public override void Hide()
    {
	    controller.Hide();
    }

    public override void Show(float delay)
    {
	    controller.Show(true);
	    ShowBackBtn();
	    Main.ChangeMenu(MainMenuDisplayState.ShowTopBar);
    }

	public void OnShow()
	{
		controller.Show(false);
		ShowBackBtn();
		//关卡后需要刷新道具
		controller.UpdatePropAfterBattle();
		Main.ChangeMenu(MainMenuDisplayState.ShowTopBar);
	}
	
	/// <summary>
	/// 不重置TabBar的显示
	/// </summary>
	public void BackFromFullScreen()
	{
		controller.Show(false);
		ShowBackBtn();
		Main.ChangeMenu(MainMenuDisplayState.ShowTopBar);
	}

	public override void OnBackClick()
	{
//		Debug.LogError("DoneComeHere?");
		if (EnterFromOther)
		{
			ModuleManager.Instance.GoBack();
			EnterFromOther = false;

		}
		else
		{
			if (controller.view.CanBack)
			{
				controller.SendMessage(new Message(MessageConst.MODULE_CARD_COLLECTION_BACK_TO_CARD_LIST_VIEW));
			}

		}

	}

	public void SetData(UserCardVo userCardVo)
	{
		//保存当前的卡牌
		controller.view.SetData(userCardVo, GlobalData.PropModel);
		//controller.view.SetToggleShow(0);
	}
}
