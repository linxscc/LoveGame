using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VisitLevelPropsItem : VisitLevelItem
{
    public override void SetData(VisitLevelVo vo)
    {
        base.SetData(vo);
        int index = 2;//奖励展示写死
        string path = "Prop/"+vo.Awards[index].ResourceId.ToString();
        Debug.Log("VisitLevelPropsItem   " + path);
        Image propImage = transform.Find("Props").GetComponent<Image>();
        propImage.sprite = ResourceManager.Load<Sprite>(path, ModuleConfig.MODULE_VISIT);
        propImage.SetNativeSize();
    }
}
