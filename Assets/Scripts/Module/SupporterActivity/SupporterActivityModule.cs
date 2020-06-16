using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Module;
using Common;
using game.main;
using UnityEngine;

public class SupporterActivityModule : ModuleBase
{


    private ActivityChoosePanel _activityChoosePanel;
    private FansDetailPanel _fansDetailPanel;
    
    public override void Init()
    {
        RegisterModel<SupporterActivityModel>();
        _activityChoosePanel=new ActivityChoosePanel();
        _activityChoosePanel.Init(this);
        _activityChoosePanel.Show(0);
        GuideManager.RegisterModule(this);
    }

    public override void OnShow(float delay)
    {
        base.OnShow(delay);
        //有粉丝再后退的话需要刷新！
        
        _fansDetailPanel?.BackReview();   //修正做个非空判断，当进应援活动后直接去点星钻直购（不判断会出空指针）
    }

    public override void OnMessage(Message message)
    {
        string name = message.Name;
        switch (name)
        {
            case MessageConst.CMD_SUPPORTERACTIVITY_SHOW_FANSDETAIL:
                if (_fansDetailPanel==null)
                {
                    _fansDetailPanel=new FansDetailPanel();
                    _fansDetailPanel.Init(this);
                    _fansDetailPanel._fansDetailController.SupporterActivityModel =
                        _activityChoosePanel._activityChooseController.SupporterActivityModel;
                }
                _fansDetailPanel.Show(0);
                _fansDetailPanel.SetData((UserEncourageActVo)message.Body);
                //_fansDetailPanel._fansDetailController.cityLevelModel=RegisterModel<CityLevelModel>();
                break;
            case MessageConst.CMD_SUPPORTERACTIVITY_STARTSUCCESS:
                if (_activityChoosePanel==null)
                {
                    _activityChoosePanel=new ActivityChoosePanel();
                    _activityChoosePanel.Init(this);
                }
                _activityChoosePanel.Show(0);
                _activityChoosePanel.SetUpgrageData((int)message.Body);
                break;
            case MessageConst.CMD_SUPPORTERACTIVITY_GOBACK:
                _fansDetailPanel.Hide();
                _activityChoosePanel.Show(0);
                break;
            case MessageConst.CMD_SUPPORTERACTIVITY_GUIDETOFANSMODULE:
                _activityChoosePanel.GuideToFansDetail();
                break;
            case MessageConst.MODULE_SUPPORTERACTIVITY_GOBACKANDCHANGE:
                _fansDetailPanel.Hide();
                _activityChoosePanel.Show(0);
                _activityChoosePanel.SetActChange((UserEncourageActVo)message.Body);
                break;
        }
    }
    
    public override void Remove(float delay)
    {
        base.Remove(delay);
        
    }
    

}
