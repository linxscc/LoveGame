using System.Linq;
using Assets.Scripts.Framework.GalaSports.Core;
using DataModel;
using Module.VisitBattle.Data;

public class VisitFinalEstimateRewardController : Controller
{

    public VisitFinalEstimateRewardView View;
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
            
        }
    }

    public override void Init()
    {
        base.Start();
        VisitBattleResultData data = GetData<VisitBattleResultData>();
        GlobalData.CardModel.UpdateUserCards(data.UserCards.ToArray());
        View.SetData(data, GlobalData.PlayerModel);
    }

    public void Show(float delay)
    {
	
    }

    public void Hide()
    {
		
    }
}
