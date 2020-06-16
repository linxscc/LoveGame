using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core;
using Module.Battle.Data;
using UnityEngine;

public class CapsuleFinalEstimateCommentController : Controller
{
    public CapsuleFinalEstimateCommentWindow Window;
    
    public override void Start()
    {                   
        Window.SetData(GetData<CapsuleBattleResultData>(),GetData<CapsuleBattleModel>().LevelVo);
    }
}
