using System;
using System.Collections.Generic;
using System.IO;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Services;
using Framework.Utils;
using game.main;
using UnityEngine;

namespace Assets.Scripts.Module.Download
{
    public class FileChecker
    {
        
        public const string DeleteHotfixMark = "DeleteHotfixMark.txt";
        
        private List<FileInfo> moveList;
        private List<IDownloadItem> downloadList;

        public static string HotfixRecordPath => AssetLoader.ExternalHotfixPath + "/HotfixRecord.txt";
        
        public List<IDownloadItem> GetReloadHotfixFiles(HotfixIndexFile indexFile)
        {
            float time = Time.realtimeSinceStartup;
            List<IDownloadItem> list = new List<IDownloadItem>();
            foreach (var item in indexFile.FileItems)
            {
                try
                {
                    FileInfo fileInfo = new FileInfo(AssetLoader.ExternalHotfixPath + "/" + item.Path);
                    if(fileInfo.Exists == false || item.Size != fileInfo.Length || MD5Util.Get(fileInfo.FullName) != item.Md5)
                    {
                        list.Add(item);
                    }
                }
                catch (Exception e)
                {
                    list.Add(item);
                }
            }

            Debug.LogWarning("GetReloadHotfixFiles time:" + (Time.realtimeSinceStartup - time));
            
            return list;
        }

        /// <summary>
        /// 清理客户端缓存
        /// </summary>
        /// <param name="onDelete"></param>
        public void CleanForRepair(Action<bool> onDelete)
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
            string path = Application.persistentDataPath + "/DataCache";
            DirectoryInfo dir = new DirectoryInfo(path);
            if (dir.Exists)
            {
                try
                {
                    dir.Delete(true);
                }
                catch (Exception e)
                {
                    BuglyAgent.ReportException("Repair Delete DataCache", e.Message, e.StackTrace);
                }
            }
            DeleteHotfixDir(onDelete);
        }

        /// <summary>
        /// 删除热更文件
        /// </summary>
        /// <param name="onDelete"></param>
        public void DeleteHotfixDir(Action<bool> onDelete)
        {
            FileMark fileMark = new FileMark(Application.persistentDataPath, FileChecker.DeleteHotfixMark);
            fileMark.Delete();
            
            DirectoryInfo dir = new DirectoryInfo(AssetLoader.ExternalHotfixPath);
            if (dir.Exists)
            {
                try
                {
                    dir.Delete(true);
                    onDelete?.Invoke(true);
                }
                catch (Exception e)
                {
                    onDelete?.Invoke(false);
                    
                    fileMark.UpdateRecord("true");
                    
                    BuglyAgent.ReportException("DeleteHotfixDir", e.Message, e.StackTrace);
                    PopupManager.ShowAlertWindow(I18NManager.Get("Update_RepairFail"), null,
                        I18NManager.Get("Update_RepairCloseGame")).WindowActionCallback = evt =>
                    {
                        Application.Quit();
                    };
                }
            }
            else
            {
                onDelete?.Invoke(true);
            }
        }
        
        /// <summary>
        /// 按照HotfixConfig的配置去删除热更文件
        /// </summary>
        public void DeleteHotfixFile()
        {
            string str = FileUtil.ReadFileText(HotfixRecordPath);
            if (string.IsNullOrEmpty(str))
                return;

            string[] arr = str.Split('_');
            string hotfixConfigPath = AssetLoader.ExternalHotfixPath + "/HotfixConfig_v" + arr[0] + "_h" + arr[1] + ".zip";

            FileInfo fileInfo = new FileInfo(hotfixConfigPath);
            if(fileInfo.Exists == false)
                return;
            
            DirectoryInfo directoryInfo = new DirectoryInfo(AssetLoader.ExternalHotfixPath);
            if(directoryInfo.Exists == false)
                return;
            
            byte[] bytes = FileUtil.ReadBytesFile(hotfixConfigPath);
            if(bytes == null)
                return;
            
            HotfixIndexFile indexFile = HotfixIndexFile.Deserialize(bytes);
            foreach (var item in indexFile.FileItems)
            {
                File.Delete(AssetLoader.ExternalHotfixPath + "/" + item.Path);
            }
            
            fileInfo.Delete();
            File.Delete(HotfixRecordPath);
        }

//        public void DoGlobalBundleCheck(Action callback)
//        {
//            BundleConfigService service = new BundleConfigService();
//            service.Execute();
//
//            Dictionary<string, BundleStruct> config = service.GetData() as Dictionary<string, BundleStruct>;
//
//            DirectoryInfo dir = new DirectoryInfo(AssetLoader.ExternalOldHotfixPath);
//            if (dir.Exists == false)
//            {
//                callback();
//                return;
//            }
//
//            FileInfo[] files = dir.GetFiles("*", SearchOption.AllDirectories);
//            moveList = new List<FileInfo>();
//            downloadList = new List<IDownloadItem>();
//            
//            foreach (var fileInfo in files)
//            {
//                string key = fileInfo.FullName.Replace(AssetLoader.ExternalOldHotfixPath + "/", "");
//                if (config.ContainsKey(key))
//                {
//                    BundleStruct item = config[key];
//                    if (item.Size == fileInfo.Length && MD5Util.Get(fileInfo.FullName) == item.Md5)
//                    {
//                        moveList.Add(fileInfo);
//                    }
//                    else
//                    {
//                        ResItem resItem = new ResItem();
//                        resItem.Md5 = item.Md5;
//                        resItem.Path = key;
//                        resItem.FileType = FileType.Original;
//                        downloadList.Add(resItem);
//                    }
//                }
//                //BundleConfig里面没有的表示是已经废弃的资源
//            }
//            
//            MoveFiles();
//
//            DownloadFiles();
//        }
//
//        private void DownloadFiles()
//        {
//            LoadingProgress.Instance.Show();
//            LoadingProgress.Instance.SetPercent(0);
//            
//            DownloadManager.AddList(downloadList);
//            DownloadManager.SetQueueCallback(OnLoadProgress, OnLoadError);
//            
//            string sysTag = Application.platform == RuntimePlatform.Android ? "Android" : "iOS";
//
//            DownloadManager.StartQueue(AppConfig.Instance.assetsServer + "/" + AppConfig.Instance.version + "/" +
//                                       I18NManager.Language + "/" + sysTag);
//        }
//
//        private void OnLoadError(DownloadItem item)
//        {
//            
//        }
//
//        private void OnLoadProgress(float progress)
//        {
//            
//        }
//
//        private void MoveFiles()
//        {
//            foreach (var fileInfo in moveList)
//            {
//                string dest = fileInfo.FullName.Replace("OldHotfix", "Hotfix");
//                string dir = Path.GetDirectoryName(dest);
//                if (string.IsNullOrEmpty(dir))
//                {
//                    Debug.LogError("获取文件夹失败：" + dest);
//                    continue;
//                }
//                    
//                if(Directory.Exists(dir) == false)
//                    Directory.CreateDirectory(dir);
//                fileInfo.MoveTo(dest);
//            }
//        }
    }
}