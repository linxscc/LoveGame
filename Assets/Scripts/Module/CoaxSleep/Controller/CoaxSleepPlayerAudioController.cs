using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Core.Events;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module.Download;
using Assets.Scripts.Module.NetWork;
using Com.Proto;
using Common;
using DataModel;
using game.main;
using UnityEngine;

public class CoaxSleepPlayerAudioController : Controller
{
    public CoaxSleepPlayerAudioView View;
    private PlayerPB _playerPb;
    private GemUnlockWindow _gemUnlockWindow;
    
    public override void Init()
    {
        base.Init();
        EventDispatcher.AddEventListener<MyCoaxSleepAudioData>(EventConst.OnClickCoaxSleepPlay, OnPlayAudioBtn);   
        EventDispatcher.AddEventListener<MyCoaxSleepAudioData>(EventConst.OnClickUnlockToGem,OnClickUnlockToGem);
    }

   
    public override void Start()
    {
        base.Start();
    }

    public override void Destroy()
    {
        base.Destroy();
        EventDispatcher.RemoveEvent(EventConst.OnClickCoaxSleepPlay);
        EventDispatcher.RemoveEvent(EventConst.OnClickUnlockToGem);
    }
    
    /// <summary>
    /// 点击解锁按钮（通过消耗Gem）
    /// </summary>
    /// <param name="data"></param>
    private void OnClickUnlockToGem(MyCoaxSleepAudioData data)
    {
        _gemUnlockWindow = PopupManager.ShowWindow<GemUnlockWindow>("CoaxSleep/Prefabs/GemUnlockWindow"); 
        _gemUnlockWindow.SetData(data.Gem);
        _gemUnlockWindow.WindowActionCallback = evt =>
        {
            if (evt == WindowEvent.Ok)
            {                
                SendCoaxSleepReq(data.AudioId);
            }
        };
    }

    
    /// <summary>
    /// 点击播放按钮
    /// </summary>
    /// <param name="data"></param>
    private void OnPlayAudioBtn(MyCoaxSleepAudioData data)
    {
        //校验下是否发送请求
       var isSend =  GetData<CoaxSleepModel>().IsSendReq(data.PlayerPb, data.AudioId);

       //校验是否需要下载资源
       var isDownLoad = GetData<CoaxSleepModel>().IsNeedDownload(data.AudioId);
       
       if (isSend)       
           SendCoaxSleepReq(data.AudioId);

       if (!isDownLoad)
       {
           SendMessage(new Message(MessageConst.CMD_CPAXSLEEP_SHOW_ON_PLAY_PANEL,data));
       }
       else
       {
           DownLoadAudio(data);
       } 
    }

    
    /// <summary>
    /// 下载资源
    /// </summary>
    private void DownLoadAudio(MyCoaxSleepAudioData data)
    {
        var curData = GetData<CoaxSleepModel>().GetLoadAudioJsonData(data.AudioId) ?? GetData<CoaxSleepModel>().GetServerAudioJsonData(data.AudioId);

        var size =Math.Round(curData.Size*1f/1048576f,2) ;
        
        var window = ConfirmDownload(size);
        
        window.WindowActionCallback = evt =>
        {
            if (evt == WindowEvent.Ok)
            {
                OpenDownloadingWindow(size,data);
            }
        };
    }
    /// <summary>
    /// 下载确认
    /// </summary>
    /// <returns></returns>
    private ConfirmWindow ConfirmDownload(double size)
    {              
        string content = I18NManager.Get("CoaxSleep_DownloadConfirm",size);                     
        return PopupManager.ShowConfirmWindow(content);       
    }
    
    /// <summary>
    /// 打开下载进度窗口
    /// </summary>
    /// <param name="size"></param>
    private void OpenDownloadingWindow(double size,MyCoaxSleepAudioData data)
    {
        var downloadingWindow = PopupManager.ShowWindow<DownloadingWindow>(Constants.DownloadingWindowPath);
        downloadingWindow.Content = I18NManager.Get("CoaxSleep_DownloadConfirm", size);
        downloadingWindow.Progress = I18NManager.Get("Download_Progress", 0);
        downloadingWindow.HindCancelBtn();
        string url = GetData<CoaxSleepModel>().GetUrl(data.AudioId);
        string localPath = GetData<CoaxSleepModel>().GetLocalPath(data.AudioId);
        
        DownloadManager.Load(url, localPath, 
        (item =>
        {
         
            downloadingWindow.Close();
            SendMessage(new Message(MessageConst.CMD_CPAXSLEEP_SHOW_ON_PLAY_PANEL,data));
            SetUpdateLoadAudioData(data);
        }), 
        (item =>
        {
            Debug.LogError("OnLoadHotfixConfigError==>" + item.ErrorText + " Url->" + item.Url);
            FlowText.ShowMessage(item.ErrorText);
        }),
        (f =>
        {
            int p = (int) (f * 100);
            downloadingWindow.Progress = I18NManager.Get("Download_Progress", p);
            downloadingWindow.SetProgress(p);
        }));
    }



    private void SetUpdateLoadAudioData(MyCoaxSleepAudioData data)
    {
        GetData<CoaxSleepModel>().CheckUpdateAudioJsonData(data.AudioId);
    }
    

    private void SendCoaxSleepReq(int audioId)
    {
        UnlockCoaxSleepAudioReq req = new UnlockCoaxSleepAudioReq
        {
            AudioId = audioId
        };
        byte[] data = NetWorkManager.GetByteData(req);
        
        NetWorkManager.Instance.Send<UnlockCoaxSleepAudioRes>(CMD.COAXSLEEPC_UNLOCKCOAXSLEEPAUDIO, data, res =>
        {

            Debug.LogError("回包数据1---》"+res.UserCoaxSleepInfo);
            Debug.LogError("回包数据2---》"+res.UserMoney);
            
            
             GetData<CoaxSleepModel>().UpdateUserInfo(res.UserCoaxSleepInfo);
             GlobalData.PlayerModel.UpdateUserMoney(res.UserMoney);
             View.UpdateItemData(GetData<CoaxSleepModel>().GetMyUserDataToPlayer(_playerPb));

        });
        
    }

  

    public void SetCurData(PlayerPB playerPb)
    {
        View.SetData(GetData<CoaxSleepModel>().GetMyUserDataToPlayer(playerPb));
        _playerPb = playerPb;
    }

   
}
