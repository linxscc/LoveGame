
using Com.Proto;

public class MyLevelVo
{
    public int UserId;
    public int LevelId;
    public int Star;
    public int Count;
    public long ResetTime;

    public int Score;

    public int BuyCount;

    public void SetData(UserLevelPB pb)
    {
        UserId = pb.UserId;
        LevelId = pb.LevelId;
        Star = pb.Star;
        Count = pb.Count;
        ResetTime = pb.ResetTime;
        Score = pb.MaxFraction;
        BuyCount = pb.BuyCount;
    }
}