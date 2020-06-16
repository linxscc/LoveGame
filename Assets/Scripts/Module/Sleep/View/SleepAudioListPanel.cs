using Assets.Scripts.Common;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Core.Events;
using Assets.Scripts.Framework.GalaSports.Interfaces;
using Common;
using DataModel;
using game.main;
using Module.Card.Controller;
using UnityEngine;

public class SleepAudioListPanel : ReturnablePanel
{
    private SleepController _controller;
    public override void Init(IModule module)
    {
        base.Init(module);

        var viewScript = InstantiateView<SleepAudioListView>("Sleep/Prefabs/SleepAudioListView");
        _controller = new SleepController { AudioListView = viewScript };
        RegisterView(viewScript);
        RegisterController(_controller);
        RegisterModel<SleepModel>();
        _controller.Start();

    }

    public override void Show(float delay)
    {
        //_controller.View.Show();
        Main.ChangeMenu(MainMenuDisplayState.ShowTopBar);
        ShowBackBtn();
    }


    public void OnShow()
    {
        //_controller.AudioListView.OnShow();
    }

    public void IsShowBackBtnAndTopBar(bool isShow)
    {
        if (isShow)
        {
            ShowBackBtn();
            Main.ChangeMenu(MainMenuDisplayState.ShowTopBar);
        }
        else
        {
            HideBackBtn();
            Main.ChangeMenu(MainMenuDisplayState.HideAll);
        }
    }

    public override void Hide()
    {
        _controller.AudioListView.Hide();
        base.Hide();
    }
}
