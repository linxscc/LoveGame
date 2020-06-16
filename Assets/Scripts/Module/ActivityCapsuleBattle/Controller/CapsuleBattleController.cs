using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core;
using Module.Battle.Data;
using UnityEngine;

public class CapsuleBattleController : Controller
{
    public CapsuleBattleView view;
    private int _power;
    private CapsuleBattleModel _model;
    
    public override void Start()
    {
        _model = GetData<CapsuleBattleModel>();
        _model.InitSupporterValue();
        view.InitData(_model);
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
            case MessageConst.CMD_CAPSULEBATTLE_SHOW_SUPPORTER_POWER:
                SendMessage(new Message(MessageConst.CMD_CAPSULEBATTLE_CHANGE_POWER, Message.MessageReciverType.DEFAULT, _model.Power));
                break;
            case MessageConst.CMD_CAPSULEBATTLE_SHOW_BATTLE_WIN_ANIMATION:
                // view.FansHappy();
                break;
            case MessageConst.CMD_CAPSULEBATTLE_SECOND_SHOW_FANS:
                view.SecondShowFans(GetData<CapsuleBattleModel>().LevelVo.HeroDescription);
                break;
        }
    }
}
