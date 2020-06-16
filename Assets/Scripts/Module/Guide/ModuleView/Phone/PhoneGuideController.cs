using Assets.Scripts.Framework.GalaSports.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Module.Guide.ModuleView.Phone
{
    public class PhoneGuideController : Controller
    {

        // Use this for initialization
        void Start()
        {

        }
        public override void OnMessage(Message message)
        {
            string name = message.Name;
            object[] body = message.Params;
            switch (name)
            {
                case MessageConst.TO_GUIDE_DRAWCARD_RESULT:
                    Debug.LogError("TO_GUIDE_DRAWCARD_RESULT .........");
                    //View.ShowNextStep();
                  //  GuideManager.Show();
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
                case MessageConst.CMD_MIANLINE_GUIDE_PHONE_TO_MAINLINE:
                   
                    View.ShowGoToMainLine(); 
                    break;
            }
        }
        public PhoneGuideView View;

        // Update is called once per frame
        void Update()
        {

        }
    }
}