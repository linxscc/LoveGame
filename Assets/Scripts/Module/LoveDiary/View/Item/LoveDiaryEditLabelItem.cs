using Com.Proto;
using System.Collections;
using System.Collections.Generic;
using game.main;
using UnityEngine;
using UnityEngine.UI;

public class LoveDiaryEditLabelItem : LoveDiaryEditItemBase {

    public override void SetData(ElementPB pb, bool isLock = false)
    {
        base.SetData(pb);
        transform.Find("Image/Text").GetComponent<Text>().text = pb.Name;
    }

    public void SetSelectState(bool b)
    {
        transform.Find("Select").gameObject.SetActive(b);
    }

    public void SetViewBg(int idx)
    {
        int index = idx % 4;
        transform.Find("Image").GetComponent<Image>().sprite =
            AssetManager.Instance.GetSpriteAtlas("UIAtlas_LoveDiary_Edit__SelectLabelItem" + index);
        
    }

}
