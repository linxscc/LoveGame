using System;
using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Module.Download;
using Common;
using DataModel;
using game.main;
using UnityEngine;

public partial class MainPanelView
{
    private Action _downloadExtendTask = null;

    /// <summary>
    /// 新手引导结束后需要判断是否下载拓展包
    /// </summary>
    public void CheckNeedToDownLoadExtend()
    {
        var curStep = GuideManager.GetRemoteGuideStep(GuideTypePB.MainGuide);
   
        if (curStep >= GuideConst.MainLineStep_OnClick_Battle2_11_Sweep)
        {
            if (_extendCacheVo == null)
            {
                _extendCacheVo = CacheManager.CheckExtendCache();
                Dictionary<string, CacheVo> backendDict = CacheManager.CheckBackEndCacheDic(false);
                
                if (_extendCacheVo.needDownload || backendDict.Count > 0)
                {
                    //要先知道玩家是否首次下载！
                    Debug.LogError("CheckNeedtoDownLoadExtend");
                    long packsize = CacheManager.GetExtendSize(_extendCacheVo) +
                                    CacheManager.CheckCurBackEndCacheSize();

                    Debug.LogError("packsize" + packsize);
                    SendMessage(new Message(MessageConst.CMD_HIDEVIEW));  
                    if (_backDownLoading)
                    {
                        PopupManager.ShowDownloadTipsWindow(packsize, null, null,
                            GlobalData.PlayerModel.PlayerVo.ExtInfo.DownloadReceive == 1).WindowActionCallback = evt =>
                        {
                            if (evt == WindowEvent.Ok)
                            {
                                LoadingProgress.Instance.Showview(true);
                                //LoadingProgress.Instance.ShowDownloadInfo(true);
                                //防止后台下载已经结束
                                if (_backDownLoading)
                                {
                                    _downloadExtendTask = () => { DownloadExtend(); };
                                }
                                else
                                {
                                    DownloadExtend();
                                }
                            }
                        };
                    }
                    else
                    {
                        PopupManager.ShowDownloadTipsWindow(packsize, null, null,
                                GlobalData.PlayerModel.PlayerVo.ExtInfo.DownloadReceive == 1).WindowActionCallback =
                            evt =>
                            {
                                if (evt == WindowEvent.Ok)
                                {
                                    LoadingProgress.Instance.Show();
                                    DownloadExtend();
                                }
                            };
                    }
                }
                else
                {
                    // Debug.LogError("hasdownload and didn't receive 5 starCard");
                    SendMessage(new Message(MessageConst.CMD_MAIN_FIRESHDOWNLOADAWARD));
                }
            }
        }
    }

    private void DownloadExtend()
    {
        CacheManager.DownloadExtendCache(str =>
        {
            Debug.LogError(str + "下载完成");
            //解压后台下载的压缩包
            CacheManager.UnzipBackend(tagString =>
            {
                Debug.LogError("UnzipBackend完成");
                LoadingProgress.Instance.ClearPackCount();
                LoadingProgress.Instance.TotalSize = 0;
                LoadingProgress.Instance.Hide();
                _extendCacheVo = CacheManager.CheckExtendCache();
                SetMainLive2d(GlobalData.PlayerModel.PlayerVo);
                SendMessage(new Message(MessageConst.CMD_MAIN_FIRESHDOWNLOADAWARD));
            });
        }, ResPath.Extend, err =>
        {
            Debug.LogError(err);
            LoadingProgress.Instance.ShowAlertWindw(true, "下载出现问题，请尝试重登游戏!", null, null, () => { DownloadExtend(); });
        });
    }


    public void CheckNeedToDownLoadBacnend()
    {
        if (_backCacheVo == null)
        {
            DownloadBackEnd();
        }
    }

    public void DownloadBackEnd()
    {
        _backCacheVo = CacheManager.CheckBackEndCacheDic(true);

        if (_backCacheVo.Count > 0)
        {
            //状态判断避免二次下载
            Debug.LogError("CheckNeedToDownLoadBacnending");
            _backDownLoading = true;
            CacheManager.DownLoadBackEndCacheQueue(_backCacheVo, str =>
            {
                _backDownLoading = false;

                if (_downloadExtendTask != null)
                {
                    _downloadExtendTask.Invoke();
                    _downloadExtendTask = null;
                }
                else
                {
                    LoadingProgress.Instance.ClearPackCount();
                    SetMainLive2d(GlobalData.PlayerModel.PlayerVo);
                }
            }, null, err =>
            {
                Debug.LogError(err);
                LoadingProgress.Instance.ShowAlertWindw(true, "下载出现问题，请重试下载！", null, null, () =>
                {
                    LoadingProgress.Instance.ClearPackCount();
                    _backCacheVo = CacheManager.CheckBackEndCacheDic(true);
                    if (_backCacheVo.Count > 0)
                    {
                        DownloadBackEnd();
                    }
                    else
                    {
                        DownloadExtend();
                    }
                });
            });
        }
    }

}