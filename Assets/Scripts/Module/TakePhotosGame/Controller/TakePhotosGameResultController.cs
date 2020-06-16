using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Core.Events;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module.NetWork;
using Assets.Scripts.Module.TakePhotosGame.Service;
using Com.Proto;
using Common;
using DataModel;
using Framework.GalaSports.Service;
using game.main;
using Google.Protobuf.Collections;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;
using static Assets.Scripts.Framework.GalaSports.Core.Message;

public class TakePhotosGameResultController : Controller
{
    public TakePhotosGameResultView View { get; internal set; }

    public override void Start()
    {
        base.Start();
        // GetService<TakePhotosGameService>().SetCallback(OnHandleData).Execute();
        var model =GetData<TakePhotosGameModel>();
        ScoreReq req = new ScoreReq();
        req.TakePhotoScore.AddRange(model.GetRunningInfo().GetPhotoResultPb());

        Debug.LogError(req.TakePhotoScore.Count);
        byte[] buffer = NetWorkManager.GetByteData(req);
        NetWorkManager.Instance.Send<ScoreRes>(CMD.TAKEPHOTOC_SCORE, buffer, OnGetGameAwardHandler, OnGetGameAwardErrorHandler);

    }


    private void OnGetGameAwardErrorHandler(HttpErrorVo obj)
    {
        Debug.LogError("OnGetGameAwardHandler Error");

    }

    private void OnGetGameAwardHandler(ScoreRes res)
    {
        OnHandleData(GetData<TakePhotosGameModel>(), res.Awrd);
    }

    private void OnHandleData(TakePhotosGameModel model, RepeatedField<AwardPB> awards)
    {
        List<AwardPB> aw = new List<AwardPB>();
        RewardUtil.AddReward(awards);
        aw.AddRange(awards);
        View.SetData(model.GetRunningInfo(), aw);
    }



}
