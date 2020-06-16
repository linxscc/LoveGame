using System;
using Assets.Scripts.Common;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Interfaces;
using Module.Battle.Data;

public class SuperStarPanel : ReturnablePanel
{
    private SuperStarController controller;

    public override void Init(IModule module)
    {
        base.Init(module);
        IView viewScript = InstantiateView<SuperStarView>("Battle/Prefabs/Panels/SuperStar");
        RegisterView(viewScript);
        
        controller = new SuperStarController();
        controller.View = (SuperStarView) viewScript;
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

