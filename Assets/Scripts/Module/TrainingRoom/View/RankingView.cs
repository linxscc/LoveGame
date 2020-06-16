using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Service;
using Com.Proto;
using DataModel;
using DG.Tweening;
using game.main;
using GalaAccount.Scripts.Framework.Utils;
using Google.Protobuf.Collections;
using UnityEngine;
using UnityEngine.UI;

public class RankingView : View
{
	private Transform _content;
	private Transform _songImgList;
	private Button _leftBtn;
	private Button _rightBtn;
	
	private Transform _playerInfo;
	private GameObject _hint;
	private Transform _parent;
	private	int _curIndex;
	private int _maxIndex;

	private float _width;
	private bool _isMove = false;

	private RepeatedField<MusicInfoPB> _musicInfoLists;
	private Transform _rankContainer;

	private void Awake()
	{
		_content = transform.Find("Content");		
		_rankContainer = transform.Find("Bg/RankTop3");
		
		_leftBtn = _content.GetButton("LeftBtn");
		_rightBtn = _content.GetButton("RightBtn");		
		
		_parent = transform.Find("Content/RankList/Viewport/Content");
		_playerInfo = transform.Find("Content/PlayerInfo");

		_hint = _content.Find("Hint").gameObject;
		
		_leftBtn.onClick.AddListener(LeftBtn);
		_rightBtn.onClick.AddListener(RightBtn);
		
		ArrowsAni(_leftBtn.transform.GetChild(0), new Vector2(1.5f,1.5f),new Vector2(1f, 1f), 0.7f ,0.3f);
		ArrowsAni(_rightBtn.transform.GetChild(0), new Vector2(1.5f,1.5f),new Vector2(1f, 1f), 0.7f ,0.3f);
	}

	private void Start()
	{
		float scaleFactor = 1.0f / (Main.ScaleY * Main.CanvasScaleFactor);

		Vector3 scaleVector = new Vector3(scaleFactor, scaleFactor, 1);
		
		RectTransform songCover = transform.Find("Bg/SongImage").GetRectTransform();
		songCover.localScale = scaleVector;
		Vector3 pos = songCover.anchoredPosition;
		songCover.anchoredPosition = new Vector3(pos.x, pos.y * scaleFactor);

		RectTransform rankTop3 = transform.Find("Bg/RankTop3").GetRectTransform();
		rankTop3.localScale = scaleVector;
		pos = rankTop3.anchoredPosition;
		rankTop3.anchoredPosition = new Vector3(pos.x * scaleFactor/2, pos.y* scaleFactor/2);

		RectTransform rankList = transform.Find("Content/RankList").GetRectTransform();
		rankList.localScale = scaleVector;
		
		if((float)Screen.height / Screen.width > 1.80f)
		{
			pos = rankList.anchoredPosition;
			rankList.anchoredPosition =
				new Vector3(pos.x * scaleFactor, (pos.y + ModuleManager.OffY / 2) * scaleFactor);
		}
	}

	private void Reset()
	{
		_parent.RemoveChildren();
		
		for (int i = 1; i <= 3; i++)
		{
			Transform child = _rankContainer.GetChild(i);
			child.GetRawImage("HeadIcon/Head").texture = ResourceManager.Load<Texture>("Head/PlayerHead/PlayerHead");
			child.GetRawImage("HeadIcon/Frame").texture = ResourceManager.Load<Texture>("HeadFrame/60000");
			child.GetText("Name/Text").text = "无排名";
			child.GetText("Rank/Text").text =  "0分";
		}
	}
	
	public void SetData(List<RankingVO> list, MyRankingVO vo, int maxIndex)
	{
		Reset();
		
		_maxIndex = maxIndex;
		
		transform.GetText("Bg/SongImage/SongNameBg/Text").text = vo.MusicName;
		transform.GetRawImage("Bg/SongImage").texture = ResourceManager.Load<Texture>(vo.MusicCoverPath);

		_playerInfo.GetRawImage("HeadIcon/Head").texture = ResourceManager.Load<Texture>(vo.IconPath);
		_playerInfo.GetRawImage("HeadIcon/Frame").texture = ResourceManager.Load<Texture>(vo.FramePath);
		_playerInfo.GetText("Name").text = vo.Name;
		
		if(vo.IsPlay)
		{
			_playerInfo.GetText("Grade").text = vo.MyRating + "";
			_playerInfo.GetText("Score").text = vo.Score + "分";
		}
		else
		{
			_playerInfo.GetText("Grade").text = "";
			_playerInfo.GetText("Score").text = "";
		}

		var text = _playerInfo.GetText("HeadIcon/Rank");
		text.text = vo.IsRank ? vo.MyRank.ToString() : "未上榜";

		int len = 3;
		if (list.Count < 3)
			len = list.Count;

		
		for (int i = 1; i <= len; i++)
		{
			RankingVO rankingVo = list[i-1];
			Transform child = _rankContainer.GetChild(i);
			child.GetRawImage("HeadIcon/Head").texture = ResourceManager.Load<Texture>(rankingVo.IconPath);
			child.GetRawImage("HeadIcon/Frame").texture = ResourceManager.Load<Texture>(rankingVo.FramePath);
			child.GetText("Name/Text").text = rankingVo.UserName;
			child.GetText("Rank/Text").text = rankingVo.Score + "分";
		}

		if (list.Count < 4)
			return;
			
		GameObject prefab = GetPrefab("TrainingRoom/Prefabs/Rank/PlayerRankItem");
		for (int i = 3; i < list.Count; i++)
		{
			RankingVO rankingVo = list[i];
			GameObject go = Instantiate(prefab, _parent, false);
			go.transform.GetText("Rank").text = rankingVo.Ranking.ToString();
			go.transform.GetText("Name").text = rankingVo.UserName;
			go.transform.GetText("Image/Score").text = rankingVo.Score.ToString();
			go.transform.GetText("Image/Grade").text = rankingVo.RatingStr;
		}
	}

	

