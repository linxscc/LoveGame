using Assets.Scripts.Common;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Interfaces;
using System;
using System.Collections.Generic;
using UnityEngine;


public class ReloadingPanel : ReturnablePanel
{  
    private ReloadingController _reloadingController;
    string path = "Reloading/ReloadingModule";

    public override void Init(IModule module)
    {
        base.Init(module);
        var viewScript = InstantiateView<ReloadingView>(path);
        _reloadingController = new ReloadingController();
        _reloadingController.View = (ReloadingView)viewScript;
      
        
        RegisterView(viewScript);
        RegisterController(_reloadingController);    
       // RegisterModel<ReloadingModel>();
       _reloadingController.Start();
    }

    public override void Hide()
    {
        _reloadingController.Hide();
    }

    public override void Show(float delay)
    {
        Main.ChangeMenu(MainMenuDisplayState.HideAll);
        ShowBackBtn();
    }

    public void BackFormSaveAndShareView()
    {
        _reloadingController.Show();
        ShowBackBtn();
        Main.ChangeMenu(MainMenuDisplayState.HideAll);
    }
}
