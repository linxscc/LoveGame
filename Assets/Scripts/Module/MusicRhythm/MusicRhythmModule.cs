using Assets.Scripts.Framework.GalaSports.Core;
using Common;
using DataModel;
using game.main;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Module;
using UnityEngine;

public class MusicRhythmModule : UpdatableModuleBase
{
    MusicRhythmPanel _musicRhythmPanel;
    MusicRhythmCountDownPanel _countDownPanel;
    MusicRhythmResultPanel _resultPanel;
    ActivityMusicVo _musicVo;
    public override void Init()
    {
        base.Init();
       var model=  RegisterModel<MusicRhythmModel>();
        model.InitData(_musicVo);
        _musicRhythmPanel = new MusicRhythmPanel();
        _musicRhythmPanel.Init(this);
        _musicRhythmPanel.Show(0);

        _countDownPanel = new MusicRhythmCountDownPanel();
        _countDownPanel.Init(this);
        _countDownPanel.Show(0);
        _countDownPanel.StartCountDown();
        Input.multiTouchEnabled = true;
    }

    public override void Remove(float delay)
    {
        Input.multiTouchEnabled = false;
        AudioManager.Instance.PlayDefaultBgMusic();
        base.Remove(delay);
    }

    public override void SetData(params object[] paramsObjects)
    {
        if (paramsObjects.Length > 0)
        {
            var vo = (ActivityMusicVo)paramsObjects[0];
            _musicVo = vo;
        }
    }
    public override void OnMessage(Message message)
    {
        string name = message.Name;
        object[] body = message.Params;
        switch (name)
        {
            case MessageConst.MODULE_MUSICRHYRHM_SHOW_COUNTDOWN_PANEL://显示倒计时界面
                break;
            case MessageConst.MODULE_MUSICRHYRHM_RESTARTGAME://显示倒计时界面
                _countDownPanel = new MusicRhythmCountDownPanel();
                _countDownPanel.Init(this);
                _countDownPanel.Show(0);
                _countDownPanel.StartCountDown();
                
                if(_resultPanel!=null)
                {
                    _resultPanel.Destroy();
                    _resultPanel = null;
                }
                break;
            case MessageConst.MODULE_MUSICRHYRHM_ClOSE_COUNTDOWN_PANEL://显示关闭倒计时界面
                if (_countDownPanel != null)
                {
                    _countDownPanel.Destroy();
                    _countDownPanel = null;
                }
                Debug.Log("游戏开始");
                if(_musicRhythmPanel==null)
                {
                    _musicRhythmPanel = new MusicRhythmPanel();
                    _musicRhythmPanel.Init(this);
                }
                _musicRhythmPanel.Show(0);
                OnStart();//游戏开始
                break;
            case MessageConst.MODULE_MUSICRHYRHM_PAUSE_GAME://暂停游戏
                OnPause();//游戏开始
                break;
            case MessageConst.MODULE_MUSICRHYRHM_CONTINUE_GAME://继续游戏
                OnPlay();//游戏开始
                break;
            case MessageConst.MODULE_MUSICRHYRHM_SHOW_RESULT_PANEL://显示结算界面
                Debug.Log("游戏结束");
                OnShutdown();
                if (_resultPanel == null) 
                {
                    _resultPanel = new MusicRhythmResultPanel();
                    _resultPanel.Init(this);
                }
                _resultPanel.Show(0);
                break;
            case MessageConst.MODULE_MUSICRHYRHM_BACK:
                
                if(_musicVo.MusicGameType == MusicGameType.TrainingRoom)
                {
                    ModuleManager.Instance.EnterModule(ModuleConfig.MODULE_TRAININGROOM, true);
                }
                else
                {
                    OnMessage(new Message(MessageConst.MODULE_MUSICRHYRHM_SHOW_RESULT_PANEL));
                    // ModuleManager.Instance.EnterModule(ModuleConfig.MODULE_ACTIVITYMUSICTEMPLATE, true);
                }
                break;
        }
    }
}
