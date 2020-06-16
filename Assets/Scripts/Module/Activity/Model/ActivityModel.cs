using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Core.Events;
using Assets.Scripts.Module.Framework.Utils;
using Com.Proto;
using Common;
using DataModel;
using game.main;
using Google.Protobuf.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module;
using Assets.Scripts.Module.NetWork;
using UnityEngine;


public enum EveryDaySignState
{
    /// <summary>
    /// 补签
    /// </summary>
    Retroactive,
    /// <summary>
    /// 已领取
    /// </summary>
    AlreadyGet,
    /// <summary>
    /// 未领取(还没到时间)
    /// </summary>
    NotGet,
    /// <summary>
    /// 当前可领取
    /// </summary>
    CurMayGet
}


/// <summary>
///  活动Type（前端自己用的）
/// </summary>
public enum ActivityType
{
    Default,
    
    /// <summary>
    /// 七日签到（跟用户走的）
    /// </summary>
    ActivitySevenDaySignin,
    
    /// <summary>
    /// 每日体力
    /// </summary>
    ActivityEveryDayPower,
      
    /// <summary>
    ///月签
    /// </summary>
    ActivityMonthSignin,
        
    /// <summary>
    /// 应援储备金
    /// </summary>
    ActivityGrowthFund, 
    
    /// <summary>
    /// 月卡
    /// </summary>
    ActivityMonthCard,
       
    /// <summary>
    /// 每日礼包
    /// </summary>
    ActivityDailyGift,
    
    /// <summary>
    /// 月卡充值
    /// </summary>
    ActivityMonthCardRecharge,
               
    /// <summary>
    /// 活动抽奖模板（月华如恋）
    /// </summary>
    ActivityDrawTemplateYueHuaRuLian,
        
    /// <summary>
    /// 累充福利
    /// </summary>
    ActivityAccumulativeRecharge,
    
    /// <summary>
    /// 活动抽奖模板（恋爱假期）
    /// </summary>
    ActivityDrawTemplateLoveHoliday,
       
    /// <summary>
    /// 活动抽奖模板（甜蜜周末）
    /// </summary>
    ActivityDrawTemplateSweetWeekend,
    
    /// <summary>
    /// 活动七日签到模板（双十一签到）（跟服务器时间走）
    /// </summary>
    ActivitySevenDaySigninTemplate,
       
    /// <summary>
    /// 抽卡十连活动
    /// </summary>
    ActivityTenDrawCard,
    
    /// <summary>
    /// 扭蛋战斗活动（言季生日）
    /// </summary>
    ActivityCapsuleTemplate,
    
    /// <summary>
    /// 音游模板
    /// </summary>
    ActivityMusicGameTemplate,
    
    
    /// <summary>
    /// 唐生日音游
    /// </summary>
    ActivityTangBirthdayMusic,
}

public class ActivityModel : Model
{

    public string ActivitySoleId;
    
    private List<ActivityPB> _activityList = new List<ActivityPB>();
    private List<ActivityVo> _activityVoList =new List<ActivityVo>();
    public List<UserSevenDaySigninInfoPB> UserSevenDaySigninInfos; 
    private List<int> _userPowerGottenIds = new List<int>();
    
    private ActivityRuleRes _activityRule;
    private ActivityRuleListRes _activityTemplateRule;//新的一套任务规则逻辑  10/16日加入
    private ActivityRes _activityRes;  
    private ActivityRedDotModel _redDotModel;

    private List<int> _redList =new List<int>();  //活动走后端后端
    /// <summary>
    /// 活动规则
    /// </summary>
    public ActivityRuleRes BaseActivityRule => _activityRule;
    
    /// <summary>
    /// 所有活动用户信息
    /// </summary>
    public ActivityRes AllActivityInfo => _activityRes;
    
    /// <summary>
    /// 活动，任务，兑换商店，剧情，副本规则
    /// </summary>
    public ActivityRuleListRes BaseTemplateActivityRule => _activityTemplateRule;

   private Dictionary<ActivityType,ActivityListRes> _activityListDic =new Dictionary<ActivityType, ActivityListRes>();
   
