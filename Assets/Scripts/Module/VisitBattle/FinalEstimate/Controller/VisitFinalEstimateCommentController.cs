using Assets.Scripts.Framework.GalaSports.Core;
using Module.VisitBattle.Data;

public class VisitFinalEstimateCommentController : Controller
{

    public VisitFinalEstimateCommentView View;
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
        SendMessage(new Message(MessageConst.CMD_VISITBATTLE_SET_FINISH_DATA));
        View.SetData(GetData<VisitBattleResultData>(),GetData<VisitBattleModel>().LevelVo);
    }

    public void Show(float delay)
	{
       
    }

    public void Hide()
    {
		
    }
}
