using System.Collections;
using System.Collections.Generic;
using game.main;
using UnityEngine;
using UnityEngine.UI;

public class ActivityCapsuleTemplateStoryWindow : Window
{
    private Transform _storyListGrid;
    private List<ActivityCapsuleTemplateStoryItem> _itemList;

    private void Awake()
    {
        _storyListGrid = transform.Find("StoryList/Content");

        _itemList = new List<ActivityCapsuleTemplateStoryItem>();
    }


    // Use this for initialization
    void Start () {
		
	}

    private void ClearItems()
    {
        for(int i = 0;i < _itemList.Count; ++i)
        {
            Destroy(_itemList[i].gameObject);
        }
        _itemList.Clear();
    }
	
    public void SetData(ActivityCapsuleTemplateModel model)
    {
        var prefab = GetPrefab("ActivityCapsuleTemplate/Prefabs/ActivityCapsuleTemplateStoryItem");
        ClearItems();
        //for (int i = 0; i < 10; ++i)
        //{
        //    GameObject go = Instantiate(prefab, _storyListGrid, false);
        //}

        bool lastIsClear = true;
        for (int i = 0; i < model.storyIds.Count; ++i)
        {
            GameObject go = Instantiate(prefab, _storyListGrid, false);
            ActivityCapsuleTemplateStoryItem item = go.GetComponent<ActivityCapsuleTemplateStoryItem>();
            item.SetData(model.storyIds[i], model, lastIsClear);
            lastIsClear = model.IsReadStory(model.storyIds[i]);
            _itemList.Add(item);
        }
    }

    private void OnDestroy()
    {
        ClearItems();
    }
}
