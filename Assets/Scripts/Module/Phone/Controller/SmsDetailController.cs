

using System;
using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Core.Events;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module.NetWork;
using Com.Proto;
using Common;
using DataModel;
using game.main;
using Google.Protobuf;
using Google.Protobuf.Collections;
using UnityEngine;

public class SmsDetailController : Controller
{
    public SmsDetailView View;
    public bool IsHide;
    public override void OnMessage(Message message)
    {
        base.OnMessage(message);
        string name = message.Name;
        object[] body = message.Params;
        switch (name)
        {
            case MessageConst.CMD_PHONE_SMS_CHOOSE:
                var data = body[0];
                // View.SetData(data);
                string st = "";
                foreach (var v in (List<int>)body[1])
                {
                    st += v.ToString() + ",";
                }
                Debug.Log("send2server  sceneId   " + (int)body[0] + "    selectIds " + st + "  IsReaded  " + (bool)body[2]);
                SendReadMsg((int) body[0], (List<int>) body[1],(bool) body[2]);
                break;
            case MessageConst.CMD_PHONE_SMS_LISTEN:
                // View.SetData(data);
                SendListenMsg((int)body[0], (int)body[1]);//根据数据判断是否发送
                break;
            case MessageConst.CMD_PHONE_GUIDE_OPEN_INPUTBAR:
                View.GuideSelect();
                break;
            case MessageConst.CMD_PHONE_GUIDE_OPEN_SELECTITEM:
                View.GuideSceneSelect(101);
               // GuideManager.Hide();
                break;
        }
    }

    public override void Start()
    {
        base.Start();
    }
    List<MySmsOrCallVo> _data;
    public void SetData(List<MySmsOrCallVo> data)
    {
        //todo
        //查找显示场景
        _data = data;
        MySmsOrCallVo temp=null;
        MySmsOrCallVo lastTemp = null;//最近一条阅读的短信
        List<MySmsOrCallVo> smsUnfinishOfOther;
        List<MySmsOrCallVo> smsUnfinishOfPlayer; ;
        //List<MySmsOrCallVo> smsUnfinish = new List<MySmsOrCallVo>();
        lastTemp = data.FindLast((item) => { return item.IsReaded == true; });
        smsUnfinishOfPlayer = data.FindAll((item) => { return item.IsReaded == false&& item.IsPlayerTrigger==true; });
        smsUnfinishOfOther = data.FindAll((item) => { return item.IsReaded == false && item.IsPlayerTrigger == false; });

        temp = FindCurShowData(data);
        if(temp!=null)
        {
            if(smsUnfinishOfOther.Contains(temp))
            {
                smsUnfinishOfOther.Remove(temp);
            }
            else if(smsUnfinishOfPlayer.Contains(temp))
            {
                smsUnfinishOfPlayer.Remove(temp);
            }
        }
        else
        {
            if(smsUnfinishOfPlayer.Count==0)
            {
                if(data.Count>0)//显示历史记录
                {
                    //data[data.Count].CreateTime
                    data.Sort((a, b) => { return a.FinishTime.CompareTo(b.FinishTime); });
                    temp = data[data.Count - 1];
                    lastTemp = null;
                }
            }
        }

        //一次只能一个场景id进入 需求更改传入主角未完成的场景ids
        View.SetData(temp, smsUnfinishOfPlayer, smsUnfinishOfOther, lastTemp, data[0].Sender);
    }

    private MySmsOrCallVo FindCurShowData(List<MySmsOrCallVo> list)
    {
        MySmsOrCallVo temp = null;
        temp = list.Find((item) => { return item.IsReaded == false && item.selectIds.Count>0; });
        if(temp!=null)
        {
            return temp;
        }
        temp = list.Find((item) => { return item.IsReaded == false && item.IsPlayerTrigger == false; });
        if (temp != null)
        {
            return temp;
        }
        return temp;
    }

    private void SendReadMsg(int sceneId,List<int> selects,bool isFinish)
    {
        var vo = _data.Find((m) => { return m.SceneId == sceneId; });
        if(vo.IsLocal)
        {
            //todo存储本地
            GlobalData.PhoneData.ReCordSelect(sceneId, selects, isFinish);
            //if(isFinish)
            //{
            //    GuideManager.Show();
            //}
            return;
        }

        var req = new ReadMsgOrCallReq();
        req.SceneId = sceneId;
        req.SelectIds.AddRange(selects);
        req.ReadState = isFinish ? 1 : 0;
        var dataBytes = NetWorkManager.GetByteData(req);
        NetWorkManager.Instance.Send<ReadMsgOrCallRes>(CMD.PHONEC_READ_MSGORCALL,dataBytes,OnReadSmsHandler);
    }

    private void OnReadSmsHandler(ReadMsgOrCallRes obj)
    {    
        GlobalData.PhoneData.UpdateSmsData(obj.MsgOrCall);
        // View
        //  var data = GlobalData.PhoneData.UpdateMsg(obj);
        //  View.SetData(data);
    }

    private void SendListenMsg(int sceneId,int talkId)
    {
        var req = new ListenMsgOrCallReq();
        req.SceneId = sceneId;
        req.TalkId = talkId;
    
        var dataBytes = NetWorkManager.GetByteData(req);
        
        NetWorkManager.Instance.Send<ListenMsgOrCallRes>(CMD.PHONEC_LISTEN_MSGORCALL, dataBytes, OnListenSmsHandler);
    }

    private void OnListenSmsHandler(ListenMsgOrCallRes res)
    {
        GlobalData.PhoneData.UpdateSmsData(res.MsgOrCall);
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