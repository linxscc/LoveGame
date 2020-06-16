using Assets.Scripts.Common;
using Assets.Scripts.Framework.GalaSports.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakePhotosGamePanel : ReturnablePanel
{
    TakePhotosGameController _takePhotosGameController;
   // TakePhotosGameCountDownController _countDownCtr;
    public override void Init(IModule module)
    {
        SetComplexPanel();
        base.Init(module);

       // _countDownCtr = new TakePhotosGameCountDownController();
        //RegisterController(_countDownCtr);

        var viewScript1 = InstantiateView<TakePhotosGameView>("TakePhotosGame/Prefabs/TakePhotosGameView");
        var viewScript2 = InstantiateView<TakePhotosGameScoreView>("TakePhotosGame/Prefabs/TakePhotosGameScoreView");
        RegisterView(viewScript1);
        _takePhotosGameController = new TakePhotosGameController();
        RegisterController(_takePhotosGameController);
        _takePhotosGameController.View = (TakePhotosGameView)viewScript1;
        _takePhotosGameController.scoreView = (TakePhotosGameScoreView)viewScript2;
        _takePhotosGameController.Start();
    }

    public override void Show(float delay)
    {
        _takePhotosGameController.SetData();
        ShowBackBtn();
        base.Show(delay);   
    }
    public override void OnBackClick()
    {
        _takePhotosGameController.GoBack();
        //base.OnBackClick();

    }

}
