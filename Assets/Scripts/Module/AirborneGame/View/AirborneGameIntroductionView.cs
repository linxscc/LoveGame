using Assets.Scripts.Framework.GalaSports.Core;
using game.main;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AirborneGameIntroductionView : View
{
    private void Awake()
    {
        transform.GetButton("StartBtn").onClick.AddListener(() =>
        {
            SendMessage(new Message(MessageConst.CMD_AIRBORNEGAME_START_GAME, Message.MessageReciverType.CONTROLLER));
        });
        transform.GetButton("HelpBtn").onClick.AddListener(() =>
        {
           PopupManager.ShowWindow<AirborneGameIntroductionWindow>("AirborneGame/Prefabs/AirborneGameIntroductionWindow");
        });
    }
    
    public void SetData(AirborneGameInfo gameInfos)
    {
        Text leftTime = transform.Find("LeftUse/LeftUseTxt").GetComponent<Text>();
        leftTime.text  = I18NManager.Get("AirborneGame_IntroductionLeftTime", gameInfos.CurPlayedLeftNum, gameInfos.MaxGameNum);

        Text IntroductionTxt = transform.Find("IntroductionTxt1").GetComponent<Text>();
        if (gameInfos.NextUnlockLevel==int.MaxValue)
        {
            IntroductionTxt.gameObject.Hide();
        }
        else
        {
            IntroductionTxt.gameObject.Show();
            IntroductionTxt.text = I18NManager.Get("AirborneGame_Introduction1", gameInfos.NextUnlockLevel);
        }
      
    }

}
