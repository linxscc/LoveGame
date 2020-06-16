using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Core.Events;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module;
using Assets.Scripts.Module.NetWork;
using Com.Proto;
using Common;
using Componets;
using DataModel;
using game.main;
using live2d;
using Module.Activity.Window;
using UnityEngine;
using Utils;

public class ActivitySevenSigninController : Controller
{

    public ActivitySevenSigninView View;
    private SevenDaysLoginAwardModel _model;

   
    private NormalAwardWindow _normalAwardWindow;

    public override void Init()
    {
        EventDispatcher.AddEventListener<SevenDaysLoginAwardVO>(EventConst.GetCardAward, GetCardAward);
        EventDispatcher.AddEventListener<SevenDaysLoginAwardVO>(EventConst.GetNormalAward, GetNormalAward);
        EventDispatcher.AddEventListener<SevenDaysLoginAwardVO>(EventConst.PreviewAward,PreviewAward );
    }

   

    public override void Start()
    {
        _model = new SevenDaysLoginAwardModel();
        View.CreateSevenSigninData(_model.GetSevenDaysLoginAwardList());      
        View.SetHintData( _model.GetSevenDaysActivityResidueDay().ToString()); 
    }

    public override void OnMessage(Message message)
    {
        string name = message.Name;
        object[] body = message.Params;
        switch (name)
        {
          case MessageConst.CMD_ACTIVITY_SEVENDAYGUIDE:
              ActivityGuide();
              break;
        }
    }

    public override void Destroy()
    {
        base.Destroy();
        EventDispatcher.RemoveEvent(EventConst.GetCardAward);
        EventDispatcher.RemoveEvent(EventConst.GetNormalAward);
        EventDispatcher.RemoveEvent(EventConst.PreviewAward);
    }

 


    /// <summary>
    /// 领取卡的请求，防止策划以后配成卡
    /// </summary>
    /// <param name="vO"></param>
    private void GetCardAward(SevenDaysLoginAwardVO vO)
    {
        var curActivity = GlobalData.ActivityModel.GetActivityVo(ActivityType.ActivitySevenDaySignin);
        if (curActivity == null) 
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
        
        GainSevenDayAwardReq req = new GainSevenDayAwardReq
        {
            ActivityId = vO.ActivityId,
        };
        byte[] data = NetWorkManager.GetByteData(req);
        NetWorkManager.Instance.Send<GainSevenDayAwardRes>(CMD.ACTIVITY_GAINSEVENDAYAWARD, data, res =>
        {         
            RewardUtil.AddReward(res.Awards);       
            _model.UpdateUserSevenDaySigninInfo(res.UserSevenDaySigninIndo);         
            _model.UpdateSevenDayAwardList(res.UserSevenDaySigninIndo.SignDay);
            View.Refresh(vO.DayId);//刷新特殊处理下
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

    /// <summary>
    /// 领取普通奖励（不是卡）
    /// </summary>
    /// <param name="vO"></param>
    private void GetNormalAward(SevenDaysLoginAwardVO vO)
    {       
        var curActivity = GlobalData.ActivityModel.GetActivityVo(ActivityType.ActivitySevenDaySignin);
        
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
        
        GainSevenDayAwardReq req =new GainSevenDayAwardReq
        {
            ActivityId = vO.ActivityId,
        };
        byte[] data = NetWorkManager.GetByteData(req);
        NetWorkManager.Instance.Send<GainSevenDayAwardRes>(CMD.ACTIVITY_GAINSEVENDAYAWARD, data, res =>
        {
            RewardUtil.AddReward(res.Awards);         
            _model.UpdateUserSevenDaySigninInfo(res.UserSevenDaySigninIndo);
            _model.UpdateSevenDayAwardList(res.UserSevenDaySigninIndo.SignDay);          
            View.Refresh(vO.DayId);//刷新特殊处理下
            SendMessage(new Message(MessageConst.CMD_ACTIVITY_REFRESH_ACTIVITYTOGGLE_REDDOT));            
            OpenNormalAwardWindow(vO); 
        });
    }


    /// <summary>
    /// 发生七日引导第一天请求
    /// </summary>
    private void ActivityGuide()
    {
        var curActivity = GlobalData.ActivityModel.GetActivityVo(ActivityType.ActivitySevenDaySignin);
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
        
        var activityId = curActivity.ActivityId;
        GainSevenDayAwardReq req =new GainSevenDayAwardReq
        {
            ActivityId = activityId           
        };
        byte[] data = NetWorkManager.GetByteData(req);

        NetWorkManager.Instance.Send<GainSevenDayAwardRes>(CMD.ACTIVITY_GAINSEVENDAYAWARD, data, res =>
        {
            RewardUtil.AddReward(res.Awards);
            _model.UpdateUserSevenDaySigninInfo(res.UserSevenDaySigninIndo);
            _model.UpdateSevenDayAwardList(res.UserSevenDaySigninIndo.SignDay);
            View.Refresh(1);
             SendMessage(new Message(MessageConst.CMD_ACTIVITY_REFRESH_ACTIVITYTOGGLE_REDDOT));            
        });
    }
    
    /// <summary>
    /// 打开普通奖励获得窗口
    /// </summary>
    /// <param name="vO"></param>
    private void OpenNormalAwardWindow(SevenDaysLoginAwardVO vO)
    {
        _normalAwardWindow = null;
        if (_normalAwardWindow == null)
        {
            _normalAwardWindow = PopupManager.ShowWindow<NormalAwardWindow>("Activity/Prefabs/NormalAwardWindow");
            _normalAwardWindow.SetData(vO);
        }
    }
    
    /// <summary>
    /// 打开预览奖励窗口
    /// </summary>
    /// <param name="vo"></param>
    private void PreviewAward(SevenDaysLoginAwardVO vo)
    {
        var window = PopupManager.ShowWindow<ActivityAwardsWindow>("Activity/Prefabs/ActivityAwardWindow");
        window.SetData(vo,true);
    }
}
