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

public class ModificationNameWindow : Window
{

    private Button okBtn;
    private Button cancelBtn;
    private InputField input;
    private GameObject isFirstText;
    private GameObject costHint;
    private void Awake()
    {
        okBtn = transform.Find("okBtn").GetComponent<Button>();
        okBtn.onClick.AddListener(OkBtn);
        cancelBtn = transform.Find("cancelBtn").GetComponent<Button>();
        cancelBtn.onClick.AddListener(delegate() { Close(); });
        input = transform.Find("InputField").GetComponent<InputField>();
        input.onValueChanged.AddListener(InputFieldValueChange);
        isFirstText = transform.Find("IsFirstText").gameObject;
        costHint = transform.Find("CostHintText").gameObject;
        isFirstText.gameObject.SetActive(false);
        costHint.gameObject.SetActive(false);
    }

   

    public void SetData(PlayerVo vo)
    {

        if (vo.ExtInfo.ModCount==0)
        {
            isFirstText.gameObject.SetActive(true);
        }
        else
        {
            costHint.gameObject.SetActive(true);
        }
        
    }


    private void OkBtn()
    {
        if (input.text == "") { FlowText.ShowMessage(I18NManager.Get("GameMain_SetPanelHint5")); return; }

        string str = input.text;
        Debug.LogError(str);
        UserModifyNameReq req = new UserModifyNameReq
        {
            NewUserName = str,
        };

        byte[] data = NetWorkManager.GetByteData(req);
        NetWorkManager.Instance.Send<UserModifyNameRes>(CMD.USERC_USERMODIFYNAME, data, res =>
        {
            GlobalData.PlayerModel.UpdateUserMoney(res.UserMoney);
            GlobalData.PlayerModel.UpdataUserGameName(res.User, res.UserExtraInfo);

            EventDispatcher.TriggerEvent(EventConst.UpDataSetPanelName);
            FlowText.ShowMessage(I18NManager.Get("GameMain_SetPanelHint4"));
            EventDispatcher.TriggerEvent(EventConst.UpdataSupporterFansViewName);
            Close();
        });

    }
    private void InputFieldValueChange(string arg0)
    {
        Debug.Log("length:" + arg0.Length + "  content:" + arg0);
        input.text = Util.RegexString1(Util.GetNoBreakingString(arg0));
    }




}
