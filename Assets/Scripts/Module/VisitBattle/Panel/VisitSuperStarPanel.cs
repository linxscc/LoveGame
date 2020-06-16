using System;
using Assets.Scripts.Common;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Interfaces;
using Module.VisitBattle.Data;
public class VisitSuperStarPanel : ReturnablePanel
{

    private VisitSuperStarController controller;

    public override void Init(IModule module)
    {
        base.Init(module);
        IView viewScript = InstantiateView<VisitSuperStarView>("VisitBattle/Prefabs/Panels/VisitSuperStar");
        RegisterView(viewScript);

        controller = new VisitSuperStarController();
        controller.View = (VisitSuperStarView)viewScript;
        RegisterController(controller);
    }

    public override void Hide()
    {
        base.Hide();

        controller.View.Hide();
    }

    public override void Destroy()
    {
        base.Destroy();
        controller.Destroy();
    }
}
