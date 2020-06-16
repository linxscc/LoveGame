using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Core.Events;
using Com.Proto;
using Common;
using DataModel;
using game.main;
using game.tools;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardCollectionView : View 
{
	private Transform _tabBar;
	private LoopVerticalScrollRect _cardList;
	
	private List<UserCardVo> _data;
	
	private Transform _collectedCard;
	private List<UserCardVo> _originalData;
	private Text _collectedCardText;
	private Transform _tips;
	private CardModule.CardViewState _curState=CardModule.CardViewState.MyCard;

	private GameObject _carddetailRedpoint;
	private GameObject _cardpuzzleRedpoint;

	private void Awake()
	{

		transform.Find("TopTab/MyCardToggle").GetComponent<Toggle>().onValueChanged.AddListener(OnMyCardClick);
		transform.Find("TopTab/CardPuzzleToggle").GetComponent<Toggle>().onValueChanged.AddListener(OnCardPuzzleClick);
		transform.Find("TopTab/CardDecompositionToggle").GetComponent<Toggle>().onValueChanged.AddListener(OnCardDecompositionClick);

		_collectedCard = transform.Find("CollectedCard");
		
		_cardList = transform.Find("CollectedCardList").GetComponent<LoopVerticalScrollRect>();
		
		_tabBar = transform.Find("TabBar");
		for (int i = 0; i < _tabBar.childCount; i++)
		{
			Toggle toggle = _tabBar.GetChild(i).GetComponent<Toggle>();
			toggle.onValueChanged.AddListener(OnTabChange);
		}

		_collectedCardText = _collectedCard.Find("Text").GetComponent<Text>();
		_cardList.prefabName = "Card/Prefabs/Collection/CollectedCardItem";
		_cardList.poolSize = 6;
		_cardList.UpdateCallback = ListUpdateCallback;
		_tips = transform.Find("Tips");

		_carddetailRedpoint = transform.Find("TopTab/MyCardToggle/RedPoint").gameObject;
		_cardpuzzleRedpoint = transform.Find("TopTab/CardPuzzleToggle/RedPoint").gameObject;
	}

	public void ChangeViewStae(CardModule.CardViewState state)
	{
		_curState = state;
		_tips.gameObject.SetActive(state == CardModule.CardViewState.MyCard&&_data.Count==0);
		if (state == CardModule.CardViewState.MyCard)
		{
			_cardList.gameObject.Show();
		}
		else
		{
			_cardList.gameObject.Hide();
		}
		_collectedCard.gameObject.SetActive(state != CardModule.CardViewState.Resolve);
	}

	public void JumpToResolveState()
	{
		transform.Find("TopTab/MyCardToggle").GetComponent<Toggle>().isOn = false;
		transform.Find("TopTab/CardPuzzleToggle").GetComponent<Toggle>().isOn = false;
		transform.Find("TopTab/CardDecompositionToggle").GetComponent<Toggle>().isOn = true;
	}
	
	public void SetMyCardData(List<UserCardVo> cards, PlayerPB filter = PlayerPB.None,bool needtorefillcells=true)
	{
		if (needtorefillcells)
		{
			_cardList.RefillCells();
		}

		_originalData = cards;

		if (filter != PlayerPB.None)
		{
			_data = cards.FindAll(match => { return match.CardVo.Player == filter; });
		}
		else
		{
			_data = cards;
		}
		
		_tips.gameObject.SetActive(_curState == CardModule.CardViewState.MyCard&&_data.Count==0);
		//Debug.LogError(_data.Count);
		_cardList.totalCount = _data.Count;
		_cardList.RefreshCells();
		
		_carddetailRedpoint.SetActive(GlobalData.CardModel.ShowCardDetailRedPoint);
		_cardpuzzleRedpoint.SetActive(GlobalData.CardModel.ShowCardPuzzleRedpoint);
		
	}

	public void SetCardNum(int totalNum)
	{
		int percent = (int)(((float)_originalData.Count / totalNum)*100);
		if (_originalData.Count>0&&percent==0)
		{
			percent = 1;
		}

		_collectedCardText.text =
			I18NManager.Get("Card_CollectNum", percent,
				_originalData.Count); //"星缘收集： " + percent + "%         拥有星缘：" + _originalData.Count + "张";

	}
	
	private void OnEnable()
	{
		if(_cardList != null)
			_cardList.RefreshCells();
	}

	private void OnCardDecompositionClick(bool isOn)
	{
		if(isOn)
			SendMessage(new Message(MessageConst.MODULE_CARD_COLLECTION_CHANEG_VIEW, CardModule.CardViewState.Resolve));
	}

	private void OnCardPuzzleClick(bool isOn)
	{
		if (isOn)
			SendMessage(new Message(MessageConst.MODULE_CARD_COLLECTION_CHANEG_VIEW, CardModule.CardViewState.Puzzle));
	}

	private void OnMyCardClick(bool isOn)
	{
		if (isOn)
			SendMessage(new Message(MessageConst.MODULE_CARD_COLLECTION_CHANEG_VIEW, CardModule.CardViewState.MyCard));
	}

	public void ListUpdateCallback(GameObject go, int index)
	{		
		go.GetComponent<CollectedCardItem>().SetData(_data[index]);
	}

	private void OnTabChange(bool isOn)
	{
		if(isOn == false)
			return;
		
		string name = EventSystem.current.currentSelectedGameObject.name;
		Debug.Log("OnTabChange===>" + name);

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
		SendMessage(new Message(MessageConst.MODULE_CARD_TABBAR_SELECT_CHANGE, pb));
	}

	public void ChangeTabBar(PlayerPB pb,bool needtorefill=true)
	{
		SetMyCardData(_originalData, pb,needtorefill);
	}
}


