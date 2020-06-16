using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core;
using Common;
using game.main;
using UnityEngine;

namespace Assets.Scripts.Module.Guide.ModuleView
{
	public class TakePhotosGameGuideController : Controller
	{
		public TakePhotosGameGuideView View;

		public override void OnMessage(Message message)
		{
			string name = message.Name;
			object[] body = message.Params;
			switch (name)
			{
				case MessageConst.GUIDE_TO_TAKEPHOTOSGAME_STARTGAME:
					AudioManager.Instance.PlayEffect(AudioManager.Instance.DefaultButtonEffect);
					break;
			}
		}
	}
}