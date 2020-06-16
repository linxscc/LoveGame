using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Common;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Interfaces;
using Common;
using UnityEngine;


public class CoaxSleepAniPanel : ReturnablePanel
{
     private string _path = "CoaxSleep/Prefabs/CoaxSleepAniView";
     private CoaxSleepAniController _controller;
     
     public override void Init(IModule module)
     {
          base.Init(module);
          var viewScript = InstantiateView<CoaxSleepAniView>(_path);
          _controller =new CoaxSleepAniController{View = viewScript};
          RegisterView(viewScript);
          RegisterController(_controller);      
          _controller.Start();  
     }
    
     public override void Show(float delay)
     {
        
         Main.ChangeMenu(MainMenuDisplayState.HideAll);  
          HideBackBtn();      
     }

     public override void Destroy()
     {
          base.Destroy();
          AudioManager.Instance.PlayDefaultBgMusic();
     }
}
