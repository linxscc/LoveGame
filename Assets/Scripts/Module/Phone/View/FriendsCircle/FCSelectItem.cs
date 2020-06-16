using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FCSelectItem : MonoBehaviour {


    public void SetData(string str)
    {
        string showStr = "";
        if(str.Length>10)
        {
            showStr = str.Substring(0, 10)+"...";
        }
        else
        {
            showStr = str;
        }
        transform.Find("Text").GetComponent<Text>().text = showStr;
        AjustSize();
    }
    private void AjustSize()
    {
        float hight = transform.Find("Text").GetComponent<Text>().preferredHeight;
        transform.gameObject.GetComponent<RectTransform>().SetHeight(hight+60);
        //_otherCommentBg.gameObject.GetComponent<RectTransform>().SetSize(new Vector2(830, _otherComment.preferredHeight));
        //_otherCommentBg.gameObject.GetComponent<LayoutElement>().preferredHeight = _otherComment.preferredHeight;
    }
}