    public void UpDataActivityMission(ActivityType type, UserActivityMissionPB pb)
    {
        var value = _activityListDic[type].UserActivityMissions;
        foreach (var t in value)
        {
            if (t.ActivityId==pb.ActivityId&&t.ActivityMissionId==pb.ActivityMissionId)
            {
                t.Status = pb.Status;
                t.Progress = pb.Progress;
                t.Finish = pb.Finish;
            }
        }
    }
    
    /// <summary>
    /// 更新关卡
    /// </summary>
    /// <param name="type"></param>
    /// <param name="pb"></param>
    public void UpdateActivityLevel(ActivityType type, UserActivityLevelInfoPB pb)
    {
        bool isData = false;
        var value = _activityListDic[type];
        foreach (var t in value.UserActivityLevelInfos)
        {
            if (t.ActivityId==pb.ActivityId&&t.LevelId==pb.LevelId)
            {
                t.Star= pb.Star;
                t.Count = pb.Count;
                t.BuyCount = pb.BuyCount;
                t.MaxScore = pb.MaxScore;
                t.ResetTime = pb.ResetTime;
                isData = true;
                break;
            }  
        }
        
        if (!isData)
        {
            value.UserActivityLevelInfos.Add(pb);  
        }                
    }

    /// <summary>
    /// 更新兑换商店
    /// </summary>
    /// <param name="type"></param>
    /// <param name="pb"></param>
    public void UpdateActivityExchangeShop(ActivityType type, UserBuyActivityMallPB pb)
    {
        bool isData = false;
        var value = _activityListDic[type];
        foreach (var t in value.UserBuyActivityMalls)
        {
            if (t.ActivityId==pb.ActivityId&&t.MallId==pb.MallId)
            {
                t.BuyNum = pb.BuyNum;
                isData = true;
                break;
            }  
        }
        
        if (!isData)
        {
            value.UserBuyActivityMalls.Add(pb);  
        }  
    }


    /// <summary>
    /// 更新剧情
    /// </summary>
    /// <param name="type"></param>
    /// <param name="pb"></param>
    public void UpdateActivityStory(ActivityType type,UserActivityPlotInfoPB pb)
    {
        var value = _activityListDic[type].UserActivityPlotInfos;
        foreach (var t in value)
        {
            if (t.ActivityId==pb.ActivityId)
            {
                var plotIds = pb.PlotIds;
                foreach (var v in plotIds)
                {
                    if (!t.PlotIds.Contains(v))
                    {
                       t.PlotIds.Add(v); 
                    }
                }
            } 
        }
    }
    

    //普通活动规则
    public void GetAllActivityRuleRes(ActivityRuleRes res)  
    {
        _activityRule = res;                
    }

    //模板活动规则
    public void InitAllActivityTemplateRuleRes(ActivityRuleListRes res)
    {
        _activityTemplateRule = res;
    }

    
    public void GetAllActivityRes(ActivityRes res)
    {          
        _activityRes = res;   
        UserSevenDaySigninInfos = res.UserSevenDaySigninInfos.ToList();
         
        InitActivityVoList(res);        
        _userPowerGottenIds.Clear();
            
        InitRedModel();
    }

    public void AddDataToActivityListDic(ActivityType type, ActivityListRes res)
    {
        var isKey = _activityListDic.ContainsKey(type);
        if (!isKey)
        {
            _activityListDic.Add(type,res);
        }
        else
        {
            _activityListDic[type] = res;
        }
    }

    public ActivityListRes GetActivityTemplateListRes(ActivityType type)
    {
        if (!_activityListDic.ContainsKey(type))
        {
            Debug.LogError("当前Key,不存在");
            return null;
        }
        return _activityListDic[type];
    }
  

    
    
 

    private void InitRedModel()
    {
        if(_redDotModel==null)
            _redDotModel =new ActivityRedDotModel();
    }

