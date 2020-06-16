using Assets.Scripts.Common;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Interfaces;
using Module.Battle.Data;

public class CallSuperStarPanel : ReturnablePanel
{
    public override void Init(IModule module)
    {
        SetComplexPanel();
        base.Init(module);

        CallSuperStarView viewScript = (CallSuperStarView)InstantiateView<CallSuperStarView>("Battle/Prefabs/Panels/CallSuperStar");

        RegisterView(viewScript);
        viewScript.SetData(GetData<BattleModel>().LevelVo);
    }
}
