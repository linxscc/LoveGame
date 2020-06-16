using System;
using Com.Proto;

namespace game.main
{

	/// <summary>
	/// 任务用户数据
	/// </summary>
	public class UserMissionVo:IComparable<UserMissionVo>
	{
		public int UserId;
		public int MissionId;
		public MissionTypePB MissionType;
		public MissionStatusPB Status;
		public long Progress;
		public long Finish;
		public int FansNum;
		public int JumpType;
		public int MissionPro;
		public bool IsPreview = false;

		public void  InitData(UserMissionPB userMissionPb)
		{
			UserId = userMissionPb.UserId;
			MissionId = userMissionPb.MissionId;
			MissionType = userMissionPb.MissionType;
			Status = userMissionPb.Status;
			Progress = userMissionPb.Progress;
			Finish = userMissionPb.Finish;
			
			//遇到奇葩的排序要求直接给权重
			UpdateMissionPro(Status);
		}

		public void UpdateMissionPro(MissionStatusPB Status)
		{
			switch (Status)
			{
				case MissionStatusPB.StatusUnclaimed:
					MissionPro = 0;
					break;
				case MissionStatusPB.StatusUnfinished:
					MissionPro = 1;
					break;
				case MissionStatusPB.StatusBeRewardedWith:
					MissionPro = 2;
					break;
			}
			
		}
		
		
		//参考应援活动进行排序:
		public int CompareTo(UserMissionVo other)
		{
			//根据MissionStatusPB来排序！
			int result = 0;
			
			if (other.MissionPro.CompareTo(MissionPro) != 0)
			{
				result = -other.MissionPro.CompareTo(MissionPro);
			}
			else if (other.MissionId.CompareTo(MissionId)!=0)
			{
				result = other.MissionId.CompareTo(MissionId);
			}


			return result;


		}
		
		
		

	}
    

    
}