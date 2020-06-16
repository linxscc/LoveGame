using Assets.Scripts.Framework.GalaSports.Interfaces;
using Assets.Scripts.Common;
using Assets.Scripts.Framework.GalaSports.Core;

public class ExchangeShopPanel : ReturnablePanel
{


	private ExchangeShopController _controller;
	private string Path ="TrainingRoom/Prefabs/ExchangeShopView";
	
	public override void Init(IModule module)
	{
		base.Init(module);
		base.Init(module);
		base.Init(module);
		var viewScript = InstantiateView<ExchangeShopView>(Path);
		
		_controller =new ExchangeShopController();
		_controller.View = viewScript;
		
		RegisterView(viewScript);
		RegisterController(_controller);
		RegisterModel<ExchangeShopModel>();	
		
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
