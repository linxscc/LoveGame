using Assets.Scripts.Common;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Core.Events;
using Assets.Scripts.Framework.GalaSports.Interfaces;
using Common;
using DataModel;
using game.main;
using Module.Card.Controller;
using UnityEngine;

public class SleepChoosePanel : ReturnablePanel
{
    private SleepController _controller;
    public override void Init(IModule module)
    {
        base.Init(module);

        var viewScript = InstantiateView<SleepChooseView>("Sleep/Prefabs/SleepChooseView");
        _controller = new SleepController { View = viewScript };
        RegisterView(viewScript);
        RegisterController(_controller);
        RegisterModel<SleepModel>();
        _controller.Start();

    }

    public override void Hide()
    {
        _controller.View.gameObject.Hide();
    }
}
