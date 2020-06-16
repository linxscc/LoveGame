using Assets.Scripts.Common;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Interfaces;
using System;
using System.Collections.Generic;
using UnityEngine;


public class ActivityGrowthCapitalPanel : Panel {

    private ActivityGrowthCapitalController controller;
    string path = "Activity/Prefabs/ActivityGrowthCapitalView";
  
   
    public override void Init(IModule module)
    {
        base.Init(module);
        var viewScript = InstantiateView<ActivityGrowthCapitalView>(path);

        controller = new ActivityGrowthCapitalController();
        controller.View = (ActivityGrowthCapitalView)viewScript;

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
