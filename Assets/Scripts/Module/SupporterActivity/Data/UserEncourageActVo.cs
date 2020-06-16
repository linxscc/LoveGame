using System;
using Com.Proto;

namespace game.main
{
	public class UserEncourageActVo: IComparable<UserEncourageActVo>
	{
		public int Id;
		public int UserId;
		public int ActId;              //活动Id
		public int StartState;        //0未开始1开始
		public int ImmediateFinishState;  //0未使用立即完成1使用了立即完成
		public int AwardState;            //0未领取1已结领取
		public long AcceptTime;            //活动接取时间
		public bool NeedToChangeAni;       //是否需要切换动画
		public bool CanReceiveAward=false;      //已经发生请求命令了
		public int PosIndex;   //排序时所在的位置

		public UserEncourageActVo(UserEncourageActPB userEncourageActPb)
		{
			Id = userEncourageActPb.Id;
			UserId = userEncourageActPb.UserId;
			ActId = userEncourageActPb.ActId;
			StartState = userEncourageActPb.StartState;
			ImmediateFinishState = userEncourageActPb.ImmediateFinishState;
			AwardState = userEncourageActPb.AwardState;
			AcceptTime = userEncourageActPb.AcceptTime;
			NeedToChangeAni = false;
		}

		
		//在这里进行排序！！！Compare!
		public int CompareTo(UserEncourageActVo other)
		{
			int result = 0;
			if (StartState==1)//是startState为0还是other。startstate为0
			{
				return 0;
			}
			
			if (other.ActId.CompareTo(ActId)!=0)
			{
				result = other.ActId.CompareTo(ActId);
			}
			return -result;

		}
		
		
		
		
	}
	
	

	

}

