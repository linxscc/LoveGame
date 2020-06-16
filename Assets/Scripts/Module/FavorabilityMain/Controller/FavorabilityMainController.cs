using System;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Core.Events;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module;
using Assets.Scripts.Module.NetWork;
using Com.Proto;
using Common;
using DataModel;
using game.main;
using System.Collections.Generic;
using Componets;
using Module.FavorabilityMain.View;
using UnityEngine;
using UnityEngine.UI;

public class FavorabilityMainController : Controller
{

	public FavorabilityMainView FavorabilityMainView; 
    private VoiceWindow _voiceWindow;
    
    
    
    public override void Start()
    {		    
        EventDispatcher.AddEventListener<CardAwardPreInfo>(EventConst.OnClickVoiceItem,OnClickVoiceItem);
    }

    private void OnClickVoiceItem(CardAwardPreInfo info)
    {
        if (_voiceWindow!=null)
        {
            _voiceWindow.OnClickClose();
            Debug.LogError("掉View播放语音的方法");
            FavorabilityMainView.Live2dTiggerVoice(info);
          
        }
    }


    public void SetView()
    {
        FavorabilityMainView.SetData(GlobalData.FavorabilityMainModel.CurrentRoleVo);
    }


  

    public void GoBackToPhoneEvent()
    {
        SendMessage(new Message(MessageConst.MODULE_DISIPOSITION_SHOW_SHOWPHONEEVENT));
    }

    /// <summary>
    /// 处理View消息
    /// </summary>
    /// <param name="message"></param>
    public override void OnMessage(Message message)
    {
        string name = message.Name;
        object[] body = message.Params;
        switch (name)
        {
            case MessageConst.MODULE_DISIPOSITION_SHOW_CLOTHPANEL_BTN:	           
	            ModuleManager.Instance.EnterModule(ModuleConfig.MODULE_RELOADING, false,false);	 
                FavorabilityMainView.HideL2D();
	            break;
            case MessageConst.MODULE_DISIPOSITION_SHOW_MAINVIEW:                       
                ShowMianView();
                break;       
            case MessageConst.MODULE_DISIPOSITION_SHOW_VOICE_WINDOW:
                 ShowVoiceWindow();
                break;
            case  MessageConst. CMD_FACORABLILITY_TIGGER_LIVE2D_VOICE:
                int pbId = (int) body[0];
                int itemId = (int) body[1];
                FavorabilityMainView.Live2dTiggerGiftVoice(pbId,itemId); 
                break;
            case MessageConst.CMD_FACORABLILITY_PLAT_NPC_INFO_VOICE:
                string dialogId = (string) body[0];
                FavorabilityMainView.Live2DTiggerNpcInfoFirstVoice(dialogId);
                break;

        }
    }


    public void JumpTo(string name)
    {      
       FavorabilityMainView.JumpTo(name);    
    }
    
    
    public void ShowVoiceWindow()
    {
        if (_voiceWindow==null)
        {          
            string path = "FavorabilityMain/Prefabs/VoiceWindow";
            _voiceWindow = PopupManager.ShowWindow<VoiceWindow>(path);
            var name = GlobalData.FavorabilityMainModel.GetPlayerName(GlobalData.FavorabilityMainModel.CurrentRoleVo.Player);         
            var list = ExpressoinUtil.GetDialogCollects((int) GlobalData.FavorabilityMainModel.CurrentRoleVo.Player);
             _voiceWindow.SetData(name,list);
        }
    }
    
    /// <summary>
    /// 展示在主界面
    /// </summary>
    private void ShowMianView()
    {
        var apparelMap = GlobalData.FavorabilityMainModel.CurrentRoleVo.Apparel;        
        byte[] buffer = NetWorkManager.GetByteData(new UserSetNpcBgStateReq()
        {
            ApparelMap = {apparelMap},
        });
        NetWorkManager.Instance.Send<UserSetNpcBgStateRes>(CMD.NPC_SETSTATE, buffer, OnBgState);
    }

    private void OnBgState(UserSetNpcBgStateRes res)
    {                
        GlobalData.PlayerModel.PlayerVo.Apparel = res.User.Apparel;
        GlobalData.PlayerModel.PlayerVo.UpDataBGID(res.User.Apparel[0]);
        EventDispatcher.TriggerEvent(EventConst.ChangeRole, res.User.Apparel);      
    }


    public void OnShowMainBtn()
    {
        FavorabilityMainView.OnShowMainBtn();
    }
 

    public override void Destroy()
    {       
        EventDispatcher.RemoveEventListener< CardAwardPreInfo >(EventConst.OnClickVoiceItem,OnClickVoiceItem);       
    }
}
