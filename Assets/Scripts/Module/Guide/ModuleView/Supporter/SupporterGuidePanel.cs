using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Interfaces;

namespace Module.Guide.ModuleView.Supporter
{
    public class SupporterGuidePanel : Panel {
	
        public override void Init(IModule module)
        {
            base.Init(module);

            SupporterGuideView guideView =
                (SupporterGuideView) InstantiateView<SupporterGuideView>(
                    "Guide/Prefabs/ModuleView/Supporter/SupporterGuideView");
        }

        public override void Hide()
        {
        
        }

        public override void Show(float delay)
        {
        
        }

    }
}
