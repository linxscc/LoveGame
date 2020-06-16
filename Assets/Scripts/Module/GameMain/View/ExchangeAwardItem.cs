using System.Collections;
using System.Collections.Generic;
using Com.Proto;
using DataModel;
using game.main;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Assets.Scripts.Framework.GalaSports.Service;
public class ExchangeAwardItem : MonoBehaviour
{


	
	private Text _num;
	private Text _name;

	private RewardVo _data;
	private Frame _smallFrame;
	
	private void Awake()
	{
		
		_num = transform.GetText("Num");
		_name = transform.GetText("NameBg/Text");
		_smallFrame = transform.Find("SmallFrame").GetComponent<Frame>();
	}


	public void SetData(RewardVo vo)
	{
		_data = vo;

		_num.text = "X"+vo.Num;
		_name.text = vo.Name;
		_smallFrame.SetData(vo);
	}


	
}
