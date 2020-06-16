using System;
using game.main;
using UnityEngine;

namespace Assets.Scripts.Module.Download
{
    public partial class CacheManager
    {
        public static void DownloadAllBundle(Action<string> onComplete, string tag = null)
        {
            _onComplete = onComplete;
            _tag = tag;

            _releasePath = "";

            ResIndex resIndex = GetIndexFile(ResPath.AllResources);
            if (resIndex==null)
            {
                onComplete?.Invoke(tag);
                return;
            }
            
            ResPack resPack = resIndex.packageDict[ResPath.AllResources];
            if (resPack.packageType == FileType.Zip)
            {
                ResItem item = new ResItem()
                {
                    Md5 = resPack.packageMd5,
                    Path = AppConfig.Instance.assetsServer + "/" + AppConfig.Instance.version + "/" +
                           resPack.downloadPath,
                    Size = resPack.packageSize,
                    FileType = resPack.packageType
                };

                DownloadManager.Load(item, AssetLoader.ExternalDownloadPath + "/" + resPack.downloadPath,
                    OnCompleteFullLoading, OnFullLoadingError, OnProgressFullLoading);

                LoadingProgress.Instance.Show(resPack.packageSize);
                LoadingProgress.Instance.SetPercent(0);
            }
        }

        private static void OnFullLoadingError(DownloadItem item)
        {
            Debug.LogError("下载失败：" +item.Url+" ErrorText:"+ item.ErrorText);
            LoadingProgress.Instance.ShowAlertWindw(true, "下载出现问题，请重试下载！", null, null,
                () =>
                {
                    DownloadAllBundle(_onComplete, _tag); 
                });
        }
        
        private static void MarkDownloadAll()
        {
            FileMark fm = new FileMark(AssetLoader.ExternalHotfixPath, ResPath.AllResources);
            fm.UpdateRecord(AppConfig.Instance.version+"");
        }
        
        private static bool IsDownloadMarked()
        {
            FileMark fm = new FileMark(AssetLoader.ExternalHotfixPath, ResPath.AllResources);
            return fm.IsMatch;
        }
    }
}