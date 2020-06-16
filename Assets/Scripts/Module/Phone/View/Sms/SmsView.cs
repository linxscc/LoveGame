using System;
using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Service;
using DataModel;
using game.tools;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SmsView : View
{
    private Transform _container;

    private Dictionary<int, List<MySmsOrCallVo>> _data;

    private bool _isShowCurrent;

    private bool _isHistoryTabInited;
    private int curSelectIdx;

    private void Awake()
    {
        curSelectIdx = 1;
        transform.Find("TopBar/CurrentBtn").GetComponent<Toggle>().onValueChanged.AddListener(delegate(bool b)
        {
            if (b)
            {

                ShowList();
                SendMessage(new Message(MessageConst.CMD_PHONE_SMS_CUR));
            }
        });
        
        transform.Find("TopBar/HistoryBtn").GetComponent<Toggle>().onValueChanged.AddListener(delegate(bool b)
        {
            if (b)
            {
                ShowHistory();
                SendMessage(new Message(MessageConst.CMD_PHONE_SMS_HISTORY));
            }
        }); 
    }

    private void ShowAllTabBg()
    {
        var tabGroupTrans = transform.Find("HistoryView/TabBar/Group");
        for (int i = 1; i < 6; i++)
        {
            tabGroupTrans.Find("TabItem" + i).GetComponent<Image>().enabled = true;
        }
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

        var prefab = GetPrefab("Phone/Prefabs/Item/SmsItem");      
        foreach(var v in _data)
        {
            v.Value.Sort();
            if(v.Value.Count>0)
            {
                var item = Instantiate(prefab) as GameObject;
                item.transform.SetParent(_container, false);
                item.transform.localScale = Vector3.one;
                item.GetComponent<SmsItem>().SetData(v.Value);
            }
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
           // SelectHistory(0);
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
        if(listdata==null)
        { return; }

        for (int i = 0; i < listdata.Count; i++)
        {
            if (listdata[i].IsReaded)
            {
                var item = Instantiate(prefab) as GameObject;
                item.transform.SetParent(container,false);
                item.transform.localScale = Vector3.one;
                item.AddComponent<SmsHistoryItem>().SetData(listdata[i]);
                PointerClickListener.Get(item).onClick= delegate(GameObject go)
                {
                   
                    var data= go.GetComponent<SmsHistoryItem>().Data;

                    List<MySmsOrCallVo> dataList = new List<MySmsOrCallVo>();
                    dataList.Add(data);
                    SendMessage(new Message(MessageConst.CMD_PHONE_SMS_SHOWDETAIL,Message.MessageReciverType.DEFAULT, dataList));
                };
                // item.transform.Find("Text").GetComponent<Text>().text = contents[i];
            }
        }
    }

    private void InitHistoryTabs()
    {
        _isHistoryTabInited = true;
        var tabGroupTrans = transform.Find("HistoryView/TabBar/Group");
        
        for (int i = 1; i < 6; i++)
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
            //if (GlobalData.NpcModel.NpcList.Count > i)
            //{
            //    tabGroupTrans.Find("TabItem" + i + "/Text").GetComponent<Text>().text =
            //        GlobalData.NpcModel.NpcList[i].NpcName;
            //}
            
        }

        tabGroupTrans.Find("TabItem1").GetComponent<Toggle>().isOn = true;
    }

    private void SelectHistory(int index)
    {
        //var npcId = GlobalData.NpcModel.NpcList[index].NpcId;

        int NpcId = index;
        List<MySmsOrCallVo> list = GetHistoryList(NpcId); 
        InitHistoryList(list);
    }

    private List<MySmsOrCallVo> GetHistoryList(int npcId)
    {
        List<MySmsOrCallVo> list = new List<MySmsOrCallVo>();
        if(npcId<5)
        {
            if(_data.ContainsKey(npcId))
            {
                return _data[npcId];
            }
            return null;
        }
        foreach(var v in _data)
        {
            if(v.Key<5)
            {
                continue;
            }
            list.AddRange(v.Value);
        }
        return list;

    }




    public void SetData(Dictionary<int, List<MySmsOrCallVo>> data)
    {
        _data = data;
        ShowList();
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
        //这里需要更新界面数据
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
}