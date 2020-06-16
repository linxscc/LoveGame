
using System;
using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Module.Framework.Utils;
using Com.Proto;
using DataModel;
using game.main;
using UnityEngine;


/// <summary>
/// 活动模块红点
/// </summary>
public class ActivityRedDotModel : Model
{

    //星盘十连没改
   
    private List<float> _everyPowerTime;       //每日体力时间点

    #region 主界面活动红点

    /// <summary>
    /// 是否显示活动主界面红点
    /// </summary>
    /// <returns>true是显示,false是不显示</returns>
    public bool IsShowMainPanelRedDot()
    {
        bool isShow = IsShowSevenSigninRedDot() || IsShowMonthSignRedDot() || IsShowEverydayPowerRedDot()
                      || IsShowGrowthFundRedDot() || IsShowMonthCardRedDot() || IsShowDailyGiftRedDot()
                      || IsShowAccumulativeRechargeRedDot()||IsShowSevenSigninTemplateActivityRedDot();
                      
        Debug.Log("ActivityMainPanelRedDot===>"+isShow);
        return isShow;
    }

    #endregion

    /// <summary>
    /// 获取指定活动红点
    /// </summary>
    /// <param name="type">活动类型</param>
    /// <returns></returns>
    public bool GetCurActivityRedDot(ActivityType type)
    {
        switch (type)
        {
            case ActivityType.ActivitySevenDaySignin:
                return IsShowSevenSigninRedDot();
            case ActivityType.ActivityEveryDayPower:
                return IsShowEverydayPowerRedDot();
            case ActivityType.ActivityMonthSignin:
                return IsShowMonthSignRedDot();
            case ActivityType.ActivityGrowthFund:
                return IsShowGrowthFundRedDot();
            case ActivityType.ActivityMonthCard:
                return IsShowMonthCardRedDot();
            case ActivityType.ActivityDailyGift:
                return IsShowDailyGiftRedDot();                   
            case ActivityType.ActivityAccumulativeRecharge:
                return IsShowAccumulativeRechargeRedDot();        
            case ActivityType.ActivitySevenDaySigninTemplate:
                return IsShowSevenSigninTemplateActivityRedDot();
            case ActivityType.ActivityTenDrawCard:
                return IsTenDrawCardRedDot();   
            default:
                return false;
        }
    }
    
    #region 七日红点
   
    /// <summary>
    /// 是否显示七天签到红点
    /// </summary>
    /// <returns>true是显示,false是不显示</returns>
    private bool IsShowSevenSigninRedDot()
    {
        bool isShow = false;

        var curActivity = GlobalData.ActivityModel.GetActivityVo(ActivityType.ActivitySevenDaySignin);
        
        var isOpenSevenSignin = curActivity == null;

        if (isOpenSevenSignin)
        {
            Debug.Log("SevenSignin No Open ===>"+isShow);
            return isShow;            
        }
        
        var isPastDue = curActivity.IsActivityPastDue;
        var curTimeStamp = ClientTimer.Instance.GetCurrentTimeStamp();
        if (!isPastDue)
        {
            var userInfo = GlobalData.ActivityModel.GetUserSevenDaySigninInfo();
            if (userInfo==null)
            {
                isShow = true;
            }
            else
            {
                if (userInfo.SignDay==0)
                {
                    isShow = true;
                }
                else
                {
                    if (userInfo.SignDay!=7 && userInfo.RefreshTime < curTimeStamp)
                    {
                        isShow = true;
                    }
                } 
            }
         
        }    
        
        Debug.Log("SevenDaySigninRedDot===>"+isShow);
        return isShow;
    }

    #endregion

    #region 十抽活动

