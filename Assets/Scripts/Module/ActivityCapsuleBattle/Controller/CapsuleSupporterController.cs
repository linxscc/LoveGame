

using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Core.Events;
using Common;
using DataModel;
using Module.Battle.Data;

public class CapsuleSupporterController : Controller
{
    public CapsuleSupporterView view;
    private CapsuleBattleModel _model;
    
    
    public override void Start()
    {
        EventDispatcher.AddEventListener(EventConst.CapsuleBattleUpdateSupporterNum, UpdatePowerData);
        _model = GetData<CapsuleBattleModel>();
        
        view.SetData(_model.LevelVo,GlobalData.DepartmentData.Fanss);
        UpdatePowerData();
    }
    
    
    void UpdatePowerData()
    {
        view.UpdatePowerData();
    }
    
    public override void Destroy()
    {
        base.Destroy();
        EventDispatcher.RemoveEvent(EventConst.CapsuleBattleUpdateSupporterNum);
    }
    

}


