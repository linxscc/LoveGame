using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core;
using Com.Proto;
using DataModel;
using game.main;
using Newtonsoft.Json;
using UnityEngine;

public class ActivityMusicTemplateModel : Model
{
    private List<ActivityPlayRuleVo> _activityPlayRuleVos;
    
    /// <summary>
    /// 是否开放音游任务入口
    /// </summary>
    public bool IsOpenMusicTemplateTaskEntrance;
    
    /// <summary>
    ///是否开放音游剧情入口
    /// </summary>
    public bool IsOpenMusicTemplateStoryEntrance;

    /// <summary>
    /// 是否开放音游兑换商店入口
    /// </summary>
    public bool IsOpenMusicExchangeShopEntrance;

    /// <summary>
    /// 音乐数据
    /// </summary>
    List<ActivityMusicVo> _activityMusicVos;
    public List<ActivityMusicVo> ActivityMusicVos
    {
        get { return _activityMusicVos; }
    }

    public ActivityMusicTemplateModel(ActivityVo curActivity)
    {
        InitActivityPlayRuleData();
        InitEntrance(curActivity);                          
    }
   private Rules _musicRules;
    public void InitActivityMusicInfo(List<ActivityMusicPoolPB> musicPbs, ActivityMusicInfoRes res, Rules rules = null)
    {
        if (rules==null&& _musicRules==rules)
        {
            return;
        }
        else
        {
            _musicRules = rules == null ? _musicRules : rules;
        }
        if(_activityMusicVos == null){
            _activityMusicVos = new List<ActivityMusicVo>();
        }
        else{
            _activityMusicVos.Clear();
        }

        foreach (var v in musicPbs)
        {
            foreach (var d in Enum.GetValues(typeof(MusicGameDiffTypePB)))
            {
                UserActivityMusicInfoPB userInfo = null;

                MusicGameDiffTypePB diff=(MusicGameDiffTypePB)d;
                foreach (var v1 in res.UserActivityMusicInfos)
                {
                    if (v1.ActivityId == v.ActivityId && v1.MusicId == v.MusicId&&
                        v1.DiffType== diff)
                    {
                        userInfo = v1;
                        break;
                    }
                }
                
                MusicGameScorePB musicGameScorePB = null;
                foreach (var s in _musicRules.MusicGameScore)
                {
                    if ( s.MusicId == v.MusicId)
                    {
                        musicGameScorePB = s;
                        break;
                    }
                }
                
                if (musicGameScorePB == null) 
                {
                    continue;
                }
                var vo = new ActivityMusicVo(userInfo);
                vo.ActivityId = v.ActivityId;
                vo.MusicId = v.MusicId;
                vo.GameScoreRule = musicGameScorePB;
                vo.Diff = diff;
                vo.MusicName = v.MusicName;

                if (vo.Diff == MusicGameDiffTypePB.Entry) 
                {
                    vo.IsOpen = true;
                }
                else
                {
                    vo.IsOpen = _activityMusicVos[_activityMusicVos.Count - 1].IsPass == true;
                }
                _activityMusicVos.Add(vo);
            }
        }
    }


    private void InitEntrance(ActivityVo curActivity)
    {     
        IsOpenMusicTemplateTaskEntrance = curActivity.ActivityExtra.MissionSwitch==0;       
        IsOpenMusicTemplateStoryEntrance = curActivity.ActivityExtra.PlotSwitch==0;        
        IsOpenMusicExchangeShopEntrance = true;
    }


    private void InitActivityPlayRuleData()
    {
        string fileName = "RulePopupWindow";
        string text = new AssetLoader().LoadTextSync(AssetLoader.GetLocalConfiguratioData("ActivityPopWindowData",fileName));
       _activityPlayRuleVos =JsonConvert.DeserializeObject<List<ActivityPlayRuleVo>>(text);
    }

    public ActivityPlayRuleVo GetPlayRule()
    {
        string name = "MusicRule";
        ActivityPlayRuleVo vo = null; 
        foreach (var t in _activityPlayRuleVos)
        {
            if (t.Name==name)
            {
                vo= t;
                break;                
            }  
        }

        return vo;
    }
}
