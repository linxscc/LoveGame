using Assets.Scripts.Common;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Interfaces;
using game.main;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ActivityFirstRechargePanel : ReturnablePanel
{
    private ActivityFirstRechargeController _controller;   
    string path = "ActivityFirstRecharge/Prefabs/ActivityFirstRechargeView";

    public override void Init(IModule module)
    {
        base.Init(module);
        var viewScript = InstantiateView<ActivityFirstRechargeView>(path);

        _controller = new ActivityFirstRechargeController();
        RegisterController(_controller);    
       
        RegisterView(viewScript);
        _controller.View = viewScript;
        
        RegisterModel<FirstRechargeModel>();
        
         _controller.Start();
    }

    public override void Show(float delay)
    {
        Main.ChangeMenu(MainMenuDisplayState.ShowTopBar);
        ShowBackBtn();
    }

    public void HideBackBtnAndTop()
    {
        HideBackBtn();
        Main.ChangeMenu(MainMenuDisplayState.HideAll);
    }
  
    
 
}
