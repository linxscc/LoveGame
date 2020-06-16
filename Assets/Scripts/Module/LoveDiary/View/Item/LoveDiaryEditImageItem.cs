using Assets.Scripts.Framework.GalaSports.Core.Events;
using Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Com.Proto;
using game.main;

public class LoveDiaryEditImageItem : LoveDiaryEditItemBase
{
   
    void Awake()
    {

    }
    // Use this for initialization
    void Start () {
		
	}
    public override void SetData(ElementPB pb, bool isLock = false)
    {
        base.SetData(pb);
        transform.Find("Bg/Image").GetComponent<Image>().sprite = AssetManager.Instance.GetSpriteAtlas("UIAtlas_LoveDiary_Element_" + pb.Id);
        transform.Find("Lock").gameObject.SetActive(isLock);
        if (isLock == false)
            return;
        transform.Find("Lock/Text").GetComponent<Text>().text = pb.UnlockClaim.Gem.ToString();
    }

}
