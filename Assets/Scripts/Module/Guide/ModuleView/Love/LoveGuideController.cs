using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core;
using Common;
using game.main;
using UnityEngine;

namespace Assets.Scripts.Module.Guide.ModuleView
{
	public class LoveGuideController : Controller
	{
		public LoveGuideView View;

		public override void OnMessage(Message message)
		{
			string name = message.Name;
			object[] body = message.Params;
			switch (name)
			{
				case MessageConst.GUIDE_LOVEAPPOINTMENT_ENTERLOVECHOOSE:
					AudioManager.Instance.PlayEffect(AudioManager.Instance.DefaultButtonEffect);
					ModuleManager.Instance.EnterModule(ModuleConfig.MODULE_LOVEAPPOINTMENT, false, true);
					break;
				case MessageConst.GUIDE_LOVEAPPOINTMENT_ENTERDAILY:
					AudioManager.Instance.PlayEffect(AudioManager.Instance.DefaultButtonEffect);
					ModuleManager.Instance.EnterModule(ModuleConfig.MODULE_LOVEDIARY, false, true);
					break;
				case MessageConst.GUIDE_LOVE_CHOOSETYPE:
					
					//Debug.LogError(GuideManager.GetRemoteGuideStep(GuideTypePB.MainGuide) < GuideConst.MainStep_Love_LoveAppointment);
					break;
				case MessageConst.GUIDE_LOVE_COAXSLEEP:
					AudioManager.Instance.PlayEffect(AudioManager.Instance.DefaultButtonEffect);
					ModuleManager.Instance.EnterModule(ModuleConfig.MODULE_COAXSLEEP, false, true);
					break;
			}
		}
	}
}