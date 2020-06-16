using Assets.Scripts.Framework.GalaSports.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Assets.Scripts.Framework.GalaSports.Core.Message;

public class AirborneGamePauseView : View
{
    
    private void Awake()
    {
        transform.GetButton("OkBtn").onClick.AddListener(() => {
      
            SendMessage(new Message(MessageConst.MODULE_AIRBORNEGAME_SHOW_AWARD_PANEL, MessageReciverType.DEFAULT));
        });
        transform.GetButton("CancelBtn").onClick.AddListener(() => {
            SendMessage(new Message(MessageConst.MODULE_AIRBORNEGAME_CLOSE_PAUSE_PANEL));
        });
    }

}
