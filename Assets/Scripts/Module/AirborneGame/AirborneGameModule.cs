using Assets.Scripts.Framework.GalaSports.Core;
using DataModel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirborneGameModule : ModuleBase
{
    AirborneGamePanel _airborneGamePanel;
    AirborneGameCountDownPanel _airborneGameCountDownPanel;
    AirborneGameIntroductionPanel _introductionPanel;
    AirborneGameTimePanel _gameTimePanel;
    AirborneGamePausePanel _gamePausePanel;
    AirborneGameOverPanel _gameOverPanel;

    AirborneGameAwardPanel _gameAwardPanel;
    AirborneGameTimer _gameTimer;
    public override void Init()
    {
        Debug.LogError("Init");
        //_airborneGamePanel = new AirborneGamePanel();
        //_airborneGamePanel.Init(this);
        //_airborneGamePanel.Show(0);
        RegisterModel<AirborneGameModel>();
        _introductionPanel = new AirborneGameIntroductionPanel();
        _introductionPanel.Init(this);
        _introductionPanel.Show(0);

    }

    public override void OnMessage(Message message)
    {
        string name = message.Name;
        object[] body = message.Params;
        switch (name)
        {
            case MessageConst.MODULE_AIRBORNEGAME_SHOW_COUNTDOWN_PANEL:

                if(_introductionPanel!=null)
                {
                    _introductionPanel.Destroy();
                    _introductionPanel = null;
                }
                if (_airborneGamePanel == null)
                {
                    _airborneGamePanel = new AirborneGamePanel();
                    _airborneGamePanel.Init(this);
                    _gameTimer = _airborneGamePanel.GetAirborneGameTimer();
                }
                _airborneGamePanel.Show(0);
                _airborneGamePanel.HideBackBtn();

                if (_gameTimePanel == null)
                {
                    _gameTimePanel = new AirborneGameTimePanel();
                    _gameTimePanel.Init(this);
                    _gameTimePanel.SetTimer(_gameTimer);
                }
                _gameTimePanel.Show(0);

                if (_airborneGameCountDownPanel == null)
                {
                    _airborneGameCountDownPanel = new AirborneGameCountDownPanel();
                    _airborneGameCountDownPanel.Init(this);
                }
                _airborneGameCountDownPanel.Show(0);

                break;
            case MessageConst.MODULE_AIRBORNEGAME_ClOSE_COUNTDOWN_PANEL:
                if (_airborneGameCountDownPanel != null)
                {
                    _airborneGameCountDownPanel.Destroy();
                    _airborneGameCountDownPanel = null;
                }
                _airborneGamePanel.ShowBackBtn();
                _gameTimer.Play();
                break;
            case MessageConst.MODULE_AIRBORNEGAME_SHOW_PAUSE_PANEL:
                if (_gamePausePanel == null)
                {
                    _gamePausePanel = new AirborneGamePausePanel();
                    _gamePausePanel.Init(this);
                }
                _gamePausePanel.Show(0);
                _gameTimer.Pause();
                //    _airborneGamePanel.HideBackBtn();
                break;
            case MessageConst.MODULE_AIRBORNEGAME_CLOSE_PAUSE_PANEL:
                if (_gamePausePanel != null)
                {
                    _gamePausePanel.Destroy();
                    _gamePausePanel = null;
                }
                _airborneGamePanel.ShowBackBtn();
                _gameTimer.Play();
                break;
            case MessageConst.MODULE_AIRBORNEGAME_OVER_GAME:
                _gameTimer.Pause();
                if (_gameOverPanel == null)
                {
                    _gameOverPanel = new AirborneGameOverPanel();
                    _gameOverPanel.Init(this);
                }
                _gameOverPanel.Show(0);
                AirborneGameOverType overType =(AirborneGameOverType)body[0];
                _gameOverPanel.SetData(overType);
                break;
            case MessageConst.MODULE_AIRBORNEGAME_SHOW_AWARD_PANEL:
    
                if (_gameAwardPanel == null)
                {
                    _gameAwardPanel = new AirborneGameAwardPanel();
                    _gameAwardPanel.Init(this);
                }
                _gameAwardPanel.Show(0);

                if(_gameOverPanel!=null)
                {
                    _gameOverPanel.Destroy();
                    _gameOverPanel = null;
                }
                break;
        }

    }
}
