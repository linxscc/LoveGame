using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Interfaces;
using Module.Battle.Data;
using UnityEngine;

public class CapsuleFinalEstimateFailPanel : Panel
{
    private string path = "ActivityCapsuleBattle/Prefabs/CapsuleFinalEstimateFail"; 
    
    public override void Init(IModule module)
    {
        base.Init(module);
        var windowScript = InstantiateWindow<CapsuleFinalEstimateFail>(path);
       
        RegisterModel<CapsuleBattleResultData>();

        Debug.LogError("失败分数===>"+GetData<CapsuleBattleResultData>().Cap);
        
        windowScript.SetData(GetData<CapsuleBattleResultData>().Cap);
    }
    
}
