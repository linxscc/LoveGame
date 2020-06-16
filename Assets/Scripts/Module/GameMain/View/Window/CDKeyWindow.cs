using Assets.Scripts.Framework.GalaSports.Core.Events;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module.NetWork;
using Com.Proto;
using Common;
using DataModel;
using game.main;
using game.tools;
using Google.Protobuf.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utils;

public class CDKeyWindow : Window
{
    private Button okBtn;

    private InputField input;


    private ExchangeAwardWindow awardWindow;


    private Transform _hint;

    private void Awake()
    {
        okBtn = transform.Find("okBtn").GetComponent<Button>();
        okBtn.onClick.AddListener(OkBtn);

        _hint = transform.Find("Hint");
        _hint.gameObject.SetActive(false);

        input = transform.Find("InputField").GetComponent<InputField>();
    }

    private void Start()
    {
        ClientData.LoadItemDescData(null);
        ClientData.LoadSpecialItemDescData(null);
    }

    private void OnDestroy()
    {
        ClientData.Clear();
    }

    private void OkBtn()
    {
        if (input.text == "")
        {
            FlowText.ShowMessage(I18NManager.Get("GameMain_SetPanelExchangeHint2"));
            _hint.gameObject.SetActive(false);
            return;
        }

        string code = input.text;


//        if (code.Length<14)
//        {
//            _hint.gameObject.SetActive(true);
//            return;
//        }
//        else
//        {

        ExchangeCodeReq req = new ExchangeCodeReq
        {
            Code = code,
        };

        byte[] data = NetWorkManager.GetByteData(req);
        NetWorkManager.Instance.Send<ExchangeCodeRes>(CMD.USERC_EXCHANGECODE, data, res =>
        {
            _hint.gameObject.SetActive(false);
            //Debug.LogError("兑换码回包奖励长度===>"+res.Award.Count);
            for (int i = 0; i < res.Award.Count; i++)
            {
                RewardUtil.AddReward(res.Award[i]);
            }

            Close();
            ShowExchangeAwardWindow(res.Award);
        });
    }


    private void ShowExchangeAwardWindow(RepeatedField<AwardPB> awards)
    {
        if (awardWindow == null)
        {
            awardWindow = PopupManager.ShowWindow<ExchangeAwardWindow>("GameMain/Prefabs/ExchangeAwardWindow");
            awardWindow.ShowAwards(awards);
        }
    }
}