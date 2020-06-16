using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Common;
using Assets.Scripts.Framework.GalaSports.Interfaces;
using game.main;
using UnityEngine;

public class CapsuleSupporterPanel : ReturnablePanel
{
    public override void Init(IModule module)
    {
        base.Init(module);
        IView viewScript = InstantiateView<CapsuleSupporterView>("ActivityCapsuleBattle/Prefabs/CapsuleSupporter");
        var control = new CapsuleSupporterController {view = (CapsuleSupporterView) viewScript};
        RegisterController(control);
        RegisterView(viewScript);            
        control.Start();
    }
    
}
