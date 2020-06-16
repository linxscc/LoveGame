#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class UGUIAdvance : MonoBehaviour {

//    [MenuItem("GameObject/UI/Image")]
    static void CreatImage()
    {
        if (Selection.activeTransform)
        {
            if (Selection.activeTransform.GetComponentInParent<Canvas>())
            {
                GameObject go = new GameObject("image", typeof(Image));
                go.GetComponent<Image>().raycastTarget = false;
                go.transform.SetParent(Selection.activeTransform);
                go.transform.localScale =Vector3.one;
                go.transform.localPosition =Vector3.zero;
                go.layer = Selection.activeTransform.gameObject.layer;
            }
        }
    }

//    [MenuItem("GameObject/UI/Text")]
    static void CreatText()
    {
        if (Selection.activeTransform)
        {
            if (Selection.activeTransform.GetComponentInParent<Canvas>())
            {
                GameObject go = new GameObject("Text", typeof(Text));
                Text text = go.GetComponent<Text>();
                if (text !=null)
                {
                    text.font = Resources.Load<Font>("Fonts/FZHTJW");
                    text.raycastTarget = false;
                }
             
                go.transform.SetParent(Selection.activeTransform);
                go.transform.localScale = Vector3.one;
                go.transform.localPosition = Vector3.zero;
                go.layer = Selection.activeTransform.gameObject.layer;
            }
        }
    }

}
#endif
