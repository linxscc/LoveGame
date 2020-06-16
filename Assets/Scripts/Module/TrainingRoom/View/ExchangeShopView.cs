using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core;
using Com.Proto;
using game.main;
using UnityEngine;
using UnityEngine.UI;

public class ExchangeShopView : View
{
	private Transform _parent;
	private Text _desc;
	private Button _refreshBtn;
	private Text _costNum;
	private Transform _costIcon;

	private RefreshDataPB _pb;
	
	private void Awake()
	{
		_parent = transform.Find("Content/ItemContent/Content");
		_desc = transform.GetText("Desc");
		_refreshBtn = transform.GetButton("RefreshBtn/Button");
		_costNum = transform.GetText("RefreshBtn/CostNum");
		_costIcon = transform.Find("RefreshBtn/GemIcon");
		_refreshBtn.onClick.AddListener(RefreshBtn);
	}


	public void SetData(List<ExchangeVO> list,RefreshDataPB pb)
	{
		CreateExchangeItem(list);

		_pb = pb;
		
		if (pb==null)
		{
			_costNum.text = string.Empty;
			_costIcon.gameObject.Hide();
		}
		else
		{
			_costNum.text = "x"+pb.ResourceNum;
		}
	}

	private void CreateExchangeItem(List<ExchangeVO> list)
	{
		for (int i = _parent.childCount - 1; i > 0; i--)
		{
			DestroyImmediate(_parent.GetChild(i).gameObject);
		}
		
		var prefab = GetPrefab("TrainingRoom/Prefabs/ExchangeShopItem");

		Transform bg = transform.Find("Content/ItemContent/Content/Bg"); 
		int bgCount = list.Count / 3 + 1;
		if (bg.childCount < bgCount )
		{
			for (int i = 0; i < bgCount-bg.childCount; i++)
			{
				Instantiate(bg.GetChild(0), bg, false);
			}
		}
		else if(bg.childCount > bgCount)
		{
			for (int i = bg.childCount - bgCount - 1; i > bgCount; i--)
			{
				DestroyImmediate(_parent.GetChild(i).gameObject);
			}
		}
		foreach (var t in list)
		{
			var item = Instantiate(prefab,_parent,false);
			item.transform.localScale = Vector3.one;
			item.GetComponent<ExchangeShopItem>().SetData(t);
			item.name = t.ShopId.ToString();
		}
	}


	public void UpdateBuyLaterExchangeItemState(ExchangeVO vo)
	{
		for (int i = 0; i < _parent.childCount; i++)
		{
			if (_parent.GetChild(i).name ==vo.ShopId.ToString())
			{
				_parent.GetChild(i).gameObject.GetComponent<ExchangeShopItem>().SetData(vo);
				break;
			}
		}
	}

	private void RefreshBtn()
	{
		if (_pb==null)
		{
			FlowText.ShowMessage("今日无剩余刷新次数");
		}
		else
		{
			//发送打开确认弹窗
			SendMessage(new Message(MessageConst.CMD_TRAININGROOM_EXCHANGESHOP_OPEN_REFRESH_NOTARIZE_WINDOW));
		}
	}
}
