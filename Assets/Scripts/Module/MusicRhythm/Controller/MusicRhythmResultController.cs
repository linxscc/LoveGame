using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module.NetWork;
using Com.Proto;
using DataModel;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicRhythmResultController : Controller
{
    public MusicRhythmResultView view;

    public override void Start()
    {
        base.Start();
        var mode = GetData<MusicRhythmModel>();
        view.SetData(GetData<MusicRhythmModel>().runningInfo, mode.MusicGameType);
        SendResult();
    }

    void SendResult()
    {
        var mode = GetData<MusicRhythmModel>();

        if (mode.MusicGameType == MusicGameType.Activity)
        {
            var req = new ChallengeActivityMusicReq();
            req.Score = mode.runningInfo.Socre;
            req.MusicId = mode.runningInfo.musicId;
            req.DiffType= mode.runningInfo.diff;
            req.ActivityId = mode.runningInfo.activityId;

            var dataBytes = NetWorkManager.GetByteData(req);
            NetWorkManager.Instance.Send<ChallengeActivityMusicRes>(CMD.ACTIVITYC_CHALLENGEACTIVITYMUSIC, dataBytes, OnSendResultHandler, OnSendResultHandlerError);
        }
        else if(mode.MusicGameType == MusicGameType.TrainingRoom)
        {
            EndPlayingReq req = new EndPlayingReq()
            {
                ActivityId = mode.runningInfo.activityId,
                DiffType = (int) mode.runningInfo.diff,
                MusicId = mode.runningInfo.musicId,
                Score = mode.runningInfo.Socre
            };
            var dataBytes = NetWorkManager.GetByteData(req);
            NetWorkManager.Instance.Send<EndPlayingRes>(CMD.MUSICGAMEC_ENDPLAYING, dataBytes, OnEndTrainingRoomGame);
        }
    }

    private void OnEndTrainingRoomGame(EndPlayingRes res)
    {
        view.ShowReward(res.Integral - GlobalData.TrainingRoomModel.GetCurIntegral());
        GlobalData.TrainingRoomModel.UpdateCurIntegral(res.Integral);
    }

    private void OnSendResultHandlerError(HttpErrorVo obj)
    {
        Debug.LogError("OnSendResultHandlerError");
        //throw new NotImplementedException();
    }

    private void OnSendResultHandler(ChallengeActivityMusicRes res)
    {
        //Debug.LogError(res);
    }
}
