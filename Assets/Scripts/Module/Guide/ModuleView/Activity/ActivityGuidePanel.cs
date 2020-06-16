using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Interfaces;
using Module.Guide.ModuleView.Activity;
using UnityEngine;

public class ActivityGuidePanel : Panel
{
    
    
    public override void Init(IModule module)
    {
        base.Init(module);
        var view = InstantiateView<ActivityGuideView>("Guide/Prefabs/ModuleView/Activity/ActivitySevenSigninGuideView");
        var controller = new ActivityGuideController {View = view};
        RegisterController(controller);
        controller.Start();
    }
}
