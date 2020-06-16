using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Com.Proto;
using DataModel;
using Assets.Scripts.Framework.GalaSports.Service;
using game.main;

public class PropUtils {

	public static void SetPropItemIcon(RewardVo vo, Image iconImg, string module = null, bool unloadLater = false)
    {
        if (vo.Resource == ResourcePB.Puzzle)
        {
            iconImg.enabled = false;
            GameObject go = GameObject.Instantiate(ResourceManager.Load<GameObject>("Module/Prefabs/CommonPrefabs/Puzzle", module, unloadLater), iconImg.transform, false);
            Material mat = ResourceManager.Load<Material>("Prefabs/CommonPrefabs/PuzzleMask", module, unloadLater);
            RawImage rawImg = go.transform.Find("Icon").GetComponent<RawImage>();
            rawImg.texture = ResourceManager.Load<Texture>(vo.IconPath, module, unloadLater);
            rawImg.material = mat;
        }
        else
        {
            iconImg.enabled = true;
            iconImg.sprite = ResourceManager.Load<Sprite>(vo.IconPath, module, unloadLater);
            iconImg.SetNativeSize();
        }
    }

    public static void SetPropItemIcon(RewardVo vo, RawImage iconImg, string module = null, bool unloadLater = false, bool isSetNativeSize = true)
    {
        if (vo.Resource == ResourcePB.Puzzle)
        {
            iconImg.enabled = false;
            GameObject go = GameObject.Instantiate(ResourceManager.Load<GameObject>("Module/Prefabs/CommonPrefabs/Puzzle", module, unloadLater), iconImg.transform, false);
            Material mat = ResourceManager.Load<Material>("Prefabs/CommonPrefabs/PuzzleMask", module, unloadLater);
            RawImage rawImg = go.transform.Find("Icon").GetComponent<RawImage>();
            rawImg.texture = ResourceManager.Load<Texture>(vo.IconPath, module, unloadLater);
            rawImg.material = mat;
        }
        else
        {
            iconImg.enabled = true;
            iconImg.texture = ResourceManager.Load<Texture>(vo.IconPath, module, unloadLater);
            if (isSetNativeSize)
                iconImg.SetNativeSize();
        }
    }

    public static Texture GetPropIcon(int resourceId)
    {
        return ResourceManager.Load<Texture>("Prop/" + resourceId, null, true);
    }

    public static int GetUserPropNum(int resourceId)
    {
        return (GlobalData.PropModel.GetUserProp(resourceId) == null) ? 0 : GlobalData.PropModel.GetUserProp(resourceId).Num;
    }

    public static void ShowPropDesc(RewardVo vo)
    {
        Debug.LogWarning("id:"+vo.Id + " res:"+vo.Resource);
        var desc = ClientData.GetItemDescById(vo.Id, vo.Resource);
        if (desc != null)
        {
            FlowText.ShowMessage(desc.ItemDesc);
        }
    }
}
