using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Interfaces;

namespace Module.Guide.ModuleView.Story
{
    public class StoryGuidePanel : Panel
    {
        public override void Init(IModule module)
        {
            base.Init(module);

            StoryGuideController controller = new StoryGuideController();
            RegisterController(controller);
        }
    }
}