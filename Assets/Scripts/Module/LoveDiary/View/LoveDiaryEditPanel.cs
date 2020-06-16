using Assets.Scripts.Common;
using Assets.Scripts.Framework.GalaSports.Interfaces;
using game.main;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoveDiaryEditPanel : ReturnablePanel
{
    LoveDiaryEditController _loveDiaryEditController;
    public override void Init(IModule module)
    {
        base.Init(module);
        var viewScript = InstantiateView<LoveDiaryEditView>("LoveDiary/Prefabs/LoveDiaryEditView");
        _loveDiaryEditController = new LoveDiaryEditController();
        RegisterController(_loveDiaryEditController);
        _loveDiaryEditController.CurLoveDiaryEditView = (LoveDiaryEditView)viewScript;

        _loveDiaryEditController.Start();
    }

    public override void Show(float delay)
    {
        base.Show(delay);
        //Main.ChangeMenu(MainMenuDisplayState.HideAll);
        Main.ChangeMenu(MainMenuDisplayState.ShowTopBar);
        ShowBackBtn();
    }
    public override void Hide()
    {
        base.Hide();

        //Main.ChangeMenu(MainMenuDisplayState.HideAll);
    }

    public void SetData(LoveDiaryEditType editType ,CalendarDetailVo calendarDetailVo)
    {
        _loveDiaryEditController.EditType = editType;
        _loveDiaryEditController.SetData(calendarDetailVo);
    }
    public override void OnBackClick()
    {
        _loveDiaryEditController.GoBack();

    }

    public override void Destroy()
    {
        //unri(_loveDiaryEditController);
        _loveDiaryEditController.Destroy();
        UnregisterController(_loveDiaryEditController);
        base.Destroy();
    }
}
