using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Text.RegularExpressions;
using FileUtil = Framework.Utils.FileUtil;


public class TextReplace : Editor
{
    [MenuItem("Batch/查找所有预制体Text")]
    static void Init()
    {
        Debug.Log("查找开始，请等待……");
        string logpath = Application.dataPath + "\\textInfo.csv";
        string kTexDir = Application.dataPath; //+ kRootPath
        string[] kFileList = Directory.GetFiles(kTexDir, "*.prefab", SearchOption.AllDirectories);
        Debug.Log("kFileList==>" + kFileList.Length);
        List<string> kList = new List<string>(kFileList);

        StreamWriter sw = new StreamWriter(logpath, true);

        foreach (string file in kList)
        {
            int nPos = file.IndexOf("Assets");
            string kTexAssetPath = file.Substring(nPos);

            GameObject kUIObj = AssetDatabase.LoadAssetAtPath(kTexAssetPath, typeof(GameObject)) as GameObject;

            GameObject obj = PrefabUtility.InstantiatePrefab(kUIObj) as GameObject;
            if (obj == null)
            {
                continue;
            }

            Text[] childrenText = obj.transform.GetComponentsInChildren<Text>(true);
            for (int i = 0; i < childrenText.Length; i++)
            {
                string txt = Regex.Escape(childrenText[i].text);
                sw.WriteLine(kTexAssetPath + ":" + obj.name + "/" +
                             FindPathEditor.GetChildPahth(obj, childrenText[i].gameObject) +
                             childrenText[i].transform.GetSiblingIndex() + "," +
                             txt);
                DestroyImmediate(obj);
            }

            sw.Flush();
            sw.Close();


            Debug.Log("text查找结束");
        }
    }


