using Com.Proto;
using Google.Protobuf.Collections;

namespace game.main
{
    public class EncourageActRuleVo
    {


        public int Id;                              //
        public string Title;					//应援活动标题
        public int NeedTime;					//完成需要的时间
        public MapField<int, int> Fans;		//需要的粉丝
        public MapField<int, int> Consume;	//应援道具
        public RepeatedField<AwardPB> Awards;			//奖励
        public int Power;						//需要花费的体力
        public int DepartmentLevel;				//解锁等级
        public int Pro;							//权重
        public int ActType;                     //应援活动类型
        public int Order;                       //奖励量级
        public int RandomFansNum;               //奖励随机粉丝数量
        public int MovieId;//动画Id

        public EncourageActRuleVo(EncourageActRulePB encourageActRulePb)
        {
            Id = encourageActRulePb.Id;
            Title = encourageActRulePb.Title;
            NeedTime = encourageActRulePb.NeedTime;
            Fans = encourageActRulePb.Fans;
            Consume = encourageActRulePb.Consume;
            Awards = encourageActRulePb.Awards;
            Power = encourageActRulePb.Power;
            DepartmentLevel = encourageActRulePb.DepartmentLevel;
            Pro = encourageActRulePb.Pro;
            ActType = encourageActRulePb.ActType;
            Order = encourageActRulePb.Order;
            RandomFansNum = encourageActRulePb.RandomeFansNum;
            MovieId = encourageActRulePb.MovieId;
        }

    }

    public class EncourageActRefreshVo
    {
        public int Count;  //刷新次数
        public int Gold;    //花费

        public EncourageActRefreshVo(EncourageActRefreshRulePB _encourageActRefresh)
        {

            Count = _encourageActRefresh.Count;
            Gold = _encourageActRefresh.Gold;
        }
        
        
    }

    public class EncourageActDoneRuleVo
    {
        public int Time;  //剩余时间
        public int Gem;   //花费

        public EncourageActDoneRuleVo(EncourageActDoneRulePB encourageActDoneRulePb)
        {
            Time = encourageActDoneRulePb.Time;
            Gem = encourageActDoneRulePb.Gem;
        }
        
        
    }
    

}

