using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Framework.GalaSports.Core;
using Com.Proto;
using DataModel;
using UnityEngine;

public class ActivityStoryModel : Model
{

    private List<ActivityPlotRulePB> _rule;
    private List<ActivityStoryVo> _userStoryInfo;

    public List<ActivityStoryVo> GetUserStoryInfo=>_userStoryInfo;

    private ActivityVo _curActivity;
    public ActivityStoryModel(ActivityVo curActivity)
    {
        _curActivity = curActivity;
        InitRule(curActivity.ActivityId);
        InitUserData(curActivity);
    }

    private void InitRule(int activityId)
    {       
        _rule=new List<ActivityPlotRulePB>();
        
        var baseRule = GlobalData.ActivityModel.BaseTemplateActivityRule.ActivityPlotRules;
        foreach (var t in baseRule)
        {
            if (t.ActivityId==activityId)
            {              
               _rule.Add(t); 
            }
        }                
    }

    
    
    private void InitUserData(ActivityVo curActivity)
    {
       
        _userStoryInfo = new List<ActivityStoryVo>();
        var baseUserInfo = GlobalData.ActivityModel.GetActivityTemplateListRes(curActivity.ActivityType).UserActivityPlotInfos;
                  
        foreach (var t in _rule)
        {
          var vo =new ActivityStoryVo(t);
          _userStoryInfo.Add(vo);
        }

        var isNull = baseUserInfo == null;
        if (!isNull)
        {
            foreach (var t in baseUserInfo)
            {                         
                UpdateUserData(t);               
            }
        }

        SetFirstStory();

        SetCanEnterStory();


      
        
        foreach (var t in _userStoryInfo)
        {
            Debug.LogError("关卡名--->"+t.PlotId+";是否开放--->"+t.IsOpen+";是否通关--->"+t.IsPass+";是否能进入--->"+t.IsCanEnterStory);
        }
    }
    
    
    public void UpdateUserData(UserActivityPlotInfoPB pb)
    {
        foreach (var t in _userStoryInfo)
        {
            t.IsPass=pb.PlotIds.Contains(t.PlotId);           
        }
                    
        GlobalData.ActivityModel.UpdateActivityStory(_curActivity.ActivityType, pb);
    }

    private void SetFirstStory()
    {           
       _userStoryInfo[0].IsOpen = true;                
    }

    public void SetCanEnterStory()
    {
        int index = 0;
        
        for (int i = 0; i < _userStoryInfo.Count; i++)
        {
            if (_userStoryInfo[i].IsPass)
            {
                _userStoryInfo[i].IsCanEnterStory = true;
            }
            else
            {
                if (_userStoryInfo[i].IsOpen)
                {
                    index = i;
                    break;
                }
            }
        }

        _userStoryInfo[index].IsCanEnterStory = true;
        
        foreach (var t in _userStoryInfo)
        {
            Debug.LogError("关卡名--->"+t.PlotId+";是否开放--->"+t.IsOpen+";是否通关--->"+t.IsPass+";是否能进入--->"+t.IsCanEnterStory);
        }
    }
    
    /// <summary>
    /// 是否显示剧情入口红点
    /// </summary>
    /// <returns></returns>
    public bool IsShowStoryRedDot()
    {
        bool isShow = false;
        foreach (var t in _userStoryInfo)
        {
            if (t.IsOpen&&!t.IsPass)
            {
                isShow = true;
                break;
            }
        }

        return isShow;
    }
  
}
