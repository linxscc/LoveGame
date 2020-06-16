using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using AssetBundleTool;
using Assets.Scripts.Module.Download;
using Framework.Utils;
using game.main;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;
using Utils;
using FileUtil = Framework.Utils.FileUtil;


public class PublishRes : Editor
{
    public const string MenuPublishToStreamingAsset = "AssetBundle/===发布===/包体优化后的Bundle+索引";
    public const string MenuPublishToStreamingAssetAll = "AssetBundle/===发布===/所有的Bundle";

    public const string MenuSwitchAssetLanguage = "AssetBundle/切换资源/切换资源和文本";
    public const string MenuSwitchAssetLanguageData = "AssetBundle/切换资源/I18NData %F12";
    public const string MenuSwitchAssetLanguageAssets = "AssetBundle/切换资源/I18NAssets %F11";

    public const string MenuSwitchAssetDelteAll = "AssetBundle/切换资源/删除StreamingAssets/ALL";
    public const string MenuSwitchAssetDelteAssets = "AssetBundle/切换资源/删除StreamingAssets/AssetBundle";
    public const string MenuSwitchAssetDelteData = "AssetBundle/切换资源/删除StreamingAssets/Other";
    
    public const string MenuOneKeyPublishAllBundle = "AssetBundle/===发布===/*一键发布所有Bundle*";

    public const string MenuFiltrateCoaxSleepAudio = "AssetBundle/筛选哄睡音频";
  
    
  
    
    [MenuItem(MenuOneKeyPublishAllBundle)]
    internal static void OneKeyPublishAllBundle()
    {
        CopyI18nData(false);
        CopyI18nAssets(false);
        EditorApplication.ExecuteMenuItem(AssetBundleBuilder.MenuItem_CreateAllBundle);
        EditorApplication.ExecuteMenuItem(MenuPublishToStreamingAssetAll);
    }
    
    [MenuItem(MenuPublishToStreamingAsset)]
    static void CopyResToStreamingAssets()
    {
        DeleteOuterStreamingAssets_AssetBundles();

        Dictionary<string, bool> fileDict = new Dictionary<string, bool>();
        LoadIndexJson(ResPath.MainStoryIndex, ref fileDict);
        LoadIndexJson(ResPath.LoveStoryIndex, ref fileDict);
        LoadIndexJson(ResPath.EvoCardIndex, ref fileDict);
        LoadIndexJson(ResPath.LoveVisitIndex, ref fileDict);
        LoadIndexJson(ResPath.PhoneAudioPackageIndex, ref fileDict);
        LoadIndexJson(ResPath.AppStart, ref fileDict);
        LoadIndexJson(ResPath.Extend, ref fileDict);
        LoadIndexJson(ResPath.Backend, ref fileDict);
        LoadIndexJson(ResPath.Special, ref fileDict);
        LoadIndexJson(ResPath.IncludeIndex, ref fileDict, true);

        string originalBundlePath = AssetBundleHelper.GetOriginalBundlesPath().Replace("\\", "/") + "/";
        string streamingAssetsPath = AssetBundleHelper.GetOuterStreamingAssetsPath() + "/";
        string[] files = Directory.GetFiles(PackageManager.GetAssetBundleDir(), "*", SearchOption.AllDirectories);

//        int len = files.Length / 2 - fileDict.Count;

        string destDir = PackageManager.GetPackageDir() + "/" + ResPath.AllResources;
        if (Directory.Exists(destDir))
            Directory.Delete(destDir, true);
        
        int len = files.Length / 2;
        int count = 0;
        int inPackageCount = 0;
        for (int i = 0; i < files.Length; i++)
        {
            string filePath = files[i];
            if (filePath.EndsWith(".manifest"))
                continue;

            string relatePath = filePath.Replace("\\", "/").Replace(originalBundlePath, "");
            if (fileDict.ContainsKey(relatePath))
            {
                string path = PackageManager.GetPackageDir() + "/" + ResPath.AllResources + "/" + relatePath;
                destDir = Path.GetDirectoryName(path);
                if (Directory.Exists(destDir) == false)
                    Directory.CreateDirectory(destDir);

                File.Copy(filePath, path);
            }
            else
            {
                string destFile = streamingAssetsPath + relatePath;
                FileInfo fileInfo = new FileInfo(destFile);
                if (fileInfo.Directory.Exists == false)
                    fileInfo.Directory.Create();
                
                File.Copy(filePath, destFile);
                inPackageCount++;
            }

            count++;

            EditorUtility.DisplayProgressBar("复制文件",
                "(" + count + "/" + len + ")"  + " 包内：" + inPackageCount,
                (float) count / len);
        }

        CreateAllDownloadJsonAndZip();

        EditorUtility.ClearProgressBar();
        
        CopyIndexFileToStreamingAsset();

        Debug.Log("<color='#00ff00'>复制优化后的Bundle到StreamingAssets完成</color> 文件数量：" + count);
    }

