using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Interfaces;
using game.main;

public class FinalEstimateRewardPanel : Panel {
	
	private FinalEstimateRewardController controller;
	
	private string path = "Battle/FinalEstimate/FinalEstimateReward"; 

	 public override void Init(IModule module)
	 {
		 base.Init(module);

		 FinalEstimateRewardWindow windowScript = InstantiateWindow<FinalEstimateRewardWindow>(path);
		 
		 controller = new FinalEstimateRewardController();       
		 RegisterController(controller);
		 
		 controller.Window = windowScript;
        
		 controller.Start();
	 }
}
