using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core;
using Com.Proto;
using DataModel;
using game.main;
using Google.Protobuf.Collections;
using Newtonsoft.Json;

public class ActivityCapsuleTemplateModel : Model
{
    public int CurActivityId;
    public long EndTimeStamp; // 活动结束时间戳

    private List<string> _storyIds = new List<string>();
    private Dictionary<string, ActivityCapsuleStoryRule> _storyRules = new Dictionary<string, ActivityCapsuleStoryRule>();
    private List<string> _readStoryIds = new List<string>();

    private List<int> _capsuleItemIds = new List<int>();
    private Dictionary<int, ActivityCapsuleItemPB> _capsuleItems = new Dictionary<int, ActivityCapsuleItemPB>();
    private List<int> _gainCapsuleItems = new List<int>();
    

    private List<ConsumeItemPB> _costItem = new List<ConsumeItemPB>();
    
  

    public List<string> storyIds
    {
        get
        {
            return _storyIds;
        }
    }
    public List<int> capsuleItemIds
    {
        get
        {
            return _capsuleItemIds;
        }
    }
    public List<int> gainCapsuleItems
    {
        get
        {
            return _gainCapsuleItems;
        }
    }
    public DropItemPB costItem {
        get
        {
            if (_costItem == null || _costItem.Count == 0) return null;
            if (_gainCapsuleItems == null) return _costItem[0].ConsumeItemPB_;

            int drawTime = _gainCapsuleItems.Count + 1;
            //if (drawCount >= _costItem.Count) drawCount = _costItem.Count - 1;
            //return _costItem[drawCount].ConsumeItemPB_;

            //UnityEngine.Debug.LogWarning("drawTime:" + drawTime);
            for (int i = 0;i < _costItem.Count; ++i)
            {
                if (i == _costItem.Count - 1) return _costItem[i].ConsumeItemPB_;
                if(drawTime >= _costItem[i].DrawCount && drawTime < _costItem[i + 1].DrawCount)
                {
                    return _costItem[i].ConsumeItemPB_;
                }
            }
            return _costItem[_costItem.Count - 1].ConsumeItemPB_;
        }
    }


    public ActivityVo CurActivity;


    private void InitCurActivityInfo()
    {
        CurActivityId = GlobalData.ActivityModel.ActivityCapsuleTemplateId();
        EndTimeStamp = GlobalData.ActivityModel.GetCurActivityTemplate(ActivityTypePB.ActivityCapsuleTemplate)[0].EndTime;
        CurActivity = GlobalData.ActivityModel.GetCurActivityTemplate(ActivityTypePB.ActivityCapsuleTemplate)[0];
    }
    
