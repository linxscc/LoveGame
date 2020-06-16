using Assets.Scripts.Framework.GalaSports.Interfaces;
using Assets.Scripts.Common;
using Assets.Scripts.Framework.GalaSports.Core;

public class RankingPanel : ReturnablePanel
{
	private RankingController _controller;
	
	public override void Init(IModule module)
	{
		base.Init(module);
		base.Init(module);
		var viewScript = InstantiateView<RankingView>("TrainingRoom/Prefabs/Rank/RankingView");
		
		_controller =new RankingController();
		_controller.View = viewScript;
		
		RegisterView(viewScript);
		RegisterController(_controller);
		RegisterModel<RankingModel>();
		
		_controller.Start();
	}
	
	public override void OnBackClick()
	{     
		Destroy();
		SendMessage(new Message(MessageConst.MODULE_TRAININGROOM_SHOW_BACKBTN));
	}


	public override void Show(float delay)
	{		
		_controller.View.Show();
	}
}
