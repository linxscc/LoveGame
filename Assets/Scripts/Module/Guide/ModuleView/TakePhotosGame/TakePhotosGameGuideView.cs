using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core;
using Common;
using DataModel;
using DG.Tweening;
using game.main;
using game.tools;
using UnityEngine;
using UnityEngine.UI;
using static Assets.Scripts.Framework.GalaSports.Core.Message;

namespace Assets.Scripts.Module.Guide.ModuleView
{
    public class TakePhotosGameGuideView : View
    {
        Transform _target;
        Transform _bg;
        private void Awake()
        {
            _target = transform.Find("Target");
            _bg = transform.Find("Bg");
            PointerClickListener.Get(transform.gameObject).onClick = go =>
            {
                SendMessage(new Message(MessageConst.MOUDLE_GUIDE_END_LOCAL, ModuleConfig.MODULE_TAKEPHOTOSGAME));
                SendMessage(new Message(MessageConst.GUIDE_TO_TAKEPHOTOSGAME_STARTGAME, MessageReciverType.UnvarnishedTransmission));

            };
        }


        private void Start()
        {
            TakePhotosGameAppointmentStep();
        }

        private void TakePhotosGameAppointmentStep()
        {
            transform.GetText("GuideView/DialogFrame/Text").text = I18NManager.Get("TakePhotosGame_GuideStep1");
            transform.Find("Arrow").gameObject.SetActive(true);
 
            GuideArrow.DoAnimation(transform.Find("Arrow"));
        }

    }
}