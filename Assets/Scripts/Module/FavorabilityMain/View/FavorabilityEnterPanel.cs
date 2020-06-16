using Assets.Scripts.Common;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Core.Events;
using Assets.Scripts.Framework.GalaSports.Interfaces;
using Common;
using game.main;

public class  FavorabilityEnternPanel : ReturnablePanel
{
    private FavorabilityEnterController _favorabilityEnterController;
    //private bool _jumpFromPhone=false;
    
    
    public override void Init(IModule module)
    {
        base.Init(module);
        FavorabilityEnterView dispositionView = (FavorabilityEnterView)InstantiateView<FavorabilityEnterView>("FavorabilityMain/Prefabs/FavorabilityChangeRole/FavorabilityChangeRoleView");
        _favorabilityEnterController=new FavorabilityEnterController();
        _favorabilityEnterController._favorabilityEnterView = dispositionView;
        RegisterController(_favorabilityEnterController);
        _favorabilityEnterController.Start();
       
    }



    public override void Hide()
    {
       
    }

    public override void OnBackClick()
    {
        base.OnBackClick();
    }


    public override void Show(float delay)
    {
        base.Show(0);
        Main.ChangeMenu(MainMenuDisplayState.HideAll);
        ShowBackBtn();
        _favorabilityEnterController.Start();
    }

    
}