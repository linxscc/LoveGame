using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;
using System;
using Assets.Scripts.Module;
using UnityEngine.Events;
using game.tools;
using game.main;
using Common;
using DataModel;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Module.Framework.Utils;

public class DrawCardActivityUI : View
{
    private Button _gemstarBtn;
    private Button _gemOnceBtn;
    private Button _gemTenBtn;

    private Text _qualifyCountTxt;
    private Text _countdownText;

    private TimerHandler _countDown = null;

    private void Awake()
    {
        _gemstarBtn = transform.Find("StarBtn").GetComponent<Button>();
        _gemOnceBtn = transform.GetButton("GemOnceBtn");
        _gemTenBtn = transform.GetButton("GemTenBtn");

        _qualifyCountTxt = transform.Find("StarBtn/Star/QualifyCountTxt").GetComponent<Text>();
        _countdownText = transform.Find("coundown/countdown").GetComponent<Text>();

        _gemOnceBtn.onClick.AddListener(OnBtnGemOnceClick);
        _gemTenBtn.onClick.AddListener(OnBtnGemTenCick);
        _gemstarBtn.onClick.AddListener(OnBtnStarCick);
    }


    // Start is called before the first frame update
    void Start()
    {
        _countDown = ClientTimer.Instance.AddCountDown("DrawCardActivityUI", GlobalData.ConfigModel.GetGameTimeConfigByKey(GameConfigKey.DRAW_ACTIVITY_END_TIME), 1, onCountdown, null);
    }

    private void onCountdown(int time)
    {
        if (time >= 0)
            _countdownText.text = I18NManager.Get("ActivityTemplate_ActivityTemplateTime2", DateUtil.GetTimeFormat4(time));
    }

    /// <summary>
    /// 更新星卡Text文本
    /// </summary>
    public void UpdateTicke(int num)
    {
        transform.Find("Ticket/TicketTxt").GetComponent<Text>().text = I18NManager.Get("Common_StarCard") + num.ToString() + I18NManager.Get("Common_Num");
    }

    public void SetData(int gemnum, int gemtotal, int[] costNum, int dropGemNum)
    {
        _qualifyCountTxt.GetComponent<Text>().text = I18NManager.Get("DrawCard_Hint8"); // + gemnum + "/" + gemtotal;
        _gemTenBtn.transform.Find("CostImage/Text").GetComponent<Text>().text = "x " + costNum[(int)DrawTypePB.ByGem10].ToString();
        transform.Find("Ticket/TicketTxt").GetComponent<Text>().text = I18NManager.Get("Common_StarCard") + dropGemNum + I18NManager.Get("Common_Num");

        _gemOnceBtn.transform.Find("CostImage/Image").gameObject.SetActive(true);
        _gemOnceBtn.transform.Find("CostImage/Text").GetComponent<Text>().text = "x " + costNum[(int)DrawTypePB.ByGem].ToString();
    }


    private void OnBtnGemOnceClick()
    {
        SendMessage(new Message(MessageConst.CMD_DRAWCARD_ACTIVITY_DRAW_ONCE));
    }

    private void OnBtnGemTenCick()
    {
        SendMessage(new Message(MessageConst.CMD_DRAWCARD_ACTIVITY_DRAW_TEN));
    }

    private void OnBtnStarCick()
    {
        SendMessage(new Message(MessageConst.MODULE_DRAWCARD_GOTO_CARD_COLLECTION, Message.MessageReciverType.DEFAULT, DrawPoolTypePB.DrawPoolActivity, DrawEventPB.GemBase));
    }

    private void OnDestroy()
    {
        if (_countDown != null)
        {
            ClientTimer.Instance.RemoveCountDown(_countDown);
            _countDown = null;
        }
    }
}
