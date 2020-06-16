using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Interfaces;
using UnityEngine;

public class ActivitySevenSigninTemplatePanel : Panel
{

    private ActivitySevenSigninTemplateController _controller;

    private string path = "Activity/Prefabs/ActivitySevenSigninTemplateView";

    public override void Init(IModule module)
    {
        base.Init(module);
        var viewScript = InstantiateView<ActivitySevenSigninTemplateView>(path);
        _controller = new ActivitySevenSigninTemplateController {View = viewScript};

        RegisterView(viewScript);
        RegisterController(_controller);
        _controller.Start();
    }
    
    public override void Show(float delay)
    {
        _controller.View.Show();
        Main.ChangeMenu(MainMenuDisplayState.ShowTopBar);
    }
    
    public override void Hide()
    {
        _controller.View.Hide();
    }
}
