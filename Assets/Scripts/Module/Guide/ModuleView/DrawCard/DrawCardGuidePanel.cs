using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Interfaces;
using Assets.Scripts.Module.Guide.DrawCard;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Module.Guide.ModuleView.DrawCard
{
    public class DrawCardGuidePanel : Panel
    {
        private object view;
        DrawCardGuideController _controller;
        public override void Init(IModule module)
        {
            base.Init(module);
            DrawCardGuideView guideView = (DrawCardGuideView)InstantiateView<DrawCardGuideView>(
                    "Guide/Prefabs/ModuleView/DrawCard/DrawCardGuideView");

            _controller = new DrawCardGuideController();
            _controller.View = guideView;
            RegisterController(_controller);

            if (Common.GuideManager.GetRemoteGuideStep(GuideTypePB.MainGuide) < GuideConst.MainLineStep_OnClick_GemDrawCard)
            {

                _controller.SetShow(false);
            }
            else if (Common.GuideManager.GetRemoteGuideStep(GuideTypePB.MainGuide) < GuideConst.MainLineStep_OnClick_GlodDrawCard)
            {
                Debug.LogError("Show.....................MainStep_DrawCard_GetFans");
                //SendMessage(new Message(MessageConst.TO_GUIDE_DRAWCARD_GOLD, Message.MessageReciverType.DEFAULT));
                _controller.SetShow(true);
            }
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
