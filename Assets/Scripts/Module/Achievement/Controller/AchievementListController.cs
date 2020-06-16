using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Core.Events;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module;
using Assets.Scripts.Module.NetWork;
using Com.Proto;
using Common;
using Componets;
using DataModel;
using UnityEngine;
using Utils;

namespace game.main
{
    public class AchievementListController : Controller
    {

        public AchievementListView View;
        private MissionModel _missionModel;
        private PlayerPB _curPlayerPb;
        private int _replaceId;

    
        public override void Start()
        {
            //获取用户任务的数据源
            _missionModel = GetData<MissionModel>();
            ClientData.LoadItemDescData(null);
            ClientData.LoadSpecialItemDescData(null);
            EventDispatcher.AddEventListener<UserMissionVo>(EventConst.RecieveAchievementReward, ReceiveItemReward);
            EventDispatcher.AddEventListener<string>(EventConst.JumpToAchievementCMD,JumpToOtherView);
        }

        private void JumpToOtherView(string target)
        {
            JumpToModule.JumpTo(target,null,_curPlayerPb);
//            if (target.Contains("Activity") && target != "SupporterActivity")
//            {
//                int acttype = int.Parse(target.Replace("Activity", null));
//                Debug.LogError(acttype);
//                ModuleManager.Instance.EnterModule(ModuleConfig.MODULE_ACTIVITY, false, false, acttype);
//                return;
//            }
//            
//            switch (target)
//            {
//                case "BuyEnergy":
//                case "BuyGold":
//                case "BuyEncouragePower":
//                    OnJumpToBuyWindow(target);
//                    break;
//                case "GameMain":
//                    OnGoBackToMain();
//                    break;
//                case "CardResolve":
//                    ModuleManager.Instance.EnterModule(ModuleConfig.MODULE_CARD,false,false,target);
//                    break;
//                case "DrawCard_Gold":
//                    ModuleManager.Instance.EnterModule(ModuleConfig.MODULE_DRAWCARD,false,false,target);
//                    break;
//                case "DrawCard_Gem":
//                    ModuleManager.Instance.EnterModule(ModuleConfig.MODULE_DRAWCARD);
//                    break;
//                case "SendGift":
//                    ModuleManager.Instance.EnterModule(ModuleConfig.MODULE_FAVORABILITYMAIN,false,false,target,_curPlayerPb);
//                    break;
//                case "FavorabilityPhoneEvent":
//                    ModuleManager.Instance.EnterModule(ModuleConfig.MODULE_FAVORABILITYMAIN,false,false,target,_curPlayerPb);
//                    break;
//                case "Reloading":
//                    GlobalData.FavorabilityMainModel.CurrentRoleVo =GlobalData.FavorabilityMainModel.GetUserFavorabilityVo((int)_curPlayerPb);    
//                    ModuleManager.Instance.EnterModule(target,false,true);
//                    break;
//                default:
//                    ModuleManager.Instance.EnterModule(target,false,true);
//                    break;
//            }
        }
        
//        private void OnGoBackToMain()
//        {
//            ModuleManager.Instance.GoBack();
//        }
//
//    
//        private void OnJumpToBuyWindow(string str)
//        {
//            int temp = 0;
//
//            switch (str)
//            {
//                case "BuyEnergy":
//                    temp = PropConst.PowerIconId;
//                    QuickBuy.BuyGlodOrPorwer(temp, PropConst.GemIconId);    
//                    break;
//                case "BuyGold":
//                    temp = PropConst.GoldIconId;
//                    QuickBuy.BuyGlodOrPorwer(temp, PropConst.GemIconId);    
//                    break;
//                case "BuyEncouragePower":
//                    temp = PropConst.EncouragePowerId;
//                    QuickBuy.BuyGlodOrPorwer(temp, PropConst.GemIconId);    
//                    break;
//            
//            }
//
//            //要等待他们购买金币成功的时候才需要返回！
//
//        }


