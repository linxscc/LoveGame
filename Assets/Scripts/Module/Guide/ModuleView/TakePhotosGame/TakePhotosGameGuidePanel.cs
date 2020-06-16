using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Interfaces;

namespace Assets.Scripts.Module.Guide.ModuleView
{
	public class TakePhotosGameGuidePanel : Panel
	{
		public override void Init(IModule module)
		{
			base.Init(module);

			TakePhotosGameGuideView view = InstantiateView<TakePhotosGameGuideView>("Guide/Prefabs/ModuleView/TakePhotosGame/TakePhotosGameGuideView");

			TakePhotosGameGuideController controller = new TakePhotosGameGuideController();
			controller.View = view;
            
			RegisterController(controller);
		}
	}
}