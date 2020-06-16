using Assets.Scripts.Framework.GalaSports.Core;
using System.Collections;
using System.Collections.Generic;
using Common;
using UnityEngine;

public class LoveModule : ModuleBase
{
    LovePanel _lovePanel;
    public override void Init()
    {
        GuideManager.RegisterModule(this);
        _lovePanel = new LovePanel();
        _lovePanel.Init(this);
        _lovePanel.Show(0);
    }
    public override void OnShow(float delay)
    {
        base.OnShow(delay);
        _lovePanel.Show(0);
    }

    public override void OnMessage(Message message)
    {
        string name = message.Name;
        object[] body = message.Params;
        switch (name)
        {
            case MessageConst.CMD_LOVE_GUIDE_GOBACKTOMAINVIEW:
                _lovePanel.OnBackClick();
                break;
        }
        
        
        
    }

    public override void OnHide()
    {
        base.OnHide();
    }
}
