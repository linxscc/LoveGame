using System;
using System.Collections.Generic;
using System.IO;
using AssetBundleTool;
using Assets.Scripts.Module.Download;
using Framework.Utils;
using UnityEditor;
using UnityEngine;


public class HotfixFileHandler : Editor
{
    private static int _minutes = 60;
    
    [MenuItem("Hotfix/筛选文件", false, 1)]
    static public void FilterHotfixFile()
    {
        DirectoryInfo info = new DirectoryInfo(HotfixBuild.HotfixDir);
        FileInfo[] arr = info.GetFiles("*.*", SearchOption.AllDirectories);
        for (int i = 0; i < arr.Length; i++)
        {
            if (arr[i].Extension == ".meta" || arr[i].Extension == ".manifest")
            {
                arr[i].Delete();
            }
        }
    }
    
    [MenuItem("Hotfix/GenerateHotfixIndexFile", false, 2)]
    static public void GenerateHotfixIndexFile()
    {
        HotfixIndexFile indexFile = new HotfixIndexFile();

#if UNITY_IOS
        FileInfo mainfestFile = new FileInfo(HotfixBuild.BundleFilesDir + "/iOS");
#else
        FileInfo mainfestFile = new FileInfo(HotfixBuild.BundleFilesDir + "/Android");
#endif
        if(mainfestFile.Exists)
            indexFile.AddFile(mainfestFile);


        FileInfo[] files = GetDirFiles(HotfixBuild.HotfixDir);
        for (int i = 0; i < files.Length; i++)
        {
            indexFile.AddFile(files[i]);
        }
        
        int version = HotfixBuild.GetHotfixVersion();
        string fileName = HotfixBuild.HotfixDir + "/HotfixConfig_v" + version + ".zip";
        indexFile.GenerateBin(fileName, HotfixFileFormat.EncryptZip, version);

        ClipBoard.Copy(MD5Util.Get(fileName));
        
        EditorUtility.DisplayDialog("GenerateHotfixIndexFile", "生成HotfixIndexFile完成！", "Close");
    }

    private static FileInfo[] GetDirFiles(string hotfixDir)
    {
        DirectoryInfo info = new DirectoryInfo(hotfixDir);
        DirectoryInfo[] dirs = info.GetDirectories("*", SearchOption.TopDirectoryOnly);

        List<FileInfo> list = new List<FileInfo>();
        foreach (var dir in dirs)
        {
            FileInfo[] arr = GetHotfixFilesInDir(dir.FullName);
            foreach (var fileInfo in arr)
            {
                list.Add(fileInfo);
            }
        }

        return list.ToArray();
    }

    static public FileInfo[] GetHotfixFilesInDir(string dir)
    {
        DirectoryInfo info = new DirectoryInfo(dir);

        FileInfo[] arr = info.GetFiles("*.*", SearchOption.AllDirectories);

        List<FileInfo> list = new List<FileInfo>();
        for (int i = 0; i < arr.Length; i++)
        {
            string ext = arr[i].Extension;
            if (ext == ".atlas" || ext == ".bytes" || ext == ".music" || ext == ".prefab" || ext == ".txt" || ext == ".json" || ext == ".csv")
            {
                list.Add(arr[i]);
            }
        }

        return list.ToArray();
    }
    
    static void FindNewBundleAndFiles()
    {
        _minutes = int.Parse(GetJenkinsParameter("minutes"));
        FindNewBundle();
        FindFiles();
    }
    
    [MenuItem("Hotfix/CopyNewBundle", false, 1000)]
    static void FindNewBundle()
    {
        string originalBundlePath = AssetBundleHelper.GetOriginalBundlesPath().Replace("\\", "/") + "/";
        string[] files = Directory.GetFiles(PackageManager.GetAssetBundleDir(), "*", SearchOption.AllDirectories);

        int count = 0;
        for (int i = 0; i < files.Length; i++)
        {
            string filePath = files[i];
            if (filePath.EndsWith(".manifest"))
                continue;
            
            string relatePath = filePath.Replace("\\", "/").Replace(originalBundlePath, "");

            FileInfo file = new FileInfo(filePath);
            TimeSpan nowSpan = new TimeSpan(DateTime.Now.Ticks);
            TimeSpan lastSpan = new TimeSpan(file.LastWriteTime.Ticks);
            if (nowSpan.TotalMilliseconds -  lastSpan.TotalMilliseconds < 1000 * 60 * _minutes)
            {
                count++;
                CopyFileToHotfix(file, relatePath);
            }
        }
        
        Debug.Log("FindNewBundle Count=>" + count);
    }
    
    [MenuItem("Hotfix/CopyNewFiles", false, 1001)]
    static void FindFiles()
    {
        string i18NDataPath = PackageManager.I18NDataPath;
        string[] files = Directory.GetFiles(i18NDataPath, "*", SearchOption.AllDirectories);

        int count = 0;
        for (int i = 0; i < files.Length; i++)
        {
            string filePath = files[i];
            
            string relatePath = filePath.Replace("\\", "/").Replace(i18NDataPath, "");

            FileInfo file = new FileInfo(filePath);
            TimeSpan nowSpan = new TimeSpan(DateTime.Now.Ticks);
            TimeSpan lastSpan = new TimeSpan(file.LastWriteTime.Ticks);
            if (nowSpan.TotalMilliseconds -  lastSpan.TotalMilliseconds < 1000 * 60 * _minutes)
            {
                count++;
                CopyFileToHotfix(file, relatePath);
            }
        }
        
        Debug.Log("FindNewBundle Count=>" + count);
    }

    private static void CopyFileToHotfix(FileInfo file, string relatePath)
    {
        string destPath = HotfixBuild.HotfixDir + "/" + relatePath;
        Debug.Log("CopyFileToHotfix===> "+destPath);

        string destDir = Path.GetDirectoryName(destPath);
        if (Directory.Exists(destDir) == false)
            Directory.CreateDirectory(destDir);
        
        File.Copy(file.FullName, destPath, true);
    }
    
        
    /// <summary>
    /// 通过key匹配接收到的Jenkins传输的参数,返回value或者""
    /// </summary>
    static string GetJenkinsParameter(string key)
    {
        foreach (string arg in Environment.GetCommandLineArgs())
        {
            if (arg.Contains(key))
            {
                string value = arg.Replace("{=}", "=").Split('=')[1];
                Console.WriteLine("key-->" + key + "    value-->" + value);
                return value;
            }
        }
        return "";
    }
}