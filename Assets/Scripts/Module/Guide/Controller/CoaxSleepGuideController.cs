using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Module.Guide.ModuleView;
using Com.Proto;
using Common;
using DataModel;
using UnityEngine;
public class CoaxSleepGuideController : Controller
{
   public CoaxSleepGuideView View;


   public override void Init()
   {
      var curStage = GuideManager.CurFunctionGuide(GuideTypePB.LoveGuideCoaxSleep);
      
      
      if (curStage== FunctionGuideStage.Function_CoaxSleep_OneStage)
      {
         View.ShowMainViewGuide();
      }
      
      
   }
   
   public override void OnMessage(Message message)
   {
      string name = message.Name;
      object[] body = message.Params;
      switch (name)
      {
         case MessageConst.CMD_GUIDE_COAXSLEEP_TOW_STAGE:
            View.ShowOnPlayView();
            break;
      }
   }

   
}