    public void InitRule(ActivityCapsuleRules res)
    {
        InitCurActivityInfo();
        _storyIds.Clear();
        _storyRules.Clear();
        int index = 0;

        ActivityRuleListRes ruleListRes = GlobalData.ActivityModel.BaseTemplateActivityRule;
        for (int i = 0;i < ruleListRes.ActivityPlotRules.Count; ++i)
        {
            if (ruleListRes.ActivityPlotRules[i].ActivityId != CurActivityId) continue;
            if (!_storyIds.Contains(ruleListRes.ActivityPlotRules[i].PlotId))
            {
                _storyIds.Add(ruleListRes.ActivityPlotRules[i].PlotId);
                index++;
                _storyRules.Add(ruleListRes.ActivityPlotRules[i].PlotId, new ActivityCapsuleStoryRule(ruleListRes.ActivityPlotRules[i], index));
            }
        }

        _capsuleItemIds.Clear();
        _capsuleItems.Clear();
        _costItem.Clear();
        for (int i = 0; i < res.ActivityCapsuleRulePB.Count; ++i)
        {
            if (res.ActivityCapsuleRulePB[i].ActivityId == CurActivityId)
            {
                for(int j = 0; j < res.ActivityCapsuleRulePB[i].ConsumeItemsPB.Count; ++j)
                {
                    _costItem.Add(res.ActivityCapsuleRulePB[i].ConsumeItemsPB[j]);
                    //UnityEngine.Debug.LogWarning("cost:"+ res.ActivityCapsuleRulePB[i].ConsumeItemsPB[j].DrawCount + " Num:"+ res.ActivityCapsuleRulePB[i].ConsumeItemsPB[j].ConsumeItemPB_.Num);
                }
                for(int j = 0; j < res.ActivityCapsuleRulePB[i].ActivityCapsuleItemPB.Count; ++j)
                {
                    if (!_capsuleItemIds.Contains(res.ActivityCapsuleRulePB[i].ActivityCapsuleItemPB[j].Id))
                    {
                        _capsuleItemIds.Add(res.ActivityCapsuleRulePB[i].ActivityCapsuleItemPB[j].Id);
                        _capsuleItems.Add(res.ActivityCapsuleRulePB[i].ActivityCapsuleItemPB[j].Id, res.ActivityCapsuleRulePB[i].ActivityCapsuleItemPB[j]);
                        //UnityEngine.Debug.Log("capsuleItem:"+ res.ActivityCapsuleRulePB[i].ActivityCapsuleItemPB[j].Id);
                    }
                }
                break;
            }
        }
        _costItem.Sort((a, b)=>
        {
            return a.DrawCount.CompareTo(b.DrawCount);
        });
        
    }

    public void SetReadStoryIds(List<string> list)
    {
        if(list == null)
        {
            _readStoryIds.Clear();
            return;
        }
        _readStoryIds = list;
        //for (int i = 0; i < list.Count; ++i)
        //{
        //    UnityEngine.Debug.LogWarning("SetReadStoryIds:" + list[i] + " i:"+i);
        //}
    }

    public ActivityCapsuleStoryRule GetStoryRule(string storyId)
    {
        if (_storyRules.ContainsKey(storyId))
            return _storyRules[storyId];
        return null;
    }

    public bool IsReadStory(string storyId)
    {
        return _readStoryIds.Contains(storyId);
    }

    public bool HaveCanReadStory()
    {
        bool state = false;
        long curTime = ClientTimer.Instance.GetCurrentTimeStamp();
        bool lastIsClear = true;
        for (int i = 0; i < storyIds.Count; ++i)
        {
            ActivityCapsuleStoryRule rule = GetStoryRule(storyIds[i]);
            if (lastIsClear)
            {
                if (curTime >= rule.data.OpenTime)
                {
                    if (!IsReadStory(storyIds[i]))
                    {
                        state = true;
                        break;
                    }
                }
            }
            lastIsClear = IsReadStory(storyIds[i]);
            
        }
        //UnityEngine.Debug.LogWarning("HaveCanReadStory:"+state);
        return state;
    }

    public ActivityCapsuleItemPB GetCapsuleItem(int id)
    {
        if (_capsuleItems.ContainsKey(id))
            return _capsuleItems[id];
        return null;
    }

    public void SetGainCapsuleItemIds(List<int> list)
    {
        if(list == null)
        {
            _gainCapsuleItems.Clear();
            return;
        }
        _gainCapsuleItems = list;
        //for(int i = 0;i < list.Count; ++i)
        //{
        //    UnityEngine.Debug.LogWarning("SetGainCapsuleItemIds:"+list[i]);
        //}
    }

    public bool IsGainCapsuleItem(int id)
    {
        return _gainCapsuleItems.Contains(id);
    }

    public bool GainAllCapsuleItem()
    {
        return _gainCapsuleItems.Count == _capsuleItemIds.Count;
    }
    


    public override void OnMessage(Message message)
    {

    }

    public override void Destroy()
    {

    }
}

public class ActivityCapsuleStoryRule{
    public int index;
    public ActivityPlotRulePB data;

    public ActivityCapsuleStoryRule(ActivityPlotRulePB data, int index = 0)
    {
        this.data = data;
        this.index = index;
    }
}
