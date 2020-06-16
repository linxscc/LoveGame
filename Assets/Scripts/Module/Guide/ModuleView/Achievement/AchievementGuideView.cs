using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Core.Events;
using Common;
using Componets;
using DataModel;
using DG.Tweening;
using GalaAccount.Scripts.Framework.Utils;
using game.main;
using game.tools;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Module.Guide.ModuleView
{
    public class AchievementGuideView : View
    {
        private RectTransform _guideView;
        private Text _guideText;
        private Transform _starroad;
        private Transform _chooseAchievementtran;
        private int _goBackStep = 0;
        private bool _isDoAni = true;

        private void Awake()
        {
//            _guideView = transform.GetRectTransform("GuideView");
//            _guideText = transform.GetText("GuideView/DialogFrame/Text");
//            _guideText.text = "偶像和应援会的一点一滴都会被记录在内，并且可以领取十分重要的奖励";
            _chooseAchievementtran = transform.Find("BtnGroup");
            Step1();

            GuideManager.TempState = TempState.AchievementOver;
        }

        private void Step1()
        {
            Transform btn = _chooseAchievementtran.Find("Role0");
            GuideArrow.DoAnimation(btn);

            PointerClickListener.Get(btn.gameObject).onClick = go =>
            {
                GuideManager.SetStatisticsRemoteGuideStep(GuideConst.MainLineStep_Enter_Achievement2);
                
                //跳转到星路旅程
                SendMessage(new Message(MessageConst.TO_GUIDE_ACHIEVEMENT_ENTERLIST));
                SendMessage(new Message(MessageConst.CMD_CHOOSEROLE,Message.MessageReciverType.UnvarnishedTransmission));
                Step2();
            };
        }

        private void Step2()
        {
            _chooseAchievementtran.gameObject.Hide();

            Transform content = transform.Find("ListContent");
            content.gameObject.Show();

            Step3();
        }

        public void Step3()
        {
            LoadingOverlay.Instance.Hide();
            
            List<UserMissionVo> data = GlobalData.MissionModel.GetMissionByPlayerPB(PlayerPB.None);
            data.Sort();

            if (data.Count > 0 && data[0].Status == MissionStatusPB.StatusUnclaimed)
            {
                Transform btn = transform.Find("ListContent/Achievement/AchievementList/Content/AchievementItem/TaskBtn");
                GameObject o = btn.gameObject;

                if(_isDoAni)
                {
                    GuideArrow.DoAnimation(btn);
                    _isDoAni = !_isDoAni;
                }
                o.Show();
               
                PointerClickListener.Get(o).onClick = go =>
                {
                    PointerClickListener.Get(o).onClick = null;
                    o.Hide();
                    LoadingOverlay.Instance.Show();
                    EventDispatcher.TriggerEvent(EventConst.RecieveAchievementReward, data[0]);
                };
            }
            else
            {
                GuideToBack();
            }
        }

        private void GuideToBack()
        {
            GuideManager.SetStatisticsRemoteGuideStep(GuideConst.MainLineStep_Achievement_Award);
            
            var view = transform.Find("ClickArea");
            view.gameObject.Show();
            
            GuideArrow.DoAnimation(view);
            
            PointerClickListener.Get(view.gameObject).onClick = go =>
            {
                if (_goBackStep == 0)
                {
                    SendMessage(new Message(MessageConst.CMD_ACHIEVEMENTBACK, Message.MessageReciverType.UnvarnishedTransmission));
                    _goBackStep++;
                }
                else
                {
                    ModuleManager.Instance.GoBack();
                }
            };
        }
    }
}