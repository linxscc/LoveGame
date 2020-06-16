using Com.Proto;
using DataModel;
using game.tools;
using Google.Protobuf.Collections;
using Module.Battle.Data;
using Module.VisitBattle.Data;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public struct MapPos
{
    public int index;
    public Vector3 pos;
}

public class VisitLevelVo
{
    public int LevelId;
    public int BeforeLevelId;
    public int AfterLevelId;
    public string StoryId
    {
        set;
        get;
    }
    public LevelTypePB LevelType;//story or battle
    public RepeatedField<AbilityPB> Abilitys;//能力标签

    /// <summary>
    /// PlotTypePB 男主 粉丝
    /// </summary>
    public MapField<int, int> LevelPlot;

    public string ChapterBackdrop;//章节背景
    public string LevelMark;
    /// <summary>
    /// 关卡描述
    /// </summary>
    public string LevelDescription;
    /// <summary>
    /// 关卡信息ID，用于匹配几个rule表的信息
    /// </summary>
    public int LevelInfoId;
    public string FansKeyword;

    public VisitChapterVo VisitChapter;//当前关卡所在章节章节信息
    public LevelFirstPassPB levelFirstPassPB;//当前关卡首通奖励
    public RepeatedField<AwardPB> Awards;

    public string Sweetness;//甜蜜度 

    public ResourcePB  LevelAwardsType
    {
        get
        {
            if(_levelAwardsType==null)
            {
                if (Awards.Count == 0)
                {
                    Debug.LogError("Awards count is zero,can not continue;");
                    return ResourcePB.Item;
                }

                _levelAwardsType = Awards[0];
                foreach (var v in Awards)
                {
                    if (v.Resource == ResourcePB.Puzzle)
                    {
                        _levelAwardsType = v;
                        break;
                    }

                }
            }

            return _levelAwardsType.Resource;
        }
    }
    AwardPB _levelAwardsType;

    public int PreLevelId;
    public int NextLevelId;

    VisitLevelVo _preVisitLevel;
    VisitLevelVo _nextVisitLevel;

    public VisitLevelVo PreVisitLevel
    {
        get
        {
            if (_preVisitLevel == null)
                _preVisitLevel = VisitChapter.GetVisitLevelVoById(PreLevelId);
            return _preVisitLevel;
        }
    }
    public VisitLevelVo NextVisitLevel
    {
        get
        {
            if (_nextVisitLevel == null)
                _nextVisitLevel = VisitChapter.GetVisitLevelVoById(NextLevelId);
            return _nextVisitLevel;
        }
    }
    /// <summary>
    /// 根据应援会等级开放
    /// </summary>
    public int DepartmentLevel;

    public bool IsPass
    {
        get
        {
            if (MyVisitLevel == null)
                return false;
            return MyVisitLevel.Star != 0;
        }
    }

    public bool IsCanPass//判断是否能调整
    {
        get
        {
            if (IsPass)
                return true;
            //todo
            if (PreVisitLevel == null)
                return true;
            if (PreVisitLevel.IsPass && !IsPass)
                return true;
            return false;
        }
    }

    public int MaxPower
    {
        get { return _maxPower; }
    }
    private int _maxPower;
    public string LevelName
    { set;
        get;
    }
    public string HeroDescription { get; set; }
    public string FansDescription { get; set; }

    public MapField<int, int> ItemMax
    {
        get; set;
    }
    public MapField<int, int> FansMax
    {
        get; set;
    }

    public MyVisitLevelVo MyVisitLevel;

    /// <summary>
    ///扫荡购买次数
    /// </summary>
    public int BuyCount
    {
        get
        {
            if (MyVisitLevel == null)
            {
                return 0;
            }
            return MyVisitLevel.BuyCount;
        }
    }

    public int CurChallengeTime
    {
        get
        {
            if(MyVisitLevel==null)
            {
                return 0;
            }
            return MyVisitLevel.Count;
        }
    }



    public int CurrentStar
    {
        get
        {
            if (MyVisitLevel == null)
                return 0;
            return MyVisitLevel.Star;
        }
    }
    public int MaxStar;
    public int Score
    {
        get
        {
            return MyVisitLevel==null?0:MyVisitLevel.Score;
        }
    }
    public GameTypePB Hardness;
    public int ChapterGroup;//章节ID
    public PlayerPB NpcId;
    public LevelExtraPB LevelExtra;//关卡额外要求

