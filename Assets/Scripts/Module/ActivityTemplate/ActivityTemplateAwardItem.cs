using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module;
using Com.Proto;
using DataModel;
using game.main;
using game.tools;
using GalaAccount.Scripts.Framework.Utils;
using UnityEngine;
using UnityEngine.UI;

public class ActivityTemplateAwardItem : MonoBehaviour
{


	private Transform _smallFrame;
	private Transform _name;
	private Transform _num;
	
	private void Awake()
	{
		_name = transform.Find("NameBg/Text");
		_num = transform.Find("Num");
		_smallFrame = transform.Find("SmallFrame");

	}
	
	
	public void SetData(AwardPB pb, bool isShowNum)
	{
		RewardVo vo =new RewardVo(pb);
		_name.GetText().text = vo.Name;

		if (isShowNum)
		{
			_num.gameObject.Show();
			_num.GetText().text = "x"+vo.Num;
		}
		else
		{
			_num.gameObject.Hide();
		}
		
		_smallFrame.GetComponent<Frame>().SetData(vo,ModuleConfig.MODULE_ACTIVITYTEMPLATE);		
	}

	
}
