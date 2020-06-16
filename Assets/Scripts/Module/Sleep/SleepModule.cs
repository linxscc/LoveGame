using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module.NetWork;
using Com.Proto;
using Common;
using DataModel;
using game.main;
using UnityEngine;

public class SleepModule : ModuleBase
{
    private SleepChoosePanel _sleepChooosePanel;
    private SleepAudioListPanel _sleepAudioListPanel;

    public override void Init()
    {
        _sleepChooosePanel = new SleepChoosePanel();
        _sleepChooosePanel.Init(this);
        _sleepChooosePanel.Show(0.5f);
    }

    public override void OnShow(float delay)
    {
        base.OnShow(delay);
        _sleepChooosePanel.Show(0);
        //_sleepChooosePanel.OnShow();
    }


    public override void OnMessage(Message message)
    {
        string name = message.Name;
        object[] body = message.Params;
        switch (name)
        {
            case MessageConst.MODULE_SLEEP_CHOOSE_VIEW_BTN_ROLE:
                ShowSleepChoosePanel(false);
                ShowSleepAudioListPanel(true);
                break;
        }
    }

    private void ShowSleepChoosePanel(bool state = true)
    {
        if (state)
        {
            if (_sleepChooosePanel == null)
                _sleepChooosePanel = new SleepChoosePanel();
            _sleepChooosePanel.Init(this);
            _sleepChooosePanel.Show(0);
        }
        else
        {
            if (_sleepChooosePanel != null)
                _sleepChooosePanel.Hide();
        }
    }


    private void ShowSleepAudioListPanel(bool state = true)
    {
        if (state)
        {
            if (_sleepAudioListPanel == null)
                _sleepAudioListPanel = new SleepAudioListPanel();
            _sleepAudioListPanel.Init(this);
            _sleepAudioListPanel.Show(0);
        }
        else
        {
            if (_sleepAudioListPanel != null)
                _sleepAudioListPanel.Hide();
        }
    }
}
