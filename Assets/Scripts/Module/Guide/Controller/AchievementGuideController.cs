using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Module;
using Assets.Scripts.Module.Guide.ModuleView;
using Com.Proto;
using Common;
using Module.Guide.ModuleView.Supporter;
using UnityEngine;

namespace Game.Guide
{
	public class AchievementGuideController : Controller
	{
		public AchievementGuideView View;

		/// <summary>
		/// 处理View消息
		/// </summary>
		/// <param name="message"></param>
		public override void OnMessage(Message message)
		{
			string name = message.Name;
			object[] body = message.Params;
			switch (name)
			{
				case MessageConst.TO_GUIDE_ACHIEVEMENT_NEXT_STEP:
				case MessageConst.TO_GUIDE_ACHIEVEMENT_RESET:
					View.Step3();
					break;
			}
		}
	}

}

