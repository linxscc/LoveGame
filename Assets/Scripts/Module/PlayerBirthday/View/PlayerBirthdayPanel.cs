
using Assets.Scripts.Common;
using Assets.Scripts.Framework.GalaSports.Interfaces;


public class PlayerBirthdayPanel : ReturnablePanel
{

	private PlayerBirthdayController _controller;

	public override void Init(IModule module)
	{
		base.Init(module);
		
		var viewScript = InstantiateView<PlayerBirthdayView>("PlayerBirthday/Prefabs/PlayerBirthdayView");
		_controller = new PlayerBirthdayController {View = viewScript};
		RegisterView(viewScript);
		RegisterController(_controller);
	}
	
	public override void Show(float delay)
	{
		base.Show(delay);
		Main.ChangeMenu(MainMenuDisplayState.ShowTopBar);
		ShowBackBtn();
		_controller.OnShow();
	}

	public void IsShowBackBtnAndTopBar(bool isShow)
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
}
