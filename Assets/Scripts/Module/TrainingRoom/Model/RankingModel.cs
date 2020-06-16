using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core;
using Com.Proto;
using DataModel;
using Google.Protobuf.Collections;


/// <summary>
/// 排行榜Model
/// </summary>
public class RankingModel : Model
{
	private List<RankingVO> _rankings;
	private MyRankingVO _myRankingVo;

	public void InitOpenRanking(SendRankRes res)
	{
		_myRankingVo =new MyRankingVO(res.MyRank,res.MyRating,res);

		_rankings =new List<RankingVO>();

		foreach (var t in res.Infos)
		{
			RankingVO vo =new RankingVO(t);
			_rankings.Add(vo);
		}		
	}

	/// <summary>
	/// 获取我的当前排名
	/// </summary>
	/// <returns></returns>
	public MyRankingVO GetMyCurRanking()
	{
		return _myRankingVo;
	}

	/// <summary>
	/// 获取音乐信息
	/// </summary>
	/// <returns></returns>
	public RepeatedField<MusicInfoPB> GetMusicInfos()
	{
		return GlobalData.TrainingRoomModel.GetMusicInfo();
	}

	/// <summary>
	/// 获取音乐Id
	/// </summary>
	/// <param name="index"></param>
	/// <returns></returns>
	public int GetCurMusicId(int index)
	{
		var list = GetMusicInfos();
		return list[index].MusicId;
	}
	
	public int GetTodaySongIndex()
	{
		return GlobalData.TrainingRoomModel.GetCurSongIndex();
	}

	public int GetSongIndexMax()
	{
		return GlobalData.TrainingRoomModel.GetMusicInfo().Count - 1;
	}
	
	/// <summary>
	/// 获取当前排行榜数据
	/// </summary>
	/// <returns></returns>
	public List<RankingVO> GetCurRankings()
	{
		return _rankings;
	}
}
