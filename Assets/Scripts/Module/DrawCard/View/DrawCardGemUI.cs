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

public class DrawCardGemUI : View
{
    private Button _gemstarBtn;
    private Button _gemTenBtn;
    private Button _gemOnceBtn;

    private Transform _ticket;     //星卡XX张。给它绑定个点击事件。进入商城


    private void Awake()
    {
        _gemstarBtn = transform.Find("StarBtn").GetComponent<Button>();
        _gemOnceBtn = transform.Find("GemOnceBtn").GetComponent<Button>();
        _gemTenBtn = transform.Find("GemTenBtn").GetComponent<Button>();
        _ticket = transform.Find("Ticket/OnClick");

        PointerClickListener.Get(_ticket.gameObject).onClick = BuyStarcard;
        _gemstarBtn.onClick.AddListener(() =>
        {  
            //这里要想服务器发送信息接收数据 然后gotoview
            SendMessage(new Message(MessageConst.MODULE_DRAWCARD_GOTO_CARD_COLLECTION, Message.MessageReciverType.DEFAULT, DrawPoolTypePB.DrawPoolCommon,DrawEventPB.GemBase));
        });
        _gemTenBtn.onClick.AddListener(() =>
        {
            SendMessage(new Message(MessageConst.CMD_DRAWCARD_GEM_DRAW_TEN));
        });
        _gemOnceBtn.onClick.AddListener(() =>
        {
            SendMessage(new Message(MessageConst.CMD_DRAWCARD_GEM_DRAW_ONCE));
        });
    }

    // Start is called before the first frame update
    void Start()
    {
        
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
        transform.Find("StarBtn/Star/QualifyCountTxt").GetComponent<Text>().text = I18NManager.Get("DrawCard_Hint8"); // + gemnum + "/" + gemtotal;
        _gemTenBtn.transform.Find("CostImage/Text").GetComponent<Text>().text = "x " + costNum[(int)DrawTypePB.ByGem10].ToString();
        transform.Find("Ticket/TicketTxt").GetComponent<Text>().text = I18NManager.Get("Common_StarCard") + dropGemNum + I18NManager.Get("Common_Num");
    }

    public void SetRemainTime(string str1, int[] costNum)
    {
        if (str1 == "0")
        {
            transform.Find("LeftTime/Text").GetComponent<Text>().text = I18NManager.Get("Common_FreeOnce");
            _gemOnceBtn.transform.Find("CostImage/Image").gameObject.SetActive(false);
            _gemOnceBtn.transform.Find("CostImage/Text").GetComponent<Text>().text = I18NManager.Get("Common_Free");
        }
        else
        {
            transform.Find("LeftTime/Text").GetComponent<Text>().text = str1 + I18NManager.Get("DrawCard_LaterFree");
            _gemOnceBtn.transform.Find("CostImage/Image").gameObject.SetActive(true);
            _gemOnceBtn.transform.Find("CostImage/Text").GetComponent<Text>().text = "x " + costNum[(int)DrawTypePB.ByGem].ToString();
        }
    }


    public void BuyStarcard(GameObject go)
    {
        //点确定按钮后调到商城
        //        if (GuideManager.GetRemoteGuideStep(GuideTypePB.MainGuide) < GuideConst.MainLineStep_Battle1_12_Over) 
        //        {
        //            if (go == null) 
        //            {
        //                FlowText.ShowMessage(I18NManager.Get("DrawCard_Hint2"));
        //            }
        //            return;
        //        }
        string content = go == null ? I18NManager.Get("DrawCard_Hint14") :
              I18NManager.Get("DrawCard_Hint13");

        PopupManager.ShowConfirmWindow(content).WindowActionCallback = evt =>
        {
            if (evt == WindowEvent.Ok)
            {
                ModuleManager.Instance.EnterModule(ModuleConfig.MODULE_SHOP, false, false, 4);
            }
        };
    }
}
