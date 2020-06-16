using System.Collections.Generic;
using System.IO;
using System.Text;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Module.Download;
using Com.Proto.Server;
using DataModel;
using Framework.Utils;
using game.main;
using UnityEngine;

public class HotfixController : Controller
{
    private HotVersionPB _hotVersionPb;

    private string _hotfixConfigPath;


    public UpdateView View;

    private HotfixIndexFile indexFile;
    private bool _isRetry = true;
    private bool _isStartDownloadHotfixFile = false;

    //private const int ConfirmSize = 1024 * 1024 * 2;
    private const int ConfirmSize =0;
    /// <summary>
    /// 处理View消息
    /// </summary>
    /// <param name="message"></param>
    public override void OnMessage(Message message)
    {
        string name = message.Name;
        object[] body = message.Params;
        switch (name)
        {
            case MessageConst.CMD_UPDATE_LOAD_HOTFIX_CONFIG:
                HotVersionPB pb = (HotVersionPB) body[0];
                InitCofig(pb);
                LoadConfig();
                if (pb.HotVersion > 0)
                    View.ShowAnnouncement(GlobalData.NoticeData.GetHotfixNotice().Content);

                break;
            case MessageConst.CMD_UPDATE_RETRY:
                _isRetry = true;
                DownloadManager.Clear();
                ClientTimer.Instance.DelayCall(LoadConfig, 0.01f);
                break;
            case MessageConst.CMD_UPDATE_START_DOWNLOAD_HOTFIX_FILE:
                LoadHotfixFiles(CheckHotfixFile());
                break;
        }
    }

    private void InitCofig(HotVersionPB hotVersionPb)
    {
        _hotVersionPb = hotVersionPb;
        _hotfixConfigPath = AssetLoader.ExternalHotfixPath + "/HotfixConfig_v" + hotVersionPb.Version + "_h" +
                            _hotVersionPb.HotVersion + ".zip";
    }

    private void OnQueueError(List<DownloadItem> items)
    {
        foreach (var item in items)
        {
            Debug.Log("热更文件下载失败：" + item.LocalPath + " **** Url->" + item.Url + "\n" + item.ErrorText);
        }

        View.ShowRetry(I18NManager.Get("Update_DownloadHotfixFail", items.Count));
        View.SetText("");
    }

    /// <summary>
    /// 下载热更索引文件
    /// </summary>
    private void LoadConfig()
    {
        if (_hotVersionPb.HotVersion == 0)
        {
            new FileChecker().DeleteHotfixFile();
            EnterGame();
            return;
        }

        DownloadManager.LoaderQueueComplete = OnQueueEnd;
        DownloadManager.LoaderQueueError = OnQueueError;

        if (File.Exists(_hotfixConfigPath) && CheckIndexFile())
        {
            //热更配置文件不存在
            List<IDownloadItem> files = CheckHotfixFile();
            if (files == null || files.Count == 0)
                EnterGame();
            else
            {
                if (_isRetry || indexFile.FileTotalSize < ConfirmSize)
                {
                    LoadHotfixFiles(files);
                }
                else
                {
                    View.ShowUpdateBtn(true);
                }
            }
        }
        else
        {
            View.SetText(I18NManager.Get("Update_WaitDownload"));
            DownloadManager.Load(
                _hotVersionPb.Addr + "hotfix_" + _hotVersionPb.Version + "/v" + _hotVersionPb.HotVersion +
                "/HotfixConfig_v" + _hotVersionPb.HotVersion + ".zip",
                _hotfixConfigPath, OnHotfixConfigLoaded, OnLoadHotfixConfigError, OnLoadHotfixConfigProgress);
        }
    }

    private void OnQueueEnd()
    {
        View.SetText("下载完成");
        ClientTimer.Instance.DelayCall(() =>
        {
            List<IDownloadItem> list = CheckHotfixFile();
            if (list == null || list.Count == 0)
            {
                EnterGame();
            }
            else
            {
                _isStartDownloadHotfixFile = false;
                LoadHotfixFiles(list);
            }
        }, 0.05f);
    }

