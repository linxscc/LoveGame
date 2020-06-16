using Assets.Scripts.Common;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Interfaces;
using Common;
using DataModel;
using UnityEngine;

public class TakePhotosGameCountDownPanel : ReturnablePanel
{
    TakePhotosGameCountDownController _countDownController;
    public override void Init(IModule module)
    {
        base.Init(module);
        var viewScript = InstantiateView<TakePhotosGameCountDownView>("TakePhotosGame/Prefabs/TakePhotosGameCountDownView");
        _countDownController = new TakePhotosGameCountDownController();
        _countDownController.View = (TakePhotosGameCountDownView)viewScript;
        RegisterController(_countDownController);
        _countDownController.Start();
        GuideManager.Show();
    }

    public override void Show(float delay)
    {
        base.Show(delay);
        HideBackBtn();
        Main.ChangeMenu(MainMenuDisplayState.HideAll);
        _countDownController.StartCountGame();
    }

    public override void Hide()
    {
        base.Hide();
    }

    public override void Destroy()
    {
        base.Destroy();
    }

}
