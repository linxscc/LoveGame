using System.Linq;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Services;
using DataModel;
using Module.Battle.Data;

public class FinalEstimateRewardController : Controller
{

    //public FinalEstimateRewardView View;

    public FinalEstimateRewardWindow Window;
    
    
    


    public override void Start()
    {
        base.Start();
        BattleResultData data = GetData<BattleResultData>();
        GlobalData.CardModel.UpdateUserCards(data.UserCards.ToArray());
        Window.SetData(data, GlobalData.PlayerModel);
        
        if (GlobalData.RandomEventModel.CheckTrigger(7002, 7003))
            new TriggerService().ShowNewGiftWindow().Execute();
    }
}
