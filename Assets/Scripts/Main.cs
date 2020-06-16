using System;
using System.Text;
using Assets.Scripts.Common;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Core.Events;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module;
using Common;
using DataModel;
using GalaSDKBase;
using game.main;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Profiling;

public enum MainMenuDisplayState
{
    ShowAll,
    ShowUserInfo,
    ShowUserInfoAndTopBar,
    ShowTopBar,
    HideAll,
    ShowVisitTopBar,
    ShowRecollectionTopBar,
    ShowExchangeIntegralBar,
}

public class Main : MonoBehaviour
{
    public static Camera UiCamera;

    public static GameObject UiContainer;
    public static GameObject CommonContainer;
    public static Canvas GuideCanvas;

    public static int StageHeight = 1920;
    public static int StageWidth = 1080;


    /// <summary>
    /// 整体缩放率
    /// </summary>
    public static float ScaleFactor = 1;

    /// <summary>
    /// Canvas缩放率
    /// </summary>
    public static float CanvasScaleFactor;

    public static float ScaleX;
    public static float ScaleY;


    /// <summary>
    /// 刘海屏
    /// </summary>
    public bool IsSpecialScreen;

    private static bool _enableBackKey = true;
    private float _appPauseTime;
    private bool _isPause;
    private GameObject _backBtn;
    private Reporter _reporter;

    public static void ChangeMenu(MainMenuDisplayState state)
    {
        EventDispatcher.TriggerEvent(EventConst.MainMenuDisplayChange, state);
    }

    private void Awake()
    {
        BuglyAgent.EnableExceptionHandler();
        
        BuglyAgent.ConfigAutoReportLogLevel(LogSeverity.LogException);
        
        BuglyAgent.RegisterLogCallback(OnBuglyLogCallback);

        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        Application.targetFrameRate = 30;

        EventDispatcher.AddEventListener<bool>(EventConst.ForceToLogin, ForceToLoginView);

        Application.lowMemory += OnLowMemory;
    }

    private void OnLowMemory()
    {
        StringBuilder sb = new StringBuilder("内存不足警告==>")
            .AppendLine(" bundle 内存占用：").Append(AssetManager.Instance.GetBundleTotalMemory())
            .AppendLine(" bundle 数量：").Append(AssetManager.Instance.BundleCount)
            .AppendLine(" 当前模块：").Append(ModuleManager.Instance.CurrentModule)
            .AppendLine(" 模块数量：").Append(ModuleManager.Instance.ModuleCount)
            .AppendLine(" Mono内存：").Append(Util.GetSizeUnit(Profiler.GetMonoUsedSizeLong()));
            
        string msg = sb.ToString();
        BuglyAgent.ReportException("Application.lowMemory", msg, "none");
        Debug.LogWarning(msg);

        Resources.UnloadUnusedAssets();
    }

    private void OnBuglyLogCallback(string condition, string stacktrace, LogType type)
    {
        if (_reporter != null && type == LogType.Exception)
        {
            _reporter.forceShow = true;
        }
    }

    void Start()
    {
        GlobalData.InitData();

        AssetManager.Initialize();
        AudioManager.Initialize();

#if UNITY_ANDROID
        GalaSDKBaseCallBack.Instance.GALASDKGameExitEvent += DoExitPopup;
#endif
       
        UiCamera = GetComponent<Camera>();
        UiContainer = gameObject.transform.Find("Canvas").gameObject;
        CommonContainer = gameObject.transform.Find("CommonCanvas").gameObject;
        GuideCanvas = gameObject.transform.Find("GuideCanvas").GetComponent<Canvas>();

        I18NManager.LoadLanguageConfig((I18NManager.LanguageType) AppConfig.Instance.language);

        NetWorkManager.Instance.GlobalNetErrorHandler = NetworkErrorHandler;
        NetWorkManager.Instance.SetServer(AppConfig.Instance.logicServer);

        
        
        //屏幕适配
        Canvas canvas = UiContainer.GetComponent<Canvas>();

        ScaleX = StageWidth / (float) Screen.width;
        ScaleY = StageHeight / (float) Screen.height;
        ScaleFactor = Mathf.Min(ScaleX, ScaleY);
        ScaleFactor *= canvas.scaleFactor;
        CanvasScaleFactor = canvas.scaleFactor;

        int offY = SetOffsetOnPhone();
        ModuleManager.Instance.SetOffY(offY);

        ModuleManager.Instance.SetContainer(UiContainer);
        _backBtn = GameObject.Find("BackBtn");
        ReturnablePanel.SetBackBtn(_backBtn);
       
        
        ModuleManager.Instance.EnterModule(ModuleConfig.MODULE_UPDATE);

        ClientData.LoadErrorCode();
        
        SdkHelper.Initialize();

        //关闭多点触控
        Input.multiTouchEnabled = false;

        GameObject.Find("PromptLayer").AddComponent<FullScreenEffect>();

        HandleTest();
        
    }

