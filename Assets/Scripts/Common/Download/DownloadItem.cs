using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using Framework.Utils;
using UnityEngine.Networking;
using Debug = UnityEngine.Debug;

namespace Assets.Scripts.Module.Download
{
    public class DownloadItem
    {
        public string Url { get; }

        public string Md5 { get; }

        public long FileSize { get; }

        public string LocalPath { get; }

        public string ErrorText { get; private set; }

        public FileType FileType { get; }

        private Action<float> _onProgress;
        private Action<DownloadItem> _onError;
        private Action<DownloadItem> _onCancel;

        private int _retryCount = 0;
        private Action<DownloadItem> _onComplete;

        private bool _isCancel = false;

        public bool UseRange = false;

        public void Cancel()
        {
            _isCancel = true;
        }

        public DownloadItem(string url, string md5, long fileSize, string localPath, FileType fileType = FileType.None)
        {
            Url = url;
            Md5 = md5;
            FileSize = fileSize;
            LocalPath = localPath;
            FileType = fileType;

            if (FileSize > 5 * 1024 * 1024)
            {
                //大于5m的文件使用断点续传
                UseRange = true;
            }
        }

        public void Start(Action<float> onProgress, Action<DownloadItem> onError, Action<DownloadItem> onComplete,
            Action<DownloadItem> onCancel)
        {
            _onProgress = onProgress;
            _onError = onError;
            _onComplete = onComplete;
            _onCancel = onCancel;

            FileInfo file = new FileInfo(LocalPath);
            if (file.Exists)
            {
                if (file.Length > FileSize)
                {
                    Debug.LogWarning("DownloadItem：文件长度大于配置，删除文件");
                    file.Delete();
                    UseRange = false;
                    MonoObject.Instance.StartCoroutine(DoDownload());
                }
                else if (file.Length == FileSize)
                {
                    LoadingProgress.Instance.SetDownloadText(I18NManager.Get("Download_CheckFiles"));
                    Loom.RunAsync(() =>
                    {
                        Stopwatch sw = Stopwatch.StartNew();
                        bool md5Fixed = CheckMd5();

                        Loom.QueueOnMainThread(() =>
                        {
                            Debug.LogWarning("===StartDownload CheckMd5耗时：" + sw.ElapsedMilliseconds + "ms Url->" + Url);

                            if (md5Fixed)
                            {
                                Debug.LogWarning("DownloadItem：文件MD5匹配，完成下载");
                                _onComplete?.Invoke(this);
                            }
                            else
                            {
                                Debug.LogWarning("DownloadItem：文件MD5不匹配，删除文件。重新开始下载");
                                file.Delete();
                                UseRange = false;
                                MonoObject.Instance.StartCoroutine(DoDownload());
                            }
                        });
                    });
                }
                else
                {
                    MonoObject.Instance.StartCoroutine(DoDownload());
                }
            }
            else
            {
                MonoObject.Instance.StartCoroutine(DoDownload());
            }
        }

        private IEnumerator DoDownload()
        {
            UnityWebRequest unityWebRequest = new UnityWebRequest(Url);
            unityWebRequest.downloadHandler =
                new DownloadHandlerFileRange(LocalPath, unityWebRequest, UseRange, FileSize);
            DownloadHandlerFileRange handler = (DownloadHandlerFileRange) unityWebRequest.downloadHandler;
            unityWebRequest.useHttpContinue = false;

            handler.StartDownloadEvent += () =>
            {
                if (FileSize > -1 && FileSize != handler.FileSize)
                {
                    ErrorText = "fileSize error: targetSize->" + handler.FileSize + " ConfigSize->" + FileSize;
                    _onError(this);
                    handler.Dispose();
                    unityWebRequest.Dispose();
                    unityWebRequest = null;
                }
            };

            handler.ErrorEvent += () =>
            {
                ErrorText = "targetSize->" + handler.FileSize + " ConfigSize->" + FileSize;
                _onError(this);
                handler.Dispose();
                unityWebRequest.Dispose();
                unityWebRequest = null;
            };

            unityWebRequest.SendWebRequest();

            if (unityWebRequest == null)
                yield break;

            while (unityWebRequest.isDone == false)
            {
                if (_isCancel)
                {
                    _onCancel?.Invoke(this);
                    break;
                }

                _onProgress?.Invoke(handler.DownloadProgress);
                yield return null;
            }

            if (_isCancel == false)
            {
                if (unityWebRequest.isHttpError || unityWebRequest.isNetworkError)
                {
                    ErrorText = unityWebRequest.error;
                    _onError?.Invoke(this);
                }
                else
                {
                    CheckFile();
                }
            }

            handler.Dispose();
            unityWebRequest.Dispose();
        }

        private void CheckFile()
        {
            if (CheckLength() == false)
            {
                Debug.LogException(new Exception("CheckFile===>CheckLength Error Url->" + Url));
                Retry();
                return;
            }

            LoadingProgress.Instance.SetDownloadText(I18NManager.Get("Download_CheckFiles"));
            
            Loom.RunAsync(() =>
            {
                Stopwatch sw = Stopwatch.StartNew();
                
                bool md5Fixed = CheckMd5();

                Loom.QueueOnMainThread(() =>
                {
                    Debug.LogWarning("===CheckFile CheckMd5耗时：" + sw.ElapsedMilliseconds + "ms Url->" + Url);

                    if (md5Fixed)
                    {
                        _onComplete?.Invoke(this);
                    }
                    else
                    {
                        Debug.LogException(new Exception("CheckFile===>md5Fixed Error Url->" + Url));
                        Retry();
                    }
                });
            });
        }

        private bool CheckLength()
        {
            FileInfo fileInfo = new FileInfo(LocalPath);
            if (FileSize < 0 || fileInfo.Length == FileSize)
            {
                return true;
            }

            ErrorText = "文件长度不一致 配置siez：" + FileSize + " 文件size：" + fileInfo.Length;
            return false;
        }

        private bool CheckMd5()
        {
            //如果md5为空，不检查md5
            if (Md5 == null)
                return true;

            string targetMd5 = MD5Util.Get(LocalPath);

            if (targetMd5 != Md5)
                return false;

            return true;
        }

        /// <summary>
        /// 下载失败自动重试一次
        /// </summary>
        private void Retry()
        {
            Debug.LogError("重新下载 retryCount:" + _retryCount);

            if (_retryCount >= 1)
            {
                ErrorText = "重试下载文件还是失败 ConfigMd5:" + Md5;
                _onError?.Invoke(this);
                return;
            }

            _retryCount++;

            File.Delete(LocalPath);

            MonoObject.Instance.StartCoroutine(DoDownload());
        }
    }
}