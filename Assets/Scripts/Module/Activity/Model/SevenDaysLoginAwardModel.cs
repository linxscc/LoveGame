using System;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Module.Framework.Utils;
using Com.Proto;
using DataModel;
using game.main;
using Google.Protobuf.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//七天登录礼Model
public class SevenDaysLoginAwardModel : Model
{   
   
    private List<SevenDaysLoginAwardVO> _sevenDayAwardList;       //七天奖励集合     
    private List<ActivityOptionalAwardRulePB> _sevenDaySigninRule;  //七日签到规则
    private UserSevenDaySigninInfoPB _userSevenDaySigninInfo;  //七日签到用户信息
    

    public SevenDaysLoginAwardModel()
    {
        InitRule();
        InitUserInfo();
        InitSevenDayAwardList();     
    }

    /// <summary>
    /// 初始化规则
    /// </summary>
    private void InitRule()
    {
        var baseRule = GlobalData.ActivityModel.BaseActivityRule;        
        _sevenDaySigninRule =new List<ActivityOptionalAwardRulePB>();
        foreach (var t in baseRule.ActivityOptionalAwardRules)
        {
            if (t.OptionalActivityType == OptionalActivityTypePB.OptionalSevenDaySig 
                && t.ActivityId == GlobalData.ActivityModel.GetActivityVo(ActivityType.ActivitySevenDaySignin)?.ActivityId)
            {
                 _sevenDaySigninRule.Add(t);
            }
        }
    }

    /// <summary>
    /// 初始化用户信息
    /// </summary>
    private void InitUserInfo()
    {

        _userSevenDaySigninInfo = GlobalData.ActivityModel.GetUserSevenDaySigninInfo();
//        var activity = GlobalData.ActivityModel.GetActivity(ActivityTypePB.ActivitySevenDaySig).ActivityId;
//        var userInfoList =  GlobalData.ActivityModel.UserSevenDaySigninInfos;
//        if (userInfoList.Count==0)
//        {
//            _userSevenDaySigninInfo = null;
//        }
//        else
//        {
//            foreach (var t in userInfoList)
//            {
//                _userSevenDaySigninInfo = t.ActivityId == activity ? t : null;
//            }
//        }
      
    }

    /// <summary>
    /// 初始化七天签到奖励List
    /// </summary>
    private void InitSevenDayAwardList()  
    {
        if (_sevenDayAwardList == null)
        {
            _sevenDayAwardList = new List<SevenDaysLoginAwardVO>();            
        } 
        
        foreach (var t in _sevenDaySigninRule)
        {             
            _sevenDayAwardList.Add(new SevenDaysLoginAwardVO(t));
        }

        if (_userSevenDaySigninInfo==null)
        {
            foreach (var t in _sevenDayAwardList)
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
             
        if (signDay< _sevenDayAwardList.Count)  
        {
            if (signDay==0)
            {
                _sevenDayAwardList[signDay].IsShowGetBtn = true;
                _sevenDayAwardList[signDay].IsShowGetMask = false;
            }
            else
            {
                if (curTime>= refreshTime)
                {
                    for (int i = 0; i < signDay; i++)
                    {
                        _sevenDayAwardList[i].IsShowGetBtn = false;
                        _sevenDayAwardList[i].IsShowGetMask = true;
                    }

                    _sevenDayAwardList[signDay].IsShowGetBtn = true;
                    _sevenDayAwardList[signDay].IsShowGetMask = false;
                }
                else
                {
                    for (int i = 0; i < signDay; i++)
                    {
                        _sevenDayAwardList[i].IsShowGetBtn = false;
                        _sevenDayAwardList[i].IsShowGetMask = true;
                    }
                }
            }         
        }
        else   //七天签满了
        {
            for (int i = 0; i < signDay; i++)
            {
                _sevenDayAwardList[i].IsShowGetBtn = false;
                _sevenDayAwardList[i].IsShowGetMask = true;
            }
        }       
    }

     
    /// <summary>
    /// 更新七日签到奖励信息
    /// </summary>
    /// <param name="dayId">天</param>
    public void UpdateSevenDayAwardList(int dayId)    //更新七天签到List
    {
        foreach (var t in _sevenDayAwardList)
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
    public void UpdateUserSevenDaySigninInfo(UserSevenDaySigninInfoPB info)
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
    public List<SevenDaysLoginAwardVO> GetSevenDaysLoginAwardList()
    {
        return _sevenDayAwardList;
    }


    /// <summary>
    /// 获取七日签到剩余天数
    /// </summary>
    /// <returns>返回剩余天数</returns>
    public int GetSevenDaysActivityResidueDay()
    {       
        var startTime = _userSevenDaySigninInfo.StartSignInTime;
        var sevenDaysActivity = GlobalData.ActivityModel.GetActivityVo(ActivityType.ActivitySevenDaySignin);
        var endTime = startTime + sevenDaysActivity.OverdueTime * 60 * 60 * 1000;             
        long curTimeStamp = ClientTimer.Instance.GetCurrentTimeStamp();       
        var difference = endTime - curTimeStamp;              
        var day =Convert.ToInt32(Math.Ceiling(difference / 86400000f))  ;           
        if (day == 0)
        {
            day = 1;
        }
        return  day;
    }
    
}
