using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Interfaces;
using Game.Guide;

namespace Module.Guide.ModuleView.Recollection
{
     
     public class RecollectionGuidePanel:Panel
    {

        private RecollentionGuideController _recollentionController;

        public override void Init(IModule module)
        {
            base.Init(module);


            RecollectionGuideView guideView =
                (RecollectionGuideView)InstantiateView<RecollectionGuideView>(
                    "Guide/Prefabs/ModuleView/Recollection/RecollectionGuideView");

            _recollentionController = new RecollentionGuideController();
            _recollentionController.View = (RecollectionGuideView)guideView;

            RegisterController(_recollentionController);
            
            _recollentionController.Start();


        }



        public override void Hide()
        {
            
        }


        public override void Show(float delay)
        {
            base.Show(delay);
          
        }
    }
}
