
using System.Collections.Generic;
using Com.Proto;
using DataModel;
using System;
using System.Linq;

public class ActivityMissionVo
{

    public int ActivityMissionId;     //活动任务序号
    public string ActivityMissionName;//活动任务名称
    public int ActivityId;            //活动Id
    public List<RewardVo> Rewards;    //奖励
    public string ActivityMissionDesc;//活动任务描述
    public string JumpTo;             //活动跳转字段
    public ExtraValuePB Extra;        //额外参数
    public MissionStatusPB Status;     //完成状态
    public long Progress;               //任务进度数值
    public long Finish;                 //任务完成数值
    public int MissionPro;
    public ActivityMissionVo(ActivityMissionRulePB rule,UserActivityMissionPB userInfo)
    {
        ActivityMissionId = rule.ActivityMissionId;
        ActivityMissionName = rule.ActivityMissionName;
        ActivityId = rule.ActivityId;
        InitReward(rule.Award.ToList());
        ActivityMissionDesc = rule.ActivityMissionDesc;
        JumpTo = rule.JumpTo;
        Extra = rule.Extra;
        Status = userInfo.Status;
        Progress = userInfo.Progress;
        Finish = userInfo.Finish;
        
        UpdateMissionPro(Status);
    }


    private void InitReward(List<AwardPB> list)
    {
        Rewards = new List<RewardVo>();
        foreach (var t in list)
        {
            var vo = new RewardVo(t);
            Rewards.Add(vo);
        }
    }
    
    public void UpdateMissionPro(MissionStatusPB Status)
    {
        switch (Status)
        {
            case MissionStatusPB.StatusUnclaimed:
                MissionPro = 0;
                break;
            case MissionStatusPB.StatusUnfinished:
                MissionPro = 1;
                break;
            case MissionStatusPB.StatusBeRewardedWith:
                MissionPro = 2;
                break;
        }
			
    }
    
  
}
