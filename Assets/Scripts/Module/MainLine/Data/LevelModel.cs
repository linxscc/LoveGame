using Assets.Scripts.Framework.GalaSports.Core;
using Com.Proto;
using System.Collections.Generic;
using DataModel;
using Google.Protobuf.Collections;
using UnityEngine;

public class LevelModel : Model
{
    /// <summary>
    /// 已经通关的关卡数量
    /// </summary>
    public int PassLevelCount;
    
    public Dictionary<int, LevelVo> LevelDict;

    
    
    
    public List<MyLevelVo> MyLevelInfoList;

    public List<ChapterVo> ChapterList;

    public RepeatedField<LevelCommentRulePB> CommentRule;

    public RepeatedField<ChallengeCardNumRulePB> CardNumRules
    {
        set;get;
    }

    public RepeatedField<LevelInfoRulePB> InfoRule;
    public RepeatedField<LevelPlotRulePB> PlotRule;


    /// <summary>
    /// 挑战次数购买规则
    /// </summary>
    public RepeatedField<LevelBuyRulePB> LevelBuyRules;

    /// <summary>
    /// 当前活跃的关卡
    /// </summary>
    public LevelVo ActiveLevel;

    public LevelVo NewNormalLevel;
    public LevelVo NewHardLevel;

    public LevelVo LastLevel;


    /// <summary>
    /// 关卡跳转
    /// </summary>
    public JumpData JumpData;

    public bool DoJump = false;
    private LevelVo _firstNormalLevel;
    private LevelVo _firstHardLevel;

    public List<List<LevelData>> LocalDataList;

    
    
    public void SetData(LevelRes res)
    {
        
        LocalDataList = ClientData.LoadLevelData();

        LevelBuyRules = res.LevelBuyRules;
        CommentRule = res.CommentRules;
        CardNumRules = res.CardNumRules;

        InfoRule = res.InfoRules;
        PlotRule = res.PlotRules;

        LevelDict = new Dictionary<int, LevelVo>();
        ChapterList = new List<ChapterVo>();

        ChapterVo chapter = null;

        Dictionary<int, ChapterVo> chapterDict = new Dictionary<int, ChapterVo>();

        for (int i = 0; i < res.Levels.Count; i++)
        {
            var level = new LevelVo();
            level.SetData(res.Levels[i], PlotRule, InfoRule, LocalDataList);
          
            LevelDict.Add(level.LevelId, level);

            if (chapterDict.ContainsKey(level.ChapterGroup) == false)
            {
                chapter = new ChapterVo();
                chapterDict[level.ChapterGroup] = chapter;
                chapter.LevelList = new List<LevelVo>();
                chapter.HardLevelList = new List<LevelVo>();
                chapter.ChapterId = level.ChapterGroup;

                for (int j = 0; j < InfoRule.Count; j++)
                {
                    var info = InfoRule[j];
                    if (info.InfoType == 1 && info.InfoId == level.ChapterGroup)
                    {
                        chapter.ChapterName = info.LevelName;
                        chapter.ChapterDesc = info.LevelDesc;
                        break;
                    }
                }
            }

            if (level.Hardness == GameTypePB.Difficult)
            {
                chapterDict[level.ChapterGroup].HardLevelList.Add(level);
            }
            else
            {
                chapterDict[level.ChapterGroup].LevelList.Add(level);
            }
        }

//        var prevPos = GetPrevChapterPos();
//        var nextPos = GetNextChapterPos();
        foreach (var chapterVo in chapterDict)
        {
            ChapterList.Add(chapterVo.Value);
//            if (nextPos.Length >= chapterVo.Value.ChapterId)
//            {
//                chapterVo.Value.NextPos = nextPos[chapterVo.Value.ChapterId - 1];
//                chapterVo.Value.PrevPos = prevPos[chapterVo.Value.ChapterId - 1];
                if (chapterDict.ContainsKey(chapterVo.Value.ChapterId + 1))
                {
                    chapterVo.Value.NextChapterVo = chapterDict[chapterVo.Value.ChapterId + 1];
                }

                if (chapterDict.ContainsKey(chapterVo.Value.ChapterId - 1))
                {
                    chapterVo.Value.PrevChapterVo = chapterDict[chapterVo.Value.ChapterId - 1];
                }
//            }
        }
    }

  
    
