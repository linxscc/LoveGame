using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirborneGameTimePanel : Panel
{
    AirborneGameTimeController _gameTimeController;
    public override void Init(IModule module)
    {
        base.Init(module);
        _gameTimeController = new AirborneGameTimeController();
        var viewScript = InstantiateView<AirborneGameTimeView>("AirborneGame/Prefabs/AirborneGameTimeView");
        _gameTimeController = new AirborneGameTimeController();
        RegisterController(_gameTimeController);
        _gameTimeController.GameTimeView = (AirborneGameTimeView)viewScript;
        _gameTimeController.Start();
    }

    public void SetTimer(AirborneGameTimer timer)
    {
        timer.AddController(_gameTimeController);
    }
}
