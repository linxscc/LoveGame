using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Interfaces;

namespace Assets.Scripts.Module.Guide.ModuleView
{
	public class LoveGuidePanel : Panel
	{
		public override void Init(IModule module)
		{
			base.Init(module);

			LoveGuideView view = InstantiateView<LoveGuideView>("Guide/Prefabs/ModuleView/Love/LoveGuideModule");

			LoveGuideController controller = new LoveGuideController();
			controller.View = view;
            
			RegisterController(controller);
		}
	}
}