    private bool IsTenDrawCardRedDot()
    {
        bool isShow = false;
      
        var curActivity = GlobalData.ActivityModel.GetActivityVo(ActivityType.ActivityTenDrawCard);
            
        var isOpenGrowthFund = curActivity == null;
        if (isOpenGrowthFund)
        {
            Debug.LogError("TenDrawCard No Open ===>" + isShow);
            return isShow;
        }
        else
        {
            if (curActivity.IsActivityPastDue)
            {
                Debug.LogError("TenDrawCard is PastDue ===>" + isShow);
                return isShow; 
            }
        }

      var isHave =  GlobalData.ActivityModel.IsGetActivityMissionRedDot(curActivity.ActivityId);
      if (isHave)
      {
          isShow = true;
      }
      else
      {
          var allActivityInfo = GlobalData.ActivityModel.GetActivityTemplateListRes(curActivity.ActivityType);
          if (allActivityInfo!=null)
          {
               var mission = allActivityInfo.UserActivityMissions;
               var rules = GlobalData.ActivityModel.BaseTemplateActivityRule.ActivityMissionRules;
               foreach (var v in mission)
               {
                   if (v.ActivityId != curActivity.ActivityId)
                       continue;
                   if(v.Status != MissionStatusPB.StatusUnclaimed)
                       continue;  
                   ActivityMissionRulePB rulePb = null;
                   foreach (var r in rules)
                   {
                       if (r.ActivityId == v.ActivityId && r.ActivityMissionId == v.ActivityMissionId)
                       {
                           rulePb = r;
                           break;
                       }
                   }

                   if (rulePb == null)
                   {
                       continue;
                   }
                   
                   if (rulePb.Award == null) continue;
                   if (rulePb.Award.Count == 0) continue;
                   isShow = true;
                   break;
               }
          }
      }
      
        
        return isShow;
        
       
//        var mission = allActivityInfo.UserActivityMissions;
//        var rules = GlobalData.ActivityModel.BaseTemplateActivityRule.ActivityMissionRules;
//        foreach (var v in mission) 
//        {
//
//            if (v.ActivityId != curActivity.ActivityId)
//                continue;
//            if(v.Status != MissionStatusPB.StatusUnclaimed)
//                continue;
//
//            ActivityMissionRulePB rulePb = null;
//            foreach (var r in rules)
//            {
//                if(r.ActivityId==v.ActivityId&&r.ActivityMissionId==v.ActivityMissionId)
//                {
//                    rulePb = r;
//                    break;
//                }
//            }
//            if(rulePb==null)
//            {
//                continue;
//            }
//            if (rulePb.Award == null) continue;
//            if (rulePb.Award.Count == 0) continue;
//
//            isShow = true;
//            break;
//
//        }
//
//
//        return isShow;
    }

    #endregion


    #region 月签红点

    /// <summary>
    /// 是否显示月累计奖励红点
    /// </summary>
    /// <returns></returns>
    private bool IsShowMonthSignAccumulativeRedDot()
    {
        bool isShow = false;
        
        var isOpenMonthSign = GlobalData.ActivityModel.GetActivityVo(ActivityType.ActivityMonthSignin)==null;
        if (isOpenMonthSign)
        {
            Debug.Log("MonthSign No Open");
            Debug.Log("MonthSignAccumulativeRedDot===>"+isShow);
            return isShow;
        }
        
        var userMonthSignInfo = GlobalData.ActivityModel.AllActivityInfo.UserMonthSign;
        if (userMonthSignInfo==null)
        {          
            Debug.LogError("userMonthSignInfo is Null ===>"+isShow);
            return isShow;
        }
        else
        {
            var count = userMonthSignInfo.Dates.Count;
            var isGet = userMonthSignInfo.ExtraRewardsState;
            if (count>=18 && isGet==0)
            {
                isShow = true;
                Debug.Log("MonthSignAccumulativeRedDot===>"+isGet);
            }
        }

        return isShow;

    }
    
