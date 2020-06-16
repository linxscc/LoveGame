
using Com.Proto;

public class MyVisitLevelVo
{
    public int UserId;
    public int LevelId;//关卡id
    public int Star;//星级
    public int Count;//当天刷的次数
    public long ResetTime;//当天刷的次数重置时间
    public int Score;//最高分数
    public int BuyCount;//三次扫荡购买次数
    public bool IsGetFirst;//首通奖励领取状态0未领取1已领取

    public PlayerPB NpcId;

    public MyVisitLevelVo(UserVisitingLevelPB pb)
    {
        UserId = pb.UserId;
        LevelId = pb.LevelId;
        Star = pb.Star;
        Count = pb.Count;
        ResetTime = pb.ResetTime;
        Score = pb.MaxFraction;
        BuyCount = pb.BuyCount;
        IsGetFirst = pb.FirstAwardsState == 1;
        NpcId = (PlayerPB)((LevelId / 1000) % 10);
    }
    public void UpdateVisitLevelVo(UserVisitingLevelPB pb)
    {
        UserId = pb.UserId;
        LevelId = pb.LevelId;
        Star = pb.Star;
        Count = pb.Count;
        ResetTime = pb.ResetTime;
        Score = pb.MaxFraction;
        BuyCount = pb.BuyCount;
        IsGetFirst = pb.FirstAwardsState == 1;
        NpcId = (PlayerPB)((LevelId / 1000) % 10);
    }


    //public void SetData(UserLevelPB pb)
    //{
    //    UserId = pb.UserId;
    //    LevelId = pb.LevelId;
    //    Star = pb.Star;
    //    Count = pb.Count;
    //    ResetTime = pb.ResetTime;
    //    Score = pb.MaxFraction;
    //    BuyCount = pb.BuyCount;
    //    IsGetFirst = pb.FirstAwardsState == 1;
    //}
}