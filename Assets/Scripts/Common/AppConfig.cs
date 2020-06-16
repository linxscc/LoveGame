using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Com.Proto.Server;
using DataModel;
using game.main;
using UnityEngine;
using GalaSDKBase;

public class AppConfig
{
    public string channelInfo;
    public string channel;
    public int gameType = 3;
    public int gameProperty;      //游戏属性

    /// <summary>
    /// 防沉迷URL
    /// </summary>
    public string wallowServer;
    /// <summary>
    /// 服务器URL
    /// </summary>
    public string logicServer;
    /// <summary>
    /// 官方账号URL
    /// </summary>
    public string accountServer;
    /// <summary>
    /// 官方客服URL
    /// </summary>
    public string customerServiceServer;
    /// <summary>
    /// 资源URL
    /// </summary>
    public string assetsServer;

    /// <summary>
    /// 选服服务器
    /// </summary>
    public string versionServer;

    /// <summary>
    /// 本地缓存版本号
    /// </summary>
    public string cacheVersion;
    
    /// <summary>
    /// 多语言ID
    /// </summary>
    public int language;
    /// <summary>
    /// 版本号
    /// </summary>
    public int version;


    /// <summary>
    /// 包参数版本   一般安卓是100，苹果是101
    /// </summary>
    public string markVersion;
    
    /// <summary>
    /// 是否需要启动选服服务器
    /// </summary>
    public bool needChooseServer;

    /// <summary>
    /// 是否需要显示平台的登陆
    /// </summary>
    public bool needShowChannelLogin;

    /// <summary>
    /// 是否需要开启新手引导
    /// </summary>
    public bool needGameGuide;

    /// <summary>
    /// 服务器名称
    /// </summary>
    public string serverName;

    /// <summary>
    /// 服务器ID
    /// </summary>
    public string serverId = "test_server";

    public SwitchControlData SwitchControl;

    /// <summary>
    /// 是否开启实名验证
    /// </summary>
//    public bool NeedRealName;

//    public bool AntiAddiction;


    //adjust事件，新手引导和第二天签到
    public string adjustToturail;

    public string adjustSecondDay;

    /// <summary>
    /// 支付对应的大区
    /// </summary>
    public string payChannel;
    
    public string isChinese;
    
    public bool paySandbox = false;

    public int hotVersion = 0;


    public string versionName
    {
        get
        {
            int v1 = version / 1000000;
            int v2 = version % 1000000 / 1000;
            int v3 = version % 1000;

            return v1 + "." + v2 + "." + v3;
        }
    }

    /// <summary>
    /// 是否越狱渠道
    /// </summary>
    public bool isJailbreak = false;

    private static AppConfig _instance;


    public static AppConfig Instance {
        get
        {
            if (_instance == null)
            {
                _instance = new AppConfig();
                _instance.Init();
            }
            return _instance;
        }
    }

    public bool EnableXLua = false;
    
    /// <summary>
    /// 商品和计费点标示
    /// </summary>
    public string payKey;

    /// <summary>
    /// lianouneedlog://true 测试模式
    /// </summary>
    public bool isTestMode = false;


    /// <summary>
    /// 是否开放用户中心（公司自己的）
    /// </summary>
    public bool UseGalaLogin = false;
    
    private void Init()
    {     
        var path = string.Empty;
        string pathAssetName = "game_config.properties";
        var str = string.Empty;
        if (Application.platform == RuntimePlatform.Android)//安卓
        {
            if(string.IsNullOrEmpty(str) || str.Equals("false"))//判断有没有sdk
            {
                path = Path.Combine("jar:file://" + Application.dataPath + "!/assets/", pathAssetName);
                WWW loadAsset = new WWW(path);
                while (!loadAsset.isDone) { }
                str = Encoding.UTF8.GetString(loadAsset.bytes);
            }
        }
        else if (Application.platform == RuntimePlatform.IPhonePlayer)//苹果
        {
            path = Path.Combine(Path.Combine(Application.dataPath, "Raw"), pathAssetName);
            str = Encoding.UTF8.GetString(File.ReadAllBytes(path));
        }
        else//编辑器
        {
            path = Path.Combine(Application.streamingAssetsPath, pathAssetName);
            str = File.ReadAllText(path);
        }

        ParseConfig(str);
        
        cacheVersion = new AssetLoader().LoadTextSync(AssetLoader.GetCacheVersionPath()).Trim();
    }

