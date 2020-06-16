using System;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Core.Events;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module;
using Assets.Scripts.Module.Framework.Utils;
using Assets.Scripts.Module.MainLine.Services;
using Assets.Scripts.Module.NetWork;
using Assets.Scripts.Module.Task.Service;
using Com.Proto;
using Common;
using Componets;
using DataModel;
using UnityEngine;
using Utils;

namespace game.main
{
    public class TaskPanelController : Controller
    {
        public TaskDailyView View;
        public MissionModel _missionModel;
        private MissionTypePB curMissionTypePb;
//        private TimerHandler _handler;
//        private DateTime refreshTime;

        private bool _nextDay=false;
//        private long _refreshTime;
//        private string refreshPoint = "6:00:00";

        public override void Start()
        {
            //获取用户任务的数据源
            EventDispatcher.AddEventListener<UserMissionVo>(EventConst.RecieveTaskReward, OnReceiveReward);
            EventDispatcher.AddEventListener(EventConst.BuyGoldSuccess, GetData);
            EventDispatcher.AddEventListener<string>(EventConst.JumpToCMD, JumpToOtherView);
            curMissionTypePb = MissionTypePB.Daily;
            _missionModel = GlobalData.MissionModel;
            GetData();
            EventDispatcher.AddEventListener(EventConst.DailyRefresh6,RefershTask);
        }

        private void JumpToOtherView(string target)
        {
            JumpToModule.JumpTo(target);
        }

        public void GetData()
        {

            GetService<MissionService>().SetCallback(OnGetMissionRule).Execute();
        }

        private void OnGetMissionRule(MissionModel model)
        {
            _missionModel = model;
            _missionModel.UserMissionList.Sort();
            View.SetMissionItemData(_missionModel);
            View.ChangeViewState(curMissionTypePb); //默认日常任务的UI
        }
        

        private void OnReceiveReward(UserMissionVo vo)
        {
            if (vo.Status == MissionStatusPB.StatusUnclaimed)
            {
                LoadingOverlay.Instance.Show();
//            Debug.LogError("StatusUnclaimed");
                curMissionTypePb = vo.MissionType;
                // Debug.LogError(vo.MissionId+" "+vo.MissionType);
                var buffer = NetWorkManager.GetByteData(new MissionAwardsReq()
                    {MissionId = vo.MissionId, MissionType = vo.MissionType});
                NetWorkManager.Instance.Send<MissionAwardsRes>(CMD.MISSION_AWARDS, buffer, OnGetAwardCallBack);
            }
        }

        private void OnGetAwardCallBack(MissionAwardsRes res)
        {
            LoadingOverlay.Instance.Hide();
//            Debug.LogError(res.UserMission);
            //Debug.LogError(res.UserMission);
            _missionModel.UpdateUserMission(res.UserMission);
            //Debug.LogError(res.StarCourseSchedule.Count);
//        _missionModel.UpdateStarCourse(res.StarCourseSchedule);

            _missionModel.UserMissionList.Sort();
            RewardUtil.AddReward(res.Awards);
            //领取奖励后要根据规则刷新道具之类的。
            _missionModel.UpdateUserMissionInfo(res.UserMissionInfo);
            View.SetMissionItemData(_missionModel);
            if (curMissionTypePb == MissionTypePB.Daily)
            {
                View.SetDailyMissionInfo();
            }
            else
            {
                View.SetWeekMissionInfo();
            }
            FlowText.ShowMessage(I18NManager.Get("Task_ReceiveRewardSuccess"));
            foreach (var award in res.Awards)
            {
                if (award.Resource==ResourcePB.Gem)
                {
                    SdkHelper.StatisticsAgent.OnReward(award.Num,"任务");   
                }
            }

            if (GlobalData.PlayerModel.PlayerVo.Level == 4)
            {
                GuideManager.SetStatisticsRemoteGuideStep(GuideConst.MainLineStep_Mission_Reward);
            }
        }


        /// <summary>
        /// 处理View消息
        /// </summary>
        /// <param name="message"></param>
        public override void OnMessage(Message message)
        {
            string name = message.Name;
            object[] body = message.Params;
            switch (name)
            {
                case MessageConst.CMD_TASK_RECEIVE_ACTREWARD:
                    ReceiveActReward((MissionTypePB) body[0], (int) body[1]);
                    break;
                case MessageConst.CMD_RECOREDSTATE:
                    curMissionTypePb = (MissionTypePB) message.Body;
                    break;
            }
        }

        private void ReceiveActReward(MissionTypePB missionTypePb, int weight)
        {
            curMissionTypePb = missionTypePb;
            LoadingOverlay.Instance.Show();
            var buffer = NetWorkManager.GetByteData(new MissionActivityAwardsReq()
                {MissionType = missionTypePb, Weight = weight});
            NetWorkManager.Instance.Send<MissionActivityAwardsRes>(CMD.MISSION_ACTIVITYAWARDS, buffer,
                OnReceiveRewardCallBack);
        }

