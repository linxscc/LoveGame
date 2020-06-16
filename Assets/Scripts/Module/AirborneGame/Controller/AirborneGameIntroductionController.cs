using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module.NetWork;
using Com.Proto;
using DataModel;
using Framework.GalaSports.Service;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirborneGameIntroductionController : Controller
{
    public AirborneGameIntroductionView View;


    public override void Start()
    {
        //NetWorkManager.Instance.Send<GameJumpInfosRes>(CMD.LITTLEGAMEC_GAMEJUMPINFOS, null, OnGetGameRuleHandler, null, true,
        //    GlobalData.VersionData.VersionDic[CMD.LITTLEGAMEC_GAMEJUMPINFOS]);
        NetWorkManager.Instance.Send<GameJumpInfosRes>(CMD.LITTLEGAMEC_GAMEJUMPINFOS, null, OnGetGameRuleHandler);
    }


    bool isSendMessage = false;
    public override void OnMessage(Message message)
    {
        string name = message.Name;
        object[] body = message.Params;
        switch (name)
        {
            case MessageConst.CMD_AIRBORNEGAME_START_GAME:
                if (isSendMessage)
                    return;
                isSendMessage = true;
                NetWorkManager.Instance.Send<MyGameJumpStartRes>(CMD.LITTLEGAMEC_GAMEJUMPSTART, null, OnGetGameStartHandler, OnGetGameStartErrorHandler);
                break;
        }
    }

    private void OnGetGameStartErrorHandler(HttpErrorVo obj)
    {
        isSendMessage = false;
    }

    private void OnGetGameStartHandler(MyGameJumpStartRes res)
    {
        isSendMessage = false;
        var model= GetData<AirborneGameModel>();
        Debug.Log(res);
        model.InitRuningData(res);
        SendMessage(new Message(MessageConst.MODULE_AIRBORNEGAME_SHOW_COUNTDOWN_PANEL));
    }

    private void OnGetGameRuleHandler(GameJumpInfosRes res)
    {
        //todo
        Debug.Log(res);
        GetData<AirborneGameModel>().InitData(res);
        View.SetData(GetData<AirborneGameModel>().GameInfo);
    }



}
