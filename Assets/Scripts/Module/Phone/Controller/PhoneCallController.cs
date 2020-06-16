

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

public class PhoneCallController : Controller
{
    public PhoneCallView View;
    public bool IsHide;
    public override void OnMessage(Message message)
    {
        base.OnMessage(message);
        string name = message.Name;
        object[] body = message.Params;
        switch (name)
        {
            case MessageConst.CMD_PHONE_TELE_CHOOSE:
                var data = body[0];
                // View.SetData(data);
                
                SendReadMsg((int) body[0], (List<int>) body[1],(bool) body[2]);
                break;
        }
    }

    public override void Start()
    {
        base.Start();   
    }

    public void SetData(MySmsOrCallVo data)
    {
        View.SetData(data);
    }

    //顶部弹出窗口跳入
    public void SetDataById(MySmsOrCallVo data)
    {
        //View.SetData(data);.
        View.SetData(data);
    }

    private void SendReadMsg(int sceneId,List<int> selects,bool isFinish)
    {
        var req = new ReadMsgOrCallReq();
        req.SceneId = sceneId;
        req.SelectIds.AddRange(selects);
        req.ReadState = isFinish ? 1 : 0;
        var dataBytes = NetWorkManager.GetByteData(req);
        NetWorkManager.Instance.Send<ReadMsgOrCallRes>(CMD.PHONEC_READ_MSGORCALL,dataBytes,OnReadTeleHandler);
    }

    private void OnReadTeleHandler(ReadMsgOrCallRes obj)
    {   
        GlobalData.PhoneData.UpdateCallData(obj.MsgOrCall);
    }

    public void Hide()
    {
        View.Hide();
        IsHide = true;
    }
    
    public void Show()
    {
        View.Show();
       
        IsHide = false;
    }
}