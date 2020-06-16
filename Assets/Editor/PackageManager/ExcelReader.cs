using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Assets.Scripts.Module.Download;
using Framework.Utils;
using Newtonsoft.Json;
using OfficeOpenXml;
using QFramework;
using UnityEditor;
using Utils;
using Debug = UnityEngine.Debug;
using FileUtil = Framework.Utils.FileUtil;


public class ExcelReader
{
    
    [MenuItem("AssetBundle/TTTT")]
    static void Test()
    {
        // FileStream fileStream = new FileStream(
        //     @"C:\Users\admin\AppData\LocalLow\DefaultCompany\SuperStarGame\Download\zh-cn\Android\AppStart\AppStart.zip"
        //     , FileMode.OpenOrCreate, FileAccess.Write, FileShare.Write, 59102150);
        //
        // byte[] buffer = new byte[59902150];
        // fileStream.Write(buffer, 0, buffer.Length);
        //
        // fileStream.Flush();
        //
        // fileStream.Close();

        for (int i = 0; i < Selection.objects.Length; i++)
        {
            var item = Selection.objects[i];
            if (AssetDatabase.GetAssetPath(item) != null)
            {
                Debug.Log(AssetDatabase.GetAssetPath(item));
            }
        }

        
    }
    public const string MenuPackageOptimize_Excel = "AssetBundle/===包体优化===/从Excel配置处理";

    private static string _bundlePath;

    public static string BundlePath
    {
        get
        {
            if (_bundlePath == null)
                _bundlePath = PackageManager.GetAssetBundleDir().Replace("\\", "/") + "/";

            return _bundlePath;
        }
    }

    [MenuItem(MenuPackageOptimize_Excel)]
    public static void DoExcelConfig()
    {
        Stopwatch sw = Stopwatch.StartNew();
        
        string path = PackageManager.GitRoot + "/包体优化.xlsx";

        PackageManager.sharedAudioDict = null;
        PackageManager.FindSharedAudio();
        
        ExcelPackage excelPackage = new ExcelPackage(new FileInfo(path));

        ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets[ResPath.AppStart];
        CreateSingleZipAndJson(worksheet, ResPath.AppStart);

        worksheet = excelPackage.Workbook.Worksheets[ResPath.Extend];
        CreateSingleZipAndJson(worksheet, ResPath.Extend);

        worksheet = excelPackage.Workbook.Worksheets[ResPath.Backend];
        CreateMultiZipAndJson(worksheet, ResPath.Backend);

        worksheet = excelPackage.Workbook.Worksheets[ResPath.Function];
        CreateFunctionZipAndJson(worksheet, ResPath.Function);

        worksheet = excelPackage.Workbook.Worksheets[ResPath.Special];
        CreateSpecial(worksheet, ResPath.Special);
        
        Debug.Log("<color='#00ff00'>包体优化总时间->"+ sw.ElapsedMilliseconds +"</color>");
    }

    private static void CreateSpecial(ExcelWorksheet worksheet, string resId)
    {
        ResIndex resIndex = new ResIndex();
        resIndex.packageDict = new Dictionary<string, ResPack>();

//        1内容	2筛选路径	3白名单	4黑名单 5ID
        var row = worksheet.Dimension.End.Row;

        List<BundleFilter> filterList = new List<BundleFilter>();
        BundleFilter filter = null;
        for (int i = 2; i < row; i++)
        {
            object date = worksheet.Cells[i, 2].Value;
            if (date != null)
            {
                filter = new BundleFilter();
                filterList.Add(filter);
                filter.DirPath = date.ToString().Replace("\\", "/");

                string rootFolder = filter.DirPath;

                rootFolder = PackageManager.GetAssetBundleDir() + "/" + rootFolder;

                filter.BlackList = new BlackList();
                filter.WhiteList = new WhiteList();

                filter.BlackList.RootFolder = rootFolder;
                filter.WhiteList.RootFolder = rootFolder;

                AddToNameList(filter.WhiteList, worksheet.Cells[i, 3].Value);
                AddToNameList(filter.BlackList, worksheet.Cells[i, 4].Value);

                filter.Id = worksheet.Cells[i, 5].Value.ToString();
            }
            else
            {
                AddToNameList(filter.WhiteList, worksheet.Cells[i, 3].Value);
                AddToNameList(filter.BlackList, worksheet.Cells[i, 4].Value);
            }
        }

        Dictionary<string, FileInfo> fileDict = new Dictionary<string, FileInfo>();
        foreach (var bundleFilter in filterList)
        {
            List<FileInfo> files = bundleFilter.SearchFile();
            foreach (var fileInfo in files)
            {
                string key = fileInfo.FullName.Replace("\\", "/");
                if (!fileDict.ContainsKey(key))
                {
                    fileDict.Add(key, fileInfo);
                }
            }
            string releasePath = "AssetBundles/" + PackageManager.SystemTag;

            ResPack resPack = new ResPack();
            resPack.id = bundleFilter.Id;
            resPack.downloadPath = PackageManager.GetPackageStorePath() + "/" +resId;
            resPack.releasePath = releasePath;
            resPack.items = new List<ResItem>();
            resIndex.packageDict.Add(resPack.id, resPack);
            foreach (var fileInfo in fileDict)
            {
                ResItem resItem = new ResItem();
                resItem.Path = GetRelatePath(fileInfo.Value.FullName);
                resItem.Md5 = MD5Util.Get(fileInfo.Key);
                resItem.Size = fileInfo.Value.Length;
                resPack.packageType = FileType.Original;
                resPack.items.Add(resItem);

                CopyFile(fileInfo.Value.FullName,PackageManager.OutputPath + resId + "/" + resItem.Path);
            }
        }

        string json = JsonConvert.SerializeObject(resIndex, Formatting.Indented);
        FileUtil.SaveFileText(PackageManager.OutputPath, resId + ".json", json);
    }