      /// <summary>
    /// 是否显示月签红点
    /// </summary>
    /// <returns></returns>
    private bool IsShowMonthSignRedDot()
    {     
        bool isShow = false;

        var isOpenMonthSign = GlobalData.ActivityModel.GetActivityVo(ActivityType.ActivityMonthSignin)==null;
        if (isOpenMonthSign)
        {
            Debug.Log("MonthSign No Open");
            Debug.Log("MonthSignRedDot===>"+isShow);
            return isShow;
        }
        
        List<MonthSignAwardVO> awardVos =new List<MonthSignAwardVO>();
               
        var monthSignRules = GlobalData.ActivityModel.BaseActivityRule.MonthSignRules;
        var userMonthSignInfo = GlobalData.ActivityModel.AllActivityInfo.UserMonthSign;
        int monthDays;  //月天数
        var starDay=GlobalData.ConfigModel.GetConfigByKey(GameConfigKey.MONTH_SIGN_RESET_DAY);//11号
        int index = starDay - 1;
        int toDayIdIndex;
       
        
        var dt =  DateUtil.GetTodayDt();
        var lastMonthDt = dt.AddMonths(-1);//上个月的Dt
        var curTimeStamp =  ClientTimer.Instance.GetCurrentTimeStamp();   
        var sixTimeStamp =DateUtil.GetNotTimezoneTimeStamp(new DateTime(dt.Year, dt.Month, starDay, 6, 0, 0));//每月11号6点，跨月刷新点

        if (curTimeStamp<sixTimeStamp)
        {                   
            monthDays =DateTime.DaysInMonth(lastMonthDt.Year, lastMonthDt.Month);
            toDayIdIndex = DurationDays(true, starDay, lastMonthDt, dt);
        }
        else
        {           
            monthDays = DateTime.DaysInMonth(dt.Year, dt.Month);
            toDayIdIndex = DurationDays(false, starDay, lastMonthDt, dt);
        }

        for (int i = 0; i < monthDays; i++)
        {
           var vo = new MonthSignAwardVO(monthSignRules[index],i+1); 
           awardVos.Add(vo);
           index++;
           if (index>=monthSignRules.Count)
           {
               index = 0;
           }
        }

        Debug.LogError("toDayIdIndex===>"+toDayIdIndex);
        
        int toDayId = awardVos[toDayIdIndex].Id;
        Debug.LogError("toDayId===>"+toDayId);
        if (userMonthSignInfo==null)
        {
            isShow = true;
            Debug.LogError("userMonthSignInfo is Null ===>"+isShow);
            return isShow;
        }
        else
        {
            var dates = userMonthSignInfo.Dates;
            foreach (var t in awardVos)
            {
                if (t.Id<= toDayId)
                {
                    if (!dates.Contains(t.DayId)&& t.Id==toDayId)
                    {
                        isShow = true;
                        Debug.Log("MonthSignRedDot===>"+isShow);
                        return isShow;
                    }
                } 
            }
        }

        isShow = IsShowMonthSignAccumulativeRedDot();
        Debug.Log("MonthSignRedDot===>"+isShow);
        return isShow;
    }

    /// <summary>
    /// 月签从11号开始持续的天数
    /// </summary>
    /// <param name="isLastMonth">上个月DT</param>
    /// <param name="starDay">开始的日期天数 11号</param>
    /// <param name="lastMonthDt"></param>
    /// <param name="nowDt">New Dt</param>
    /// <returns></returns>
    private int DurationDays(bool isLastMonth,int starDay,DateTime lastMonthDt,DateTime nowDt)
    {
        var dateTime = isLastMonth ? lastMonthDt : nowDt;
               
        var dt1 =new DateTime(dateTime.Year,dateTime.Month,starDay,6,0,0);
        var dt2 = new DateTime(nowDt.Year,nowDt.Month,nowDt.Day,nowDt.Hour,nowDt.Minute,nowDt.Second);
        
        var ts1 = new TimeSpan(dt1.Ticks);            
        var ts2 = new TimeSpan(dt2.Ticks);

        return ts1.Subtract(ts2).Duration().Days;
    }

    #endregion

    #region 每日体力红点

    /// <summary>
    /// 是否显示每日体力红点
    /// </summary>
    /// <returns>true是显示,false是不显示</returns>
    private bool IsShowEverydayPowerRedDot()
    {
        bool isShow = false;
        var isOpenEverydayPower = GlobalData.ActivityModel.GetActivityVo(ActivityType.ActivityEveryDayPower) == null;
        if (isOpenEverydayPower)
        {          
            Debug.Log("EverydayPower No Open ===>"+isShow);
            return isShow;
        }

        var dt = DateUtil.GetDataTime(ClientTimer.Instance.GetCurrentTimeStamp());
        float time = dt.Hour * 60 + dt.Minute;

        var rules = GlobalData.ActivityModel.BaseActivityRule.ActivityPowerGetRules;
        var userPowerGetInfo = GlobalData.ActivityModel.GetUserPowerGottenIds();


        if (_everyPowerTime==null)
        {
            _everyPowerTime =new List<float>();
            foreach (var t in rules)
            {
                var starStr = t.Start.Split(':');
                var starMinute = float.Parse(starStr[0]) * 60 + float.Parse(starStr[1]) ;
               
                var endStr = t.End.Split(':');
                var endMinute = float.Parse(endStr[0]) * 60+ float.Parse(endStr[1]) ;
                
                _everyPowerTime.Add(starMinute);
                _everyPowerTime.Add(endMinute);
            }
            
        }

        //不在领取范围内
        if (time<_everyPowerTime[0]||time>_everyPowerTime[_everyPowerTime.Count-1])
        {
            Debug.Log("Everyday Power Not Time Range ===>"+isShow);
            return isShow;
        }


        int getPowerId = 0;
        for (int i = 0; i < _everyPowerTime.Count; i+=2)
        {
            getPowerId++;
            var isCurTime = _everyPowerTime[i] <= time && time <= _everyPowerTime[i + 1];
            var isGet = userPowerGetInfo.Contains(getPowerId);
            if (isCurTime&& !isGet)
            {
                isShow = true;
            }
        }               
        Debug.Log("EverydayPowerRedDot===>"+isShow);
        return isShow;
    }