    private void HandleTest()
    {
        string starWay = GalaSDKBaseFunction.GetAPPStartWay();
        if (starWay.Contains("lianouneedlog") && starWay.Contains("true"))
        {
            AppConfig.Instance.paySandbox = true;

            AppConfig.Instance.isTestMode = true;
            
            GameObject go = Instantiate(Resources.Load<GameObject>("Prefabs/Reporter"));
            _reporter = go.GetComponent<Reporter>();
        }
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        _isPause = pauseStatus;
        if (_isPause)
        {
            _appPauseTime = ClientTimer.Instance.GetCurrentTimeStamp();
        }
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        if (hasFocus && _isPause)
        {
            _isPause = false;
            if (ClientTimer.Instance.GetCurrentTimeStamp() - _appPauseTime > 10 * 60 * 1000)
            {
                ForceToLoginView();
            }
        }
    }

    
#if UNITY_ANDROID
    private void Update()
    {
        if (_enableBackKey)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                string ret = GalaSDKBaseFunction.ExitGame();
                Debug.LogError("GalaSDKBaseFunction.ExitGame:" + ret);
                if(ret == "false")
                    DoExitPopup();
            }
        }
        
#if UNITY_EDITOR
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.Return))
        {
            string url = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "/ScreenCapture.png";
            Debug.LogError("ScreenCapture->" + url);
            ScreenCapture.CaptureScreenshot(url);
        }

        if (Input.GetKey(KeyCode.Space))
        {
            NetWorkManager.CookieStr = null;
            NetWorkManager.InitHead();
        }

        if (Input.GetKeyDown(KeyCode.F9))
        {
            AssetManager.Instance.LogMessage();
        }
            
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SdkHelper.AccountAgent.OnLogout();
        }

        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.M))
        {
            AssetManager.Instance.GetBundleTotalMemory();
        }
#endif
    }

    private void DoExitPopup()
    {
        if (GuideManager.CurStage() < GuideStage.Over)
            return;
        
        _enableBackKey = false;
        PopupManager.ShowConfirmWindow(I18NManager.Get("Common_Quit")).WindowActionCallback = evt =>
        {
            _enableBackKey = true;
            if (evt == WindowEvent.Ok)
            {
                Application.Quit();
            }
        };
    }

#endif


    public static bool EnableBackKey
    {
        get { return _enableBackKey; }
        set { _enableBackKey = value; }
    }

    private int SetOffsetOnPhone()
    {
        IsSpecialScreen = false;

#if !UNITY_EDITOR
        string isban = "";
        string banheight = "";
        GalaSDKBaseFunction.GalaPhoneInfoType[] galaPhoneInfos =
        {
            GalaSDKBaseFunction.GalaPhoneInfoType.IsBand,
            GalaSDKBaseFunction.GalaPhoneInfoType.BandHeight,
        };
        string deviceInfoString = GalaSDKBaseFunction.FetchdeviceSysInfo(galaPhoneInfos);
        JSONObject deviceInfo = new JSONObject(deviceInfoString);
        isban = deviceInfo["IsBand"].str;
        banheight = deviceInfo["BandHeight"].str;

        Debug.Log("Device Info :" + isban);
        
        if (isban.Equals("true"))
        {
            IsSpecialScreen = true;
            int height = Int32.Parse(banheight);
            height = (int) (height / CanvasScaleFactor);
            return height;
        }
#endif

        Debug.Log("=======IsSpecialScreen=======" + IsSpecialScreen);
        return 0;
    }

    private void NetworkErrorHandler(HttpErrorVo vo)
    {
        int errorCode = vo.ErrorCode;
        if (errorCode != -1)
        {
            switch (errorCode)
            {
                case ErrorCode.NOT_USER_OTHER_LOGIN: //账号在异处登录
                case ErrorCode.NOT_USER_LOGIN:
                case ErrorCode.NOT_CHANNEL_LOGIN:
                case ErrorCode.BAN_CONDITION:
                    //回到登陆界面
                    ForceToLoginView();
                    break;
            }

            if (ClientData.ErrorCodeDict.ContainsKey(errorCode))
            {
                FlowText.ShowMessage(ClientData.ErrorCodeDict[errorCode]);
            }
            else
            {
                FlowText.ShowMessage(errorCode == 0
                    ? I18NManager.Get("Common_MainNetworkAbnormal")
                    : I18NManager.Get("Common_MainErrorCode", errorCode));
            }
        }
    }

   

    private void ForceToLoginView(bool isShowSwitch = false)
    {
        //这下面的执行顺序不要轻易修改
        UnityWebRequest.ClearCookieCache();
        SdkHelper.AccountAgent.IsLogin = false;
        _backBtn.Hide();
        ClientData.CustomerSelectedLevel = null;
        ClientData.CustomerSelectedCapsuleLevel = null;
        GuideManager.Reset();
        ClientTimer.Instance.Cleanup();
        NetWorkManager.CookieStr = null;
        NetWorkManager.InitHead();
        ModuleManager.Instance.DestroyAllModule();
        GlobalData.InitData();
        PopupManager.CloseAllWindow();
        SdkHelper.CustomServiceAgent.Reset();
        LoadingProgress.Instance.Show();
        
        if (isShowSwitch)
        {
            ModuleManager.Instance.EnterModule(ModuleConfig.MODULE_UPDATE, false, true, LoginCallbackType.Switch);
        }
        else
        {
            ModuleManager.Instance.EnterModule(ModuleConfig.MODULE_UPDATE);
        }
    }
}

public enum LoginCallbackType
{
    None,
    Success,
    Fail,
    Logout,
    Switch,
    UserCenter,
    RepairResource
}