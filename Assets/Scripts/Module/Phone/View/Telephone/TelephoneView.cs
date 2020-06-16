using System;
using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Service;
using DataModel;
using game.tools;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TelephoneView : View
{
    private Transform _container;

    Dictionary<int, List<MySmsOrCallVo>> _data;
    
    private bool _isShowCurrent;

    private bool _isHistoryTabInited;
    private int curSelectIdx;

    private void Awake()
    {
        _isShowCurrent = true;
        curSelectIdx = 0;
        transform.Find("TopBar/CurrentBtn").GetComponent<Toggle>().onValueChanged.AddListener(delegate(bool b)
        {
            if (b)
            {
                ShowList();
            }
        });
        
        transform.Find("TopBar/HistoryBtn").GetComponent<Toggle>().onValueChanged.AddListener(delegate(bool b)
        {
            if (b)
            {
                ShowHistory();
            }
        });
    }

    
    private void ShowList()
    {
        _isShowCurrent = true;
        _container = transform.Find("ListContainer/Contents");
        _container.gameObject.Show();
        transform.Find("HistoryView").gameObject.Hide();
        for (int i = 0; i < _container.childCount; i++)
        {
            Destroy(_container.GetChild(i).gameObject);
        }

        var containerTrans = transform.Find("ListContainer");
        var rect = containerTrans.GetComponent<ScrollRect>();
        rect.verticalNormalizedPosition = 1;

        var prefab = GetPrefab("Phone/Prefabs/Item/TelephoneItem");

        foreach(var vk in _data)
        {
            MySmsOrCallVo callVo = null;
            List<MySmsOrCallVo> mySms= vk.Value;
            mySms.Sort();
            callVo =vk.Value.Find((match) => { return match.IsReaded == false; });
            if(callVo==null)
            {
                mySms.Sort((a, b) => { return a.FinishTime.CompareTo(b.FinishTime); });
                callVo = mySms[mySms.Count - 1];
            }
            else
            {

            }
            var item = Instantiate(prefab) as GameObject;
            item.transform.SetParent(_container, false);
            item.transform.localScale = Vector3.one;
            item.GetComponent<TelephoneItem>().SetData(callVo);
        }
    }
    

      private void ShowHistory()
    {
        _isShowCurrent = false;
        
        _container = transform.Find("ListContainer/Contents");
        _container.gameObject.Hide();
     

        if (!_isHistoryTabInited)
        {
            InitHistoryTabs();
        }
        SelectHistory(curSelectIdx);
    }

    private void InitHistoryList(List<MySmsOrCallVo> listdata)
    {
        var container = transform.Find("HistoryView/Container/Viewport/Content");
        transform.Find("HistoryView").gameObject.Show();
        for (int i = 0; i < container.childCount; i++)
        {
            Destroy(container.GetChild(i).gameObject);
        }

        var containerTrans = transform.Find("ListContainer");
        var rect = containerTrans.GetComponent<ScrollRect>();
        rect.verticalNormalizedPosition = 1;

        var prefab = GetPrefab("Phone/Prefabs/Item/SmsSelectionItem");

        if (listdata == null)
        { return; }

        for (int i = 0; i < listdata.Count; i++)
        {
            if (listdata[i].IsReaded)
            {
                var item = Instantiate(prefab) as GameObject;
                item.transform.SetParent(container,false);
                item.transform.localScale = Vector3.one;
                item.AddComponent<SmsHistoryItem>().SetData(listdata[i]);
                int index = i;
                PointerClickListener.Get(item).onClick= delegate(GameObject go)
                {
                    var data= go.GetComponent<SmsHistoryItem>().Data;
                    SendMessage(new Message(MessageConst.CMD_PHONE_CALL_SHOWDETAIL,Message.MessageReciverType.DEFAULT,data));

                };
                // item.transform.Find("Text").GetComponent<Text>().text = contents[i];
            }
        }
    }

    private void InitHistoryTabs()
    {
        _isHistoryTabInited = true;
        var tabGroupTrans = transform.Find("HistoryView/TabBar/Group");
        
        for (int i = 0; i < 4; i++)
        {
            tabGroupTrans.Find("TabItem"+i).GetComponent<Toggle>().onValueChanged.AddListener(delegate(bool B) {
                if (B)
                {
                    ShowAllTabBg();
                    var obj = EventSystem.current.currentSelectedGameObject;
                    if (obj.name.IndexOf("TabItem", StringComparison.Ordinal) != -1)
                    {
                        int nameIndex = int.Parse(obj.name.Substring(7));
                        ClientTimer.Instance.DelayCall(delegate
                        {
                            obj.GetComponent<Image>().enabled = false;
                        }, 0.1f);
                        SelectHistory(nameIndex);
                        curSelectIdx = nameIndex;
                    }
                }
            });
            if (GlobalData.NpcModel.NpcList.Count > i)
            {
                tabGroupTrans.Find("TabItem" + i + "/Text").GetComponent<Text>().text =
                    GlobalData.NpcModel.NpcList[i].NpcName;
            }
            
        }

        tabGroupTrans.Find("TabItem0").GetComponent<Toggle>().isOn = true;
    }

    
    private void ShowAllTabBg()
    {
        var tabGroupTrans = transform.Find("HistoryView/TabBar/Group");
        for (int i = 0; i < 4; i++)
        {
            tabGroupTrans.Find("TabItem" + i).GetComponent<Image>().enabled = true;
        }
    }
    private void SelectHistory(int index)
    {
        var npcId = GlobalData.NpcModel.NpcList[index].NpcId;
        //var list = _data.FindAll((item) => { return item.NpcId == npcId; });
        if(_data.ContainsKey(npcId))
        {
            InitHistoryList(_data[npcId]);
        }
        else
        {
            InitHistoryList(null);
        }
       // InitHistoryList(list);
    }
    
    
    public void SetData(Dictionary<int, List<MySmsOrCallVo>> data)
    {
        _data = data;
       // ShowList();
        if (_isShowCurrent)
        {
            ShowList();
        }
        else
        {
            ShowHistory();
        }
    }
    
    public void Refresh()
    {
        if (_isShowCurrent)
        {
            ShowList();
        }
        else
        {
            ShowHistory();
        }
    }
    

    public override void Hide()
    {
        base.Hide();
        transform.gameObject.Hide();
    }

    public override void Show(float delay = 0)
    {
        base.Show(delay);
        transform.gameObject.Show();
    }
}