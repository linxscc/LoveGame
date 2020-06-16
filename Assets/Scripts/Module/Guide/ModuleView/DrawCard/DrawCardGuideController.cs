using Assets.Scripts.Framework.GalaSports.Core;
using Common;
using Module.Guide.ModuleView.DrawCard;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Assets.Scripts.Module.Guide.DrawCard
{
    public class DrawCardGuideController : Controller
    {

        public DrawCardGuideView View;
        public override void OnMessage(Message message)
        {
            string name = message.Name;
            object[] body = message.Params;
            switch (name)
            {
                case MessageConst.TO_GUIDE_DRAWCARD_RESULT:
                    Debug.LogError("TO_GUIDE_DRAWCARD_RESULT .........");
                    View.ShowNextStep();
                    GuideManager.Show();
                    break;
                case MessageConst.TO_GUIDE_DRAWCARD_GOLD:  
                    break;
                case MessageConst.TO_GUIDE_DRAWCARD_GEM:               
                    break;
                case MessageConst.TO_GUIDE_DRAWCARD_SHOWGUIDE:
                    Debug.LogError("TO_GUIDE_DRAWCARD_SHOWGUIDE .........");
                    break;
                case MessageConst.TO_GUIDE_DRAWCARD_HIDEGUIDE:
                    Debug.LogError("TO_GUIDE_DRAWCARD_HIDEGUIDE .........");
                    break;
            }
        }

        bool _isGoldShow = false;
       public  void SetShow(bool isGoldShow=false)
        {
            _isGoldShow = isGoldShow;
            if(isGoldShow)
            {
                Debug.LogError("TO_GUIDE_DRAWCARD_GOLD .........");
                View.InitGoldGuide();
            }
            else
            {
                Debug.LogError("TO_GUIDE_DRAWCARD_GEM .........");
                View.InitGemGuide();
            }
        }
    }

         
}