    public UserActivityMonthCardRechargeInfoPB GetUserActivityMonthCardRechargeInfo()
    {
        foreach (var v in _activityRes.Activitys)
        {
            if (v.Type==ActivityTypePB.ActivityMonthCardRecharge)
            {
                if (_activityRes.UserActivityMonthCardRechargeInfo.Count>0)
                {
                    foreach (var a in _activityRes.UserActivityMonthCardRechargeInfo)
                    {
                        if (a.ActivityId==v.ActivityId)
                        {
                            return a;
                        }
                    } 
                }
            }

        }
        
        return null;

    }


    public int ActivityDrawTemplateId()
    {

        var curDrawTemplate = GetCurActivityTemplate(ActivityTypePB.ActivityDrawTemplate);
        if (curDrawTemplate.Count!=0)
        {
            if (curDrawTemplate.Count==1)
            {
                var curTimeStamp =  ClientTimer.Instance.GetCurrentTimeStamp();
                if (curTimeStamp<curDrawTemplate[0].EndTime)
                {
                    return curDrawTemplate[0].ActivityId;
                }
                
            }
            else
            {
               Debug.LogError("开放了多个抽奖活动模板");
            }
           
        }
        else 
        {
            Debug.LogError("当前没有抽奖活动模板开放");
            return 0;
        }
        Debug.LogError("ActivityDrawTemplateId is 0");
        return 0;
    }

    public int ActivityCapsuleTemplateId()
    {

        foreach (var t in _activityRes.Activitys)
        {
            if (t.Type == ActivityTypePB.ActivityCapsuleTemplate)
            {
                var curTimeStamp = ClientTimer.Instance.GetCurrentTimeStamp();
                if (curTimeStamp < t.EndTime)
                {
                    return t.ActivityId;
                }
            }
        }

        Debug.LogError("ActivityCapsuleTemplateId is 0");
        return 0;
    }


    /// <summary>
    /// 获取长期累充活动的UserData
    /// </summary>
    /// <returns></returns>
    public UserActivityAccumulativeRechargeInfoPB GetLongLastRechargeInfoBb()
    {
        foreach (var v in _activityRes.Activitys)
        {
            if (v.Type==ActivityTypePB.ActivityAccumulativeRecharge&&v.EndType==ActivityEndTypePB.EndLong)
            {
                if (_activityRes.UserActivityAccumulativeRechargeInfo.Count>0)
                {
                    foreach (var a in _activityRes.UserActivityAccumulativeRechargeInfo)
                    {

                        if (a.ActivityId==v.ActivityId)
                        {
                            return a;
                        }
                    }                   
                }
            }
        }
        return null;
    }

    public void UpdateLongLastRechargeInfoBb(UserActivityAccumulativeRechargeInfoPB pb)
    {
        foreach (var v in _activityRes.Activitys)
        {
            if (v.Type==ActivityTypePB.ActivityAccumulativeRecharge&&v.EndType== ActivityEndTypePB.EndLong)
            {
                if (_activityRes.UserActivityAccumulativeRechargeInfo.Count>0)
                {
                    for (int i = 0; i < _activityRes.UserActivityAccumulativeRechargeInfo.Count; i++)
                    {
                        if (_activityRes.UserActivityAccumulativeRechargeInfo[i].ActivityId==v.ActivityId)
                        {
                            _activityRes.UserActivityAccumulativeRechargeInfo[i] = pb;
                        }
                    }
                }
            }
        }
        
        
    }


   
    
    
    public List<ActivityVo> GetActivityVoList()
    {                
        List<ActivityVo> newList = new List<ActivityVo>();
        for (int i = _activityVoList.Count - 1; i >= 0; i--)
        {
            var isPastDue = _activityVoList[i].IsActivityPastDue;
            if (_activityVoList[i].IsDisplay==0)
            {
                if (isPastDue)
                {
                   // Debug.LogError("移除的活动===>"+_activityVoList[i].Name);
                    _activityVoList.RemoveAt(i);
                }
                else
                {
                    newList.Add(_activityVoList[i]);
                }
            }
            else
            {
                if (isPastDue)
                {
                    // Debug.LogError("移除的活动===>"+_activityVoList[i].Name);
                    _activityVoList.RemoveAt(i); 
                } 
            } 
        }
        
       
        ActivityVoListSort(newList);    
        return newList;
    }
      
