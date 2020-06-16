using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Module;
using Assets.Scripts.Module.Guide;
using Common;
using DataModel;
using game.tools;

namespace Module.Guide.ModuleView
{
    public class MainPanelGuideView : View
    {
        private void Awake()
        {
            var curStage = GuideManager.CurStage();

            HandleStep(curStage);
        }

        public void HandleStep(GuideStage stage)
        {
            if (GuideManager.TempState == TempState.Level3_3_Fail)
            {
                GuideManager.SetStatisticsRemoteGuideStep(GuideConst.MainLineStep_Enter_Achievement);
                AchievementGuide();
            }
            else if (GuideManager.TempState == TempState.AchievementOver)
            {
                GuideManager.SetStatisticsRemoteGuideStep(GuideConst.MainLineStep_CardView);
                CardBtnGuide();
            }
            else if (stage <= GuideStage.MainLine1_4Level_2_3Level_Stage)
            {
                if (stage == GuideStage.SevenDaySigninActivityStage)
                    SevenDaySiginActivityGuide();
                else if (stage == GuideStage.MainLineStep_Stroy1_9_Over)
                    MissionGuide();
                else
                    StartBtnGuide();
            }
            else
            {
                // case GuideStage.PhoneSmsStage:
                //     PhoneGuideStage();
                //     break;
                // case GuideStage.MainLine1_6Level_1_7Level_Stage:
                //     GuideToMainLine(I18NManager.Get("Guide_MainLine1_6_Star"));
                //     StartBtnGuide();
                //     break;

                switch (stage)
                {
                    case GuideStage.CardLevelUpStage:
                        CardBtnGuide();
                        break;
                    case GuideStage.MainStep_MainStory2_4_Fail:
                        DrawCardBtnGuide();
                        break;
                    case GuideStage.LoveStoryStage:
                        LoveAppointmentBtnGuide(true);
                        break;
                    case GuideStage.MainStep_MainStory2_4_Start:
                    case GuideStage.MainStep_MainStory2_10_Start:
                        StartBtnGuide();
                        break;
                    case GuideStage.FavorabilityShowRoleStage:
                        ChangeRoleBtnGuide();
                        break;
                    case GuideStage.ExtendDownloadStage:
                        ExtendDownloadGuide();
                        break;
                    // case GuideStage.MainLine1_9Level_Over_Stage:                  
                    //     gameObject.Hide();
                    //     break;
                    // case GuideStage.LoveDiaryStage:
                    //     LoveAppointmentBtnGuide(false);
                    //     break;
                    case GuideStage.Over:
                        gameObject.Hide();
                        LoveCoaxSleepGuide();
                        break;
                }
            }
        }

        private void AchievementGuide()
        {
            var achievementBtn = transform.Find("Buttons/RightBtnContent/AchievementBtn");
            GuideArrow.DoAnimation(achievementBtn);
            achievementBtn.gameObject.Show();

            PointerClickListener.Get(achievementBtn.gameObject).onClick = go =>
            {
                SendMessage(new Message(MessageConst.CMD_GOTOACHIEVEMENT,
                    Message.MessageReciverType.UnvarnishedTransmission));
                achievementBtn.gameObject.Hide();
            };
        }

        private void MissionGuide()
        {
            var missionBtn = transform.Find("Buttons/RightBtnContent/MissionBtn");
            GuideArrow.DoAnimation(missionBtn);
            missionBtn.gameObject.Show();

            PointerClickListener.Get(missionBtn.gameObject).onClick = go =>
            {
                // GuideManager.SetStatisticsRemoteGuideStep(GuideConst.MainLineStep_OnClick_Activity_Banner);
                SendMessage(new Message(MessageConst.CMD_TASK_SHOW_DAILYTASK,
                    Message.MessageReciverType.UnvarnishedTransmission));
                missionBtn.gameObject.Hide();
            };
        }


        private void LoveCoaxSleepGuide()
        {
            var isPass4_12 = GuideManager.IsPass4_12();
            var coaxSleepGuide = GuideManager.CurFunctionGuide(GuideTypePB.LoveGuideCoaxSleep);

            if (isPass4_12 && coaxSleepGuide == FunctionGuideStage.Function_CoaxSleep_OneStage)
            {
                gameObject.Show();

                var parent = transform.Find("StartPathBtnContainer");
                var btn = parent.Find("AppointmentBtn");
                GuideArrow.DoAnimation(btn);
                parent.gameObject.Show();
                btn.gameObject.Show();

                PointerClickListener.Get(btn.gameObject).onClick = go =>
                {
                    SendMessage(new Message(MessageConst.CMD_APPOINTMENT_JUMPCHOOSEROLE,
                        Message.MessageReciverType.UnvarnishedTransmission));
                    parent.gameObject.Hide();
                    btn.gameObject.Hide();
                    gameObject.Hide();
                };
            }
        }


        private void ExtendDownloadGuide()
        {
            gameObject.Hide();
        }


