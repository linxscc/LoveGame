using Assets.Scripts.Common;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Interfaces;
using DataModel;
using System;
using System.Collections.Generic;
using UnityEngine;

public class TrainingRoomPanel : ReturnablePanel
{


	private TrainingRoomController _controller;

	private string Path = "TrainingRoom/Prefabs/TrainingRoomView";
	
	
	public override void Init(IModule module)
	{
		base.Init(module);
		var viewScript = InstantiateView<TrainingRoomView>(Path);
		
		_controller =new TrainingRoomController();
		_controller.View = viewScript;
		
		RegisterView(viewScript);
		RegisterController(_controller);
				
		_controller.Start();

	}


	public override void Show(float delay)
	{
		base.Show(delay);
		Main.ChangeMenu(MainMenuDisplayState.ShowExchangeIntegralBar);
	
	}
	
	
	
}
