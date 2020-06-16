using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Core.Events;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module.NetWork;
using Assets.Scripts.Module.TakePhotosGame.Service;
using Com.Proto;
using Common;
using Componets;
using DataModel;
using Framework.GalaSports.Service;
using game.main;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Assets.Scripts.Framework.GalaSports.Core.Message;

public class TakePhotosGameIntroductionController : Controller
{
    public TakePhotosGameIntroductionView View { get; internal set; }

    public override void OnMessage(Message message)
    {
        base.OnMessage(message);
        string name = message.Name;
        object[] body = message.Params;
        switch (name)
        {
            case MessageConst.CMD_TAKEPHOTOSGAME_START_GAME_ONCLICK:
                var model = GetData<TakePhotosGameModel>();
                if (model.GetMaxLimit() - model.GetConsumeTime() <= 0)
                {
                    //string showText = I18NManager.Get("TakePhotosGame_NoLeftLimit",
                    //GetData<TakePhotosGameModel>().BuyConsume());
                    string showText = I18NManager.Get("TakePhotosGame_NoLeftLimit");
                    PopupManager.ShowAlertWindow(showText).WindowActionCallback = evt =>
                    {
                    };
                    return;
                }
                SendStartMessage();
                break;
            case MessageConst.CMD_TAKEPHOTOSGAME_BUYTIME_ONCLICK:

                bool isCanbuy = GetData<TakePhotosGameModel>().isCanBuy();

                if(!isCanbuy)
                {
                    string text = I18NManager.Get("TakePhotosGame_BuyTimesUpperLimit");
                    PopupManager.ShowAlertWindow(text).WindowActionCallback = evt =>
                    {
                    };
                }
                else
                {
                    string showText = I18NManager.Get("TakePhotosGame_IsbuyAndConsume",
                       GetData<TakePhotosGameModel>().BuyConsume());
                    PopupManager.ShowConfirmWindow(showText).WindowActionCallback = evt =>
                    {
                        if (evt == WindowEvent.Ok)
                        {
                            DoBuy();
                        }
                    };
                }
                break;
        }
    }

    private void DoBuy()
    {
        BuyPhotoCountReq req = new BuyPhotoCountReq();
        byte[] buffer = NetWorkManager.GetByteData(req);
        LoadingOverlay.Instance.Show();
        NetWorkManager.Instance.Send<BuyPhotoCountRes>(CMD.TAKEPHOTOC_BUYCOUNT, buffer, OnBuyHandler, OnBuyErrorHandler);      
    }

    private void OnBuyErrorHandler(HttpErrorVo obj)
    {
        LoadingOverlay.Instance.Hide();
    }

    private void OnBuyHandler(BuyPhotoCountRes res)
    {     
        GlobalData.PlayerModel.UpdateUserMoney(res.UserMoney);
        var model = GetData<TakePhotosGameModel>();
        model.UpdateBuyCount(res.UserTakePhotoInfo);
        View.SetData(model.GetMaxLimit() - model.GetConsumeTime(), 
            model.GetMaxLimit(),
            model.isCanBuy());
        LoadingOverlay.Instance.Hide();
    }

    void SendStartMessage()
    {
        LoadingOverlay.Instance.Show();
        StartTakePhotoReq req = new StartTakePhotoReq();
        byte[] buffer = NetWorkManager.GetByteData(req);
        NetWorkManager.Instance.Send<StartTakePhotoRes>(CMD.TAKEPHOTOC_STARTTAKEPHOTO, buffer, OnStartHandler, OnGetGameAwardErrorHandler);
    }

    private void OnGetGameAwardErrorHandler(HttpErrorVo obj)
    {
        LoadingOverlay.Instance.Hide();
    }

    private void OnStartHandler(StartTakePhotoRes res)
    {
        GetData<TakePhotosGameModel>().InitRunningInfo(res);

        SendMessage(new Message(MessageConst.MODULE_TAKEPHOTOSGAME_GOTO_COUNTDOWN_PANEL));
        LoadingOverlay.Instance.Hide();
    }

    public override void Start()
    {
        base.Start();
        GetService<TakePhotosGameService>().SetCallback(OnHandleData).Execute();
        EventDispatcher.AddEventListener(EventConst.DailyRefresh6, Refresh6);
    }

    void Refresh6()
    {
        GetService<TakePhotosGameService>().SetCallback(OnHandleData).Execute();
    }
    public override void Destroy()
    {
        EventDispatcher.RemoveEventListener(EventConst.DailyRefresh6, Refresh6);
        base.Destroy();
    }
    private void OnHandleData(TakePhotosGameModel model)
    {
        View.SetData(model.GetMaxLimit() - model.GetConsumeTime(),
            model.GetMaxLimit(),
            model.isCanBuy()
            );
    }

}
