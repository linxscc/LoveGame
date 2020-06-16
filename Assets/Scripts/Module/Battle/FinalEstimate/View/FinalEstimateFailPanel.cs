using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Interfaces;
using game.main;
using Module.Battle.Data;
using UnityEngine;

public class FinalEstimateFailPanel : Panel {
	
    private string path = "Battle/FinalEstimate/FinalEstimateFail"; 
    
    public override void Init(IModule module)
    {
        base.Init(module);
        var windowScript = InstantiateWindow<FinalEstimateFailWindow>(path);
       
        RegisterModel<BattleResultData>();

        Debug.LogError("失败分数===>"+GetData<BattleResultData>().Cap);
        
        windowScript.SetData(GetData<BattleResultData>().Cap);
    }
}