        // private void PhoneGuideStage()
        // {
        //     Transform startPathBtnContainer = transform.Find("StartPathBtnContainer");
        //     startPathBtnContainer.gameObject.Show();
        //
        //     Transform phone = startPathBtnContainer.Find("PhoneBtn");
        //     phone.gameObject.Show();
        //
        //     GuideArrow.DoAnimation(phone);
        //     
        //     PointerClickListener.Get(phone.gameObject).onClick = go =>
        //     {
        //         GuideManager.SetStatisticsRemoteGuideStep(GuideConst.MainLineStep_GoTo_Phone);
        //         gameObject.Hide();
        //         ModuleManager.Instance.EnterModule(ModuleConfig.MODULE_PHONE);
        //     };
        // }

        private void SevenDaySiginActivityGuide()
        {
            var activityBar = transform.Find("ActivityBar");
            GuideArrow.DoAnimation(activityBar);
            activityBar.gameObject.Show();

            PointerClickListener.Get(activityBar.gameObject).onClick = go =>
            {
                GuideManager.SetStatisticsRemoteGuideStep(GuideConst.MainLineStep_OnClick_Activity_Banner);
                var id = GlobalData.ActivityModel.GetActivityVo(ActivityType.ActivitySevenDaySignin).JumpId; //唯一标识
                ModuleManager.Instance.EnterModule(ModuleConfig.MODULE_ACTIVITY, false, true, id);
                activityBar.gameObject.Hide();
            };
        }


        private void ChangeRoleBtnGuide()
        {
            var btn = transform.Find("Buttons/ChangeRoleBtn");
            btn.gameObject.Show();
            GuideArrow.DoAnimation(btn);

            PointerClickListener.Get(btn.gameObject).onClick = go =>
            {
                GuideManager.SetStatisticsRemoteGuideStep(GuideConst.MainLineStep_OnClick_FavorabilityBtn);
                SendMessage(new Message(MessageConst.CMD_MAIN_ON_CHANGE_ROLE_BTN,
                    Message.MessageReciverType.UnvarnishedTransmission));
                gameObject.Hide();
            };
        }

        private void DrawCardBtnGuide()
        {
            var btn = transform.Find("Buttons/RightBtnContent/DrawCardBtn");
            btn.gameObject.Show();
            GuideArrow.DoAnimation(btn);
            PointerClickListener.Get(btn.gameObject).onClick = go =>
            {
                GuideManager.SetStatisticsRemoteGuideStep(GuideConst.MainLineStep_OnClick_DrawCard);
                SendMessage(new Message(MessageConst.CMD_MAIN_ON_DRAWCARD_BTN,
                    Message.MessageReciverType.UnvarnishedTransmission));
            };
        }

        private void StartBtnGuide()
        {
            var btn = transform.Find("Buttons/StartBtn");
            btn.gameObject.Show();
            GuideArrow.DoAnimation(btn);
            PointerClickListener.Get(btn.gameObject).onClick = go =>
            {
                SendMessage(new Message(MessageConst.CMD_MAIN_ON_START_BTN,
                    Message.MessageReciverType.UnvarnishedTransmission, false));
            };
        }

        private void CardBtnGuide()
        {
            var btn = transform.Find("Buttons/CardBtn");
            btn.gameObject.Show();

            var guideView = transform.Find("GuideView_CardBtn");
            guideView.gameObject.Show();

            GuideArrow.DoAnimation(btn);

            PointerClickListener.Get(btn.gameObject).onClick = go =>
            {
                GuideManager.SetStatisticsRemoteGuideStep(GuideConst.MainLineStep_OnClick_CardBtn);
                SendMessage(new Message(MessageConst.CMD_MAIN_ON_CARD_BTN,
                    Message.MessageReciverType.UnvarnishedTransmission));
            };
        }


        private void LoveAppointmentBtnGuide(bool loveAppointment = false)
        {
            var btn = transform.Find("StartPathBtnContainer");
            btn.gameObject.Show();

            transform.Find("StartPathBtnContainer/AppointmentBtn").gameObject.Show();

            var guideView = transform.Find("GuideView_LoveBtn");

            if (loveAppointment)
            {
                guideView.gameObject.Show();
                GuideManager.SetStatisticsRemoteGuideStep(GuideConst.MainLineStep_OnClick_LoveBtn);
            }
            else
            {
                guideView.gameObject.Hide();
            }

            var guidetext = transform.GetText("GuideView_LoveBtn/DialogFrame/Text");
            guidetext.text = loveAppointment
                ? I18NManager.Get("Guide_MainLineLoveStory")
                : I18NManager.Get("Guide_LoveDiaryGuide");
            GuideArrow.DoAnimation(transform.Find("StartPathBtnContainer/AppointmentBtn"));


            PointerClickListener.Get(transform.Find("StartPathBtnContainer/AppointmentBtn").gameObject).onClick = go =>
            {
                SendMessage(new Message(MessageConst.CMD_APPOINTMENT_JUMPCHOOSEROLE,
                    Message.MessageReciverType.UnvarnishedTransmission));
            };
        }


        private void GuideToMainLine(string hint)
        {
            var _guideView = transform.GetRectTransform("GuideView_GuideToMainLine");
            var _guideText = transform.GetText("GuideView_GuideToMainLine/DialogFrame/Text");
            _guideText.text = hint;
            _guideView.gameObject.SetActive(true);
        }
    }
}