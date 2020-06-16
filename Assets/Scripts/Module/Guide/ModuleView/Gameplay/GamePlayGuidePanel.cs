using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Interfaces;

namespace Module.Guide.ModuleView.Gameplay
{
    public class GamePlayGuidePanel : Panel
    {
        public override void Init(IModule module)
        {
            base.Init(module);
            
            GameplayGuideView view =
                InstantiateView<GameplayGuideView>(
                    "Guide/Prefabs/ModuleView/Gameplay/GamePlayGuideView");
        }
    }
}