using Assets.Scripts.Framework.GalaSports.Core;
using Com.Proto;
using DataModel;
using game.main;
using Google.Protobuf.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ActivityEverydayPowerSigninView : View
{

    private Transform _parent;

    private void Awake()
    {
        _parent = transform.Find("Content/AwardBg");

    }


    public void CreateEverydayPowerData(List<EveryDayPowerVO> list)
    {
        //刷新在这里做 (第一次生成是不会删除的)
       // Refresh();

        var everydayPowerItemPrefab = GetPrefab("Activity/Prefabs/EverydayPowerItem");
        foreach (var t in list)
        {
            var go = Instantiate(everydayPowerItemPrefab, _parent, false) as GameObject;
            go.transform.localScale = Vector3.one;
            go.name = t.Id.ToString();
            go.GetComponent<EverydayPowerItem>().SetData(t);
        }
    }


    public void RefreshData(List<EveryDayPowerVO> list)
    {
        for (int i = 0; i < _parent.childCount; i++)
        {
            _parent.GetChild(i).GetComponent<EverydayPowerItem>().SetData(list[i]); 
        }
    }

//    private void Refresh()
//    {
//        if (_parent.childCount != 0)
//        {
//            for (int i = 0; i < _parent.childCount; i++)
//            {
//                DestroyImmediate(_parent.GetChild(i).gameObject);
//            }
//        }
//    }


}
