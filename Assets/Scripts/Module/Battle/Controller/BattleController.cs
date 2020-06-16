using Assets.Scripts.Framework.GalaSports.Core;
using Module.Battle.Data;

public class BattleController : Controller {
    
    public BattleView view;
    private int _power;
    private BattleModel _model;

    public override void Start()
    {
        _model = GetData<BattleModel>();
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
            case MessageConst.CMD_BATTLE_SHOW_SUPPORTER_POWER:
                SendMessage(new Message(MessageConst.CMD_BATTLE_CHANGE_POWER, Message.MessageReciverType.DEFAULT, _model.Power));
                break;
            case MessageConst.CMD_BATTLE_SHOW_BATTLE_WIN_ANIMATION:
               // view.FansHappy();
                break;
            case MessageConst.CMD_BATTLE_SECOND_SHOW_FANS:
                view.SecondShowFans(GetData<BattleModel>().LevelVo.HeroDescription);
                break;
        }
    }
    
    private void ShowSupporterPower()
    {
        
    }
  
}
