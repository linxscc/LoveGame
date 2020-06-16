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
using Assets.Scripts.Framework.GalaSports.Core;

/// <summary>
/// 十连Model
/// </summary>
public class ActivityDrawCardModel: Model
{
    List<ActivityMissionRulePB> _activityMissionRules;
    List<UserActivityMissionPB> _userActivityMissionPBs;
    public int activitId ;
    public long StartTime()
    {
        bool isNew = IsNewDrawCardTenContinuousActivity();
        var createTime = GlobalData.PlayerModel.PlayerVo.CreateTime;
        var pv = GlobalData.ActivityModel.GetActivityVo(ActivityType.ActivityTenDrawCard);

        Debug.LogError(GlobalData.ActivityModel.AllActivityInfo.UserActivityExtraInfo);


        if (isNew)
        { return createTime; }
        else
        {
            long startTime = 0; GlobalData.ActivityModel.AllActivityInfo.UserActivityExtraInfo.ActivityIsOpenOrTime.TryGetValue(pv.ActivityId, out startTime);
            return startTime;
            //return GlobalData.ConfigModel.GetGameTimeConfigByKey(GameConfigKey.OLD_ACTIVITY_DRAW_TEN_CONTINUOUS_START_TIME);
        }
    }


    public long EndTime()
    {
        bool isNew = IsNewDrawCardTenContinuousActivity();
        var pv = GlobalData.ActivityModel.GetActivityVo(ActivityType.ActivityTenDrawCard);

        var createTime = GlobalData.PlayerModel.PlayerVo.CreateTime;

        if (isNew)
        {
            return createTime + pv.OverdueTime * 60 * 60 * 1000; 
        }
        else
        {
            long startTime = 0;               GlobalData.ActivityModel.AllActivityInfo.UserActivityExtraInfo.ActivityIsOpenOrTime.TryGetValue(pv.ActivityId,out startTime);
            return startTime+
                pv.OverdueTime * 60 * 60 * 1000; 
            //return GlobalData.ConfigModel.GetGameTimeConfigByKey(GameConfigKey.OLD_ACTIVITY_DRAW_TEN_CONTINUOUS_END_TIME);
        }
    }
    public long LeftTime()//剩余时间
    {
      var cur=  ClientTimer.Instance.GetCurrentTimeStamp();
        var offset = EndTime() - cur;
        return offset < 0?0: offset;
    }


    public long CurDrawTime
    {
        get
        {
            long t = 0;
            foreach(var v in _userActivityMissionPBs)
            {
                if (t < v.Progress)
                    t = v.Progress;
            }

            return t;
        }
    }

    /// <summary>
    /// createTime>NEW_ACTIVITY_DRAW_TEN_CONTINUOUS_TIME为新玩家
    /// </summary>
    /// <returns></returns>
    public bool IsNewDrawCardTenContinuousActivity()
    {
        var timePoint = GlobalData.ConfigModel.GetGameTimeConfigByKey(GameConfigKey.NEW_ACTIVITY_DRAW_TEN_CONTINUOUS_TIME);
        var createTime = GlobalData.PlayerModel.PlayerVo.CreateTime;
        return createTime >= timePoint;
    }

    public void Init(ActivityRuleListRes list, ActivityListRes allActivityInfo)
    {
        _activityMissionRules = new List<ActivityMissionRulePB>();

        

        foreach (var v in list.ActivityMissionRules)
        {
            if(v.ActivityId == activitId)
            {
                
                _activityMissionRules.Add(v);
            }
        }
        _userActivityMissionPBs = new List<UserActivityMissionPB>();
        foreach (var v in allActivityInfo.UserActivityMissions)
        {
            if (v.ActivityId == activitId)
            {
                _userActivityMissionPBs.Add(v);
            }
        }
    }

    public List<ActivityDrawCardVo> GetActivityVo()
    {
        List<ActivityDrawCardVo> vos = new List<ActivityDrawCardVo>();

        foreach(var v  in _activityMissionRules)
        {
         
            UserActivityMissionPB userPb = _userActivityMissionPBs.Find((m) =>
            {
                return m.ActivityMissionId == v.ActivityMissionId;
            });
            ActivityDrawCardVo vo = new ActivityDrawCardVo(v, userPb);
            vo.activity_mission_id = v.ActivityMissionId;
            vos.Add(vo);
        }
        return vos;
    }
}
