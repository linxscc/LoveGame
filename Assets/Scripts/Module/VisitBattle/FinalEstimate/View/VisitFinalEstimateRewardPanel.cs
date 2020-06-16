using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Interfaces;

public class VisitFinalEstimateRewardPanel : Panel {
	
	 public override void Init(IModule module)
    {
        base.Init(module);
        var rewardView = InstantiateView<VisitFinalEstimateRewardView>("VisitBattle/FinalEstimate/FinalEstimateReward");
        var rewardController = new VisitFinalEstimateRewardController();
        rewardController.View = (VisitFinalEstimateRewardView)rewardView;
        RegisterController(rewardController);

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
