using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Interfaces;

public class ActivityTenDaysSigninPanel : Panel
{

    private ActivityTenDaysSigninController controller;
    string path = "Activity/Prefabs/ActivitySigninView";
    public override void Init(IModule module)
    {
        base.Init(module);
        var viewScript = InstantiateView<ActivityTenDaysSigninView>(path);

        controller = new ActivityTenDaysSigninController();
        controller.View = (ActivityTenDaysSigninView)viewScript;

        RegisterView(viewScript);
        RegisterController(controller);
        controller.Start();
    }

    public override void Show(float delay)
    {
       controller.View.Show();
        Main.ChangeMenu(MainMenuDisplayState.ShowTopBar);
    }

    public override void Hide()
    {
        controller.View.Hide();
    }

    public override void Destroy()
    {
        base.Destroy();
    }

}
