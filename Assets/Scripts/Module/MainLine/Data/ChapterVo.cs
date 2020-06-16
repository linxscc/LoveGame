
using System.Collections.Generic;
using DataModel;
using UnityEngine;

public class ChapterVo
{
    public string ChapterName;
    public int ChapterId;
    public bool IsNormalOpen;
    public bool IsHardOpen;
    
    public List<LevelVo> LevelList;
    public List<LevelVo> HardLevelList;
    public string ChapterDesc;
//    public Vector2 NextPos;
//    public Vector2 PrevPos;

    public ChapterVo NextChapterVo;
    public ChapterVo PrevChapterVo;

    public GameTypePB HardnessState = GameTypePB.Ordinary;

    /// <summary>
    /// 应援会等级是否满足
    /// </summary>
    public bool IsSupporterLevelSatisfy
    {
        get
        {
            LevelVo firstLevel = LevelList[0];
            if (firstLevel.BeforeLevelId == 0)
                return true;
            
            return GlobalData.PlayerModel.PlayerVo.Level >= firstLevel.DepartmentLevel;
        }
    }

    public int CurrentStar
    {
        get
        {
            List<LevelVo> list = LevelList;
            int currentStar = 0;
            if (HardnessState == GameTypePB.Difficult)
            {
                list = HardLevelList;
            }
            foreach (var levelVo in list)
            {
                if(levelVo.LevelType == LevelTypePB.Story)
                    continue;
                currentStar += levelVo.CurrentStar;
            }
            return currentStar;
        }
    }

    public int MaxStar
    {
        get
        {
            List<LevelVo> list = LevelList;
            int maxStart = 0;
            if (HardnessState == GameTypePB.Difficult)
            {
                list = HardLevelList;
            }
            foreach (var levelVo in list)
            {
                if(levelVo.LevelType == LevelTypePB.Story)
                    continue;
                maxStart += levelVo.MaxStar;
            }
            return maxStart;
        }
    }

    /// <summary>
    /// 普通章节通关
    /// </summary>
    /// <returns></returns>
    public bool IsNormalPass
    {
        get
        {
            return LevelList[LevelList.Count - 1].CurrentStar > 0;
        }
    }
    
    /// <summary>
    /// 精英章节通关
    /// </summary>
    /// <returns></returns>
    public bool IsHardPass
    {
        get
        {
            return HardLevelList[HardLevelList.Count - 1].CurrentStar > 0;
        }
    }

    
}