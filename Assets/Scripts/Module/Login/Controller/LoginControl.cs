using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Core.Events;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module;
using Assets.Scripts.Module.NetWork;
using Com.Proto;
using Common;
using Componets;
using DataModel;
using Framework.GalaSports.Service;
using GalaAccount;
using GalaSDKBase;
using Module.Login.View;
using UnityEngine;

namespace game.main
{
    public class LoginControl : Controller
    {
        public LoginView view;

        LoginCallbackType _currentLoginState = LoginCallbackType.None;

        private bool _isCreateUser = false;

        private bool _isGalaAccountFinshed = false;
        private bool _isSwichClick = false;

        private float _prevClickLoginTime;

        public override void Start()
        {
            LoadingProgress.Instance.Hide();

            SdkHelper.AccountAgent.SetLoginCallback(OnAccountCenterLogin);
            
       

            if (Channel.LoginType() == GalaSDKBaseFunction.GalaSDKType.Gala)
            {
              //  Debug.LogError("-----Gala-----");
                view.ShowLoginEntrance(true);
            //    GalaAccountManager.Instance.Login(Channel.LoginType());
            }
            else if(Channel.IsTencent)
            {        
             //   Debug.LogError("-----Tencent-----1");
               // GalaAccountManager.Instance.Login(Channel.LoginType());
                
                view.ShowLoginEntrance(false);
                view.ShowTencent();                         
            }           
            else if(!Channel.IsTencent)
            {
              //  Debug.LogError("-----Other-----");
                view.ShowLoginEntrance(true);
            }
        }

        public void SetData(LoginCallbackType loginCallbackType)
        {
            _currentLoginState = loginCallbackType;
        }

        public override void OnMessage(Message message)
        {
            string name = message.Name;
            object[] body = message.Params;
            switch (name)
            {
                case MessageConst.CMD_LOGIN_LOAD_DATAFINISHED:
                    OnFinishLoginOperate();
                    break;
                case MessageConst.CMD_LOGIN_DO_LOGIN:
                    //使用渠道token登陆
//                    byte[] buffer = NetWorkManager.GetByteData(new LoginReq
//                    {
//                        Account = "340001008921",
//                        Pwd = "ad93efe5a597278d0efdf8e54e9da679",
//                        Channel = AppConfig.Instance.channel,
//                        ChannelInfo = AppConfig.Instance.channelInfo,
//                        ClientVersion = AppConfig.Instance.versionName,
//                        Language = AppConfig.Instance.language,
//                        MobileOs = Application.platform == RuntimePlatform.IPhonePlayer ? 1 : 0
//                    });
//                    NetWorkManager.Instance.Send<LoginRes>(CMD.LOGINC_LOGIN, buffer, OnAccountLogin, OnAccountLoginError);
//                    return;
                    
                    //登陆
                   
                    if (Time.realtimeSinceStartup - _prevClickLoginTime > 5)
                    {
                        _prevClickLoginTime = Time.realtimeSinceStartup;
                        Debug.LogError("Channel.IsTencent----"+Channel.IsTencent);
                        if (Channel.IsTencent)
                        {
                            //腾讯应用宝特殊处理
                            GalaSDKBaseFunction.GalaSDKType type = GalaSDKBaseFunction.GalaSDKType.Channel;
                            if (body != null && body.Length >= 1)
                            {
                                type = (GalaSDKBaseFunction.GalaSDKType) body[0];
                            }
                            GalaAccountManager.Instance.Login(type);
                        }
                        else
                        {
                            Debug.LogError("AppConfig.Instance.channelInfo:" + AppConfig.Instance.channelInfo);
                            Debug.LogError("Channel.LoginType()-----"+Channel.LoginType());
                            GalaAccountManager.Instance.Login(Channel.LoginType()); 
                        }
                    }
                    break;
                case MessageConst.CMD_LOGIN_SWITCH_LOGIN: //切换账号
                    if(GalaAccountManager.Instance.IsLogin)
                        GalaAccountManager.Instance.ShowUserCenter(Channel.LoginType());
                    else
                        GalaAccountManager.Instance.Login(Channel.LoginType());
                    break;
            }
        }

        //登录成功的设置
        private void SuccessSet()
        {
          //  Debug.LogError("登录成功");
            if (Channel.IsTencent)   //是应用宝把微信和QQ隐藏掉
            {
           //     Debug.LogError("登录成功，微信和QQ隐藏掉");
                view.HideTencent(); 
            }
         //   Debug.LogError("登录成功，打开进入游戏入口");
            view.ShowLoginEntrance(true);  //打开登录游戏入口
        }

