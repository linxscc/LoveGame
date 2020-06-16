using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using game.main;
using Debug = UnityEngine.Debug;

namespace Assets.Scripts.Module.Download
{
    public partial class CacheManager
    {
        public static CacheVo CheckAppStartCache()
        {
            Stopwatch sw = Stopwatch.StartNew();

            CacheVo vo = new CacheVo();
            
            FileMark fm = new FileMark(AssetLoader.ExternalHotfixPath, ResPath.AppStart);
            if (fm.IsMatch)
                return vo;

            ResIndex resIndex = GetIndexFile(ResPath.AppStart);
            if (resIndex == null)
                return vo;

            ResPack resPack = resIndex.packageDict[ResPath.AppStart];

            _releasePath = resPack.releasePath;
            foreach (var resItem in resPack.items)
            {
                FileInfo fileInfo =
                    new FileInfo(AssetLoader.ExternalHotfixPath + "/" + _releasePath + "/" + resItem.Path);

                if (fileInfo.Exists == false || fileInfo.Length != resItem.Size)
                {
                    vo.needDownload = true;
                    break;
                }
            }

            vo.sizeList = new List<long>() {resPack.packageSize};

            Debug.LogError("CheckAppStartCache 时间：" + sw.ElapsedMilliseconds);

            return vo;
        }

        public static void DownloadAppStartCache(Action<string> onComplete, string tag = null)
        {
            _onComplete = onComplete;
            _tag = tag;
            
            ResIndex resIndex = GetIndexFile(ResPath.AppStart);
            ResPack resPack = resIndex.packageDict[ResPath.AppStart];
            _releasePath = resPack.releasePath;
            if (resPack.packageType == FileType.Zip)
            {
                ResItem item = new ResItem()
                {
                    Md5 = resPack.packageMd5,
                    Path = AppConfig.Instance.assetsServer + "/" + AppConfig.Instance.version + "/" + resPack.downloadPath,
                    Size = resPack.packageSize,
                    FileType = resPack.packageType
                };
               
                DownloadManager.Load(item, AssetLoader.ExternalDownloadPath + "/" + resPack.downloadPath,
                    OnCompleteFullLoading, OnFullLoadingAppStartError, OnProgressFullLoading);
                
                LoadingProgress.Instance.Show(resPack.packageSize);
                LoadingProgress.Instance.SetPercent(0);
            }
        }
        
        private static void OnFullLoadingAppStartError(DownloadItem item)
        {
            LoadingProgress.Instance.ShowAlertWindw(true, "下载出现问题，请重试下载！", null, null,
                () =>
                {
                    DownloadAppStartCache(_onComplete, _tag); 
                });
        }
    }
}