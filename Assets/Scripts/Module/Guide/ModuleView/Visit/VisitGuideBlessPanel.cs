using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Interfaces;
using Assets.Scripts.Module.Guide.Visit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Module.Guide.ModuleView.Visit
{
    public class VisitGuideBlessPanel : Panel
    {

        private object view;
        VisitGuideBlessController _controller;
        public override void Init(IModule module)
        {
            base.Init(module);
            VisitGuideBlessView guideView = (VisitGuideBlessView)InstantiateView<VisitGuideBlessView>(
                    "Guide/Prefabs/ModuleView/Visit/VisitGuideBlessView");

            _controller = new VisitGuideBlessController();
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
