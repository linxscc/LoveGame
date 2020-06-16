using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Interfaces;
using Game.Guide;

public class CoaxSleepGuidePanel : Panel
{
    public override void Init(IModule module)
    {
        base.Init(module);
            
        CoaxSleepGuideView view = InstantiateView<CoaxSleepGuideView>("Guide/Prefabs/ModuleView/CoaxSleep/CoaxSleepGuideView");

        CoaxSleepGuideController controller = new CoaxSleepGuideController {View = view};
        RegisterController(controller);
    }
}
