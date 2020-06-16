using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core;
using Common;
using QFramework.Example;
using UnityEngine;


public enum  OpenCoaxSleepAniType
{
    None,
    EnterInto,         //刚进入
    MainOnClick,       //哄睡主界面点击
    PlayerViewOnClick, //角色界面点击
}


public class CoaxSleepModule : ModuleBase
{
    private OpenCoaxSleepAniType _curType;
    
    private CoaxSleepMainPanel _mainPanel;
    private CoaxSleepAniPanel _aniPanel;
    private CoaxSleepPlayerAudioPanel _playerAudioPanel;
    private CoaxSleepOnPlayAudioPanel _onPlayAudioPanel;
    
    public override void Init()
    {   
        
        
        _curType = OpenCoaxSleepAniType.EnterInto;
        ShowCoaxSleepAniView();                 
    }

        
    public override void OnMessage(Message message)
    {
        string name = message.Name;
        object[] body = message.Params;
        switch (name)
        {
            case MessageConst.CMD_COAXSLEEP_OPEN_ANI:                 
                _curType = (OpenCoaxSleepAniType)message.Body;                  
                ShowCoaxSleepAniView();               
                break;
            case MessageConst.CMD_COAXSLEEP_PLAY_OVER:
                _aniPanel.Destroy();             
                switch (_curType)
                {
                    case OpenCoaxSleepAniType.EnterInto:
                        ShowCoaxSleepMainView();
                        break;
                    case OpenCoaxSleepAniType.MainOnClick:
                        _mainPanel.OnShow();
                        break;
                    case OpenCoaxSleepAniType.PlayerViewOnClick:
                        _playerAudioPanel.OnShow();
                        break;
                }
                break;
            case MessageConst.CMD_COAXSLEEP_GOTO_CUR_PLAYER:
                var player = (PlayerPB) body[0];                      
                ShowCoaxSleepPlayerAudioView(player);
                break;  
            case  MessageConst.CMD_CPAXSLEEP_DESTROY_PANEL:

                
                string panelName = (string) message.Body;
                Debug.LogError("后退1111");
                switch (panelName)
                {
                    case "PlayerAudioPanel":
                        _playerAudioPanel.Destroy();
                        _mainPanel.OnShow();
                        break;
                    case "OnPlayAudioPanel":
                        _onPlayAudioPanel.Destroy();
                        _playerAudioPanel.OnShow();
                        break;
                }
                break;
            case MessageConst.CMD_CPAXSLEEP_SHOW_ON_PLAY_PANEL:
                var data = (MyCoaxSleepAudioData) message.Body;
                ShowCoaxSleepOnPlayAudioView(data);
                break;
            
        }
    }

    public override void OnShow(float delay)
    {
        base.OnShow(delay);
        if (_playerAudioPanel!=null)
        {
            _playerAudioPanel.OnShow();
        }

        if (_onPlayAudioPanel!=null)
        {
             _onPlayAudioPanel.OnShow();
        }
       
    }

    private void ShowCoaxSleepAniView()
    {
       
        _aniPanel = new CoaxSleepAniPanel();
        _aniPanel.Init(this);      
        _aniPanel.Show(0);       
    }

    
    private void ShowCoaxSleepMainView()
    {
        GuideManager.RegisterModule(this);
        _mainPanel = new CoaxSleepMainPanel();
        _mainPanel.Init(this);
        _mainPanel.Show(0f);
    }

    private void ShowCoaxSleepPlayerAudioView(PlayerPB player)
    {
         _playerAudioPanel =new CoaxSleepPlayerAudioPanel();
         _playerAudioPanel.Init(this);
         _playerAudioPanel.Show(0f);
         _playerAudioPanel.SetCurPlayerPb(player);        
    }


    private void ShowCoaxSleepOnPlayAudioView(MyCoaxSleepAudioData data)
    {
       _onPlayAudioPanel =new CoaxSleepOnPlayAudioPanel();
       _onPlayAudioPanel.Init(this);
       _onPlayAudioPanel.Show(0f);
       _onPlayAudioPanel.GetCurData(data);
    }
}
