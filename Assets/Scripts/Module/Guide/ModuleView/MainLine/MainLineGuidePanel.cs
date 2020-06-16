using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Interfaces;

namespace Assets.Scripts.Module.Guide.ModuleView
{
    public class MainLineGuidePanel : Panel
    {
        public override void Init(IModule module)
        {
            base.Init(module);

            MainLineGuideView view = InstantiateView<MainLineGuideView>("Guide/Prefabs/ModuleView/MainLine/MainLineGuideView");

            MainLineGuideController controller = new MainLineGuideController();
            controller.View = view;
            
            RegisterController(controller);
        }
    }
}