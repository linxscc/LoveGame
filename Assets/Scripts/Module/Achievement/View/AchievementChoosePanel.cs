using Assets.Scripts.Common;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Interfaces;
using UnityEngine;

namespace game.main
{
	public class  AchievementChoosePanel : ReturnablePanel
	{
		private AchievementChooseController _achievementChooseController;
		//private bool _jumpFromPhone=false;
    
    
		public override void Init(IModule module)
		{
			base.Init(module);
			AchievementChooseView achievementChooseView = (AchievementChooseView)InstantiateView<AchievementChooseView>("Achievement/Prefabs/AchievementChooseView");
			_achievementChooseController=new AchievementChooseController();
			_achievementChooseController.View = achievementChooseView;
			RegisterController(_achievementChooseController);
			_achievementChooseController.Start();
		}



		public override void Hide()
		{
       
		}

		public override void OnBackClick()
		{
			base.OnBackClick();
		}

		public void OnShowChooseView()
		{
//			Debug.LogError("HERE?");
			_achievementChooseController.SetViewData();
		}
	

		public override void Show(float delay)
		{
			base.Show(0);
			Main.ChangeMenu(MainMenuDisplayState.HideAll);
			ShowBackBtn();

		}
	}	

}
