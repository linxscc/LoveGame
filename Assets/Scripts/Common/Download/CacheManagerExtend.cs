using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using DG.Tweening;
using Framework.Utils;
using game.main;
using Utils;
using Debug = UnityEngine.Debug;

namespace Assets.Scripts.Module.Download
{
    public partial class CacheManager
    {
        public static void DownloadExtendCache(Action<string> onComplete = null, string tag = null,Action<string> onError=null)
        {
            _onComplete = onComplete;
            _onError = onError;
            _tag = tag;

            ResIndex resIndex = GetIndexFile(ResPath.Extend);
            ResPack resPack = resIndex.packageDict[ResPath.Extend];
            _releasePath = resPack.releasePath;
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
                    OnCompleteFullLoading, OnError, OnProgressFullLoading);
                LoadingProgress.Instance.Show(resPack.packageSize);
                LoadingProgress.Instance.SetPercent(0);
            }
        }

        public static CacheVo CheckExtendCache()
        {
            Stopwatch sw = Stopwatch.StartNew();

            CacheVo vo = new CacheVo();

            ResIndex resIndex = GetIndexFile(ResPath.Extend);
            if (resIndex == null)
                return vo;

            if (IsDownloadMarked())
                return vo;
            
            if (resIndex.packageDict.ContainsKey(ResPath.Extend) == false)
            {
                FlowText.ShowMessage(I18NManager.Get("Download_IndexNoExist"));
                return vo;
            }

            ResPack resPack = resIndex.packageDict[ResPath.Extend];

            _releasePath = resPack.releasePath;
            
            FileMark fm = new FileMark(AssetLoader.ExternalHotfixPath, ResPath.Backend);
            if (fm.IsMatch == false)
            {
                vo.needDownload = true;
            }
            
            vo.sizeList = new List<long>() {resPack.packageSize};

            Debug.LogError("CheckExtend 时间：" + sw.ElapsedMilliseconds);

            return vo;
        }

        public static long GetExtendSize(CacheVo vo)
        {
            long allsize=0;
            foreach (var v in vo.sizeList)
            {
                allsize += v;
            }

            if (allsize>0)
            {
                LoadingProgress.Instance.AddPackCount(1);
                
            }
            
            return allsize;


        }
    }
}