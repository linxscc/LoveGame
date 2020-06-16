

using System;
using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Core.Events;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module.NetWork;
using Com.Proto;
using Common;
using DataModel;
using Google.Protobuf;
using Google.Protobuf.Collections;
using UnityEngine;

public class SmsController : Controller
{
    public SmsView View;
    public override void Start()
    {
        base.Start();
        var data = GlobalData.PhoneData.NpcSmsDic;
        View.SetData(data);
        EventDispatcher.AddEventListener<List<MySmsOrCallVo>>(EventConst.PhoneSmsItemClick,OnClickMsgItem);
    }
    
    public override void OnMessage(Message message)
    {
        string name = message.Name;
        object[] body = message.Params;
        switch (name)
        {
            case MessageConst.CMD_PHONE_GUIDE_GOTO_SMSITEM:

               

                var data=  GlobalData.PhoneData.NpcSmsDic[1000];
                SendMessage(new Message(MessageConst.CMD_PHONE_SMS_SHOWDETAIL, Message.MessageReciverType.DEFAULT, data));
                break;
        }
    }

    private void OnClickMsgItem(List<MySmsOrCallVo> data)
    {
        SendMessage(new Message(MessageConst.CMD_PHONE_SMS_SHOWDETAIL,Message.MessageReciverType.DEFAULT,data));
    }

    //private void OnReadSmsHandler(ReadMsgOrCallRes obj)
    //{
    //    GlobalData.PhoneData.UpdateSmsData(obj.MsgOrCall);
    //    var data = GlobalData.PhoneData.NpcSmsDic;
    //    View.SetData(data);
    //}

    public void Hide()
    {
        View.Hide();
    }
    
    public void Show()
    {
        View.Show();
    } 
    
    public void Refresh()
    {
        View.Refresh();
    }

    public override void Destroy()
    {
        base.Destroy();
        EventDispatcher.RemoveEvent(EventConst.PhoneSmsItemClick);
    }
}