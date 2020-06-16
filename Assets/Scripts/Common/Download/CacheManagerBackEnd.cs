using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using DG.Tweening;
using Framework.Utils;
using game.main;
using Utils;
using Debug = UnityEngine.Debug;

namespace Assets.Scripts.Module.Download
{
    public partial class CacheManager
    {
        private static Queue<ResPack> _backendPackQueue;
        private static int _unzipIndex;

        public static void DownLoadBackEndCacheQueue(Dictionary<string, CacheVo> cacheVoDic,
            Action<string> onComplete = null, string tag = null, Action<string> onError = null)
        {
            _tag = tag;
            _onError = onError;

            _onComplete = onComplete;

            ResIndex resIndex = GetIndexFile(ResPath.Backend);
            _backendPackQueue = new Queue<ResPack>();

            foreach (var v in resIndex.packageDict)
            {
                if (cacheVoDic.ContainsKey(v.Key))
                {
                    _backendPackQueue.Enqueue(v.Value);
                }
            }

            if (_backendPackQueue.Count > 0)
            {
                DownloadBackEndCache();
            }
        }

        private static void DownloadBackEndCache()
        {
            ResPack resPack = _backendPackQueue.Dequeue();
            _releasePath = resPack.releasePath;
            ResItem item = new ResItem()
            {
                Md5 = resPack.packageMd5,
                Path = AppConfig.Instance.assetsServer + "/" + AppConfig.Instance.version + "/" +
                       resPack.downloadPath,
                Size = resPack.packageSize,
                FileType = resPack.packageType
            };

            Debug.LogError("StarDownloadBackEnd " + resPack.downloadPath);
            DownloadManager.Load(item, AssetLoader.ExternalDownloadPath + "/" + resPack.downloadPath,
                OnBackendStepEnd, OnError, OnProgressFullLoading);
            LoadingProgress.Instance.TotalSize = Math.Round(resPack.packageSize * 1f / 1048576f, 2);
        }

        public static void OnBackendStepEnd(DownloadItem downloadItem)
        {
            Debug.LogError("OnBackendStepEnd=>" + _backendPackQueue.Count);
            if (_backendPackQueue.Count > 0)
            {
                DownloadBackEndCache();
            }
            else
            {
                _onComplete?.Invoke(_tag);
            }

            LoadingProgress.Instance.HasDownloadCount++;
        }

        /// <summary>
        /// 获取到需要后台下载的包cachevo
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, CacheVo> CheckBackEndCacheDic(bool handlePackCount)
        {
            Stopwatch sw = Stopwatch.StartNew();
            Dictionary<string, CacheVo> cacheVodic = new Dictionary<string, CacheVo>();
            ResIndex resIndex = GetIndexFile(ResPath.Backend);
            if (resIndex == null)
                return cacheVodic;

            if (resIndex.packageDict.Count == 0)
            {
                Debug.LogError("Download_IndexNoExist");
                return cacheVodic;
            }

            if (IsDownloadMarked())
                return cacheVodic;

            foreach (var v in resIndex.packageDict)
            {
                FileMark fm = new FileMark(AssetLoader.ExternalHotfixPath, ResPath.Backend + "_" + v.Key);
                if (fm.IsMatch == false)
                {
                    CacheVo vo = new CacheVo();
                    vo.needDownload = true;
                    cacheVodic.Add(v.Key, vo);
                }
            }

            if(handlePackCount)
                LoadingProgress.Instance.AddPackCount(cacheVodic.Count);
            
            return cacheVodic;
        }

        /// <summary>
        /// 获取到需要后台下载的包cachevo
        /// </summary>
        /// <returns></returns>
        public static long CheckCurBackEndCacheSize()
        {
            Stopwatch sw = Stopwatch.StartNew();

            ResIndex resIndex = GetIndexFile(ResPath.Backend);
            if (resIndex == null)
                return 0;

            if (resIndex.packageDict.Count == 0)
            {
                Debug.LogError("Download_IndexNoExist");
                return 0;
            }

            long allsize = 0;

            foreach (var v in resIndex.packageDict)
            {
                //要遍历几遍！
                CacheVo vo = new CacheVo();
                ResPack resPack = v.Value;

                _releasePath = resPack.releasePath;
                vo.sizeList = new List<long>() {resPack.packageSize};

                FileMark fm = new FileMark(AssetLoader.ExternalHotfixPath, ResPath.Backend + "_" + v.Key);
                if (fm.IsMatch == false)
                {
                    allsize += resPack.packageSize;
                }
            }
            Debug.Log("CheckBackEnd===Size 时间：" + sw.ElapsedMilliseconds);

            return allsize;
        }

        public static void UnzipBackend(Action<string> callback)
        {
            _tag = ResPath.Backend;

            _onComplete = callback;

            ResIndex resIndex = GetIndexFile(ResPath.Backend);

            _unzipIndex = 0;
            string[] keys = resIndex.packageDict.Keys.ToArray();
            string key = keys[_unzipIndex];

            UnzipBackendStep(resIndex.packageDict, key, OnBackendUnzipComplete);

            timerHandler = ClientTimer.Instance.AddCountDown("Upzip", Int64.MaxValue, 0.1f,
                val =>
                {
                    int progress = (int) ((float) ZipUtil.CurrentSize / ZipUtil.TotalSize * 100);
                    LoadingProgress.Instance.SetPercent(progress, true);
                }, null);
        }

        private static void OnBackendUnzipComplete(Dictionary<string, ResPack> dict)
        {
            _unzipIndex++;
            string[] keys = dict.Keys.ToArray();
            if (_unzipIndex >= keys.Length)
            {
                if (timerHandler != null)
                {
                    ClientTimer.Instance.RemoveCountDown(timerHandler);
                    timerHandler = null;
                }

                _onComplete?.Invoke(_tag);
            }
            else
            {
                string key = keys[_unzipIndex];
                UnzipBackendStep(dict, key, OnBackendUnzipComplete);
            }
        }

        private static void UnzipBackendStep(Dictionary<string, ResPack> dict, string fileKey,
            Action<Dictionary<string, ResPack>> onUnzipComplete)
        {
            ResPack resPack = dict[fileKey];
            string filePath = AssetLoader.ExternalDownloadPath + "/" + resPack.downloadPath;

            Loom.RunAsync(() =>
            {
                try
                {
                    ZipUtil.UnzipThread(filePath, AssetLoader.ExternalHotfixPath + "/" + resPack.releasePath);
                }
                catch (IOException e)
                {
                    Debug.LogException(e);
                }

                Loom.QueueOnMainThread(() =>
                {
                    File.Delete(filePath);
                    FileMark fm = new FileMark(AssetLoader.ExternalHotfixPath, ResPath.Backend + "_" + fileKey);
                    fm.UpdateRecord(AppConfig.Instance.version + "");

                    onUnzipComplete?.Invoke(dict);
                });
            });
        }
    }
}