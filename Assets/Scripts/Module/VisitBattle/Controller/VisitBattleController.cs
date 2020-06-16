using Assets.Scripts.Framework.GalaSports.Core;
using Module.VisitBattle.Data;


public class VisitBattleController : Controller
{
    public VisitBattleView view;
    private VisitBattleModel _model;

    public override void Start()
    {
        _model = GetData<VisitBattleModel>();
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
            case MessageConst.CMD_VISITBATTLE_SHOW_SUPPORTER_POWER:
                SendMessage(new Message(MessageConst.CMD_VISITBATTLE_CHANGE_POWER, Message.MessageReciverType.DEFAULT, _model.Power));
                break;
            case MessageConst.CMD_VISITBATTLE_SHOW_BATTLE_WIN_ANIMATION:
                view.FansHappy();
                break;
        }
    }

}
