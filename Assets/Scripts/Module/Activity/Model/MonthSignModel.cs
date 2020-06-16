using Assets.Scripts.Module.Framework.Utils;
using Com.Proto;
using DataModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using game.main;
using Google.Protobuf.Collections;
using UnityEngine;


/// <summary>
/// 月签Model
/// </summary>
public class MonthSignModel
{
    private List<MonthSignAwardVO> _monthSignAwards;//月签奖励  
    private Dictionary<int, MonthSignExtraVO> _monthSignExtras;   //月签到累计奖励  
    
    
    private List<MonthSignRulePB> _monthSignRule;      //月签规则
    private List<MonthSignBuyRulePB> _monthSignBuyRule; //月签补签规则
    private List<MonthSignExtraRulePB> _monthSignExtraRule; //月签累计奖励规则

    private UserMonthSignPB _userMonthSign;
    
    
    //后端没有给用户月签信息初始化，前端需要自己初始化
    private int _buyCounts;                               
    private int _extraRewardsState;
    private int _monthSignNum;
    private int _totalDate;
    public int ToDayId;
    
    
    //开始的天
    private readonly int _starDay = GlobalData.ConfigModel.GetConfigByKey(GameConfigKey.MONTH_SIGN_RESET_DAY);
    
    //累计18天
    public int TotalDate => _totalDate;

    /// <summary>
    /// 已经补签次数
    /// </summary>
    public int BuyCounts
    {
        get { return _buyCounts; }
        set { _buyCounts = value; }     
    }
  
    /// <summary>
    /// 累计签到奖励领取状态0未领取1已领取
    /// </summary>
    public int ExtraRewardsState
    {
        get { return _extraRewardsState; }
        set { _extraRewardsState = value; }       
    }

    /// <summary>
    /// 月签累计次数
    /// </summary>
    public int MonthSignNum
    {
        get { return _monthSignNum; }
        set { _monthSignNum = value; }      
    }

    public MonthSignModel()
    {
        InitRule();
        InitUserInfo();
        InitMonthSignAwards();       
        InitMonthSignExtras();
    }


    /// <summary>
    /// 初始化规则
    /// </summary>
    private void InitRule()
    {
        var baseActivityRule = GlobalData.ActivityModel.BaseActivityRule;      
       _monthSignRule = baseActivityRule.MonthSignRules.ToList(); //初始化月签规则     
       _monthSignExtraRule = baseActivityRule.MonthSignExtraRules.ToList(); //初始化月签累计规则     
       _monthSignBuyRule = baseActivityRule.MonthSignBuyRules.ToList(); //初始化月签补签规则
    }


    private void InitUserInfo()
    {
        var allInfo = GlobalData.ActivityModel.AllActivityInfo;
        _userMonthSign = allInfo.UserMonthSign;
    }
        
    
    /// <summary>
    /// 初始化月签奖励
    /// </summary>
    private void InitMonthSignAwards()
    {       
        _monthSignAwards = new List<MonthSignAwardVO>();
    
        int monthDays = 0;   //月的天数        
        int index = _starDay-1;  //11号的下标索引
        int toDayIdIndex = 0;
        
        var dt = DateUtil.GetTodayDt();
        var curTimeStamp = ClientTimer.Instance.GetCurrentTimeStamp();    
        var sixTimeStamp = DateUtil.GetNotTimezoneTimeStamp(new DateTime(dt.Year, dt.Month, _starDay, 6, 0, 0));
      
        if (curTimeStamp<sixTimeStamp )  //自然日期小于11号的6点，是上个月
        {
            var lastMonthDt = dt.AddMonths(-1);  //获取上个月的DT          
            monthDays =DateTime.DaysInMonth(lastMonthDt.Year, lastMonthDt.Month);  //获取上个月的天数
            Debug.LogError("上个月的天数monthDays===>"+monthDays);
            var dt1 = new DateTime(lastMonthDt.Year,lastMonthDt.Month,_starDay,6,0,0);           
            var dt2 =new DateTime(dt.Year,dt.Month,dt.Day,dt.Hour,dt.Minute,dt.Second);

            var ts1 = new TimeSpan(dt1.Ticks);            
            var ts2 = new TimeSpan(dt2.Ticks);

            var tsSub = ts1.Subtract(ts2).Duration();
            toDayIdIndex = tsSub.Days;
            Debug.LogError("上个月toDayIdIndex===>"+toDayIdIndex);
       }
        else    //自然日期>=11号是且大于11号早上6点，本月                                                                            
        {
            monthDays = DateTime.DaysInMonth(dt.Year, dt.Month); 
            Debug.LogError("当月的天数monthDays===>"+monthDays);
            var dt1 = new DateTime(dt.Year,dt.Month,_starDay,6,0,0);
            var dt2 =new DateTime(dt.Year,dt.Month,dt.Day,dt.Hour,dt.Minute,dt.Second);
            
            var ts1 = new TimeSpan(dt1.Ticks);            
            var ts2 = new TimeSpan(dt2.Ticks);
            
            var tsSub = ts1.Subtract(ts2).Duration();
            toDayIdIndex = tsSub.Days; 
            Debug.LogError("当月toDayIdIndex===>"+toDayIdIndex);
        }
  
        for (int i = 0; i < monthDays; i++)
        {                       
            var vo = new MonthSignAwardVO(_monthSignRule[index],i+1);                       
            _monthSignAwards.Add(vo);
            index++;
            if (index>=_monthSignRule.Count)
            {
                index = 0;
            }
        }
        ToDayId = _monthSignAwards[toDayIdIndex].Id;
        Debug.LogError("ToDayId===>"+ToDayId);
        SetSignState();
       
    }

