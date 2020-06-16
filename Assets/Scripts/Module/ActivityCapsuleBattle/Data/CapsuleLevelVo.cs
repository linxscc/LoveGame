using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Com.Proto;
using DataModel;
using game.tools;
using Google.Protobuf.Collections;
using Module.Battle.Data;
using UnityEngine;

public class CapsuleLevelVo : LevelVo
{

    public int ActivityId;
    
    
    /// <summary>
    /// 每日重置的挑战次数
    /// </summary>
    public int Count;

    /// <summary>
    /// 当前可玩次数
    /// </summary>
    public int CurPlayNum;

    /// <summary>
    /// 是否首次免费
    /// </summary>
    public bool IsFree=true;


    /// <summary>
    /// 扭蛋战斗买次次数
    /// </summary>
    public int CapsuleBattleBuyCount = 0;
    
    public void SetData(ActivityLevelRulePB pb,RepeatedField<LevelPlotRulePB> plotRule,RepeatedField<LevelInfoRulePB> infoRule)
    {
        
        ActivityId = pb.ActivityId;
        Count = pb.Count; 
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
        CurPlayNum = pb.Count;
        switch (LevelType)
        {
            case LevelTypePB.Story:
                StoryId = LevelMark;
                break;
            case LevelTypePB.Value:
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
                        else if(rulePb.LevelPlotType == LevelPlotTypePB.PlotPlayer)
                        {
                            HeroDescription = rulePb.PlotDesc;
                            List<string> temp = new List<string>();
                            foreach (var t in Abilitys)
                            {
                                temp.Add($"<color=#DC88A7>{ViewUtil.AbilityToString(t)}</color>");
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
                break;        
        }
        MaxChallangeTimes = pb.Max;

        var values = new List<int>();
        values.AddRange(pb.StarSource.Values);
        MaxPower = values.Count>0 ? values[values.Count - 1] : 100;
    }
    
    public new List<DropVo> DropsList
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
