using System;
using System.Collections.Generic;
using System.Text;
using Com.Proto;
using DataModel;
using game.main;
using game.tools;
using Google.Protobuf.Collections;
using Module.Battle.Data;
using UnityEngine;

public class LevelVo
{
    public int LevelId;
    public int BeforeLevelId;
    public int AfterLevelId;

    /// <summary>
    /// 每日最大挑战次数
    /// </summary>
    public int MaxChallangeTimes;

    /// <summary>
    /// 已经挑战次数
    /// </summary>
    public int ChallangeTimes;

    /// <summary>
    /// 挑战购买次数
    /// </summary>
    public int BuyCount;

   // public int EventId;
    public LevelTypePB LevelType;
    public RepeatedField<AbilityPB> Abilitys;
    public MapField<int, int> ItemMax;
    public MapField<int, int> FansMax;

    public bool IsPass;
    public int CurrentStar;
    public int MaxStar;
    public int MaxPower;
    
    public Vector2 Positon
    {
        get {
            List<LevelData> levelDataList = _localDataList[ChapterGroup-1];
            foreach (var levelData in levelDataList)
            {
                if (LevelId == levelData.levelId)
                {
                    return levelData.position;
                }
            }
            return new Vector2();
        }
    }

    public int Score;
    public string StoryId;

    public int ChapterGroup;


    /// <summary>
    /// 关卡信息ID，用于匹配几个rule表的信息
    /// </summary>
    public int LevelInfoId;

    public GameTypePB Hardness;

    public string LevelMark;

    public string FansKeyword;


    /// <summary>
    /// 粉丝描述
    /// </summary>
    public string FansDescription;

    /// <summary>
    /// 男主描述
    /// </summary>
    public string HeroDescription;

    /// <summary>
    /// 关卡描述
    /// </summary>
    public string LevelDescription;

    /// <summary>
    /// 关卡名字
    /// </summary>
    public string LevelName;

    public RepeatedField<AwardPB> Awards;


    /// <summary>
    /// PlotTypePB 男主 粉丝
    /// </summary>
    public MapField<int, int> LevelPlot;

    public bool IsOpen;
    private List<List<LevelData>> _localDataList;

    /// <summary>
    /// 根据应援会等级开放
    /// </summary>
    public int DepartmentLevel;

    public void SetData(LevelPB pb, RepeatedField<LevelPlotRulePB> plotRule, RepeatedField<LevelInfoRulePB> infoRule,
        List<List<LevelData>> localDataList)
    {
        _localDataList = localDataList;
        
        MaxStar = 3;
        Hardness = pb.GameType;
        LevelMark = pb.LevelMark;
        LevelInfoId = pb.LevelInfoId;
        ChapterGroup = pb.ChapterGroup;
        LevelPlot = pb.LevelPlot;
        LevelId = pb.LevelId;
        BeforeLevelId = pb.BeforeLevelId;
        AfterLevelId = pb.AfterLevelId;
        LevelType = pb.Type;
        Abilitys = pb.Abilities;
        Awards = pb.Awards;

        DepartmentLevel = pb.LevelExtra.DepartmentLevel;
        

//        if (pb.LevelCoordinate.Count > 0)
//            Positon = new Vector2(pb.LevelCoordinate[0], -pb.LevelCoordinate[1]);


 
        if (LevelType == LevelTypePB.Story)
        {
            StoryId = LevelMark;
        }
        else if (LevelType == LevelTypePB.Value) //数值关卡
        {
            ItemMax = pb.ItemMax;
            FansMax = pb.FansMax;

            StringBuilder sb = new StringBuilder();
            sb.Append("关键字：");
            List<string> list = new List<string>();
            foreach (var fan in FansMax)
            {
                string name = GlobalData.DepartmentData.GetFans(fan.Key).Name;
                sb.Append(name);
                sb.Append("、");
                list.Add("<color='#DC88A7'>" + name + "</color>");
            }

            FansKeyword = sb.ToString();
            FansKeyword = FansKeyword.Substring(0, FansKeyword.Length - 2);

            foreach (var rulePb in plotRule)
            {
                if (rulePb.PlotId == LevelPlot[0])
                {
                    if (rulePb.LevelPlotType == LevelPlotTypePB.PlotFans)
                    {
                        FansDescription = rulePb.PlotDesc;

                        FansDescription = string.Format(FansDescription, list.ToArray());
                    }
                    else if (rulePb.LevelPlotType == LevelPlotTypePB.PlotPlayer)
                    {
                        HeroDescription = rulePb.PlotDesc;
                      
                        List<string> temp = new List<string>();
                        for (int i = 0; i < Abilitys.Count; i++)
                        {
                            temp.Add($"<color=#DC88A7>{ViewUtil.AbilityToString(Abilitys[i])}</color>");
                           // temp.Add($"<color='#DC88A7'>" + ViewUtil.AbilityToString(Abilitys[i]) + "</color>");
                        }
                       
                        HeroDescription = string.Format(HeroDescription, temp.ToArray());
                      
                    }
                }
            }

            foreach (var infoRulePb in infoRule)
            {
                if (infoRulePb.InfoId == LevelInfoId && infoRulePb.InfoType == 0)
                {
                    LevelDescription = infoRulePb.LevelDesc;
                    LevelName = infoRulePb.LevelName;
                }
            }
        }

        MaxChallangeTimes = pb.Max;
        //EventId = pb.EventId;
        var values = new List<int>();
        values.AddRange(pb.StarSource.Values);
        if (values.Count > 0)
        {
            MaxPower = values[values.Count - 1];
        }
        else
        {
            MaxPower = 100;
        }
    }

    public int CostEnergy
    {
        get
        {
            if (Hardness == GameTypePB.Difficult)
                return GlobalData.ConfigModel.GetConfigByKey(GameConfigKey.CAREER_POWER_ELITE);

            return GlobalData.ConfigModel.GetConfigByKey(GameConfigKey.CAREER_POWER_NOMAL);
        }
    }

    public List<DropVo> DropsList
    {
        get
        {
            List<DropVo> dropsList = new List<DropVo>();
            foreach (var award in Awards)
            {
                var vo = new DropVo();
                vo.MaxNum = award.Num;

                if (award.Resource == ResourcePB.Item)
                {
                    vo.Name = GlobalData.PropModel.GetPropBase(award.ResourceId).Name;
                }
                else
                {
                    vo.Name = ViewUtil.ResourceToString(award.Resource);
                }

                if (award.Resource == ResourcePB.Gold)
                {
                    vo.OwnedNum = (int) GlobalData.PlayerModel.PlayerVo.Gold;
                    vo.IconPath = "Prop/particular/" + PropConst.GoldIconId;
                }
                else
                {
                    var prop = GlobalData.PropModel.GetUserProp(award.ResourceId);
                    vo.OwnedNum = prop != null ? prop.Num : 0;
                    vo.PropId = award.ResourceId;
                    vo.IconPath = "Prop/" + vo.PropId;
                }

                dropsList.Add(vo);
            }

            return dropsList;
        }
    }
}