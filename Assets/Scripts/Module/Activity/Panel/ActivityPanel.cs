using Assets.Scripts.Common;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Interfaces;
using DataModel;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ActivityPanel : ReturnablePanel
{

    private Dictionary<string, Panel> activityPanels = new Dictionary<string, Panel>();
    private Dictionary<string, Panel> tempActivityPanels = new Dictionary<string, Panel>();

    private IModule module;
    private ActivityController  _activityController;
    string path = "Activity/Prefabs/ActivityView";
    public override void Init(IModule module)
    {
        base.Init(module);
       this.module = module;
        var viewScript = InstantiateView<ActivityView>(path);
        _activityController = new ActivityController();
        _activityController.ActivityView = (ActivityView)viewScript;

        RegisterView(viewScript);
        RegisterController(_activityController);
  
        CreateActivityPanels(module);
            
        _activityController.Start();
      
    }


    
    private void CreateActivityPanels(IModule module)
    {
        var list = GlobalData.ActivityModel. GetActivityVoList();
        foreach (var t in list)
        {
            var panels = CreateActivityPanelsFactory.CreateActivityPanels(t.ActivityType);
            string id = t.JumpId;
            activityPanels[id] = panels;
        }    
    }

    public override void Show(float delay)
    {
        
        _activityController.ActivityView.Show(delay);
        Main.ChangeMenu(MainMenuDisplayState.ShowTopBar);
        ShowBackBtn();
    }

    public override void Hide()
    {
        _activityController.ActivityView.Hide();
    }

    public override void Destroy()
    {
        base.Destroy();
    }


    public void HideBackBtnAndTop()
    {
        HideBackBtn();
        Main.ChangeMenu(MainMenuDisplayState.HideAll);
        
    }


    public void ShowBackBtnAndTop()
    {
        ShowBackBtn();
        Main.ChangeMenu(MainMenuDisplayState.ShowTopBar);
    }
    
    public void ControllerActivityPanelsShowAndHide(string id)
    {    
        if (!tempActivityPanels.ContainsKey(id))
        {
            if (!activityPanels.ContainsKey(id))//容错
            {
                id = GlobalData.ActivityModel.ActivitySoleId;
            }
            var panel = activityPanels[id];
            panel.Init(module);
            tempActivityPanels.Add(id, activityPanels[id]);
        }
      
        if (tempActivityPanels.ContainsKey(id))
        {
            tempActivityPanels[id].Show(0.5f);

            if (tempActivityPanels.Count>1)       
            foreach (var key in tempActivityPanels.Keys)
            {
                if (id != key)
                {
                    tempActivityPanels[key].Hide();
                }             
            }
        }
           
    }

   
}
