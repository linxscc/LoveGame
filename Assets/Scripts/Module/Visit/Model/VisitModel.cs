using Assets.Scripts.Framework.GalaSports.Core;
using UnityEngine;
using game.main;
using System.Collections.Generic;
using System;
using Com.Proto;
using Google.Protobuf.Collections;
using DataModel;

public enum VISIT_WEATHER
{
    None,//无0
    Fine,//晴1
    Wind,//风2
    Cloud,//云3
    Cloudy,//阴天
    Heat,//暑5
    Rain,//雨6
    Thunder,//雷7
    Snow,//雪8

}

public enum BlessResult
{
    Best,
    Invalid,
    Better,

}


public class VisitModel : Model
{


    List<WeatherPB> _weatherRules;
    List<WeatherBlessPB> _weatherBlessRules;
    List<VisitingLevelRulePB> _levelRules;
    List<LevelFirstPassPB> _levelFirstPassRules;
    List<WeatherBlessCostPB> _weatherBlessCostPBs;
    List<VisitingResetConsumptionPB> _visitingResetConsumptionPBs;//重置关卡消耗规则
    public List<WeatherPB> WeatherRules
    {
        get
        {
            return _weatherRules;
        }
    }
    public List<WeatherBlessPB> WeatherBlessRules
    {
        get
        {
            return _weatherBlessRules;
        }
    }

    private List<VisitVo> _listVo;
    public List<VisitVo> VisitVoList
    {
        get
        {
            return _listVo;
        }
    }

    private List<MyVisitLevelVo> _myVisitLevelVos;

    public void InitRule(VisitingRuleRes res)
    {
        _weatherRules = new List<WeatherPB>();
        _weatherBlessRules = new List<WeatherBlessPB>();
        _levelRules = new List<VisitingLevelRulePB>();
        _levelFirstPassRules = new List<LevelFirstPassPB>();
        _challengeCardNumRule = new List<ChallengeCardNumRulePB>();
        _weatherBlessCostPBs = new List<WeatherBlessCostPB>();
        _visitingResetConsumptionPBs = new List<VisitingResetConsumptionPB>();
        CommentRule = res.CommentRules;
        _infoRule = res.InfoRules;
        _plotRule = res.PlotRules;
        _weatherRules.AddRange(res.WeatherRules);
        _weatherBlessRules.AddRange(res.WeatherBlessRules);
        _levelRules.AddRange(res.LevelRules);
        _levelFirstPassRules.AddRange(res.LevelFirstPassRules);
        _challengeCardNumRule.AddRange(res.CardNumRules);
        _weatherBlessCostPBs.AddRange(res.WeatherBlessCostRules);
        _visitingResetConsumptionPBs.AddRange(res.VisitingResetConsumptionRules);
        SetData(res);
    }

    public List<List<LevelData>> LocalDataList;
    private RepeatedField<VisitingLevelInfoRulePB> _infoRule;
    private RepeatedField<VisitingLevelPlotRulePB> _plotRule;
    private List<ChallengeCardNumRulePB> _challengeCardNumRule;
    public List<ChallengeCardNumRulePB> ChallengeCardNumRule
    {
        get { return _challengeCardNumRule; }
    }

    public RepeatedField<VisitingLevelCommentRulePB> CommentRule
    {
        set; get;
    }

    public void UpdateMyLevel(UserVisitingLevelPB pb)
    {
        MyVisitLevelVo level = _myVisitLevelVos.Find((m) => { return pb.LevelId == m.LevelId; });
        level.BuyCount = pb.BuyCount;
        level.Count = pb.Count;
    }
    public MyVisitLevelVo GetMyLevelByLevelId(int LevelId)
    {
        MyVisitLevelVo level = _myVisitLevelVos.Find((m) => { return LevelId == m.LevelId; });
        return level;
    }
    public Dictionary<PlayerPB, List<VisitChapterVo>> VisitChapterDir
    {
        set; get;
    }


    private void SetData(VisitingRuleRes res)
    {
        VisitChapterDir = new Dictionary<PlayerPB, List<VisitChapterVo>>();
        VisitChapterDir[PlayerPB.TangYiChen] = new List<VisitChapterVo>();
        VisitChapterDir[PlayerPB.QinYuZhe] = new List<VisitChapterVo>();
        VisitChapterDir[PlayerPB.YanJi] = new List<VisitChapterVo>();
        VisitChapterDir[PlayerPB.ChiYu] = new List<VisitChapterVo>();

        //LevelPB pb = null;
        for (int i = 0; i < res.LevelRules.Count; i++)
        {
            VisitLevelVo level = new VisitLevelVo(res.LevelRules[i], _plotRule, _infoRule);
            VisitChapterVo visitChapterVo = VisitChapterDir[level.NpcId].Find((m) => { return m.ChapterId == level.ChapterGroup; });
            if (visitChapterVo == null)
            {
                visitChapterVo = new VisitChapterVo("123", level.ChapterGroup, true);//I provisionally do it;
                VisitChapterDir[level.NpcId].Add(visitChapterVo);
            }
            visitChapterVo.LevelList.Add(level);
            level.VisitChapter = visitChapterVo;

            foreach (var v in res.LevelFirstPassRules)
            {
                if (v.LevelId != level.LevelId)
                    continue;
                level.levelFirstPassPB = v;
                break;
            }
        }
    }

