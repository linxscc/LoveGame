using UnityEngine;
using System.Collections;
using System.IO;

public class PathUtil
{
    public static string GetStreamAssetsPath()
    {
        string path = "";
        if (Application.platform == RuntimePlatform.Android) //安卓
        {
            path = "jar:file://" + Application.dataPath + "!/assets";
        }
        else if (Application.platform == RuntimePlatform.IPhonePlayer) //苹果
        {
            path = Path.Combine(Application.dataPath, "Raw");
        }
        else //编辑器
        {
            path = Application.streamingAssetsPath;
        }

        return path;
    }

    public static string GetProjectRoot()
    {
        return Application.dataPath.Replace("Assets", "");
    }

    /// <summary>
    /// 获取资源路径，优先使用外部资源
    /// </summary>
    /// <param name="suffix"></param>
    /// <param name="externalPath"></param>
    /// <returns></returns>
    public static string GetPath(string suffix, string externalPath)
    {
        externalPath += "/" + suffix;

        if (File.Exists(externalPath))
        {
            Debug.Log("load from externalPath====>" + externalPath);
            return externalPath;
        }
#if UNITY_EDITOR
        return GetProjectRoot() + "StreamingAssets/" + suffix;
#endif
        return GetStreamAssetsPath() + "/" + suffix;
    }

    public static string PlatfromBundlePath()
    {
        string path = "AssetBundles/Android/";
#if UNITY_IOS
        path = "AssetBundles/iOS/";
#endif
        return path;
    }
}