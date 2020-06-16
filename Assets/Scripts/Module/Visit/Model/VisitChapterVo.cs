using System.Collections.Generic;
using DataModel;

public class VisitChapterVo  {
    public string ChapterName;
    public int ChapterId;
    public bool IsOpen;
    public List<VisitLevelVo> LevelList;

    public string ChapterDesc;

    public GameTypePB HardnessState = GameTypePB.Visiting;


    public VisitChapterVo(string chapterName,int chapterId,bool isOpen)
    {
        ChapterName = chapterName;
        ChapterId = chapterId;
        IsOpen = isOpen;
        LevelList = new List<VisitLevelVo>();
    }


    public VisitLevelVo GetVisitLevelVoById(int levelId)
    {
        return LevelList.Find((m) => { return m.LevelId == levelId; });
    }


    /// <summary>
    /// 应援会等级是否满足
    /// </summary>
    public bool IsSupporterLevelSatisfy
    {
        get
        {
            //LevelVo firstLevel = LevelList[0];
            //if (firstLevel.BeforeLevelId == 0)
            //    return true;

            //return GlobalData.PlayerModel.PlayerVo.Level >= firstLevel.DepartmentLevel;
            return true;
        }
    }
}
