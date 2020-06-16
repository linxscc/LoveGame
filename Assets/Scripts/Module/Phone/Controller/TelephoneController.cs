

using System;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Core.Events;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module.NetWork;
using Com.Proto;
using Common;
using DataModel;
using Google.Protobuf;
using Google.Protobuf.Collections;

public class TelephoneController : Controller
{
    public TelephoneView View;
    public override void Start()
    {
        base.Start();     
        EventDispatcher.AddEventListener<MySmsOrCallVo>(EventConst.PhoneCallItemClick,OnClickMsgItem);       
        var data = GlobalData.PhoneData.NpcCallDic;
        View.SetData(data);
    }

    private void OnClickMsgItem(MySmsOrCallVo data)
    {
        SendMessage(new Message(MessageConst.CMD_PHONE_CALL_SHOWDETAIL,Message.MessageReciverType.DEFAULT,data));
    }

    private void OnReadSmsHandler(ReadMsgOrCallRes obj)
    {
        GlobalData.PhoneData.UpdateCallData(obj.MsgOrCall);
        var data = GlobalData.PhoneData.NpcCallDic;
        View.SetData(data);
    }
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
        var data = GlobalData.PhoneData.NpcCallDic;
        View.SetData(data);
       // View.Refresh();
    }
    public override void Destroy()
    {
        base.Destroy();
        EventDispatcher.RemoveEvent(EventConst.PhoneCallItemClick);
    }
}