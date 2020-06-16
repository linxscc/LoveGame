using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Module.Guide.ModuleView.Phone
{
    public class PhoneGuidePanel : Panel
    {
        PhoneGuideController _controller;
        public override void Init(IModule module)
        {
            base.Init(module);
            PhoneGuideView guideView = (PhoneGuideView)InstantiateView<PhoneGuideView>(
                    "Guide/Prefabs/ModuleView/Phone/PhoneGuideView");

            _controller = new PhoneGuideController();
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