	private void LeftBtn()
	{
		_curIndex--;
		if (_curIndex<0)
		{			
			_curIndex = _maxIndex;
		}	
		
		SendMessage(new Message(MessageConst.CMD_TRAININGROOM_RANKING_LEFT_OR_RIGHT_MOVE,_curIndex));	
	}
	
	private void RightBtn()
	{
		_curIndex++;
		if (_curIndex>_maxIndex)
		{			
			_curIndex = 0;
		}
	
		SendMessage(new Message(MessageConst.CMD_TRAININGROOM_RANKING_LEFT_OR_RIGHT_MOVE,_curIndex));	
		
		
	}

	// private void SetLeftItemSiblingIndex()
	// {
	// 	var firstRect =_songImgList.GetChild(0).GetComponent<RectTransform>();
	// 	var lastRect =_songImgList.GetChild(_songImgList.childCount - 1).GetComponent<RectTransform>();
	// 	var x = lastRect.localPosition.x + _width;
	// 	firstRect.localPosition =new Vector2(x,0);
	// 	firstRect.SetSiblingIndex(_songImgList.childCount - 1);		
	// }
	//
	// private void SetRigthItemSiblingIndex()
	// {
	// 	var firstRect =_songImgList.GetChild(0).GetComponent<RectTransform>();
	// 	var lastRect =_songImgList.GetChild(_songImgList.childCount - 1).GetComponent<RectTransform>();
	// 	var x = firstRect.localPosition.x - _width;
	// 	lastRect.localPosition =new Vector2(x,0);
	// 	lastRect.SetSiblingIndex(0);	
	// }
	
	private void Ani(float width)
	{
		for (int i = 0; i < _songImgList.childCount; i++)
		{
			var rect = _songImgList.GetChild(i).transform.GetComponent<RectTransform>();
			var endPos =rect.localPosition.x + width;
			var tween = rect.DOLocalMoveX(endPos,1.0f);
			tween.OnPlay(() => {_isMove = true;});
			tween.OnComplete(() => {_isMove = false;});
		}
	}
	
	
	
	// private void RefreshBtn()
	// {
	// 	if(_isCooling)
	// 	{
	//        FlowText.ShowMessage("冷却中");		
	// 	}
	// 	else
	// 	{
	// 		SendMessage(new Message(MessageConst.CMD_TRAININGROOM_RANKING_LEFT_OR_RIGHT_MOVE,_curIndex));
	// 		FlowText.ShowMessage("发送刷新请求");
	// 		_isCooling = true;
	// 	}
	// 	
	// }
		
	/// <summary>
	/// 生成歌曲图片
	/// </summary>
	/// <param name="list"></param>
	// public void CreateSongImg(RepeatedField<MusicInfoPB> list,int todaySongIndex,int maxIndex)
	// {
	// 	_musicInfoLists =new RepeatedField<MusicInfoPB>();
	// 	_musicInfoLists = list;
	// 	
	// 	_curIndex = todaySongIndex;
	// 	_maxIndex = maxIndex;
	// 	
	// 	// _songImgList.RemoveChildren();
	// 	//
	// 	// var prefab = GetPrefab("TrainingRoom/Prefabs/SmallSongImage");
	// 	//
	// 	// for (int i = 0; i < list.Count; i++)
	// 	// {
	// 	// 	var item = Instantiate(prefab, _songImgList, false);
	// 	// 	item.transform.localScale = Vector3.one;
	// 	// 	item.transform.GetChild(0).GetText().text = list[i].MusicName;
	// 	// 	item.name = list[i].MusicId.ToString();
	// 	// 	
	// 	// 	var rect =  item.GetComponent<RectTransform>();
	// 	// 	var w = rect.GetWidth();
	// 	// 	_width = w;
	// 	// 	var localPosition = rect.localPosition;
	// 	// 	float x = (localPosition.x + w * i)-(todaySongIndex*w)  ;
	// 	// 	
	// 	// 	localPosition = new Vector3(x, localPosition.y, 0);
	// 	// 	rect.localPosition = localPosition;
	// 	// }
	// }

	private int CurItemIndex()
	{
	  int temp=0;		
	  var musicId =	_musicInfoLists[_curIndex].MusicId+"";
	  for (int i = 0; i < _songImgList.childCount; i++)
	  {
		  if (musicId==_songImgList.GetChild(i).name)
		  {
			  temp= _songImgList.GetChild(i).GetRectTransform().GetSiblingIndex();
			  break;
		  }
	  }
	  return temp;
	}
	
	
	private void ArrowsAni(Transform arrows,Vector2 starValue,Vector2 endValue ,float duration,float waitTime)
	{
		var t1 = arrows.DOScale(endValue, duration).SetEase(Ease.InOutSine );
		var t2 = arrows.DOScale(starValue, duration).SetEase(Ease.InOutSine );

		var tween = DOTween.Sequence()
			.Append(t2)
        
			.Append(t1);
		tween.onComplete = () => { ArrowsAni(arrows, starValue, endValue, duration, waitTime); };      
	}
}