    public void UpdateMyLevel(UserLevelPB myLevelPb)
    {
        LevelVo level = GetLevelInfo(myLevelPb.LevelId);
        level.BuyCount = myLevelPb.BuyCount;
        level.ChallangeTimes = myLevelPb.Count;
    }

    public void SetMyLevelData(MyLevelRes res)
    {
        PassLevelCount = res.UserLevels.Count;
        MyLevelInfoList = new List<MyLevelVo>();
        MyLevelVo lastLevel = null;
        int myLevle = GlobalData.PlayerModel.PlayerVo.Level;
        for (int i = 0; i < res.UserLevels.Count; i++)
        {
            var level = new MyLevelVo();
            level.SetData(res.UserLevels[i]);
            MyLevelInfoList.Add(level);
            LevelVo item = GetLevelInfo(level.LevelId);
            item.ChallangeTimes = level.Count;
            item.BuyCount = level.BuyCount;
            if (level.Star > 0)
            {
                item.IsPass = true;
                item.CurrentStar = level.Star;
                item.Score = level.Score;
            }

            if (item.Hardness == GameTypePB.Ordinary)
            {
                if(myLevle >= item.DepartmentLevel)
                    lastLevel = level;
            }
        }

        JudgeChaptersOpen();

        foreach (var levelVo in LevelDict)
        {
            if (levelVo.Value.BeforeLevelId == 0)
            {
                //第一关特殊处理
                if (levelVo.Value.Hardness == GameTypePB.Difficult)
                {
                    List<LevelVo> list = ChapterList[levelVo.Value.ChapterGroup - 1].LevelList;
                    levelVo.Value.IsOpen = list[list.Count - 1].IsPass;
                }
                else
                {
                    levelVo.Value.IsOpen = true;
                }
            }
            else
            {
                LevelVo beforeLevel = GetLevelInfo(levelVo.Value.BeforeLevelId);
                //上一个关通关
                if (levelVo.Value.Hardness == GameTypePB.Ordinary)
                {
                    levelVo.Value.IsOpen = beforeLevel.IsPass;
                }
                else if (levelVo.Value.Hardness == GameTypePB.Difficult && beforeLevel.IsPass)
                {
                    levelVo.Value.IsOpen = ChapterList[levelVo.Value.ChapterGroup - 1].IsHardOpen;
                }
            }
        }

        if (JumpData != null)
        {
            //需要跳转到战斗开始
            ActiveLevel = FindLevel(JumpData);

            if (ActiveLevel != null)
            {
                DoJump = true;
                ClientData.CustomerSelectedLevel = ActiveLevel;
                return;
            }
        }

        DoJump = false;

        if (ClientData.CustomerSelectedLevel == null)
        {
            if (lastLevel != null)
            {
                //在玩家没有选择关卡的时候，找到最后一个通关的普通关卡
                LevelVo levelVo = GetLevelInfo(lastLevel.LevelId);
                if (levelVo.IsPass && levelVo.AfterLevelId != 0)
                {
                    //AfterLevelId=0最后一关
                    LevelVo tempLevel = GetLevelInfo(levelVo.AfterLevelId);
                    if (myLevle >= tempLevel.DepartmentLevel)
                    {
                        levelVo = tempLevel;
                    }
                }

                ActiveLevel = levelVo;
                ClientData.CustomerSelectedLevel = ActiveLevel;
            }
            else
            {
                //没有通关任何关卡
                if (res.UserLevels.Count == 0)
                {
                    ActiveLevel = ChapterList[0].LevelList[0];
                }
            }
        }

        if (ClientData.CustomerSelectedLevel != null)
        {
            //使用上次选择的关卡
            ActiveLevel = ClientData.CustomerSelectedLevel;
        }

        
    }

    private LevelVo GetNewLevel(LevelVo levelVo)
    {
        if (levelVo.BeforeLevelId == 0 && levelVo.IsPass == false)
            return levelVo;

        LevelVo level = GetNextLevelInfo(levelVo);

        if (level == null)
        {
            //到最后一关返回
            return null;
        }

        if (level.IsPass)
        {
            return GetNewLevel(level);
        }

        return level;
    }

