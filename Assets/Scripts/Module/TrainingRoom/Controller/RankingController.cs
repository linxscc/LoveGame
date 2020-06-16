using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module.NetWork;
using Com.Proto;
using Componets;
using Google.Protobuf.Collections;
using Random = UnityEngine.Random;


public class RankingController : Controller
{
	public RankingView View;

	private RankingModel _model;
	
	public override void Start()
	{		
		_model = GetData<RankingModel>();
		// View.CreateSongImg(_model.GetMusicInfos(),_model.GetTodaySongIndex(),_model.GetSongIndexMax());
		SendRankingReq();
	}

	
	private void SendRankingReq(int musicId =0)
	{
		LoadingOverlay.Instance.Show();
		SendRankReq req = new SendRankReq {MusicId = musicId};
			
		byte [] data =NetWorkManager.GetByteData(req);		
		NetWorkManager.Instance.Send<SendRankRes>(CMD.MUSICGAMEC_SENDRANK,data,GetMusicGameRankingData);		
	}

	private void GetMusicGameRankingData(SendRankRes res)
	{
		// res = new SendRankRes()
		// {
		// 	MusicId = 1001,
		// 	MyRank = 999,
		// 	MyRating = 1,
		// 	MyScore = 666,
		// 	Ret = 1,
		// 	Infos = { res.Infos }
		// };
		// for (int i = 0; i < 100 - res.Infos.Count; i++)
		// {
		// 	RankInfoPB pb = new RankInfoPB()
		// 	{
		// 		Ranking = i,
		// 		Rating = 8,
		// 		Score = Random.Range(0, 2000),
		// 		UseIcon = "{\"avatarBox\":60000,\"avatar\":342411}",
		// 		UseName = "Name_" + Random.Range(0, 2000),
		// 		UserId = 1,
		// 	};
		// 	res.Infos.Add(pb);
		// }
		
		_model.InitOpenRanking(res);
		
		View.SetData(_model.GetCurRankings(),_model.GetMyCurRanking(), _model.GetSongIndexMax());	
		LoadingOverlay.Instance.Hide();
	}


	public override void OnMessage(Message message)
	{
		string name = message.Name;
		object[] body = message.Params;
		switch (name)
		{
			case MessageConst.CMD_TRAININGROOM_RANKING_LEFT_OR_RIGHT_MOVE:
				
				var index = Convert.ToInt32(message.Body) ;
				var musicId = _model.GetCurMusicId(index);
				SendRankingReq(musicId);								
				break;
				
		}
	}
}
