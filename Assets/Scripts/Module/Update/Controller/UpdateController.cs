using System.Diagnostics;
using System.IO;
using System.Text;
using Assets.Scripts;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module;
using Assets.Scripts.Module.Download;
using Assets.Scripts.Module.NetWork;
using Com.Proto.Server;
using DataModel;
using game.main;
using GalaSDKBase;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Assets.Scripts.Framework.GalaSports.Core.Events;

public class UpdateController : Controller
{
    public UpdateView View { get; set; }

    public const string LastVersionKey = "LastVersion";

    private LoginCallbackType _isCallbackTypeSwitch = LoginCallbackType.None;
    
    private GameServerRes _gameServerRes;

    public bool _needHandleForceUpdate = false;

    public UpdateController()
    {
        int lastVersion = PlayerPrefs.GetInt(LastVersionKey, 0);

        Debug.LogError("LastVersion=>" + lastVersion + " : " + AppConfig.Instance.version);
        if (lastVersion == 0)
        {
            PlayerPrefs.SetInt(LastVersionKey, AppConfig.Instance.version);
        }
        else if (lastVersion != AppConfig.Instance.version)
        {
            //强更处理
            Stopwatch stopwatch = Stopwatch.StartNew();

            _needHandleForceUpdate = true;
            
            //版本更新处理
            if (Directory.Exists(AssetLoader.ExternalHotfixPath))
            {
                Directory.Delete(AssetLoader.ExternalHotfixPath, true);
                Debug.LogError("Delete ExternalHotfixPath");
            }
            
//            if (Directory.Exists(AssetLoader.ExternalDownloadPath))
//            {
//                Directory.Delete(AssetLoader.ExternalDownloadPath, true);
//                Debug.LogError("Delete ExternalDownloadPath");
//            }
            Debug.LogError("删除文件时间：" + stopwatch.ElapsedMilliseconds);
            
            PlayerPrefs.SetInt(UpdateController.LastVersionKey, AppConfig.Instance.version);
        }
    }

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
            case MessageConst.CMD_UPDATE_ENTER_GAME:
                EnterGame();
                break;
        }
    }

    private void EnableLua()
    {
        if (AppConfig.Instance.EnableXLua == false)
            return;
        
        ReloadBundleManifest();
        
        XLuaManager.InitXLua();
        XLuaManager.LoadMainSctipt();
        Debug.Log("<color='#00ff99'>EnableLua Success!!!</color>");
    }

    private void ReloadBundleManifest()
    {
        AssetManager.Instance.InitManifest();
    }

    public void SetData(LoginCallbackType isCallbackTypeSwich)
    {
        _isCallbackTypeSwitch = isCallbackTypeSwich;
    }

    public override void Start()
    {
        if (AppConfig.Instance.needChooseServer == false)
        {
            //测试的时候不用选服
            EnterGame();
            return;
        }

        Debug.Log("versionServer=>" + AppConfig.Instance.versionServer);

        FileMark fileMark = new FileMark(Application.persistentDataPath, FileChecker.DeleteHotfixMark);

        if (_isCallbackTypeSwitch == LoginCallbackType.RepairResource || fileMark.ReadRecord() == "true")
        {
            if (_isCallbackTypeSwitch != LoginCallbackType.RepairResource)
            {
                BuglyAgent.ReportException("UpdateController Start", "app restart for repair", "none");
            }
            
            FileChecker checker = new FileChecker();
            checker.CleanForRepair(isDelete =>
            {
                if (isDelete)
                {
                    FlowText.ShowMessage(I18NManager.Get("Update_RepairFinish"));
                    ConnetToServer();
                }
            });
        }
        else
        {
            ConnetToServer();
        }
       
    }

    private bool DownloadAppStart()
    {
        if (_needHandleForceUpdate)
        {
            CacheManager.DownloadAllBundle(tag =>
            {
                _needHandleForceUpdate = false;
                HandleVersion();
            }, ResPath.AllResources);
            return true;
        }
        
        if (CacheManager.CheckAppStartCache().needDownload)
        {
            CacheManager.DownloadAppStartCache(tag =>
            {
                LoadingProgress.Instance.Hide();
                HandleVersion();
            }, ResPath.AppStart);

            return true;
        }

        return false;
    }

    private void ConnetToServer()
    {
        //版本控制逻辑
        GameServerReq req = new GameServerReq();
        req.Version = AppConfig.Instance.version;
        req.Game = AppConfig.Instance.gameType;
        req.Channel = AppConfig.Instance.channel;
        req.ChannelInfo = AppConfig.Instance.channelInfo;

        req.Language = AppConfig.Instance.language;
        req.GameProperty = AppConfig.Instance.gameProperty; //现在测试阶段我在game_config文件里写死的是1。

        req.Idfa = GalaSDKBaseFunction.GetDeviceId();
        
        Debug.Log("DeviceId=> " + req.Idfa);
        
        var dataBytes = NetWorkManager.GetByteData(req);

        HttpMessage<GameServerRes> http = NetWorkManager.Instance.Send<GameServerRes>(CMD.GAMESERVERC_GAMESERVER,
            dataBytes, res =>
            {
                GlobalData.VersionData.ForceUpdateAddr = res.MainVersion.Addr;
                _gameServerRes = res;
                StringBuilder sb = new StringBuilder("<color='#007891'>缓存版本号：</color>\n");
                //初始化缓存数据版本号
                foreach (var v in res.CacheVersion)
                {
                    sb.AppendLine("Context:" + v.Context + "  Version:" + v.Version);

                    if (GlobalData.VersionData.VersionDic.ContainsKey(v.Context))
                    {
                        GlobalData.VersionData.VersionDic[v.Context] = v.Version.ToString();
                    }
                }
                Debug.Log(sb.ToString());

                if (res.SwitchControl == null)
                    res.SwitchControl = new SwitchControlPB();
                
                AppConfig.Instance.SwitchControl = new SwitchControlData();
                AppConfig.Instance.SwitchControl.Init(res.SwitchControl);
                
                SdkHelper.InitAccount();

                if (res.ImgServerInfo != null)
                {
                    AppConfig.Instance.assetsServer = res.ImgServerInfo.Addr + ":" + res.ImgServerInfo.Port + "/" +
                                                      res.ImgServerInfo.Folder;                  
                }

                //获取公告信息
                GlobalData.NoticeData.InitData(res.Notice);

                var serverInfoPb = NetWorkManager.Instance.SetServerDatas(res.GameServerInfo);
                AppConfig.Instance.logicServer = "http://" + serverInfoPb.Addr + ":" + serverInfoPb.Port + "/";

                NetWorkManager.Instance.SetServer(AppConfig.Instance.logicServer);

                AppConfig.Instance.serverId = serverInfoPb.ServerId;
                AppConfig.Instance.serverName = serverInfoPb.Name;

                //foreach (var serverInfoPb in res.GameServerInfo)
                //{
                //    AppConfig.Instance.logicServer = "http://" + serverInfoPb.Addr + ":" + serverInfoPb.Port + "/";

                //    NetWorkManager.Instance.SetServer(AppConfig.Instance.logicServer);

                //    AppConfig.Instance.serverId = serverInfoPb.ServerId;
                //    AppConfig.Instance.serverName = serverInfoPb.Name;
                //    break;
                //}
                EventDispatcher.TriggerEvent(Common.EventConst.OnConnetToServer);

                if (AppConfig.Instance.version < _gameServerRes.MainVersion.MinVersion)
                {
                    View.SetText("");
                    GlobalData.VersionData.ForceUpdateAddr = _gameServerRes.MainVersion.Addr;
                    SendMessage(new Message(MessageConst.CMD_UPDATE_SHOW_FORCE_UPDATE, Message.MessageReciverType.DEFAULT,
                        _gameServerRes.MainVersion.Addr));
                    return;
                }

                if(DownloadAppStart() == false)
                    HandleVersion();
            }, 
            httpError =>
            {
                View.SetText(I18NManager.Get("Update_Hint1"));
                FlowText.ShowMessage(I18NManager.Get("Update_Hint1"));
            }, false, "", false, AppConfig.Instance.versionServer);

        http.AutoRetry = true;
    }

    private void HandleVersion()
    {
        if (_gameServerRes.HotVersion != null)
        {
            AppConfig.Instance.hotVersion = _gameServerRes.HotVersion.HotVersion;
        }
        else
        {
            new FileChecker().DeleteHotfixFile();
        }

        if (AppConfig.Instance.version < _gameServerRes.MainVersion.MinVersion)
        {
            LoadingProgress.Instance.Hide();
            View.SetText("");
            GlobalData.VersionData.ForceUpdateAddr = _gameServerRes.MainVersion.Addr;
            SendMessage(new Message(MessageConst.CMD_UPDATE_SHOW_FORCE_UPDATE, Message.MessageReciverType.DEFAULT,
                _gameServerRes.MainVersion.Addr));
        }
        else if (_gameServerRes.HotVersion != null && AppConfig.Instance.version == _gameServerRes.HotVersion.Version)
        {
            LoadingProgress.Instance.Hide();
            SendMessage(new Message(MessageConst.CMD_UPDATE_LOAD_HOTFIX_CONFIG, Message.MessageReciverType.CONTROLLER,
                _gameServerRes.HotVersion));
        }
        else
        {
            EnterGame();
        }
    }

    private void EnterGame()
    {
        EnableLua();
        if (_isCallbackTypeSwitch == LoginCallbackType.Switch || _isCallbackTypeSwitch == LoginCallbackType.UserCenter)
        {
            ModuleManager.Instance.EnterModule(ModuleConfig.MODULE_LOGIN, true, false, _isCallbackTypeSwitch);
        }
        else
        {
            LoadingProgress.Instance.Hide();
            ModuleManager.Instance.EnterModule(ModuleConfig.MODULE_LOGIN);
        }
    }
}