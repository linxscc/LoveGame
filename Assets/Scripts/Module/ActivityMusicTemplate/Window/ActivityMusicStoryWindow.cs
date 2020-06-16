using System;
using System.Collections;
using System.Collections.Generic;
using game.main;
using UnityEngine;
using UnityEngine.UI;

public class ActivityMusicStoryWindow : Window
{
    private Text _title;
    private Text _tips;
    private Transform _parent;

    private void Awake()
    {
        _title = transform.GetText("Title/Text");
        _tips = transform.GetText("Tips");
        _parent = transform.Find("StoryList/Content");
    }


    public void SetData(List<ActivityStoryVo> list)
    {

        SetTitle();
     
        CreateStoryItem(list);
    }


    private void SetTitle()
    {
        _title.text = I18NManager.Get("ActivityMusicTemplate_storyTittle");        
        _tips.text = I18NManager.Get("ActivityMusicTemplate_storyTips",I18NManager.Get("ActivityMusicTemplate_storyName1"));
    }

    private void CreateStoryItem(List<ActivityStoryVo> list)
    {
        if (_parent.childCount != 0)
        {
            for (int i = _parent.childCount - 1; i >= 0; i--)
            {
                DestroyImmediate(_parent.GetChild(i).gameObject);
            }
        }

        var prefab = GetPrefab("ActivityMusicTemplate/Prefabs/ActivityMusicStoryItem");
        for (int i = 0; i < list.Count; i++)
        {
            var go = Instantiate(prefab, _parent, false);
            go.GetComponent<ActivityMusicStoryItem>().SetData(list[i],i+1);
        }
    }
    
    
    
}
