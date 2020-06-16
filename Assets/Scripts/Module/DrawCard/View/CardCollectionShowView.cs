using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class CardCollectionShowView : View
{

    private Transform _cardParent;
    private Transform _showBtn;


    private LoopVerticalScrollRect _cardShowList;
    private LoopVerticalScrollRect _cardFunShowList;
    //本地更新数据  用于C处的赋值操作！！！
    private List<ShowCardModel> _data1;
    private List<ShowCardModel> _allData;
    private DrawEventPB _showDrawEvent;

    private bool _isFinishedOne;
    private Transform _tabBar;
    private void Awake()
    {
        _isFinishedOne = false;
        _showBtn = transform.Find("ShowBg/ShowBtn");


        //这个是总体的那个
        _cardShowList = transform.Find("ShowBg/VerticalScroll_Grid").GetComponent<LoopVerticalScrollRect>();
        //_cardShowList.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0,-87);
        _cardShowList.UpdateCallback = ListUpdataCallback;
        _cardShowList.poolSize = 20;

        //这个是总体的那个
        _cardFunShowList = transform.Find("ShowBg/FunsVerticalScroll_Grid").GetComponent<LoopVerticalScrollRect>();
        //_cardShowList.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0,-87);
        _cardFunShowList.UpdateCallback = FunListUpdataCallback;
        _cardFunShowList.poolSize = 20;

        _tabBar = transform.Find("TabBar");
        for (int i = 0; i < _tabBar.childCount; i++)
        {
            Toggle toggle = _tabBar.GetChild(i).GetComponent<Toggle>();
            toggle.onValueChanged.AddListener(OnTabChange);
        }
    }

    private void OnTabChange(bool isOn)
    {
        if (isOn == false)
            return;

        string name = EventSystem.current.currentSelectedGameObject.name;
        Debug.Log("OnTabChange===>" + name);

        PlayerPB pb = PlayerPB.None;
        bool isFans = false;
        switch (name)
        {
            case "All":
                pb = PlayerPB.None;
                break;
            case "Fang":
                pb = PlayerPB.ChiYu;
                break;
            case "Tang":
                pb = PlayerPB.YanJi;
                break;
            case "Lin":
                pb = PlayerPB.TangYiChen;
                break;
            case "Li":
                pb = PlayerPB.QinYuZhe;
                break;
            case "Fans":
                pb = PlayerPB.None;
                isFans = true;
      
                break;
        }
        SetListData(pb, isFans);
    }


    public void BackDrawCard()
    {
        SendMessage(new Message(MessageConst.MODULE_VIEW_BACK_DRAWCARD));
    }

    public void SetListData(PlayerPB pb = PlayerPB.None, bool isFun = false)
    {

        if (_allData == null)
        {
            return;
        }

        if (isFun)
        {
  
            _data1 = _allData.FindAll(match => { return match.Resource == SortResouce.Fans && match.DrawEvent == _showDrawEvent; });
            _cardFunShowList.RefillCells();
            _cardFunShowList.totalCount = _data1.Count;

            _cardFunShowList.gameObject.Show();
            _cardShowList.gameObject.Hide();
            _cardFunShowList.RefreshCells();
        }
        else
        {
            if (pb != PlayerPB.None)
            {
                _data1 = _allData.FindAll(match => { return match.Resource != SortResouce.Fans && match.Player == pb && match.DrawEvent == _showDrawEvent; });
            }
            else
            {
                _data1 = _allData.FindAll(match => { return match.Resource != SortResouce.Fans && match.DrawEvent == _showDrawEvent; });
            }
            _cardShowList.RefillCells();//解决更新列表元素，index超出范围的问题

            _cardShowList.totalCount = _data1.Count;

            _cardFunShowList.gameObject.Hide();
            _cardShowList.gameObject.Show();
            _cardShowList.RefreshCells();
        }
    }

    private void ListUpdataCallback(GameObject obj, int index)
    {
        if (_data1.Count <= index)
        {
            return;
        }
        obj.GetComponent<CardItem>().SetData(_data1[index]);
        obj.transform.localScale = new Vector3(1, 1, 0);
    }
    private void FunListUpdataCallback(GameObject obj, int index)
    {
        if (_data1.Count <= index)
        {
            return;
        }
        obj.GetComponent<CardFunItem>().SetData(_data1[index]);
        obj.transform.localScale = new Vector3(1, 1, 0);
    }


    public void SetData(DrawEventPB drawEvent, List<ShowCardModel> data)
    {
        if (data == null)
        {
            return;
        }
        _allData = _data1 = data;
        _showDrawEvent = drawEvent;
        // _fansCard.gameObject.SetActive(_showDrawEvent == DrawEventPB.GoldBase);
        bool isShowFans = _showDrawEvent == DrawEventPB.GoldBase;
        _tabBar.Find("Fans").gameObject.SetActive(isShowFans);
        _tabBar.GetComponent<GridLayoutGroup>().cellSize = isShowFans ? new Vector2(180, 100) : new Vector2(216, 100);
        SetListData(PlayerPB.None);

    }
}
