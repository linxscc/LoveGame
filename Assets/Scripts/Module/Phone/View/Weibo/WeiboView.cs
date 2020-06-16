using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Framework.GalaSports.Core;
using System;
using UnityEngine.UI;
using Assets.Scripts.Framework.GalaSports.Core.Events;
using Common;
using game.tools;
using Assets.Scripts.Framework.GalaSports.Service;
//using System.Text.RegularExpressions;

public class WeiboView : View
{
    private Transform _container;
    private List<WeiboVo> _data;
    private Button _publishBtn;
    private int _publishSelect;
    private Transform _ClickImage;
    private Transform _publishContainer;
    private Transform _redPoint;
    private void Awake()
    {
        _publishSelect = -1;
        _redPoint = transform.Find("PublishBtn/Tips");
        _container = transform.Find("Scroll View/Viewport/Content");
        _publishBtn = transform.Find("PublishBtn").GetComponent<Button>();
        _publishBtn.onClick.AddListener(ShowPublish);
        _ClickImage = transform.Find("PublishSelect/BgClick");
        _publishContainer = transform.Find("PublishSelect/Scroll View/Viewport/Content");
    }

    private void ShowPublish()
    {
        Debug.Log("ShowPubish  ");

        PointerClickListener.Get(_ClickImage.gameObject).onClick = delegate (GameObject go)
        {
            transform.Find("PublishSelect").gameObject.Hide();
        };

        for (int i = _publishContainer.childCount - 1; i >= 0; i--)
        {

            Destroy(_publishContainer.GetChild(i).gameObject);
        }

        List<WeiboVo> infoList;
        infoList = _data.FindAll(match => { return match.IsPublish == false && match.WeiboRuleInfo.weiboSceneInfo.NpcId == 0; });
        if (infoList.Count > 0)
        {
            transform.Find("PublishSelect").gameObject.Show();
            _publishBtn.interactable = true;
           // _publishBtn.transform.Find("Tips").gameObject.SetActive(true);
            foreach (var v in infoList)
            {
                var FCSelectPrefab = GetPrefab("Phone/Prefabs/Item/FCSelectItem");
                var item = Instantiate(FCSelectPrefab) as GameObject;
                item.transform.SetParent(_publishContainer, false);
                item.transform.localScale = Vector3.one;
                string showText = v.WeiboRuleInfo.weiboSceneInfo.TitleContent;
                if (showText.Length>15)
                {
                    showText = showText.Substring(0, 15) + "...";
                }
                item.GetComponent<FCSelectItem>().SetData(showText);
                item.transform.GetComponent<Button>().onClick.AddListener(() =>
                {
                    EventDispatcher.TriggerEvent<int>(EventConst.PhoneWeiboItemPublishClick, v.SceneId);
                    transform.Find("PublishSelect").gameObject.Hide();
                });
            }
        }
    }

    private void ShowList()
    {
        for (int i = 0; i < _container.childCount; i++)
        {
            Destroy(_container.GetChild(i).gameObject);
        }

        var weiboprefab = GetPrefab("Phone/Prefabs/Item/WeiboItem");
        var commentPrefab = GetPrefab("Phone/Prefabs/Item/WeiboCommentItem");
        var linePrefab = GetPrefab("Phone/Prefabs/Item/WeiboLineItem");
        foreach (var v in _data)
        {
            if(!v.IsPublish)
            {
                continue;
            }
            var item = Instantiate(weiboprefab) as GameObject;
            item.transform.SetParent(_container, false);
            //item.transform.localScale = Vector3.one;
            item.GetComponent<WeiboItem>().SetData(v);
            foreach (var v1 in v.WeiboRuleInfo.weiboTalkInfos)
            {
                var citem = Instantiate(commentPrefab) as GameObject;
                citem.transform.SetParent(_container, false);
                // citem.transform.localScale = Vector3.one;

                var strTemp = "";
                if (v1.ReplyFromNpcId < 5)
                {
                    strTemp = "<color=#d0a4d9>" + PhoneData.GetNpcNameById(v1.ReplyFromNpcId) + "</color>";
                }
                else
                {
                    strTemp = "<color=#d0a4d9>" + v1.ReplyFromNpcName + "</color>";
                }

                citem.GetComponent<WeiboCommentItem>().SetData(strTemp, v1.Content);
            }

            item = Instantiate(linePrefab) as GameObject;
            item.transform.SetParent(_container, false);
        }


    }

    public void SetData(List<WeiboVo> data)
    {
        data.Sort();
        Debug.Log("SetData");
        _data = data;
        CheckShowPublish();
        ShowList();
    }

    private void CheckShowPublish()
    {
        List<WeiboVo> infoList;
        infoList = _data.FindAll(match => { return match.IsPublish == false && match.WeiboRuleInfo.weiboSceneInfo.NpcId == 0; });
        if (infoList.Count > 0)
        {
            _publishBtn.interactable = true;
            _redPoint.gameObject.Show();
        }
        else
        {
            _publishBtn.interactable = false;
            _redPoint.gameObject.Hide();
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
