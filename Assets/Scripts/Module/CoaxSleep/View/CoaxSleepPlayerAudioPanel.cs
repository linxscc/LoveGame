using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Common;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Interfaces;
using UnityEngine;

public class CoaxSleepPlayerAudioPanel : ReturnablePanel
{
    private string _path = "CoaxSleep/Prefabs/CoaxSleepPlayerAudioView";
    
    private CoaxSleepPlayerAudioController _controller;

    public override void Init(IModule module)
    {
        base.Init(module);
        var viewScript = InstantiateView<CoaxSleepPlayerAudioView>(_path);
        _controller =new CoaxSleepPlayerAudioController{View = viewScript};
        RegisterView(viewScript);
        RegisterController(_controller);
        _controller.Start();
    }


    public override void Show(float delay)
    {
        Main.ChangeMenu(MainMenuDisplayState.ShowTopBar);
        ShowBackBtn();
    }

    public void OnShow()
    {
        ShowBackBtn();
        Main.ChangeMenu(MainMenuDisplayState.ShowTopBar);
    }
    
    public override void OnBackClick()
    {
        SendMessage(new Message(MessageConst.CMD_CPAXSLEEP_DESTROY_PANEL,"PlayerAudioPanel"));
    }

    public void SetCurPlayerPb(PlayerPB playerPb)
    {
        _controller.SetCurData(playerPb);
    }
}
