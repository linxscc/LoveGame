using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Interfaces;
using game.main;

public class AnnouncementPanel : Panel
{

    private AnnouncementController controller;

    public override void Init(IModule module)
    {
        base.Init(module);

        var viewScript = InstantiateView<AnnouncementView>("Login/Prefabs/AnnouncementPanel");

        controller = new AnnouncementController();
        controller.View = (AnnouncementView)viewScript;

        RegisterView(viewScript);
        RegisterController(controller);

        controller.Start();

    }


    public override void Show(float delay)
    {
        base.Show(delay);
    }


    public override void Hide()
    {
        base.Hide();
    }
}
