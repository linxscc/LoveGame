using Assets.Scripts.Framework.GalaSports.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AirborneGameCountDownController : Controller
{
    public AirborneGameCountDownView View;

    int _CountDown = 3;
    public override void Start()
    {
     //   View.SetData(_CountDown);
        SetShow();
    }

    void SetShow()
    {
        View.SetData(_CountDown);
        if (_CountDown <= 0)
        {
            SendMessage(new Message(MessageConst.MODULE_AIRBORNEGAME_ClOSE_COUNTDOWN_PANEL));
            return;
        }
        _CountDown--;
        ClientTimer.Instance.DelayCall(() =>
        {
            SetShow();
        }, 1.0f);
    }

}