    private static void CreateFunctionZipAndJson(ExcelWorksheet worksheet, string resId)
    {
//        1内容 2函数	3黑名单
        var row = worksheet.Dimension.End.Row;

        List<BundleFilter> filterList = new List<BundleFilter>();
        BundleFilter filter = null;
        for (int i = 2; i <= row; i++)
        {
            object date = worksheet.Cells[i, 2].Value;
            if (date != null)
            {
                filter = new BundleFilter();
                filterList.Add(filter);

                filter.Id = date.ToString();

                filter.BlackList = new BlackList();
                filter.WhiteList = new WhiteList();

                AddToNameList(filter.BlackList, worksheet.Cells[i, 3].Value);
            }
            else
            {
                AddToNameList(filter.BlackList, worksheet.Cells[i, 3].Value);
            }
        }

        Type type = Type.GetType("PackageManager");
        foreach (var bundleFilter in filterList)
        {
            MethodInfo method = type.GetMethod(bundleFilter.Id, BindingFlags.Static | BindingFlags.NonPublic);
            method.Invoke(null, new object[] {bundleFilter.BlackList});
        }
    }

    public static void CreateMultiZipAndJson(ExcelWorksheet worksheet, string resId)
    {
        Stopwatch sw = Stopwatch.StartNew();
        
        ResIndex resIndex = new ResIndex();

//        1内容	2筛选路径	3白名单	4黑名单 5ID
        var row = worksheet.Dimension.End.Row;

        List<BundleFilter> filterList = new List<BundleFilter>();
        BundleFilter filter = null;
        for (int i = 2; i < row; i++)
        {
            object date = worksheet.Cells[i, 2].Value;
            if (date != null)
            {
                filter = new BundleFilter();
                filterList.Add(filter);
                filter.DirPath = date.ToString().Replace("\\", "/");

                string rootFolder = filter.DirPath;

                rootFolder = PackageManager.GetAssetBundleDir() + "/" + rootFolder;

                filter.BlackList = new BlackList();
                filter.WhiteList = new WhiteList();

                filter.BlackList.RootFolder = rootFolder;
                filter.WhiteList.RootFolder = rootFolder;

                AddToNameList(filter.WhiteList, worksheet.Cells[i, 3].Value);
                AddToNameList(filter.BlackList, worksheet.Cells[i, 4].Value);

                filter.Id = worksheet.Cells[i, 5].Value.ToString();
            }
            else
            {
                AddToNameList(filter.WhiteList, worksheet.Cells[i, 3].Value);
                AddToNameList(filter.BlackList, worksheet.Cells[i, 4].Value);
            }
        }

        resIndex.belong = resId;
        resIndex.language = PackageManager.Language;
        resIndex.packageDict = new Dictionary<string, ResPack>();

        Debug.Log("<color='#00ff00'>" + resId + "时间1->"+ sw.ElapsedMilliseconds +"</color>");
        
        DeleteFile(resIndex);

        Debug.Log("<color='#00ff00'>" + resId + "时间2.1->"+ sw.ElapsedMilliseconds +"</color>");
        
        string packagePath = PackageManager.OutputPath + resId;
        if (Directory.Exists(packagePath))
            Directory.Delete(packagePath, true);
        Directory.CreateDirectory(packagePath);

        string releasePath = "AssetBundles/" + PackageManager.SystemTag;
        
        foreach (var bundleFilter in filterList)
        {
            List<FileInfo> files = bundleFilter.SearchFile();
            
            ResPack resPack = new ResPack();
            resPack.id = bundleFilter.Id;
            resPack.releasePath = releasePath;
            resPack.packageType = FileType.Zip;
            resPack.downloadPath =
                PackageManager.GetPackageStorePath() + "/" + resIndex.belong + "/" + resPack.id + ".zip";
            resPack.items = new List<ResItem>();
            resIndex.packageDict.Add(resPack.id, resPack);

            foreach (var fileInfo in files)
            {
                ResItem resItem = new ResItem();
                resItem.Path = GetRelatePath(fileInfo.FullName);
                resItem.Md5 = MD5Util.Get(fileInfo.FullName);
                resItem.Size = fileInfo.Length;
                resPack.items.Add(resItem);

                CopyFile(fileInfo.FullName, PackageManager.GetPackageDir() + "/" + resId + "/" + resItem.Path);
            }
        }

        Debug.Log("<color='#00ff00'>" + resId + "时间2.2->"+ sw.ElapsedMilliseconds +"</color>");

        foreach (var resPack in resIndex.packageDict)
        {
            string zipFileName = packagePath + "/" + resPack.Value.id + ".zip";

            List<string> fileList = new List<string>();
            string rootPath = PackageManager.GetPackageDir().Replace("\\", "/") + "/" + resId + "/";
            foreach (var item in resPack.Value.items)
            {
                fileList.Add(rootPath + item.Path);
            }

            EditorUtility.DisplayProgressBar("压缩", "正在压缩文件(" + Path.GetFileName(packagePath) + ")", 1);

            List<string> pathInZipList = new List<string>();
            for (int i = 0; i < fileList.Count; i++)
            {
                string str = Path.GetDirectoryName(fileList[i]);
                str = str.Replace("\\", "/");
                str = str.Replace(
                    PackageManager.GetPackageDir().Replace("\\", "/") + "/" + resId + "/","");
                
                pathInZipList.Add(str + "/");
            }

            ZipUtil.CreateZip(fileList.ToArray(), zipFileName, pathInZipList.ToArray(), 0);

            EditorUtility.ClearProgressBar();

            resPack.Value.packageMd5 = MD5Util.Get(zipFileName);
            resPack.Value.packageSize = new FileInfo(zipFileName).Length;
        }
        
        Debug.Log("<color='#00ff00'>" + resId + "时间3->"+ sw.ElapsedMilliseconds +"</color>");

        string json = JsonConvert.SerializeObject(resIndex, Formatting.Indented);
        FileUtil.SaveFileText(PackageManager.OutputPath, resId + ".json", json);
    }

