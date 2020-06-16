using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Service;
using DataModel;
using game.main;
using game.tools;
using UnityEngine;
using UnityEngine.UI;

public class ActivityMusicExchangeShopView : Window
{
    private RawImage _consumeItemImg;
    private Text _consumeItemNum;
    private Transform _parent;
    private int _exchangeItemId;
    
    
    private void Awake()
    {
        _consumeItemImg = transform.GetRawImage("ConsumeItem/Icon");
        _consumeItemNum = transform.GetText("ConsumeItem/Num");
        _parent = transform.Find("ScrollRect/Content");

        PointerClickListener.Get(transform.Find("ConsumeItem").gameObject).onClick = go =>
        {
            FlowText.ShowMessage(I18NManager.Get("ActivityMusicTemplate_ExchangeShopTitle"));
        };
    }


    public void SetData(List<ActivityExchangeShopVo> list,int id,string path)
    {
        _exchangeItemId = id;
        CreateExchangeShopItem(list);
        _consumeItemNum.text = GlobalData.PropModel.GetUserProp(id).Num.ToString();
        _consumeItemImg.texture = ResourceManager.Load<Texture>(path);
    }


    private void CreateExchangeShopItem(List<ActivityExchangeShopVo> list)
    {
        var prefabParent = GetPrefab("ActivityMusicTemplate/Prefabs/ExchangeShopItemParent");       
        var prefab = GetPrefab("ActivityMusicTemplate/Prefabs/ActivityMusicExchangeShopItem");
        Transform parent = null;
        int temp = 0;
        int nameIndex = 0;
        for (int i = 0; i < list.Count; i++)
        {                     
            if (i==0)
            {
                var goParent =  Instantiate(prefabParent, _parent, false);
                parent = goParent.transform;
                nameIndex++;
                goParent.name = "Parent" + nameIndex;
            }
            else
            {
                if (temp==3)
                {
                  var goParent =  Instantiate(prefabParent, _parent, false);
                  parent = goParent.transform;     
                  nameIndex++;
                  goParent.name = "Parent" + nameIndex;
                  temp = 0;                                 
                }
            }
            var go = Instantiate(prefab,parent,false);
            go.GetComponent<ActivityMusicExchangeShopItem>().SetData(list[i]);
            go.name = list[i].MallId.ToString();           
            temp++;
        }
    }

    public void RefreshExchangeShopItem(List<ActivityExchangeShopVo> list)
    {
        for (int i = 0; i < _parent.childCount; i++)
        {
            var parent = _parent.GetChild(i);
            for (int j = 0; j < parent.childCount; j++)
            {
                var go = parent.GetChild(j).gameObject;
                foreach (var t in list)
                {
                    if (go.name==t.MallId.ToString())
                    {
                        Debug.LogError("---刷新道具名--->"+go.name);
                        go.GetComponent<ActivityMusicExchangeShopItem>().SetData(t);
                        break;
                    }
                }
            }
        }
    }

    public void RefreshExchangeItemNum()
    {
        Debug.LogError("刷新数量");
        _consumeItemNum.text = GlobalData.PropModel.GetUserProp(_exchangeItemId).Num.ToString();
    }
    
    protected override void OnClickOutside(GameObject go)
    {
        SendMessage(new Message(MessageConst.CMD_ACTIVITY_MUSCI_BACK_EXCHANGESHOP));       
        base.OnClickOutside(go);
    }
}
