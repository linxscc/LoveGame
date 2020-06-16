using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Common;
using Assets.Scripts.Framework.GalaSports.Interfaces;
using UnityEngine;

public class CapsuleSuperStarPanel : ReturnablePanel
{
    private CapsuleSuperStarController controller;

    public override void Init(IModule module)
    {
        base.Init(module);
        IView viewScript = InstantiateView<CapsuleSuperStarView>("ActivityCapsuleBattle/Prefabs/CapsuleSuperStar");
        RegisterView(viewScript);

        controller = new CapsuleSuperStarController {View = (CapsuleSuperStarView) viewScript};
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
