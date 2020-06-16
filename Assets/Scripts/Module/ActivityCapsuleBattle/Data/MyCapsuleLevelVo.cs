

using Com.Proto;

public class MyCapsuleLevelVo
{

    public int LevelId;
    
    /// <summary>
    /// 星级
    /// </summary>
    public int Star;
    
    /// <summary>
    /// 购买次数
    /// </summary>
    public int BuyCount;
    
    /// <summary>
    /// 挑战次数
    /// </summary>
    public int Count;

    /// <summary>
    /// 历史最高分
    /// </summary>
    public int Score;


    /// <summary>
    /// 当前刷新的次数重置时间
    /// </summary>
    public long ResetTime;

    public int ActivityId;
    public void SetData(UserActivityLevelInfoPB pb)
    {
        LevelId = pb.LevelId;
        Star = pb.Star;
        BuyCount = pb.BuyCount;
        Count = pb.Count;
        Score = pb.MaxScore;
        ResetTime = pb.ResetTime;
        ActivityId = pb.ActivityId;
    }
}
