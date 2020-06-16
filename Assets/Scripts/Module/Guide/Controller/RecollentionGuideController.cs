using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Core.Events;
using Common;
using Module.Guide.ModuleView.Recollection;


namespace Game.Guide
{
    public class RecollentionGuideController : Controller
    {

        public  RecollectionGuideView View;
       

       

        public override void Start()
        {
            View.Step1(); 
            EventDispatcher.AddEventListener(EventConst.RecollentionRewardGetWindowClose, OnShowRecollentionRewardWindowStep);
            EventDispatcher.AddEventListener(EventConst.MemoriesReselatNumWindowClose, OnShowCardDropPropWindowStep);
        }

        /// <summary>
        /// 处理View消息
        /// </summary>
        /// <param name="message"></param>
        public override void OnMessage(Message message)
        {

        }

        private void OnShowRecollentionRewardWindowStep()
        {
            View.Step6();
       
        }

        private void OnShowCardDropPropWindowStep()
        {
            View.Step7();
        
        }

        

        public override void Destroy()
        {
            EventDispatcher.RemoveEvent(EventConst.RecollentionRewardGetWindowClose);
            EventDispatcher.RemoveEvent(EventConst.MemoriesReselatNumWindowClose);
         
        }
    }
}


