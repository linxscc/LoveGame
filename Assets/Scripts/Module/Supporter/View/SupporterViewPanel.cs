using Assets.Scripts.Common;
using Assets.Scripts.Framework.GalaSports.Interfaces;
using Assets.Scripts.Module.Supporter.Data;
using game.main;

public class SupporterViewPanel : ReturnablePanel
{
    private SupporterViewController _control;

    public override void Init(IModule module)
    {
        base.Init(module);

        IView supporterView = InstantiateView<SupporterFansView>("Supporter/Prefabs/SupporterView");

        _control = new SupporterViewController();
        _control.View = (SupporterFansView) supporterView;
        RegisterController(_control);		
        
        RegisterModel<SupporterModel>();

        _control.Start();
    }

    public override void Show(float delay)
    {
        _control.View.Show(delay);
        Main.ChangeMenu(MainMenuDisplayState.HideAll);
        ShowBackBtn();
    }

    public void RefreshRedPoint()
    {
        Main.ChangeMenu(MainMenuDisplayState.HideAll);
        ShowBackBtn();
        _control.View.RefreshRedPoint();
    }

    public override void Hide()
    {
        _control.View.Hide();
    }

    public override void Destroy()
    {
        base.Destroy();
    }
}