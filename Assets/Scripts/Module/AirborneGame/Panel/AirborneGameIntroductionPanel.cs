using Assets.Scripts.Common;
using Assets.Scripts.Framework.GalaSports.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirborneGameIntroductionPanel : ReturnablePanel
{
    AirborneGameIntroductionController _introductionController;
    public override void Init(IModule module)
    {
        base.Init(module);
        var viewScript = InstantiateView<AirborneGameIntroductionView>("AirborneGame/Prefabs/AirborneGameIntroductionView");
        _introductionController = new AirborneGameIntroductionController();
        RegisterController(_introductionController);
        _introductionController.View = (AirborneGameIntroductionView)viewScript;
        _introductionController.Start();
    }
    public override void Show(float delay)
    {
        base.Show(delay);
    }
}
