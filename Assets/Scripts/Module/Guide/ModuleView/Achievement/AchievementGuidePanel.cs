using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Interfaces;
using Game.Guide;

namespace Assets.Scripts.Module.Guide.ModuleView
{
	public class AchievementGuidePanel : Panel
	{
		public override void Init(IModule module)
		{
			base.Init(module);

			AchievementGuideView view = InstantiateView<AchievementGuideView>("Guide/Prefabs/ModuleView/Achievement/AchievementGuideView");

			AchievementGuideController controller = new AchievementGuideController();
			controller.View = view;
            
			RegisterController(controller);
		}
	}
}