using System;
using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Module.DrawCard;
using Assets.Scripts.Services;
using Com.Proto;
using DataModel;
using Google.Protobuf.Collections;
using UnityEngine;

public class DrawCardResultController : Controller {
	
	public DrawCardResultView View;

	public override void Start()
    {
    }

    /// <summary>
    /// 处理View消息
    /// </summary>
    /// <param name="message"></param>
    public override void OnMessage(Message message)
    {
        string name = message.Name;
        object[] body = message.Params;
        switch (name)
        {
            
        }
    }

	public void GoBack()
	{
		SendMessage(new Message(MessageConst.MODULE_VIEW_BACK_DRAWCARD));
	}

    public void SetData(List<DrawCardResultVo> awardPbs)
    {
        //List<DrawCardResultVo> VList = new List<DrawCardResultVo>();
        //foreach(var v in awardPbs)
        //{
        //    DrawCardResultVo drawCardResultVo = new DrawCardResultVo(v);

        //    VList.Add(drawCardResultVo);
        //}
        SendMessage(new Message(MessageConst.TO_GUIDE_DRAWCARD_RESULT, Message.MessageReciverType.DEFAULT));
        View.SetData(awardPbs);

        if (GlobalData.RandomEventModel.SsrGet || GlobalData.RandomEventModel.CheckTrigger(7005,7006))
        {
	        new TriggerService()
		        .ShowNewGiftWindow()
		        .Execute();
        }
    }
}
