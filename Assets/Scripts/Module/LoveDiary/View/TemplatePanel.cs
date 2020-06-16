using Assets.Scripts.Common;
using Assets.Scripts.Framework.GalaSports.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemplatePanel : ReturnablePanel
{
    TemplateController _templateController;
    public override void Init(IModule module)
    {
        base.Init(module);
        var viewScript = InstantiateView<TemplateView>("LoveDiary/Prefabs/TemplateView");
        _templateController = new TemplateController();
        RegisterController(_templateController);
        _templateController.CurTemplateView = (TemplateView)viewScript;

        _templateController.Start();
    }

    public override void Show(float delay)
    {
        base.Show(delay);
     //   _templateController.CurTemplateView.Show();
       // Main.ChangeMenu(MainMenuDisplayState.HideAll);
        Main.ChangeMenu(MainMenuDisplayState.ShowTopBar);
        ShowBackBtn();
    }

    public override void Hide()
    {
        //todo 
       // _templateController.CurTemplateView.Hide();
        base.Hide();

    }

    public void SetData(DateTime dateTime)
    {
        _templateController.SetData(dateTime);
    }
    public override void OnBackClick()
    {
        _templateController.GoBack();
  
    }

    public override void Destroy()
    {
        _templateController.Destroy();
        UnregisterController(_templateController);
        base.Destroy();
    }

}
