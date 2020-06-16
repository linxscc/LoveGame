using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Common;
using Assets.Scripts.Framework.GalaSports.Interfaces;
using UnityEngine;

public class ActivityMusicTemplatePanel : ReturnablePanel
{

    private string _path = "ActivityMusicTemplate/Prefabs/ActivityMusicTemplateView";
    private ActivityMusicTemplateController _controller;

    public override void Init(IModule module)
    {
        base.Init(module);
        var viewScript = InstantiateView<ActivityMusicTemplateView>(_path);
        _controller = new ActivityMusicTemplateController {View = viewScript};
        RegisterView(viewScript);
        RegisterController(_controller);
      //  RegisterModel<ActivityMusicTemplateModel>();
        _controller.Start();
    }
    
    
    public override void Show(float delay)
    {
        Main.ChangeMenu(MainMenuDisplayState.ShowTopBar);
        ShowBackBtn();            
    }

    
}
