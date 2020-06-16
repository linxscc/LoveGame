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

public class AKeyToDeleteWindow : Window
{

    private Text contentText;
    private Button okBtn;
    private Button cancelBtn;
    private GameObject _onClick;
    
    private void Awake()
    {
        _onClick = transform.Find("OnClick").gameObject;
        contentText = transform.Find("Window/ContentText").GetComponent<Text>();
        okBtn = transform.Find("Window/OkBtn").GetComponent<Button>();
        cancelBtn = transform.Find("Window/CancelBtn").GetComponent<Button>();

        contentText.text = I18NManager.Get("Mail_IsOnDeletAllMail");

        PointerClickListener.Get(okBtn.gameObject).onClick = go =>
        {          
            EventDispatcher.TriggerEvent(EventConst.DeleteReadMail);
            Close();
        };

        PointerClickListener.Get(cancelBtn.gameObject).onClick = go =>
        {
            Close();
        };
        
        PointerClickListener.Get(_onClick).onClick = go =>
        {
            Close();
        };
    }


    protected override void OnClickOutside(GameObject go)
    {
      //  base.OnClickOutside(go);
    }
}
