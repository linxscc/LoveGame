using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Common;
using Assets.Scripts.Framework.GalaSports.Interfaces;
using UnityEngine;

/// <summary>
/// 哄睡主Panel
/// </summary>
public class CoaxSleepMainPanel : ReturnablePanel
{
    
    private string _path = "CoaxSleep/Prefabs/CoaxSleepMainView";
    
    private CoaxSleepMainController _controller;

    public override void Init(IModule module)
    {
        base.Init(module);
        var viewScript = InstantiateView<CoaxSleepMainView>(_path);
        _controller =new CoaxSleepMainController{View = viewScript};
        RegisterView(viewScript);
        RegisterController(_controller);
        RegisterModel<CoaxSleepModel>();
        _controller.Start();  
    }
    
    public override void Show(float delay)
    {
        Main.ChangeMenu(MainMenuDisplayState.ShowTopBar);
        ShowBackBtn();            
    }

    public void OnShow()
    {
        Main.ChangeMenu(MainMenuDisplayState.ShowTopBar);
        ShowBackBtn();
    }
}
