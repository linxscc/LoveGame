using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module.NetWork;
using Com.Proto;
using DataModel;
using Framework.GalaSports.Service;
using game.main;
using Google.Protobuf;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public class AirborneGameAwardController : Controller
{
    public AirborneGameAwardView AwardView;

    public override void Start()
    {


        if (GetData<AirborneGameModel>().RunningInfo.WinRunningItem.Count == 0)
        {
            PopupManager.ShowAlertWindow(I18NManager.Get("AirborneGame_Hint1")).WindowActionCallback = evt =>
            {
                if (evt == WindowEvent.Ok)
                {
                    SendGet();
                }
            };
            AwardView.gameObject.Hide();
            return;
        }

        AwardView.SetData(GetData<AirborneGameModel>().RunningInfo.WinRunningItem);
    }

    bool isSendMessage = false;
    public override void OnMessage(Message message)
    {
        string name = message.Name;
        object[] body = message.Params;
        switch (name)
        {
            case MessageConst.CMD_AIRBORNEGAME_GET_GAME_AWARD:
                SendGet();
                break;
        }
    }
    private void SendGet()
    {
        if (isSendMessage)
            return;
        isSendMessage = true;
        MyGameJumpEndReq req = new MyGameJumpEndReq();
        req.Items.AddRange(GetData<AirborneGameModel>().RunningInfo.GameJumpItemPBs);
        byte[] buffer = NetWorkManager.GetByteData(req);
        NetWorkManager.Instance.Send<MyGameJumpEndRes>(CMD.LITTLEGAMEC_GAMEJUMPEND, buffer, OnGetGameAwardHandler, OnGetGameAwardErrorHandler);
    }

    private void OnGetGameAwardHandler(MyGameJumpEndRes res)
    {
        RewardUtil.AddReward(res.Awards);
       
        isSendMessage = false;
        ModuleManager.Instance.GoBack();

    }

    private void OnGetGameAwardErrorHandler(HttpErrorVo obj)
    {
        isSendMessage = false;
    }

}
