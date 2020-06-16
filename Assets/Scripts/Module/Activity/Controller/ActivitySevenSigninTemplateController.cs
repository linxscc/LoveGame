using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Core.Events;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module;
using Assets.Scripts.Module.NetWork;
using Com.Proto;
using Common;
using DataModel;
using game.main;
using Module.Activity.Window;
using UnityEngine;
using Utils;

public class ActivitySevenSigninTemplateController : Controller
{
   public ActivitySevenSigninTemplateView View;
   private SevenSigninTemplateModel _model;

   public override void Init()
   {
      base.Init();
      EventDispatcher.AddEventListener<SevenSigninTemplateAwardVo>(EventConst.ActivityTemplatePreviewAward,ActivityTemplatePreviewAward );
      EventDispatcher.AddEventListener<SevenSigninTemplateAwardVo>(EventConst.GetActivityTemplateAward,GetActivityTemplateAward );
   }

   /// <summary>
   /// 获取七天活动模板奖励
   /// </summary>
   /// <param name="vo"></param>
   private void GetActivityTemplateAward(SevenSigninTemplateAwardVo vo)
   {

      var curActivity = GlobalData.ActivityModel.GetActivityVo(ActivityType.ActivitySevenDaySigninTemplate);
      if (curActivity==null)
      {
         FlowText.ShowMessage(I18NManager.Get("Common_ActivityPastDue"));
         return;
      }
      else
      {
         if (curActivity.IsActivityPastDue)
         {
             FlowText.ShowMessage(I18NManager.Get("Common_ActivityPastDue"));
             return;
         }
      }
      
      if (vo.IsCardAward)
      {
         GetCardAward(vo);
      }
      else
      {
         GetNormalAward(vo);
      }
   }


   private void GetCardAward(SevenSigninTemplateAwardVo vo)
   {
      GainSevenDayAwardReq req = new GainSevenDayAwardReq
      {
         ActivityId = vo.ActivityId,
      };
      byte[] data = NetWorkManager.GetByteData(req);
      NetWorkManager.Instance.Send<GainSevenDayAwardRes>(CMD.ACTIVITY_GAINSEVENDAYAWARD, data, res =>
      {
         RewardUtil.AddReward(res.Awards);   
         _model.UpdateUserSevenDaySigninTemplateInfo(res.UserSevenDaySigninIndo);
         _model.UpdateSevenDayAwardList(res.UserSevenDaySigninIndo.SignDay);
         View.Refresh(vo.DayId);//刷新特殊处理下
         SendMessage(new Message(MessageConst.CMD_ACTIVITY_REFRESH_ACTIVITYTOGGLE_REDDOT)); 
         Action finish = () =>
         {
            SendMessage(new Message(MessageConst.CMD_ACTIVITY_SHOW_BAR_AND_BACKBTN));                
         };
         SendMessage(new Message(MessageConst.CMD_ACTIVITY_HINT_BAR_AND_BACKBTN));
         
         List<AwardPB> temp = res.Awards.ToList();
         ModuleManager.Instance.EnterModule(ModuleConfig.MODULE_DRAWCARD,
            false,false,"DrawCard_CardShow",temp,finish,false);
      });
   }


   private void GetNormalAward(SevenSigninTemplateAwardVo vo)
   {
      GainSevenDayAwardReq req =new GainSevenDayAwardReq
      {
         ActivityId = vo.ActivityId,
      };
      byte[] data = NetWorkManager.GetByteData(req);
      NetWorkManager.Instance.Send<GainSevenDayAwardRes>(CMD.ACTIVITY_GAINSEVENDAYAWARD, data, res =>
      {
         RewardUtil.AddReward(res.Awards);         
         _model.UpdateUserSevenDaySigninTemplateInfo(res.UserSevenDaySigninIndo);
         _model.UpdateSevenDayAwardList(res.UserSevenDaySigninIndo.SignDay);          
         View.Refresh(vo.DayId);//刷新特殊处理下
         SendMessage(new Message(MessageConst.CMD_ACTIVITY_REFRESH_ACTIVITYTOGGLE_REDDOT));            
         OpenNormalAwardWindow(vo); 
      });
      
   }

   /// <summary>
   ///获得奖励弹窗
   /// </summary>
   /// <param name="vo"></param>
   private void OpenNormalAwardWindow(SevenSigninTemplateAwardVo vo)
   {
     var window =  PopupManager.ShowWindow<NormalAwardWindow>("Activity/Prefabs/NormalAwardWindow");
     window.SetSevenSigninTemplateAwardData(vo);
   }
   
   /// <summary>
   /// 预览七天活动模板奖励
   /// </summary>
   /// <param name="vo"></param>
   private void ActivityTemplatePreviewAward(SevenSigninTemplateAwardVo vo)
   {
      var window = PopupManager.ShowWindow<ActivityAwardsWindow>("Activity/Prefabs/ActivityAwardWindow"); 
      window.SetDataActivityTemplate(vo,true);
   }


   public override void Start()
   {
      _model =new SevenSigninTemplateModel();
      View.CreateSevenSigninData(_model.GetSevenDaysLoginAwardList());    
      View.SetResidueDay(_model.EndTimeTamp);
   }


   public override void OnMessage(Message message)
   {
      base.OnMessage(message);
   }


   public override void Destroy()
   {
      base.Destroy();
      EventDispatcher.RemoveEvent(EventConst.ActivityTemplatePreviewAward);
      EventDispatcher.RemoveEvent(EventConst.GetActivityTemplateAward);
      View.DestroyCountDown();
   }

   
}