    private static void CreateAllDownloadJsonAndZip()
    {
        //压缩文件
        string packagePath = PackageManager.OutputPath + ResPath.AllResources;

        if (Directory.Exists(packagePath))
            Directory.Delete(packagePath, true);
        Directory.CreateDirectory(packagePath);

        string zipFileName = packagePath + "/" + ResPath.AllResources + ".zip";

        string destDir = PackageManager.GetPackageDir() + "/" + ResPath.AllResources;
        string[] files = Directory.GetFiles(destDir, "*", SearchOption.AllDirectories);

        EditorUtility.DisplayProgressBar("压缩", "正在压缩文件(" + Path.GetFileName(packagePath), 1);

        List<string> pathInZipList = new List<string>();
        for (int i = 0; i < files.Length; i++)
        {
            string str = Path.GetDirectoryName(files[i]);
            str = str.Replace("\\", "/");
            str = str.Replace(PackageManager.GetPackageDir().Replace("\\", "/") + "/" + ResPath.AllResources + "/", "");
            pathInZipList.Add(str + "/");
        }

        ZipUtil.CreateZip(files, zipFileName, pathInZipList.ToArray(), 0);

        EditorUtility.ClearProgressBar();

        //生成Index文件
        ResIndex resIndex = new ResIndex();
        resIndex.language = PackageManager.Language;
        resIndex.belong = ResPath.AllResources;
        resIndex.packageDict = new Dictionary<string, ResPack>();

        ResPack resPack = new ResPack();
        resPack.id = ResPath.AllResources;
        resPack.items = new List<ResItem>();
        resPack.packageType = FileType.Zip;
        resPack.packageMd5 = MD5Util.Get(packagePath + "/" + resPack.id + ".zip");
        resPack.packageSize = new FileInfo(packagePath + "/" + resPack.id + ".zip").Length;
        resPack.downloadPath = PackageManager.GetPackageStorePath() + "/" + resIndex.belong + "/" + resPack.id + ".zip";
        resIndex.packageDict.Add(resPack.id, resPack);

        string json = JsonConvert.SerializeObject(resIndex, Formatting.Indented);
        FileUtil.SaveFileText(PackageManager.OutputPath, ResPath.AllResources + ".json", json);
    }

    private static List<CoaxSleepAudioJsonData> _sleepAudioJson;

    [MenuItem(MenuPublishToStreamingAssetAll)]
    static void CopyAllResToStreamingAssets()
    {
        DeleteOuterStreamingAssets_AssetBundles();
         
        string originalBundlePath = AssetBundleHelper.GetOriginalBundlesPath().Replace("\\", "/") + "/";
        string streamingAssetsPath = AssetBundleHelper.GetOuterStreamingAssetsPath() + "/";
               
        string[] files = Directory.GetFiles(PackageManager.GetAssetBundleDir(), "*", SearchOption.AllDirectories);

        int count = 0;
        int len = files.Length / 2;
        for (int i = 0; i < files.Length; i++)
        {
            string filePath = files[i];
            if (filePath.EndsWith(".manifest"))
                continue;
                      
            string relatePath = filePath.Replace("\\", "/").Replace(originalBundlePath, "");

            string destFile = string.Empty;
            FileInfo fileInfo = null;

            destFile = streamingAssetsPath + relatePath;
            fileInfo = new FileInfo(destFile);


            if (fileInfo.Directory.Exists == false)
                fileInfo.Directory.Create();

            count++;

            EditorUtility.DisplayProgressBar("复制文件",
                "(" + count + "/" + len + ")",
                (float) count / len);

            File.Copy(filePath, destFile,true);
        }

     
        EditorUtility.ClearProgressBar();

        Debug.Log("<color='#00ff00'>复制所有Bundle到StreamingAssets完成</color> 文件数量：" + count);
        
        FiltrateCoaxSleepAudio();
    }


   
    
