using Assets.Scripts.Common;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Interfaces;

namespace game.main
{
	public class AchiementListPanel : ReturnablePanel
	{

		private AchievementListController _achievementListController;
    
		public override void Init(IModule module)
		{
			SetComplexPanel();//待研究这个方法
			base.Init(module);

			var _achievementListView = (AchievementListView) InstantiateView<AchievementListView>("Achievement/Prefabs/AchievementListView");
			_achievementListController=new AchievementListController();
			RegisterController(_achievementListController);
			_achievementListController.View = _achievementListView;
			//StartController();
			_achievementListController.Start();
		}


		public void SetRoleItem(int roleId)
		{
			_achievementListController.SetView(roleId);
			
		}
		
		public override void Show(float delay)
		{
			base.Show(0);
			Main.ChangeMenu(MainMenuDisplayState.ShowTopBar);
			ShowBackBtn();
		}

		public override void OnBackClick()
		{
			SendMessage(new Message(MessageConst.CMD_ACHIEVEMENTBACK));
		}
		
		public void RefreshMission()
		{
			_achievementListController.RefreshMission();
		}

		//参考CardCollectionPanel
		public override void Hide()
		{
		}

	}  

}

