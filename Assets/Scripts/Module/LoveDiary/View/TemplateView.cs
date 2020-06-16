using Assets.Scripts.Framework.GalaSports.Core;
using Com.Proto;
using System;
using System.Collections;
using System.Collections.Generic;
using game.main;
using UnityEngine;
using UnityEngine.UI;

public class TemplateView : View
{
    ScrollRect _scrollRect;
    private void Awake()
    {
        _scrollRect = transform.Find("Scroll View").GetComponent<ScrollRect>();

    }
    private void Start()
    {

    }

    private void CreateItem(ElementPB pb,bool isLock=true)
    {
        var prefab = GetPrefab("LoveDiary/Prefabs/TemplateItem");
        var item = Instantiate(prefab) as GameObject;
        item.transform.SetParent(_scrollRect.content, false);
        // item.transform.localScale = Vector3.one;
        item.transform.Find("Lock").gameObject.SetActive(isLock);
        item.transform.Find("Bg").GetComponent<Image>().sprite= AssetManager.Instance.GetSpriteAtlas("UIAtlas_LoveDiary_Element_" + pb.Id); ;
        item.transform.Find("Text").GetComponent<Text>().text = pb.Name;
        if (isLock == true) return;

        UIEventListener.Get(item).onClick = (data) =>
        {
            int elementId = pb.Id;
            SendMessage(new Message(MessageConst.CMD_LOVEDIARY_TEMPLATE_SELECT, Message.MessageReciverType.CONTROLLER, elementId));
        };
        return;
    }

    public void SetData(List<ElementPB> unlocks, List<ElementPB> locks)
    {
        for(int i=0;i<unlocks.Count;i++)
        {
            CreateItem(unlocks[i],false);
        }
        for (int i = 0; i < locks.Count; i++)
        {
            CreateItem(locks[i]);
        }
    }
}
