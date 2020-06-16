using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Framework.GalaSports.Core;
using DataModel;
using Common;
using Assets.Scripts.Module;

public class TakePhotosGameCountDownController : Controller
{
    public TakePhotosGameCountDownView View;

    int _CountDown = 3;
    bool _isGuide;
    public override void Start()
    {
        //View.SetData(_CountDown);
        _isGuide = !(GuideManager.GetStepState(ModuleConfig.MODULE_TAKEPHOTOSGAME) == GuideStae.Close);
    }

    public override void OnMessage(Message message)
    {
        base.OnMessage(message);
        string name = message.Name;
        object[] body = message.Params;
        switch (name)
        {
            case MessageConst.GUIDE_TO_TAKEPHOTOSGAME_STARTGAME:
                Debug.Log("GUIDE_TO_TAKEPHOTOSGAME_STARTGAME");
                _isGuide = false;
                SetShow();        
                break;
        }
    }


    public void StartCountGame()
    {
  

        _CountDown = 4;
        var m = GetData<TakePhotosGameModel>();
        View.SetPhotoArea(m.GetRunningInfo());
        if (_isGuide)
        {
            View.SetData(_CountDown);
            return;
        }
        SetShow();
    
    }

    void SetShow()
    {
        View.SetData(_CountDown);
        if (_CountDown < 0)
        {
            SendMessage(new Message(MessageConst.MODULE_TAKEPHOTOSGAME_GOTO_START_PANEL));
            return;
        }
        _CountDown--;
        ClientTimer.Instance.DelayCall(() =>
        {
            SetShow();
        }, 1.0f);
    }
}
