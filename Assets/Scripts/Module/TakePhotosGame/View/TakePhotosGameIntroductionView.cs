using Assets.Scripts.Framework.GalaSports.Core;
using Com.Proto;
using DG.Tweening;
using game.main;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static Assets.Scripts.Framework.GalaSports.Core.Message;

public class TakePhotosGameIntroductionView : View
{
    Button _start;
    Text _leftUse;
    Button _buy;
    private void Awake()
    {
        _start = transform.GetButton("StartBtn");
        _start.interactable = false;
        _leftUse = transform.GetText("LeftUse/LeftUseTxt");
        _start.onClick.AddListener(()=> {
            StartGame();
        });
        _buy = transform.GetButton("Buy");
        _buy.onClick.AddListener(() =>      
        {
 
            if (_canUse == _maxCount) 
            {
                string showText = I18NManager.Get("TakePhotosGame_LeftLimit");
                PopupManager.ShowAlertWindow(showText).WindowActionCallback = evt =>
                {
                };
                return;
            }

            if (!_isCanbuy)
            {
                string showText = I18NManager.Get("TakePhotosGame_BuyLimit");
                PopupManager.ShowAlertWindow(showText).WindowActionCallback = evt =>
                {

                };
                return;
            }


            Buy();
        });

    }

    private void Buy()
    {
        SendMessage(new Message(MessageConst.CMD_TAKEPHOTOSGAME_BUYTIME_ONCLICK, MessageReciverType.CONTROLLER));
    }
    private void StartGame()
    {
        SendMessage(new Message(MessageConst.CMD_TAKEPHOTOSGAME_START_GAME_ONCLICK, MessageReciverType.CONTROLLER));  
    }


    int _canUse = 0;
    int _maxCount = 0;
    bool _isCanbuy = false;
    public void SetData(int canUse, int maxCount, bool isCanbuy)
    {
        _canUse = canUse;
        _maxCount = maxCount;
        _isCanbuy = isCanbuy;
        _start.interactable = true;
        _leftUse.text = I18NManager.Get("TakePhotosGame_IntroductionContent3", canUse, maxCount);
        //if(canUse==maxCount)
        //{
        //    _buy.gameObject.Hide();
        //}
        //else
        //{
        //    _buy.gameObject.Show();
        //}
    }

}
