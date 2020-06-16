using System.Collections.Generic;
using game.main;
using game.tools;
using Spine.Unity;
using UnityEngine;


public partial class StoryView
{
    private List<SkeletonGraphic> _spineCache;
    
    private void CreateSpine(EntityVo entity, int spineIndex, int childIndex)
    {
        GameObject go = null;
        if (_spineCache.Count < spineIndex)
        {
            go = InstantiatePrefab("Spine/SpineSkeletonGraphic");
            SkeletonGraphic skeletonGraphic = go.GetComponent<SkeletonGraphic>();
            _spineCache.Add(skeletonGraphic);
        }
        else
        {
            go = _spineCache[spineIndex - 1].gameObject;
        }

        SpineHeadAnimation sha = go.GetComponent<SpineHeadAnimation>();
        sha.LoadAnimation(entity.id, entity.playableList);

        go.transform.SetParent(transform, false);

        RectTransform rt = go.GetComponent<RectTransform>();
        rt.pivot = new Vector2(0.5f, 0);
        rt.sizeDelta = new Vector2(entity.width, entity.height);
        rt.anchoredPosition = new Vector2(entity.x, entity.y - _offsetY);
        
        rt.SetSiblingIndex(childIndex);
    }

    
}