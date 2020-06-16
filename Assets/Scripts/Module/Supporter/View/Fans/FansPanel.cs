using Assets.Scripts.Common;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Interfaces;
using Assets.Scripts.Module.Supporter.Data;
using game.main;
using Module.Supporter.Data;

public class FansPanel : ReturnablePanel 
{
	private FansController controller;

	public override void Init(IModule module)
	{
	    base.Init(module);

		IView view = InstantiateView<FansView>("Supporter/Prefabs/FansView");
		controller = new FansController();
		controller.View = (FansView)view;
		RegisterController(controller);

		controller.Start();
	}

	public override void OnBackClick()
	{
		controller.GoBack();
	}

	public override void Hide()
    {
	    controller.View.Hide();
    }

    public override void Show(float delay)
    {
	    controller.View.Show(0);
	    ShowBackBtn();
    }

}
