using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Core.Events;
using Assets.Scripts.Module.Recollection.View;
using Common;
using DataModel;
using game.main;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardListView : View 
{
	private LoopVerticalScrollRect _cardList;
	
	private List<UserCardVo> _data;
	private Text _collectedCardText;
	private List<UserCardVo> _originalData;
	private Transform _tabBar;

	private void Awake()
	{
		_cardList = transform.Find("CollectedCardList").GetComponent<LoopVerticalScrollRect>();
		
		_cardList.prefabName = "Recollection/Prefabs/RecollectionCardItem";
		_cardList.poolSize = 12;
		_cardList.UpdateCallback = ListUpdateCallback;

		_collectedCardText = transform.Find("CollectedCard/Text").GetComponent<Text>();
		
		_tabBar = transform.Find("TabBar");
		for (int i = 0; i < _tabBar.childCount; i++)
		{
			Toggle toggle = _tabBar.GetChild(i).GetComponent<Toggle>();
			toggle.onValueChanged.AddListener(OnTabChange);
		}
	}

	public void SetMyCardData(List<UserCardVo> cards, PlayerPB filter = PlayerPB.None)
	{
		
		_cardList.RefillCells();
		_originalData = cards;

		if (filter != PlayerPB.None)
		{
			_data = cards.FindAll(match => { return match.CardVo.Player == filter; });
		}
		else
		{
			Toggle toggle = _tabBar.GetChild(0).GetComponent<Toggle>();
			if(toggle.isOn != true)
				toggle.isOn = true;
			
			_data = cards;
		}
		
		_cardList.totalCount = _data.Count;
		_cardList.RefreshCells();
	}

	public void SetTotalNum(int totalNum)
	{
		int percent = (int)(((float)_originalData.Count / totalNum)*100);
        //_collectedCardText.text = "星缘收集： " + percent + "%         拥有星缘：" + _originalData.Count + "张";
        _collectedCardText.text = I18NManager.Get("Recollection_CardListViewCollectedCardText", percent, _originalData.Count);


    }
	
	private void OnTabChange(bool isOn)
	{
		if(isOn == false || EventSystem.current.currentSelectedGameObject == null)
			return;
		
		string name = EventSystem.current.currentSelectedGameObject.name;

		PlayerPB pb = PlayerPB.None;;
		switch (name)
		{
			case "All" :
				pb = PlayerPB.None;
				break;
			case "Fang" :
				pb = PlayerPB.ChiYu;
				break;
			case "Tang" :
				pb = PlayerPB.YanJi;
				break;
			case "Lin" :
				pb = PlayerPB.TangYiChen;
				break;
			case "Li" :
				pb = PlayerPB.QinYuZhe;
				break;
		}

		SetMyCardData(_originalData, pb);
	}

	private void ListUpdateCallback(GameObject go, int index)
	{
		go.GetComponent<RecollectionCardItem>().SetData(_data[index]);
	}
}


