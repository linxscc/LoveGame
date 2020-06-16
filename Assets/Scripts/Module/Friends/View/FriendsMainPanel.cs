using Assets.Scripts.Common;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Interfaces;

public class FriendsMainPanel : ReturnablePanel
{
    private FriendsMainController _friendsMainController;

    public override void Init(IModule module)
    {
        SetComplexPanel();
        base.Init(module);
        FriendsMainView viewScript = (FriendsMainView) InstantiateView<FriendsMainView>("Friends/Prefabs/FriendsMain");
        _friendsMainController = new FriendsMainController();
        _friendsMainController.FriendsMainView = viewScript;
        RegisterController(_friendsMainController);
        _friendsMainController.Start();
    }

    public override void Show(float delay)
    {
        base.Show(delay);
        Main.ChangeMenu(MainMenuDisplayState.ShowTopBar);
        ShowBackBtn();
    }

    public void GoBackRefresh()
    {
        _friendsMainController.RefreshInfo();
    }
    
    public override void OnBackClick()
    {
        ModuleManager.Instance.GoBack();
        
    }
}