using Assets.Scripts.Common;
using Assets.Scripts.Framework.GalaSports.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicRhythmResultPanel : ReturnablePanel
{
    MusicRhythmResultController _controller;
    public override void Show(float delay)
    {
        ShowBackBtn();
        Main.ChangeMenu(MainMenuDisplayState.HideAll);
        base.Show(delay);
    }
    public override void Init(IModule module)
    {
        base.Init(module);

        var viewScript = InstantiateView<MusicRhythmResultView>("MusicRhythm/Prefabs/MusicRhythmResultView");
        _controller = new MusicRhythmResultController();
        RegisterController(_controller);
        _controller.view = (MusicRhythmResultView)viewScript;
        _controller.Start();
    }
}
