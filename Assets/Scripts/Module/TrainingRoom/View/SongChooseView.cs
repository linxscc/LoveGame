using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core;
using DG.Tweening;
using game.main;
using UnityEngine;
using UnityEngine.UI;

public class SongChooseView : View
{
    private const float Height = 237 + 320 + 60;
    private Transform _parent;

    private RectTransform _rect;
    private ScrollRect _scrollRect;
    private Transform _songContent;
    private SpecialSongItem _specialSongItem;

    private Transform _textHintContent;

    private void Awake()
    {
        _textHintContent = transform.Find("TextHintContent");
        _songContent = transform.Find("SongContent");

        _specialSongItem = transform.Find("SongContent/SpecialSongItem").GetComponent<SpecialSongItem>();

        _textHintContent.GetText("TitleText").text = "演奏选择";
        _textHintContent.GetText("Text").text = "每日6:00刷新练习内容";

        _scrollRect = _songContent.Find("ScrollRect").GetComponent<ScrollRect>();
        _parent = _songContent.Find("ScrollRect/Content");
        _rect = _songContent.GetRectTransform("ScrollRect");

        transform.GetButton("HelpBtn").onClick.AddListener(ShowHelp);
        
        string key = "SongChooseView_ShowHelp";
        string str = PlayerPrefs.GetString(key);
        if (string.IsNullOrEmpty(str))
        {
            ShowHelp();
            PlayerPrefs.SetString(key, "1688");
            PlayerPrefs.Save();
        }
    }

    private void ShowHelp()
    {
        var window = PopupManager.ShowWindow<PopupWindow>("GameMain/Prefabs/PropWindow");
        window.SetTrainingRoom();    
    }

    private void Start()
    {
        ConnectModule(_specialSongItem);

        var scrollHeight = GetComponent<RectTransform>().rect.height - Height;
        var rect = _scrollRect.GetComponent<RectTransform>();
        rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, scrollHeight);
    }

    public void SetData(List<UserMusicGameVO> list)
    {
        CreateMusicActivity(list);
    }

    private void CreateMusicActivity(List<UserMusicGameVO> list)
    {
        _parent.RemoveChildren();

        foreach (var t in list)
        {
            var item = InstantiatePrefab("TrainingRoom/Prefabs/SongItem", _parent);
            item.transform.localScale = Vector3.one;
            item.GetComponent<SongItem>().IsUnfold = false;
            item.GetComponent<SongItem>().SetData(t);
            item.name = t.ActivityId.ToString();
        }

        _specialSongItem.SetData(list.Count + 1);
        // var specialItem = InstantiatePrefab("TrainingRoom/Prefabs/SpecialSongItem", _parent);
        // specialItem.transform.localScale = Vector3.one;
        // specialItem.GetComponent<SpecialSongItem>().IsUnfold = false;
        // specialItem.GetComponent<SpecialSongItem>().SetData(list.Count + 1);
        // specialItem.name = (list.Count + 1).ToString();

        LayoutRebuilder.ForceRebuildLayoutImmediate(_scrollRect.content);
        // _max = -specialItem.transform.localPosition.y;
    }


    public void CreateChooseCards(int activityId, List<TrainingRoomCardVo> chooseCards)
    {
        GameObject go = null;

        for (var i = 0; i < _parent.childCount; i++)
            if (activityId.ToString() == _parent.GetChild(i).name)
            {
                go = _parent.GetChild(i).gameObject;
                go.GetComponent<SongItem>().SetCards(chooseCards);
                break;
            }
    }


    public void SetChildrenUnfold(int activityId)
    {
        for (var i = 0; i < _parent.childCount; i++)
        {
            var go = _parent.GetChild(i).gameObject;
            if (activityId != int.Parse(go.name))
                if (go.GetComponent<SongItem>() != null)
                    go.GetComponent<SongItem>().SetShrinkState();
        }

        var scrollHeight = GetComponent<RectTransform>().rect.height - Height;

        if (activityId != _specialSongItem.ActivityId) _specialSongItem.SetShrinkState();

        var rect = _scrollRect.GetComponent<RectTransform>();
        if (_specialSongItem.IsUnfold)
            rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, scrollHeight - 230);
        else
            rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, scrollHeight);

        Ani(activityId, _specialSongItem.IsUnfold);
    }

    private void Ani(int activityId, bool specialIsUnfold)
    {
        var index = activityId - 1;
        if (index >= _parent.childCount)
            return;

        LayoutRebuilder.ForceRebuildLayoutImmediate(_scrollRect.content);

        var offset = 0;
        var big = 1328;
        var small = 348;
        for (var i = 0; i < index; i++) offset += small;

        var maxHeight = 0;
        for (var i = 0; i < _parent.childCount; i++)
        {
            if (i == 0 && !specialIsUnfold)
            {
                maxHeight += big;
            }
            else
            {
                maxHeight += small;
            }
        }

        maxHeight = maxHeight - (int) _scrollRect.GetComponent<RectTransform>().rect.height;

        if (offset > maxHeight)
            offset = maxHeight;

        _scrollRect.content.DOAnchorPosY(offset, 0.5f);
    }


    public void ChangeAbility(UserMusicGameVO vo)
    {
        for (var i = 0; i < _parent.childCount; i++)
        {
            var go = _parent.GetChild(i).gameObject;
            if (go.name == vo.ActivityId.ToString())
                go.GetComponent<SongItem>().SetData(vo);
        }
    }
}