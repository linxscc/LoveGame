using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Common;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Interfaces;
using UnityEngine;

public class FavorabilityGiftPanel : ReturnablePanel
{
    private FavorabilityGiftController _controller;
    public bool IsJump = false;
        
    
    public override void Init(IModule module)
    {
        base.Init(module);
        var viewScript = InstantiateView<FavorabilityGiftView>("FavorabilityMain/Prefabs/FavorabilityGiftView");
        _controller = new FavorabilityGiftController {View = viewScript};
        RegisterController(_controller);
        RegisterView(viewScript);
        _controller.Start();
    }


    public void StarCreate(Transform tra)
    {
        _controller.StarCreate(tra);
    }

    public override void Show(float delay)
    {
        base.Show(0);
       // Main.ChangeMenu(MainMenuDisplayState.HideAll);
        ShowBackBtn(); 
    }

    public override void OnBackClick()
    {
        if (IsJump)
        {
           //发消息直接退出模块 
           SendMessage(new Message(MessageConst.CMD_FACORABLILITY_GOBACK));
        }
        else
        {
            //发消息摧毁这个Panel
            SendMessage(new Message(MessageConst.CMD_FACORABLILITY_DESTROY_PANEL, Message.MessageReciverType.DEFAULT,"SendGift"));
        }
    }
   
}