        private void FailSet()
        {
            Debug.LogError("登录失败");
            if (Channel.IsTencent)
            {
                 Debug.LogError("登录失败，腾讯应用宝");
                 view.ShowLoginEntrance(false);
                 view.ShowTencent();
            }
            else
            { 
                Debug.LogError("登录失败，其他渠道");
                view.ShowLoginEntrance(true); 
            }
        }
        
        /// <summary>
        /// 账号中心登陆成功
        /// </summary>
        /// <param name="loginCallbackType">登陆的各种情况</param>
        private void OnAccountCenterLogin(LoginCallbackType loginCallbackType)
        {
            Debug.LogError("loginCallbackType====>"+loginCallbackType);
            switch (loginCallbackType)
            {
                case LoginCallbackType.Success:
                    Debug.LogWarning("---- --------------------------------------回调函数，登录成功 ----");
                    SuccessSet();
                
                    //第一次进入游戏是自动登录
//                    Debug.LogError("SdkHelper.AccountAgent.IsAutoLogin===>"+SdkHelper.AccountAgent.IsAutoLogin);
//                    if (SdkHelper.AccountAgent.IsAutoLogin)
//                    {
//                        if (Channel.IsTencent)
//                        {
//                            view.ShowLoginEntrance(true);
//                            view.HideTencent();
//                        }
//                        break;
//                    }

                    byte[] buffer = NetWorkManager.GetByteData(new LoginReq
                    {
                        Account = SdkHelper.AccountAgent.AccountId,
                        Pwd = SdkHelper.AccountAgent.Token,
                        Channel = AppConfig.Instance.channel,
                        ChannelInfo = AppConfig.Instance.channelInfo,
                        ClientVersion = AppConfig.Instance.versionName,
                        Language = AppConfig.Instance.language,
                        MobileOs = Application.platform == RuntimePlatform.IPhonePlayer ? 1 : 0
                    });
                    NetWorkManager.Instance.Send<LoginRes>(CMD.LOGINC_LOGIN, buffer, OnAccountLogin, OnAccountLoginError);
                    break;
                case LoginCallbackType.Fail:
                    FailSet();
                  
                    break;
                case LoginCallbackType.Logout:
                  
                    break;
                case LoginCallbackType.Switch:
                    
                    break;
                case LoginCallbackType.UserCenter:
                    
                    break;
            }

            _currentLoginState = loginCallbackType;
        }

        /// <summary>
        /// 账号在游戏服登陆成功
        /// </summary>
        /// <param name="res"></param>
        private void OnAccountLogin(LoginRes res)
        {     
            LoadingOverlay.Instance.Hide();

            GlobalData.PlayerModel.PlayerVo.IsAdult = res.IsAdult == 1;
            GlobalData.PlayerModel.PlayerVo.Addication = res.Addication == 1;

            if (res.Ret == ErrorCode.USER_NOT_CARD_CODE)
            {
                //账号未激活
                PopupManager.ShowWindow<ActiveCodeWindow>("Login/Prefabs/ActiveCodeWindow");
                EventDispatcher.RemoveEvent(EventConst.CheckActiveCode);
                EventDispatcher.AddEventListener<string>(EventConst.CheckActiveCode, OnCheckActiveCode);
                return;
            }

            if (res.Users != null && res.Users.Count > 0)
            {               
                //有游戏角色，走玩家登陆
                _isCreateUser = false;
                UserLogin(res.Users[0].UserId);
            }
            else
            {               
               GotoCreateUserModule(-1);
            }

            ClientTimer.Instance.InitTimeStamp(res.TimeStamp); //初始化时间工具
        }


        private void GotoCreateUserModule(int guideStep)
        {
            EventDispatcher.AddEventListener<string>(EventConst.CreateRoleSubmit, DoCreateRole);
            EventDispatcher.AddEventListener(EventConst.CreateUserEnd, OnCreateUserEnd);
            ModuleManager.Instance.EnterModule(ModuleConfig.MODULE_CREATE_USER, false, true, guideStep);
        }

        private void OnAccountLoginError(HttpErrorVo vo)
        {
            Debug.LogError("游戏登录失败_" + vo.ErrorCode);
            LoadingOverlay.Instance.Hide();
        }

