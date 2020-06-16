using System;
using System.Collections;
using System.Collections.Generic;
using Com.Proto;
using DataModel;
using game.main;
using UnityEngine;

public class ActivityVo
{

    public int ActivityId;
    public string Name;
    public string BackgroundPicture;
    public string Desc;
    public long StarTime;
    public long EndTime;
    public long ShowEndTime;
    public int Sort;
    public ActivityEndTypePB EndType;
        
    /// <summary>
    ///是否显示页签(0是需要，1是不需要)
    /// </summary>
    public int IsDisplay;
        
    /// <summary>
    /// 针对用户过期时间：单位小时
    /// </summary>
    public int OverdueTime;
    
    /// <summary>
    ///唯一标志：用于判断活动是否是新活动(0是旧的，1是新的)
    /// </summary>
    public int UniqueIdentify;
    
    /// <summary>
    /// 后端传过来的活动类型
    /// </summary>
    public ActivityTypePB BaseActivityType;
        
    /// <summary>
    /// 是否需要做过期判断
    /// </summary>
    public bool IsNeedPastDue;    
    
    /// <summary>
    /// 活动Type前端自己用的
    /// </summary>
    public ActivityType ActivityType;
       
    /// <summary>
    /// 唯一Id
    /// </summary>
    public string JumpId;

    /// <summary>
    /// 是否需要开关控制
    /// </summary>
    public bool IsNeedRechargeSwitchController;

    /// <summary>
    /// 主界面Bar的Path
    /// </summary>
    public string GameMainBarPath;

    /// <summary>
    /// 活动Toggle背景图Path
    /// </summary>
    public string ActivityToggleBgPath;

    /// <summary>
    /// 活动Toggle三角框Path
    /// </summary>
    public string ActivityToggleFramePath;
   
    /// <summary>
    /// 活动是否过期
    /// </summary>
    public bool IsActivityPastDue => IsPastDue();

   /// <summary>
   /// 活动额外信息
   /// </summary>
    public ActivityExtraPB ActivityExtra;


   public bool RedDod;
   
    public ActivityVo(ActivityPB pb)
    {
           
        ActivityId = pb.ActivityId;
        Name = pb.Name;
        BackgroundPicture = pb.BackgroundPicture;
        Desc = pb.Desc;
        StarTime = pb.StartTime;
        EndTime = pb.EndTime;
        ShowEndTime = pb.ShowEndTime;
        Sort = pb.Sort;
        EndType = pb.EndType;
        IsDisplay = pb.IsDisplay;
        OverdueTime = pb.OverdueTime;
        UniqueIdentify = pb.UniqueIdentify;
        BaseActivityType = pb.Type;
        ActivityExtra = pb.ActivityExtra;
        InitActivityType(pb.Type,pb.ActivityId);
        InitIsNeedPastDue(pb.EndType);
        InitIsNeedRechargeSwitchController(pb.Type);
        InitTexturePath();
        JumpId = "Activity"+"T"+(int)BaseActivityType + "I"+ActivityId;       
    }


    /// <summary>
    /// 初始化活动图片路径
    /// </summary>
    private void InitTexturePath()
    {
        GameMainBarPath = "GameMain/"+BackgroundPicture;

        if (IsDisplay==0) //展示在活动内部的
        {
            ActivityToggleBgPath = "Activity/ActivityName"+BackgroundPicture;
            ActivityToggleFramePath = "Activity/TriangleFrame"+BackgroundPicture;
        }                           
    }


 

    /// <summary>
    /// 初始化前端活动类型
    /// </summary>
    /// <param name="baseType">后端的类型</param>
    /// <param name="activityId">活动Id</param>
    private void InitActivityType(ActivityTypePB baseType,int activityId)
    {
        switch (baseType)
        {          
            case ActivityTypePB.ActivitySevenDaySig:
                if (activityId == 1)
                {
                    ActivityType = ActivityType.ActivitySevenDaySignin;
                }
                else
                {
                    ActivityType = ActivityType.ActivitySevenDaySigninTemplate; 
                }
//                if (activityId == 1)
//                   
//                else if (activityId == 11) 
                break;
            case ActivityTypePB.ActivityPowerGet:
                ActivityType = ActivityType.ActivityEveryDayPower;
                break;
            case ActivityTypePB.ActivityMonthSign:
                ActivityType = ActivityType.ActivityMonthSignin;
                break;
            case ActivityTypePB.ActivityGrowthFund:             
                ActivityType = ActivityType.ActivityGrowthFund;
                break;
            case ActivityTypePB.ActivityMonthCard:
                ActivityType = ActivityType.ActivityMonthCard;             
                break;
            case ActivityTypePB.ActivityDailyGift:
                ActivityType = ActivityType.ActivityDailyGift;
                break;
            case ActivityTypePB.ActivityMonthCardRecharge:
                ActivityType = ActivityType.ActivityMonthCardRecharge;
                break;
            case ActivityTypePB.ActivityDrawTemplate:
                if (activityId==7)
                {
                    ActivityType = ActivityType.ActivityDrawTemplateYueHuaRuLian;
                }
                else if(activityId==9)
                {
                    ActivityType = ActivityType.ActivityDrawTemplateLoveHoliday;
                }
                else if(activityId==10)
                {
                    ActivityType = ActivityType.ActivityDrawTemplateSweetWeekend;
                }
                break;
            case ActivityTypePB.ActivityAccumulativeRecharge:
                ActivityType = ActivityType.ActivityAccumulativeRecharge;
                break;
            case ActivityTypePB.ActivityCapsuleTemplate:
                ActivityType = ActivityType.ActivityCapsuleTemplate; 
                break;
            case ActivityTypePB.ActivityDrawTenContinuous:
                ActivityType = ActivityType.ActivityTenDrawCard;
                break;
            case ActivityTypePB.ActivityMusicGame :
                if (activityId==14)
                {
                    ActivityType= ActivityType.ActivityMusicGameTemplate; 
                }
                else if(activityId==15)
                {
                    ActivityType = ActivityType.ActivityTangBirthdayMusic;
                }
                
                break;
           
        }
    }