    private void UpdateRecord()
    {
        FileStream fileStream =
            new FileStream(FileChecker.HotfixRecordPath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None);
        string str = _hotVersionPb.Version + "_" + _hotVersionPb.HotVersion;
        byte[] bytes = Encoding.UTF8.GetBytes(str);
        fileStream.Write(bytes, 0, bytes.Length);
        fileStream.Close();
    }

    private void OnLoadHotfixConfigProgress(float progress)
    {
        int per = (int)progress * 100;
        View.SetProgress((int) (progress * 100));  
    }

    private void OnLoadHotfixConfigError(DownloadItem item)
    {
        Debug.LogError("OnLoadHotfixConfigError==>" + item.ErrorText + " Url->" + item.Url);
        FlowText.ShowMessage(item.ErrorText);
    }

    /// <summary>
    /// 校验索引文件md5
    /// </summary>
    /// <returns></returns>
    private bool CheckIndexFile()
    {
        string targetMd5 = MD5Util.Get(_hotfixConfigPath);
        return _hotVersionPb.Md5 == targetMd5;
    }

    private void EnterGame()
    {
        if (_hotVersionPb.HotVersion > 0)
        {
            I18NManager.LoadLanguageConfig((I18NManager.LanguageType) AppConfig.Instance.language);
            //标记这个版本的更新完成
            UpdateRecord();
        }

        SendMessage(new Message(MessageConst.CMD_UPDATE_ENTER_GAME));
    }

    /// <summary>
    /// 检查文件数量和记录的版本号
    /// </summary>
    /// <returns></returns>
    private List<IDownloadItem> CheckHotfixFile()
    {
        View.SetText(I18NManager.Get("Update_CheckFile"));

        int loadedFileCount = 0;
        DirectoryInfo directoryInfo = new DirectoryInfo(AssetLoader.ExternalHotfixPath);
        DirectoryInfo[] dirs = directoryInfo.GetDirectories();
        foreach (var dir in dirs)
        {
            loadedFileCount += dir.GetFiles("*", SearchOption.AllDirectories).Length;
        }

        indexFile = GetIndexFile();

        if (loadedFileCount >= indexFile.FileItems.Count && FileUtil.ReadFileText(FileChecker.HotfixRecordPath) ==
            _hotVersionPb.Version + "_" + _hotVersionPb.HotVersion)
        {
            //检查文件数量 核对记录文件的版本号
            return null;
        }

        View.SetText("");
        FileChecker checker = new FileChecker();
        return checker.GetReloadHotfixFiles(indexFile);
    }

    private void OnHotfixConfigLoaded(DownloadItem downloadItem)
    {
        if (CheckIndexFile() == false)
        {
            View.SetText(I18NManager.Get("Update_IndexFileError"));
        }
        else
        {
            if (GetIndexFile().FileTotalSize < ConfirmSize)
            {
                LoadHotfixFiles(CheckHotfixFile());
            }
            else
            {
                View.ShowUpdateBtn(true);
            }
        }
    }

    private HotfixIndexFile GetIndexFile()
    {
        byte[] bytes = FileUtil.ReadBytesFile(_hotfixConfigPath);
        indexFile = HotfixIndexFile.Deserialize(bytes);
        return indexFile;
    }


    private void LoadHotfixFiles(List<IDownloadItem> files)
    {
        if (_isStartDownloadHotfixFile || files == null || files.Count == 0)
            EnterGame();
        _isStartDownloadHotfixFile = true;
        View.SetText(I18NManager.Get("Update_FileDownloading"));
        DownloadManager.AddList(files);
        DownloadManager.SetQueueCallback(        
            (per) => {
                int iPer = (int)per * 100;
                View.SetText(I18NManager.Get("Update_FileDownloading") + iPer.ToString() + "%");
                OnLoadHotfixConfigProgress(per);
        }, null);
        DownloadManager.StartQueue(_hotVersionPb.Addr + "hotfix_" +
                                   _hotVersionPb.Version + "/v" +
                                   _hotVersionPb.HotVersion + "/");
    }

    public override void Destroy()
    {
        base.Destroy();
        DownloadManager.Clear();
    }
}