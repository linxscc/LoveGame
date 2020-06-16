using System.Collections.Generic;
using System.Text;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Core.Events;
using Com.Proto;
using Common;
using DataModel;
using DG.Tweening;
using game.main;
using game.tools;
using Module.Battle.Data;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Module.Guide.ModuleView
{
    public class BattleGuideView : View
    {
        private void Awake()
        {
            gameObject.Hide();
        }

        public void ShowSupport()
        {
            gameObject.Show();
            
            Transform view = transform.Find("Supporter");
            view.gameObject.Show();
            
            Transform okBtn = transform.Find("Supporter/OkBtn");
            
            GuideArrow.DoAnimation(okBtn);
            
            PointerClickListener.Get(okBtn.gameObject).onClick = go =>
            {
                view.gameObject.Hide();
                gameObject.Hide();
                GuideManager.SetStatisticsRemoteGuideStep(GuideConst.MainLineStep_OnClick_Battle1_4_OKBtn);
                SendMessage(new Message(MessageConst.GUIDE_BATTLE_SUPPORTER_OK, Message.MessageReciverType.UnvarnishedTransmission));
            };
        }

        public void ShowSuperStar(LevelVo levelVo)
        {
            gameObject.Show();
            
            Transform view = transform.Find("SuperStar");
            view.gameObject.Show();
            
            Transform guideView1 = transform.Find("SuperStar/GuideView1");
            guideView1.gameObject.Show();

            Text text = transform.GetText("SuperStar/GuideView1/DialogFrame/Text");

            text.text = I18NManager.Get("Guide_Battle2_new");

            Transform needStrengthText = transform.Find("SuperStar/BgTop/NeedStrengthText");
            needStrengthText.gameObject.Show();
            
            GuideArrow.DoAnimation(needStrengthText);

            PointerClickListener.Get(gameObject).onClick = go =>
            {
                PointerClickListener.Get(gameObject).onClick = null;
                ClientTimer.Instance.DelayCall(() =>
                {
                    PickCard();
                    guideView1.gameObject.Hide();
                    needStrengthText.gameObject.Hide();
                }, 0.2f);
            };
        }

        private void PickCard()
        {
            RectTransform bgBottom = transform.Find("SuperStar/BgBottom").GetComponent<RectTransform>();
            bgBottom.gameObject.Show();

            RectTransform rect = GetComponent<RectTransform>();

            RectTransform bgTop = transform.GetRectTransform("SuperStar/BgTop");
            float containerHeight = rect.GetSize().y;
            float topHeight = bgTop.sizeDelta.y;
            float topTop = bgTop.offsetMax.y;
            float bottomHeight = containerHeight - (topHeight - topTop) - 90;

            bgBottom.sizeDelta = new Vector2(bgBottom.sizeDelta.x, bottomHeight);

            Transform guideView = transform.Find("SuperStar/GuideView2");
            guideView.gameObject.Show();
            
            Transform card1 = transform.Find("SuperStar/BgBottom/Card1");

            GuideArrow.DoAnimation(card1);
            
            PointerClickListener.Get(card1.gameObject).onClick = go =>
            {
                UserCardVo userCardVo = GlobalData.CardModel.UserCardList[0];
                BattleUserCardVo vo = new BattleUserCardVo();
                vo.UserCardVo = userCardVo;
                EventDispatcher.TriggerEvent(EventConst.SmallHeroCardClick, vo);
                
                bgBottom.gameObject.Hide();
                guideView.gameObject.Hide();

                ClickOk();
            };
        }

        private void ClickOk()
        {
            Transform view = transform.Find("SuperStar/BgTop/OkBtn");
            view.gameObject.Show();
                       
             GuideArrow.DoAnimation(view);
            PointerClickListener.Get(view.gameObject).onClick = go =>
            {
                SendMessage(new Message(MessageConst.GUIDE_BATTLE_SUPERSTAR_CONFIRM, Message.MessageReciverType.UnvarnishedTransmission));
                view.gameObject.Hide();
                GuideManager.SetStatisticsRemoteGuideStep(GuideConst.MainLineStep_OnClikc_Battle1_4_InvitationBtn);
                gameObject.Hide();
            };
        }
             
        public void ShowReward()
        {
            gameObject.Show();
            
            Transform view = transform.Find("FinalEstimateReward");
            view.gameObject.Show();

            Transform guideView = view.Find("GuideView1");
            guideView.gameObject.Show();

            PointerClickListener.Get(gameObject).onClick = go =>
            {
                guideView.gameObject.Hide();

                PointerClickListener.Get(gameObject).onClick = null;
                
                Transform btn = view.Find("FinishBtn");
                btn.gameObject.Show();

                GuideArrow.DoAnimation(btn);

                PointerClickListener.Get(btn.gameObject).onClick = go1 =>
                {
                    
                    view.gameObject.Hide();
                    SendMessage(new Message(MessageConst.CMD_BATTLE_FINISH, Message.MessageReciverType.UnvarnishedTransmission));
                };
            };
        }

        public void ShowFail3_3()
        {
            PlayerVo playerVo = GlobalData.PlayerModel.PlayerVo;
            string key = ModuleConfig.MODULE_BATTLE + "__" + playerVo.UserId + "__" + AppConfig.Instance.serverId;
            
            if (GuideManager.GetStepState(key, "levelup") == GuideStae.Close)
            {
                GuideManager.Hide();
                return;
            }
            
            gameObject.Show();
            
            GuideManager.SetStepState(GuideStae.Close, key, "levelup");
            GuideManager.TempState = TempState.Level3_3_Fail;
            
            GuideManager.SetStatisticsRemoteGuideStep(GuideConst.MainLineStep_3_3_Fail);
            
            PointerClickListener.Get(gameObject).onClick = go =>
            {
                ModuleManager.Instance.GoBack();
            };
            
            // Transform view = transform.Find("FinalEstimateFail3_3");
            // view.gameObject.Show();
            //
            // var btn = view.Find("Bg/Buttons/GotoCardCollention");
            // GuideArrow.DoAnimation(btn);

           
            // PointerClickListener.Get(btn.gameObject).onClick = go =>
            // {
            //     SendMessage(new Message(MessageConst.CMD_BATTLE_FINISH, Message.MessageReciverType.UnvarnishedTransmission, ModuleConfig.MODULE_CARD));
            // };
        }

        public void ShowFail2_4()
        {
            gameObject.Show();
            
            Transform view = transform.Find("FinalEstimateFail2_4");
            view.gameObject.Show();

            GuideManager.SetRemoteGuideStep(GuideTypePB.MainGuide, GuideConst.MainStep_MainStory2_4_Fail);
            
            //防止网络异常先模拟数据
            UserGuidePB userGuide = new UserGuidePB()
            {
                GuideId = GuideConst.MainStep_MainStory2_4_Fail,
                GuideType = GuideTypePB.MainGuide
            };
            GuideManager.UpdateRemoteGuide(userGuide);
            
            PointerClickListener.Get(view.gameObject).onClick = go =>
            {
                ModuleManager.Instance.GoBack();
            };
        }
    }
}