using Assets.Scripts.Framework.GalaSports.Core;
using Common;

namespace Module.Guide.ModuleView.Card
{
    public class CardGuideController : Controller
    {
        public CardGuideView View;

        public override void OnMessage(Message message)
        {
            string name = message.Name;
            object[] body = message.Params;
            switch (name)
            {
                case MessageConst.TO_GUIDE_CARD_LEVELUP:
                case MessageConst.TO_GUIDE_CARD_LEVELUP_RESET:
                    if (GuideManager.TempState == TempState.AchievementOver)
                    {
                        View.DoLevelUp();
                    }
                    else
                    {
                        View.Step3();
                    }
                    break;
            }
        }
    }
}