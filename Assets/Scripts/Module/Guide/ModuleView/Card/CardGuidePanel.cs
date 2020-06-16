using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Interfaces;

namespace Module.Guide.ModuleView.Card
{
    public class CardGuidePanel : Panel
    {
        public override void Init(IModule module)
        {
            base.Init(module);
            
            CardGuideView view = InstantiateView<CardGuideView>("Guide/Prefabs/ModuleView/Card/CardGuideView");

            CardGuideController controller = new CardGuideController();
            controller.View = view;
            
            RegisterController(controller);
        }
    }
}