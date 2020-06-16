using Assets.Scripts.Common;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Interfaces;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ActivityMonthCardPanel : Panel
{


    private ActivityMonthCardController controller;
    string path = "Activity/Prefabs/ActivityMonthCardView";
    public override void Init(IModule module)
    {
        base.Init(module);
        var viewScript = InstantiateView<ActivityMonthCardView>(path);

        controller = new ActivityMonthCardController();
        controller.View = (ActivityMonthCardView)viewScript;

        RegisterView(viewScript);
        RegisterController(controller);
        controller.Start();
    }

    public override void Show(float delay)
    {
        controller.View.Show();
        Debug.Log("OnShowMonthCardData");
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
