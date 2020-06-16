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
        private static List<int> _phoneCacheList;

        public static void DownloadPhoneCache(int role, Action<string> onComplete = null, Action onCancel=null, string tag = null)
        {
            _onComplete = onComplete;
            _tag = tag;

            ResIndex resIndex = GetIndexFile(ResPath.PhoneAudioPackageIndex);
            if (!resIndex.packageDict.ContainsKey(role.ToString()))
            {
                FlowText.ShowMessage(I18NManager.Get("Download_ErrorRole") + role);
                return;
            }

            DownloadManager.Clear();

            ResPack resPack = resIndex.packageDict[role.ToString()];
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
                var downItem =  DownloadManager.Load(item, AssetLoader.ExternalDownloadPath + "/" + resPack.downloadPath, OnCompletePhone,
                    OnErrorPhone, OnProgress);
                _downloadingWindow.WindowActionCallback = evt =>
                {
                    if (evt == WindowEvent.Cancel)
                    {
                        onCancel?.Invoke();
                        downItem.Cancel();
                    }
                };    
            }
        }

        private static void OnErrorPhone(DownloadItem item)
        {
            Debug.LogError("下载压缩包失败：" + item.ErrorText);

            FlowText.ShowMessage(I18NManager.Get("Download_ErrorAndRetry"));
        }

        private static void OnCompletePhone(DownloadItem downloadItem)
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
               // _downloadingWindow.Content = I18NManager.Get("Download_Unzip");
                _downloadingWindow.Progress = I18NManager.Get("Download_Progress",progress);
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

        /// <summary>
        /// 返回未下载的索引数值
        /// </summary>
        /// <returns>未下载的索引数值</returns>
        public static CacheVo CheckPhoneCache()
        {
            Stopwatch sw = Stopwatch.StartNew();
            
            CacheVo vo = new CacheVo();
            vo.sizeList = new List<long>();
            vo.ids = new List<int>();
            _phoneCacheList = new List<int>();

            ResIndex resIndex = GetIndexFile(ResPath.PhoneAudioPackageIndex);
            if (resIndex == null)
                return vo;
            
            foreach (var resPack in resIndex.packageDict)
            {
                string role = resPack.Key;
                _releasePath = resPack.Value.releasePath;
                bool isNeedDownload = false;
                foreach (var resItem in resPack.Value.items)
                {
                    FileInfo fileInfo =
                        new FileInfo(AssetLoader.ExternalHotfixPath + "/" + _releasePath + "/" + resItem.Path);

                    if (fileInfo.Exists == false || fileInfo.Length != resItem.Size)
                    {
                        vo.needDownload = true;
                        isNeedDownload = true;
                        break;
                    }
                }

                vo.sizeList.Add(resPack.Value.packageSize);

                if (isNeedDownload)
                {
                    _phoneCacheList.Add(Convert.ToInt32(role));
                    vo.ids.Add(Convert.ToInt32(role));
                }
            }
            Debug.LogError("CheckPhoneCache 时间：" + sw.ElapsedMilliseconds);
            
            return vo;
        }

        public static bool isGuideSmsBySceneId(int SceneId)
        {
            if (SceneId == 4512 || 
                SceneId == 1025 ||
                SceneId == 2036 || 
                SceneId == 3128 || 
                SceneId == 4537 ||
                SceneId==101)
            {
                return true;
            }
            return false;
        }

        public static void ClickItem(int NpcId, Action finish = null, Action cancel = null)
        {
            int needtoTips = 1;
            int cunId = 1008699 + NpcId;
            if (PlayerPrefs.HasKey("RecordLoveStory" + cunId))
            {
                needtoTips = PlayerPrefs.GetInt("RecordLoveStory" + cunId);
            }
            Debug.LogError("cunId " + cunId);
            CacheVo vo = CacheManager.CheckPhoneCache();

            if (vo.needDownload == false || !vo.ids.Contains(NpcId) || needtoTips != 1)
            {
                finish?.Invoke();
                return;
            }

            PlayerPB playerPB = (PlayerPB)(NpcId);
            string NpcName = Util.GetPlayerName(playerPB);

            long size = 10;
            if(NpcId< vo.sizeList.Count)
            {
                size = vo.sizeList[NpcId-1] / (1024 * 1024);
            }
           
            int isize = int.Parse(size.ToString());
            if (Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork)
            {
                CacheManager.DownloadPhoneCache(NpcId, (str) =>
                {
                    finish?.Invoke();
                });
            }
            else
            {
                //弹出确认框
                string content = I18NManager.Get("Phone_IsDownloadContext", NpcName, isize);
                CacheManager.ConfirmPhoneNeedToDownload(content
                , I18NManager.Get("Download_JumpDownload"), cunId, NpcId, str =>
                {
                    finish?.Invoke();

                }, () =>
                {
                    cancel?.Invoke();
                });
            }
        }


        public static void ConfirmPhoneNeedToDownload(string content, string canclestr, int cardid, int npcId, Action<string> onComplete = null, Action cancleCallBack = null)
        {
            _confirmChooseWindow = PopupManager.ShowWindow<ConfirmChooseWindow>(Constants.ConfirmChooseWindowPath);

            _confirmChooseWindow.Content = content;
            _confirmChooseWindow.RecordCardId = cardid;
            _confirmChooseWindow.CancelText = canclestr;
            _confirmChooseWindow.WindowActionCallback = evt =>
            {
                if (evt == WindowEvent.Ok)
                {
                    DownloadPhoneCache(npcId, onComplete, cancleCallBack);
                }
                else if (evt == WindowEvent.Cancel)
                {
                    cancleCallBack?.Invoke();
                }
            };


        }

    }
}