    private void ParseConfig(string appData)
    {
        int index = appData.IndexOf("###", StringComparison.Ordinal);
        if(index != -1)
            appData = appData.Substring(0, index);
        appData = appData.Replace("\r", "");
        string[] baseList = appData.Split(Convert.ToChar("\n"));
        Dictionary<string, string> dict = new Dictionary<string, string>();
        for (int i = 0; i < baseList.Length; i++)
        {
            string str = baseList[i];
            string temp = str.Trim();
            if (str.StartsWith("#"))
                continue;
            if (temp.Length == 0)
                continue;

            string[] arr = str.Split(Convert.ToChar("="));
            if (arr.Length < 2)
            {
                //ULog.W("AppConfig ERROR-> " + str);
                continue;
            }

            dict.Add(arr[0], arr[1]);
        }

        channelInfo = dict["channelInfo"];

        channel = dict["channel"];
        
        if (dict.ContainsKey("logicServer"))
        {
            logicServer = dict["logicServer"];
        }
        
        accountServer = dict["accountServer"];
        wallowServer = dict["wallowServer"];
        GalaAccountManager.Instance.SetGalaLoginServiceUrlAndWallowUrl(accountServer,wallowServer);

        if (dict.ContainsKey("customerServiceServer"))
        {
            customerServiceServer = dict["customerServiceServer"];
        }

        versionServer = dict["versionServer"];
        markVersion = dict["markVersion"];
        if (dict.ContainsKey("assetsServer"))
        {
            assetsServer = dict["assetsServer"];
        }

        gameProperty = int.Parse(dict["gameProperty"]);
        
        if (dict.ContainsKey("gameType"))
        {
            gameType= int.Parse(dict["gameType"]);
        }

        if (dict.ContainsKey("serverId"))
        {
            serverId = dict["serverId"];
        }

        if (dict.ContainsKey("isChinese"))
        {
            isChinese = dict["isChinese"];
        }
        
        if (dict.ContainsKey("payChannel"))
        {
            payChannel = dict["payChannel"];
        }
        
        language = int.Parse(dict["language"]);
        
        needChooseServer = bool.Parse(dict["needChooseServer"]);
        needShowChannelLogin = bool.Parse(dict["needShowChannelLogin"]);
        needGameGuide = bool.Parse(dict["needGameGuide"]);

		if (dict.ContainsKey ("adjustSecondDay")) 
		{
			adjustSecondDay = dict["adjustSecondDay"];
		}

		if (dict.ContainsKey ("adjustToturail ")) 
		{
			adjustToturail = dict["adjustToturail"];
		}

        if (dict.ContainsKey("version"))
        {
            version = int.Parse(dict["version"]);
            Debug.Log("Config version======>" + version);
        }
        else
        {
            GalaSDKBaseFunction.GalaPhoneInfoType[] galaPhoneInfos = {
                GalaSDKBaseFunction.GalaPhoneInfoType.GameVersion,
            };
            string deviceInfoString = GalaSDKBaseFunction.FetchdeviceSysInfo(galaPhoneInfos);
            JSONObject deviceInfo = new JSONObject(deviceInfoString);
            string GameVersion = deviceInfo["GameVersion"].str;
            version = int.Parse(GameVersion);
            Debug.Log("Native version======>" + version);
        }

        if (dict.ContainsKey("isJailbreak"))
        {
            isJailbreak = bool.Parse(dict["isJailbreak"]);
        }

        if(dict.ContainsKey("EnableXLua"))
        {
            EnableXLua = bool.Parse(dict["EnableXLua"]);
        }
        
        if(dict.ContainsKey("payKey"))
        {
            payKey = dict["payKey"];
        }


        if (dict.ContainsKey("useGalaLogin"))
        {            
            UseGalaLogin = bool.Parse(dict["useGalaLogin"]);
        }
    }
}