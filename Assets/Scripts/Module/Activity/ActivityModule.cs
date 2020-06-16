using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module.NetWork;
using Com.Proto;
using DataModel;
using game.main;
using System.Collections.Generic;
using Common;
using UnityEngine;
using UnityEngine.UI;

public class ActivityModule : ModuleBase
{
    private ActivityPanel _activityPanel;

    public override void Init()
    {      
         _activityPanel = new ActivityPanel();
         _activityPanel.Init(this);
       
        GuideManager.RegisterModule(this);
    }
    public override void OnShow(float delay)
    {
        base.OnShow(delay);
        _activityPanel?.ShowBackBtnAndTop();
        SendMessage(new Message(MessageConst.CMD_ACTIVITY_ONSHOW));
    }

    public override void OnMessage(Message message)
    {

        string name = message.Name;
        object[] body = message.Params;
        switch (name)
        {

            case MessageConst.CMD_ACTIVITY_SHOW_ACTIVITYDATA:
                var id = (string)message.Body;             
                _activityPanel.ControllerActivityPanelsShowAndHide(id);
                break;     
            case MessageConst.CMD_ACTIVITY_HINT_BAR_AND_BACKBTN:
                _activityPanel?.HideBackBtnAndTop();
                    
                break;
            case MessageConst.CMD_ACTIVITY_SHOW_BAR_AND_BACKBTN:
                 _activityPanel?.ShowBackBtnAndTop();
                break;
        }
    }

    
    public override void SetData(params object[] paramsObjects)
    {
        if (paramsObjects.Length > 0)
        {           
            GlobalData.ActivityModel.ActivitySoleId = ((string)paramsObjects[0]);
        }


       
    }

  
  
}
