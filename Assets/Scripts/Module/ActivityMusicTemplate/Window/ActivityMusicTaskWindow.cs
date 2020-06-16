using System.Collections;
using System.Collections.Generic;
using game.main;
using UnityEngine;

public class ActivityMusicTaskWindow : Window
{
    private Transform _parent;
    
    private void Awake()
    {
        _parent = transform.Find("TaskContent/Content");
    }


    public void SetData(List<ActivityMissionVo> list)
    {
        CreateTaskItem(list);
    }

    private void CreateTaskItem(List<ActivityMissionVo> list)
    {

        if (_parent.childCount==0)
        {
            Debug.LogError("生成任务");
            var prefab = GetPrefab("ActivityMusicTemplate/Prefabs/ActivityMusicTaskItem");
            foreach (var t in list)
            {
                var go = Instantiate(prefab, _parent, false);
                go.GetComponent<ActivityMusicTaskItem>().SetData(t);
                go.name = t.ActivityMissionId.ToString();
            } 
        }
        else
        {
            Debug.LogError("刷新任务");
            for (int i = 0; i < list.Count; i++)
            {
                var go = _parent.GetChild(i).gameObject;
                go.GetComponent<ActivityMusicTaskItem>().SetData(list[i]);
                go.name = list[i].ActivityMissionId.ToString();
            } 
        }
     
    }


   

    private void DestroyItem()
    {
        if (_parent.childCount!=0)
        {
            for (int i = _parent.childCount - 1; i >= 0; i--)
            {            
                DestroyImmediate(_parent.GetChild(i).gameObject);
            }
        }
    }

}
