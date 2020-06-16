using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Core.Events;
using Com.Proto;
using Common;
using game.main;
using UnityEngine;

namespace Assets.Scripts.Module.Guide.ModuleView
{
    public class LoveAppointmentGuideController : Controller
    {
        public LoveAppointmentGuideView View;

        public override void Start()
        {
            EventDispatcher.AddEventListener(EventConst.GuideToLoveStoryGoBack,GuideToGoback);
        }

        private void GuideToGoback()
        {
            //后退引导
            View.gameObject.SetActive(true);
            SetStep_320();
            View.LoveStep3_2();
        }

        public override void OnMessage(Message message)
        {
            string name = message.Name;
            object[] body = message.Params;
            switch (name)
            {
                case MessageConst.MOUDLE_GUIDE_SUPPORTERACT_STARTSUCCESS:
                    View.LoveSetp2((List<AppointmentRuleVo>)message.Body);
                    break;
                case MessageConst.UIDE_LOVEAPPOINTMENT_ENDSUCCESS:
                    Debug.LogError("UIDE_LOVEAPPOINTMENT_ENDSUCCESS");
                    GuideManager.SetRemoteGuideStep(GuideTypePB.MainGuide, GuideConst.MainLineStep_OnClick_LoveStroy_1);
                    
                    UserGuidePB guidePb = new UserGuidePB()
                    {
                        GuideId = GuideConst.MainLineStep_OnClick_LoveStroy_1,
                        GuideType = GuideTypePB.MainGuide
                    };
                    GuideManager.UpdateRemoteGuide(guidePb);
                    break;
            }
        }

        private void SetStep_320()
        {
            Debug.LogError("SetStep_320");
            GuideManager.SetStatisticsRemoteGuideStep(GuideConst.MainLineStep__LoveStroy_1_Over); 
        }
        
        public override void Destroy()
        {
            EventDispatcher.RemoveEventListener(EventConst.GuideToLoveStoryGoBack,GuideToGoback);
        }
    }
}