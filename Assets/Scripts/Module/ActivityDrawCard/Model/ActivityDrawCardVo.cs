using Com.Proto;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ActivityDrawCardVo 
{
    public int activity_mission_id;//任务序号

    private ActivityMissionRulePB _rule;
    private UserActivityMissionPB _userPb;

    public int ActivityId
    {
        get
        {
            return _rule.ActivityId;
        }
    }
    public int LimitValue
    {
        get
        {
            return _rule.Extra.LimitValue;
        }
    }

    public ActivityDrawCardVo(ActivityMissionRulePB rule, UserActivityMissionPB userPb)
    {
        this._rule = rule;
        this._userPb = userPb;
    }
    public MissionStatusPB MissionStatusPB
    {
        get
        {
            return _userPb.Status;
        }
    }
    public string MissionName
    {
        get
        {
            return _rule.ActivityMissionName;
        }
    }

    public string MissionDesc
    {
        get
        {
            return _rule.ActivityMissionDesc;
        }
    }


    public List<AwardPB>  Awards
    {
        get
        {
            if(_rule.Award==null||_rule.Award.Count==0)
            {
                return null;
            }
            return _rule.Award.ToList();//默认只有一个
        }
    }

}
