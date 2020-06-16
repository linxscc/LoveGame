using Assets.Scripts.Common;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivityDrawCardPanel : Panel
{
    private ActivityDrawCardController controller;
    string path = "Activity/Prefabs/ActivityDrawCardView";
    public override void Init(IModule module)
    {
        base.Init(module);
        var viewScript = InstantiateView<ActivityDrawCardView>(path);

        controller = new ActivityDrawCardController();
        controller.View = (ActivityDrawCardView)viewScript;
        
        RegisterView(viewScript);
        RegisterController(controller);
        controller.Start();
    }

    public override void Show(float delay)
    {
        base.Show(delay);
        controller.View.Show();
        Main.ChangeMenu(MainMenuDisplayState.ShowTopBar);
    }

    public override void Hide()
    {
        base.Hide();
        controller.View.Hide();
    }

    public override void Destroy()
    {
        base.Destroy();
    }
}
