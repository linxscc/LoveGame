
using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Core.Events;
using Assets.Scripts.Module;
using Com.Proto;
using Common;
using DataModel;
using DG.Tweening;
using game.main;
using UnityEngine;
using UnityEngine.UI;
using game.tools;
using GalaAccount.Scripts.Framework.Utils;
using GalaAccountSystem;
using QFramework;

public class SongItem : View
{
	private const float FinishHeight = 245f;   //完成的高度
	private const float ShrinkHeight = 328f;   //收缩的高度
	private const float UnfoldHeight = 1308f;  //展开的高度
	
	private Transform _finishContent;
	private Transform _shrinkContent;
	private Transform _unfoldContent;
	private Transform _noFinishContent;
	private Text _activityName;

	public bool IsUnfold;
	private UserMusicGameVO _data;
	private RectTransform _rect;

	// private Button _unfoldBtn;
	private Button _shrinkBtn;
	private Button _changeBtn;
	private Button _hintBtn;
	private Button _chooseBtn;
	private Button _playBtn;

	private Transform _toggleContent;
	private Transform _cardContent;
	private Transform _chooseCardBtn;


	private void Awake()
	{
		_finishContent = transform.Find("Finish");
		_shrinkContent = transform.Find("Shrink");
		_unfoldContent = transform.Find("Unfold");
		_noFinishContent = transform.Find("NoFinish");
		
		_activityName = transform.GetText("ActivityName");
		_rect = transform.GetRectTransform();
		
		// _unfoldBtn = _shrinkContent.GetButton("UnfoldBtn");
		_shrinkBtn = _unfoldContent.GetButton("ShrinkBtn");
		_changeBtn = _noFinishContent.GetButton("ChangeBtn");
		_hintBtn = _unfoldContent.GetButton("HintBtn");
		_playBtn = _unfoldContent.GetButton("PlayBtn/OnClick");
		_chooseBtn = _unfoldContent.GetButton("CardContent/ChooseCardBtn/OnClick");
		_toggleContent = _unfoldContent.Find("ToggleContent");

		_cardContent = transform.Find("Unfold/CardContent/ScrollRect/Content");
		_chooseCardBtn = transform.Find("Unfold/CardContent/ChooseCardBtn");
		
		
		// _unfoldBtn.onClick.AddListener(UnfoldBtn);

		PointerClickListener.Get(_shrinkContent.gameObject).onClick = UnfoldBtn;
		
		_shrinkBtn.onClick.AddListener(ShrinkBtn);
		_changeBtn.onClick.AddListener(ChangeBtn);
	    _hintBtn.onClick.AddListener(HintBtn);
	    _playBtn.onClick.AddListener(PlayBtn);
	    _chooseBtn.onClick.AddListener(ChooseBtn);
	}
	
	public void SetCards(List<TrainingRoomCardVo> list)
	{
		_cardContent.RemoveChildren();

		var prefab = GetPrefab("TrainingRoom/Prefabs/ChooseCardItem");
		foreach (var t in list)
		{
			var item = Instantiate(prefab, _cardContent, false);
			item.GetComponent<ChooseCardItem>().SetData(t);
		}

		_chooseCardBtn.Hide();

	}

	//设置难度开放等级
	private void SetDifficultyOpenLevel()
	{
		var alreadyOpen = GlobalData.TrainingRoomModel.GetMusicDiffProgress();
		var maskContent = _toggleContent.Find("Mask");
		for (int i = 0; i < maskContent.childCount; i++)
		{
			if (i<alreadyOpen)
			{
				maskContent.GetChild(i).Find("Lock").gameObject.Hide();
			}
			else
			{
			  var lockIcon = maskContent.GetChild(i).Find("Lock").gameObject;
			  lockIcon.gameObject.Hide();
			  PointerClickListener.Get(lockIcon).onClick = go => {FlowText.ShowMessage("请先完成前一难度曲目");};
			}
		}

	}

