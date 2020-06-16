using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Assets.Scripts.Services;
using DG.Tweening;
using Framework.Utils;
using game.main;
using UnityEngine;
using Utils;
using Debug = UnityEngine.Debug;

namespace Assets.Scripts.Module.Download
{
    /// <summary>
    /// 用于下载需要缓存的文件
    /// </summary>
    public partial class CacheManager
    {
        private static Action<string> _onComplete;
        private static Action<string> _onError;

        private static string _tag;
        private static DownloadingWindow _downloadingWindow;
        private static ConfirmChooseWindow _confirmChooseWindow;
        private static ConfirmNoChooseWindow _confirmNoChooseWindow;
        private static string _releasePath;

        private static int progress;
        
        private static TimerHandler timerHandler;

        public static void DownloadChapterCache(int chapter, Action<string> onComplete = null, string tag = null)
        {
            _onComplete = onComplete;
            _tag = tag;

            ResIndex resIndex = GetIndexFile(ResPath.MainStoryIndex);
            if (!resIndex.packageDict.ContainsKey(chapter.ToString()))
            {
                FlowText.ShowMessage(I18NManager.Get("Download_ErrorChapter") + chapter);
                return;
            }

            DownloadManager.Clear();

            ResPack resPack = resIndex.packageDict[chapter.ToString()];
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
                _downloadingWindow.Progress = I18NManager.Get("Download_Progress", 0);
                var downItem = DownloadManager.Load(item, AssetLoader.ExternalDownloadPath + "/" + resPack.downloadPath,
                    OnComplete,
                    OnError, OnProgress);
                _downloadingWindow.WindowActionCallback = evt =>
                {
                    if (evt == WindowEvent.Cancel)
                    {
                        downItem.Cancel();
                    }
                };
            }
        }

        private static void OnProgress(float progress)
        {
            int p = (int) (progress * 100);

            _downloadingWindow.Content = I18NManager.Get("Download_Downloading");
            _downloadingWindow.Progress = I18NManager.Get("Download_Progress", p);
            _downloadingWindow.SetProgress(p);
        }

        private static void OnError(DownloadItem item)
        {
            Debug.LogError("下载压缩包失败：" + item.Url + " ErrorText:" + item.ErrorText);

            FlowText.ShowMessage(I18NManager.Get("Download_ErrorAndRetry"));

            if (_onError != null)
            {
                _onError.Invoke(item.Url + " " + item.ErrorText);
                _onError = null;
            }
        }

