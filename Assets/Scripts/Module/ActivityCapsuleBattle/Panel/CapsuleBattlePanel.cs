using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Common;
using Assets.Scripts.Framework.GalaSports.Interfaces;
using UnityEngine;

public class CapsuleBattlePanel : ReturnablePanel
{
  private CapsuleBattleController controller;

  public override void Init(IModule module)
  {
    base.Init(module);
    var viewScript =  InstantiateView<CapsuleBattleView>("ActivityCapsuleBattle/Prefabs/CapsuleBattleView");
    RegisterView(viewScript);

    controller = new CapsuleBattleController {view = viewScript};
    RegisterController(controller);
    
    HideBackBtn();
  }
  
  public override void Show(float delay)
  {
    controller.Start();
  }
  
  public void Restart()
  {
    controller.Start();
    controller.view.ResetView();
  }
  
  public void GetFansInfo(Queue<string> fansInfo)
  {
    controller.view.GetFansInfo(fansInfo);
  }
  
  public void GetRolesId(Queue<int> roleIds)
  {
    controller.view.GetRolesId(roleIds);
  }
        
  public void GetPower(int power)
  {
    controller.view.GetPower(power); 
  }
}
