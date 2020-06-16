using Assets.Scripts.Framework.GalaSports.Core.Events;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module.NetWork;
using Com.Proto;
using Common;
using DataModel;
using game.main;
using game.tools;
using Google.Protobuf.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utils;

public class AKeyToGetWindow : Window
{
   
    private LoopVerticalScrollRect scrollRect;
    private List<MailAwardVO> tempList = new List<MailAwardVO>();
    private GameObject _onClick;
    private void Awake()
    {
        _onClick = transform.Find("OnClick").gameObject;
        scrollRect = transform.Find("Window/Awards").GetComponent<LoopVerticalScrollRect>();


        PointerClickListener.Get(_onClick).onClick = go => { Close();};
    }


    public void SetData(int count, List<MailAwardVO> list)
    {
        tempList = list;

        scrollRect.prefabName = "Mail/Prefabs/MailAwardItem";
        scrollRect.poolSize = 6;
        scrollRect.UpdateCallback = AllMailCallback;
        scrollRect.totalCount = count;
        scrollRect.RefreshCells();
    }

    private void AllMailCallback(GameObject go, int index)
    {
        go.GetComponent<MailAwardItem>().SetData(tempList[index]);
    }


    protected override void OnClickOutside(GameObject go)
    {
        //base.OnClickOutside(go);
    }
}
