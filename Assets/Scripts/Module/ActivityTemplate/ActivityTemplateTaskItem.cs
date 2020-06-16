using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core.Events;
using Common;
using UnityEngine;
using UnityEngine.UI;

public class ActivityTemplateTaskItem : MonoBehaviour
{


	private Text _descTxt;
	private Text _progressTxt;
	private Button _gotoBtn;


	private ActivityTemplateTaskVo _data;
	private void Awake()
	{
		_descTxt = transform.GetText("Desc");
		_progressTxt = transform.GetText("Progress");
		_gotoBtn = transform.GetButton("GotoBtn");
		
		_gotoBtn.onClick.AddListener(Goto);
			
		
	}

	private void Goto()
	{
		EventDispatcher.TriggerEvent(EventConst.ActivityTemplateJumpTo,_data.JumpTo);
	}

	public void SetData(ActivityTemplateTaskVo vo)
	{
		_data = vo;
		_descTxt.text = _data.Desc;

		if (_data.IsDrop)
			_progressTxt.text = I18NManager.Get("ActivityTemplate_ActivityTemplateTaskProgress",  _data.CurNum,_data.Max);
	
	}
}