        /// <summary>
        /// 使用激活码
        /// </summary>
        /// <param name="code"></param>
        private void OnCheckActiveCode(string code)
        {
            byte[] data = NetWorkManager.GetByteData(new ExchangeCodeReq()
            {
                Code = code
            });
            NetWorkManager.Instance.Send<ExchangeCodeRes>(CMD.USERC_ACTIVATIONACCOUNT, data, OnCheckActiveCodeSuccess);
        }

        private void OnCheckActiveCodeSuccess(ExchangeCodeRes obj)
        {
            if (obj.Ret == -1)
            {
                PopupManager.CloseLastWindow();
                Debug.Log("OnCheckActiveCodeSuccess");
            }
        }

        private void DoCreateRole(string name)
        {
            //创建角色
            byte[] buffer = NetWorkManager.GetByteData(new CreateUserReq()
            {
                Name = name
            });
            NetWorkManager.Instance.Send<CreateUserRes>(CMD.CREATE_USER, buffer, OnCreateUser, OnCreateUserFail);

            LoadingOverlay.Instance.Show();
        }

        private void OnCreateUserFail(HttpErrorVo vo)
        {
            LoadingOverlay.Instance.Hide();
            EventDispatcher.TriggerEvent(EventConst.OnCreateUser, false);
        }

        private void OnCreateUser(CreateUserRes res)
        {
            LoadingOverlay.Instance.Hide();
            GlobalData.PlayerModel.SetUser(res.User);

            _isCreateUser = true;

            LoadingProgress.Instance.Show();
            UserLogin(GlobalData.PlayerModel.PlayerVo.UserId);
            
            SdkHelper.StatisticsAgent.OnRegister();
        }


        private void UserLogin(int userId)
        {
            GlobalData.PlayerModel.PlayerVo.UserId = userId;

            SdkHelper.StatisticsAgent.OnLogin();

            byte[] buffer = NetWorkManager.GetByteData(new UserLoginReq
            {
                UserId = userId
            });
            NetWorkManager.Instance.Send<UserLoginRes>(CMD.USER_LOGIN, buffer, OnUserLogin,
                res => { LoadingProgress.Instance.Hide(); });
        }

        private void OnUserLogin(UserLoginRes res)
        {
            //登陆状态不再走登陆流程
            if (SdkHelper.AccountAgent.IsLogin)
            {
                BuglyAgent.ReportException("OnUserLogin", "SdkHelper.AccountAgent.IsLogin=true", "none");
                return;
            }

            SdkHelper.AccountAgent.IsLogin = true;
            GlobalData.PlayerModel.UserLoginRes = res;
            SendMessage(new Message(MessageConst.CMD_LOGIN_LOAD_DATA));
        }


        private void OnFinishLoginOperate()
        {
            if (_isCreateUser)
            {
                //创建完角色显示视频
                EventDispatcher.TriggerEvent(EventConst.OnCreateUser, true);
                SdkHelper.SetSdkData(SdkHelper.TYPE_CREATE_ROLE);
                return;
            }

            if (GuideManager.CurStage() == GuideStage.MainLine1_1Level_1_3Level_Stage)
            {
                ModuleManager.Instance.OpenCommonModule(ModuleConfig.MODULE_GAME_MAIN, Main.CommonContainer);
                ModuleManager.Instance.EnterModule(ModuleConfig.MODULE_MAIN_LINE);
            }
            else
            {
                EnterMain();
            }
            
            doSdkWork(); //sdk相关
        }

        private void EnterMain()
        {
            ModuleManager.Instance.OpenCommonModule(ModuleConfig.MODULE_GAME_MAIN, Main.CommonContainer);
            ModuleManager.Instance.Remove(ModuleConfig.MODULE_LOGIN);
        }

        private void OnCreateUserEnd()
        {
            _isCreateUser = false;
            ModuleManager.Instance.Remove(ModuleConfig.MODULE_LOGIN);
            OnFinishLoginOperate();
        }

        private void doSdkWork()
        {
            SdkHelper.SetSdkData();
        }

        public override void Destroy()
        {
            base.Destroy();
            EventDispatcher.RemoveEvent(EventConst.CreateRoleSubmit);
            EventDispatcher.RemoveEvent(EventConst.CreateUserEnd);
            EventDispatcher.RemoveEvent(EventConst.CheckActiveCode);
        }
    }
}