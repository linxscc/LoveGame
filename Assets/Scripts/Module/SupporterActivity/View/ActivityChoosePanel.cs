using Assets.Scripts.Common;
using Assets.Scripts.Framework.GalaSports.Interfaces;
using game.main;
using UnityEngine;

public class ActivityChoosePanel : ReturnablePanel {


	private ActivityChooseView _activityChooseView;
	public ActivityChooseController _activityChooseController;
	private PlayerPB _currentTab = PlayerPB.None;
    
	public override void Init(IModule module)
	{
		SetComplexPanel();//待研究这个方法
		base.Init(module);
		_activityChooseView = (ActivityChooseView)InstantiateView<ActivityChooseView>("SupporterActivity/Prefabs/ActivityChooseView");

		if (_activityChooseView==null)
		{
			//Debug.LogError("_activityChooseView==null");
		}
		
		_activityChooseController=new ActivityChooseController();
		RegisterController(_activityChooseController);
		_activityChooseController.View = _activityChooseView;
		_activityChooseController.Start();

	}
	
	public override void Show(float delay)
	{
		base.Show(0);
		Main.ChangeMenu(MainMenuDisplayState.ShowTopBar);
		_activityChooseView.gameObject.Show();
		ShowBackBtn();
	}

	public void GuideToFansDetail()
	{

		_activityChooseView.GoToFansModule();

	}

	public void SetUpgrageData(int lefttime)
	{
		//pb?
		_activityChooseController.RefreshBuyTime(lefttime);
		
		//bug 这种更新全部的方式是错的！
		_activityChooseController.RefreshStartItem();
		//_activityChooseController.UpgrageUserData(true);
	}

	public void SetActChange(UserEncourageActVo vo)
	{
		_activityChooseController.OnRefeshReq(vo);
	}
	
	public override void Hide()
	{
		_activityChooseView.gameObject.Hide();
	}

	
	public void BackView()
	{
	}
}
