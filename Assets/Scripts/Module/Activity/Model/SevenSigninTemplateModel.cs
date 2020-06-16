using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core;
using Com.Proto;
using DataModel;
using UnityEngine;

public class SevenSigninTemplateModel : Model
{
    private List<SevenSigninTemplateAwardVo> _sevenSigninTemplateAwards;   //七天签到模板奖励
    private List<ActivityOptionalAwardRulePB> _sevenSigninTemplateRule;    //七天签到模板规则
    private UserSevenDaySigninInfoPB _userSevenDaySigninInfo;              //七天签到模板用户信息

    /// <summary>
    /// 七日签到模板活动结束时间
    /// </summary>
    public long EndTimeTamp;
    
    public SevenSigninTemplateModel()
    {
        InitRule();
        InitUserInfo();
        InitSevenSigninTemplateAwards();
    }

    private void InitRule()
    {
        var rule = GlobalData.ActivityModel.BaseActivityRule.ActivityOptionalAwardRules;
        var activity = GlobalData.ActivityModel.GetActivityVo(ActivityType.ActivitySevenDaySigninTemplate);
        EndTimeTamp = activity.EndTime;
        
        _sevenSigninTemplateRule =new List<ActivityOptionalAwardRulePB>();
        foreach (var t in rule)
        {
            if (t.OptionalActivityType== OptionalActivityTypePB.OptionalSevenDaySig &&
                t.ActivityId == activity.ActivityId)
            {
                _sevenSigninTemplateRule.Add(t); 
            }
           
        }

        
    }

    private void InitUserInfo()
    {
        _userSevenDaySigninInfo = GlobalData.ActivityModel.GetUserSevenDaySigninTemplateInfo();
//        var activityId = GlobalData.ActivityModel.GetActivity(ActivityTypePB.ActivitySevensigninTemplate).ActivityId;
//        var userInfoList = GlobalData.ActivityModel.UserSevenDaySigninInfos;
//        if (userInfoList.Count==0)
//        {
//            _userSevenDaySigninInfo = null;
//        }
//        else
//        {
//            foreach (var t in userInfoList)
//            {
//                _userSevenDaySigninInfo = t.ActivityId==activityId ? t : null;
//            }
//        }      
    }

    private void InitSevenSigninTemplateAwards()
    {
        if (_sevenSigninTemplateAwards == null)
        {
            _sevenSigninTemplateAwards =new List<SevenSigninTemplateAwardVo>();
        }

        foreach (var t in _sevenSigninTemplateRule)
        {
            _sevenSigninTemplateAwards.Add(new SevenSigninTemplateAwardVo(t));
        }

        if (_userSevenDaySigninInfo==null)
        {
            foreach (var t in _sevenSigninTemplateAwards)
            {
                if (t.DayId==1)
                {
                    t.IsShowGetBtn = true;
                    t.IsShowGetMask = false;
                    break;
                } 
            }
            return;
        }
        
        long curTime = ClientTimer.Instance.GetCurrentTimeStamp();     
        var signDay = _userSevenDaySigninInfo.SignDay;            //签到第几天 
        var refreshTime = _userSevenDaySigninInfo.RefreshTime;    //用户刷新时间
        
        if (signDay< _sevenSigninTemplateAwards.Count)  
        {
            if (signDay==0)
            {
                _sevenSigninTemplateAwards[signDay].IsShowGetBtn = true;
                _sevenSigninTemplateAwards[signDay].IsShowGetMask = false;
            }
            else
            {
                if (curTime>= refreshTime)
                {
                    for (int i = 0; i < signDay; i++)
                    {
                        _sevenSigninTemplateAwards[i].IsShowGetBtn = false;
                        _sevenSigninTemplateAwards[i].IsShowGetMask = true;
                    }

                    _sevenSigninTemplateAwards[signDay].IsShowGetBtn = true;
                    _sevenSigninTemplateAwards[signDay].IsShowGetMask = false;
                }
                else
                {
                    for (int i = 0; i < signDay; i++)
                    {
                        _sevenSigninTemplateAwards[i].IsShowGetBtn = false;
                        _sevenSigninTemplateAwards[i].IsShowGetMask = true;
                    }
                }
            }         
        }
        else   //七天签满了
        {
            for (int i = 0; i < signDay; i++)
            {
                _sevenSigninTemplateAwards[i].IsShowGetBtn = false;
                _sevenSigninTemplateAwards[i].IsShowGetMask = true;
            }
        }  
        
    }
    
    
    /// <summary>
    /// 更新七日签到模板奖励信息
    /// </summary>
    /// <param name="dayId">天</param>
    public void UpdateSevenDayAwardList(int dayId)    //更新七天签到List
    {
        foreach (var t in _sevenSigninTemplateAwards)
        {
            if (dayId == t.DayId)
            {
                t.IsShowGetBtn = false;
                t.IsShowGetMask = true;
            }
        }
    }

    
    
    /// <summary>
    /// 更新七日签到用户信息
    /// </summary>
    /// <param name="info">用户信息</param>
    public void UpdateUserSevenDaySigninTemplateInfo(UserSevenDaySigninInfoPB info)
    {
        _userSevenDaySigninInfo = info;

        var userInfoList = GlobalData.ActivityModel.UserSevenDaySigninInfos;
      
        if (userInfoList.Count==0)
        {
           userInfoList.Add(info); 
        }
        else
        {
            bool isHave = false;
            for (int i = 0; i < userInfoList.Count; i++)
            {
                if (userInfoList[i].ActivityId==info.ActivityId)
                {
                    userInfoList[i] = info;
                    isHave = true;
                }
            }

            if (!isHave)
            {
                userInfoList.Add(info); 
            }
        }

        GlobalData.ActivityModel.UserSevenDaySigninInfos = userInfoList;
    }
    
    /// <summary>
    /// 获取七日登录奖励信息
    /// </summary>
    /// <returns></returns>
    public List<SevenSigninTemplateAwardVo> GetSevenDaysLoginAwardList()
    {
        return _sevenSigninTemplateAwards;
    }
    
    
    
}
