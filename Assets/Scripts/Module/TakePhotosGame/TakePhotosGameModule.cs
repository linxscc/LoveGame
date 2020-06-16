using Assets.Scripts.Framework.GalaSports.Core;
using Common;
using DataModel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakePhotosGameModule : ModuleBase
{
    TakePhotosGamePanel _TakePhotosGamePanel;
    TakePhotosGameCountDownPanel _countDownPanel;
    TakePhotosGameIntroductionPanel _introductionPanel;
    TakePhotosGameResultPanel _resultPanel;
    public override void Init()
    {
        RegisterModel<TakePhotosGameModel>();
        _introductionPanel = new TakePhotosGameIntroductionPanel();
        _introductionPanel.Init(this);
        _introductionPanel.Show(0);
        GuideManager.RegisterModule(this);
        GuideManager.Hide();
    }

    public override void OnMessage(Message message)
    {
        string name = message.Name;
        object[] body = message.Params;
        switch (name)
        {
            case MessageConst.MODULE_TAKEPHOTOSGAME_GOTO_START_PANEL:
                if(_introductionPanel!=null)
                {
                    _introductionPanel.Hide();
                }
                if(_TakePhotosGamePanel==null)
                {
                    _TakePhotosGamePanel = new TakePhotosGamePanel();
                    _TakePhotosGamePanel.Init(this);
                }

                _TakePhotosGamePanel.Show(0);
                break;
            case MessageConst.MODULE_TAKEPHOTOSGAME_GOTO_COUNTDOWN_PANEL:
                if (_introductionPanel != null)
                {
                    _introductionPanel.Hide();
                }
                if(_countDownPanel==null)
                {
                    _countDownPanel = new TakePhotosGameCountDownPanel();
                    _countDownPanel.Init(this);
                }
                _countDownPanel.Show(0);
                break;
            case MessageConst.MODULE_TAKEPHOTOSGAME_GOTO_RESULT_PANEL:
                if (_resultPanel == null)
                {
                    _resultPanel = new TakePhotosGameResultPanel();
                    _resultPanel.Init(this);
                }   
                _resultPanel.Show(0);
                break;
            case MessageConst.MODULE_TAKEPHOTOSGAME_GOTO_INTRODUCTION_PANEL:
                if (_introductionPanel == null)
                {
                    _introductionPanel = new TakePhotosGameIntroductionPanel();
                    _introductionPanel.Init(this);
                } 
                _introductionPanel.Show(0);
                if(_resultPanel!=null)
                {
                    _resultPanel.Hide();
                }
                break;
            case MessageConst.MODULE_TAKEPHOTOSGAME_HIDE_BACKBTN:
                if (_TakePhotosGamePanel != null)
                {
                    _TakePhotosGamePanel.HideBackBtn();
                }
                break;
            case MessageConst.MODULE_TAKEPHOTOSGAME_SHOW_BACKBTN:
                if (_TakePhotosGamePanel != null)
                {
                    _TakePhotosGamePanel.ShowBackBtn();
                }
                break;
        }
    }
}
