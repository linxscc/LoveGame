using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Interfaces;
using UnityEngine;

public class CapsuleFinalEstimateCommentPanel : Panel
{
   private CapsuleFinalEstimateCommentController controller; 
   private string path = "ActivityCapsuleBattle/Prefabs/CapsuleFinalEstimateComment"; 
   
   
   public override void Init(IModule module)
   {
      base.Init(module);

      CapsuleFinalEstimateCommentWindow win = InstantiateWindow<CapsuleFinalEstimateCommentWindow>(path);

      controller = new CapsuleFinalEstimateCommentController();       
      RegisterController(controller);
        
      controller.Window = win;
        
      controller.Start();
   }
}
