using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Interfaces;

namespace Assets.Scripts.Module.Guide.ModuleView.Task
{
    public class TaskGuidePanel : Panel
    {
        public override void Init(IModule module)
        {
            base.Init(module);

            TaskGuideView taskGuideView = InstantiateView<TaskGuideView>("Guide/Prefabs/ModuleView/Task/TaskGuideView");
        }
    }
}