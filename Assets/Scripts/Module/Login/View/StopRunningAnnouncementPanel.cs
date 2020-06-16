using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Interfaces;


public class StopRunningAnnouncementPanel : Panel
{
	private StopRunningAnnouncementController _controller;

	public override void Init(IModule module)
	{
		base.Init(module);
		var viewScript = InstantiateView<StopRunningAnnouncementView>("Login/Prefabs/AnnouncementPanel");
		
		_controller = new StopRunningAnnouncementController();
		_controller.View = (StopRunningAnnouncementView) viewScript;
		
		RegisterView(viewScript);
		RegisterController(_controller);
		_controller.Start();
	}
	
	public override void Show(float delay)
	{
		base.Show(delay);
	}


	public override void Hide()
	{
		base.Hide();
	}

}
