using System;
using System.Collections.Generic;
using game.main;
using UnityEngine;

namespace Assets.Scripts.Module.Download
{
    public class DownloadManager
    {
        public static Action LoaderQueueComplete;
        public static Action<List<DownloadItem>> LoaderQueueError;

        public static int LoaderMaxCount = 5;

        private static int _currentLoaderCount = 0;

        private static Action<DownloadItem> _onComplete;
        private static Action<float> _onProgress;
        private static Action<DownloadItem> _onError;

        private static List<DownloadItem> _errorQueueItems = new List<DownloadItem>();

        private static int _totalFiles = 0;
        private static int _loadedFiles = 0;
        private static Queue<IDownloadItem> _queue;

        private static string _queueUrl;
        private static int _errorFiles;
        private static Action<float> _onQueueProgress;
        private static Action<DownloadItem> _onQueueError;
        private static Action<DownloadItem> _onCancel;

        public static DownloadItem Load(string url, string localPath, Action<DownloadItem> onComplete,
            Action<DownloadItem> onError, Action<float> onProgress, Action<DownloadItem> onCancel = null)
        {
            _onComplete = onComplete;
            _onProgress = onProgress;
            _onError = onError;
            _onCancel = onCancel;

            DownloadItem item = new DownloadItem(url, null, -1, localPath);
            item.Start(OnProgress, OnError, OnLoaded, OnCancel);

            return item;
        }

        private static void OnCancel(DownloadItem downloadItem)
        {
            _onCancel?.Invoke(downloadItem);

            _onError = null;
            _onComplete = null;
            _onProgress = null;
            _onCancel = null;
        }

        public static DownloadItem Load(IDownloadItem downloadItem, string localPath, Action<DownloadItem> onComplete,
            Action<DownloadItem> onError, Action<float> onProgress, Action<DownloadItem> onCancel = null)
        {
            _onComplete = onComplete;
            _onProgress = onProgress;
            _onError = onError;
            _onCancel = onCancel;

            DownloadItem item = new DownloadItem(downloadItem.Path, downloadItem.Md5, downloadItem.Size, localPath,
                downloadItem.FileType);
            item.Start(OnProgress, OnError, OnLoaded, OnCancel);
            
            return item;
        }

        private static void OnLoaded(DownloadItem downloadItem)
        {
            _onComplete?.Invoke(downloadItem);

//            _onError = null;
//            _onComplete = null;
//            _onProgress = null;
//            _onCancel = null;
        }

        private static void OnError(DownloadItem downloadItem)
        {
            _onError?.Invoke(downloadItem);

            _onError = null;
            _onComplete = null;
            _onProgress = null;
            _onCancel = null;
        }

        private static void OnProgress(float progress)
        {
            _onProgress?.Invoke(progress);
        }

        public static void AddList(List<IDownloadItem> items)
        {
            _totalFiles += items.Count;

            if (_queue == null)
                _queue = new Queue<IDownloadItem>();

            for (int i = 0; i < items.Count; i++)
            {
                _queue.Enqueue(items[i]);
            }
        }

        public static void SetQueueCallback(Action<float> onProgress, Action<DownloadItem> onError)
        {
            _onQueueProgress = onProgress;
            _onQueueError = onError;
        }

        public static void StartQueue(string url = null)
        {
            if (url != null)
                _queueUrl = url;

            if (_queueUrl == null)
                throw new Exception("DownloadManager：下载队列没有设置Url");

            int num = LoaderMaxCount - _currentLoaderCount;
            for (int i = 0; i < num; i++)
            {
                if (_queue.Count == 0)
                    break;

                IDownloadItem item = _queue.Dequeue();

                DownloadItem downloadItem = new DownloadItem(_queueUrl + item.Path, item.Md5, item.Size,
                    AssetLoader.ExternalHotfixPath + "/" + item.Path);
                downloadItem.Start(OnQueueItemProgress, OnQueueItemError, OnQueueItemLoaded, null);

                _currentLoaderCount++;
            }
        }

        private static void OnQueueItemLoaded(DownloadItem downloadItem)
        {
            _loadedFiles++;
            _currentLoaderCount--;

            StartQueue();

            if (_loadedFiles == _totalFiles)
            {
                LoaderQueueComplete?.Invoke();
                Clear();
            }

            _onQueueProgress?.Invoke((float) _loadedFiles / _totalFiles);
            Debug.LogError("=========OnQueueItemLoaded:" + _loadedFiles);
        }

        private static void OnQueueItemProgress(float progress)
        {
            _onQueueProgress?.Invoke(progress);
            if (progress >= 1)
            {
            }
        }

        private static void OnQueueItemError(DownloadItem downloadItem)
        {
            _errorFiles++;
            _errorQueueItems.Add(downloadItem);

            Debug.Log("downloadItem Error:" + downloadItem.Url + " " + downloadItem.ErrorText);

            _currentLoaderCount--;

            StartQueue();

            if (_errorFiles + _loadedFiles == _totalFiles)
            {
                LoaderQueueError?.Invoke(_errorQueueItems);
            }
        }

        public static void Clear()
        {
            _totalFiles = 0;
            _loadedFiles = 0;
            _errorFiles = 0;
            _errorQueueItems = new List<DownloadItem>();
            LoaderQueueComplete = null;
            LoaderQueueError = null;
        }
    }
}