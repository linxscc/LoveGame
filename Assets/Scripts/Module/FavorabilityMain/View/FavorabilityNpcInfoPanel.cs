using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Common;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Interfaces;
using UnityEngine;

public class FavorabilityNpcInfoPanel : ReturnablePanel
{
   private FavorabilityNpcInfoController _controller;

   public override void Init(IModule module)
   {
      base.Init(module);
      var viewScript = InstantiateView<FavorabilityNpcInfoView>("FavorabilityMain/Prefabs/FavorabilityNpcInfoView");
      _controller = new FavorabilityNpcInfoController {View = viewScript};
      RegisterView(viewScript);
      RegisterController(_controller);
      _controller.Start();
   }
   
   public override void Show(float delay)
   {
     // Main.ChangeMenu(MainMenuDisplayState.HideAll);
      ShowBackBtn();            
   }

   public override void OnBackClick()
   {
      //发消息摧毁这个Panel
      SendMessage(new Message(MessageConst.CMD_FACORABLILITY_DESTROY_PANEL, Message.MessageReciverType.DEFAULT,"NpcInfo"));
   }
}
