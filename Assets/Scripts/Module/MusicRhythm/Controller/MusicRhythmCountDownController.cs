using Assets.Scripts.Framework.GalaSports.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicRhythmCountDownController : Controller
{

    public MusicRhythmCountDownView view;

    int _CountDown = 3;
    // Use this for initialization
    public override void Start()
    {
        _CountDown = 3;
        SetShow();
    }

    void SetShow()
    {
        view.SetData(_CountDown);
        if (_CountDown <= 0)
        {
            SendMessage(new Message(MessageConst.MODULE_MUSICRHYRHM_ClOSE_COUNTDOWN_PANEL));
            return;
        }
        _CountDown--;
        ClientTimer.Instance.DelayCall(() =>
        {
            SetShow();
        }, 1.0f);
    }

}