    public WeatherPB GetWeatherRulesById(int id)
    {
        return _weatherRules.Find((m) => { return m.WeatherId == id; });
    }

    public int GetVisitedTimesById(PlayerPB Pb)
    {
        int num = 0;
        for (int i = 0; i < _myVisitLevelVos.Count; i++)
        {
            if (_myVisitLevelVos[i].NpcId != Pb)
                continue;
            num += _myVisitLevelVos[i].Count;

            num += _myVisitLevelVos[i].BuyCount * VisitVo.MaxSingleVisitTime;


        }

        return num;
    }




    public List<VisitChapterVo> GetVisitChapterVo(PlayerPB NpcId)
    {
        return VisitChapterDir[NpcId];
    }


    public VisitLevelVo GetVisitLevelVoById(int levelId, PlayerPB NpcId)
    {
        List<VisitChapterVo> list = VisitChapterDir[NpcId];
        for (int i = 0; i < list.Count; i++)
        {
            VisitLevelVo vo = list[i].LevelList.Find((m) => { return levelId == m.LevelId; });
            if (vo != null)
            {
                return vo;
            }
        }
        return null;
    }

    public void InitMyData(MyVisitingRes res)
    {
        //关卡数据必须在前
        _myVisitLevelVos = new List<MyVisitLevelVo>();
        for (int i = 0; i < res.UserLevels.Count; i++)
        {
            MyVisitLevelVo vo = new MyVisitLevelVo(res.UserLevels[i]);
            _myVisitLevelVos.Add(vo);
            PlayerPB NpcId = (PlayerPB)((vo.LevelId / 1000) % 10);
            VisitLevelVo levelVo = GetVisitLevelVoById(vo.LevelId, NpcId);
            levelVo.MyVisitLevel = vo;

        }

        _listVo = new List<VisitVo>();
        for (int i = 0; i < res.UserWeathers.Count; i++)
        {
            VisitVo vo = new VisitVo(res.UserWeathers[i],
                GetWeatherRulesById(res.UserWeathers[i].WeatherId),
                GetVisitedTimesById(res.UserWeathers[i].Player),
                this
                );

            _listVo.Add(vo);
        }
    }
    public void UpdateMyWeather(UserWeatherPB pb)
    {
        var vo = _listVo.Find((m) => { return m.NpcId == pb.Player; });
        vo.UpdateUserWeatherData(
            pb,
            GetWeatherRulesById(pb.WeatherId)
            );
    }

    /// <summary>
    /// 获取第blessTime次祈福的成功率
    /// </summary>
    /// <param name="weatherId"></param>
    /// <param name="blessTime"></param>
    /// <returns></returns>
    public int GetSuccessRate(int weatherId, int blessTime)
    {
        var weaterPb = WeatherBlessRules.Find((m) => { return m.WeatherId == weatherId && m.BlessNum == blessTime; });
        if (weaterPb == null)
        {
            Debug.LogError("GetSuccessRate weatherId" + weatherId + "  blessTime  " + blessTime);
            return 100;
        }
        return weaterPb.BlessShowPro;
    }

    public VISIT_WEATHER GetWeatherByNpcId(PlayerPB npcId)
    {
        //return VISIT_WEATHER.Cloud;
        return _listVo.Find((match) => { return match.NpcId == npcId; }).CurWeather;
    }

    public VisitVo GetVisitVo(PlayerPB npcId)
    {
        return _listVo.Find((m) => { return m.NpcId == npcId; });
    }

    public int GetBlessCost(int time)
    {
        int max = 0;
        foreach (var v in _weatherBlessCostPBs)
        {
            if (v.BlessNum == time)
            {
                return v.BlessCost;
            }
            max = max > v.BlessCost ? max : v.BlessCost;
        }
        return max;
    }
    /// <summary>
    /// 重设次数消费数量
    /// </summary>
    /// <param name="第几次重置"></param>
    /// <returns></returns>
    public int GetResetCost(int time)
    {
        if (time <= 0)
            return 0;

        int maxConsume = 0;
        foreach (var v in _visitingResetConsumptionPBs)
        {
            if (time == v.ResetTimes)
            {
                return v.Consume;
            }
            maxConsume = v.Consume > maxConsume ? v.Consume : maxConsume;
        }
        return maxConsume;

    }
}