    [MenuItem(MenuFiltrateCoaxSleepAudio)]
    public static void FiltrateCoaxSleepAudio()
    {
        string needDeleteCoaxSleepAudioPath = AssetBundleHelper.NeedDeleteCoaxSleepAudioPath();
    
        //先把StreamingAssets下的资源删掉
        if (Directory.Exists(needDeleteCoaxSleepAudioPath))                
            UnityEditor.FileUtil.DeleteFileOrDirectory(needDeleteCoaxSleepAudioPath);
         
     
        
        string originalBundlePath = AssetBundleHelper.GetOriginalBundlesPath().Replace("\\", "/") + "/";
        
        string outPathRoot = AssetBundleHelper.GetOutCoaxSleepAudioFolderPath();
        bool isExists = Directory.Exists(outPathRoot);
        
        if (isExists)                  
             UnityEditor.FileUtil.DeleteFileOrDirectory(outPathRoot);        
                           
          
        Directory.CreateDirectory(outPathRoot);
        
        
        //把OriginalBundles的资源拷贝出来
        string findPath = AssetBundleHelper.CopyCoaxSleepAudioPath();
        
        string[] files = Directory.GetFiles(findPath, "*", SearchOption.AllDirectories);


        for (int i = 0; i < files.Length; i++)
        {
            string filePath = files[i];
            if (filePath.EndsWith(".manifest"))
                continue;
            string relatePath = filePath.Replace("\\", "/").Replace(originalBundlePath, "");

            string destFile = outPathRoot + "/" + relatePath;
            var fileInfo = new FileInfo(destFile);
            if (fileInfo.Directory.Exists == false)
                fileInfo.Directory.Create();
            File.Copy(filePath, destFile, true);
        }
   
        CreateCoaxSleepAudioJson(outPathRoot);
        
    }
    
    private static void CreateCoaxSleepAudioJson(string outPathRoot)
    {      
        _sleepAudioJson =new List<CoaxSleepAudioJsonData>();
        
        DirectoryInfo direction = new DirectoryInfo(outPathRoot);
        FileInfo[] files = direction.GetFiles("*", SearchOption.AllDirectories);

        foreach (var file in files)
        {
            string fullName =  file.FullName.Replace("\\", "/");
            fullName = Util.GetNoBreakingString(fullName);
            
            var data = new CoaxSleepAudioJsonData
            {
                AudioId =int.Parse(file.Name.Remove(file.Name.LastIndexOf(".", StringComparison.Ordinal))),   
                MD5 =MD5Util.Get(file.FullName),
                Path = fullName.Split(new string[] {"CoaxSleepAudioFolder/"},StringSplitOptions.RemoveEmptyEntries)[1],               
                Size = file.Length,
            };
            _sleepAudioJson.Add(data); 
        }
       
        string path = Application.dataPath.Replace("Assets", "I18NData/zh-cn/CoaxSleepCache");
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
            
        string jsonPath = path + "/coaxsleep.json";

        if (File.Exists(jsonPath))
        {            
            File.Delete(jsonPath);
        }
       
        
        string json = JsonConvert.SerializeObject(_sleepAudioJson,Formatting.Indented);              
        FileUtil.SaveFileText(path, "coaxsleep.json", json);
        CopyI18nData(false);
    }


    
    private static void LoadIndexJson(string jsonPath, ref Dictionary<string, bool> fileDitc, bool exclude = false)
    {
        string str = FileUtil.ReadFileText(PackageManager.OutputPath + jsonPath + ".json");
        if (string.IsNullOrEmpty(str))
            return;

        ResIndex resIndex = JsonConvert.DeserializeObject<ResIndex>(str);

        int count = 0;
        foreach (var resPack in resIndex.packageDict)
        {
            string releasePath = resPack.Value.releasePath;
            foreach (var item in resPack.Value.items)
            {
                string key = releasePath + "/" + item.Path.ToLower();
                if (exclude)
                {
                    count++;
                    fileDitc.Remove(key);
                }
                else if (fileDitc.ContainsKey(key) == false)
                {
                    fileDitc.Add(key, true);
                }
            }
        }

        if (count > 0)
        {
            Debug.Log("<color='#660000>排除音频文件数量：" + count + "</color>");
        }
    }

