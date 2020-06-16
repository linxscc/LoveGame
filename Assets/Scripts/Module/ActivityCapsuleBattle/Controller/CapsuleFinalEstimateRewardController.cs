using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Services;
using DataModel;
using Module.Battle.Data;
using UnityEngine;

public class CapsuleFinalEstimateRewardController : Controller
{
    public CapsuleFinalEstimateRewardWindow Window;
    
    public override void Start()
    {
        base.Start();
        CapsuleBattleResultData data = GetData<CapsuleBattleResultData>();
        GlobalData.CardModel.UpdateUserCards(data.UserCards.ToArray());
        Window.SetData(data, GlobalData.PlayerModel);
        
//        if (GlobalData.RandomEventModel.CheckTrigger(7002, 7003))
//            new TriggerService().ShowNewGiftWindow().Execute();
    }
}
