using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Core.Events;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module.NetWork;
using Com.Proto;
using Common;
using DataModel;
using game.main;
using UnityEngine;

public class MusicRhythmController : Controller {

    public MusicRhythmView musicRhythmView;
    MusicRhythmModel _musicRhythmModel;


    AudioClip _clip;//音乐数据

    private AudioClip _clickClip;

    // Use this for initialization
    public override void Start()
    {
        new AssetLoader().LoadAudio(AssetLoader.GetSoundEffectById("progress_bar"), (clip, loader) => { _clickClip = clip; });
        
        EventDispatcher.AddEventListener<Tick>(EventConst.MusicRhythmItemFinishedUse, MusicRhythmItemFinishedUse);
        EventDispatcher.AddEventListener<Tick, MusicRhythmClickCallbackType>(EventConst.MusicRhythmItemShortValidClick, OnMusicRhythmItemShortValidClick);
        EventDispatcher.AddEventListener<Tick, MusicRhythmClickCallbackType>(EventConst.MusicRhythmItemLongValidDownClick, OnMusicRhythmItemLongValidDownClick);
        EventDispatcher.AddEventListener<Tick, MusicRhythmClickCallbackType>(EventConst.MusicRhythmItemLongValidUpClick, OnMusicRhythmItemLongValidUpClick);

        NetWorkManager.Instance.Send<Rules>(CMD.MUSICGAMEC_RULES, null, OnGetMusicGameRule, null, true, GlobalData.VersionData.VersionDic[CMD.MUSICGAMEC_RULES]);
        _musicRhythmModel = GetData<MusicRhythmModel>();
        musicRhythmView.UpdateViewScoreAndCombo(0, 0);
        musicRhythmView.SetPercent(0);

    
    }

    private void OnGetMusicGameRule(Rules obj)
    {
        GetData<MusicRhythmModel>().InitRule(obj);
        string musicId = _musicRhythmModel.MusicId.ToString();
        _clip = null;
        new AssetLoader().LoadAudio(AssetLoader.GetBackgrounMusicById(musicId),
        (clip, loader) =>
        {
            _clip = clip;
        });
        musicRhythmView.SetData(_musicRhythmModel.MusicId, _musicRhythmModel.GetMusicNameById(_musicRhythmModel.MusicId),
        I18NManager.Get("MusicRhythm_Level" + ((int)_musicRhythmModel.Diff).ToString()));
    }

    private void OnMusicRhythmItemLongValidUpClick(Tick tick, MusicRhythmClickCallbackType musicRhythmClickCallbackType)
    {
        musicRhythmView.PlayLongUp(tick);
        if (musicRhythmClickCallbackType == MusicRhythmClickCallbackType.None)
        {
            return;
        }

        Debug.LogError("OnMusicRhythmItemLongValidUpClick" + musicRhythmClickCallbackType);
        HandleClick(musicRhythmClickCallbackType);
    
    }

    public void OnShutdown()
    {
        musicRhythmView.OnShutdown();
        musicRhythmView.UpdateViewScoreAndCombo(0, 0);
        musicRhythmView.SetPercent(0);
        AudioManager.Instance.StopBackgroundMusic();
    }

    private void OnMusicRhythmItemLongValidDownClick(Tick tick, MusicRhythmClickCallbackType musicRhythmClickCallbackType)
    {
        if (musicRhythmClickCallbackType == MusicRhythmClickCallbackType.None)
        {
            return;
        }
        Debug.LogError("OnMusicRhythmItemLongValidDownClick "+ musicRhythmClickCallbackType);
        HandleClick(musicRhythmClickCallbackType);
        musicRhythmView.PlayLongDown(tick);
    }

    void StartInit()
    {
        _musicRhythmModel.InitRunningData(_clip.length);
        musicRhythmView.SetData(_musicRhythmModel.runningInfo.musicId, _musicRhythmModel.runningInfo.musicName,
           _musicRhythmModel.runningInfo.diffName);
        AudioManager.Instance.PlayBackgroundMusic(_clip);
        musicRhythmView.SetPercent(0);

    }