	//选择星缘
	private void ChooseBtn()
	{
		EventDispatcher.TriggerEvent<UserMusicGameVO>(EventConst.GotoChooseCard, _data);
	}
	
	//演奏
	private void PlayBtn()
	{
		int diff = 0;

		var arr = _toggleContent.Find("Toggle").GetComponent<ToggleGroup>().ActiveToggles();
		foreach (Toggle toggle in arr)
		{
			if (toggle.isOn)
			{
				diff = int.Parse(toggle.name);
				break;
			}
		}
		SendMessage(new Message(MessageConst.MODULE_TRAININGROOM_ENTRY_GAME, Message.MessageReciverType.CONTROLLER, diff));
	}

	//提示
	private void HintBtn()
	{
		var hideFrame = _noFinishContent.Find("HideFrame");
		hideFrame.gameObject.Show();

		var imageRect = hideFrame.GetRectTransform("image");
	    var tween= imageRect.DOSizeDelta(new Vector2(500,310),0.3f);
	    tween.onComplete = () =>
	    {
		    imageRect.Find("Text").gameObject.Show();
		    PointerClickListener.Get(hideFrame.gameObject).onClick= go =>
		    {
			    imageRect.Find("Text").gameObject.Hide();
			    imageRect.sizeDelta = new Vector2(0,310);
			    hideFrame.gameObject.Hide();
		    };
	    };
	}

	//更换
	private void ChangeBtn()
	{
		EventDispatcher.TriggerEvent(EventConst.ChangeAbility, _data.ActivityId);
	}

	//收缩
	private void ShrinkBtn()
	{
		IsUnfold = false;
		_rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical,ShrinkHeight);
		_shrinkContent.gameObject.Show();
		_unfoldContent.gameObject.Hide();
	}


	//展开
	private void UnfoldBtn(GameObject go)
	{
		IsUnfold = true;
		_rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical,UnfoldHeight);
		_shrinkContent.gameObject.Hide();
		_unfoldContent.gameObject.Show();
		
		SendMessage(new Message( MessageConst.CMD_TRAININGROOM_ONCLICK_UNFOLD_BTN,_data.ActivityId));
	}

	public void SetShrinkState()
	{
		IsUnfold = false;
		_rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical,ShrinkHeight);
		_shrinkContent.gameObject.Show();
		_unfoldContent.gameObject.Hide();
		
		 _noFinishContent.Find("HideFrame").gameObject.Hide();
		 _noFinishContent.GetRectTransform("HideFrame/image").sizeDelta = new Vector2(0,310);
		 _noFinishContent.Find("HideFrame/image/Text").gameObject.Hide();
	}
	
	
	
	public void SetData(UserMusicGameVO vo)
	{
		_data = vo;
		_activityName.text = vo.ActivityName;			
		SetState();
		SetDifficultyOpenLevel();
	}

	private void SetState()
	{
		if (_data.IsFinish)
		{
			SetFinishState();
		}
		else
		{
			SetNoFinishState();
		}
	}

	private void SetFinishState()
	{		
	   _rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical,FinishHeight);
	   _finishContent.gameObject.Show();
	   _shrinkContent.gameObject.Hide();
	   _unfoldContent.gameObject.Hide();
	   _noFinishContent.gameObject.Hide();	
	}

	private void SetNoFinishState()
	{
		_finishContent.gameObject.Hide();
		_noFinishContent.gameObject.Show();
		SetNoFinishData();
		IsVipGame();
	}

	private void SetNoFinishData()
	{
		_noFinishContent.GetText("AwardIcon/Num").text = _data.AwardIntegral.ToString();
		_noFinishContent.GetText("DescText").text = _data.NeedAbilityDesc;
	}

	private void IsVipGame()
	{
		if (_data.IsVipMusicGame)
		{
			if (!_data.IsVip)
			{				
				_shrinkContent.gameObject.Show();
				_unfoldContent.gameObject.Hide();
				_rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical,ShrinkHeight);
				
			   var vip = _noFinishContent.Find("Vip").gameObject;
			   vip.gameObject.Show();
			   PointerClickListener.Get(vip).onClick = go => { FlowText.ShowMessage("仅限月卡用户可进入哦~");};			   
			}
			else
			{
				IsUnfoldState();
			}			
		}
		else
		{
			IsUnfoldState();
		}
	}

	private void IsUnfoldState()
	{
		if (IsUnfold)
		{
			_shrinkContent.gameObject.Hide();
			_unfoldContent.gameObject.Show();
			_rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical,UnfoldHeight);
		}
		else
		{
			_shrinkContent.gameObject.Show();
			_unfoldContent.gameObject.Hide();
			_rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical,ShrinkHeight);
		}
	}
	
	


