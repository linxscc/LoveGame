using System;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Common;
using Assets.Scripts.Framework.GalaSports.Interfaces;

public class CumulativeRechargePanel : ReturnablePanel
{
    CumulativeRechargeController _cumulativerechargemoduleController;

    public override void Init(IModule module)
    {
        base.Init(module);
        var viewScript = (CumulativeRechargeView)InstantiateView<CumulativeRechargeView>("CumulativeRecharge/Prefabs/CumulativeRechargeView");
        _cumulativerechargemoduleController = new CumulativeRechargeController();
        _cumulativerechargemoduleController.View = viewScript;
        RegisterView(viewScript);
        RegisterController(_cumulativerechargemoduleController);
        _cumulativerechargemoduleController.Start();
    }

    public override void Show(float delay)
    {
        _cumulativerechargemoduleController.View.Show(); 
        Main.ChangeMenu(MainMenuDisplayState.ShowTopBar);
        ShowBackBtn();
    }

    public override void Hide()
    {
        _cumulativerechargemoduleController.View.Hide(); 
    }
    
    public override void Destroy()
    {
        base.Destroy();
    }
}
