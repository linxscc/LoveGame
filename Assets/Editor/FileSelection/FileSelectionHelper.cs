/*
 * 文件筛选小助手(重要提示)
 * 1.生成缓存Json 在发布正式包时点一次就可以了。谨记就一次。一次。一次。
 * 2.如果没有缓存文件直接点筛选是直接给你return,没有对比缓存文件就没有修改~
 * 3.生成出来的修改文件会放在CacheFileRoot文件夹下，路径都给你生成好了。
 *    Bundle就去 CacheFileRoot/OriginalBundles下直接拷贝就好
 *    多语言相关或者json数据去CacheFileRoot/I18NData下面那
 */

using System;
using UnityEngine;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Framework.Utils;
using Newtonsoft.Json;
using UnityEditor;
using Debug = UnityEngine.Debug;
using FileUtil = UnityEditor.FileUtil;


public class FileSelectionHelper : Editor
{
    private static Dictionary<string, string> _dataDic; //Key是你文件路径，Value是MD5

    /// <summary>
    /// 生成Bundle的根目录
    /// </summary>
    private static string OriginalBundlesPath => Application.dataPath.Replace("Assets", "OriginalBundles");


    /// <summary>
    /// 多语言的根目录
    /// </summary>
    private static string I18NData => Application.dataPath.Replace("Assets", "I18NData");

    /// <summary>
    /// 缓存文件根目录
    /// </summary>
    private static string CacheFileRootPath => Application.dataPath.Replace("Assets", "CacheFileRoot");

    /// <summary>
    /// 缓存Json数据路径
    /// </summary>
    private static string CacheDataPath => Application.dataPath.Replace("Assets", "CacheFileRoot/Cache.json");

    /// <summary>
    /// 修改的文件信息List
    /// </summary>
    private static List<FileInfo> _modificationFiles;

    /// <summary>
    /// 需要删除的文件
    /// </summary>
    private static List<string> _needDeleteFiles;


    [MenuItem("文件筛选小助手/生成缓存Json")]
    public static void CreateCacheJson()
    {
        Stopwatch sw = Stopwatch.StartNew();
        CheckCacheFileRootFolderIsExist(CacheFileRootPath);

        if (File.Exists(CacheDataPath)) //生成会把上次的Json先删掉，然后下面重新生成。   
            File.Delete(CacheDataPath);

        var originalBundlesPath = OriginalBundlesPath;
        var i18NDataPath = I18NData + "/zh-cn";
        var cacheJsonPath = CacheDataPath;

        //读取数据阶段
        var files = GetAllFileInfos(originalBundlesPath, i18NDataPath);
        
        //写入数据据阶段
        CreateCacheJsonData(files, cacheJsonPath);
        AssetDatabase.Refresh();
        sw.Stop();
        long time = sw.ElapsedMilliseconds;
        Debug.Log("<color='#00ff66'> ====== 生成json总耗时：" + time + "ms</color>");
    }


    [MenuItem("文件筛选小助手/筛选文件")]
    public static void StartComparison()
    {
        CheckCacheFileRootFolderIsExist(CacheFileRootPath);
        
        if (!File.Exists(CacheDataPath))
        {
            Debug.LogError("没有缓存Json，无法筛选");
            return;
        }

        Stopwatch sw = Stopwatch.StartNew();

        var originalBundlesPath = OriginalBundlesPath;
        var i18NDataPath = I18NData + "/zh-cn";
        var cacheJsonPath = CacheDataPath;
        var cacheFileRootPath = CacheFileRootPath;

        _dataDic = GetCacheJsonData(cacheJsonPath);
        Debug.LogError("json长度--->"+_dataDic.Count);
       
       var files = GetAllFileInfos(originalBundlesPath, i18NDataPath);
        ComparisonFile(files, cacheFileRootPath);

        sw.Stop();
        long time = sw.ElapsedMilliseconds;
        Debug.Log("<color='#00ff66'> ====== 总耗时：" + time + "ms</color>");
        AssetDatabase.Refresh();
    }

