using System;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Core.Events;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module;
//using Assets.Scripts.Module.Phone.Data;
using Common;
using game.tools;
using UnityEngine;
using UnityEngine.UI;

public class SmsDetailMyItem:MonoBehaviour
{

    SmsTalkInfo _data;


    private void Awake()
    {

    }

    public void SetData(SmsTalkInfo data)
    {
        string headPath = PhoneData.GetHeadPath(data.NpcId);
        Debug.LogError("SmsDetailMyItem  SetData  " + data.MusicID + " Headpath " + headPath);
        transform.Find("LeftHeadIcon/Image").GetComponent<RawImage>().texture= ResourceManager.Load<Texture>(headPath, ModuleConfig.MODULE_PHONE);
        _data = data;
        transform.Find("Msg/MsgText").GetComponent<Text>().text =_data.TalkContent;
        AjustSize();
    }
    
    public void SetMsg(string msg)
    {
        transform.Find("Msg/MsgText").GetComponent<Text>().text = msg;
        AjustSize();
    }

    private void AjustSize()
    {
        var widthText = transform.Find("Msg/MsgText").GetComponent<Text>();
       // Debug.LogError(widthText.preferredHeight+ "        widthText.flexibleWidth "   +      widthText.flexibleWidth);

        if (widthText.preferredWidth > 530)
        {
            widthText.alignment = TextAnchor.UpperLeft;
        }
        //if (widthText.preferredHeight > 42)
        //{
        //    widthText.alignment = TextAnchor.UpperLeft;
        //}

        if (widthText.preferredWidth > 530)
        {
            transform.Find("Msg").GetComponent<RectTransform>().SetSize(new Vector2(560 + 60, widthText.preferredHeight+ 70));
        }
        else
        {
            transform.Find("Msg").GetComponent<RectTransform>().SetSize(new Vector2(widthText.preferredWidth + 110, widthText.preferredHeight + 70));
        }
        GetComponent<RectTransform>().SetHeight(transform.Find("Msg").GetComponent<RectTransform>().GetHeight() + 30);
    }
    
}