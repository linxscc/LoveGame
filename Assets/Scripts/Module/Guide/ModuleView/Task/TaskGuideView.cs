using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Core.Events;
using Common;
using DataModel;
using game.main;
using game.tools;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Module.Guide.ModuleView.Task
{
    public class TaskGuideView : View
    {
        private GameObject _btn;
        private int _step = 1;

        private void Awake()
        {
            GuideManager.SetStatisticsRemoteGuideStep(GuideConst.MainLineStep_OnClick_Mission);
            
            GuideManager.TempState = TempState.NONE;
            
            _btn = transform.Find("ListContent/DailyTask/DailyTaskList/Content/DailyTaskItem/GetBtn").gameObject;
            GuideArrow.DoAnimation(_btn.transform);

            PointerClickListener.Get(_btn).onClick = go =>
            {
                List<UserMissionVo> arr = GlobalData.MissionModel.GetMissionListByType(MissionTypePB.Daily);
                EventDispatcher.TriggerEvent(EventConst.RecieveTaskReward, arr[0]);
                _btn.Hide();
                transform.Find("ListContent").gameObject.Hide();

                GuideManager.SetStatisticsRemoteGuideStep(GuideConst.MainLineStep_Finish_Mission1);
                BackToMainUI();
            };

            PointerClickListener.Get(gameObject).onClick = go =>
            {
                PopupManager.CloseAllWindow();
            };
        }
        
        private void BackToMainUI()
        {
            Transform view = transform.Find("ClickArea");
            view.gameObject.Show();  
            
            GuideArrow.DoAnimation(view);
            
            PointerClickListener.Get(view.gameObject).onClick = go =>
            {
                gameObject.Hide();
                ModuleManager.Instance.GoBack();
                PopupManager.CloseAllWindow();
            };
            
        }
    }
}