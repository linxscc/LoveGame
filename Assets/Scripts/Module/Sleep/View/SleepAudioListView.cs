using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module;
using Com.Proto;
using DataModel;
using DG.Tweening;
using game.main;
using game.tools;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SleepAudioListView : View
{
    private Transform _audioListGrid;

    private List<SleepAudioListItem> _itemList;

    private void Awake()
    {
        _audioListGrid = transform.Find("AudioList/Viewport/Content");

        _itemList = new List<SleepAudioListItem>();
    }

    // Start is called before the first frame update
    void Start()
    {
        SetData();
    }

    private void SetData()
    {
        var prefab = GetPrefab("Sleep/Prefabs/SleepAudioListItem");
        ClearItems();
        for (int i = 0; i < 10; ++i)
        {
            GameObject go = Instantiate(prefab, _audioListGrid, false);
            SleepAudioListItem item = go.GetComponent<SleepAudioListItem>();
            _itemList.Add(item);
        }
    }

    private void ClearItems()
    {
        for (int i = 0; i < _itemList.Count; ++i)
        {
            Destroy(_itemList[i].gameObject);
        }
        _itemList.Clear();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {
        ClearItems();
    }
}
