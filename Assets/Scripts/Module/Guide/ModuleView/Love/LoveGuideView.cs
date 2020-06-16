using Assets.Scripts.Framework.GalaSports.Core;
using Common;
using game.tools;
using UnityEngine;

namespace Assets.Scripts.Module.Guide.ModuleView
{
    public class
        LoveGuideView : View
    {
        private Transform _guideView2;
        private Transform lovemain;
        private Transform pageArrow;
        private Transform stroyList;

        private void Awake()
        {
            HandleStep();
        }

        public void HandleStep(bool loveAppointment = false)
        {
            gameObject.Show();
            lovemain = transform.Find("Bg");
            lovemain.gameObject.SetActive(true);

            var curStage = GuideManager.CurStage();
            Debug.LogError("恋爱获取当前阶段===>" + curStage);

            if (curStage == GuideStage.LoveStoryStage)
                LoveAppointmentStep();
            // else if(curStage == GuideStage.LoveDiaryStage )
            // {
            //     LoveDailyStep();   
            // }
            else if (GuideManager.IsPass4_12() && GuideManager.CurFunctionGuide(GuideTypePB.LoveGuideCoaxSleep) ==
                     FunctionGuideStage.Function_CoaxSleep_OneStage)
                StarCoaxSleepGuide();
            else if (curStage == GuideStage.CardLevelUpStage) BackToMainUI();

            //            if (GuideManager.GetRemoteGuideStep(GuideTypePB.MainGuide) < GuideConst.MainStep_Love_LoveAppointment)
//            {
//                LoveAppointmentStep();
//            }
//            else if(GuideManager.GetRemoteGuideStep(GuideTypePB.MainGuide)== GuideConst.MainStep_Favorability_ChangeRole)
//            {
//                LoveDailyStep();
//            }
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


        private void LoveAppointmentStep()
        {
            //bug 要判断是否已经解锁恋爱剧情
            lovemain.Find("GuideStep1").gameObject.SetActive(true);
//            Image arrow = lovemain.Find("GuideStep1/Arrow").GetComponent<Image>();
//            RectTransform rect = arrow.rectTransform;
//            rect.DOLocalMove(new Vector2(rect.localPosition.x - 30.0f,
//                rect.localPosition.y + 30.0f), 0.5f).SetLoops(-1, LoopType.Yoyo);
            GuideArrow.DoAnimation(lovemain.Find("GuideStep1"));

            PointerClickListener.Get(lovemain.Find("GuideStep1/ClickArea").gameObject).onClick = go =>
            {
                SetStep_290();
                //跳转到恋爱剧情模块 
                SendMessage(new Message(MessageConst.GUIDE_LOVEAPPOINTMENT_ENTERLOVECHOOSE));
            };
        }

        private void SetStep_290()
        {
            //  GuideManager.SetStatisticsRemoteGuideStep(GuideConst.MianStep_Love_Star); 
        }

        private void LoveDailyStep()
        {
            //bug 要判断是否已经解锁恋爱剧情
            lovemain.Find("GuideStep2").gameObject.SetActive(true);
            GuideArrow.DoAnimation(lovemain.Find("GuideStep2"));
//            Image arrow = lovemain.Find("GuideStep2/Arrow").GetComponent<Image>();
//            RectTransform rect = arrow.rectTransform;
//            rect.DOLocalMove(new Vector2(rect.localPosition.x + 30.0f,
//                rect.localPosition.y + 30.0f), 0.5f).SetLoops(-1, LoopType.Yoyo);

            PointerClickListener.Get(lovemain.Find("GuideStep2/ClickArea").gameObject).onClick = go =>
            {
                // GuideManager.SetStatisticsRemoteGuideStep(GuideConst.MainStep_LoveDaily_SaveBtn);
                SendMessage(new Message(MessageConst.GUIDE_LOVEAPPOINTMENT_ENTERDAILY));
                gameObject.Hide();
                //进入恋爱不需要打点
                // GuideManager.SetRemoteGuideStep(GuideTypePB.MainGuide, GuideConst.MainStep_LoveDaily_EntryBtn);
            };
        }

        private void StarCoaxSleepGuide()
        {
            var coaxSleep = lovemain.Find("GuideStep3").gameObject;
            coaxSleep.SetActive(true);
            GuideArrow.DoAnimation(coaxSleep.transform);
            var onClick = coaxSleep.transform.Find("ClickArea").gameObject;

            PointerClickListener.Get(onClick).onClick = go =>
            {
                gameObject.Hide();
                SendMessage(new Message(MessageConst.GUIDE_LOVE_COAXSLEEP));
            };
        }
    }
}