        private void OnReceiveRewardCallBack(MissionActivityAwardsRes res)
        {
            LoadingOverlay.Instance.Hide();
            _missionModel.UpdateUserMissionInfo(res.UserMissionInfo);
            RewardUtil.AddReward(res.Awards);
            View.SetMissionItemData(_missionModel);
            if (curMissionTypePb == MissionTypePB.Daily)
            {
                View.SetDailyMissionInfo();
            }
            else
            {
                View.SetWeekMissionInfo();
            }

            FlowText.ShowMessage(I18NManager.Get("Task_GetActRewardSuccess"));

            foreach (var award in res.Awards)
            {
                if (award.Resource==ResourcePB.Gem)
                {
                    SdkHelper.StatisticsAgent.OnReward(award.Num,"任务");   
                }
            }
            
        }


        private void RefershTask()
        {
            var buffer = NetWorkManager.GetByteData(new MissionRefreshReq { });
            NetWorkManager.Instance.Send<MissionRefreshRes>(CMD.MISSION_REFRESH, buffer, DailyRefreshMission);
        }

        private void DailyRefreshMission(MissionRefreshRes res)
        {
            //貌似回包的东西有点讲究，应该是更新后的数据吧！太坑爹了！
            _missionModel.UpdateDailyMission(res.UserMissions);
            _missionModel.UpdateUserMissionInfo(res.UserMissionInfo);
            _missionModel.UserMissionList.Sort();
            View.SetMissionItemData(_missionModel);
            View.ChangeViewState(curMissionTypePb); //默认日常任务的UI
            Debug.Log("整点刷新");

            if (curMissionTypePb == MissionTypePB.Daily)
            {
                View.SetDailyMissionInfo();
            }
            else
            {
                View.SetWeekMissionInfo();
            }
            
        }


        public override void Destroy()
        {
            base.Destroy();
            EventDispatcher.RemoveEventListener<UserMissionVo>(EventConst.RecieveTaskReward, OnReceiveReward);
            EventDispatcher.RemoveEventListener(EventConst.BuyGoldSuccess, GetData);
            EventDispatcher.RemoveEventListener<string>(EventConst.JumpToCMD, JumpToOtherView);
            EventDispatcher.RemoveEventListener(EventConst.DailyRefresh6,RefershTask);
            //ClientTimer.Instance.RemoveCountDown(_handler);
        }
    }
}

//        private void CheckNeedToRefresh()
//        {
//            //进来后要先判断是否已经到了刷新时间点，如果过了，就不会倒计时了，如果没有过，那么就要开始倒计时！
//            _handler = ClientTimer.Instance.AddCountDown("UpdateDailyTask", Int64.MaxValue, 5f, UpdateDailyTask, null);      
//            
//            
//
//                      
//        }
//
//        private void UpdateDailyTask(int time)
//        {
//
//
//            if (DateTime.Compare(DateTime.Now, refreshTime) > 0)
//            {
//                //已经过了整点
//                var serverDateTime = DateUtil.GetDataTime(ClientTimer.Instance.GetCurrentTimeStamp());
//                refreshTime=new DateTime(serverDateTime.Year,serverDateTime.Month,serverDateTime.Day,6,0,0);//DateTime.Parse($"{DateTime.Today.Year}-{DateTime.Today.Month}-{DateTime.Today.Day+1} {refreshPoint}");
////                _refreshTime = DateUtil.GetTimeStamp(refreshTime)+86400000;
//                refreshTime=refreshTime.AddDays(1);
//            }
//            else
//            {
//                if (ClientTimer.Instance.GetCurrentTimeStamp()>_refreshTime)
//                {
//                    Debug.Log(DateUtil.GetDataTime(ClientTimer.Instance.GetCurrentTimeStamp())+" "+refreshTime);
//
//
//                    var serverDateTime = DateUtil.GetDataTime(ClientTimer.Instance.GetCurrentTimeStamp());
//                    //refreshTime=new DateTime(serverDateTime.Year,serverDateTime.Month,serverDateTime.Day,6,0,0);//DateTime.Parse($"{serverDateTime.Year}-{serverDateTime.Month}-{serverDateTime.Day} {refreshPoint}");
//                    _refreshTime = DateUtil.GetTimeStamp(new DateTime(serverDateTime.Year,serverDateTime.Month,serverDateTime.Day,6,0,0))+86400000;
//                }
////                else
////                {
////                    Debug.LogError("dont reach!"); 
////                }
//            }
//            
//
//        }