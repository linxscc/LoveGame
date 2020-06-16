using Assets.Scripts.Common;
using Assets.Scripts.Framework.GalaSports.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalendarPanel : ReturnablePanel
{
    CalendarController _calendarController;
    
    public override void Init(IModule module)
    {
        base.Init(module);
        var viewScript = InstantiateView<CalendarView>("LoveDiary/Prefabs/CalendarView");
        _calendarController = new CalendarController();
        RegisterController(_calendarController);
        _calendarController.CurCalendarView = (CalendarView) viewScript;
        _calendarController.Start();
    }

    public override void Show(float delay)
    {
        base.Show(delay);
        Main.ChangeMenu(MainMenuDisplayState.HideAll);
        ShowBackBtn();
    }
    public void UpdatePanel()
    {
        _calendarController.UpdateView();
    }
}
