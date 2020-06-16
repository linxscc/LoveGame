using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Module;
using Assets.Scripts.Module.Guide;
using Common;
using Componets;
using DataModel;
using game.tools;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Module.Guide.ModuleView.Card
{
    public class CardGuideView : View
    {
        private int _backStep;

        private void Awake()
        {
            var collectedCardList = transform.Find("CollectedCardList");
            collectedCardList.gameObject.Show();

            var clickArea = collectedCardList.Find("Content/CollectedCardItem/ClickArea");
            GuideArrow.DoAnimation(clickArea);

            PointerClickListener.Get(clickArea.gameObject).onClick = go =>
            {
                var vo = GlobalData.CardModel.UserCardList[0];
                SendMessage(new Message(MessageConst.MODULE_CARD_COLLECTION_SHOW_CARD_DETAIL_VIEW,
                    Message.MessageReciverType.UnvarnishedTransmission, vo));

                collectedCardList.gameObject.Hide();
                
                ClientTimer.Instance.DelayCall(()=>
                {
                    if (GuideManager.TempState == TempState.AchievementOver && CanDoLevelUp() == false)
                    {
                        GuideToStarUp();
                    }
                    else
                    {
                        Step2();
                    }
                }, 0.5f);
            };
        }

       

        private void Step2()
        {
            var upgradeStarContainer = transform.Find("UpgradeLevelContainer");
            upgradeStarContainer.gameObject.Show();

            var btn = transform.Find("UpgradeLevelContainer/UpgradeOneLevel");
            GuideArrow.DoAnimation(btn);

            PointerClickListener.Get(btn.gameObject).onClick = go =>
            {
                GuideManager.SetStatisticsRemoteGuideStep(GuideConst.MainLineStep_OnClick_CardLevelUp);
                var vo = GlobalData.CardModel.UserCardList[0];
                SendMessage(new Message(MessageConst.CMD_CARD_UPGRADE_ONELEVEL,
                    Message.MessageReciverType.UnvarnishedTransmission,
                    vo));
                upgradeStarContainer.gameObject.Hide();
                
                if(GuideManager.TempState == TempState.AchievementOver)
                    GuideManager.SetStatisticsRemoteGuideStep(GuideConst.MainLineStep_CardView_levelUp);
            };
        }

        public void Step3()
        {
            //提示两次！

            //Guide_Card_2
            var guideview = transform.Find("GuideView").gameObject;
            guideview.Show();
            var guidetext = guideview.transform.Find("DialogFrame/Text").GetText();
            guidetext.text =
                I18NManager.Get(
                    "Guide_CardGuideTips1"); //$"星缘<color='#fed877'>20级</color>即可升心，升心后即可解锁更多<color='#fed877'>恋爱剧情</color>";

            var btn = guideview.transform.Find("BG").GetComponent<Button>();
            var i = 0;
            btn.onClick.AddListener(() =>
            {
                switch (i)
                {
                    case 0:
                        guidetext.text = I18NManager.Get("Guide_CardGuideTips2");
                        //"现在，让我们回到<color='#fed877'>主线剧情</color>，去看看新的一天发生了什么事吧";
                        i++;
                        break;
                    case 1:
                        guideview.Hide();
                        OnCardLevelup();
                        break;
                }
            });
        }

        private void OnCardLevelup()
        {
            _backStep = 1;

            var view = transform.Find("LevelupEnd");
            view.gameObject.Show();

            var clickArea = view.Find("ClickArea");
            GuideArrow.DoAnimation(clickArea);

            PointerClickListener.Get(clickArea.gameObject).onClick = go =>
            {
                if (_backStep == 1)
                {
                    _backStep = 2;
                    SendMessage(new Message(MessageConst.MODULE_CARD_COLLECTION_BACK_TO_CARD_LIST_VIEW,
                        Message.MessageReciverType.UnvarnishedTransmission));
                }
                else
                {
                    ModuleManager.Instance.GoBack();
                }
            };
        }

        public void DoLevelUp()
        {
            LoadingOverlay.Instance.Hide();
            
            if (CanDoLevelUp() == false)
            {
                GuideToStarUp();
                return;
            }

            var upgradeStarContainer = transform.Find("UpgradeLevelContainer");
            upgradeStarContainer.gameObject.Show();

            var btn = transform.Find("UpgradeLevelContainer/UpgradeOneLevel");

            PointerClickListener.Get(btn.gameObject).onClick = go =>
            {
                PointerClickListener.Get(btn.gameObject).onClick = null;
                
                var vo = GlobalData.CardModel.UserCardList[0];
                SendMessage(new Message(MessageConst.CMD_CARD_UPGRADE_ONELEVEL,
                    Message.MessageReciverType.UnvarnishedTransmission,
                    vo));
                upgradeStarContainer.gameObject.Hide();
                LoadingOverlay.Instance.Show();
            };
        }

        private void GuideToStarUp()
        {
            Transform container = transform.Find("UpgradeStarContainer");
            container.gameObject.Show();
            
            var guideview = transform.Find("GuideView").gameObject;
            guideview.Show();
            var guidetext = guideview.transform.Find("DialogFrame/Text").GetText();
            guidetext.text = "通过扫荡可以获得升星道具，快去体验一下吧";

            Transform btn = container.Find("NoMaxStar/UpgradeStarProps");

            Transform arrow = btn.Find("PropItem1");
            GuideArrow.DoAnimation(arrow);

            GuideManager.TempState = TempState.NONE;
            
            PointerClickListener.Get(btn.gameObject).onClick = go =>
            {
                GuideManager.SetStatisticsRemoteGuideStep(GuideConst.MainLineStep_CardView_StarUp);
                
                guideview.Hide();
                btn.gameObject.Hide();
                
                var userCardVo = GlobalData.CardModel.UserCardList[0];
                SendMessage(new Message(MessageConst.CMD_CARD_GET_MORE_PROPS, Message.MessageReciverType.UnvarnishedTransmission,
                    userCardVo.GetUpgradeStarProps[0], userCardVo.CardId));
                
                transform.Find("GainPropWindow").gameObject.Show();

                Transform btn1 = transform.Find("GainPropWindow/BtnGroup/GainWayBtn1");
                GuideArrow.DoAnimation(btn1);

                PointerClickListener.Get(btn1.gameObject).onClick = GotoMainline;
            };
        }

        private void GotoMainline(GameObject go)
        {
            PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
            pointerEventData.position = Input.mousePosition;
            List<RaycastResult> result = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerEventData, result);
            if (result.Count > 1)
            {
                for (int i = 1; i < result.Count; i++)
                {
                    if (result[i].gameObject.name == "GainWayBtn1")
                    {
                        EventSystem.current.SetSelectedGameObject(result[i].gameObject);
                        ExecuteEvents.Execute(result[i].gameObject, pointerEventData, ExecuteEvents.pointerClickHandler);
                        break;
                    }
                }
            }
        }

        private bool CanDoLevelUp()
        {
            var vo = GlobalData.CardModel.UserCardList[0];
            var needExp = vo.NeedExp - vo.CurrentLevelExp;
            var smallPropExp = GlobalData.PropModel.GetPropBase(PropConst.CardUpgradePropSmall).Exp;
            var bigPropExp = GlobalData.PropModel.GetPropBase(PropConst.CardUpgradePropBig).Exp;
            var largePropExp = GlobalData.PropModel.GetPropBase(PropConst.CardUpgradePropLarge).Exp;
            var smallProCurNum = GlobalData.PropModel.GetUserProp(PropConst.CardUpgradePropSmall).Num;
            var bigPropCurNum = GlobalData.PropModel.GetUserProp(PropConst.CardUpgradePropBig).Num;
            var largePropCurNum = GlobalData.PropModel.GetUserProp(PropConst.CardUpgradePropLarge).Num;

            if (smallProCurNum * smallPropExp + bigPropCurNum * bigPropExp + largePropCurNum * largePropExp < needExp ||
                smallProCurNum * smallPropExp + bigPropCurNum * bigPropExp + largePropCurNum * largePropExp == 0)
                return false;

            return vo.Level < 20;
        }
    }
}