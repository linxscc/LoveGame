using System.Diagnostics;
using System.IO;
using System.Net;
using AssetBundleTool;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;
using FileUtil = Framework.Utils.FileUtil;

public class PackagePC : Editor
{
    public const string Menu_DownloadAllBundle = "AssetBundle/打包机器交互/下载所有Bundle";

    public const string Menu_DownloadOriginalBundles = "AssetBundle/打包机器交互/下载OriginalBundles";

    public const string Menu_DownloadPackageFiles = "AssetBundle/打包机器交互/下载PackageFiles";
    
    public const string Menu_DownloadFirstRunBunlde = "AssetBundle/打包机器交互/下载最少运行Bundle+IndexFiles)";
    
    
    private static bool isAndroid => EditorUserBuildSettings.selectedBuildTargetGroup == BuildTargetGroup.Android;

    public static string GetPackageFilesDir => Application.dataPath.Replace("Assets", "PackageFiles");
    public static string GetPackagePcRoot
    {
        get
        {
            string path = @"\\192.168.1.134";
#if UNITY_EDITOR_OSX
            path = "/Volumes";
#endif
            return path;
        }
    }

    public static string Folder
    {
        get
        {
            string folder = "SuperStarGameIOS";
            if (isAndroid)
            {
                folder = "SuperStarGame";
            }
            return folder;
        }
    }

    static void UploadRes()
    {
//        FtpWebRequest ftpWebRequest = FtpWebRequest.Create("ftp://192.168.1.243:40000")
    }

    [MenuItem(Menu_DownloadOriginalBundles)]
    private static void DownloadOriginalBundles()
    {
        CleanOriginalBundles();
        
        Stopwatch sw = Stopwatch.StartNew();
        FileUtil.CopyFolder(GetPackagePcRoot + "/" + Folder + "/OriginalBundles", AssetBundleHelper.GetOriginalBundlesPath(), ".manifest");
        Debug.LogWarning("复制OriginalBundles文件时间：" + sw.ElapsedMilliseconds/1000.0f + "ms");
    }

    [MenuItem(Menu_DownloadPackageFiles)]
    private static void DownloadPackageFiles()
    {
        CleanPackageFiles();
        
        Stopwatch sw = Stopwatch.StartNew();
        FileUtil.CopyFolder(GetPackagePcRoot + "/" + Folder + "/PackageFiles", GetPackageFilesDir);
        Debug.LogWarning("复制PackageFiles文件时间：" + sw.ElapsedMilliseconds/1000.0f + "ms");
    }

    [MenuItem(Menu_DownloadFirstRunBunlde)]
    static void DownloadFirstRunBunlde()
    {
        Stopwatch sw = Stopwatch.StartNew();
        CleanIndexFiles();
        PublishRes.DeleteOuterStreamingAssets_AssetBundles();
        Debug.LogWarning("删除文件时间：" + sw.ElapsedMilliseconds/1000.0f + "ms");
        
        sw.Restart();
        FileUtil.CopyFolder(GetPackagePcRoot + "/" + Folder + "/StreamingAssets/AssetBundles",
            AssetBundleHelper.GetOuterStreamingAssetsPath() + "/AssetBundles");
        Debug.LogWarning("复制OuterStreamingAssetsPath文件时间：" + sw.ElapsedMilliseconds/1000.0f + "ms");
        
        sw.Restart();
        FileUtil.CopyFolder(GetPackagePcRoot + "/" + Folder + "/StreamingAssets/IndexFiles",
            AssetBundleHelper.GetOuterStreamingAssetsPath() + "/IndexFiles");
        Debug.LogWarning("复制IndexFiles文件时间：" + sw.ElapsedMilliseconds/1000.0f + "ms");
    }

    private static void CleanPackageFiles()
    {
        if(Directory.Exists(GetPackageFilesDir))
            Directory.Delete(GetPackageFilesDir, true);
    }
    
    private static void CleanIndexFiles()
    {
        if(Directory.Exists(AssetBundleHelper.GetOuterStreamingAssetsPath() + "/IndexFiles"))
            Directory.Delete(AssetBundleHelper.GetOuterStreamingAssetsPath() + "/IndexFiles", true);
    }

    private static void CleanOriginalBundles()
    {
        string dir = AssetBundleHelper.GetOriginalBundlesPath();
        if(Directory.Exists(dir))
            Directory.Delete(dir, true);
    }
}