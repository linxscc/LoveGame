using Assets.Scripts.Common;
using Assets.Scripts.Framework.GalaSports.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirborneGamePanel : ReturnablePanel
{

    private AirborneGameController _airborneGameController;
    private AirborneGamePlayerController _airborneGamePlayerController;
    AirborneGameTimer _gameTimer;
    public override void Init(IModule module)
    {
        base.Init(module);
        var viewScript = InstantiateView<AirborneGameView>("AirborneGame/Prefabs/AirborneGameView");
        _airborneGameController = new AirborneGameController();
        RegisterController(_airborneGameController);
        _airborneGameController.CurAirborneGameView = (AirborneGameView)viewScript;
        _airborneGameController.Start();

        _gameTimer= _airborneGameController.CurAirborneGameView.gameObject.AddComponent<AirborneGameTimer>();
        //_gameTimer.InitTimer();
        _gameTimer.AddController(_airborneGameController);

        _airborneGamePlayerController = new AirborneGamePlayerController();
        RegisterController(_airborneGamePlayerController);
        _airborneGamePlayerController.CurAirborneGameView = (AirborneGameView)viewScript;
        _airborneGamePlayerController.Start();
    }

    public AirborneGameTimer GetAirborneGameTimer()
    {
        return _gameTimer;
    }


    public override void Show(float delay)
    {
        base.Show(delay);
        Main.ChangeMenu(MainMenuDisplayState.HideAll);
        ShowBackBtn();
    }
    public override void Hide()
    {
        base.Hide();

        //Main.ChangeMenu(MainMenuDisplayState.HideAll);
    }

    public void SetData()
    {

    }
    public override void OnBackClick()
    {
        _airborneGameController.GoBack();
        //base.OnBackClick();

    }

    public override void Destroy()
    {
        //unri(_loveDiaryEditController);
        base.Destroy();
    }
}
