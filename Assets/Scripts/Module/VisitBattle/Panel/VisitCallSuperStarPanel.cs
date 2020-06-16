using Assets.Scripts.Common;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Interfaces;
using Module.VisitBattle.Data;


public class VisitCallSuperStarPanel : ReturnablePanel
{
    public override void Init(IModule module)
    {
        SetComplexPanel();
        base.Init(module);

        VisitCallSuperStarView viewScript = (VisitCallSuperStarView)InstantiateView<VisitCallSuperStarView>("Battle/Prefabs/Panels/CallSuperStar");

        RegisterView(viewScript);
        viewScript.SetData(GetData<VisitBattleModel>().LevelVo);
    }
}
