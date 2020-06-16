using System.Collections.Generic;
using game.main;
using game.main.Live2d;
using game.tools;
using Spine.Unity;
using UnityEngine;

public partial class StoryView
{
    private List<Live2dGraphic> _live2DCache;
    
    private void CreateLive2D(EntityVo entity, int live2DIndex, int childIndex)
    {
        GameObject go = null;
        Live2dGraphic live2DGraphic;
        if (_live2DCache.Count < live2DIndex)
        {
            go = InstantiatePrefab(L2DConst.Live2dGraphicPath);
            live2DGraphic = go.GetComponent<Live2dGraphic>();
            _live2DCache.Add(live2DGraphic);
        }
        else
        {
            go = _live2DCache[live2DIndex - 1].gameObject;
            live2DGraphic = go.GetComponent<Live2dGraphic>();
            live2DGraphic.Show();
        }

        if (live2DGraphic.GetMainLive2DView != null && live2DGraphic.GetMainLive2DView.ModelId == entity.id)
        {
            live2DGraphic.ChangeAnimation(entity);
        }
        else
        {
            live2DGraphic.SetData(entity);
            live2DGraphic.Play();
        }
        
        go.transform.SetParent(transform, false);

        RectTransform rt = go.GetComponent<RectTransform>();
        rt.pivot = new Vector2(0.5f, 0.5f);
        rt.sizeDelta = new Vector2(entity.width, entity.height);
        rt.anchoredPosition = new Vector2(entity.x, entity.y - _offsetY);
        
        rt.SetSiblingIndex(childIndex);
    }
}