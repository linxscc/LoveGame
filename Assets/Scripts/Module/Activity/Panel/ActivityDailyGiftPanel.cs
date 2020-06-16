using Assets.Scripts.Common;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Interfaces;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ActivityDailyGiftPanel : Panel
{


	private ActivityDailyGiftController controller;
	string path = "Activity/Prefabs/ActivityDailyGiftView";
	public override void Init(IModule module)
	{
		base.Init(module);
		var viewScript = InstantiateView<ActivityDailyGiftView>(path);

		controller = new ActivityDailyGiftController();
		controller.View = (ActivityDailyGiftView)viewScript;

		RegisterView(viewScript);
		RegisterController(controller);
		controller.Start();
	}

	public override void Show(float delay)
	{
		controller.View.Show();
		Main.ChangeMenu(MainMenuDisplayState.ShowTopBar);
	}

	public override void Hide()
	{
		controller.View.Hide();
	}

	public override void Destroy()
	{
		base.Destroy();
	}
}
