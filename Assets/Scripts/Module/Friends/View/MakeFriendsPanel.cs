using Assets.Scripts.Common;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Interfaces;

public class MakeFriendsPanel : ReturnablePanel
{
    private MakeFriendsController _makeFriendsController;

    public override void Init(IModule module)
    {
        SetComplexPanel();
        base.Init(module);
        MakeFriendsView viewScript =
            (MakeFriendsView) InstantiateView<MakeFriendsView>("Friends/Prefabs/MakeFriendsView");
        _makeFriendsController = new MakeFriendsController();
        _makeFriendsController.MakeFriendsView = viewScript;
        RegisterController(_makeFriendsController);
        _makeFriendsController.Start();
    }

    public override void Show(float delay)
    {
        base.Show(delay);
        Main.ChangeMenu(MainMenuDisplayState.ShowTopBar);
        ShowBackBtn();
    }

    public override void OnBackClick()
    {
        SendMessage(new Message(MessageConst.MODULE_FRIENDS_GOBACK));
    }
}