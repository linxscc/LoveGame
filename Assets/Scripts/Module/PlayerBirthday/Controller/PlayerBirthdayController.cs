using System;
using System.Collections;
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
using UnityEngine;
using Utils;

public class PlayerBirthdayController : Controller
{

    public PlayerBirthdayView View;
    private MissionModel _missionModel;
    private AwardWindow _window;
    private PlayerPB _curPlayerPb;
    public override void Init()
    {
        ClientData.LoadItemDescData(null);
        ClientData.LoadSpecialItemDescData(null);
        EventDispatcher.AddEventListener<UserMissionVo>(EventConst.PlayerBirthdayGetAward,GetPlayerBirthdayAward);
        EventDispatcher.AddEventListener<string>(EventConst.PlayerBirthdayGotoTask,GotoStarActivityTask);
    }
    
      /// <summary>
        /// 前往星活动任务
        /// </summary>
        /// <param name="jumpTarget">跳入的模块</param>
        private void GotoStarActivityTask(string jumpTarget)
        {
           JumpToModule.JumpTo(jumpTarget,null,_curPlayerPb);
//            if (jumpTarget.Contains("Activity") && jumpTarget!= "SupporterActivity")
//            {
//               int activityId = int.Parse(jumpTarget.Replace("Activity", null));
//               Debug.LogError("跳入的活动模块ID===>"+activityId);
//               ModuleManager.Instance.EnterModule(ModuleConfig.MODULE_ACTIVITY, false, false, activityId);
//               return;
//            }
//
//            switch (jumpTarget)
//            {
//                 case "BuyEnergy":
//                 case "BuyGold":
//                 case "BuyEncouragePower":
//                       OnJumpToBuyWindow(jumpTarget); 
//                     break;
//                 case "GameMain":
//                     OnGoBackToMain();
//                     break;
//                 case "CardResolve":
//                     ModuleManager.Instance.EnterModule(ModuleConfig.MODULE_CARD,false,false,jumpTarget);
//                     break;
//                 case "DrawCard_Gold":
//                     ModuleManager.Instance.EnterModule(ModuleConfig.MODULE_DRAWCARD,false,false,jumpTarget);
//                     break;
//                 case "DrawCard_Gem":
//                     ModuleManager.Instance.EnterModule(ModuleConfig.MODULE_DRAWCARD);
//                     break;
//                 case "SendGift":
//                   //  ModuleManager.Instance.EnterModule(ModuleConfig.MODULE_FAVORABILITYMAIN,false,false,jumpTarget,_curPlayerPb);
//                     break;
//                 case "FavorabilityPhoneEvent":
//                   //  ModuleManager.Instance.EnterModule(ModuleConfig.MODULE_FAVORABILITYMAIN,false,false,jumpTarget,_curPlayerPb);
//                     break;
//                 case "Reloading":
//                     GlobalData.FavorabilityMainModel.CurrentRoleVo =GlobalData.FavorabilityMainModel.GetUserFavorabilityVo((int)_curPlayerPb);    
//                     ModuleManager.Instance.EnterModule(jumpTarget,false,true);
//                     break;
//                 default:
//                     ModuleManager.Instance.EnterModule(jumpTarget,false,true);
//                     break;
//                 
//            }
            
        }

      
//      private void OnJumpToBuyWindow(string buyType)
//      {
//          int temp = 0;
//
//          switch (buyType)
//          {
//              case "BuyEnergy":
//                  temp = PropConst.PowerIconId;
//                  QuickBuy.BuyGlodOrPorwer(temp, PropConst.GemIconId);    
//                  break;
//              case "BuyGold":
//                  temp = PropConst.GoldIconId;
//                  QuickBuy.BuyGlodOrPorwer(temp, PropConst.GemIconId);    
//                  break;
//              case "BuyEncouragePower":
//                  temp = PropConst.EncouragePowerId;
//                  QuickBuy.BuyGlodOrPorwer(temp, PropConst.GemIconId);    
//                  break;          
//          }
//      }
      
//      private void OnGoBackToMain()
//      {
//          ModuleManager.Instance.GoBack();
//      }
      