    static void CopyIndexFileToStreamingAsset()
    {
        string dir = PackageManager.OutputPath;
        string[] files = Directory.GetFiles(dir);

        if (files.Length == 0)
        {
            EditorUtility.DisplayDialog("警告", "索引文件不存在（" + dir + "）", "OK");
            return;
        }

        string destDir = AssetBundleHelper.GetOuterStreamingAssetsPath() + "/IndexFiles/";
        CreateDir(destDir);

        foreach (var file in files)
        {
            string fileName = Path.GetFileName(file);
            File.Copy(file, destDir + fileName, true);
        }
    }

    [MenuItem(MenuSwitchAssetLanguage)]
    static void SwitchAssetLanguage()
    {
        if (EditorUtility.DisplayDialog("切换资源", "是否切换资源到" + PackageManager.Language + "语言", "确定切换", "取消") == false)
            return;

        CopyI18nData(false);

        CopyI18nAssets(false);
    }

    [MenuItem(MenuSwitchAssetLanguageData)]
    static void CopyI18nData()
    {
        CopyI18nData(true);
    }

    static void CopyI18nData(bool showConfirm)
    {
        if (showConfirm &&
            EditorUtility.DisplayDialog("切换资源", "是否切换资源到" + PackageManager.Language + "语言", "确定切换", "取消") == false)
            return;

        I18NManager.LanguageType languageType =
            (I18NManager.LanguageType) EditorPrefs.GetInt(PublishRes.ResPublish_LanguageKey);

        if (languageType != I18NManager.LanguageType.ChineseSimplified)
        {
            CopyI18nDataZH_CN();
        }

        string path = PackageManager.I18NDataPath.Replace("\\", "/") + "/";
        string streamingAssetsPath = AssetBundleHelper.GetOuterStreamingAssetsPath() + "/";

        if(Directory.Exists(path) == false)
            return;
        
        string[] files = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories);

        foreach (var file in files)
        {
            string filePath = file.Replace("\\", "/");
            string destFile = filePath.Replace(path, "");
            destFile = streamingAssetsPath + destFile;
            CreateDir(destFile);
            File.Copy(file, destFile, true);
        }