    public bool IsShowFirstRechargeBtn()
    {              
        var firstPrize = GlobalData.PlayerModel.PlayerVo.ExtInfo.FirstPrize;
        return firstPrize != FirstPrizeStatusPB.FpReceived;
    }


    /// <summary>
    /// 是否显示主机面活动模板入口
    /// </summary>
    /// <param name="typePb">Type是模板类型的</param>
    /// <returns></returns>
    public bool IsShowActivityTemplateBtn(ActivityTypePB typePb)
    {
        bool isShow = false;
        var list = GetCurActivityTemplate(typePb);
        if (list.Count!=0)
        {
            var isOneDrawActivity = list.Count == 1;
            if (isOneDrawActivity)
            {
                var curActivity = list[0];
                isShow = !curActivity.IsActivityPastDue;              
            }
            else
            {
                //同一个抽奖模板出多个活动  //需求出了在具体改
            }  
        }

        return isShow;
    }

    /// <summary>
    /// 获取当前活动模板对应的具体活动
    /// </summary>
    /// <param name="typePb"></param>
    /// <returns></returns>
    public List<ActivityVo> GetCurActivityTemplate(ActivityTypePB typePb)
    {
        List<ActivityVo> curTemplateActivity = new List<ActivityVo>();
        foreach (var t in _activityVoList)
        {
            if (t.BaseActivityType == typePb)
            {
               curTemplateActivity.Add(t); 
            } 
        }

        return curTemplateActivity;
    }
    

 

    /// <summary>
    /// 是否显示首充红点
    /// </summary>
    /// <returns></returns>
    public bool IsShowFirstRechargeRedDot()
    {       
        bool isShow = false;     
        var firstPrize = GlobalData.PlayerModel.PlayerVo.ExtInfo.FirstPrize;
        if (firstPrize == FirstPrizeStatusPB.FpUnaccalimed  )
        {
            isShow = true;
        }
        return isShow;

    }

    /// <summary>
    /// 抽将活动模块掉落物品规则
    /// </summary>
    /// <returns></returns>
    public RepeatedField<ActivityHolidayAwardRulePB> GetActivityHolidayAwardRules(int activityId)
    {
        RepeatedField<ActivityHolidayAwardRulePB> pbs = new RepeatedField<ActivityHolidayAwardRulePB>();
        foreach (var t in _activityRule.ActivityHolidayAwardRule)
        {
            if (t.ActivityId== activityId)
            {
                pbs.Add(t); 
            }
        }
        return pbs;
    }    
    /// <summary>
    /// 音友
    /// </summary>
    /// <returns></returns>
    public List<ActivityMusicPoolPB> GetActivityMusicPoolRules(int activityId)
    {
        List<ActivityMusicPoolPB> pbs = new List<ActivityMusicPoolPB>();
        foreach (var t in _activityRule.ActivityMusicPools)
        {
            if (t.ActivityId == activityId)
            {
                pbs.Add(t);
            }
        }
        return pbs;
    }


    /// <summary>
    /// 获取抽奖模块掉落上限
    /// </summary>
    /// <param name="activityId">活动Id</param>
    /// <param name="modulePb">对应模块</param>
    /// <returns></returns>
    public int Limit(int activityId, HolidayModulePB modulePb)
    {       
       var consumeItemId =  GetActivityDrawRules(activityId)[0].ConsumeType.ResourceId;
       var rules=  GetActivityHolidayAwardRules(activityId);
       foreach (var t in rules)
       {
           if (t.HolidayModule==modulePb)
           {
               var list = t.DropItems;
               foreach (var d in list)
               {
                   if (d.DropItems.ResourceId == consumeItemId)
                   {
                       return d.Limit;
                   }
               }              
           }  
       }

       Debug.LogError("Limit is 0");
       return 0;
    }
    
