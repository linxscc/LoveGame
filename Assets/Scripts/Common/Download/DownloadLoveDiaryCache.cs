using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using DG.Tweening;
using Framework.Utils;
using game.main;
using UnityEngine;
using Utils;
using Debug = UnityEngine.Debug;

namespace Assets.Scripts.Module.Download
{
    public partial class CacheManager
    {
        private static List<int> _LoveDiaryCacheList;
        private static Action _onLoveDiaryCancel;
        public static void DownloadLoveDiaryCache(string musicId, Action<string> onComplete = null, Action onCancel = null, string tag = null)
        {
            _onComplete = onComplete;
            _onLoveDiaryCancel = onCancel;
            _tag = tag;

            ResIndex resIndex = GetIndexFile(ResPath.Special);

            string packKey = "lovediary";

            if (!resIndex.packageDict.ContainsKey(packKey))
            {
                FlowText.ShowMessage(I18NManager.Get("Download_ErrorLoveDiary") + musicId);
                _onLoveDiaryCancel?.Invoke();
                return;
            }

            DownloadManager.Clear();
            ResPack resPack = resIndex.packageDict[packKey.ToString()];
            string resPath = "music/mainplay/" + musicId + ".music";
            var rm = resPack.items.Find((m) => { return m.Path == resPath; });
            if (rm == null)
            {
                return;
            }

            _releasePath = resPack.releasePath;
            if (resPack.packageType == FileType.Original)
            {
                ResItem item = new ResItem()
                {
                    Md5 = rm.Md5,
                    Path = AppConfig.Instance.assetsServer + "/" + AppConfig.Instance.version + "/" +
                           resPack.downloadPath+"/"+ resPath,
                    Size = rm.Size,
                    FileType = resPack.packageType
                };

                //_downloadingWindow = PopupManager.ShowWindow<DownloadingWindow>(Constants.DownloadingWindowPath);
                //_downloadingWindow.Content = I18NManager.Get("Download_Downloading");
                //_downloadingWindow.Progress = I18NManager.Get("Download_Progress", 0);

                string storePath = AssetLoader.ExternalHotfixPath + "/" + _releasePath+"/"+ resPath;
                Debug.Log(storePath);
                var downItem = DownloadManager.Load(item, storePath, OnCompleteLoveDiary,
                    OnErrorLoveDiary, null, OnErrorLoveDiary);
                //_downloadingWindow.WindowActionCallback = evt =>
                //{
                //    if (evt == WindowEvent.Cancel)
                //    {
                //        onCancel?.Invoke();
                //        downItem.Cancel();
                //    }
                //};
            }
        }

        private static void OnErrorLoveDiary(DownloadItem item)
        {
            Debug.LogError("下载压缩包失败：" + item.ErrorText+ " Url:"+item.Url);
            _onLoveDiaryCancel?.Invoke();

            FlowText.ShowMessage(I18NManager.Get("Download_ErrorAndRetry"));
        }

        private static void OnCompleteLoveDiary(DownloadItem downloadItem)
        {
            if (downloadItem.Md5 != MD5Util.Get(downloadItem.LocalPath))
            {
                FlowText.ShowMessage(I18NManager.Get("Download_ErrorAndRetry"));
                File.Delete(downloadItem.LocalPath);
                return;
            }


            bool isEnd = false;
            progress = 0;
            Tweener tween = DOTween.To(() => progress, val => progress = val, 100, 1.5f);



            tween.onComplete = () =>
            {
                if (isEnd == false)
                {
                    isEnd = true;
                }
                else
                {
                    _onComplete?.Invoke(_tag);
                }
            };

            Loom.RunAsync(() =>
            {
                if (downloadItem.FileType == FileType.Zip)
                {
                    ZipUtil.Unzip(downloadItem.LocalPath, AssetLoader.ExternalHotfixPath + "/" + _releasePath);
                }

                Loom.QueueOnMainThread(() =>
                {
                    if (isEnd == false)
                    {
                        isEnd = true;
                    }
                    else
                    {

                        _onComplete?.Invoke(_tag);
                    }
                });
            });
        }



        public static bool IsLoveDiaryNeedDown(string musicId)
        {
            Stopwatch sw = Stopwatch.StartNew();
            ResIndex resIndex = GetIndexFile(ResPath.Special);
            if (resIndex == null)
            {
                return false;
            }
            Debug.LogError(musicId);
            string packKey = "lovediary";
            if (resIndex.packageDict.ContainsKey(packKey) == false)
            {
                FlowText.ShowMessage(I18NManager.Get("Download_IndexNoExist") + packKey);
                return false;
            }

            ResPack resPack = resIndex.packageDict[packKey.ToString()];

            _releasePath = resPack.releasePath;


            string resPath = "music/mainplay/" + musicId + ".music";
            var rm = resPack.items.Find((m) => { return m.Path == resPath; });
            if (rm == null)
            {
                return false;
            }

    
            string storePath = AssetLoader.ExternalHotfixPath + "/" + _releasePath +"/" + rm.Path;
            Debug.Log("IsLoveDiaryNeedDown::"+ storePath);
            FileInfo fileInfo =
             new FileInfo(storePath);

            if (fileInfo.Exists == false)
            {
                return true;
            }
            return false;
        }

    }
}