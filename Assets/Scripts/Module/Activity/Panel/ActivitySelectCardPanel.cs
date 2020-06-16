//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using Assets.Scripts.Common;
//using Assets.Scripts.Framework.GalaSports.Core;
//using Assets.Scripts.Framework.GalaSports.Interfaces;
//using System;
//using System.Collections.Generic;
//using UnityEngine;
//
//
//public class ActivitySelectCardPanel : Panel
//{
//
//
//    private ActivitySelectCardController _controller;
//    string path = "Activity/Prefabs/ActivitySelectCardView";
//
//    public override void Init(IModule module)
//    {
//        base.Init(module);
//        var viewScript = InstantiateView<ActivitySelectCardView>(path);
//        
//        _controller = new ActivitySelectCardController();
//        _controller.View = (ActivitySelectCardView)viewScript;
//
//        RegisterView(viewScript);
//        RegisterController(_controller);
//        _controller.Start();
//        
//    }
//
//    public override void Show(float delay)
//    {
//        Main.ChangeMenu(MainMenuDisplayState.ShowTopBar);
//    }
//
//
//    public void CreateSevenDaysSelectCardInfo(SevenDaysLoginAwardVO vo)
//    {
//        _controller.View.CreateSevenDaysSelectCardInfo(vo);
//    }
//
//}