    /// <summary>
    ///抽奖活动模板规则
    /// </summary>
    /// <returns></returns>
    public RepeatedField<ActivityDrawRulePB> GetActivityDrawRules(int activityId )
    {
        RepeatedField<ActivityDrawRulePB> pbs = new RepeatedField<ActivityDrawRulePB>();
        foreach (var t in _activityRule.ActivityDrawRules)
        {
            if (t.ActivityId == activityId)
            {
                pbs.Add(t);
            }
        }

        return pbs;  
        
    }

 


       
    /// <summary>
    /// 获取用户体力领取信息
    /// </summary>
    /// <returns></returns>
    public List<int> GetUserPowerGottenIds()
    {       
        if (_userPowerGottenIds.Count == 0 && _activityRes.PowerGottenIds.Count > 0)
        {
            foreach (var t in _activityRes.PowerGottenIds)
            {
                _userPowerGottenIds.Add( t);
            }
        }
        return _userPowerGottenIds;
    }

    /// <summary>
    /// 更新用户每日体力签到详情
    /// </summary>
    /// <param name="list"></param>
    public void UpdataUserPowerGottenIds(RepeatedField<int> list)
    {
       
        _userPowerGottenIds.Clear();
        for (int i = 0; i < list.Count; i++)
        {         
           _userPowerGottenIds.Add(list[i]);                   
        }
        _userPowerGottenIds.Sort();
    }


    private void InitActivityVoList(ActivityRes res)
    {
        _activityVoList = new List<ActivityVo>();
        foreach (var t in res.Activitys)
        {
            ActivityVo vo =new ActivityVo(t);
            if (vo.IsNeedRechargeSwitchController)
            {
                var isAdd = AppConfig.Instance.SwitchControl.Recharge;
                if (isAdd) { _activityVoList.Add(vo);}                                              
            }
            else
            {
                _activityVoList.Add(vo);  
            }
        }    
        ActivityVoListSort(_activityVoList);
    }
    
   private void ActivityVoListSort(List<ActivityVo> list)
   {
       list.Sort((x,y)=>x.Sort.CompareTo(y.Sort));
   }

    public ActivityVo GetActivityVo(ActivityType type)
    {
        foreach (var t in _activityVoList)
        {
            if (t.ActivityType == type && !t.IsActivityPastDue)
            {
                return t;
            }
        }
        return null;
    }

    public void SetActivityMissionRedDot(string key,int extI)
    {
        if (key == Constants.ACTIVITY_MISSION && !_redList.Contains(extI))
        {
            _redList.Add(extI);
        }             
    }


    public bool IsGetActivityMissionRedDot(int activityId)
    {
      return _redList.Contains(activityId);
    }

    public void RemoveActivityMissionRedDot(int activityId)
    {
        _redList.Remove(activityId);
    }
    

    /// <summary>
    /// 活动是否显示红点
    /// </summary>
    /// <returns>提供给主界面</returns>
    public bool IsShowRedDot()
    {      
      if (_activityRes==null)
      {
          return false;
      }
      else
      {         
          return _redDotModel.IsShowMainPanelRedDot();
      }   
          
    }

    /// <summary>
    /// 获取指定活动红点
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public bool GetCurActivityRedDot(ActivityType type)
    {
        return _redDotModel.GetCurActivityRedDot(type);
    }

    
  

    /// <summary>
    /// 获取跟用户时间走的七日签到
    /// </summary>
    /// <returns></returns>
    public UserSevenDaySigninInfoPB GetUserSevenDaySigninInfo()
    {
        UserSevenDaySigninInfoPB userInfo = null;
        var activityId = GetActivityVo(ActivityType.ActivitySevenDaySignin).ActivityId;
        foreach (var t in UserSevenDaySigninInfos)
        {
            if (t.ActivityId==activityId)
            {
                userInfo = t;
                break;
            }
        }

        return userInfo;
    }

    /// <summary>
    /// 获取跟服务器时间走的七日签到用户信息
    /// </summary>
    /// <returns></returns>
    public UserSevenDaySigninInfoPB GetUserSevenDaySigninTemplateInfo()
    {
        UserSevenDaySigninInfoPB userInfo = null;
        var activityId = GetActivityVo(ActivityType.ActivitySevenDaySigninTemplate).ActivityId;
        foreach (var t in UserSevenDaySigninInfos)
        {
            if (t.ActivityId==activityId)
            {
                userInfo = t;
                break;
            }
        }

        return userInfo;
    }
    




    
    
}