    public void JudgeChaptersOpen()
    {
        for (int i = 0; i < ChapterList.Count; i++)
        {
            ChapterVo chapter = ChapterList[i];
//            int firstLevelId = chapter.LevelList[0].LevelId;
//            int beforeLevelId = GetLevelInfo(firstLevelId).BeforeLevelId;

            LevelVo firstLevel = chapter.LevelList[0];
            int beforeLevelId = firstLevel.BeforeLevelId;
            
            if (beforeLevelId == 0)
            {
                _firstNormalLevel = firstLevel;
                chapter.IsNormalOpen = true;
            }
            else
            {
                //找到章节第一关卡的上一个关卡是否通过
                LevelVo level = GetLevelInfo(beforeLevelId);
                chapter.IsNormalOpen = level.IsPass;
            }
        }

//        //精英关卡开放逻辑
//        for (int i = 0; i < ChapterList.Count; i++)
//        {
//            ChapterVo chapter = ChapterList[i];
//            int firstLevelId = chapter.HardLevelList[0].LevelId;
//            int beforeLevelId = GetLevelInfo(firstLevelId).BeforeLevelId;
//            if (beforeLevelId == 0)
//            {
//                _firstHardLevel = GetLevelInfo(firstLevelId);
//                if (chapter.LevelList[chapter.LevelList.Count - 1].IsPass)
//                {
//                    chapter.IsHardOpen = true;
//                }
//            }
//            else
//            {
//                //找到章节第一个关卡的上一个关卡是否通过
//                //判断普通的这一章是否通关
//                if (chapter.ChapterId - 1 >= 0)
//                {
//                    ChapterVo thisChapter = ChapterList[chapter.ChapterId - 1];
//                    if (thisChapter.LevelList[thisChapter.LevelList.Count - 1].IsPass == false)
//                    {
//                        chapter.IsHardOpen = false;
//                        continue;
//                    }
//                }
//
//                chapter.IsHardOpen = GetLevelInfo(beforeLevelId).IsPass;
//            }
//        }
        
        NewNormalLevel = GetNewLevel(_firstNormalLevel);
//        NewHardLevel = GetNewLevel(_firstHardLevel);
    }

    public LevelVo GetLevelInfo(int levelId)
    {
        if (!LevelDict.ContainsKey(levelId))
        {
            return null;
        }
        return LevelDict[levelId];
    }

    public LevelVo GetNextLevelInfo(LevelVo vo)
    {
        if (LevelDict.ContainsKey(vo.AfterLevelId))
        {
            return LevelDict[vo.AfterLevelId];
        }

        return null;
    }

    public LevelVo GetPrevLevelInfo(LevelVo vo)
    {
        if (LevelDict.ContainsKey(vo.BeforeLevelId))
        {
            return LevelDict[vo.BeforeLevelId];
        }

        return null;
    }


    private Vector2[] GetPrevChapterPos()
    {
        Vector2[] preArr =
        {
            Vector2.zero,
            new Vector2(358, -415),
            new Vector2(550, -454),
            new Vector2(279, -425),
            new Vector2(279, -425),
            new Vector2(279, -425),
        };

        return preArr;
    }

    private Vector2[] GetNextChapterPos()
    {
        Vector2[] nextArr =
        {
            new Vector2(208, -2453),
            new Vector2(503, -2447),
            new Vector2(196, -2447),
            new Vector2(362, -2419),
            new Vector2(362, -2419),
            new Vector2(362, -2419),
        };

        return nextArr;
    }

    public LevelVo FindLevel(string level)
    {
        if (LevelDict == null)
            return null;
        
        foreach (var levelVo in LevelDict)
        {
            if (level == levelVo.Value.LevelMark)
            {
                return levelVo.Value;
            }
        }

        return null;
    }

    public LevelVo FindLevel(JumpData jumpData)
    {
        foreach (var vo in LevelDict)
        {
            if (jumpData.Type == "H")
            {
                if (vo.Value.Hardness == GameTypePB.Difficult && vo.Value.LevelMark == jumpData.Data)
                {
                    return vo.Value;
                }
            }
            else
            {
                if (vo.Value.Hardness == GameTypePB.Ordinary && vo.Value.LevelMark == jumpData.Data)
                {
                    return vo.Value;
                }
            }
        }

        return null;
    }



    
}