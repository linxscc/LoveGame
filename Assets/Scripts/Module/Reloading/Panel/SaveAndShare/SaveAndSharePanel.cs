//using Assets.Scripts.Common;
//using Assets.Scripts.Framework.GalaSports.Core;
//using Assets.Scripts.Framework.GalaSports.Interfaces;
//using System;
//using System.Collections.Generic;
//using UnityEngine;



//public class SaveAndSharePanel : ReturnablePanel
//{
//    #region ToDo...交互定义

//    private SaveAndShareController _saveAndShareController;
   
//    #endregion

//    string path = "Favorability/Prefabs/SaveAndShare/SaveAndShareContainers";

//    public override void Init(IModule module)
//    {
//        base.Init(module);

//        var viewScript = InstantiateView<SaveAndShareView>(path);

//        _saveAndShareController = new SaveAndShareController();
//        _saveAndShareController._saveAndShareView = (SaveAndShareView)viewScript;

//        _saveAndShareController.Start();

//        RegisterView(viewScript);
//        RegisterController(_saveAndShareController);
//    }

//    public override void Hide()
//    {
      
//    }


//    public override void Show(float delay)
//    {
//        Main.ChangeMenu(MainMenuDisplayState.HideAll);
//        HideBackBtn();
//        base.Show(delay);
//    }
//}
