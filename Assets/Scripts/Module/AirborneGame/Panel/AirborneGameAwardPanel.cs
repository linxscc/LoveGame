using Assets.Scripts.Common;
using Assets.Scripts.Framework.GalaSports.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirborneGameAwardPanel : ReturnablePanel
{
    AirborneGameAwardController _gameAwardController;

    public override void Init(IModule module)
    {
        base.Init(module);
        var viewScript = InstantiateView<AirborneGameAwardView>("AirborneGame/Prefabs/AirborneGameAwardView");
        _gameAwardController = new AirborneGameAwardController();
        RegisterController(_gameAwardController);
        _gameAwardController.AwardView = (AirborneGameAwardView)viewScript;
        _gameAwardController.Start();
    }

    public override void Show(float delay)
    {
        base.Show(delay);
        HideBackBtn();
    }

    public override void OnBackClick()
    {
        base.OnBackClick();
    }

}
