using System.Linq;
using Assets.Scripts.Framework.GalaSports.Core;
using Com.Proto;
using Common;
using UnityEngine;

namespace Assets.Scripts.Module.Guide.ModuleView
{
    public class MainLineGuideController : Controller
    {
        public MainLineGuideView View;

        public override void OnMessage(Message message)
        {
            string name = message.Name;
            object[] body = message.Params;
            switch (name)
            {
                case MessageConst.TO_GUIDE_MAINLINE_GETDATA:
                    View.gameObject.Show();
                    ClientTimer.Instance.DelayCall(View.HandleStep, 0.5f);
                    break;
                case MessageConst.TO_GUIDE_HIDE_MAINLIENGUIDEVIEW:
                    if (GuideManager.GuideType==false)
                    {
                      View.gameObject.Hide();    
                    }
                    break;

              
            }
        }
    }
}