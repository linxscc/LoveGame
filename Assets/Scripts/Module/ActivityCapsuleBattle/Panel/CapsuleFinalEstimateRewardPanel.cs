using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Interfaces;
using UnityEngine;

public class CapsuleFinalEstimateRewardPanel : Panel
{
    private CapsuleFinalEstimateRewardController controller;
    
    private string path = "ActivityCapsuleBattle/Prefabs/CapsuleFinalEstimateReward"; 

    public override void Init(IModule module)
    {
        base.Init(module);

        CapsuleFinalEstimateRewardWindow windowScript = InstantiateWindow<CapsuleFinalEstimateRewardWindow>(path);
		 
        controller = new CapsuleFinalEstimateRewardController();       
        RegisterController(controller);
		 
        controller.Window = windowScript;
        
        controller.Start();
    }
    
   
}
