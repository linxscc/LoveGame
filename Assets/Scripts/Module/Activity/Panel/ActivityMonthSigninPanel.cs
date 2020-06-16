using Assets.Scripts.Common;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Interfaces;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ActivityMonthSigninPanel : Panel
{
    private ActivityMonthSigninController controller;
    string path = "Activity/Prefabs/ActivityMonthSigninView";

    public override void Init(IModule module)
    {
        base.Init(module);
        var viewScript = InstantiateView<ActivityMonthSigninView>(path);

        controller = new ActivityMonthSigninController();
        controller.View = (ActivityMonthSigninView)viewScript;

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
