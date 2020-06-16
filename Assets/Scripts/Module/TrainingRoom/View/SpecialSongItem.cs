using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core.Events;
using Common;
using DataModel;
using game.main;
using game.tools;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Framework.GalaSports.Core;
using DG.Tweening;


public class SpecialSongItem : View
{
	private const float UnfoldHeight = 535f;
	private const float ShrinkHeight = 245f;

	private Transform _shrinkContent;
	private Transform _unfoldContent;
	private Transform _toggleContent;
	
	private Text _activityName;
	private Text _awardTxt;

	private Button _shrinkBtn;
	// private Button _unfoldBtn;
	private Button _hintBtn;
	private RectTransform _rect;
	
	private int _activityId;

	public int ActivityId => _activityId;

	public bool IsUnfold;
	private Button _playBtn;

	private void Awake()
	{
		_shrinkContent = transform.Find("Shrink");
		_unfoldContent = transform.Find("Unfold");
		_toggleContent = _unfoldContent.Find("ToggleContent");
		
		_activityName = transform.GetText("ActivityName");
		_awardTxt = transform.GetText("AwardText");

		_shrinkBtn = _unfoldContent.GetButton("ShrinkBtn");
		// _unfoldBtn = _shrinkContent.GetButton("UnfoldBtn");
		_hintBtn = _unfoldContent.GetButton("HintBtn");

		_rect = transform.GetRectTransform();
		
		_shrinkBtn.onClick.AddListener(ShrinkBtn);
		 // _unfoldBtn.onClick.AddListener(UnfoldBtn);
		 _hintBtn.onClick.AddListener(HintBtn);
		 
		 _playBtn = _unfoldContent.GetButton("PlayBtn/OnClick");
		 _playBtn.onClick.AddListener(OnPlayBtn);
		 
		 PointerClickListener.Get(_shrinkContent.gameObject).onClick = UnfoldBtn;
	}

	private void OnPlayBtn()
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
		SendMessage(new Message(MessageConst.MODULE_TRAININGROOM_ENTRY_GAME, Message.MessageReciverType.CONTROLLER, -diff));
	}

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
	
	

	//提示按钮
	private void HintBtn()
	{
		var hideFrame = _unfoldContent.Find("HideFrame");
		hideFrame.gameObject.Show();
		
		var imageRect = hideFrame.GetRectTransform("image");
		var tween= imageRect.DOSizeDelta(new Vector2(500,200),0.3f);
		tween.onComplete = () =>
		{
			imageRect.Find("Text").gameObject.Show();
			PointerClickListener.Get(hideFrame.gameObject).onClick= go =>
			{
				imageRect.Find("Text").gameObject.Hide();
				imageRect.sizeDelta = new Vector2(0,200);
				hideFrame.gameObject.Hide();
			};
		};
		
	}

	//展开
	private void UnfoldBtn(GameObject go)
	{
		IsUnfold = true;
		_rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical,UnfoldHeight);
		_shrinkContent.gameObject.Hide();
		_unfoldContent.gameObject.Show();
		SendMessage(new Message( MessageConst.CMD_TRAININGROOM_ONCLICK_UNFOLD_BTN,_activityId));	
	}

	//收缩
	private void ShrinkBtn()
	{
		IsUnfold = false;
		_rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical,ShrinkHeight);
		_shrinkContent.gameObject.Show();
		_unfoldContent.gameObject.Hide();
		SendMessage(new Message( MessageConst.CMD_TRAININGROOM_ONCLICK_UNFOLD_BTN,_activityId));	
	}

	
	public void SetShrinkState()
	{
		IsUnfold = false;
		_rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical,ShrinkHeight);
		_shrinkContent.gameObject.Show();
		_unfoldContent.gameObject.Hide();
		
		_unfoldContent.Find("HideFrame").gameObject.Hide();
		_unfoldContent.GetRectTransform("HideFrame/image").sizeDelta = new Vector2(0,310);
		_unfoldContent.Find("HideFrame/image/Text").gameObject.Hide();
	}

	public void SetData(int activityId)
	{
		_activityId = activityId;
		_activityName.text = "演奏练习";
		_awardTxt.text = "无奖励";

		SetDifficultyOpenLevel();
	}



//
//
//	public void SetCurHeight()
//	{
//		if (IsUnfold)
//		{
//			
//			_unfold.gameObject.Show();
//			_shrink.gameObject.Hide();
//			_rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical,700);
//			SetUnfold();
//		}
//		else
//		{
//			_shrink.gameObject.Show();
//			_unfold.gameObject.Hide();
//			_rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical,250);
//			SetShrink();
//		}
//	}
//	
//	private void SetUnfold()
//	{						
//		_unfold.GetText("ActivityName").text = "演奏练习";
//		_unfold.GetText("FinishAward").text = "无奖励";
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
//		var togGround = _unfold.Find("TogGround");
//		var playBtn = _unfold.GetButton("PlayBtn");		
//		playBtn.onClick.AddListener(() =>
//		{
//			for (int i = 0; i < togGround.childCount; i++)
//			{
//				var tog = togGround.GetChild(i).gameObject;
//				if (tog.GetComponent<Toggle>().isOn)
//				{
//				    var temp = int.Parse(tog.name);
//					//触发事件，把难度传出去					
//					break;					
//				}								
//			}
//		});
//		
//		var togGroundMask = _unfold.Find("TogGroundMask");
//		var musicDiffProgress = GlobalData.TrainingRoomModel.GetMusicDiffProgress();
//		for (int i = 0; i < togGroundMask.childCount; i++)
//		{
//			var mask =	togGroundMask.GetChild(i).gameObject;
//			if (mask.name==musicDiffProgress.ToString())
//			{
//				mask.Hide();
//			}
//			else
//			{
//				PointerClickListener.Get(mask).onClick = go =>
//				{
//					FlowText.ShowMessage("请先完成前一难度曲目");
//				};
//			}
//		}
//	}
	
}
