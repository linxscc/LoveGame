using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Core.Events;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module.NetWork;
using Com.Proto;
using Common;
using DataModel;
using game.main;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Assets.Scripts.Framework.GalaSports.Core.Message;

public class TakePhotosGameController : Controller
{
    public TakePhotosGameView View;

    public TakePhotosGameScoreView scoreView;
    private TakePhotosGameModel _model;
    public override void Start()
    {
        base.Start();

    }

    public override void OnMessage(Message message)
    {
        string name = message.Name;
        object[] body = message.Params;
        switch (name)
        {
            case MessageConst.CMD_TAKEPHOTOS_SHOW_SCORE:
                TakePhotosGameShowState cur = (TakePhotosGameShowState)body[0];

                _model = GetData<TakePhotosGameModel>();
                var vo = AddPhotoResult(cur);
                if (vo == null)
                    return;

                bool isfinished = _model.GetRunningInfo().CheckFinished();
                scoreView.SetData(vo, cur, isfinished);
                scoreView.Show();
                break;
            case MessageConst.CMD_TAKEPHOTOSGAME_SCORE_VIEW_ONCLICK:
                View.UpdateScore(_model.GetRunningInfo().GetCurScore());
                View.ShowCurShowState();
                if (_model.GetRunningInfo().GetCurPhotoVo().isFinished)
                {
                    _model.GetRunningInfo().DoNext();
                    SendMessage(new Message(MessageConst.MODULE_TAKEPHOTOSGAME_GOTO_COUNTDOWN_PANEL));
                }
                break;
        }
    }

    public void GoBack()
    {
        string showText = I18NManager.Get("TakePhotosGame_PauseTips");
        PopupManager.ShowConfirmWindow(showText).WindowActionCallback = evt =>
        {
            if (evt == WindowEvent.Ok)
            {
                SendMessage(new Message(MessageConst.MODULE_TAKEPHOTOSGAME_GOTO_RESULT_PANEL));
            }
        };
    }

    public void SetData()
    {
        scoreView.Hide();
        var m = GetData<TakePhotosGameModel>();
        View.SetData(m.GetRunningInfo());

        View.UpdateScore(m.GetRunningInfo().GetCurScore());

    }

    private TakePhotoGameStateVo AddPhotoResult(TakePhotosGameShowState state)
    {
        var r = GetData<TakePhotosGameModel>().GetRunningInfo();
        switch (state)
        {
            case TakePhotosGameShowState.Scale:
                return r.AddCurPhotoResult(state, CalculationsAccForScale());
            case TakePhotosGameShowState.Move:
                return r.AddCurPhotoResult(state, CalculationsAccForMove());
            case TakePhotosGameShowState.Blur:
                DoGenerateT();
                return r.AddCurPhotoResult(state, CalculationsAccForBlur());
        }
        return null;
    }

    int CalculationsAccForScale()
    {
        float acc = 0;
        float curScale= View.GetScale();
        float target =_model.GetRunningInfo().GetCurPhotoVo().scale;

        float offset = Mathf.Abs(target - curScale);

        float lmax = target - View.ScaleRange[0];
        float tmax = View.ScaleRange[1]-target;
        float maxOffset =
            Mathf.Abs(lmax) > Mathf.Abs(tmax) ? lmax : tmax;
        acc = (maxOffset - offset) / maxOffset;
        int iacc = (int)(acc*100);
        return iacc;
    }
    int CalculationsAccForMove()
    {
        float acc = 0;
        Vector2 curPos = View.GetPos()/ View.GetScale();
        Vector2 target= 
            _model.GetRunningInfo().GetCurPhotoVo().targetPos/ _model.GetRunningInfo().GetCurPhotoVo().scale;
        Vector2 offset = Vector2.zero;
        offset.x = Mathf.Abs(target.x - curPos.x);
        offset.y = Mathf.Abs(target.y - curPos.y);


        Vector2 maxOffset = View.GetSize() * 0.5f;

        float maxLen = Mathf.Sqrt( maxOffset.x * maxOffset.x + maxOffset.y * maxOffset.y)*0.7f;
        float len = Mathf.Sqrt(offset.x * offset.x + offset.y * offset.y)* _model.GetRunningInfo().GetCurPhotoVo().scale;

    
        if(len<=maxLen)
        {
            acc = (maxLen - len) / maxLen;
        }

        int iacc = (int)(acc*100);
        return iacc;
    }


    int CalculationsAccForBlur()
    {
        float acc = 0;
        float blur= View.GetBlur();
        acc = (1 - blur);
        int iacc= (int)(acc * 100);
        return iacc;
    }

    private void DoGenerateT()
    {
        Vector2 pos = View.GetPos();
        float scale = View.GetScale();
        TakePhotosGameRunningInfo info = GetData<TakePhotosGameModel>().GetRunningInfo();
        Texture blur = View.GetTexture();
        Texture2D blur2d = TakePhotosGameRunningInfo.Texture2Texture2D(blur);
        info.playerTexture = TakePhotosGameRunningInfo.GenerateTexture(blur2d,- pos, 500, 500, scale);
        info.GetCurPhotoVo().playerTexture = info.playerTexture;
    }
}
