using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Interfaces;
using UnityEngine;

public class ActivityDeleteTestPanel : Panel {

	private ActivityDeleteTestController controller;
	string path = "Activity/Prefabs/ActivityDeleteTestView";
	
	
	public override void Init(IModule module)
	{
		base.Init(module);
		var viewScript = InstantiateView<ActivityDeleteTestView>(path);

		controller = new ActivityDeleteTestController {View = viewScript};

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
}
