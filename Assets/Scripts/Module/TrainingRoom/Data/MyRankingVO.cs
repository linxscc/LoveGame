using System.Collections;
using System.Collections.Generic;
using Com.Proto;
using DataModel;
using UnityEngine;

public class MyRankingVO
{
	public bool IsRank =false;   //是否上榜
	public bool IsPlay =false;   //是否有分数
	
	public int MyRank;    ////我的排行 -1未上帮
	public string MyRating;  //我的评级
	public long Score;     //我的分数 -1没分数                  

	public string Name;  //我的姓名
	public string IconPath;  //我的头像
	public string FramePath;  //我的头像

	public int MusicId;

	public string MusicName;

	public string MusicCoverPath;

	public MyRankingVO(int myRank, int myRating, SendRankRes res)
	{
		MyRank = myRank;
		if (myRating < 1)
		{
			MyRating = "";
		}
		else
		{
			MyRating = RankingVO.RATINGS[myRating];
		}
		
		Score = res.MyScore;
		if (myRank !=-1) {IsRank = true;}    //不等于-1 ，说明上榜了		
		if (res.MyScore != -1){IsPlay = true;}   //不等于-1，说明玩过，有分数	
		
		Name = GlobalData.PlayerModel.PlayerVo.UserName;
		
		IconPath = GlobalData.DiaryElementModel.GetHeadPath(GlobalData.PlayerModel.PlayerVo.UserOther.Avatar, ElementTypePB.Avatar);
		FramePath = GlobalData.DiaryElementModel.GetHeadPath(GlobalData.PlayerModel.PlayerVo.UserOther.AvatarBox, ElementTypePB.AvatarBox);

		MusicInfoPB musicInfoPb = GlobalData.TrainingRoomModel.GetMusicInfoPbById(res.MusicId);
		MusicName = musicInfoPb?.MusicName;

		MusicCoverPath = "TrainingRoom/cover2/" + res.MusicId;
	}
}
