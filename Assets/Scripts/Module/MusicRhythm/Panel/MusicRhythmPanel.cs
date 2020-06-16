using Assets.Scripts.Common;
using Assets.Scripts.Framework.GalaSports.Interfaces;
using Common;
using DataModel;
using game.main;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core;
using UnityEngine;

public class MusicRhythmPanel : ReturnablePanel, IUpdatable
{
    private MusicRhythmController _musicRhythmController;
    public override void Init(IModule module)
    {
        base.Init(module);
        var m = (UpdatableModuleBase)module;
        m.RegisterUpdatablePanel(this);
        var viewScript = InstantiateView<MusicRhythmView>("MusicRhythm/Prefabs/MusicRhythmView");
        _musicRhythmController = new MusicRhythmController();
        RegisterController(_musicRhythmController);
        _musicRhythmController.musicRhythmView = (MusicRhythmView)viewScript;
        _musicRhythmController.Start();
    }

    public override void Show(float delay)
    {
       //ShowBackBtn();
        HideBackBtn();
        Main.ChangeMenu(MainMenuDisplayState.HideAll);
        base.Show(delay);
    }

    public override void Destroy()
    {
        var m = (UpdatableModuleBase)Module;
        m.UnRegisterUpdatablePanel(this);
        base.Destroy();
    }

    public override void OnBackClick()
    {
        SendMessage(new Assets.Scripts.Framework.GalaSports.Core.Message(MessageConst.MODULE_MUSICRHYRHM_PAUSE_GAME));

        string content= GetData<MusicRhythmModel>().runningInfo.musicName+"-"+ GetData<MusicRhythmModel>().runningInfo.diffName;
        PopupManager.ShowConfirmWindow(content,
            null,
            I18NManager.Get("MusicRhythm_OverGame"),
            I18NManager.Get("MusicRhythm_ContinueGame")
            ).WindowActionCallback = evt =>
        {
            if (evt == WindowEvent.Ok)
            {
                SendMessage(new Assets.Scripts.Framework.GalaSports.Core.Message(MessageConst.MODULE_MUSICRHYRHM_BACK));
            }
            else
            {
                SendMessage(new Assets.Scripts.Framework.GalaSports.Core.Message(MessageConst.MODULE_MUSICRHYRHM_CONTINUE_GAME));
            }
        };



        //base.OnBackClick();
    }

    public void Pause()
    {
        AudioManager.Instance.PauseBackgroundMusic();
    }

    public void Play()
    {
        AudioManager.Instance.PlayBackgroundMusic();
    }

    public void Shutdown()
    {
        _musicRhythmController.OnShutdown();
    }

    public void Start()
    {
        ShowBackBtn();
        _musicRhythmController.OnStart();
        Debug.LogError("Start");
    }

    public void Update(float delaytime)
    {
        _musicRhythmController.OnUpdate(delaytime);
    }
}