    /// <summary>
    /// 领取生日任务奖励
    /// </summary>
    /// <param name="vo"></param>
    private void GetPlayerBirthdayAward(UserMissionVo vo)
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
    /// 生日任务奖励回包
    /// </summary>
    /// <param name="res"></param>
    private void OnGetAwardCallBack(MissionAwardsRes res)
    {
        LoadingOverlay.Instance.Hide();
        RewardUtil.AddReward(res.Awards);

        FlowText.ShowMessage(I18NManager.Get("Task_ReceiveRewardSuccess"));
        
        //刷新数据
        _missionModel.UpdateUserMission(res.UserMission);
        _missionModel.UpdateUserMissionInfo(res.UserMissionInfo);
        _missionModel.UpdateUserPlayerBirthdayMission(res.UserMission, _missionModel.PlayerBirthdayMissionsDay);
        
   
        View.SetData(_missionModel,_missionModel.PlayerBirthdayMissionsDay);
    }
    
    public void OnShow()
    {
        GetService<MissionService>().SetCallback(OnGetMissionRule).Execute();
    }

    private void OnGetMissionRule(MissionModel model)
    {
           GlobalData.MissionModel = model;
           _missionModel = GlobalData.MissionModel;
           View.SetData(_missionModel,_missionModel.PlayerBirthdayMissionsDay);
           CheckRefresh();
    }

    private void CheckRefresh()
    {
        ClientTimer.Instance.AddCountDown("UpdatePlayerBirthdayRefresh", Int64.MaxValue, 1, UpdatePlayerBirthdayRefresh,
            null); 
    }

    private void UpdatePlayerBirthdayRefresh(int obj)
    {
       
        var refreshTimeStamp =
            _missionModel.GetPlayerBirthdayRefreshTimePointList()[_missionModel.GetPlayerBirthdayOpenDay()];
        var curTimeStamp = ClientTimer.Instance.GetCurrentTimeStamp();
        var overTimeStamp = _missionModel.GetPlayerBirthdayOverTimeStamp();

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
                _missionModel.UpdatePlayerBirthdayMission(res.UserMissions);

                _missionModel.PlayerBirthdayMissionsDay = _missionModel.GetPlayerBirthdayOpenDay();
                View.SetData(_missionModel,_missionModel.PlayerBirthdayMissionsDay);
            });
        }
    }
    

    public override void OnMessage(Message message)
    {
        
        string name = message.Name;
        object[] body = message.Params;
        switch (name)
        {
           case  MessageConst.CMD_PLAYER_BIRTHDAY_TOGGLE_SELECT_DAY:
               var day = (int)message.Body;
               GlobalData.MissionModel.PlayerBirthdayMissionsDay = day;
             
               View.SetData(_missionModel,GlobalData.MissionModel.PlayerBirthdayMissionsDay);
               break;
           case MessageConst.CMD_PLAYER_BIRTHDAY_ACTIVE_REWARD:
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
        View.SetData(_missionModel,_missionModel.PlayerBirthdayMissionsDay);


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
                SendMessage(new Message(MessageConst.CMD_PLAYERBITTHDAY_SHOW_TOPBAR_AND_BACKBTN, Message.MessageReciverType.UnvarnishedTransmission,true));    
            };
            ModuleManager.Instance.EnterModule(ModuleConfig.MODULE_DRAWCARD,
                false,false,"DrawCard_CardShow",award,finish,false);
            ClientTimer.Instance.DelayCall(() =>
            {
                SendMessage(new Message(MessageConst.CMD_PLAYERBITTHDAY_SHOW_TOPBAR_AND_BACKBTN, Message.MessageReciverType.UnvarnishedTransmission,false)); 
            }, 0.1f);
        }
        else
        {
            var  window = PopupManager.ShowWindow<CommonAwardWindow>("GameMain/Prefabs/AwardWindow/CommonAwardWindow");
            window.SetData(res.Awards.ToList(),false,ModuleConfig.MODULE_PLAYERBIRTHDAY);  
        }
    }
    
    
    
    
    public override void Destroy()
    {
        ClientTimer.Instance.RemoveCountDown("UpdatePlayerBirthdayRefresh");
        base.Destroy();
        EventDispatcher.RemoveEvent(EventConst.PlayerBirthdayGetAward);
        EventDispatcher.RemoveEvent(EventConst.PlayerBirthdayGotoTask);
    }
}
