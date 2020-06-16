using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Module.Guide;
using Common;
using game.tools;

namespace Module.Guide.ModuleView.Gameplay
{
    public class GameplayGuideView : View
    {
        private void Awake()
        {
            HandleStep();
        }

        private void HandleStep()
        {
//            if (GuideManager.GuideType==false)
//            {
//                gameObject.Hide();
//                return;
//            }

            if (GuideManager.TempState == TempState.Level3_3_Fail)
            {
                BackToMainUI();
                return;
            }

            var curStage = GuideManager.CurStage();

            var coaxSleepGuide = GuideManager.CurFunctionGuide(GuideTypePB.LoveGuideCoaxSleep);
            if (GuideManager.IsPass4_12() && coaxSleepGuide == FunctionGuideStage.Function_CoaxSleep_OneStage)
            {
                BackToMainUI();
                return;
            }

            if (curStage == GuideStage.Over)
            {
                gameObject.Hide();
                return;
            }

            if (curStage == GuideStage.SevenDaySigninActivityStage ||
                curStage == GuideStage.MainLineStep_Stroy1_9_Over ||
                curStage == GuideStage.MainStep_MainStory2_4_Fail ||
                curStage == GuideStage.FavorabilityShowRoleStage)
                BackToMainUI();
            else if (curStage <= GuideStage.MainLine2_12_Over_Stage ||
                     curStage == GuideStage.MainLineStep_Stroy2_1_Start ||
                     curStage == GuideStage.MainStep_MainStory2_4_Start ||
                     curStage == GuideStage.MainStep_MainStory2_10_Start)
                GuideToMainline();

            // LevelModel levelModel = GlobalData.LevelModel;
            // LevelVo level2_10 = levelModel.FindLevel("2-10");
            // LevelVo level2_11 = levelModel.FindLevel("2-11");
            // if (level2_10.IsPass && level2_11.IsPass == false)
            // {
            //     GuideToMainline();
            // }
            // else
            // {
            //     GuideToMainline();
            // }
            else if (curStage == GuideStage.MainStep_MainStory2_10_Start) GuideToMainline();

            // else if (curStage== GuideStage.SevenDaySigninActivityStage||
            //     curStage== GuideStage.PhoneSmsStage||
            //     curStage== GuideStage.DrawCardStage||             
            //     curStage==GuideStage.FavorabilityShowRoleStage ||
            //     curStage==GuideStage.LoveDiaryStage)//||
            //     //coaxSleepGuide== FunctionGuideStage.Function_CoaxSleep_OneStage && GuideManager.IsPass4_12())             
            // {
            //     BackToMainUI();
            // }
            // else if(curStage== GuideStage.MainLine1_9Level_Over_Stage)
            // {
            //     gameObject.Hide();
            // }
        }

        private void BackToMainUI()
        {
            var view = transform.Find("ClickArea");
            view.gameObject.Show();

            GuideArrow.DoAnimation(view);

            PointerClickListener.Get(view.gameObject).onClick = go =>
            {
                gameObject.Hide();
                ModuleManager.Instance.GoBack();
            };
        }

        private void GuideToMainline()
        {
            var view = transform.Find("Viewport");
            view.gameObject.Show();

            var btn = transform.Find("Viewport/Content/StroyBanner");
            GuideArrow.DoAnimation(btn);

            PointerClickListener.Get(btn.gameObject).onClick = go =>
            {
                SendMessage(new Message(MessageConst.MODULE_GAMEPLAY_GOTO_MAIN_LINE,
                    Message.MessageReciverType.UnvarnishedTransmission));
            };
        }
    }
}