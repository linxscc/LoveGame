using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Interfaces;
using UnityEngine;

namespace Module.Guide.ModuleView.LoveDiary
{
    public class LoveDiaryGuidePanel : Panel
    {

        public override void Init(IModule module)
        {
            base.Init(module);

            LoveDiaryGuideView guideView =
                (LoveDiaryGuideView)InstantiateView<LoveDiaryGuideView>(
                    "Guide/Prefabs/ModuleView/LoveDiary/LoveDiaryGuideView");
        }


        public override void Hide()
        {

        }

        public override void Show(float delay)
        {

        }
    }
}
