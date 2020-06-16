using Assets.Scripts.Common;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherPanel : ReturnablePanel
{
    private WeatherController _weatherController;
    string path = "Visit/Prefabs/WeatherView";
    WeatherView viewScript;
    public override void Init(IModule module)
    {
        base.Init(module);
        viewScript = InstantiateView<WeatherView>(path);
        _weatherController = new WeatherController();
        _weatherController.WeatherView = (WeatherView)viewScript;

        RegisterView(viewScript);
        RegisterController(_weatherController);
        _weatherController.Start();

    }
    public override void Show(float delay)
    {
        _weatherController.WeatherView.Show(delay);
        base.Show(delay);
        Main.ChangeMenu(MainMenuDisplayState.ShowVisitTopBar);
        ShowBackBtn();

    }

    public override void Destroy()
    {
        RegisterView(viewScript);
        UnregisterController(_weatherController);

        base.Destroy();
    }

    public void SetData(PlayerPB npcId)
    {

        _weatherController.SetData(npcId);
    }
    public override void Hide()
    {
        _weatherController.WeatherView.Hide();
        base.Hide();
    }

    public override void OnBackClick()
    {
        SendMessage(new Message(MessageConst.MODULE_VISIT_SHOW_VISIT_PANEL, Message.MessageReciverType.DEFAULT));
    }

}