        Debug.Log("<color='#00ff00'>切换文本到" + PackageManager.Language + "语言完成</color> 文件数量：" + files.Length);
    }

    private static void CreateDir(string destFile)
    {
        string dir = Path.GetDirectoryName(destFile);
        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }
    }

    /// <summary>
    /// 以简体中文配置为主，其他多语言文件夹放置有区别的文本就行了
    /// </summary>
    private static void CopyI18nDataZH_CN()
    {
        string path = Application.dataPath.Replace("Assets", "I18NData").Replace("\\", "/") + "/zh-cn";
        string streamingAssetsPath = AssetBundleHelper.GetOuterStreamingAssetsPath() + "/";

        string[] files = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories);

        foreach (var file in files)
        {
            string filePath = file.Replace("\\", "/");
            string destFile = filePath.Replace(path, "");
            destFile = streamingAssetsPath + destFile;
            CreateDir(destFile);
            File.Copy(file, destFile, true);
        }
    }

    [MenuItem(MenuSwitchAssetDelteAll)]
    static void SwitchAssetDelteAll()
    {
        Directory.Delete(AssetBundleHelper.GetOuterStreamingAssetsPath(), true);
        Debug.Log("<color='#00ff00'>删除所有OuterStreamingAssets资源完成</color>");
    }

    [MenuItem(MenuSwitchAssetDelteData)]
    static void SwitchAssetDelteData()
    {
        string[] files = Directory.GetFiles(AssetBundleHelper.GetOuterStreamingAssetsPath(), "*.*",
            SearchOption.AllDirectories);
        foreach (var file in files)
        {
            string filePath = file.Replace("\\", "/");
            if (filePath.Contains("StreamingAssets/AssetBundles"))
                continue;

            File.Delete(filePath);
        }

        files = Directory.GetDirectories(AssetBundleHelper.GetOuterStreamingAssetsPath(), "*.*",
            SearchOption.AllDirectories);
        foreach (var file in files)
        {
            string filePath = file.Replace("\\", "/");
            if (filePath.Contains("StreamingAssets/AssetBundles") || Directory.Exists(filePath) == false)
                continue;

            Directory.Delete(filePath, true);
        }

        Debug.Log("<color='#00ff00'>删除StreamingAssets Other资源完成</color>");
    }

    [MenuItem(MenuSwitchAssetDelteAssets)]
    public static void DeleteOuterStreamingAssets_AssetBundles()
    {
        if (Directory.Exists(AssetBundleHelper.GetOuterStreamingAssetsPath() + "/AssetBundles") == false)
            return;

        Directory.Delete(AssetBundleHelper.GetOuterStreamingAssetsPath() + "/AssetBundles", true);
        Debug.Log("<color='#00ff00'>删除StreamingAssets AssetBundles资源完成</color>");
    }

    [MenuItem(MenuSwitchAssetLanguageAssets)]
    static void CopyI18nAssets()
    {
        CopyI18nAssets(true);
    }

    static void CopyI18nAssets(bool showConfirm)
    {
        if (showConfirm &&
            EditorUtility.DisplayDialog("切换资源", "是否切换资源到" + PackageManager.Language + "语言", "确定切换", "取消") == false)
            return;

        string path = PackageManager.I18NAssetPath.Replace("\\", "/") + "/";
        string bundleAssetPath = Application.dataPath + "/BundleAssets/";
        
        if(Directory.Exists(path) == false)
            return;
        
        string[] files = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories);

        foreach (var file in files)
        {
            string filePath = file.Replace("\\", "/");
            string destFile = filePath.Replace(path, "");
            destFile = bundleAssetPath + destFile;
            File.Copy(file, destFile, true);
        }

        AssetDatabase.Refresh();

        Debug.Log("<color='#00ff00'>切换资源到" + PackageManager.Language + "语言完成</color> 文件数量：" + files.Length);
    }

    #region 语言设置菜单

    public const string MenuZhCn = "AssetBundle/语言设置/zh-CN";
    public const string MenuZhHk = "AssetBundle/语言设置/zh-HK";
    public const string MenuEnUs = "AssetBundle/语言设置/en-US";

    public const string ResPublish_LanguageKey = "ResPublish_Language";

    [MenuItem("AssetBundle/语言设置", true, 0)]
    static bool MenuFunc()
    {
        return false;
    }

    //中文简体
    [MenuItem(MenuZhCn, true)]
    static bool MenuFunc_zh_cn()
    {
        Menu.SetChecked(MenuZhCn, IsLanguage(I18NManager.LanguageType.ChineseSimplified));
        return true;
    }

    [MenuItem(MenuZhCn, false)]
    static void Set_zh_cn()
    {
        EditorPrefs.SetInt(ResPublish_LanguageKey, (int) I18NManager.LanguageType.ChineseSimplified);
    }

    //中文繁体
    [MenuItem(MenuZhHk, true)]
    static bool MenuFunc_zh_hk()
    {
        Menu.SetChecked(MenuZhHk, IsLanguage(I18NManager.LanguageType.ChineseTraditionalHk));
        return true;
    }

    [MenuItem(MenuZhHk, false)]
    static void Set_zh_hk()
    {
        EditorPrefs.SetInt(ResPublish_LanguageKey, (int) I18NManager.LanguageType.ChineseTraditionalHk);
    }

    //英文
    [MenuItem(MenuEnUs, true)]
    static bool MenuFunc_en_us()
    {
        Menu.SetChecked(MenuEnUs, IsLanguage(I18NManager.LanguageType.English));
        return true;
    }

    [MenuItem(MenuEnUs, false)]
    static void Set_en_us()
    {
        EditorPrefs.SetInt(ResPublish_LanguageKey, (int) I18NManager.LanguageType.English);
    }

    private static bool IsLanguage(I18NManager.LanguageType language)
    {
        I18NManager.LanguageType lang = (I18NManager.LanguageType) EditorPrefs.GetInt(ResPublish_LanguageKey);
        return lang == language;
    }

    #endregion
}