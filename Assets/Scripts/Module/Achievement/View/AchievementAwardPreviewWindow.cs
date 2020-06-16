using System.Collections;
using System.Collections.Generic;
using game.main;
using UnityEngine;
using UnityEngine.UI;

public class AchievementAwardPreviewWindow : Window
{
    private Transform _parent;

    private void Awake()
    {
        _parent = transform.Find("ScrollRect/Content");
    }


    public void SetData(List<AwardPreviewVo> list)
    {
        CreateItem(list);
    }

    private void CreateItem(List<AwardPreviewVo> list)
    {
        var prefab = GetPrefab("Achievement/Prefabs/AchievementAwardPreviewItem");

        foreach (var t in list)
        {
            var go = Instantiate(prefab, _parent, false);
            go.GetComponent<AchievementAwardPreviewItem>().SetData(t);
        }
        
    }
    
}
