using Assets.Scripts.Framework.GalaSports.Core;
using Module.Battle.Data;


public class FinalEstimateCommentController : Controller
{
    public FinalEstimateCommentWindow Window;
       
    public override void Start()
    {                   
       Window.SetData(GetData<BattleResultData>(),GetData<BattleModel>().LevelVo);
    }
}
