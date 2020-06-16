using Assets.Scripts.Common;
using Assets.Scripts.Framework.GalaSports.Interfaces;
using DataModel;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirborneGameOverPanel : ReturnablePanel
{

    AirborneGameOverController _gameOverController;

    public override void Init(IModule module)
    {
        base.Init(module);
        var viewScript = InstantiateView<AirborneGameOverView>("AirborneGame/Prefabs/AirborneGameOverView");
        _gameOverController = new AirborneGameOverController();
        RegisterController(_gameOverController);
        _gameOverController.OverView = (AirborneGameOverView)viewScript;
        _gameOverController.Start();
        //HideBackBtn();
    }

    public override void Show(float delay)
    {
        base.Show(delay);
        HideBackBtn();
    }

    public override void Hide()
    {
        base.Hide();
    }

    public override void Destroy()
    {
        base.Destroy();
    }

    public void SetData(AirborneGameOverType overType)
    {
        _gameOverController.SetData(overType);
    }
}
