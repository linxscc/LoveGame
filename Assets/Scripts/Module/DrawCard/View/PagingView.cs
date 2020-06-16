using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core;
using UnityEngine;
using  UnityEngine.UI;

public class PagingView : View
{


	private Button _backBtn;
	private Button _starBtn;
	private Text _qualifyCountTxt;
	private Button _pagingBtn;
	private Button _goldTenBtn;
	private Button _goldOnceBtn;
	
	private void Awake()
	{

		//合并主页面··········
		_backBtn = transform.Find("Container/BackBtn").GetComponent<Button>();
		_starBtn = transform.Find("Container/StarBtn").GetComponent<Button>();
		_pagingBtn =transform.Find("Container/PagingBtn").GetComponent<Button>();
		_qualifyCountTxt = transform.Find("Container/StarBtn/QualifyCountTxt").GetComponent<Text>();
		_goldTenBtn = transform.Find("Container/GoldTenBtn").GetComponent<Button>();
		_goldOnceBtn = transform.Find("Container/GoldOnceBtn").GetComponent<Button>();

	}
	
	//让逻辑层去调用
    public override void Hide()
    {
    	base.Hide();
  		gameObject.SetActive(false);
	    
    }

	public override void Show(float delay = 0)
	{
		base.Show(delay);
		gameObject.SetActive(true);
	}
	
}
