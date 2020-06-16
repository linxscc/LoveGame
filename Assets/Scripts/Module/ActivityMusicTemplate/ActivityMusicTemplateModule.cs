using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Module;
using UnityEngine;

public class ActivityMusicTemplateModule : ModuleBase
{

    private ActivityMusicTemplatePanel _panel;
    private ActivityMusicExchangeShopPanel _exchangeShopPanel;
    
    public override void Init()
    {
        _panel =new ActivityMusicTemplatePanel();
        _panel.Init(this);
        _panel.Show(0.5f);
    }


    public override void OnShow(float delay)
    {
        base.OnShow(delay);
        _panel.Show(0);
        SendMessage(new Message(MessageConst.CMD_ACTIVITY_MUSIC_TEMPLATE_ON_SHOW_REFRESH));
    }
    
    public override void OnMessage(Message message)
    {
        string name = message.Name;
        object[] body = message.Params;
        switch (name)
        {    
            case MessageConst.CMD_SHOW_ACTIVITYMUSIC_EXCHANGESHOP:
                _exchangeShopPanel =new ActivityMusicExchangeShopPanel();               
                _exchangeShopPanel.Init(this);                
                _exchangeShopPanel.Show(0.5f);
                var model = (ActivityExchangeShopModel)message.Body;
                _exchangeShopPanel.SetModel(model);
                break;
            case MessageConst.CMD_ACTIVITY_MUSCI_BACK_EXCHANGESHOP:                                       
                _exchangeShopPanel?.Destroy();
                _panel.Show(0);
                SendMessage(new Message(MessageConst.CMD_ACTIVITY_MUSIC_TEMPLATE_ON_SHOW_REFRESH)); 
                break;
        }
    }

    


   
}