    /// <summary>
    /// 生成缓存Json数据
    /// </summary>
    /// <param name="files">读取的文件信息</param>
    /// <param name="path">json路径</param>
    private static void CreateCacheJsonData(List<FileInfo> files, string path)
    {
        Stopwatch sw = Stopwatch.StartNew();
        _dataDic = new Dictionary<string, string>();

        var list = ArraySplit(files.ToArray(), 8);
 
        List<bool> isFinish = new List<bool>();
        foreach (var t in list){isFinish.Add(false);}
        
        for (int i = 0; i < list.Count; i++)
        {
            var index = i;
            Loom.RunAsync(() =>
            {
                var dicFrom =  AddDataToDic(list[index]);
                dicFrom.ToList().ForEach(x=>_dataDic.Add(x.Key,x.Value));
                isFinish[index] = true;
            });
        }
        
        while (true)
        {
            if (!isFinish.Contains(false))
                break;
        }
        
        WriteInJson(_dataDic, path);
        sw.Stop();
        long time = sw.ElapsedMilliseconds;

        Debug.Log("<color='#00ff66'> ====== 写入时间耗时：" + time + "ms</color>");
        Debug.Log("<color='#00ff66'>----缓存数据写入完毕,文件路径----</color>" + path);
    }

    /// <summary>
    /// 添加数据到Dic
    /// </summary>
    /// <param name="files"></param>
    private static Dictionary<string,string> AddDataToDic(FileInfo [] files)
    {
        Dictionary<string,string> dic =new Dictionary<string, string>();
        foreach (var file in files)
        {
            if (file.Extension == ".manifest" || file.Extension == ".meta")
                continue;
            string key = FileNameSplit(file.FullName);
            string value = MD5Util.Get(file.FullName);
            if (_dataDic.ContainsKey(key)&&_dataDic.ContainsKey(value))
                continue;
            dic.Add(key, value);
        }

        return dic;
    }

