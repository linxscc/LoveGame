using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Framework.GalaSports.Core;

public class ActivityCapsuleTemplateModule : ModuleBase
{

    private ActivityCapsuleTemplatePanel _panel;
    private ActivityCapsuleTemplateDrawPanel _drawPanel;

    public override void Init()
    {
        _panel = new ActivityCapsuleTemplatePanel();
        _panel.Init(this);
        _panel.Show(0.5f);
    }


    public override void OnShow(float delay)
    {
        base.OnShow(delay);
        _panel.Show(0);
        _panel.OnShow();
    }


    public override void OnMessage(Message message)
    {
        string name = message.Name;
        object[] body = message.Params;
        switch (name)
        {
            case MessageConst.MODULE_ACTIVITY_CAPSULE_TEMPLATE_OPEN_MAIN_PANEL:
                if (_drawPanel != null)
                    _drawPanel.Hide();
                if (_panel == null)
                {
                    _panel = new ActivityCapsuleTemplatePanel();
                    _panel.Init(this);
                }
                else
                {
                    _panel.Refresh();
                }
                _panel.Show(0.5f);
                break;
            case MessageConst.MODULE_ACTIVITY_CAPSULE_TEMPLATE_OPEN_DRAW_PANEL:
                if (_drawPanel == null)
                {
                    _drawPanel = new ActivityCapsuleTemplateDrawPanel();
                    _drawPanel.Init(this);
                }
                else
                {
                    _drawPanel.Refresh();
                }
                _drawPanel.Show(0.5f);
                break;
            case MessageConst.MODULE_ACTIVITY_CAPSULE_TEMPLATE_Hide_BACKBTN:
                var isShow = (bool)body[0];
                if (_drawPanel != null)
                {
                    _drawPanel.IsShowBackBtnAndTopBar(isShow);
                }
                break;
        }
    }
    

    public override void Remove(float delay)
    {
        base.Remove(delay);
        _panel.DestroyCountDown();
    }
}
