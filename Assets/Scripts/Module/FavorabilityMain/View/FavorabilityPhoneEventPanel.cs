using Assets.Scripts.Common;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Interfaces;

public class FavorabilityPhoneEventPanel : ReturnablePanel
{
	private FavorabilityPhoneEventController _favorabilityPhoneEventController;
	public bool jumpfromOther=false;
    
    
	public override void Init(IModule module)
	{
		base.Init(module);
		FavorabilityPhoneEventView _favorabilityPhoneEventView = (FavorabilityPhoneEventView)InstantiateView<FavorabilityPhoneEventView>("FavorabilityMain/Prefabs/FavorabilityPhoneEventView");
		_favorabilityPhoneEventController=new FavorabilityPhoneEventController();
		_favorabilityPhoneEventController._favorabilityPhoneEventView = _favorabilityPhoneEventView;
		RegisterController(_favorabilityPhoneEventController);
		_favorabilityPhoneEventController.Start();

	}

	public override void Hide()
	{
       base.Hide(); 
        
	}

	public override void OnBackClick()
	{
//		base.OnBackClick();
		if (jumpfromOther)
		{
			base.OnBackClick();
		}
		else
		{
			SendMessage(new Message(MessageConst.MODULE_DISIPOSITION_BACKMAINVIEW));
		}

	}

	public override void Show(float delay)
	{
		base.Show(0);
		Main.ChangeMenu(MainMenuDisplayState.HideAll);
		_favorabilityPhoneEventController.SetView();
		ShowBackBtn();
	}
}