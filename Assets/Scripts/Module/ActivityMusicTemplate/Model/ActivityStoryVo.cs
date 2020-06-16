using System.Collections;
using System.Collections.Generic;
using Com.Proto;
using UnityEngine;

public class ActivityStoryVo
{

    public int ActivityId;    //活动Id
    
    /// <summary>
    /// 场景Id
    /// </summary>
    public string PlotId;       
    public long OpenTime;

    public bool IsOpen=false;
    public bool IsPass=false;
    public bool IsCanEnterStory = false;
    public ActivityPlotRulePB Rule;


   
    public ActivityStoryVo(ActivityPlotRulePB rule)
    {
        Rule = rule;
        ActivityId = rule.ActivityId;
        PlotId = rule.PlotId;
        OpenTime = rule.OpenTime;
        IsOpen = ClientTimer.Instance.GetCurrentTimeStamp() >= OpenTime;
       
    }

    
   
}
