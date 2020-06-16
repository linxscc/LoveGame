using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core;
using game.tools;
using UnityEngine;
using UnityEngine.UI;

public class CoaxSleepMainView : View
{
    private Transform _bottom;
    private Button _aniBtn;

    private Button _chi;
    private Button _yan;
    private Button _tang;
    private Button _qin;
    
    private void Awake()
    {
        _bottom = transform.Find("Bottom");
        _aniBtn = _bottom.GetButton("AniBtn");
        _chi = _bottom.GetButton("Chi");
        _yan = _bottom.GetButton("Yan");
        _tang = _bottom.GetButton("Tang");
        _qin = _bottom.GetButton("Qin");
         
        _aniBtn.onClick.AddListener(OnAniBtn);               
        _chi.onClick.AddListener((() => SetOnClickPlayerBtn(PlayerPB.ChiYu)));
        _yan.onClick.AddListener((() => SetOnClickPlayerBtn(PlayerPB.YanJi)));
        _tang.onClick.AddListener((() => SetOnClickPlayerBtn(PlayerPB.TangYiChen)));
        _qin.onClick.AddListener((() => SetOnClickPlayerBtn(PlayerPB.QinYuZhe)));
      
        
       
    }

    private void OnAniBtn()
    {
        SendMessage( new Message(MessageConst.CMD_COAXSLEEP_OPEN_ANI,OpenCoaxSleepAniType.MainOnClick, Message.MessageReciverType.DEFAULT));
    }


    private void SetOnClickPlayerBtn(PlayerPB pb)
    {
        SendMessage(new Message(MessageConst.CMD_COAXSLEEP_GOTO_CUR_PLAYER,Message.MessageReciverType.DEFAULT,pb)); 
    }
    
  

   
}