//	private Transform _finish;	
//	private Transform _shrink;
//	private Transform _unfold;
//
//	
//	private UserMusicGameVO _data;
//	private RectTransform _rect;
//	
//
//	public bool IsUnfold;
//	
//
//	private Button _unfoldBtn;
//	private Button _shrinkBtn;
//	private Button _playBtn;
//	
//	private Transform _content;
//
//
//	private void Awake()
//	{	
//		_finish = transform.Find("Finish");			
//				
//		_shrink = transform.Find("Shrink");
//		_unfold = transform.Find("Unfold");
//		_rect =GetComponent<RectTransform>();
//		
//
//		_unfoldBtn = _shrink.GetButton("UnfoldBtn");
//		_shrinkBtn = _unfold.GetButton("ShrinkBtn");
//		_playBtn = _unfold.GetButton("PlayBtn");
//		
//		_unfoldBtn.onClick.AddListener(UnfoldBtn);
//		_shrinkBtn.onClick.AddListener(ShrinkBtn);
//		_playBtn.onClick.AddListener(PlayBtn);
//		
//		_content = _unfold.Find("Content");
//	}
//	
//	private void ShrinkBtn()
//	{
//		IsUnfold = false;
//		SetCurHeight();	
//	}
//
//	private void UnfoldBtn()
//	{
//			
//		IsUnfold = true;	
//		base.SetChildrenUnfold(_data.ActivityId);
//
//	}
//
//
//	
//	
//	
//
//	public void SetData(UserMusicGameVO vo)
//	{
//		_data = vo;	
//		
//		SetCurHeight();	
//	}
//
//	public void SetCurHeight()
//	{
//		if (_data.IsFinish)
//		{
//			
//			 _rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical,200f);
//			 _finish.gameObject.Show();
//			
//			 _shrink.gameObject.Hide();
//			 _unfold.gameObject.Hide();
//			 
//			 SetFinish();
//		}
//		else
//		{
//
//			if (IsUnfold)
//			{
//				
//				_rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical,1300f);		
//				
//				_finish.gameObject.Hide();				
//				_shrink.gameObject.Hide();
//				_unfold.gameObject.Show();
//				SetNoFinishUnfold();
//			}
//			else
//			{
//			
//				_rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical,300f);							
//				_finish.gameObject.Hide();				
//				_shrink.gameObject.Show();
//				_unfold.gameObject.Hide();
//
//				SetNoFinishShrink();
//			}
//			
//		}
//		
//	}
//
//	//设置完成
//	private void SetFinish()
//	{
//	    //	_bg.texture = ;//完成的Bg不一样	    	    
//	    _finish.GetText("ActivityName").text = _data.ActivityName;
//	    _finish.GetText("FinishAward").text = "已完成";
//	}
//
//
//	//设置没完成收缩
//	private void SetNoFinishShrink()
//	{
//		//	_bg.texture = ;//未完成收缩的Bg不一样
//		
//		
//		
//		_shrink.GetText("ActivityName").text = _data.ActivityName;
//		_shrink.GetText("FinishAward").text = "完成奖励 " + _data.AwardIntegral;
//		_shrink.GetText("NeedAbilityDesc").text = _data.NeedAbilityDesc;
//
//		var vip = _shrink.Find("Vip").gameObject;
//		vip.gameObject.Hide();
//		
//		if (_data.IsVipMusicGame && !_data.IsVip)
//		{
//			vip.gameObject.Show();
//			PointerClickListener.Get(vip).onClick = go =>
//				{
//					FlowText.ShowMessage("仅月卡用户可进行");
//				};
//			return;
//		}
//		
//		
//		_shrink.GetButton("ChangeBtn").onClick.AddListener(() =>
//		{			
//			EventDispatcher.TriggerEvent(EventConst.ChangeAbility,_data.ActivityId);			
//		});				
//	}
//
//	//设置没完成展开
//	private void SetNoFinishUnfold()
//	{
//		//	_bg.texture = ;//未完展开的Bg不一样
//		_unfold.GetText("ActivityName").text = _data.ActivityName;
//		_unfold.GetText("FinishAward").text = "完成奖励 " + _data.AwardIntegral;
//		_unfold.GetText("NeedAbilityDesc").text = _data.NeedAbilityDesc;
//		
//		var chooseCardBtn = _unfold.GetButton("ChooseCardBtn");
//		
//	
//		
//		Debug.LogError(_data.UserCards.Count);
//
//		
//		
//		if (_data.UserCards.Count==0) //说明没选卡
//		{
//			chooseCardBtn.gameObject.Show();
//			chooseCardBtn.onClick.AddListener(() =>
//			{								//跳到选卡界面
//				EventDispatcher.TriggerEvent(EventConst.GotoChooseCard,_data);
//			}); 				
//		}
//		else 
//		{
//			chooseCardBtn.gameObject.Hide();
//
//		}
//
//		var difficultyHintBtn = _unfold.GetButton("Hint/DifficultyHintBtn");
//		var frame = _unfold.Find("Hint/Frame");
//		var hideFrame = _unfold.Find("HideFrame");
//		
//		difficultyHintBtn.onClick.AddListener(()=>
//		{
//			frame.gameObject.Show();
//			hideFrame.gameObject.Show();
//			
//			hideFrame.GetButton().onClick.AddListener(() =>
//			{
//				frame.gameObject.Hide();
//				hideFrame.gameObject.Hide();
//			});			
//		});
//
//		
//		var togGroundMask = _unfold.Find("TogGroundMask");
//
//		var musicDiffProgress = GlobalData.TrainingRoomModel.GetMusicDiffProgress();
//		for (int i = 0; i < togGroundMask.childCount; i++)
//		{
//		     var mask =	togGroundMask.GetChild(i).gameObject;
//		     if (mask.name==musicDiffProgress.ToString())
//		     {
//			     mask.Hide();
//		     }
//		     else
//		     {
//			     PointerClickListener.Get(mask).onClick = go =>
//			     {
//				     FlowText.ShowMessage("请先完成前一难度曲目");
//			     };
//		     }
//		}
//				
//	}
//
//
//	private void PlayBtn()
//	{
//		if (_data.UserCards.Count==0)
//		{
//			FlowText.ShowMessage("未选择星缘");
//		}
//		else
//		{
//			int temp = 0;
//			var togGround = _unfold.Find("TogGround");
//			for (int i = 0; i < togGround.childCount; i++)
//			{
//				if (togGround.GetChild(i).GetComponent<Toggle>().isOn)
//				{
//				  temp =int.Parse(togGround.GetChild(i).name);
//				  break;
//				}
//			}
//			
//			EventDispatcher.TriggerEvent(EventConst.StartPlay,_data,temp);			
//		}
//	}
//	
//	public Transform GetCreateParent()
//	{		
//		return _content;
//	}


	
}
