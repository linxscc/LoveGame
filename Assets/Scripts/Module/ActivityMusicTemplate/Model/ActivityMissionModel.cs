using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core;
using Com.Proto;
using DataModel;
using UnityEngine;

public class ActivityMissionModel : Model
{
    
    private List<ActivityMissionRulePB> _activityMissionRule;//活动任务
    private List<ActivityMissionVo> _activityMissionUserInfo; //活动任务信息

    public List<ActivityMissionVo> MusicUserMissionInfo => _activityMissionUserInfo;
    private ActivityVo _curActivity;
    
    public ActivityMissionModel(ActivityVo curActivity)
    {
        _curActivity = curActivity;
        InitActivityMissionRule(curActivity.ActivityId);
        InitActivityMissionUserInfo(curActivity);
    }
    
    /// <summary>
    /// 初始化任务活动规则
    /// </summary>
    /// <param name="activityId"></param>
    private void InitActivityMissionRule(int activityId)
    {
        _activityMissionRule= new List<ActivityMissionRulePB>();

        var baseRule = GlobalData.ActivityModel.BaseTemplateActivityRule.ActivityMissionRules;  //任务活动规则    
        foreach (var t in baseRule)
        {
            if (t.ActivityId==activityId)
            {
                _activityMissionRule.Add(t); 
            } 
        }       
    }
    
    
    /// <summary>
    /// 初始化对应活动任务用户信息
    /// </summary>
    /// <param name="activityId"></param>
    private void InitActivityMissionUserInfo(ActivityVo curActivityVo)
    {
        _activityMissionUserInfo =new  List<ActivityMissionVo>();
       
        var baseUserInfo = GlobalData.ActivityModel.GetActivityTemplateListRes(curActivityVo.ActivityType).UserActivityMissions;

        if (baseUserInfo==null)
        {
            return;
        }
        foreach (var t in baseUserInfo)
        {
            if (t.ActivityId==curActivityVo.ActivityId)
            {
                var rule = GetActivityMissionRule(t.ActivityMissionId);
                ActivityMissionVo vo =new ActivityMissionVo(rule,t);
                _activityMissionUserInfo.Add(vo);
            }
        }
       
        _activityMissionUserInfo.Sort((x,y)=>x.MissionPro.CompareTo(y.MissionPro));
    }

    /// <summary>
    /// 获取对应活动任务规则
    /// </summary>
    /// <param name="activityMissionId"></param>
    /// <returns></returns>
    private ActivityMissionRulePB GetActivityMissionRule(int activityMissionId)
    {
        ActivityMissionRulePB rule = null;
        foreach (var t in _activityMissionRule)
        {
            if (t.ActivityMissionId==activityMissionId)
            {
                rule = t;
                break;
            }
        }
        return rule;
    }

    public void UpdateActivityMissionData(UserActivityMissionPB pb)
    {
        foreach (var t in _activityMissionUserInfo)
        {
            if (t.ActivityId==pb.ActivityId && t.ActivityMissionId ==pb.ActivityMissionId)
            {
                
                t.Status = pb.Status;
                t. Progress = pb.Progress;
                t. Finish = pb.Finish; 
                t.UpdateMissionPro(pb.Status);
                break;
            } 
        }
        _activityMissionUserInfo.Sort((x,y)=>x.MissionPro.CompareTo(y.MissionPro));
                  
        GlobalData.ActivityModel.UpDataActivityMission(_curActivity.ActivityType, pb);
    }

    /// <summary>
    /// 是否显示任务入口红点
    /// </summary>
    /// <returns></returns>
    public bool IsShowMissionRedDot()
    {
        bool isShow = false;
        foreach (var t in _activityMissionUserInfo)
        {
            if (t.Status == MissionStatusPB.StatusUnclaimed)
            {
                isShow = true;
                break;
            }
        }      
        return isShow;
    }
    
}
