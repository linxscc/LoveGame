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
using UnityEngine;
using UnityEngine.EventSystems;
using Utils;
public class ActivityMonthSigninController : Controller
{
    public ActivityMonthSigninView View;
    private MonthSignModel _model;
    private MonthRetroactiveWindow _window;
    public override void Init()
    {
        EventDispatcher.AddEventListener<MonthSignAwardVO>(EventConst.MonthSigin, MonthSigin);
        EventDispatcher.AddEventListener<MonthSignAwardVO>(EventConst.MonthRetroactive, MonthRetroactive);
       
    }

    public override void Start()
    {
        _model = new MonthSignModel();
        View.Init(_model.GetCurMonthSignExtraAward() ,_model.MonthSignNum,_model.TotalDate,_model.ExtraRewardsState);      
        View.CreateMonthSignData(_model.GetMonthSignAwards());
        View.SetContentPos(_model.ToDayId);
    }

   

    private void MonthSigninSixRefresh()
    {
        if (View.gameObject!=null)
        {
            Debug.LogError("刷新月签");
            _model = new MonthSignModel();
            View.Init(_model.GetCurMonthSignExtraAward() ,_model.MonthSignNum,_model.TotalDate,_model.ExtraRewardsState);      
            View.CreateMonthSignData(_model.GetMonthSignAwards());
            View.SetContentPos(_model.ToDayId);
        }        
    }

    public override void OnMessage(Message message)
    {
        string name = message.Name;
        object[] body = message.Params;
        switch (name)
        {
            case MessageConst.CMD_ACTIVITY_MONTH_SIGIN_GET_BTN:
                SendMonthSiginAccumulativeReq();
                break;
            case MessageConst.CMD_ACTIVITY_SIXREFRESHACTIVITY:
                MonthSigninSixRefresh();
                break;
        }
    }

    public override void Destroy()
    {
        base.Destroy();
        EventDispatcher.RemoveEvent(EventConst.MonthSigin);
        EventDispatcher.RemoveEvent(EventConst.MonthRetroactive);
      
    }

    /// <summary>
    /// 发送签到请求
    /// </summary>
    /// <param name="vO"></param>
    private void MonthSigin(MonthSignAwardVO vO)
    {
        NetWorkManager.Instance.Send<MonthSignRewardRes>(CMD.ACTIVITY_MONTH_SING_REWARD,null, MonthSiginSuccCallback);
    }

    /// <summary>
    /// 签到请求回包
    /// </summary>
    /// <param name="res"></param>
    private void MonthSiginSuccCallback(MonthSignRewardRes res)
    {
        
        Debug.LogError("30签到奖励回包===>"+res.Awards);
        foreach (var t in res.Awards)
        {
            RewardUtil.AddReward(t);
            RewardVo vo = new RewardVo(t);
            FlowText.ShowMessage(I18NManager.Get("Activity_Get", vo.Name,vo.Num));
        }  
        
        _model.UpdateUserMonthSignInfo(res.UserMonthSign);
        foreach (var t in res.UserMonthSign.Dates)
        {
            _model.UpdateMonthSignAwards(t);
            var vo = _model.GetMonthSignAwardVO(t);
            View.UpdateMonthSignItemUI(vo, vo.DayId);
        }

        _model.BuyCounts = res.UserMonthSign.BuyCounts;
        _model.ExtraRewardsState = res.UserMonthSign.ExtraRewardsState;
        _model.MonthSignNum = res.UserMonthSign.Dates.Count;

       
        View.RefreshAccumulativeSignin(_model.MonthSignNum,_model.ExtraRewardsState,_model.TotalDate);
      SendMessage(new Message(MessageConst.CMD_ACTIVITY_REFRESH_ACTIVITYTOGGLE_REDDOT));
    }

    private void MonthRetroactive(MonthSignAwardVO vO)
    {
        //先判断是否还有补签次数
        var maxSignNum = _model.GetMonthSignBuysNum();
        var curSignNum = _model.BuyCounts;
        if (curSignNum >= maxSignNum)
        {
            FlowText.ShowMessage(I18NManager.Get("Activity_Hint4"));  //本月无剩余签到次数
            return;
        }    
        var pB = _model.GetMonthSignBuysRule(curSignNum);        
        if (_window==null)
        {
            _window = PopupManager.ShowWindow<MonthRetroactiveWindow>("Activity/Prefabs/MonthRetroactiveWindow");
            _window.SetData(pB.Gem, maxSignNum - curSignNum);
            _window.WindowActionCallback = evt =>
            {
                if (evt== WindowEvent.Yes)
                {                  
                    SendMonthSignBuyReq(vO);
                }
            };
        }      
    }


