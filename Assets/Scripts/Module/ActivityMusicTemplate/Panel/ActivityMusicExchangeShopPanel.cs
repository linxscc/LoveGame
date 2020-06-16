using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Common;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Interfaces;
using UnityEngine;

public class ActivityMusicExchangeShopPanel : ReturnablePanel
{
    private string _path = "ActivityMusicTemplate/Prefabs/ActivityMusicExchangeShopWindow";
    private ActivityMusicExchangeShopController _controller;

    public override void Init(IModule module)
    {
        base.Init(module);
        var viewScript = InstantiateWindow<ActivityMusicExchangeShopView>(_path);
        _controller = new ActivityMusicExchangeShopController {View = viewScript};
        RegisterView(viewScript);
        RegisterController(_controller);      
       
    }
    
    
    public override void Show(float delay)
    {
        Main.ChangeMenu(MainMenuDisplayState.HideAll);               
    }


    public void SetModel(ActivityExchangeShopModel model)
    {
        _controller.ShopModel = model;
        //_controller.CurActivity = model;
        _controller.Start();
    }
}
