using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core;
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

public class DrawCardGoldUI : View
{
    private Button _goldstarBtn;
    private Button _goldTenBtn;
    private Button _goldOnceBtn;

    private void Awake()
    {
        _goldstarBtn = transform.Find("StarBtn").GetComponent<Button>();
        _goldOnceBtn = transform.Find("GoldOnceBtn").GetComponent<Button>();
        _goldTenBtn = transform.Find("GoldTenBtn").GetComponent<Button>();

        _goldstarBtn.onClick.AddListener(() =>
        {
            //这里要想服务器发送信息接收数据 然后gotoview
            SendMessage(new Message(MessageConst.MODULE_DRAWCARD_GOTO_CARD_COLLECTION, Message.MessageReciverType.DEFAULT, DrawPoolTypePB.DrawPoolCommon, DrawEventPB.GoldBase));
        });
        _goldOnceBtn.onClick.AddListener(() =>
        {
            SendMessage(new Message(MessageConst.CMD_DRAWCARD_GOLD_DRAW_ONCE));
        });
        _goldTenBtn.onClick.AddListener(() =>
        {
            SendMessage(new Message(MessageConst.CMD_DRAWCARD_GOLD_DRAW_TEN));
        });
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SetData(int goldnum, int goldtotal, int[] costNum)
    {
        transform.Find("StarBtn/Star/QualifyCountTxt").GetComponent<Text>().text = I18NManager.Get("DrawCard_Hint8"); // + goldnum + "/" + goldtotal;
        _goldTenBtn.transform.Find("CostImage/Text").GetComponent<Text>().text = "x " + costNum[(int)DrawTypePB.ByGold10].ToString();
    }

    public void SetRemainTime(string str2, int[] costNum)
    {
        if (str2 == "0")
        {

            transform.Find("LeftTime/Text").GetComponent<Text>().text = I18NManager.Get("Common_FreeOnce");
            _goldOnceBtn.transform.Find("CostImage/Image").gameObject.SetActive(false);
            _goldOnceBtn.transform.Find("CostImage/Text").GetComponent<Text>().text = I18NManager.Get("Common_Free");

        }
        else
        {

            transform.Find("LeftTime/Text").GetComponent<Text>().text = str2 + I18NManager.Get("DrawCard_LaterFree");
            _goldOnceBtn.transform.Find("CostImage/Image").gameObject.SetActive(true);
            _goldOnceBtn.transform.Find("CostImage/Text").GetComponent<Text>().text = "x " + costNum[(int)DrawTypePB.ByGold].ToString();
        }
    }
}
