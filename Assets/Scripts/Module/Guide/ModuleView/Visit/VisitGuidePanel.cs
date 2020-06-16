using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Interfaces;
using Assets.Scripts.Module.Guide.Visit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Module.Guide.ModuleView.Visit
{
    public class VisitGuidePanel : Panel
    {
        private object view;
        VisitGuideController _controller;
        public override void Init(IModule module)
        {
            base.Init(module);
            VisitGuideView guideView =(VisitGuideView)InstantiateView<VisitGuideView>(
                    "Guide/Prefabs/ModuleView/Visit/VisitGuideView");

             _controller = new VisitGuideController();
            _controller.View = guideView;
            RegisterController(_controller);
        }
        public override void Show(float delay)
        {
            base.Show(delay);
        }

        public override void Destroy()
        {

            UnregisterController(_controller);
            base.Destroy();
        }
    }
}
