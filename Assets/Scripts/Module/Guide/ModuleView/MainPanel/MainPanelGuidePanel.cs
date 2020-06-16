using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Interfaces;
using Game.Guide;
using Module.Guide.ModuleView.Supporter;

namespace Module.Guide.ModuleView
{
    public class MainPanelGuidePanel : Panel
    {
        public override void Init(IModule module)
        {
            base.Init(module);

            MainPanelGuideView view =
                InstantiateView<MainPanelGuideView>(
                    "Guide/Prefabs/ModuleView/MainPanel/MainPanelGuideView");
            
            MainPanelGuideController controller = new MainPanelGuideController();
            controller.View = view;
            
            RegisterController(controller);
        }

        public override void Hide()
        {
        }

        public override void Show(float delay)
        {
        }
    }
}