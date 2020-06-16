using Assets.Scripts.Framework.GalaSports.Core;
using Common;
using DataModel;
using game.main;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoveDiaryModule :  ModuleBase
{
    CalendarPanel _calendarPanel;
    TemplatePanel _templatePanel;
    LoveDiaryEditPanel _loveDiaryEditPanel;
    public override void Init()
    {

        RegisterModel<LoveDiaryModel>();
        _calendarPanel = new CalendarPanel();
        _calendarPanel.Init(this);
        _calendarPanel.Show(0);
 

        GuideManager.RegisterModule(this);

    }

    public override void OnMessage(Message message)
    {
        string name = message.Name;
        object[] body = message.Params;
        switch (name)
        {
            case MessageConst.MODULE_LOVEDIARY_SHOW_TEMPLATE_PANEL:
                if (_templatePanel == null)
                {
                    _templatePanel = new TemplatePanel();
                    _templatePanel.Init(this);
                    DateTime Dt = (DateTime)body[0];
                    _templatePanel.SetData(Dt);
                }
                _templatePanel.Show(0);

                break;
            case MessageConst.MODULE_LOVEDIARY_SHOW_EDIT_PANEL:
                if(_templatePanel!=null)
                    _templatePanel.Hide();
                if (_loveDiaryEditPanel == null)
                {
                    _loveDiaryEditPanel = new LoveDiaryEditPanel();
                    _loveDiaryEditPanel.Init(this);
                }
                _loveDiaryEditPanel.Show(0);

                _loveDiaryEditPanel.SetData((LoveDiaryEditType)body[0],(CalendarDetailVo)body[1]);
                break;
            case MessageConst.MODULE_LOVEDIARY_SHOW_CALENDAR_PANEL:
                if (_templatePanel != null)
                {
                    _templatePanel.Destroy();
                    _templatePanel = null;
                }

                if (_loveDiaryEditPanel != null)
                {
                    _loveDiaryEditPanel.Destroy();
                    _loveDiaryEditPanel = null;
                }
                _calendarPanel.Show(0);
                _calendarPanel.UpdatePanel();
                break;
            case MessageConst.MODULE_LOVEDIARY_SHOW_CALENDARORTEMPLATE_PANEL:
                if (_loveDiaryEditPanel != null)
                {
                    _loveDiaryEditPanel.Destroy();
                    _loveDiaryEditPanel = null;
                }
                if (_templatePanel != null)
                {
                    _templatePanel.Show(0);
                }
                else
                {
                    _calendarPanel.Show(0);
                    _calendarPanel.UpdatePanel();
                }
                break;

        }

    }

}