    /// <summary>
    /// 得到月签奖励
    /// </summary>
    /// <returns></returns>
    public List<MonthSignAwardVO> GetMonthSignAwards()
    {
        return _monthSignAwards;
    }

    public void UpdateUserMonthSignInfo(UserMonthSignPB pB)
    {
        GlobalData.ActivityModel.AllActivityInfo.UserMonthSign = pB;
        _userMonthSign = pB;
        
    }

    public MonthSignAwardVO GetMonthSignAwardVO(int day)
    {
        MonthSignAwardVO vo = null;
        foreach (var t in _monthSignAwards)
        {
            if (t.DayId == day)
            {
                vo = t;
                break;
            }
        }

        return vo;
    }

    /// <summary>
    /// 更新月签奖励
    /// </summary>
    /// <param name="day">第几天</param>
    public void UpdateMonthSignAwards(int day)
    {
        foreach (var t in _monthSignAwards)
        {
            if (t.DayId ==day)
            {
                  t.State =EveryDaySignState.AlreadyGet;
                  break;
            }  
        }       
    }

    /// <summary>
    /// 初始化月签到累计  
    /// </summary>
    private void InitMonthSignExtras()
    {      
         _monthSignExtras = new Dictionary<int, MonthSignExtraVO>();
         
        foreach (var t in _monthSignExtraRule)
        {         
            var monthSignExtraVo = new MonthSignExtraVO(t);
            _monthSignExtras.Add(t.Month, monthSignExtraVo);
        }
     
        var dt = DateUtil.GetTodayDt();
        var curTimeStamp = ClientTimer.Instance.GetCurrentTimeStamp();
        var sixTimeStamp = DateUtil.GetNotTimezoneTimeStamp(new DateTime(dt.Year, dt.Month, _starDay, 6, 0, 0));

        _totalDate = curTimeStamp<sixTimeStamp ?
            _monthSignExtras[DateUtil.GetTodayDt().AddMonths(-1).Month].TotalDate : _monthSignExtras[DateUtil.GetTodayDt().Month].TotalDate;
    }


    /// <summary>
    /// 获取当月累计签到奖励
    /// </summary>
    /// <returns></returns>
    public MonthSignExtraVO GetCurMonthSignExtraAward()
    {
        var dt = DateUtil.GetTodayDt();
        var curTimeStamp = ClientTimer.Instance.GetCurrentTimeStamp();
        var sixTimeStamp = DateUtil.GetNotTimezoneTimeStamp(new DateTime(dt.Year, dt.Month, _starDay, 6, 0, 0));
        
        if (curTimeStamp<sixTimeStamp)
        {
            return _monthSignExtras[DateUtil.GetTodayDt().AddMonths(-1).Month];
        }
        else
        {
            return _monthSignExtras[DateUtil.GetTodayDt().Month];
        }               
    }


    /// <summary>
    /// 获取月补签规则
    /// </summary>
    /// <param name="retroactiveNum">补签次数</param>
    /// <returns>对应次数的规则</returns>
    public MonthSignBuyRulePB GetMonthSignBuysRule(int retroactiveNum)
    {
        MonthSignBuyRulePB pB = null;
        retroactiveNum = retroactiveNum + 1;      
        foreach (var t in _monthSignBuyRule)
        {
            if (retroactiveNum == t.Count)
            {
                pB = t;
                break;
            }
        }
        return pB;
    }

   

    /// <summary>
    /// 获取月补签上限
    /// </summary>
    /// <returns></returns>
    public int GetMonthSignBuysNum()
    {
        return _monthSignBuyRule.Count;
    }
  

    /// <summary>
    /// 设置月签的状态
    /// </summary>
    private void SetSignState()
    {                       
       // var curTimeStamp = ClientTimer.Instance.GetCurrentTimeStamp();
       // var toDaySixDt = new DateTime(DateUtil.GetTodayDt().Year,DateUtil.GetTodayDt().Month,DateUtil.GetTodayDt().Day,6,0,0);
       // var sixPointTimeStamp = DateUtil.GetNotTimezoneTimeStamp(toDaySixDt);
        
        
        //拉用户月签信息时，后端没有初始化，前端需要进行非空判断
        if (_userMonthSign== null) 
        {
            _buyCounts = 0;
            _extraRewardsState = 0;
            _monthSignNum = 0;
           
            foreach (var t in _monthSignAwards)
            {
                if (t.Id<ToDayId)
                {
                    t.State = EveryDaySignState.Retroactive;
                }
                else if(t.Id ==ToDayId )
                {
                    t.State = EveryDaySignState.CurMayGet;
                }              
            }
        }
        else
        {
            _buyCounts = _userMonthSign.BuyCounts;
            _extraRewardsState = _userMonthSign.ExtraRewardsState;
            _monthSignNum = _userMonthSign.Dates.Count;
            var dates = _userMonthSign.Dates;//用户签到日期列表

            foreach (var t in dates)
            {
                Debug.LogError("签到===>"+t);
            }
            
            foreach (var t in _monthSignAwards)
            {
                if (t.Id<=ToDayId)
                {                   
                    if (dates.Contains(t.DayId))
                    {                       
                        t.State = EveryDaySignState.AlreadyGet;
                    }
                    else if(!dates.Contains(t.DayId) && t.Id!=ToDayId)
                    {                       
                        t.State = EveryDaySignState.Retroactive;
                    }
                    else if(!dates.Contains(t.DayId)&& t.Id ==ToDayId)
                    {                       
                        t.State = EveryDaySignState.CurMayGet;
                    }
                 
                }
            }                      
        }

    }


  
    
 

}
