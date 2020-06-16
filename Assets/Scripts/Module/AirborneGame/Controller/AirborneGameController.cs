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

public class AirborneGameController : AirborneGameUpdateController
{
    public AirborneGameView CurAirborneGameView;
    AirborneGameRunningInfo _runningInfo;

    public override void Start()
    {
        _runningInfo= GetData<AirborneGameModel>().RunningInfo;
        EventDispatcher.AddEventListener<AirborneGameRunningItemVo>(EventConst.AirborneGameItemOnTriggerEnter2D, AirborneGameItemOnTriggerEnter2D);

        new AssetLoader().LoadAudio(AssetLoader.GetBackgrounMusicById("Recording(mix2)"),
                (clip, loader) => { AudioManager.Instance.PlayBackgroundMusic(clip); });


        Input.multiTouchEnabled = true;
    }

    private void AirborneGameItemOnTriggerEnter2D(AirborneGameRunningItemVo vo)
    {
        Debug.Log(" Itemtype  "+ vo.Itemtype + " ReourceId  " + vo.ResourceId);

        switch(vo.Itemtype)
        {
            case ItemTypeEnum.Normal:
            case ItemTypeEnum.Rare:
            case ItemTypeEnum.GirlStar:
            case ItemTypeEnum.Gems:
                _runningInfo.AddWinRunningItem(vo);
                break;
            case ItemTypeEnum.Dead:
                SendMessage(new Message(MessageConst.MODULE_AIRBORNEGAME_OVER_GAME, MessageReciverType.DEFAULT, AirborneGameOverType.Durian));
                break;
            case ItemTypeEnum.Double:
                Debug.LogError("Double");
                _runningInfo.TriggerDouble(_curRunningTime);
                break;
            case ItemTypeEnum.Overtime:
                Debug.LogError("Overtime");
                _runningInfo.MaxTime += 5;//todo 暂时写死
                CurAirborneGameView.ShowAddTime();
                break;
        }
    }

    public void GoBack()
    {
        SendMessage(new Message(MessageConst.MODULE_AIRBORNEGAME_SHOW_PAUSE_PANEL));
    }

    float _curRunningTime = 0;
    public override void Update(float delay) 
    {
        _curRunningTime += delay;

        if(_curRunningTime>= _runningInfo.MaxTime)
        {
            SendMessage(new Message(MessageConst.MODULE_AIRBORNEGAME_OVER_GAME, MessageReciverType.DEFAULT, AirborneGameOverType.OverTime));
            return; 
        }


        if (!_runningInfo.CheckHasRunningItem(_curRunningTime)) 
        {
            return;
        }
        var info = _runningInfo.GetRunningItem(_curRunningTime);
        if (info == null)
        { return; }
        CurAirborneGameView.SetItemDropDown(info);
    }

    public override void Pause()
    {
        CurAirborneGameView.Pause();
    }
    public override void Play()
    {
        CurAirborneGameView.Play();
    }

    public override void Destroy()
    {
        Input.multiTouchEnabled = false;
        AudioManager.Instance.PlayDefaultBgMusic();
        EventDispatcher.RemoveEventListener<AirborneGameRunningItemVo>(EventConst.AirborneGameItemOnTriggerEnter2D, AirborneGameItemOnTriggerEnter2D);
        base.Destroy();

    }
}
