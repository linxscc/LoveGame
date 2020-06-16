using Assets.Scripts.Common;
using Assets.Scripts.Framework.GalaSports.Interfaces;
using DataModel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakePhotosGameIntroductionPanel : ReturnablePanel
{
    TakePhotosGameIntroductionController _introductionController;
    public override void Init(IModule module)
    {
        base.Init(module);
        var viewScript = InstantiateView<TakePhotosGameIntroductionView>("TakePhotosGame/Prefabs/TakePhotosGameIntroductionView");
        _introductionController = new TakePhotosGameIntroductionController();
        RegisterController(_introductionController);
        _introductionController.View = (TakePhotosGameIntroductionView)viewScript;
        RegisterModel<TakePhotosGameModel>();

    
    }

    public override void Show(float delay)
    {
        base.Show(delay);
        _introductionController.Start();
        ShowBackBtn();
        Main.ChangeMenu(MainMenuDisplayState.ShowTopBar);
    }
    public override void OnBackClick()
    {
        base.OnBackClick();
    }
}