    /// <summary>
    /// 初始化是否需要做过期判断
    /// </summary>
    /// <param name="endType"></param>
    private void InitIsNeedPastDue(ActivityEndTypePB endType)
    {
        switch (endType)
        {
            case ActivityEndTypePB.EndLong:
                IsNeedPastDue = false;
                break;
            case ActivityEndTypePB.EndRule:
            case ActivityEndTypePB.EndUser:
                IsNeedPastDue = true;
                break;                
        } 
    }

    /// <summary>
    /// 初始化是否需要做充值开关控制
    /// </summary>
    /// <param name="baseType"></param>
    private void InitIsNeedRechargeSwitchController(ActivityTypePB baseType)
    {
        IsNeedRechargeSwitchController = baseType == ActivityTypePB.ActivityMonthCard;
    }


    /// <summary>
    /// 活动是否过期
    /// </summary>
    /// <returns></returns>
    private bool IsPastDue()
    {
        bool isPastDue = false;

        switch (EndType)
        {           
            case ActivityEndTypePB.EndLong:
                isPastDue = false;
                break;
            case ActivityEndTypePB.EndRule:
                isPastDue = IsPastDueToEndRule();
                break;
            case ActivityEndTypePB.EndUser:
                isPastDue = IsPastDueToUser();
                break;
        }

        return isPastDue;
    }


    /// <summary>
    /// 是否过期根据规则
    /// </summary>
    /// <returns></returns>
    private bool IsPastDueToEndRule()
    {
        bool isEndRulePastDue = false;
        var curTimeStamp = ClientTimer.Instance.GetCurrentTimeStamp();
        if (curTimeStamp<StarTime||curTimeStamp>=EndTime)
        {
            isEndRulePastDue = true;
        }             
        return isEndRulePastDue;
    }

    
    private bool IsPastDueToUser()
    {
        bool isPastDueToUser = false;
        switch (ActivityType)
        {
            case  ActivityType.ActivityTenDrawCard:
                isPastDueToUser = IsTenDrawCardPastDue();
                break;
            case  ActivityType.ActivitySevenDaySignin:
                isPastDueToUser = IsSevenDaySigninPastDue();
                break;
        }

        return isPastDueToUser;
    }

    /// <summary>
    /// 七天签到根据用户走
    /// </summary>
    /// <returns></returns>
    private bool IsSevenDaySigninPastDue()
    {
        bool isPastDue = false;
        UserSevenDaySigninInfoPB userInfo = null;
        var userSigninList = GlobalData.ActivityModel.UserSevenDaySigninInfos;
        foreach (var t in userSigninList)
        {
            if (t.ActivityId ==ActivityId)
            {
                userInfo = t;
                break;
            }
        }
        var timeOffset = OverdueTime * 60 * 60 * 1000;
        var endTime = userInfo?.StartSignInTime + timeOffset;
        var curTimeStamp = ClientTimer.Instance.GetCurrentTimeStamp();

        if (curTimeStamp>=endTime)
        {
            isPastDue = true;
        }
        
        return isPastDue;
    }

    /// <summary>
    /// 抽卡十连活动
    /// </summary>
    /// <returns></returns>
    private bool IsTenDrawCardPastDue()
    {
        bool isNew = IsNewDrawCardTenContinuousActivity();
        var createTime = GlobalData.PlayerModel.PlayerVo.CreateTime;
        long endTime = 0;
        if (isNew)
        {
            endTime= createTime +OverdueTime * 60 * 60 * 1000; ; 
        }
        else
        {
            long currentTimeStamp = ClientTimer.Instance.GetCurrentTimeStamp();
            long startTime = 0; 
            GlobalData.ActivityModel.AllActivityInfo.UserActivityExtraInfo.ActivityIsOpenOrTime.TryGetValue(ActivityId, out startTime);
            
            if (startTime < GlobalData.ConfigModel.GetGameTimeConfigByKey(GameConfigKey.OLD_ACTIVITY_DRAW_TEN_CONTINUOUS_START_TIME)||
                startTime > GlobalData.ConfigModel.GetGameTimeConfigByKey(GameConfigKey.OLD_ACTIVITY_DRAW_TEN_CONTINUOUS_END_TIME))
            {
                return false;
            }
            
            if (startTime > currentTimeStamp)
            {
                return false;
            }
            endTime = startTime +OverdueTime * 60 * 60 * 1000;
        }
        return endTime < ClientTimer.Instance.GetCurrentTimeStamp();
    }

    private bool IsNewDrawCardTenContinuousActivity()
    {
        var timePoint = GlobalData.ConfigModel.GetGameTimeConfigByKey(GameConfigKey.NEW_ACTIVITY_DRAW_TEN_CONTINUOUS_TIME);
        var createTime = GlobalData.PlayerModel.PlayerVo.CreateTime;
        return createTime >= timePoint;
    }
}
