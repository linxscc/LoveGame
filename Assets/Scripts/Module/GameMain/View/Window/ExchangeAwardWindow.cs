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

public class ExchangeAwardWindow : Window
{
    private Button okBtn;
    private Transform parent;


    private void Awake()
    {
        okBtn = transform.Find("okBtn").GetComponent<Button>();
        okBtn.onClick.AddListener(OkBtn);

        parent = transform.Find("Awards/Content");
        //
    }

   


    public void ShowAwards(RepeatedField<AwardPB> awards)
    {
        var prefab = GetPrefab("GameMain/Prefabs/ExchangeAwardItem");

        for (int i = 0; i < awards.Count; i++)
        {
            RewardVo vo = new RewardVo(awards[i]);

            var item = Instantiate(prefab, parent, false) as GameObject;
            item.transform.localScale = Vector3.one;
         
            item.GetComponent<ExchangeAwardItem>().SetData(vo);
          

        }
    }

    protected override void OnClickOutside(GameObject go)
    {
       
    }

    private void OkBtn()
    {
        Close();
    }
}
