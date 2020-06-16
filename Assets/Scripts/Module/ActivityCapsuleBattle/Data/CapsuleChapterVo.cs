using System.Collections;
using System.Collections.Generic;
using DataModel;
using UnityEngine;

public class CapsuleChapterVo 
{
    public string ChapterName;
    public int ChapterId;
    public bool IsNormalOpen;
    public bool IsHardOpen;

    public List<CapsuleLevelVo> LevelList;
    public List<CapsuleLevelVo> HardLevelList;
    
    public string ChapterDesc;
    
    public CapsuleChapterVo NextChapterVo;
    public CapsuleChapterVo PrevChapterVo;

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
            List<CapsuleLevelVo> list = LevelList;
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
            List<CapsuleLevelVo> list = LevelList;
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
