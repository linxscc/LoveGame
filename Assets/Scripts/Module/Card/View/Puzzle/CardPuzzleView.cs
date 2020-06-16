using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core;
using game.main;
using UnityEngine;
using UnityEngine.UI;

public class CardPuzzleView : View 
{
	private LoopVerticalScrollRect _puzzleList;
	private List<CardPuzzleVo> _puzzleData;
	private List<CardPuzzleVo> _originalData;
	private PlayerPB _currentTab = PlayerPB.None;
	private Text _tips;

	private void Awake()
	{
		_tips = transform.Find("Tips").GetComponent<Text>();
		_puzzleList = transform.Find("List").GetComponent<LoopVerticalScrollRect>();
		_puzzleList.prefabName = "Card/Prefabs/Puzzle/CardPuzzleItem";
		_puzzleList.poolSize = 20;
		_puzzleList.totalCount = 0;
	}

	public void ShowView(PlayerPB currentTab)
	{
		_currentTab = currentTab;
		if(_puzzleData == null)
			SendMessage(new Message(MessageConst.CMD_CARD_COLLECTION_GET_USER_PUZZLUE));
	}

	public void SetData(List<CardPuzzleVo> data)
	{
		_puzzleList.RefillCells();
		_originalData = data;
		if (_currentTab != PlayerPB.None)
		{
			_puzzleData = data.FindAll(match => { return match.Player == _currentTab&&match.Num>0; });
			_puzzleList.verticalNormalizedPosition = -0.5f;
		}
		else
		{
			//后端会把数量为0的东西也发给我！
			_puzzleData = data.FindAll(match => { return match.Num > 0;});
		}

		if (_puzzleData.Count==0)
		{
			_tips.gameObject.Show();
		}
		else
		{
			_tips.gameObject.Hide();
		}
		
		_puzzleList.UpdateCallback = PuzzleListUpdateCallback;
		_puzzleList.totalCount = _puzzleData.Count;
		_puzzleList.RefreshCells();
	}
	
	private void PuzzleListUpdateCallback(GameObject go, int index)
	{
		if (index>_puzzleData.Count-1)
		{
			Destroy(go);
			return;
		}
		go.GetComponent<CardPuzzleItem>().SetData(_puzzleData[index]);
	}

	public void ChangeTabBar(PlayerPB pb)
	{
		if (_originalData == null)
			return;

		_currentTab = pb;
		_originalData.Sort();
		SetData(_originalData);
	}

	
}