    private void SendMonthSignBuyReq(MonthSignAwardVO vO)
    {
        MonthSignBuyReq req = new MonthSignBuyReq { Date = vO.DayId };   
        byte[] data = NetWorkManager.GetByteData(req);
        NetWorkManager.Instance.Send<MonthSignBuyRes>(CMD.ACTIVITY_MONTH_SING_BUY, data, res =>
        {
            Debug.LogError("30补签回包数据："+res.Awards);
            GlobalData.PlayerModel.UpdateUserMoney(res.UserMoney);
            RewardUtil.AddReward(res.Awards); 
            foreach (var t in res.Awards)
            {
                RewardVo vo = new RewardVo(t);
                FlowText.ShowMessage(I18NManager.Get("Activity_Get", vo.Name, vo.Num));
            }
           _model.UpdateUserMonthSignInfo(res.UserMonthSign);
            UpdateMonthSignAwards(res.UserMonthSign);
        });
    }
    
    
    private void UpdateMonthSignAwards(UserMonthSignPB pB)
    {      
        foreach (var t in pB.Dates)
        {
            _model.UpdateMonthSignAwards(t);
            var vo = _model.GetMonthSignAwardVO(t);
            View.UpdateMonthSignItemUI(vo, vo.DayId);
        }

        _model.BuyCounts = pB.BuyCounts;
        _model.ExtraRewardsState = pB.ExtraRewardsState;
        _model.MonthSignNum = pB.Dates.Count;     
        View.RefreshAccumulativeSignin(_model.MonthSignNum,_model.ExtraRewardsState,_model.TotalDate);
    
    }

    /// <summary>
    /// 发送累计18天签到请求
    /// </summary>
    private void SendMonthSiginAccumulativeReq()
    {       
        NetWorkManager.Instance.Send<MonthSignExtraRes>(CMD.ACTIVITY_MONTH_SING_EXTRA, null, MonthSiginAccumulativeSuccCallback);
    }

    /// <summary>
    /// 累计18天回包
    /// </summary>
    /// <param name="res"></param>
    private void MonthSiginAccumulativeSuccCallback(MonthSignExtraRes res)
    {

        foreach (var t in res.Awards)
        {
          Debug.LogError("累计签到回包====》"+t);  
        }

        bool isItem = false;
      //增加奖励
        foreach (var t in res.Awards)
        {
            if (t.Resource== ResourcePB.Item)
            {
                isItem = true;
            }
            RewardUtil.AddReward(t);          
        }
        
        _model.UpdateUserMonthSignInfo(res.UserMonthSign);
        _model.ExtraRewardsState = res.UserMonthSign.ExtraRewardsState;
        View.RefreshAccumulativeSignin(_model.MonthSignNum,_model.ExtraRewardsState,_model.TotalDate);
        View.RefreshAccumulate(_model.TotalDate);


        if (isItem)
        {
            
            var window = PopupManager.ShowWindow<NormalAwardWindow>("Activity/Prefabs/NormalAwardWindow");
            List<RewardVo> list =new List<RewardVo>();
            foreach (var t in res.Awards)
            {
                RewardVo vo =new RewardVo(t);
                list.Add(vo);
            }
            window.SetData(list);
            
        }
        else
        {   
            LoadingOverlay.Instance.Show();
        
            ClientTimer.Instance.DelayCall(() =>
            {

                Action finish = () =>
                {
                    View.ShowMask(true);
                    SendMessage(new Message( MessageConst.CMD_ACTIVITY_SHOW_BAR_AND_BACKBTN));
                };
            
                List<AwardPB> temp = res.Awards.ToList();
           
                SendMessage(new Message(MessageConst.CMD_ACTIVITY_HINT_BAR_AND_BACKBTN));

            
                ModuleManager.Instance.EnterModule(ModuleConfig.MODULE_DRAWCARD,
                    false,false,"DrawCard_CardShow",temp,finish,false);
           
                ClientTimer.Instance.DelayCall(() => {  View. ShowMask(false);},0.2f);
            
                LoadingOverlay.Instance.Hide();
            }, 1.0f); 
            
        }
    
        SendMessage(new Message(MessageConst.CMD_ACTIVITY_REFRESH_ACTIVITYTOGGLE_REDDOT));
    }

}
