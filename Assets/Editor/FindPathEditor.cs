using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

/// <summary>
/// </summary>
public class FindPathEditor {


    /// <summary>快捷键 Ctrl+Shift + C ===>复制选中两个游戏对象之间的查找路径x ：transform.FindChild("路径x")
    /// </summary>
    [MenuItem("GameObject/Create Other/Copy Find Child Path _%#_ J")]
    static void CopyFindChildPath()
    {

        Object[] objAry = Selection.objects;
        //Debug.Log(objAry.Length);

        if (objAry.Length == 2)
        {
            GameObject gmObj0 = (GameObject)objAry[0];
            GameObject gmObj1 = (GameObject)objAry[1];
            List<Transform> listGameParent0 = new List<Transform>(gmObj0.transform.GetComponentsInParent<Transform>(true));
            List<Transform> listGameParent1 = new List<Transform>(gmObj1.transform.GetComponentsInParent<Transform>(true));
            System.Text.StringBuilder strBd = new System.Text.StringBuilder("");
            //gmObj0.transform.FindChild("");
            //string findCode = "gmObj0"
            if (listGameParent0.Contains(gmObj1.transform))
            {
                int startIndex = listGameParent0.IndexOf(gmObj1.transform);
                Debug.Log(startIndex);
                for (int i = startIndex; i >= 0; i--)
                {
                    if (i != startIndex)
                    {
                        strBd.Append(listGameParent0[i].gameObject.name).Append(i != 0 ? "/" : "");
                    }
                    
                }
            }
            
            if (listGameParent1.Contains(gmObj0.transform))
            {
                int startIndex = listGameParent1.IndexOf(gmObj0.transform);
                for (int i = startIndex; i >= 0; i--)
                {
                    if (i != startIndex)
                    {
                        strBd.Append(listGameParent1[i].gameObject.name).Append(i != 0 ? "/" : "");
                    }
                    
                }
            }

            TextEditor textEditor = new TextEditor();
            textEditor.text = "\"" + strBd.ToString() + "\"";// "hello world";
            textEditor.OnFocus();
            textEditor.Copy();
            string colorStr = strBd.Length > 0 ? "<color=green>" : "<color=red>";
            Debug.Log(colorStr + "复制：【\"" + strBd.ToString() + "\"】" + "</color>");
        }


    }



    public static string GetChildPahth(GameObject obj1,GameObject obj2)
    {
        GameObject gmObj0 = obj1;
        GameObject gmObj1 = obj2;
        List<Transform> listGameParent0 = new List<Transform>(gmObj0.transform.GetComponentsInParent<Transform>(true));
        List<Transform> listGameParent1 = new List<Transform>(gmObj1.transform.GetComponentsInParent<Transform>(true));
        System.Text.StringBuilder strBd = new System.Text.StringBuilder("");
        //gmObj0.transform.FindChild("");
        //string findCode = "gmObj0"
        if (listGameParent0.Contains(gmObj1.transform))
        {
            int startIndex = listGameParent0.IndexOf(gmObj1.transform);
            Debug.Log(startIndex);
            for (int i = startIndex; i >= 0; i--)
            {
                if (i != startIndex)
                {
                    strBd.Append(listGameParent0[i].gameObject.name).Append(i != 0 ? "/" : "");
                }
                    
            }
        }
            
        if (listGameParent1.Contains(gmObj0.transform))
        {
            int startIndex = listGameParent1.IndexOf(gmObj0.transform);
            for (int i = startIndex; i >= 0; i--)
            {
                if (i != startIndex)
                {
                    strBd.Append(listGameParent1[i].gameObject.name).Append(i != 0 ? "/" : "");
                }
                    
            }
        }

        return strBd.ToString();


    }
    
    [MenuItem("GameObject/Create Other/ObjectPath")]
    static void CopyGameObjectPath()
    {
        UnityEngine.Object obj = Selection.activeObject;
        if (obj == null)
        {
            Debug.LogError("You must select Obj first!");
            return;
        }
        string result = AssetDatabase.GetAssetPath(obj);
        if (string.IsNullOrEmpty(result))//如果不是资源则在场景中查找
        {
            Transform selectChild = Selection.activeTransform;
            if (selectChild != null)
            {
                result = selectChild.name;
                while (selectChild.parent != null)
                {
                    selectChild = selectChild.parent;
                    result = string.Format("{0}/{1}", selectChild.name, result);
                }
            }
        }
        ClipBoard.Copy(result);
        Debug.Log(string.Format("The gameobject:{0}'s path has been copied to the clipboard!", obj.name));
    }
}




/// <summary>
/// 剪切板
/// </summary>
public class ClipBoard
{
    /// <summary>
    /// 将信息复制到剪切板当中
    /// </summary>
    public static void Copy(string format, params object[] args)
    {
        string result = string.Format(format, args);
        TextEditor editor = new TextEditor();
        editor.content = new GUIContent(result);
        editor.OnFocus();
        editor.Copy();
    }
}