    #endregion

    #region 应援储备金红点
    
    /// <summary>
    /// 是否显示应援储备金红点
    /// </summary>
    /// <returns>true是显示,false是不显示</returns>
    private bool IsShowGrowthFundRedDot()
    {
        bool isShow = false;
        
        var isOpenGrowthFund = GlobalData.ActivityModel.GetActivityVo(ActivityType.ActivityGrowthFund) == null;
        if (isOpenGrowthFund)
        {
            Debug.Log("GrowthFund No Open ===>"+isShow);          
            return isShow;
        }

        var playerVo = GlobalData.PlayerModel.PlayerVo;
        
        if (playerVo.ExtInfo.GrowthFund == 0) //0是没购买，1是购买;没购买过不应该显示红点
        {
            Debug.Log("Player No Buy GrowthFund ===> "+isShow);
            return isShow; 
        }

        int playerLevel = playerVo.Level;

        var rules = GlobalData.ActivityModel.BaseActivityRule.GrowthFundRules;
        var userGrowthData = GlobalData.ActivityModel.AllActivityInfo.UserGrowthFund?.AwardStates;        
        var isNull = userGrowthData == null;
        
        foreach (var t in rules)
        {
            var isReachLevel = t.DepartmentLevel <= playerLevel;  //是否达到等级
            Debug.LogError("isReachLevel===>"+isReachLevel);
            if(!isReachLevel)
                break;
                        
            if (isNull)
            {
                isShow = true;
               break;
            }
            else
            {                            
                if (!userGrowthData.Contains(t.Id))
                {
                    isShow = true;
                    break;
                }
           }
        }
        
        
        Debug.Log("IsShowGrowthFundRedDot===>"+isShow);
        return isShow;

    }

    #endregion

    #region 月卡红点

    /// <summary>
    /// 是否显示月卡红点
    /// </summary>
    /// <returns>true是显示,false是不显示</returns>
    private bool IsShowMonthCardRedDot()
    {
        bool isShow = false;

        var isOpenMonthCard = GlobalData.ActivityModel.GetActivityVo(ActivityType.ActivityMonthCard) == null;
        if (isOpenMonthCard)
        {
            Debug.Log("MonthCard No Open ===>"+isShow);          
            return isShow;
        }
        
        
        var isSwitch = AppConfig.Instance.SwitchControl.Recharge;
        if (!isSwitch)
        {
            Debug.Log("MonthCard Switch No Open ===>"+isShow);
            return isShow;
        }

        var userMonthCard = GlobalData.PlayerModel.PlayerVo.UserMonthCard;
        var isNull = userMonthCard == null;

        if (isNull)
        {
            Debug.Log("userMonthCard is Null ===>"+isShow);
        }
        else
        {
            var curTimeStamp = ClientTimer.Instance.GetCurrentTimeStamp();
            isShow = curTimeStamp - userMonthCard.PrizeTime >= 0 && userMonthCard.EndTime > curTimeStamp;
            Debug.Log("IsShowMonthCardRedDot ===>"+isShow);
        }
        
        
        return isShow;
    }
    

    #endregion

    #region 每日礼包红点

