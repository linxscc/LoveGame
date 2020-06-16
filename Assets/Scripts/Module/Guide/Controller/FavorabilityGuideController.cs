using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Module.Guide.ModuleView;
using Com.Proto;
using Common;
using DataModel;
using UnityEngine;


namespace Game.Guide
{
    
    public class FavorabilityGuideController : Controller
    {


        public FavorabilityGuideView View;


        public override void Init()
        {
            var stage= GuideManager.CurStage();

            if (stage == GuideStage.FavorabilityShowRoleStage)
            {
                View.ShowChangeRole();  
            }
//
//            if ( stage== GuideStage.Favorability_PhoneEventStage)
//            {
//                View.ShowChangeRolePhoneEventGuide();
//            }      
        }


        public override void OnMessage(Message message)
        {
            string name = message.Name;
            object[] body = message.Params;
            switch (name)
            {
               case MessageConst.TO_GUIDE_FAVORABILITY_PHONE_EVENT_GOBACK_COERCE:
                   
                   var curStep = GuideManager.GetRemoteGuideStep(GuideTypePB.MainGuide);

//                   if (curStep==GuideConst.MainStep_Favorability_PhoneEvent_End)
//                   {                      
//                       View.ShowPhoneEventOverCoerceGuide();
//                   }
                   break;
            }
        }

    }   
}

