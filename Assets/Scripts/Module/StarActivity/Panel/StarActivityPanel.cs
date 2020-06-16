using Assets.Scripts.Common;
using Assets.Scripts.Framework.GalaSports.Interfaces;

	public class StarActivityPanel : ReturnablePanel
	{

		private StarActivityController _controller;
		
		
		public override void Init(IModule module)
		{
			base.Init(module);

			StarActivityView viewScript = InstantiateView<StarActivityView>("StarActivity/Prefabs/StarActivityView");
			_controller = new StarActivityController {View = viewScript};

			RegisterView(viewScript);
			RegisterController(_controller);
			
			_controller.Start();

		}
		
		public void GuideIsShowBackBtnAndTopBar(bool isShow)
		{
			if (isShow)
			{
				ShowBackBtn();
				Main.ChangeMenu(MainMenuDisplayState.ShowTopBar);
			}
			else
			{
				HideBackBtn();
				Main.ChangeMenu(MainMenuDisplayState.HideAll);
			}
		}


		public override void Show(float delay)
		{
			base.Show(delay);
			Main.ChangeMenu(MainMenuDisplayState.ShowTopBar);
			ShowBackBtn();
			_controller.OnShow();
		}

	
		
	}

