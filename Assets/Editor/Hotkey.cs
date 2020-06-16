using System;
using System.IO;
using AssetBundleTool;
using UnityEditor;
using UnityEngine;

public class Hotkey
{
    

    [MenuItem("Hotkey/Run _F12", false, 0)]
    static void RunApp()
    {
        EditorApplication.ExecuteMenuItem("XLua/Hotfix Inject In Editor");
        EditorApplication.isPlaying = !EditorApplication.isPlaying;
    }

    

    [MenuItem("Hotkey/FindChinese _F8", false, 0)]
    static void FindChinese()
    {
        string[] arr = {
            
            Application.streamingAssetsPath+@"/AssetBundles/Android",
            Application.dataPath+@"/BundleAssets",
            @"C:\unityproject\SuperStarGame\AndroidProject\SuperStarGame\src\main\assets\AssetBundles\Android",
            @"F:\资料\backup\soccergame1.0\apkbuild\publish\MOS"
        };
        
//        @"C:\unityproject\SuperStarGame\SuperStarGame\Assets\BundleAssets",
//        @"C:\unityproject\SuperStarGame\SuperStarGame\Assets\StreamingAssets\AssetBundles\Android",
//        @"C:\unityproject\SuperStarGame\AndroidProject\SuperStarGame\src\main\assets\AssetBundles\Android"
        
        for (int k = 0; k < arr.Length; k++)
        {
            string path = arr[k];

            if (!Directory.Exists(path))
                continue;
            
            string[] files = Directory.GetFiles(path, "*", SearchOption.AllDirectories);
            int count = 0;
            for (int i = 0; i < files.Length; i++)
            {
                FileInfo fileInfo = new FileInfo(files[i]);

                string str = fileInfo.Name;
                for (int j = 0; j < str.Length; j++)
                {
                    if ((int) str[j] > 128)
                    {
                        count++;
                        Debug.LogError("XXXXXXX=>"+fileInfo.FullName);
                        break;
                    }
                }
            }
        
            Debug.LogError("============FindChinese count=>" + count + "\nPath: "+path);
        }
        
        
    }

//    [InitializeOnLoadMethod]
//    static void OnHierarchy()
//    {
//        
//    }
}