    /// <summary>
    /// 是否显示每日礼包红点
    /// </summary>
    /// <returns></returns>
    private bool IsShowDailyGiftRedDot()
    {
        bool isShow = false;

        var curActivity = GlobalData.ActivityModel.GetActivityVo(ActivityType.ActivityDailyGift);
        var isOpenDailyGift = curActivity == null;
        if (isOpenDailyGift)
        {
            Debug.Log("DailyGift No Open ===>"+isShow);          
            return isShow;
        }
        
        var isPastDue = curActivity.IsActivityPastDue;
        if (isPastDue)
        {
            Debug.Log("DailyGift PastDue ===>"+isShow);
            return isShow;
        }
        
        isShow =!GlobalData.PlayerModel.PlayerVo.ExtInfo.GotDailyPackageStatus;
        
        Debug.Log("DailyGiftRedDot===>"+isShow);
        return isShow;
    }
  
    #endregion

    #region 累计充值红点

    /// <summary>
    /// 是否显示累计充值红点
    /// </summary>
    /// <returns></returns>
    private bool IsShowAccumulativeRechargeRedDot()
    {
        bool isShow = false;

        var isOpenAccumulativeRecharge =
            GlobalData.ActivityModel.GetActivityVo(ActivityType.ActivityAccumulativeRecharge) == null;
        if (isOpenAccumulativeRecharge)
        {
            Debug.Log("AccumulativeRecharge No Open ===>"+ isShow);
            return isShow;
        }

        var userInfo = GetEndLongUserActivityAccumulativeRechargeInfo();

        if (userInfo==null)
        {
            Debug.Log("AccumulativeRecharge UserInfo is Null ===>"+ isShow);
            return isShow;
        }
        
        List<AccumulativeRechargeVO> accumulativeRechargeVos=new List<AccumulativeRechargeVO>();

        var rules = GlobalData.ActivityModel.BaseActivityRule.ActivityAccumulativeRechargeRules;

        foreach (var v in rules)
        {
            AccumulativeRechargeVO vo = new AccumulativeRechargeVO(v, userInfo.ReceiveStatus, userInfo.Amount);  
            accumulativeRechargeVos.Add(vo);
        }

        if (accumulativeRechargeVos.Count > 0)
        {
            foreach (var v in accumulativeRechargeVos)
            {
                if (v.Weight == 2)
                {
                    isShow = true;
                    break;
                }
                
            }
        }

        Debug.Log("AccumulativeRecharge RedDot ===>"+isShow);
        return isShow;

    }

    private UserActivityAccumulativeRechargeInfoPB GetEndLongUserActivityAccumulativeRechargeInfo()
    {
        var allActivityInfo = GlobalData.ActivityModel.AllActivityInfo;
        var activitys = allActivityInfo.Activitys;
        
        foreach (var v in activitys)
        {
            if (v.Type == ActivityTypePB.ActivityAccumulativeRecharge && v.EndType == ActivityEndTypePB.EndLong)
            {
                if (allActivityInfo.UserActivityAccumulativeRechargeInfo.Count > 0)
                {
                    foreach (var a in allActivityInfo.UserActivityAccumulativeRechargeInfo)
                    {
                        if (a.ActivityId == v.ActivityId)
                        {
                            return a;
                        }
                    }
                }
            }
        }

        return null;
    }

    #endregion

    #region 七日签到模板红点


    private bool IsShowSevenSigninTemplateActivityRedDot()
    {
        bool isShow = false;
        var curActivity = GlobalData.ActivityModel.GetActivityVo(ActivityType.ActivitySevenDaySigninTemplate);
        var isOpenSevenSignin = curActivity == null;


        if (isOpenSevenSignin)
        {
             Debug.Log("SevenSigninTemplate No Open ===>"+isShow);
             return isShow; 
        }
        
        var isPastDue = curActivity.IsActivityPastDue;
        var curTimeStamp = ClientTimer.Instance.GetCurrentTimeStamp();
        if (!isPastDue)
        {
            var userInfo = GlobalData.ActivityModel.GetUserSevenDaySigninTemplateInfo();
            if (userInfo==null)
            {
                isShow = true;
            }
            else
            {
                if (userInfo.SignDay==0)
                {
                    isShow = true;
                }
                else
                {
                    if (userInfo.SignDay!=7 && userInfo.RefreshTime < curTimeStamp)
                    {
                        isShow = true;
                    }
                } 
            }
         
        }
        Debug.Log("SevenSigninTemplateRedDot===>"+isShow);
        return isShow;
    }
    

    #endregion
  
}
