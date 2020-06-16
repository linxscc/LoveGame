using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Assets.Scripts.Module.Supporter.Data;
using Com.Proto;
using Common;
using DataModel;
using game.main;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpWindow : Window
{

    private Text _level;
    private Text _extraAward;
    private Text _energybefore;
    private Text _energyafter;
    private Text _activebefore;
    private Text _activeAfter;
    private Text _financialbefore;
    private Text _financialafter;
    private Text _resourcebefore;
    private Text _resourceafter;
    private Text _transimissionbefore;
    private Text _transimissionafter;
    private SupporterVo _active;
    private SupporterVo _financial;
    private SupporterVo _resource;
    private SupporterVo _transmission;
    

    private void Awake()
    {
 
        _level=transform.GetText("Title/lv");
        _extraAward = transform.GetText("ExtraAward");
        _energybefore = transform.GetText("Energylimit/BeforeNum");
        _energyafter = transform.GetText("Energylimit/AfterNum");
        _activebefore = transform.GetText("Active/BeforeNum");
        _activeAfter = transform.GetText("Active/AfterNum");
        _financialbefore = transform.GetText("Financial/BeforeNum");
        _financialafter = transform.GetText("Financial/AfterNum");
        _resourcebefore = transform.GetText("Resource/BeforeNum");
        _resourceafter = transform.GetText("Resource/AfterNum");
        _transimissionbefore = transform.GetText("Transimission/BeforeNum");
        _transimissionafter = transform.GetText("Transimission/AfterNum");
        transform.Find("Button").GetButton().onClick.AddListener(() =>
        {
            Close();
        });
    }

    
    /// <summary>
    /// 沟通好规则，再赋值
    /// </summary>
    public void SetData(MyDepartmentData myDepartmentData)
    {
        AudioManager.Instance.PlayEffect("departmentLevelup"); 
        //要知道播放的顺序
        GetSupporterPower(myDepartmentData);
        
        
        _level.text = "Lv."+GlobalData.PlayerModel.PlayerVo.Level;
        
        _extraAward.text =  (GlobalData.ConfigModel.GetConfigByKey(GameConfigKey.UPGRADE_DEPARTMENT_POWER_NUM) * (GlobalData.PlayerModel.PlayerVo.Level - GlobalData.PlayerModel.PlayerVo.PreLevel)).ToString();
        _energybefore.text = (GlobalData.PlayerModel.PlayerVo.MaxEnergy - 1).ToString();
            
//            (GlobalData.PlayerModel.PlayerVo.Energy -
//             20 * (GlobalData.PlayerModel.PlayerVo.Level - GlobalData.PlayerModel.PlayerVo.PreLevel)).ToString();
        _energyafter.text =GlobalData.PlayerModel.PlayerVo.MaxEnergy.ToString();
        _activeAfter.text = _active.Power.ToString();
        _financialafter.text = _financial.Power.ToString();
        _resourceafter.text = _resource.Power.ToString();
        _transimissionafter.text = _transmission.Power.ToString();
        
        
        //应援会的基础能力也是需要根据等级改变的
//        GlobalData.PlayerModel.BaseSupportPower = MyDepartmentData.GetDepartmentRule(DepartmentTypePB.Support
//            , GlobalData.PlayerModel.PlayerVo.Level).Power;  
        var reduceNum=GlobalData.PlayerModel.BaseSupportPower-MyDepartmentData.GetDepartmentRule(DepartmentTypePB.Support
             , GlobalData.PlayerModel.PlayerVo.PreLevel).Power;
        

        //之前的数据是错误的！
        //有可能出现体力数值错误的！
        
        _activebefore.text = (_active.Power-(reduceNum/4)).ToString();
        _financialbefore.text = (_financial.Power-(reduceNum/4)).ToString();
        _resourcebefore.text = (_resource.Power-(reduceNum/4)).ToString();
        _transimissionbefore.text = (_transmission.Power-(reduceNum/4)).ToString();



    }

    public void GetSupporterPower(MyDepartmentData myDepartmentData)
    {
        for (int i = 0; i < myDepartmentData.MyDepartments.Count; i++)
        {
            UserDepartmentPB pb = myDepartmentData.MyDepartments[i].UserDepartmentPb;
            switch (pb.DepartmentType)
            {
                case DepartmentTypePB.Active:
                    _active=new SupporterVo(pb);
                    break;
                case DepartmentTypePB.Financial:
                    _financial=new SupporterVo(pb);
                    break;
                case DepartmentTypePB.Resource:
                    _resource=new SupporterVo(pb);
                    break;
                case DepartmentTypePB.Transmission:
                    _transmission=new SupporterVo(pb);
                    break;                               
            }

        }
        
        
    }
    
    
}
