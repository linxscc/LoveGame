  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Assets.Scripts.Framework.GalaSports.Core;
  using Assets.Scripts.Framework.GalaSports.Core.Events;
  using Assets.Scripts.Framework.GalaSports.Service;
  using Assets.Scripts.Module;
  using Assets.Scripts.Module.NetWork;
  using Assets.Scripts.Module.Task.Service;
  using Com.Proto;
  using Common;
  using Componets;
  using DataModel;
  using game.main;
  using Utils;

  public class StarActivityController : Controller
    {

        public StarActivityView View;        
        private PlayerPB _curPlayerPb;
        private MissionModel _missionModel;      
        private TimerHandler _countDown;
        
        public override void Init()
        {
            ClientData.LoadItemDescData(null);
            ClientData.LoadSpecialItemDescData(null);
            base.Init();
            EventDispatcher.AddEventListener<UserMissionVo>(EventConst.StarActivityGetAward,GetStarActivityAward);
            EventDispatcher.AddEventListener<string>(EventConst.StarActivityGotoTask,GotoStarActivityTask);
        }

        public void OnShow()
        {
            GetService<MissionService>().SetCallback(OnGetMissionRule).Execute();
        }
        

        private void CheckRefresh()
        {
            _countDown= ClientTimer.Instance.AddCountDown("UpdateStarActivityRefresh", Int64.MaxValue, 1, UpdateStarActivityRefresh,
                null);           
        }
        
        private void UpdateStarActivityRefresh(int obj)
        {        
            var refreshTimeStamp = _missionModel.GetStarActivityRefreshTimePintList()[_missionModel.GetOpenDay()];
            var curTimeStamp = ClientTimer.Instance.GetCurrentTimeStamp();
            var overTimeStamp = _missionModel.GetStarActivityOverTimeStamp();
            if (curTimeStamp==overTimeStamp)
            {
                ModuleManager.Instance.GoBack();
                return;
            }
            if (refreshTimeStamp==curTimeStamp)
            {
                MissionRefreshReq req =new MissionRefreshReq();
                var data =NetWorkManager.GetByteData(req);
                NetWorkManager.Instance.Send<MissionRefreshRes>(CMD.MISSION_REFRESH, data, res =>
                {                                 
                    _missionModel.UpdateUserMissionInfo(res.UserMissionInfo);
                    _missionModel.UpdateStarActivityMission(res.UserMissions);                    
                    View.SetData(_missionModel,_missionModel.Day);
                });
            }
        }
        
        
        /// <summary>
        /// 前往星活动任务
        /// </summary>
        /// <param name="jumpTarget">跳入的模块</param>
        private void GotoStarActivityTask(string jumpTarget)
        {                    
            JumpToModule.JumpTo(jumpTarget,null,_curPlayerPb);       
        }
        
     
        private void OnGetMissionRule(MissionModel model)
        {
            GlobalData.MissionModel = model;
            _missionModel = GlobalData.MissionModel;
            View.SetData(_missionModel,_missionModel.Day);
            CheckRefresh();
        }

        /// <summary>
        /// 领取星活动奖励
        /// </summary>
        /// <param name="vo"></param>
        private void GetStarActivityAward(UserMissionVo vo)
        {
            if (vo.Status== MissionStatusPB.StatusUnclaimed)
            {
                LoadingOverlay.Instance.Show(); 
                MissionAwardsReq req =new MissionAwardsReq
                {
                    MissionId = vo.MissionId, 
                    MissionType = vo.MissionType 
                };
                byte[] data = NetWorkManager.GetByteData(req);
                NetWorkManager.Instance.Send<MissionAwardsRes>(CMD.MISSION_AWARDS,data,OnGetAwardCallBack); 
                
            } 
        }

        /// <summary>
        /// 星活动奖励回包
        /// </summary>
        /// <param name="res"></param>
        private void OnGetAwardCallBack(MissionAwardsRes res)
        {
            LoadingOverlay.Instance.Hide();
           
            foreach (var t in res.Awards)
            {
                RewardUtil.AddReward(t);
                RewardVo vo = new RewardVo(t);
                FlowText.ShowMessage(I18NManager.Get("Activity_Get", vo.Name,vo.Num));  
            }
           
            
            //刷新数据
            _missionModel.UpdateUserMission(res.UserMission);
            _missionModel.UpdateUserMissionInfo(res.UserMissionInfo);
            _missionModel.UpdateUserStarActivityMission(res.UserMission,_missionModel.Day);
                        
            //刷新UI
            View.SetData(_missionModel,_missionModel.Day);            
        }


        public override void OnMessage(Message message)
        {
          
            string name = message.Name;
            object[] body = message.Params;
            switch (name)
            {
               case MessageConst.CMD_STAR_ACTIVITY_TOGGLE_SELECT_DAY:
                   var day = (int)message.Body;
                   GlobalData.MissionModel.Day = day;
                   View.SetData(_missionModel,GlobalData.MissionModel.Day);
                   break;
               case MessageConst.CMD_STAR_ACTIVITY_ACTIVE_REWARD:
                   GetActiveReward((MissionTypePB) body[0], (int) body[1]);
                   break;
            }
        }

        /// <summary>
        /// 领取活跃奖励
        /// </summary>
        /// <param name="type"></param>
        /// <param name="weight"></param>
        private void GetActiveReward(MissionTypePB type, int weight)
        {
            LoadingOverlay.Instance.Show();
            MissionActivityAwardsReq req =new MissionActivityAwardsReq
            {
                MissionType = type,
                Weight = weight
            }; 
            byte[] data = NetWorkManager.GetByteData(req);
            NetWorkManager.Instance.Send<MissionActivityAwardsRes>(CMD.MISSION_ACTIVITYAWARDS,data,OnGetActiveRewardCallBack);  
        }

        /// <summary>
        /// 活跃奖励回包
        /// </summary>
        /// <param name="res"></param>
        private void OnGetActiveRewardCallBack(MissionActivityAwardsRes res)
        {
            LoadingOverlay.Instance.Hide();
            RewardUtil.AddReward(res.Awards);
            _missionModel.UpdateUserMissionInfo(res.UserMissionInfo);
           
           
            //刷新UI
            View.SetData(_missionModel,_missionModel.Day);

            var isCard = false;

            foreach (var t in res.Awards)
            {
                if (t.Resource== ResourcePB.Card)
                {
                    isCard = true;
                    break;                   
                }  
            }

            if (isCard)
            {
                List<AwardPB> award =new  List<AwardPB>();
                foreach (var t in res.Awards)
                {
                    if (t.Resource == ResourcePB.Card)
                    {
                        award.Add(t); 
                        break;
                    }                 
                }
                
                Action finish = () =>
                {
                    SendMessage(new Message(MessageConst.CMD_STAR_ACTIVITY_SHOW_TOPBAR_AND_BACKBTN, Message.MessageReciverType.UnvarnishedTransmission,true));    
                };
                ModuleManager.Instance.EnterModule(ModuleConfig.MODULE_DRAWCARD,
                    false,false,"DrawCard_CardShow",award,finish,false);
                ClientTimer.Instance.DelayCall(() =>
                {
                   SendMessage(new Message(MessageConst.CMD_STAR_ACTIVITY_SHOW_TOPBAR_AND_BACKBTN, Message.MessageReciverType.UnvarnishedTransmission,false)); 
                }, 0.1f);
            }
            else
            {
                var  window = PopupManager.ShowWindow<CommonAwardWindow>("GameMain/Prefabs/AwardWindow/CommonAwardWindow");
                window.SetData(res.Awards.ToList(),false,ModuleConfig.MODULE_STAR_ACTIVITY);
            }           
        }


        public override void Destroy()
        {
            base.Destroy();
            EventDispatcher.RemoveEvent(EventConst.StarActivityGetAward);
            EventDispatcher.RemoveEvent(EventConst.StarActivityGotoTask);          
            ClientTimer.Instance.RemoveCountDown(_countDown);	                          
            ClientTimer.Instance.RemoveCountDown("SetStarActivityCountDown");
        }
    }
 


