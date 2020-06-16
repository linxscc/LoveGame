using System.Collections;
using System.Collections.Generic;
using Com.Proto;
using DataModel;
using UnityEngine;

public class RankingVO
{
	public int UserId;          //玩家Id
	public string IconPath;        //玩家头像
	public string FramePath;        //玩家头像框
	public string UserName;     //玩家名字
	public int Rating;          //玩家评级
	public long Score;           //玩家分数
	public int Ranking;          //排名 后端是从0开始的，前端要+1

	public string RatingStr;

	public static readonly string[] RATINGS = {"-1", "SS", "S", "A", "B", "C", "D", "E"};
	
	public RankingVO(RankInfoPB pb)
	{
		UserId = pb.UserId;
		
		if (!string.IsNullOrEmpty(pb.UseIcon))
		{
			var json = new JSONObject(pb.UseIcon);

			IconPath = GlobalData.DiaryElementModel.GetHeadPath((int) json["avatar"].i, ElementTypePB.Avatar);
			FramePath = GlobalData.DiaryElementModel.GetHeadPath((int) json["avatarBox"].i, ElementTypePB.AvatarBox);
		} 
		
		UserName = pb.UseName;
		Rating = pb.Rating;
		Score = pb.Score;
		Ranking = pb.Ranking+1;

		RatingStr = RATINGS[Ranking];
	}
}