    private static void CopyFile(string srcFile, string destFile)
    {
        string dir = Path.GetDirectoryName(destFile);
        if (Directory.Exists(dir) == false)
            Directory.CreateDirectory(dir);

        File.Copy(srcFile, destFile, true);
    }

    public static void CreateSingleZipAndJson(ExcelWorksheet worksheet, string resId)
    {
        ResIndex resIndex = new ResIndex();

//        1内容	2筛选路径 3白名单 4黑名单
        var row = worksheet.Dimension.End.Row;

        List<BundleFilter> filterList = new List<BundleFilter>();
        BundleFilter filter = null;
        for (int i = 2; i < row; i++)
        {
            object date = worksheet.Cells[i, 2].Value;
            if (date != null)
            {
                filter = new BundleFilter();
                filterList.Add(filter);
                filter.DirPath = date.ToString();

                string rootFolder = filter.DirPath;

                rootFolder = PackageManager.GetAssetBundleDir() + "/" + rootFolder;

                filter.BlackList = new BlackList();
                filter.WhiteList = new WhiteList();

                filter.BlackList.RootFolder = rootFolder;
                filter.WhiteList.RootFolder = rootFolder;

                AddToNameList(filter.WhiteList, worksheet.Cells[i, 3].Value);
                AddToNameList(filter.BlackList, worksheet.Cells[i, 4].Value);
            }
            else
            {
                AddToNameList(filter.WhiteList, worksheet.Cells[i, 3].Value);
                AddToNameList(filter.BlackList, worksheet.Cells[i, 4].Value);
            }
        }

        resIndex.belong = resId;
        resIndex.language = PackageManager.Language;
        resIndex.packageDict = new Dictionary<string, ResPack>();

        Dictionary<string, FileInfo> fileDict = new Dictionary<string, FileInfo>();

        if (resId == ResPath.AppStart)
        {
            //把共享音频放到AppStart里面下载
            string root = PackageManager.GetAssetBundleDir();
            foreach (var id in PackageManager.sharedAudioDict)
            {
                FileInfo fileInfo = new FileInfo(root + "/story/dubbing/" + id.Key + ".bytes");
                string key = fileInfo.FullName.Replace("\\", "/");
                fileDict.Add(key, fileInfo);
            }
        }
        
        foreach (var bundleFilter in filterList)
        {
            List<FileInfo> files = bundleFilter.SearchFile();
            foreach (var fileInfo in files)
            {
                string key = fileInfo.FullName.Replace("\\", "/");
                if (!fileDict.ContainsKey(key))
                {
                    fileDict.Add(key, fileInfo);
                }
            }
        }

        ResPack resPack = new ResPack();
        resPack.id = resIndex.belong;
        resPack.releasePath = "AssetBundles/" + PackageManager.SystemTag;
        resPack.downloadPath = PackageManager.GetPackageStorePath() + "/" + resIndex.belong + "/" + resPack.id + ".zip";
        resPack.items = new List<ResItem>();
        resPack.packageType = FileType.Zip;
        resIndex.packageDict.Add(resPack.id, resPack);
        foreach (var fileInfo in fileDict)
        {
            ResItem resItem = new ResItem();
            resItem.Path = GetRelatePath(fileInfo.Value.FullName);
            resItem.Md5 = MD5Util.Get(fileInfo.Key);
            resItem.Size = fileInfo.Value.Length;
            resPack.items.Add(resItem);
        }

        DeleteFile(resIndex);
        CopyFile(resIndex);

        string packagePath = PackageManager.OutputPath + resPack.id;
        if (Directory.Exists(packagePath))
            Directory.Delete(packagePath, true);
        Directory.CreateDirectory(packagePath);

        string zipFileName = packagePath + "/" + resPack.id + ".zip";

        string destDir = PackageManager.GetPackageDir() + "/" + resPack.id;
        string[] fileList = Directory.GetFiles(destDir, "*", SearchOption.AllDirectories);

        EditorUtility.DisplayProgressBar("压缩", "正在压缩文件(" + Path.GetFileName(packagePath), 1);

        List<string> pathInZipList = new List<string>();
        for (int i = 0; i < fileList.Length; i++)
        {
            string str = Path.GetDirectoryName(fileList[i]);
            str = str.Replace("\\", "/");
            str = str.Replace(PackageManager.GetPackageDir().Replace("\\", "/") + "/" + resPack.id + "/", "");
            pathInZipList.Add(str + "/");
        }

        ZipUtil.CreateZip(fileList, zipFileName, pathInZipList.ToArray(), 0);

        EditorUtility.ClearProgressBar();

        resPack.packageMd5 = MD5Util.Get(zipFileName);
        resPack.packageSize = new FileInfo(zipFileName).Length;
        string json = JsonConvert.SerializeObject(resIndex, Formatting.Indented);
        FileUtil.SaveFileText(PackageManager.OutputPath, resId + ".json", json);
    }

    private static void DeleteFile(ResIndex resIndex)
    {
        string path = PackageManager.GetPackageDir() + "/" + resIndex.belong;
        if (Directory.Exists(path))
            Directory.Delete(path, true);
    }

    private static void CopyFile(ResIndex resIndex)
    {
        foreach (var pack in resIndex.packageDict)
        {
            foreach (var item in pack.Value.items)
            {
                string destDir = PackageManager.GetPackageDir() + "/" + pack.Value.id + "/" + item.Path;

                FileInfo fileInfo = new FileInfo(destDir);
                destDir = fileInfo.DirectoryName;

                if (Directory.Exists(destDir) == false)
                    Directory.CreateDirectory(destDir);

                File.Copy(BundlePath + item.Path, fileInfo.FullName);
            }
        }
    }

    private static string GetRelatePath(string filePath)
    {
        filePath = filePath.Replace("\\", "/");
        filePath = filePath.Replace(BundlePath, "");

        return filePath;
    }

    private static void AddToNameList(NameList nameList, object obj)
    {
        if (obj != null)
        {
            nameList.List.Add(obj.ToString().Replace("\\", "/"));
        }
    }
}