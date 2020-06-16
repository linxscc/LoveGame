using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Core.Events;
using Common;
using DataModel;
using game.main;
using Module.Battle.Data;

public class SupporterController : Controller
{
    public SupporterView view;
    private BattleModel _model;

    public override void Start()
    {

        EventDispatcher.AddEventListener(EventConst.UpdateSupporterNum, UpdatePowerData);
        _model = GetData<BattleModel>();
        
        view.SetFansData(_model.LevelVo,GlobalData.DepartmentData.Fanss);
        view.SetGoodsData(_model.LevelVo);
        UpdatePowerData();
    }

    public override void Destroy()
    {
        base.Destroy();
        EventDispatcher.RemoveEvent(EventConst.UpdateSupporterNum);
    }

    void UpdatePowerData()
    {
        view.UpdatePowerData();
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
            case MessageConst.GUIDE_BATTLE_SUPPORTER_OK:
                view.NextStep();
                break;
                
        }
    }
}
