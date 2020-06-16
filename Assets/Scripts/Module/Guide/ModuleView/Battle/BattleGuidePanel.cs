using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Interfaces;
using Game.Guide;

namespace Assets.Scripts.Module.Guide.ModuleView
{
    public class BattleGuidePanel : Panel
    {
        public override void Init(IModule module)
        {
            base.Init(module);

            BattleGuideView view = InstantiateView<BattleGuideView>("Guide/Prefabs/ModuleView/Battle/BattleGuideView");

            BattleGuideController battleController = new BattleGuideController();
            battleController.View = view;
            RegisterController(battleController);
        }
    }
}