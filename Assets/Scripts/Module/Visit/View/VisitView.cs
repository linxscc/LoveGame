
using System;
using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core;
using game.main;
using UnityEngine;
using UnityEngine.UI;

public class VisitView : View
{

    Transform _contain;
    LoopVerticalScrollRect _rect;
    List<VisitVo> _visitVoList;
    private void Awake()
    {
        //_contain = transform.Find("Scroll View");
        //_rect = _contain.GetComponent<LoopVerticalScrollRect>();
        //_rect.prefabName = "Visit/Prefabs/Item/VisitSelectItem";
        //_rect.poolSize = 4;
        //_rect.UpdateCallback = VisitSelectItemUpdateCallback;
    }


    private void VisitSelectItemUpdateCallback(GameObject go, int index)
    {
        Debug.Log("VisitSelectItemUpdateCallback " + index);
        go.GetComponent<VisitSelectItem>().SetData(_visitVoList[index]);
    }

    public void SetData(List<VisitVo> visitVoList)
    {
        //_rect.RefillCells();
        _visitVoList = visitVoList;
        //_rect.totalCount = visitVoList.Count;
        //_rect.RefreshCells();




        foreach (var v in visitVoList)
        {
            transform.Find("BgImage/VisitSelectItem" + (int)v.NpcId).GetComponent<VisitSelectItem>().SetData(v);


        }
    }

    

}