        private static void OnComplete(DownloadItem downloadItem)
        {
            _downloadingWindow.HindCancelBtn();
            _downloadingWindow.Content = I18NManager.Get("Download_Unzip");
            _downloadingWindow.Progress = I18NManager.Get("Download_Progress", 0);

            float time = (float) downloadItem.FileSize / 1024 / 1024 / 16;
            if (time < 0.5f)
                time = 0.5f;

            bool isEnd = false;
            progress = 0;
            Tweener tween = DOTween.To(() => progress, val => progress = val, 100, time);

            tween.onUpdate = () =>
            {
                _downloadingWindow.SetProgress(progress);
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
                    File.Delete(downloadItem.LocalPath);
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

        public static bool IsNeedDownLoad(int chapter)
        {
            bool isNeed = false;
            ResIndex resIndex = GetIndexFile(ResPath.MainStoryIndex);
            if (resIndex == null)
            {
                return isNeed;
            }

            if (resIndex.packageDict.ContainsKey(chapter.ToString()))
            {
                ResPack resPack = resIndex.packageDict[chapter.ToString()];

                foreach (var resItem in resPack.items)
                {
                    FileInfo fileInfo =
                        new FileInfo(AssetLoader.ExternalHotfixPath + "/" + resPack.releasePath + "/" + resItem.Path);
                    if (fileInfo.Exists == false || fileInfo.Length != resItem.Size)
                    {
                        isNeed = true;
                        break;
                    }
                }
            }

            return isNeed;
        }


        /// <summary>
        /// 返回未下载的索引数值
        /// </summary>
        /// <returns>未下载的索引数值</returns>
        public static CacheVo CheckMainStoryCache(int chapter)
        {
            Stopwatch sw = Stopwatch.StartNew();

            CacheVo vo = new CacheVo();

            vo.ids = new List<int>();

            ResIndex resIndex = GetIndexFile(ResPath.MainStoryIndex);

            if (resIndex == null)
                return vo;

            if (resIndex.packageDict.ContainsKey(chapter.ToString()) == false)
            {
                FlowText.ShowMessage(I18NManager.Get("Download_IndexNoExist") + chapter);
                return vo;
            }

            ResPack resPack = resIndex.packageDict[chapter.ToString()];

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

            if (vo.needDownload)
                vo.ids.Add(Convert.ToInt32(chapter));

            vo.sizeList = new List<long>() {resPack.packageSize};

            Debug.LogError("CheckMainStoryCache 时间：" + sw.ElapsedMilliseconds);

            return vo;
        }

        private static ResIndex GetIndexFile(string path)
        {
            IndexFileService service = new IndexFileService();
            service.SetPath(AssetLoader.GetIndexFilePath(path)).Execute();

            ResIndex resIndex = (ResIndex) service.GetData();

            return resIndex;
        }


        public static long GetDownloadAllSize()
        {
            ResIndex resIndex = GetIndexFile(ResPath.AllAudioIndex);

            if (resIndex != null)
            {
                ResPack resPack = resIndex.packageDict["AllAudio"];
                return resPack.packageSize;
            }
            else
            {
                return 0;
            }
        }


        public static void DownloadAllAudio(Action<string> onComplete = null, string tag = null)
        {
            _onComplete = onComplete;
            _tag = tag;

            ResIndex resIndex = GetIndexFile(ResPath.AllAudioIndex);
            ResPack resPack = resIndex.packageDict["AllAudio"];
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
                _downloadingWindow.Progress = I18NManager.Get("Download_Progress", 0);
                var downItem = DownloadManager.Load(item, AssetLoader.ExternalDownloadPath + "/" + resPack.downloadPath,
                    OnComplete,
                    OnError, OnProgressAll);

                _downloadingWindow.WindowActionCallback = evt =>
                {
                    if (evt == WindowEvent.Cancel)
                    {
                        downItem.Cancel();
                    }
                };
            }
        }


        private static void OnProgressAll(float progress)
        {
            int p = (int) (progress * 100);

            _downloadingWindow.Content = I18NManager.Get("Download_Downloading");
            _downloadingWindow.Progress = I18NManager.Get("Download_Progress", p);
            _downloadingWindow.SetProgress(p);
        }


        public static void ConfirmNeedToDownloadChapterAudio(string content, string ok, string cancel, int chapterId,
            Action<string> onComplete = null, Action cancelCallBack = null)
        {
            _confirmNoChooseWindow =
                PopupManager.ShowWindow<ConfirmNoChooseWindow>(Constants.ConfirmNoChooseWindowPath);
            _confirmNoChooseWindow.Content = content;
            _confirmNoChooseWindow.OkText = ok;
            _confirmNoChooseWindow.CancelText = cancel;
            _confirmNoChooseWindow.WindowActionCallback = evt =>
            {
                if (evt == WindowEvent.Ok)
                {
                    DownloadChapterCache(chapterId, onComplete);
                }
                else if (evt == WindowEvent.Cancel)
                {
                    cancelCallBack?.Invoke();
                }
            };
        }

        private static void OnProgressFullLoading(float progress)
        {
            LoadingProgress.Instance.SetPercent(progress * 100);
        }

        private static void OnCompleteFullLoading(DownloadItem downloadItem)
        {
            Loom.RunAsync(() =>
            {
                if (downloadItem.FileType == FileType.Zip)
                {
                    try
                    {
                        ZipUtil.UnzipThread(downloadItem.LocalPath,
                            AssetLoader.ExternalHotfixPath + "/" + _releasePath);
                    }
                    catch (IOException e)
                    {
                        Debug.LogException(e);
                    }
                }

                Loom.QueueOnMainThread(() =>
                {
                    File.Delete(downloadItem.LocalPath);
                    if (_tag == ResPath.AllResources)
                    {
                        //下载完所有资源标记下载
                        PlayerPrefs.SetInt(UpdateController.LastVersionKey, AppConfig.Instance.version);
                        MarkDownloadAll();
                    }
                    else if (_tag == ResPath.AppStart)
                    {
                        FileMark fm = new FileMark(AssetLoader.ExternalHotfixPath, ResPath.AppStart);
                        fm.UpdateRecord(AppConfig.Instance.version + "");
                    }
                    else if(_tag == ResPath.Extend)
                    {
                        FileMark fm = new FileMark(AssetLoader.ExternalHotfixPath, ResPath.Backend);
                        fm.UpdateRecord(AppConfig.Instance.version + "");
                    }

                    if(timerHandler != null)
                        ClientTimer.Instance.RemoveCountDown(timerHandler);
                    
                    _onComplete?.Invoke(_tag);
                });
            });

            if (downloadItem.FileType == FileType.Zip)
            {
                timerHandler = ClientTimer.Instance.AddCountDown("Upzip", Int64.MaxValue, 0.1f,
                    val =>
                    {
                        int progress = (int) ((float) ZipUtil.CurrentSize / ZipUtil.TotalSize * 100);
                        LoadingProgress.Instance.SetPercent(progress, true);
                    }, null);
            }
        }
    }
}