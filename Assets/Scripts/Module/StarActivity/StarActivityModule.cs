using Assets.Scripts.Framework.GalaSports.Core;
using DataModel;
using UnityEngine;

	public class StarActivityModule : ModuleBase
	{

		private StarActivityPanel _panel;
		
		public override void Init()
		{			
			_panel =new StarActivityPanel();
			_panel.Init(this);
			_panel.Show(0.5f);
		}

		public override void OnMessage(Message message)
		{
			string name = message.Name;
			object[] body = message.Params;
			switch (name)
			{
				case MessageConst.CMD_STAR_ACTIVITY_SHOW_TOPBAR_AND_BACKBTN:
					var isShow = (bool) body[0];
					_panel.GuideIsShowBackBtnAndTopBar(isShow);
					break;
			}
		}


		public override void OnShow(float delay)
		{
			base.OnShow(delay);
			_panel.Show(0);
			
		}

		public override void SetData(params object[] paramsObjects)
		{
			if (paramsObjects.Length > 0)
			{
				var day = (int) paramsObjects[0];
				if (day>7)
				{
					day = 7;
				}

				GlobalData.MissionModel.Day = day;
			}
			
		}
	}
