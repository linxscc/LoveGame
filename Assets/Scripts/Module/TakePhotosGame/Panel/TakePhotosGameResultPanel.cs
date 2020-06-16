using Assets.Scripts.Common;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Interfaces;
using DataModel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakePhotosGameResultPanel : ReturnablePanel
{
    TakePhotosGameResultController _introductionController;
    public override void Init(IModule module)
    {
        base.Init(module);
        var viewScript = InstantiateView<TakePhotosGameResultView>("TakePhotosGame/Prefabs/TakePhotosGameResultView");
        _introductionController = new TakePhotosGameResultController();
        RegisterController(_introductionController);
        _introductionController.View = (TakePhotosGameResultView)viewScript;

    }
    public override void Show(float delay)
    {
        _introductionController.Start();
        ShowBackBtn();
        base.Show(delay);
    }
    public override void OnBackClick()
    {
        SendMessage(new Message(MessageConst.MODULE_TAKEPHOTOSGAME_GOTO_INTRODUCTION_PANEL));
        //base.OnBackClick();

    }
}
