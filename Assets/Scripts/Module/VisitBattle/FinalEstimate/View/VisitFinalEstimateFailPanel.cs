using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Interfaces;
using Module.VisitBattle.Data;

public class VisitFinalEstimateFailPanel : Panel {
	
	 public override void Init(IModule module)
    {
        base.Init(module);
        VisitFinalEstimateFailView view =(VisitFinalEstimateFailView)InstantiateView<VisitFinalEstimateFailView>("VisitBattle/FinalEstimate/FinalEstimateFail");
        
        view.SetData(GetData<VisitBattleResultData>(), GetData<VisitBattleModel>().LevelVo);
        
    }

    public override void Hide()
    {
        
    }

    public override void Show(float delay)
    {
        
    }

    public override void Destroy()
    {
        base.Destroy();
    }
}