    [MenuItem("Batch/替换预制体Text")]
    static void ReplaceText()
    {
//        EditorUtility.DisplayProgressBar("提示", "开始加载CSV文件", 0f);


        string vFile = new DirectoryInfo(Application.dataPath).Parent.FullName + "/battle.csv";

        string str = FileUtil.ReadFileText(vFile);
        var strings = str.Split(new char[] {'\n'}, StringSplitOptions.RemoveEmptyEntries);

        Dictionary<string, List<string>> prefabsDict = new Dictionary<string, List<string>>();
        int count = 0;
        foreach (var line in strings)
        {
            string trim = line.Trim();
            if (string.IsNullOrEmpty(trim))
                continue;

            string[] arr = trim.Split(new char[] {','}, 2, StringSplitOptions.RemoveEmptyEntries);
            string path = arr[0].Trim();
            string[] separator = new[] {".prefab:"};
            arr = path.Split(separator, 2, StringSplitOptions.RemoveEmptyEntries);

            string prefab = arr[0] + ".prefab";

            if (prefabsDict.ContainsKey(prefab) == false)
            {
                prefabsDict.Add(prefab, new List<string>());
            }

            try
            {
                prefabsDict[prefab].Add(arr[1]);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            count++;
        }

//        EditorUtility.DisplayProgressBar("提示", "替换Text （0/" + count + "）", 0.1f);

        count = 0;
        foreach (var dict in prefabsDict)
        {
//            if (count > 1)
//                break;

            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(dict.Key);
            GameObject go = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
            foreach (string textPath in dict.Value)
            {
                //去掉最后的数字
                str = textPath.Substring(0, textPath.Length - 1);

                //去掉开头
                int index = str.IndexOf("/", StringComparison.Ordinal);
                str = str.Substring(index + 1);

                int num;

                if (int.TryParse(str[str.Length - 1].ToString(), out num))
                {
                    str = str.Substring(0, str.Length - 1);
                }

                GameObject targetText;
                try
                {
                    targetText = go.transform.Find(str).gameObject;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }

                TextI18N textI18N = targetText.GetComponent<TextI18N>();
                Text text = targetText.GetComponent<Text>();

                if (text.GetType().Name.Contains("TextI18N"))
                {
                    EditorUtility.DisplayDialog("Error!", dict.Key + "->" + str + "里面已经存在TextI18N", "Close");
                    return;
                }

                string childtext = text.text;
                Font childFont = text.font;
                FontStyle childFontStyle = text.fontStyle;
                int childFontSize = text.fontSize;
                float childSpacing = text.lineSpacing;
                bool childRichText = text.supportRichText;
                TextAnchor childAlignment = text.alignment;
                bool alignByGeometry = text.alignByGeometry;

                HorizontalWrapMode horizontalOverflow = text.horizontalOverflow;
                VerticalWrapMode verticalOverflow = text.verticalOverflow;
                bool resizeTextForBestFit = text.resizeTextForBestFit;
                Color color = text.color;
                Material material = text.material;
                bool raycastTarget = text.raycastTarget;

                DestroyImmediate(text);
                if (textI18N == null)
                    textI18N = targetText.AddComponent<TextI18N>();

                textI18N.TestString = childtext;
                textI18N.font = childFont;

                if (childFont.name == "lantingTeHei")
                {
                    string[] bundles = AssetDatabase.GetAssetPathsFromAssetBundle("fonts/secondfont.bytes");
                    textI18N.CustomFont = AssetDatabase.LoadAssetAtPath<I18NFont>(bundles[0]);
                }
                else if (childFont.name == "huaWenYuanTi")
                {
                    string[] bundles = AssetDatabase.GetAssetPathsFromAssetBundle("fonts/thirdfont.bytes");
                    textI18N.CustomFont = AssetDatabase.LoadAssetAtPath<I18NFont>(bundles[0]);
                }
                else
                {
                    string[] bundles = AssetDatabase.GetAssetPathsFromAssetBundle("fonts/mainfont.bytes");
                    textI18N.CustomFont = AssetDatabase.LoadAssetAtPath<I18NFont>(bundles[0]);
                }

                textI18N.fontStyle = childFontStyle;
                textI18N.fontSize = childFontSize;
                textI18N.lineSpacing = childSpacing;
                textI18N.supportRichText = childRichText;
                textI18N.alignment = childAlignment;
                textI18N.alignByGeometry = alignByGeometry;
                textI18N.horizontalOverflow = horizontalOverflow;
                textI18N.verticalOverflow = verticalOverflow;
                textI18N.resizeTextForBestFit = resizeTextForBestFit;
                textI18N.color = color;
                textI18N.material = material;
                textI18N.raycastTarget = raycastTarget;
            }


            Debug.Log("<color='#CDEA11'>" + dict.Key + "</color>");

            PrefabUtility.ReplacePrefab(go, prefab);

            DestroyImmediate(go);

//            count++;
        }


        EditorUtility.ClearProgressBar();
    }


    [MenuItem("Batch/替换所有预制体的Text")]
    static void ReplaceAllText()
    {
        EditorUtility.ClearProgressBar();
        
        string[] kFileList = Directory.GetFiles(Application.dataPath, "*.prefab", SearchOption.AllDirectories);
        List<string> pList = new List<string>(kFileList);

        EditorUtility.DisplayProgressBar("提示", "替换中（0/" + pList.Count + "）", 0f);

        List<string> prefabList = new List<string>();
        foreach (var file in pList)
        {
            if (file.ToLower().Contains("resources") || file.ToLower().Contains("streamingassets"))
                continue;
            
            prefabList.Add(file);
        }
        
        int count = 0;
        foreach (string file in prefabList)
        {
            int nPos = file.IndexOf("Assets");
            string kTexAssetPath = file.Substring(nPos);

            GameObject prefab = AssetDatabase.LoadAssetAtPath(kTexAssetPath, typeof(GameObject)) as GameObject;

            GameObject go = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
            if (go == null)
                continue;

            Text[] childrenText = go.transform.GetComponentsInChildren<Text>(true);
            foreach (var text in childrenText)
            {
                if (text.GetType().Name.Contains("TextI18N"))
                {
                    EditorUtility.DisplayDialog("Error!",
                        go.name + "/" + FindPathEditor.GetChildPahth(go, text.gameObject) + "里面已经存在TextI18N",
                        "Close");
                    return;
                }

                GameObject textParent = text.gameObject;

                string childtext = text.text;
                Font childFont = text.font;
                FontStyle childFontStyle = text.fontStyle;
                int childFontSize = text.fontSize;
                float childSpacing = text.lineSpacing;
                bool childRichText = text.supportRichText;
                TextAnchor childAlignment = text.alignment;
                bool alignByGeometry = text.alignByGeometry;

                HorizontalWrapMode horizontalOverflow = text.horizontalOverflow;
                VerticalWrapMode verticalOverflow = text.verticalOverflow;
                bool resizeTextForBestFit = text.resizeTextForBestFit;
                Color color = text.color;
                Material material = text.material;
                bool raycastTarget = text.raycastTarget;

                DestroyImmediate(text);

                TextI18N textI18N = textParent.AddComponent<TextI18N>();

                textI18N.TestString = childtext;
                textI18N.font = childFont;

                if (childFont == null)
                {
                    string[] bundles = AssetDatabase.GetAssetPathsFromAssetBundle("fonts/mainfont.bytes");
                    textI18N.CustomFont = AssetDatabase.LoadAssetAtPath<I18NFont>(bundles[0]);
                }
                else
                {
                    if (childFont.name == "lantingTeHei")
                    {
                        string[] bundles = AssetDatabase.GetAssetPathsFromAssetBundle("fonts/secondfont.bytes");
                        textI18N.CustomFont = AssetDatabase.LoadAssetAtPath<I18NFont>(bundles[0]);
                    }
                    else if (childFont.name == "huaWenYuanTi")
                    {
                        string[] bundles = AssetDatabase.GetAssetPathsFromAssetBundle("fonts/thirdfont.bytes");
                        textI18N.CustomFont = AssetDatabase.LoadAssetAtPath<I18NFont>(bundles[0]);
                    }
                    else
                    {
                        string[] bundles = AssetDatabase.GetAssetPathsFromAssetBundle("fonts/mainfont.bytes");
                        textI18N.CustomFont = AssetDatabase.LoadAssetAtPath<I18NFont>(bundles[0]);
                    }
                }

                textI18N.fontStyle = childFontStyle;
                textI18N.fontSize = childFontSize;
                textI18N.lineSpacing = childSpacing;
                textI18N.supportRichText = childRichText;
                textI18N.alignment = childAlignment;
                textI18N.alignByGeometry = alignByGeometry;
                textI18N.horizontalOverflow = horizontalOverflow;
                textI18N.verticalOverflow = verticalOverflow;
                textI18N.resizeTextForBestFit = resizeTextForBestFit;
                textI18N.color = color;
                textI18N.material = material;
                textI18N.raycastTarget = raycastTarget;
            }
            
            count++;
            
            PrefabUtility.ReplacePrefab(go, prefab);
            DestroyImmediate(go);
            
            EditorUtility.DisplayProgressBar("提示", "替换中（"+count+"/" + prefabList.Count + "）" + kTexAssetPath, 0f);
            Debug.Log("<color='#CDEA11'>" + kTexAssetPath + "</color>");
        }
        EditorUtility.ClearProgressBar();
    }
    
    [MenuItem("Batch/查找新增I18N文本", false, 0)]
    static void FindAdditionalI18NText()
    {
        //先创建一个中文的字典，然后创建一个HK的字典！最后对比K值
        var _zhcnlanguageDict = new Dictionary<string, string>();
        var _zhhklanguageDict = new Dictionary<string, string>();  

        char[] separator = new char[] { '=' };

        string filePath1 = PathUtil.GetProjectRoot() + "I18NData/zh-cn/Languages/zh-CN.txt";//外部路径
        string str1 = FileUtil.ReadFileText(filePath1);
        var strings = str1.Split(new char[] { '\n'}, StringSplitOptions.RemoveEmptyEntries);
        int count = 0;
        foreach (var line in strings)
        {
            string trim = line.Trim();
            if (string.IsNullOrEmpty(trim) || line.StartsWith("//"))
                continue;

            count++;
			
            string[] arr = trim.Split(separator, 2, StringSplitOptions.RemoveEmptyEntries);
            try
            {
                _zhcnlanguageDict.Add(arr[0].Trim(), Regex.Unescape(arr[1].Trim()));
            }
            catch (Exception e)
            {
                Debug.LogError("I18NManager lint:" + count  +"  "+e.Message);
            }
        }
        
        string filePath2 = PathUtil.GetProjectRoot() + "I18NData/zh-hk/Languages/zh-CN.txt";//外部路径
        string str2 = FileUtil.ReadFileText(filePath2);
        var strings1 = str2.Split(new char[] { '\n'}, StringSplitOptions.RemoveEmptyEntries);
        int count1 = 0;
        foreach (var line in strings1)
        {
            string trim = line.Trim();
            if (string.IsNullOrEmpty(trim) || line.StartsWith("//"))
                continue;

            count1++;
			
            string[] arr = trim.Split(separator, 2, StringSplitOptions.RemoveEmptyEntries);
            try
            {
                _zhhklanguageDict.Add(arr[0].Trim(), Regex.Unescape(arr[1].Trim()));
            }
            catch (Exception e)
            {
                Debug.LogError("I18NManager lint:" + count  +"  "+e.Message);
            }
        }

        string logpath = PathUtil.GetProjectRoot() + "\\AdditionalI18Ntext.txt";
        StreamWriter sw = new StreamWriter(logpath, true);
        List<string> additionalI18Ntext=new List<string>(); 
        foreach (var v in _zhcnlanguageDict)
        {
            if (!_zhhklanguageDict.ContainsKey(v.Key))
            {
                string trim =v.Value.Replace("\n","\\n");
                Debug.LogError(v.Key+"="+trim);   
                additionalI18Ntext.Add(v.Key+" = "+trim);
                //sw.WriteLine(v.Key + "=" + v.Value);
            }
        }

        foreach (var v in additionalI18Ntext)
        {
            sw.WriteLine(v);
        }
        
        sw.Flush();
        sw.Close();
        
        Debug.LogError("对比结束");
        _zhcnlanguageDict.Clear();
        _zhhklanguageDict.Clear();
        
    }

    [MenuItem("Batch/查找需要新增的I18N图片", false, 0)]
    static void FindAdditionalI18NAsset()
    {
//        var _zhcnassetDict = new Dictionary<string, string>();
//        var _zhhkassetDict = new Dictionary<string, string>();  
        var _zhhkassetlist = new List<string>();
        
        
        string filePathzhHK=PathUtil.GetProjectRoot() + "I18NAssets/zh-hk";//外部路径
        
        string[] hKfiles=Directory.GetFiles(filePathzhHK, "*.*", SearchOption.AllDirectories);

        foreach (var file in hKfiles)
        {
            //Debug.LogError(file.Replace(filePathzhHK,""));
            _zhhkassetlist.Add(file.Replace(filePathzhHK,""));
        }
        
        string filePathzhcn=PathUtil.GetProjectRoot() + "I18NAssets/zh-cn";//外部路径
        string[] cnfiles=Directory.GetFiles(filePathzhcn, "*.*", SearchOption.AllDirectories);

        foreach (var cnfile in cnfiles)
        {
            var cnfilename = cnfile.Replace(filePathzhcn, "");
            if (!_zhhkassetlist.Contains(cnfilename))
            {
                Debug.LogError(cnfilename);
            }

        }
        
        
        
        
        
        
    }
    
}