    /// <summary>
    /// 有效点击
    /// </summary>
    /// <param name="tick"></param>
    /// <param name="musicRhythmClickCallbackType"></param>
    private void OnMusicRhythmItemShortValidClick(Tick tick, MusicRhythmClickCallbackType musicRhythmClickCallbackType)
    {
        if (musicRhythmClickCallbackType == MusicRhythmClickCallbackType.None)
        {
            return;
        }
        //Debug.LogError("OnMusicRhythmItemShortValidClick");
        Debug.LogError("OnMusicRhythmItemShortValidClick" + musicRhythmClickCallbackType);
        HandleClick(musicRhythmClickCallbackType);

        if (musicRhythmClickCallbackType == MusicRhythmClickCallbackType.Miss)
        {
            //todo 
            return;
        }
        musicRhythmView.RemoveMusicRhythmItem(tick);
        musicRhythmView.PlayShortOnce(tick);

        AudioManager.Instance.PlayEffect(_clickClip);
    }


    private void HandleClick(MusicRhythmClickCallbackType musicRhythmClickCallbackType)
    {
        musicRhythmView.PlayResult(musicRhythmClickCallbackType);
       // AudioManager.Instance.PlayEffect("cardResolve");
        _musicRhythmModel.runningInfo.AddOperate(musicRhythmClickCallbackType);
        musicRhythmView.UpdateViewScoreAndCombo(
            _musicRhythmModel.runningInfo.Socre,
            _musicRhythmModel.runningInfo.Combo);
    }

    public override void Destroy()
    {
        AssetLoader.UnloadAllAudio();
        Input.multiTouchEnabled = false;
        AudioManager.Instance.PlayDefaultBgMusic();
        EventDispatcher.RemoveEventListener<Tick>(EventConst.MusicRhythmItemFinishedUse, MusicRhythmItemFinishedUse);
        EventDispatcher.RemoveEventListener<Tick, MusicRhythmClickCallbackType>(EventConst.MusicRhythmItemShortValidClick, OnMusicRhythmItemShortValidClick);
        EventDispatcher.RemoveEventListener<Tick,MusicRhythmClickCallbackType > (EventConst.MusicRhythmItemLongValidDownClick, OnMusicRhythmItemLongValidDownClick);
        EventDispatcher.RemoveEventListener<Tick, MusicRhythmClickCallbackType>(EventConst.MusicRhythmItemLongValidUpClick, OnMusicRhythmItemLongValidUpClick);
        base.Destroy();

    }

    private void MusicRhythmItemFinishedUse(Tick tick)
    {
        musicRhythmView.RemoveMusicRhythmItem(tick);
    }

    /// <summary>
    /// 游戏开始
    /// </summary>
    public void OnStart()
    {
        if(_clip==null)
        {
            string musicId = _musicRhythmModel.MusicId.ToString();
            new AssetLoader().LoadAudio(AssetLoader.GetBackgrounMusicById(musicId),
                (clip, loader) =>
                {
                    _clip = clip;
                    StartInit();
                });
        }
        else
        {
            StartInit();
        }   
    }
    // Update is called once per frame
    public  void OnUpdate (float delay) {
        float before = _musicRhythmModel.runningInfo.curRuningTime;
        _musicRhythmModel.runningInfo.curRuningTime += delay;
        float after = _musicRhythmModel.runningInfo.curRuningTime;

        var tick = _musicRhythmModel.CheckSatisfyTick(before, after);

        while(tick != null)
        {
            Debug.Log("before  " + before + "   after" + after);
            musicRhythmView.AddMusicRhythmItem(tick);
            tick = _musicRhythmModel.CheckSatisfyTick(before, after);
        }

        musicRhythmView.OnUpdate(delay);
        float per = after / _musicRhythmModel.runningInfo.musicLen;
        musicRhythmView.SetPercent(per);

        if (after > _musicRhythmModel.runningInfo.musicLen)
        {
            SendMessage(new Message(MessageConst.MODULE_MUSICRHYRHM_SHOW_RESULT_PANEL));
        }
    }
}
