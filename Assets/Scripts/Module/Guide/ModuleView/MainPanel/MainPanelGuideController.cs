
using Assets.Scripts.Framework.GalaSports.Core;
using Com.Proto;
using Common;
using Module.Guide.ModuleView;
using  UnityEngine;
namespace Game.Guide
{
    public class MainPanelGuideController : Controller 
    {
        public MainPanelGuideView View { get; set; }

        public override void OnMessage(Message message)
        {
            string name = message.Name;
            object[] body = message.Params;
            switch (name)
            {
                case MessageConst.CMD_DOWNLOAD_OK:      
                    var curStage = GuideManager.CurStage();
                    View.HandleStep(curStage);
                    break;
                case MessageConst.CMD_HIDEVIEW:
                    View.gameObject.Hide();
                    break;
           
            }
        }
    }
}


