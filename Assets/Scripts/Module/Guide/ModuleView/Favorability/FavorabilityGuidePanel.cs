using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Interfaces;
using Game.Guide;


namespace Assets.Scripts.Module.Guide.ModuleView
{
    public class FavorabilityGuidePanel : Panel 
    {
        public override void Init(IModule module)
        {
            base.Init(module);
            
            FavorabilityGuideView view = InstantiateView<FavorabilityGuideView>("Guide/Prefabs/ModuleView/Favorability/FavorabilityGuideView");
            
            FavorabilityGuideController favorabilityController = new FavorabilityGuideController();
            favorabilityController.View = view;
            RegisterController(favorabilityController);
        }
           
        

    }
}

