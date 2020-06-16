using Assets.Scripts.Common;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public class AirborneGamePausePanel : ReturnablePanel

public class AirborneGamePausePanel : ReturnablePanel
{
    AirborneGamePauseController _gamePauseController;

    public override void Init(IModule module)
    {
        base.Init(module);
        var viewScript = InstantiateView<AirborneGamePauseView>("AirborneGame/Prefabs/AirborneGamePauseView");
        _gamePauseController = new AirborneGamePauseController();
        RegisterController(_gamePauseController);
        _gamePauseController.GamePauseView = (AirborneGamePauseView)viewScript;
        _gamePauseController.Start();
        HideBackBtn();
    }

    public override void Show(float delay)
    {
        base.Show(delay);
     //   HideBackBtn();
    }
}
