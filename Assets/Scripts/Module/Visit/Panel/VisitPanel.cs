using Assets.Scripts.Common;
using Assets.Scripts.Framework.GalaSports.Interfaces;


public class VisitPanel : ReturnablePanel
{

    private VisitController  _visitController;
    string path = "Visit/Prefabs/VisitView";

    public override void Init(IModule module)
    {
        base.Init(module);

        var viewScript = InstantiateView<VisitView>(path);
        _visitController = new  VisitController();
        _visitController.VisitView = (VisitView)viewScript;

        RegisterView(viewScript);
        RegisterController(_visitController);
        RegisterModel<VisitModel>();

        _visitController.Start();
    }

    public override void Show(float delay)
    {
        _visitController.VisitView.Show(delay);
      // Main.ChangeMenu(MainMenuDisplayState.HideAll);
        Main.ChangeMenu(MainMenuDisplayState.ShowVisitTopBar);
        ShowBackBtn();
        base.Show(delay);
    }

    public  void Refeash()
    {
        _visitController.Refeash();
    }

    public override void Hide()
    {
        _visitController.VisitView.Hide();
        base.Hide();
    }

    public override void Destroy()
    {
        base.Destroy();
    }

}
