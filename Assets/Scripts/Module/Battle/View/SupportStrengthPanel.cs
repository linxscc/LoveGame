
using Assets.Scripts.Common;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Interfaces;

public class SupportStrengthPanel : ReturnablePanel
{
    public override void Init(IModule module)
    {
        base.Init(module);

        SupportStrengthView viewScript = (SupportStrengthView)InstantiateView<SupportStrengthView>("Battle/Prefabs/Panels/SupportStrength");

        RegisterView(viewScript);

        viewScript.SetData();

    }
}