        private void ReceiveItemReward(UserMissionVo vo)
        {
            if (vo.Status==MissionStatusPB.StatusUnclaimed)
            {
                LoadingOverlay.Instance.Show();
//            Debug.LogError("StatusUnclaimed");
                _curPlayerPb = _missionModel.GetMissionById(vo.MissionId).Player;
//                Debug.LogError(vo.MissionId+" "+vo.MissionType);
                _replaceId = vo.MissionId;
                var buffer = NetWorkManager.GetByteData(new MissionAwardsReq()
                    {MissionId = vo.MissionId, MissionType = vo.MissionType});
                NetWorkManager.Instance.Send<MissionAwardsRes>(CMD.MISSION_AWARDS,buffer,OnGetAwardCallBack, OnGetAwardError);           
            } 
        }

        private void OnGetAwardError(HttpErrorVo obj)
        {
            SendMessage(new Message(MessageConst.TO_GUIDE_ACHIEVEMENT_RESET));
        }

        private void OnGetAwardCallBack(MissionAwardsRes res)
        {
            LoadingOverlay.Instance.Hide();
            _missionModel.ReplaceUserMission(res.UserMission,_replaceId);       
//            _missionModel.UserMissionList.Sort();
            RewardUtil.AddReward(res.Awards);
            //领取奖励后要根据规则刷新道具之类的。
            _missionModel.UpdateUserMissionInfo(res.UserMissionInfo);
            View.SetData(_missionModel,(int)_curPlayerPb);        
            FlowText.ShowMessage(I18NManager.Get("Task_ReceiveRewardSuccess")); 
            foreach (var award in res.Awards)
            {
                if (award.Resource==ResourcePB.Gem)
                {
                    SdkHelper.StatisticsAgent.OnReward(award.Num,"星路里程");   
                }
            }
            SendMessage(new Message(MessageConst.TO_GUIDE_ACHIEVEMENT_NEXT_STEP));
        }

        public void SetView(int id)
        {
            _curPlayerPb = (PlayerPB) id;
            View.SetData(_missionModel,id);
        }


        public void RefreshMission()
        {
            NetWorkManager.Instance.Send<UserMissionsRes>(CMD.MISSION_MYMISSION,null,OnGetUserMissionData);
            
        }

        private void OnGetUserMissionData(UserMissionsRes res)
        {
            _missionModel.InitUserData(res);
//            _missionModel.UserMissionList.Sort();
            View.SetData(_missionModel,(int)_curPlayerPb);
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
                    ReceiveActReward((MissionTypePB)body[0],(int)body[1],(PlayerPB)body[2]);
                    break;
            }
        }

        private void ReceiveActReward(MissionTypePB missionTypePb, int weight,PlayerPB playerPb)
        {
            _curPlayerPb = playerPb;
            var buffer = NetWorkManager.GetByteData(new MissionActivityAwardsReq() {MissionType = missionTypePb,Weight = weight,Player = playerPb});
            NetWorkManager.Instance.Send<MissionActivityAwardsRes>(CMD.MISSION_ACTIVITYAWARDS,buffer,OnReceiveRewardCallBack);
        }

        private void OnReceiveRewardCallBack(MissionActivityAwardsRes res)
        {
            _missionModel.UpdateUserMissionInfo(res.UserMissionInfo);
            RewardUtil.AddReward(res.Awards);
            View.SetData(_missionModel,(int)_curPlayerPb);

            FlowText.ShowMessage(I18NManager.Get("Task_GetActRewardSuccess"));
            foreach (var award in res.Awards)
            {
                if (award.Resource==ResourcePB.Gem)
                {
                    SdkHelper.StatisticsAgent.OnReward(award.Num,"星路里程");   
                }
            }
            
        }


        public override void Destroy()
        {
            base.Destroy();
            ClientData.Clear();
            EventDispatcher.RemoveEventListener<UserMissionVo>(EventConst.RecieveAchievementReward, ReceiveItemReward);
            EventDispatcher.RemoveEventListener<string>(EventConst.JumpToAchievementCMD,JumpToOtherView);
        }
    } 

}

