using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class TabSelectAnimBtnData
{
    public string path = "";
    public string nameText = "";
    public bool isAlwayShow = true;
    public long startTime = 0;
    public long endTime = 0;
    public bool lockState = false;
    //public System.Action<bool> onValueChange;
}


public class TabSelectAnimBtnWidget : MonoBehaviour
{
    private RectTransform _grid;
    //private RectTransform[] _tabBtnObjs;
    //private Toggle[] _tabBtns;
    //private Text[] _tabLabels;

    //private int _btnCount = 0;
    //private int _curIndex = -1;
    //private int _lastIndex = -1;

    private Vector2 _cellSize;
    private Vector2 _space;

    private Action<string> _onTabBtnSelect;


    private List<TabSelectAnimBtnData> _tabDatas;
    
    private Dictionary<string, TabSelectAnimBtnItem> _items;
    private List<string> _showItems;

    private string _curTab = null;
    private string _lastTab = null;
    private int _tabCount;


    private TimerHandler _countDown = null;

    private void Awake()
    {
        _grid = transform.Find("Grid").GetComponent<RectTransform>();
        GridLayoutGroup glg = _grid.GetComponent<GridLayoutGroup>();
        _cellSize = glg.cellSize;
        _space = glg.spacing;

        //int count = _grid.childCount;
        //_btnCount = count;
        //Transform childObj;
        //_tabBtnObjs = new RectTransform[count];
        //_tabBtns = new Toggle[count];
        //_tabLabels = new Text[count];
        //for (int i = 0;i < count; ++i)
        //{
        //    childObj = _grid.Find((i + 1).ToString());
        //    _tabBtnObjs[i] = childObj.GetComponent<RectTransform>();
        //    _tabBtns[i] = childObj.GetComponent<Toggle>();
        //    _tabLabels[i] = childObj.Find("Label").GetComponent<Text>();
        //    int index = i;
        //    _tabBtns[i].onValueChanged.AddListener((state)=>
        //    {
        //        OnTabBtnValueChange(index, state);
        //    });
        //}


        _items = new Dictionary<string, TabSelectAnimBtnItem>();
        _showItems = new List<string>();
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Init(Action<string> onTabBtnSelect = null)
    {
        _onTabBtnSelect = onTabBtnSelect;
    }

    public void SetData(List<TabSelectAnimBtnData> datas)
    {
        _items.Clear();
        for(int i = 0;i < datas.Count; ++i)
        {
            TabSelectAnimBtnItem item = _grid.Find(datas[i].path).GetComponent<TabSelectAnimBtnItem>();
            item.Init(datas[i], OnTabBtnValueChange);
            item.onSelectNextTab = OnSelectNextTab;
            item.onSelectPreTab = OnSelectPreTab;
            _items.Add(datas[i].path, item);
        }
        CheckShowItems();
    }

    private void CheckShowItems()
    {
        long nextStartTime = 0;
        TabSelectAnimBtnData data;
        _showItems.Clear();
        long curTime = ClientTimer.Instance.GetCurrentTimeStamp();
        _tabCount = 0;
        foreach (KeyValuePair<string, TabSelectAnimBtnItem> item in _items)
        {
            data = item.Value.data;
            if (data.isAlwayShow)
            {
                _showItems.Add(item.Key);
                _tabCount++;
            }
            else
            {
                if(curTime >= data.startTime && curTime < data.endTime)
                {
                    _showItems.Add(item.Key);
                    _tabCount++;
                    GetTabBtn(item.Key).SetActive(true);
                }
                else
                {
                    GetTabBtn(item.Key).SetActive(false);
                }
            }

            if(data.startTime > 0 && data.startTime > curTime)
            {
                if (nextStartTime == 0 || data.startTime < nextStartTime)
                    nextStartTime = data.startTime;
            }

        }

        if (_countDown != null)
        {
            ClientTimer.Instance.RemoveCountDown(_countDown);
            _countDown = null;
        }
        if (nextStartTime != 0)
        {
            _countDown = ClientTimer.Instance.AddCountDown("TabSelectAnimBtnWidget", nextStartTime, 1, null, OnNextStartTime);
        }
    }

    private void OnNextStartTime()
    {
        CheckShowItems();
    }


    private void OnSelectNextTab()
    {
        string tab = _curTab;
        int len = _showItems.Count;
        int index = _showItems.IndexOf(tab);
        index++;
        if (index < 0) index = 0;
        if (index >= len) index = len - 1;
        tab = _showItems[index];
        TabSelectAnimBtnItem item = GetTabBtn(tab);
        if (item == null || item.data == null) return;
        if (item.data.lockState) return;
        SelectTabBtn(tab);
    }

    private void OnSelectPreTab()
    {
        string tab = _curTab;
        int len = _showItems.Count;
        int index = _showItems.IndexOf(tab);
        index--;
        if (index < 0) index = 0;
        if (index >= len) index = len - 1;
        tab = _showItems[index];
        TabSelectAnimBtnItem item = GetTabBtn(tab);
        if (item == null || item.data == null) return;
        if (item.data.lockState) return;
        SelectTabBtn(tab);
    }

    public TabSelectAnimBtnItem GetTabBtn(string tab)
    {
        if (_items.ContainsKey(tab)) return _items[tab];
        return null;
    }
    

    public void SelectTabBtn(string tab)
    {
        TabSelectAnimBtnItem item = GetTabBtn(tab);
        if(item != null)
        {
            item.isOn = true;
        }
        OnTabBtnValueChange(tab, true);
    }

    private void OnTabBtnValueChange(string tab, bool state)
    {
        if (tab == null || !_showItems.Contains(tab)) return;
        if (tab == _curTab) return;
        _lastTab = _curTab;
        _curTab = tab;

        if (state)
        {
            if(_onTabBtnSelect != null){
                _onTabBtnSelect(tab);
            }
        }
        StartCoroutine(TabBtnSelectAnim());
    }


    private IEnumerator TabBtnSelectAnim()
    {
        int curIndex = _showItems.IndexOf(_curTab);
        RectTransform tabBtnObj = _items[_curTab].root;
        RectTransform lastTabBtnObj = null;
        if (_lastTab != null && _items.ContainsKey(_lastTab))
        {
            lastTabBtnObj = _items[_lastTab].root;
        }
        float selectSize = 1.1f;

        Vector3 fromTagSize = tabBtnObj.localScale;
        Vector3 toTagSize = Vector3.one * selectSize;
        Vector3 fromLastSize = Vector3.one;
        Vector3 toLastSize = Vector3.one;
        if (lastTabBtnObj != null)
        {
            fromLastSize = lastTabBtnObj.localScale;
        }
        Vector2 toPos = new Vector2(-_cellSize.x / 2f - curIndex * (_cellSize.x + _space.x), _grid.anchoredPosition.y);
        Vector2 fromPos = _grid.anchoredPosition;
        float pastTime = 0f;
        float totalTime = 0.25f;
        float percent;

        //Debug.Log("toPos:"+toPos + " cellSize:"+_cellSize.x + " curIndex:"+ _curIndex);

        WaitForEndOfFrame waitFrame = new WaitForEndOfFrame();
        while (pastTime < totalTime)
        {
            percent = pastTime / totalTime;
            _grid.anchoredPosition = Vector2.Lerp(fromPos, toPos, percent);
            tabBtnObj.localScale = Vector3.Lerp(fromTagSize, toTagSize, percent);
            if(lastTabBtnObj != null)
                lastTabBtnObj.localScale = Vector3.Lerp(fromLastSize, toLastSize, percent);

            pastTime += Time.deltaTime;
            yield return waitFrame;
        }

        _grid.anchoredPosition = toPos;
        tabBtnObj.SetSize(toTagSize);
        if (lastTabBtnObj != null) lastTabBtnObj.localScale = toLastSize;

    }

    private void OnDestroy()
    {
        if (_countDown != null)
        {
            ClientTimer.Instance.RemoveCountDown(_countDown);
            _countDown = null;
        }
    }
}
