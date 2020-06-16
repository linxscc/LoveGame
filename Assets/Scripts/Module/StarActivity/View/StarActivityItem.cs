using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core.Events;
using Assets.Scripts.Framework.GalaSports.Service;
using Com.Proto;
using Common;
using DataModel;
using game.main;
using game.tools;
using GalaAccount.Scripts.Framework.Utils;
using GalaAccountSystem;
using UnityEngine;
using UnityEngine.UI;

public class StarActivityItem : MonoBehaviour
{

	private Text _titleNameText;
	private Text _progressText;
	private Text _numText;

	private RawImage _rewardIconImage;
	

	private Transform _gotoTran;
	private Transform _getTran;
	private Transform _alreadyGetTran;

	private Button _gotoBtn;  //前往Btn;
	private Button _getBtn;  //领取Btn
	private Button _onClick;

	private Transform _preview; //预览
		
	
	private ProgressBar _progressBar;

	private UserMissionVo _data;
	private MissionRulePB _missionPb;
	
	private int  _resourceId;
	private ResourcePB _resourcePb; 
	
	
	private void Awake()
	{

		_preview = transform.Find("Preview");
		_titleNameText = transform.GetText("TitleName");
		_progressText = transform.GetText("ProgressText");
		_numText = transform.GetText("Reward/Item/Num");
		_rewardIconImage =transform.GetRawImage("Reward/Item");
		_onClick = transform.GetButton("Reward/Item");
		_gotoTran = transform.Find("GotoBtn");
		_getTran = transform.Find("GetBtn");
		_alreadyGetTran = transform.Find("AlreadyGet");

		_gotoBtn = _gotoTran.GetButton();
		_getBtn = _getTran.GetButton();

		_gotoBtn.onClick.AddListener(GotoBtn);
		_getBtn.onClick.AddListener(GetBtn);
		
		_progressBar = transform.Find("ExpSlider/ProgressBar").GetComponent<ProgressBar>();
		
		_onClick.onClick.AddListener((() =>
		{
			var desc = ClientData.GetItemDescById(_resourceId,_resourcePb);
			if (desc!=null)
			{
				FlowText.ShowMessage(desc.ItemDesc); 
			
			}
			
		}));
		
		


	}

	private void GetBtn()
	{
		//领取奖励
		EventDispatcher.TriggerEvent(EventConst.StarActivityGetAward,_data);
	}

	private void GotoBtn()
	{				
		//前往任务
		EventDispatcher.TriggerEvent(EventConst.StarActivityGotoTask,_missionPb.JumpTo);
	}


	public void SetData(UserMissionVo vo,MissionModel missionModel)
	{
		_data = vo;
		_missionPb =missionModel.GetMissionById(vo.MissionId);
		_titleNameText.text = _missionPb.MissionDesc;       //任务名称
		
	
		
		

		if (vo.IsPreview)
		{
			_preview.gameObject.Show();
			_gotoTran.gameObject.Hide();
			_getTran.gameObject.Hide();
			_alreadyGetTran.gameObject.Hide();

		}
		else
		{
			_preview.gameObject.Hide();
			SetState();
		}
		
		SetProgress();
		SetAwardData();
	}


	/// <summary>
	/// 设置领取状态
	/// </summary>
	private void SetState()
	{
		switch (_data.Status)
		{
			case MissionStatusPB.StatusUnfinished:
				_gotoTran.gameObject.Show();
				_getTran.gameObject.Hide();
				_alreadyGetTran.gameObject.Hide();
			
				break;
			case MissionStatusPB.StatusUnclaimed:
				_gotoTran.gameObject.Hide();
				_getTran.gameObject.Show();
				_alreadyGetTran.gameObject.Hide();
				
				break;
			case MissionStatusPB.StatusBeRewardedWith:
				_gotoTran.gameObject.Hide();
				_getTran.gameObject.Hide();
				_alreadyGetTran.gameObject.Show();
				break;			
		}
	}

	/// <summary>
	/// 设置进度
	/// </summary>
	private void SetProgress()
	{
		_progressText.text = (_data.Progress>_data.Finish?_data.Finish:_data.Progress) + "/" + _data.Finish;
		_progressBar.DeltaX = 0;
		_progressBar.Progress = (int) ((float)_data.Progress / _data.Finish*100);
		
	}


	/// <summary>
	/// 设置奖励信息(数量，图片，点击描述)
	/// </summary>
	private void SetAwardData()
	{
		foreach (var t in _missionPb.Award)
		{
			RewardVo rewardVo=new RewardVo(t);
			_numText.text =rewardVo.Num.ToString(); 
			_rewardIconImage.texture =ResourceManager.Load<Texture>(rewardVo.IconPath);
			_resourcePb = rewardVo.Resource;

			switch (rewardVo.Resource)
			{
				case ResourcePB.Power:
					_resourceId = 20001;
					break;
				case ResourcePB.Gem:
					_resourceId = 30001;
					break;
				case ResourcePB.Gold:
					_resourceId = 10001;
					break;
				default:
					_resourceId = t.ResourceId;
					break;
			}
		}
	}
	
}
