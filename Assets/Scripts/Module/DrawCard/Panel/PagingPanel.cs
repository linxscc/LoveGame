using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Interfaces;
using UnityEngine;

public class PagingPanel : Panel
{
	private PagingController _control;
	public bool isExist=false;
	public override void Init(IModule module)
	{
		if (isExist == false)
		{
			base.Init(module);
			IView viewScript = InstantiateView<PagingView>("DrawCard/Prefabs/PagingView");

			_control = new PagingController();
			_control.View = (PagingView) viewScript;
			_control.Start();

			RegisterController(_control);
		}
		else
		{
			_control.View.Show();
		}

		
	}
}
