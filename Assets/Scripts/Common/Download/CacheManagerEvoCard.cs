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
        public static void DownloadEvoCard(int cardId, Action<string> onComplete = null, string tag = null)
        {
            _onComplete = onComplete;
            _tag = tag;

            ResIndex resIndex = GetIndexFile(ResPath.EvoCardIndex);
            ResPack resPack = resIndex.packageDict["EvoCard"];

            string id = cardId + ".bytes";
            ResItem resItem = null;
            foreach (var item in resPack.items)
            {
                if (item.Path == id)
                {
                    resItem = item;
                    break;
                }
            }
            
            if(resItem == null)
            {
                FlowText.ShowMessage(I18NManager.Get("Download_IndexNoExist") + cardId);
            }
            else
            {
                resItem.Path = AppConfig.Instance.assetsServer + "/" + AppConfig.Instance.version + "/" +
                               resPack.downloadPath + "/" + id;

                _downloadingWindow = PopupManager.ShowWindow<DownloadingWindow>(Constants.DownloadingWindowPath);
                _downloadingWindow.Content = I18NManager.Get("Download_Downloading");
                _downloadingWindow.Progress = I18NManager.Get("Download_Progress",0);
                DownloadManager.Load(resItem, AssetLoader.ExternalHotfixPath + "/" + resPack.releasePath + "/" + id,
                    OnCompleteEvoCard, OnErrorEvoCard, OnProgress);
            }            
        }

        public static void DownloadEvoCardAll(Action<string> onComplete = null, string tag = null)
        {
            _onComplete = onComplete;
            _tag = tag;

            ResIndex resIndex = GetIndexFile(ResPath.EvoCardPackageIndex);

            DownloadManager.Clear();

            ResPack resPack = resIndex.packageDict["EvoCard"];
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

                _downloadingWindow = PopupManager.ShowWindow<DownloadingWindow>(Constants.DownloadingWindowPath);
                _downloadingWindow.Content = I18NManager.Get("Download_Downloading");
                _downloadingWindow.Progress = I18NManager.Get("Download_Progress",0);
                DownloadManager.Load(item, AssetLoader.ExternalDownloadPath + "/" + resPack.downloadPath,
                    OnCompleteEvoCard,
                    OnErrorEvoCard, OnProgress);
            }
        }

        private static void OnErrorEvoCard(DownloadItem item)
        {
            Debug.LogError("下载压缩包失败：" + item.ErrorText);

            FlowText.ShowMessage(I18NManager.Get("Download_ErrorAndRetry"));
        }

        private static void OnCompleteEvoCard(DownloadItem downloadItem)
        {
            _downloadingWindow.HindCancelBtn();
            _downloadingWindow.Content = I18NManager.Get("Download_Unzip");
            _downloadingWindow.Progress = I18NManager.Get("Download_Progress",0);

            bool isEnd = false;
            progress = 0;
            Tweener tween = DOTween.To(() => progress, val => progress = val, 100, 1.5f);

            tween.onUpdate = () =>
            {
                _downloadingWindow.SetProgress(progress);
                //_downloadingWindow.Content = I18NManager.Get("Download_Unzip");
                _downloadingWindow.Progress = I18NManager.Get("Download_Progress", progress);
            };

            tween.onComplete = () =>
            {
                if (isEnd == false)
                {
                    isEnd = true;
                }
                else
                {
                    _downloadingWindow.Close();
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
                        _downloadingWindow.Close();
                        _onComplete?.Invoke(_tag);
                    }
                });
            });
        }

        public static CacheVo CheckEvoCardCache(int cardId)
        {
            Stopwatch sw = Stopwatch.StartNew();

            CacheVo vo = new CacheVo();

            ResIndex resIndex = GetIndexFile(ResPath.EvoCardIndex);
            if (resIndex == null)
                return vo;

            if (resIndex.packageDict.ContainsKey(cardId.ToString()) == false)
            {
                FlowText.ShowMessage(I18NManager.Get("Download_IndexNoExist") + cardId);
                return vo;
            }

            ResPack resPack = resIndex.packageDict[cardId.ToString()];

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

            Debug.LogError("CheckEvoCardCache 时间：" + sw.ElapsedMilliseconds);

            return vo;
        }
    }
}