    /// <summary>
    /// 写入Json
    /// </summary>
    /// <param name="dic"></param>
    /// <param name="path"></param>
    private static void WriteInJson(Dictionary<string,string> dic,string path)
    {
        Debug.LogError("写入的长度---》"+dic.Count);
        string json = JsonConvert.SerializeObject(dic);
        FileStream fileStream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite);
        StreamWriter sw = new StreamWriter(fileStream, System.Text.Encoding.UTF8);
        sw.WriteLine(json);
        sw.Close();
        sw.Dispose();
    }
    
    /// <summary>
    /// 对比文件
    /// </summary>
    private static void ComparisonFile(List<FileInfo> files, string cacheFileRootPath)
    {
        Stopwatch sw = Stopwatch.StartNew();
        _modificationFiles = new List<FileInfo>();
        
        var list = ArraySplit(files.ToArray(), 8);
        List<bool> isFinish = new List<bool>();
        foreach (var t in list){ isFinish.Add(false);}
   
        for (int i = 0; i < list.Count; i++)
        {
            var index = i;
            Loom.RunAsync(() =>
            {
                var listFrom =  AddDataToModificationFiles(list[index]);
                _modificationFiles.AddRange(listFrom);
                isFinish[index] = true;
            });
        }
        
        while (true)
        {
            if (!isFinish.Contains(false))
                break;
        }
        
        sw.Stop();
        long time = sw.ElapsedMilliseconds;
        Debug.Log("<color='#00ff66'> ====== 对比耗时：" + time + "ms</color>");
        CopyToModificationFile(cacheFileRootPath);
       
    }

    private static List<FileInfo[]> ArraySplit(FileInfo[] files, int num)
    {
        List<FileInfo[]> list = new List<FileInfo[]>();
        var stepSize = files.Length/num;
        
        for (int i = 0; i < num; i++)
        {
            var isLastIndex = num - 1 == i;

            if (!isLastIndex)
            {
                FileInfo[] fileArray = new FileInfo[stepSize];
                Array.Copy(files, i*stepSize, fileArray, 0, stepSize);
                list.Add(fileArray);
            }
            else
            {
                FileInfo[] fileArray = new FileInfo[files.Length - (i*stepSize)];
                Array.Copy(files, i*stepSize, fileArray, 0, files.Length -  (i*stepSize));
                list.Add(fileArray);
            }
        }
        return list;
    }
    
    
    /// <summary>
    /// 添加修改的文件
    /// </summary>
    /// <param name="files"></param>
    private static List<FileInfo> AddDataToModificationFiles(FileInfo[] files)
    {
        List<FileInfo> list =new List<FileInfo>();
        foreach (var file in files)
        {
            if (file.Extension == ".manifest" || file.Extension == ".meta")
                continue;

            string key = FileNameSplit(file.FullName);
            var md5 = MD5Util.Get(file.FullName);
            var isKey = _dataDic.ContainsKey(key);
            var isValue = _dataDic.ContainsValue(md5);

            if (isKey)
            {
                if (!isValue) //修改文件内容
                    list.Add(file);
            }
            else
            {
              //  if (!list.Contains(file))
                    list.Add(file);
            }
        }

        return list;

    }

    /// <summary>
    /// 导出需要修改的文件，文件存放路径在CacheFileRootPath下
    /// </summary>
    private static void CopyToModificationFile(string root)
    {
        Stopwatch sw = Stopwatch.StartNew();

        var modificationBundlesPath = root + "/OriginalBundles";
        var modificationI18NDataPath = root + "/I18NData";

        if (Directory.Exists(modificationBundlesPath))
        {FileUtil.DeleteFileOrDirectory(modificationBundlesPath);
           // Directory.Delete(modificationBundlesPath);
        }
        
        if (Directory.Exists(modificationI18NDataPath))
            FileUtil.DeleteFileOrDirectory(modificationI18NDataPath);

        Debug.Log("<color='#00ff66'>----修改的文件----</color>" + _modificationFiles.Count);
        if (_modificationFiles.Count>0)
        {
            foreach (var file in _modificationFiles)
            {
                string sourceFile = file.FullName;
                string destFile = root + "/" + FileNameSplit(file.FullName);
                FileInfo fileInfo = new FileInfo(destFile);
                if (fileInfo.Directory.Exists == false)
                    fileInfo.Directory.Create();

                File.Copy(sourceFile, destFile, true);
            }  
        }
       

        sw.Stop();
        long time = sw.ElapsedMilliseconds;
        Debug.Log("<color='#00ff66'> ====== Copy耗时：" + time + "ms</color>");
    }

    #region 记录修改文件名操作

    /// <summary>
    /// 生成需要删除的文件信息Txt
    /// </summary>
    private static void CreateNeedDeleteInfoTxt()
    {
        string txtName = "NeedDeleteInfo.txt";
        string path = CacheFileRootPath + "/" + txtName;
        bool isExists = File.Exists(path);

        if (isExists)
            File.Delete(path);

        if (_needDeleteFiles.Count == 0)
        {
            Debug.Log("<color='#00ff66'>----没有要删除的文件----</color>");
            return;
        }

        Debug.Log("<color='#00ff66'>----删除的文件数量----</color>" + _needDeleteFiles.Count);
        FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.ReadWrite);
        StreamWriter sw = new StreamWriter(fileStream, System.Text.Encoding.UTF8);
        foreach (var fileName in _needDeleteFiles)
        {
            sw.WriteLine(fileName);
        }

        sw.Close();
        sw.Dispose();
    }


    /// <summary>
    /// 设置删除的文件
    /// </summary>
    /// <param name="files">读的文件信息，不是缓存数据</param>
    private static void SetDeleteFile(FileInfo[] files)
    {
        List<string> originalFileNames = new List<string>();
        List<string> modificationFileNames = new List<string>();
        foreach (var file in files)
        {
            originalFileNames.Add(FileNameSplit(file.FullName));
        }

        foreach (var file in _modificationFiles)
        {
            modificationFileNames.Add(FileNameSplit(file.FullName));
        }

        foreach (var t in _dataDic)
        {
            if (!originalFileNames.Contains(t.Key) && !modificationFileNames.Contains(t.Key) &&
                !_needDeleteFiles.Contains(t.Key))
            {
                _needDeleteFiles.Add(t.Key);
            }
        }
    }

    /// <summary>
    /// 获取修改之前的文件名
    /// </summary>
    /// <param name="md5"></param>
    /// <returns></returns>
    private static string GetModificationBeforeOriginalFileName(string md5)
    {
        string oldFileName = string.Empty;
        foreach (var info in _dataDic)
        {
            if (info.Value == md5)
            {
                oldFileName = info.Key;
                break;
            }
        }

        return oldFileName;
    }

    #endregion

    /// <summary>
    /// 检查CacheFileRoot文件夹是否存在，不存在则创建
    /// </summary>
    /// <param name="path">路径</param>
    private static bool CheckCacheFileRootFolderIsExist(string path)
    {
        if (Directory.Exists(path))
        {
            Debug.Log("<color='#00ff66'>----缓存根文件夹存在----</color>" + path);
            return true;
        }
        else
        {
            Directory.CreateDirectory(path);
            Debug.Log("<color='#00ff66'>----缓存根文件夹不存在，已经创建----</color>" + path);
            return false;
        }
    }


    /// <summary>
    /// 获取所有文件信息
    /// </summary>
    /// <param name="originalBundlesPath"></param>
    /// <param name="i18NDataPath"></param>
    /// <returns></returns>
    private static List<FileInfo> GetAllFileInfos(string originalBundlesPath, string i18NDataPath)
    {
        Stopwatch sw = Stopwatch.StartNew();
        
        List<FileInfo> list = new List<FileInfo>();
        bool isFinish1 = false;
        bool isFinish2 = false;
        Loom.RunAsync((() =>
        {
            list.AddRange(GetAssignPathFileInfos(originalBundlesPath));
            isFinish1 = true;
        }));
        Loom.RunAsync((() =>
        {
            list.AddRange(GetAssignPathFileInfos(i18NDataPath));
            isFinish2 = true;
        }));

        while (true)
        {
            if (isFinish1 && isFinish2)
                break;
        }
        
      
        sw.Stop();
        long time = sw.ElapsedMilliseconds;
        Debug.Log("<color='#00ff66'> ====== 读文件耗时：" + time + "ms</color>");
        Debug.LogError("读文件的数量--->" + list.Count);
        return list;
    }

    /// <summary>
    /// 获取指定路径下的文件信息
    /// </summary>
    /// <param name="path">路径</param>
    /// <returns>返回该文件下的信息</returns>
    private static FileInfo[] GetAssignPathFileInfos(string path)
    {
        DirectoryInfo direction = new DirectoryInfo(path);
        FileInfo[] files = direction.GetFiles("*", SearchOption.AllDirectories);
        return files;
    }

    /// <summary>
    /// 获取缓存Json数据
    /// </summary>
    /// <param name="path">json路径</param>
    /// <returns></returns>
    private static Dictionary<string, string> GetCacheJsonData(string path)
    {
        Stopwatch sw = Stopwatch.StartNew();

        string json = File.ReadAllText(path);

        sw.Stop();
        long time = sw.ElapsedMilliseconds;
        Debug.Log("<color='#00ff66'> ====== 读Json耗时：" + time + "ms</color>");
        return JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
    }


    /// <summary>
    /// 分割 SuperStarGame
    /// SuperStarGame 这里要改成项目名称
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    private static string FileNameSplit(string path)
    {
        string str = path.Replace("\\", "/");
        ;
        str = str.Split(new string[] {"SuperStarGame/"}, StringSplitOptions.RemoveEmptyEntries)[1];
        return str;
    }
}