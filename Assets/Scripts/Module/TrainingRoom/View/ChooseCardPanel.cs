using Assets.Scripts.Framework.GalaSports.Interfaces;
using Assets.Scripts.Common;
using Assets.Scripts.Framework.GalaSports.Core;

public class ChooseCardPanel : ReturnablePanel
{

	private ChooseCardController _controller;
	private string Path = "TrainingRoom/Prefabs/ChooseCardView";
	
	public override void Init(IModule module)
	{
		base.Init(module);

		var viewScript = InstantiateView<ChooseCardView>(Path);
		
		_controller =new ChooseCardController();
		_controller.View = viewScript;
		
		RegisterView(viewScript);
		RegisterController(_controller);
		
		_controller.Start();
	}
	
	public override void OnBackClick()
	{       	
		Destroy();
		SendMessage(new Message(MessageConst.MODULE_TRAININGROOM_SHOW_SONGCHOOSEVIEW_BACKBTN));
	}
}
