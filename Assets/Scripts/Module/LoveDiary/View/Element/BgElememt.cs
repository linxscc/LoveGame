using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BgElememt :DiaryElementBase
{

    private void Awake()
    {
        _rect = transform.GetComponent<RectTransform>();
    }
    public void SetBg(int id)
    {
        pb.ElementId = id;
       GetComponent<RawImage>().texture=  ResourceManager.Load<Texture>("LoveDiary/Element/" + id.ToString(), ModuleConfig.MODULE_LOVEDIARY);
    }
    protected override void UpdateView()
    {
        SetBg(pb.ElementId);
    }
}
