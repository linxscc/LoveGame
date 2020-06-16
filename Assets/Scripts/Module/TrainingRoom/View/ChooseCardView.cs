using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Module.Recollection.View;
using DataModel;
using game.main;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ChooseCardView : View {

	private LoopVerticalScrollRect _cardList;
	private List<TrainingRoomCardVo> _data;
	private List<TrainingRoomCardVo> _originalData;	
	private Transform _tabBar;
	private Text _hintTxt;

	private Button _chooseBtn;
	private Text _chooseTxt;
	private void Awake()
	{
		_cardList = transform.Find("CollectedCardList").GetComponent<LoopVerticalScrollRect>();
		_cardList.prefabName = "TrainingRoom/Prefabs/TrainingRoomCardItem";
		_cardList.poolSize = 12;
		_cardList.UpdateCallback = ListUpdateCallback;
		
		_tabBar = transform.Find("TabBar");
		
		for (int i = 1; i < _tabBar.childCount; i++)
		{
			Toggle toggle = _tabBar.GetChild(i).GetComponent<Toggle>();
			toggle.onValueChanged.AddListener(OnTabChange);
		}

		_hintTxt = transform.GetText("TabBar/Title/Text");
		_chooseBtn = transform.GetButton("Choose/Button");
		_chooseTxt = transform.GetText("Choose/Button/Text");
		_chooseBtn.onClick.AddListener(ChooseBtn);
	}

	private void ChooseBtn()
	{
		var list = GlobalData.TrainingRoomModel.ChooseCards;
		var curNeedCardNum = GlobalData.TrainingRoomModel.CurMusicGame.NeedStarNum;
		if (list.Count<curNeedCardNum)
		{
			FlowText.ShowMessage("未完成选择");
		}
		else
		{
			SendMessage(new Message(MessageConst.MODULE_TRAININGROOM_CHOOSE_NUM_ENOUGH));
		}
	}


	public void SetChooseBtnTxt(int num)
	{
		var curNeedCardNum = GlobalData.TrainingRoomModel.CurMusicGame.NeedStarNum;
		if (num==curNeedCardNum)
		{
			_chooseTxt.text = "确定选择";
			return;
		}
		_chooseTxt.text = "已选择："+num;
	}
	
	public void SetData(UserMusicGameVO vo)
	{		
		_hintTxt.text = vo.NeedAbilityDesc;		
	}
	
	public void SetMyCardData(List<TrainingRoomCardVo> cards, PlayerPB filter = PlayerPB.None )
	{
		
		_cardList.RefillCells();
		_originalData = cards;

		if (filter != PlayerPB.None)
		{			
			_data = cards.FindAll(match => { return match.UserCardVo.CardVo.Player == filter; });			
		}
		else
		{
			Toggle toggle = _tabBar.GetChild(1).GetComponent<Toggle>();
			if(toggle.isOn != true)
				toggle.isOn = true;
			
			_data = cards;
		}
		
		_cardList.totalCount = _data.Count;
		_cardList.RefreshCells();
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
		go.GetComponent<TrainingRoomCardItem>().SetData(_data[index]);
	}
}