    public VisitLevelVo(VisitingLevelRulePB pb, RepeatedField<VisitingLevelPlotRulePB> plotRule, RepeatedField<VisitingLevelInfoRulePB> infoRule)
    {
        LevelId = pb.LevelId;
        BeforeLevelId = pb.BeforeLevelId;
        AfterLevelId = pb.AfterLevelId;
        LevelMark = pb.LevelMark;
        LevelInfoId = pb.LevelInfoId;
        NpcId = (PlayerPB)((LevelId / 1000) % 10);
        LevelType = pb.Type;
        Abilitys = pb.Abilities;
        ChapterGroup = pb.ChapterGroup;
        DepartmentLevel = pb.LevelExtra.DepartmentLevel;
        ItemMax = pb.ItemMax;
        FansMax = pb.FansMax;
        PreLevelId = pb.BeforeLevelId;
        NextLevelId = pb.AfterLevelId;
        Awards = pb.Awards;
        LevelExtra = pb.LevelExtra;
        LevelPlot = pb.LevelPlot;
        ChapterBackdrop = pb.ChapterBackdrop;
        Sweetness = pb.Sweetness;
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
                if (rulePb.LevelPlotType == LevelPlotTypePB.PlotFans&& rulePb.PlotId == LevelPlot[0])
                {
                    FansDescription = rulePb.PlotDesc;
                    FansDescription = string.Format(FansDescription, list.ToArray());
                }
                else if (rulePb.LevelPlotType == LevelPlotTypePB.PlotPlayer&& rulePb.PlotId == LevelPlot[1])
                {
                    HeroDescription = rulePb.PlotDesc;

                    List<string> temp = new List<string>();
                    for (int i = 0; i < Abilitys.Count; i++)
                    {
                        temp.Add($"<color=#DC88A7>{ViewUtil.AbilityToString(Abilitys[i])}</color>");
                    }
                    HeroDescription = string.Format(HeroDescription, temp.ToArray());
                }
            }

            foreach (var infoRulePb in infoRule)
            {
                if (infoRulePb.InfoId == LevelInfoId && infoRulePb.InfoType == 0)
                {
                    LevelDescription = infoRulePb.LevelDesc;
                    LevelName = infoRulePb.LevelName;
                    break;
                }
            }
        }



        Hardness = pb.GameType;
        var values = new List<int>();
        values.AddRange(pb.StarSource.Values);
        if (values.Count > 0)
        {
            _maxPower = values[values.Count - 1];
        }
        else
        {
            _maxPower = 100;
        }
    }

    public List<VisitDropVo> DropsList
    {
        get
        {
            List<VisitDropVo> dropsList = new List<VisitDropVo>();
            foreach (var award in Awards)
            {
                var vo = new VisitDropVo();
                vo.MaxNum = award.Num;
                vo.PropId = award.ResourceId;
                vo.Resource = award.Resource;

                if (award.Resource == ResourcePB.Item)
                {
                    vo.Name = GlobalData.PropModel.GetPropBase(award.ResourceId).Name;
                } else if (award.Resource == ResourcePB.Puzzle) 
                {
                    vo.Name = GlobalData.CardModel.GetCardBase(award.ResourceId).CardName;
                }
                else
                {
                    vo.Name = ViewUtil.ResourceToString(award.Resource);
                }

                if (award.Resource == ResourcePB.Gold)
                {
                    vo.OwnedNum = (int)GlobalData.PlayerModel.PlayerVo.Gold;
                    vo.IconPath = "Prop/particular/" + PropConst.GoldIconId;
                } else if (award.Resource == ResourcePB.Puzzle) 
                {
                    var pubzzle = GlobalData.CardModel.GetUserPuzzleVo(award.ResourceId);
                    vo.OwnedNum = pubzzle == null ? 0 : pubzzle.Num;   
                    vo.IconPath = "Head/" + vo.PropId;
                }
                else
                {
                    var prop = GlobalData.PropModel.GetUserProp(award.ResourceId);
                    vo.OwnedNum = prop != null ? prop.Num : 0;
                    vo.IconPath = "Prop/" + vo.PropId;
                }

        

                dropsList.Add(vo);
            }

            return dropsList;
        }
    }

}
