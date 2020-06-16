using System;
using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Core.Events;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module;
using Assets.Scripts.Module.Download;
using Assets.Scripts.Module.Framework.Utils;
using Assets.Scripts.Module.NetWork;
using Assets.Scripts.Services;
using Com.Proto;
using Common;
using DataModel;
using GalaAccount;
using Google.Protobuf.Collections;
using UnityEngine;
using Utils;

namespace game.main
{
    public class MainPanelController : Controller
    {
        public MainPanelView view;
        private PopupWindow _window;
        private StoreScoreWindow _storeScoreWindow;
        private AwardWindow _awardWindow;
        private CacheVo _cacheVo;
        private bool hasComment = false;
        
       // private BindIdCardWindow _bindIdCardWindow;
     //   private ConfirmBindIdCardWindow _confirmBindIdCardWindow;
//        private bool online1h = false;
//        private bool online2h = false;
//        private bool online3h = false;
        private TimerHandler _checkAntiCountDown;
        private TimerHandler _dailyRefreshtimer;
        private string recordTime = "";
        private int _recordhour = 0;
        private TimerHandler _addictionTimerHandler;
        public override void Init()
        {         
            base.Init();
            EventDispatcher.AddEventListener<MapField<int,int>>(EventConst.ChangeRole, OnChangeRole);       
        }

        public override void Start()
        {
           
            view.ChangeBackground();

            UpdateTopBar();
            EventDispatcher.AddEventListener(EventConst.CreateActivityContenet, CreateActivityContenet);
            EventDispatcher.AddEventListener(EventConst.UpdateUserMoney, UpdateTopBar);
            EventDispatcher.AddEventListener(EventConst.UpdateEnergy, UpdateTopBar);
            EventDispatcher.AddEventListener<RepeatedField<UserBuyRmbMallPB>>(EventConst.GetPayInfoSuccess, arr=>
            {
                UpdateTopBar();
            });
            
            EventDispatcher.AddEventListener(EventConst.UserLevelUp, OnUserLevelup);
            EventDispatcher.AddEventListener(EventConst.MainLineLevelUpdate, view.HandleFunctionOpen);

            EventDispatcher.AddEventListener(EventConst.OnDataLoadComplete, OnDataLoadComplete);
            EventDispatcher.AddEventListener<int>(EventConst.ActivitySignInClick, OnActivitySignInClick);

            EventDispatcher.AddEventListener<int>(EventConst.LoveDiaryEditSaveAndGoBackMainModule, OnLoveDiaryEditSaveAndGoBackMainModule);
            EventDispatcher.AddEventListener<bool>(EventConst.ChangeTopPower, SetPowerState);
          
            EventDispatcher.AddEventListener<PlayerVo>(EventConst.UpDataUserName, UpDataUserName);

            EventDispatcher.AddEventListener<bool>(EventConst.CloseFirstRechargeBtn, CloseFirstRechargeBtn);

            EventDispatcher.AddEventListener(EventConst.RefreshActivityImageAndActivityPage, RefreshActivityImageAndActivityPage);
            EventDispatcher.AddEventListener(EventConst.RefreshPoint,SendRedPoint);

            EventDispatcher.AddEventListener(EventConst.UpdateExchangeIntegral,UpdateExchangeIntegral);
            EventDispatcher.AddEventListener(EventConst.ShowStoreScore,OnScoreStore);
           
            EventDispatcher.AddEventListener(EventConst.OnTriggerGiftChange,OnTriggerGiftChange);
            
            EventDispatcher.AddEventListener<RepeatedField<long>>(EventConst.TriggerGiftPaySuccess, OnGiftChange);
           // EventDispatcher.AddEventListener(EventConst.ShowConfirmBind,OpenConfirmWindow);
            EventDispatcher.AddEventListener(EventConst.SettingUserInfoUpdate,SetIsAddictionTime);
            EventDispatcher.AddEventListener(EventConst.GetRealNameAward,GetRealNameAward);
            EventDispatcher.AddEventListener(EventConst.UpdateMainViewHeadInfo,UpdateMainViewHeadInfo);
            InitMainLive2d();

            ShowWindow();

            _lastDate = new DateTime(ClientTimer.Instance.GetCurrentTimeStamp());
            _dailyRefreshtimer=ClientTimer.Instance.AddCountDown("DailyRefresh", long.MaxValue, 5, DailyRefresh, null);
           // CheckAddicationTime();

            OnTriggerGiftChange();
            SetGameLoginHasChange();
            SetIsAddictionTime();
        }

        private void UpdateMainViewHeadInfo()
        {
            view. SetHeadImg();
        }

     
        private void GetRealNameAward()
        {
            
           //本地校验下
           int awardID  =(int) OptionalActivityTypePB.OptionalVerified;
           var awardList = GlobalData.PlayerModel.PlayerVo.ExtraAwardInfo;
           GlobalData.PlayerModel.PlayerVo.Addication = true;
           bool isGet = awardList.Contains(awardID);
           if (isGet)
               return;
           
           VerifiedInfoPB verifiedInfoPb =new VerifiedInfoPB
           {
               Account = SdkHelper.AccountAgent.AccountId,
               Pwd = SdkHelper.AccountAgent.Token,
           };
           ReceiveUserExtraAwardsReq req =new ReceiveUserExtraAwardsReq
           {
               OptionalActivityType = OptionalActivityTypePB.OptionalVerified,
               Verified = verifiedInfoPb,
           };         
           byte[] data = NetWorkManager.GetByteData(req);
           NetWorkManager.Instance.Send<ReceiveUserExtraAwardsRes>(CMD.USERC_RECEIVEUSEREXTRAAWARDS, data, res =>
           {             
               Debug.LogError(res.UserExtraInfo);
               GlobalData.PlayerModel.UpdataUserExtra(res.UserExtraInfo);
               ManualSetMailRedPoint();
           });

        }

        private void ManualSetMailRedPoint()
        {
            view.SetRedPoint( Constants.MAIL_MSG_KEY);  //不想在拉一次红点请求，前端手动传邮件红点显示  
        }

        private void SetGameLoginHasChange()
        {
            GalaAccountManager.Instance.GameLoginHasChange(true,AppConfig.Instance.serverId,GlobalData.PlayerModel.PlayerVo.UserId.ToString()); 
        }

       
        /// <summary>
        /// 是否显示防沉迷计时
        /// </summary>
        private void SetIsAddictionTime()
        {
            var addictionTime=  GalaAccountManager.Instance.FetchUserCanPlayTime();//还剩多少秒
            var isStar = addictionTime >= 0;
            
            if (isStar)
            {
                addictionTime = addictionTime * 1000+ClientTimer.Instance.GetCurrentTimeStamp();
                  Debug.LogError("--------------"+addictionTime);
                  
                if (_addictionTimerHandler==null)
                {
                    _addictionTimerHandler = ClientTimer.Instance.AddCountDown("CountDown",Int64.MaxValue, 1f, (
                        delegate(int i)
                        {
                            string timeStr = "";
                            var curTimeStamp = ClientTimer.Instance.GetCurrentTimeStamp();
                            long time = addictionTime - curTimeStamp;
                            if (time<0)
                            {
                                 ClientTimer.Instance.RemoveCountDown(_addictionTimerHandler);
                                 return;
                            }
                            else
                            {
                                long s = (time / 1000) % 60;
                                long m = (time / (60 * 1000)) % 60;
                                long h = time / (60 * 60 * 1000);
                                timeStr = $"{h:D2}:{m:D2}:{s:D2}";
                            }
                            
                            view.SetCountDownTime(true, timeStr);

                        } ), null);
                } 
            }
            else
            {
                if (_addictionTimerHandler!=null)       
                     ClientTimer.Instance.RemoveCountDown(_addictionTimerHandler);

                view.SetCountDownTime(false, "");
                //隐藏防沉迷计时Image
                

            }
        }
        
        
        

//        private void CheckAddicationTime()
//        {
//            Debug.Log("IsAdult"+GlobalData.PlayerModel.PlayerVo.Addication);
//            Debug.Log("Addication"+GlobalData.PlayerModel.PlayerVo.IsAdult);
//            Debug.Log("AppConfig.Instance.NeedRealName:"+AppConfig.Instance.SwitchControl.NeedRealName);
//            if ((!GlobalData.PlayerModel.PlayerVo.Addication||!GlobalData.PlayerModel.PlayerVo.IsAdult)&&AppConfig.Instance.SwitchControl.AntiAddiction)
//            {
//                //Debug.LogError(GlobalData.PlayerModel.PlayerVo.IsAdult);
//                AppConfig.Instance.SwitchControl.NeedRealName = true;
//                //这里要开启实名认证！！！
//                Dictionary<string, string> settingInfo = new Dictionary<string, string>();
//                settingInfo.Add("needRealName", "true"); //实名认证,"true"打开,"false"关闭,
//                GalaAccountManager.Instance.UpdateSetting(settingInfo); 
//                CheckAntiAddition(0);
//                _checkAntiCountDown=ClientTimer.Instance.AddCountDown("CheckAntiAddition", Int64.MaxValue, GetRefreshTime(), CheckAntiAddition, null);
//            }
//
//        }
        
//        private float GetRefreshTime()
//        {
//            return 600f;
//        }
//        
//        private void UpdateUserInfo()
//        {
//            Debug.Log("userdata has update!");
//            GlobalData.PlayerModel.PlayerVo.Addication = true;
//            GlobalData.PlayerModel.PlayerVo.IsAdult = true;
//            FlowText.ShowMessage("身份认证成功！");
//            
//            _confirmBindIdCardWindow?.Close();
//            ClientTimer.Instance.RemoveCountDown(_checkAntiCountDown);
//        }

//        private void OpenConfirmWindow()
//        {
//            //调用
//            Debug.LogError("Call bindId here!");
//           
//         //   GalaAccountManager.Instance.ShowRealName(); 
//         
//        }

//        private void CheckAntiAddition(int obj)
//        {
//            NetWorkManager.Instance.Send<AntiAddictionRes>(CMD.USERC_ANTIADDICTION, null, GetOnlineTime);
//        }

//        private void GetOnlineTime(AntiAddictionRes res)
//        {
//            Debug.Log("res.UserAntiAddiction.OnlineTime"+DateUtil.GetTimeFormat4(res.UserAntiAddiction.OnlineTime));
//            //SetStartOnlineTime(res.UserAntiAddiction.OnlineTime);
//            int hour = NeedToTipslongTime(res.UserAntiAddiction.OnlineTime);
//            
//            
//            if (hour!=0&&_recordhour!=hour&&!GlobalData.PlayerModel.PlayerVo.Addication)
//            {
//                if (_confirmBindIdCardWindow == null&&_bindIdCardWindow==null)
//                {
//                    _confirmBindIdCardWindow =
//                        PopupManager.ShowWindow<ConfirmBindIdCardWindow>(
//                            "GameMain/Prefabs/BindCardId/ConfirmBindIdCard");
//                    string tips = "";
//                    bool enableOk = true;
//                    bool needtoquit = false;
//                    //DateTime dt = DateUtil.GetDataTime(res.UserAntiAddiction.LoginTime + 1000 * 60 * 60 * 5);
//                    _recordhour = hour;
//                    switch (hour)
//                    {
//                        case 1:
//                            tips = "您累计在线时间已满<color='#DC88A7'>1</color>小时，累计在线<color='#DC88A7'>3</color>小时后，收益将<color='#DC88A7'>-50%</color>。（满18岁的玩家，进行实名验证后，可维持满收益）";//"您累计在线游戏时间已满<color='#DC88A7'>1</color>小时，请您适当游戏，注意休息。";
//                            break;
//                        case 2:
//                            tips ="您累计在线时间已满<color='#DC88A7'>2</color>小时，累计在线<color='#DC88A7'>3</color>小时后，收益将<color='#DC88A7'>-50%</color>。（满18岁的玩家，进行实名验证后，可维持满收益）";//"您累计在线游戏时间已满<color='#DC88A7'>2</color>小时，请您适当游戏，注意休息。";
//                            break;
//                        case 3:
//                            tips = "您累计在线游戏时间已满<color='#DC88A7'>3</color>小时，当前收益<color='#DC88A7'>-50%</color>,累计在线<color='#DC88A7'>4</color>小时后，收益将<color='#DC88A7'>-70%</color>。（满18岁的玩家，进行实名验证后，可维持满收益）";//{dt.Hour}:{dt.Minute}分后再上线吧
//                            enableOk = !GlobalData.PlayerModel.PlayerVo.IsAdult;
//                            needtoquit = true;
//                            break;
//                        case 4:
//                            tips = "您累计在线游戏时间已满<color='#DC88A7'>4</color>小时，当前收益<color='#DC88A7'>-70%</color>,累计在线<color='#DC88A7'>5</color>小时后，收益将<color='#DC88A7'>-100%</color>。（满18岁的玩家，进行实名验证后，可维持满收益）";//"您累计在线时间已满<color='#DC88A7'>4</color>小时，当前收益<color='#DC88A7'>-70%</color>，累计在线<color='#DC88A7'>5/color>小时后，收益将<color='#DC88A7'>-100%</color>。";
//                            enableOk = !GlobalData.PlayerModel.PlayerVo.IsAdult;
//                            needtoquit = true;
//                            break;
//                        case 5:
//                            tips = "您累计在线时间已满<color='#DC88A7'>5</color>小时，当前收益<color='#DC88A7'>-100%</color>，请您下线休息，做适当身体活动。（满18岁的玩家，进行实名验证后，可维持满收益）";
//                            enableOk = !GlobalData.PlayerModel.PlayerVo.IsAdult;
//                            needtoquit = true;
//                            break;
//                        default:
//                            tips = "您累计在线时间已满<color='#DC88A7'>5</color>小时，当前收益<color='#DC88A7'>-100%</color>，请您下线休息，做适当身体活动。（满18岁的玩家，进行实名验证后，可维持满收益）";
//                            enableOk =!GlobalData.PlayerModel.PlayerVo.IsAdult;;
//                            needtoquit = true;
//                            break;
//                    }
//
//                    recordTime = tips;
//                    _confirmBindIdCardWindow.SetData(tips,enableOk,needtoquit);
//                    
//                    
//                }  
//                
//            }
//
//        }
        

//        private int NeedToTipslongTime(long time)
//        {
//            return (int)(time / (1000 * 60 * 60));
//        }
        

        private void DailyRefresh(int obj)
        {
            DateTime dateTime = DateUtil.GetDataTime(ClientTimer.Instance.GetCurrentTimeStamp());

            if (_lastDate.Hour < 6 && dateTime.Hour == 6 && dateTime.Millisecond > 0)
            {
                EventDispatcher.TriggerEvent(EventConst.DailyRefresh6);
                Debug.LogError("===DailyRefresh===6点刷新");
                
                UpdateTopBar();
                SixRefreshMainPanelActivityBar();
            }
            _lastDate = dateTime;
        }


        /// <summary>
        /// 六点刷新主界面活动Bar
        /// </summary>
        private void SixRefreshMainPanelActivityBar()
        {
            view.RefreshActivityImageAndActivityPage();
        }
            

        private void OnGiftChange(RepeatedField<long> arr)
        {
            GlobalData.RandomEventModel.Delete(arr);
        }
        

        private void ShowWindow()
        {

            if (GuideManager.IsPass4_12()&& GuideManager.CurFunctionGuide(GuideTypePB.LoveGuideCoaxSleep)== FunctionGuideStage.Function_CoaxSleep_OneStage)
            {
                return;
            }
            
            if (GuideManager.CurStage()== GuideStage.Over)              
            {
                _cacheVo=CacheManager.CheckExtendCache();
                if (!_cacheVo.needDownload)
                {
                    ShowPopupWindow();  
                } 
            }
            
        }

        private void OnScoreStore()
        {
            //Debug.LogError("屏蔽商店评分");
//            if (hasComment)
//            {
//                return;
//            }
//
//
//            hasComment = true;
//            Debug.LogError("Goto comment");
//            NetWorkManager.Instance.Send<StoreEvaluationRes>(CMD.STORESCORE_CANCOMMENT, null, canComment =>
//            {
//                if (canComment.Comment)
//                {
//                    if (_storeScoreWindow == null)
//                    {
//                        _storeScoreWindow =
//                            PopupManager.ShowWindow<StoreScoreWindow>("GameMain/Prefabs/StoreScoreWindow/StoreScoreWindow",
//                                Container);
//                    }
//                }
//
//            });
        }

        private void OnTriggerGiftChange()
        {
            List<TriggerGiftVo> giftList = GlobalData.RandomEventModel.GiftList;
            if (giftList == null || giftList.Count == 0)
            {
                view.ChangeTriggerGift(false);
            }
            else
            {
                view.ChangeTriggerGift(true, GlobalData.RandomEventModel.GiftCount, giftList[0].MaturityTime);
            }
        }

        private void ShowPopupWindow()
        {
            if (GetData<ActivityPopupWindowModel>().GetDate().Count!=0)
            {
                                
                if (_window==null&&GetData<ActivityPopupWindowModel>().IsShow())
                {
                    _window = PopupManager.ShowWindow<PopupWindow>("GameMain/Prefabs/PropWindow",Container);
                    _window .SetData(GetData<ActivityPopupWindowModel>());
                }  
            }
                     
        }
        
        private void UpdateExchangeIntegral()
        {
            view.UpdateExchangeIntegral(GlobalData.TrainingRoomModel.GetCurIntegral());
        }

        private void UpDataUserName(PlayerVo vo)
        {
            view.SetData(vo);
        }


        private void CloseFirstRechargeBtn(bool isShow)
        {
            view.IsShowFirstRechargeBtn(isShow);
        }

        private void CreateActivityContenet()
        {
           view.IsShowFirstRechargeBtn(GlobalData.ActivityModel.IsShowFirstRechargeBtn());
           //view.CreateActivityImageAndActivityPage(GlobalData.ActivityModel.GetActivityList());
           view.CreateActivityImageAndActivityPage(GlobalData.ActivityModel. GetActivityVoList());
        }

        private void RefreshActivityImageAndActivityPage()
        {
            view.RefreshActivityImageAndActivityPage();
        }


        public void StopPlayingDubbing()
        {
            if (AudioManager.Instance.IsPlayingDubbing)
            {
                Debug.Log(" OnHide  AudioManager.Instance.StopDubbing();");
                AudioManager.Instance.StopDubbing();
            }
        }

      

        private void OnLoveDiaryEditSaveAndGoBackMainModule(int labelId)
        {
            Debug.LogError(labelId);
            view.Live2dTigger(EXPRESSIONTRIGERTYPE.LOVEDIARY, labelId,false);
            int num = GlobalData.ConfigModel.GetConfigByKey(GameConfigKey.DIARY_ADD_FAVOR);

            view.ShowNumFavorability(num);

        }

        private void OnChangeRole(MapField<int,int> apparel)
        {
            view.SetRoleBg(apparel[0]);
            FlowText.ShowMessage(I18NManager.Get("FavorabilityMain_SucceedShow"));
        }

        private void OnUserLevelup()
        {
            view.SetLevel(GlobalData.PlayerModel.PlayerVo.Level);
            //玩家升级之后就要更新当前等级应援会规则能力。
            GlobalData.PlayerModel.BaseSupportPower = MyDepartmentData.GetDepartmentRule(DepartmentTypePB.Support
                , GlobalData.PlayerModel.PlayerVo.Level).Power;

           // FlowText.ShowMessage(I18NManager.Get("GameMain_Hint1", GlobalData.PlayerModel.PlayerVo.Level));// ("应援会等级提升至" + GlobalData.PlayerModel.PlayerVo.Level);
            //弹出应援会升级弹窗
            var _levelupWindow= PopupManager.ShowWindow<LevelUpWindow>("GameMain/Prefabs/LevelupWindow/LevelUpWindow");
            _levelupWindow.SetData(GlobalData.DepartmentData);
            
            UpdateTopBar();
            view.HandleFunctionOpen();
            
            SdkHelper.SetSdkData(SdkHelper.TYPE_DATA_CHANGE);
        }

        private void UpdateTopBar()
        {
            view.SetData(GlobalData.PlayerModel.PlayerVo);
            EventDispatcher.TriggerEvent(EventConst.UpdateShopTopBar);
        }

        private void InitMainLive2d()
        {
            Debug.Log("COMMENT URL"+GlobalData.VersionData.ForceUpdateAddr);
            view.CheckDownLoadQueue();
            view.SetMainLive2d(GlobalData.PlayerModel.PlayerVo);
        }

        private void SetPowerState(bool state)
        {
            view.SetSupporterPowerInfo(state);
        }
        
        


        public override void OnMessage(Message message)                       //主菜单消息发送
        {
            string name = message.Name;
            object[] body = message.Params;
            bool isStopPlayDubbing = true;

            switch (name)
            {
                
                case MessageConst.CMD_MAIN_ON_STAR_ACTIVITY_BTN:
                    if (GlobalData.MissionModel.IsShowStarActivity())
                    {
                        ModuleManager.Instance.EnterModule(ModuleConfig.MODULE_STAR_ACTIVITY,false,true,GlobalData.MissionModel.GetOpenDay()); 
                    }
                    else
                    {
                        view.HandleFunctionOpen();
                    }
                    break;
                case MessageConst.CMD_MAIN_CHANGE_DISPLAY:                                //--主菜单显示状态 Star
                    MainMenuDisplayState state = (MainMenuDisplayState)message.Body;

                    PopupManager.StopHandleShowWindow();

                    switch (state)
                    {
                        case MainMenuDisplayState.ShowAll:
                            view.ShowAll();
                            //CheckNeedToDownLoadExtend();
                           
                            PopupManager.ShowPhoneTipsWindow(()=> SendRedPoint());
                            break;
                        case MainMenuDisplayState.ShowUserInfo:
                            view.ShowUserInfo();
                            break;
                        case MainMenuDisplayState.ShowUserInfoAndTopBar:
                            view.ShowTopBarAndUserInfo();
                            break;
                        case MainMenuDisplayState.ShowTopBar:
                            view.ShowTopBar();
                            break;
                        case MainMenuDisplayState.ShowVisitTopBar:
                            view.ShowTopBar(MainMenuDisplayState.ShowVisitTopBar);
                            break;
                        case MainMenuDisplayState.HideAll:
   
                         if(_window!=null)
                             _window.Close();
                         
                            view.ShowAll(false);
                            
                            break;
                        case MainMenuDisplayState.ShowRecollectionTopBar:
                            view.ShowTopBar(MainMenuDisplayState.ShowRecollectionTopBar);
                            break;
                        case MainMenuDisplayState.ShowExchangeIntegralBar:
                            view.ExchangeIntegralBarSetData(GlobalData.TrainingRoomModel.GetCurIntegral());
                            view.ShowTopBar(MainMenuDisplayState.ShowExchangeIntegralBar);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }                                                                    //--主菜单显示状态 End
                    break;
           
                case MessageConst.CMD_MAIN_ON_START_BTN:
                    var isShowArrow = (bool) body[0];
                    Debug.LogError("isShowArrow===>"+isShowArrow);
                    ModuleManager.Instance.EnterModule(ModuleConfig.MODULE_GAME_PLAY,false,true,isShowArrow);      //--模块消息发送 Start
                    break;
                case MessageConst.CMD_MAIN_ON_CARD_BTN:
                    ModuleManager.Instance.EnterModule(ModuleConfig.MODULE_CARD);
                    break;
                case MessageConst.CMD_MAIN_ON_SUPPORTER_BTN:
                    ModuleManager.Instance.EnterModule(ModuleConfig.MODULE_SUPPORTER);
                    break;
                case MessageConst.CMD_TASK_SHOW_DAILYTASK:
                    ModuleManager.Instance.EnterModule(ModuleConfig.MODULE_MISSION);
                    break;
                case MessageConst.CMD_GOTOACHIEVEMENT:
                    ModuleManager.Instance.EnterModule(ModuleConfig.MODULE_ACHIEVEMENT);
                    break;
                case MessageConst.CMD_APPOINTMENT_JUMPCHOOSEROLE:
                    ModuleManager.Instance.EnterModule(ModuleConfig.MODULE_LOVE);
                    //PopupManager.ShowWindow<LoveJumpWindow>("GameMain/Prefabs/LoveJumpWindow/LoveJumpWindow");
                    break;
                case MessageConst.CMD_MAIN_ON_CHANGE_ROLE_BTN:                          
                    ModuleManager.Instance.EnterModule(ModuleConfig.MODULE_FAVORABILITYMAIN);
                    break;
                case MessageConst.CMD_MAIN_ON_DRAWCARD_BTN:
                    ModuleManager.Instance.EnterModule(ModuleConfig.MODULE_DRAWCARD);
                    break;
                case MessageConst.CMD_MAIN_ON_PHONE_BTN:
                    ModuleManager.Instance.EnterModule(ModuleConfig.MODULE_PHONE);
                    break;
                case MessageConst.CMD_MAIN_ON_ACTIVITY_BTN:
                    //SendActivityMsg();
                    //进入活动模块
                    var id = (string)message.Body;                                         
                    ModuleManager.Instance.EnterModule(ModuleConfig.MODULE_ACTIVITY, false, true, id);
                    break;
                case MessageConst.CMD_MAIN_ON_MAIL_BTN:

                    PopupManager.StopHandleShowWindow();
                    ModuleManager.Instance.EnterModule(ModuleConfig.MODULE_MAIL);
                    break;
                case MessageConst.CMD_MAIN_ON_ALBUM_BTN:
                    FlowText.ShowMessage(I18NManager.Get("Common_Underdevelopment"));
                    break;
                case MessageConst.CMD_MAIN_ON_STAGINGPOST_BTN:
                    ModuleManager.Instance.EnterModule(ModuleConfig.MODULE_SHOP,false,true,0);
                    break;
                case MessageConst.CMD_MAIN_SHOW_BUY_POWER:     //买体力
                    var _buyPowerUpperlimit = GlobalData.PlayerModel.BuyPowerUpperlimit; //10
                    if (GlobalData.PlayerModel.PlayerVo.PowerNum >= _buyPowerUpperlimit)
                    {
                        FlowText.ShowMessage(I18NManager.Get("Common_TodaysBuyUpperlimit"));// ("今日兑换次数已达上限");
                        return;
                    }
                    else
                    {
                        ShowBuyPowerWindow();
                    }
                    break;
                
                case MessageConst.CMD_MAIN_SHOW_BUY_GOLD:    //买金币
                    var _buyGlodUpperlimit = GlobalData.PlayerModel.BuyGoldUpperlimit;  //10
                    if (GlobalData.PlayerModel.PlayerVo.GoldNum >= _buyGlodUpperlimit)
                    {
                        FlowText.ShowMessage(I18NManager.Get("Common_TodaysBuyUpperlimit"));
                        return;
                    }
                    else {
                        ShowBuyGlodWindow();
                    }
                    break;
                
                case MessageConst.CMD_SUPPORTERACTIVITY_BUYENCOURAGEPOWER:    //买应援行动力

                    var _buyEncouragePowerUpperlimit = GlobalData.PlayerModel.BuyEncouragePowerUpperlimit; //5
                    if (GlobalData.PlayerModel.PlayerVo.EncourageNum>= _buyEncouragePowerUpperlimit)
                    {
                        FlowText.ShowMessage(I18NManager.Get("Common_TodaysBuyUpperlimit"));
                        return;
                    }
                    else
                    {
                        BuyEncouragePower();
                    }
                    break;
                
                case MessageConst.CMD_MAIN_SHOW_BUY_GEM:
                    ModuleManager.Instance.EnterModule(ModuleConfig.MODULE_SHOP,false,true,5);
                    //ModuleManager.Instance.EnterModule(ModuleConfig.MODULE_BUYGEN);
//                    ModuleManager.Instance.EnterModule(ModuleConfig.MODULE_PAY,false); 

//                    (SdkHelper.PayAgent as PayAgentGooglePlay)?.Test();
                    break;
                case MessageConst.CMD_RECOLLECTION_SENDBUYEVENT:                
                    EventDispatcher.TriggerEvent(EventConst.SendBuyRecolletionPowerEvent);                   
                    break;
                
                case MessageConst.CMD_MAIN_ON_LIVE2DCLICK:
                    isStopPlayDubbing = false;
                    SendNpcMainStageInteract();
                    break;
                
                case MessageConst.CMD_MAIN_ON_FIRITSRECHARGE_BTN:
                    //通过首冲进入活动模块
                    ModuleManager.Instance.EnterModule(ModuleConfig.MODULE_ACTIVITYFIRSTRECHARGE, false, true);
                    break;
                case MessageConst.CMD_MAIN_STORESCORECOMMENT:
                    bool commentType = (bool)body[0];
                    string comment = (string) body[1];
                    int star = (int) body[2];
                    Debug.LogError(comment.Length);
                    if (commentType)
                    {
                        SetCommentRes(true,comment,star);
                    }
                    else
                    {
                        if (comment.Length<20)
                        {
                            FlowText.ShowMessage(I18NManager.Get("GameMain_MinTextCount"));
                            return;
                        }
                        else if(comment.Length>600)
                        {
                            FlowText.ShowMessage(I18NManager.Get("GameMain_MaxTextCount"));
                            return;
                        }
                        SetCommentRes(false,comment,star);
                    }
                    

                    break;
                case MessageConst.CMD_MAIN_FIRESHDOWNLOADAWARD:
//                    Debug.LogError("has receiveDownload"+GlobalData.PlayerModel.PlayerVo.ExtInfo.DownloadReceive);
                    //GuideManager.SetStatisticsRemoteGuideStep(GuideConst.MainLineStep_Extend_DowndLoad); 
                   
                    if (GlobalData.PlayerModel.PlayerVo.ExtInfo.DownloadReceive==0)
                    {
                        NetWorkManager.Instance.Send<ReceiveDownloadAwardsRes>(CMD.DOWNLOAD_RECEIVEAWARD, null, OnDownloadReceiveAward);                       
                    }
//                    else
//                    {
//                        Debug.LogError("另外奖励又重新删包，在下载");
//                        SendMessage(new Message(MessageConst.CMD_DOWNLOAD_OK));  
//                    }
                    
                    
                    
                    break;
                case MessageConst.CMD_MAIN_ON_PLAYERBIRTHDAY:
                    if (GlobalData.MissionModel.IsShowPlayerBirthday())
                    {
                        ModuleManager.Instance.EnterModule(ModuleConfig.MODULE_PLAYERBIRTHDAY, false, true,GlobalData.MissionModel.GetPlayerBirthdayOpenDay());
                    }
                    else
                    {
                        view.HandleFunctionOpen();
                    }
                    break;
                case MessageConst.CMD_MAIN_ON_ACTIVITYTEMPLATE_BTN:
                    if (GlobalData.ActivityModel.IsShowActivityTemplateBtn(ActivityTypePB.ActivityDrawTemplate))
                    {
                        ModuleManager.Instance.EnterModule(ModuleConfig.MODULE_ACTIVITYTEMPLATE,false,true);  
                    } 
                    else
                    {
                        view.HandleFunctionOpen();
                    }
                    break;
                case MessageConst.CMD_MAIN_ON_ACTIVITYCAPSULETEMPLATE_BTN:

                    ModuleManager.Instance.EnterModule(ModuleConfig.MODULE_ACTIVITYCAPSULETEMPLATE, false, true);
                    break;
                case MessageConst.CMD_MAIN_ON_TOTALRECHARGE_BTN:
                    ModuleManager.Instance.EnterModule(ModuleConfig.MODULE_ACTIVITY, false, true,GlobalData.ActivityModel.GetActivityVo(ActivityType.ActivityAccumulativeRecharge).JumpId);
                    break;
                case MessageConst.CMD_MAIN_ON_ACTIVITYMUSICTEMPLATE_BTN:
                    ModuleManager.Instance.EnterModule(ModuleConfig.MODULE_ACTIVITYMUSICTEMPLATE, false, true);
                    break;
                default:
                    return;
            }

            if (isStopPlayDubbing)
                StopPlayingDubbing();
        }

        private void OnDownloadReceiveAward(ReceiveDownloadAwardsRes res)
        {
            RewardUtil.AddReward(res.Awards);
            GuideManager.SetStatisticsRemoteGuideStep(GuideConst.MainLineStep_Extend_DowndLoad);                
             view.InitOpenArrowCondition();    
                 
            //todo 整包的时候不会弹出领奖阻碍引导！
            if (_awardWindow==null)
            {
                SendMessage(new Message(MessageConst.CMD_HIDEVIEW));  
                _awardWindow=PopupManager.ShowWindow<AwardWindow>("GameMain/Prefabs/AwardWindow/AwardWindow");
                _awardWindow.WindowActionCallback = evt => {
                    if (evt== WindowEvent.ClickOutsideToClose)
                    {
                       
                    //  SendMessage(new Message(MessageConst.CMD_DOWNLOAD_OK));
                      PopupManager.ShowPhoneTipsWindow(() => { SendRedPoint(); });
                      view.InitOpenArrowCondition();
                    }
                      
                };
            }
            _awardWindow.SetData(res.Awards,"引导通过奖励");
            GlobalData.PlayerModel.PlayerVo.ExtInfo = res.UserExtraInfo;

        }

        private void SetCommentRes(bool comtype,string comment,int star)
        {

            var buffer = NetWorkManager.GetByteData(new CommentReq
                {Comment = comment, Star = star, Type = comtype ? 1 : 0});
            
            NetWorkManager.Instance.Send<CommentRes>(CMD.STORESCORE_Comment, buffer, res =>
            {
                GlobalData.PlayerModel.UpdateUserMoney(res.UserMoney);
                _storeScoreWindow.Close();
                if (_awardWindow==null)
                {
                    _awardWindow=PopupManager.ShowWindow<AwardWindow>("GameMain/Prefabs/AwardWindow/AwardWindow");
                }
                RepeatedField<AwardPB> gemAward=new RepeatedField<AwardPB>();
                AwardPB awardPb=new AwardPB(){Num = 10,Resource = ResourcePB.Gem,ResourceId = 30001};
                gemAward.Add(awardPb);
                
                _awardWindow.SetData(gemAward);
                
                            
                //评分完成且拿完奖励后去商店评分！
                if (comtype)
                {
                    Debug.LogError("COMMENT URL"+GlobalData.VersionData.ForceUpdateAddr);
                    Application.OpenURL(GlobalData.VersionData.ForceUpdateAddr);
                }

            });

            
            
        }
        
        private void BuyEncouragePower()
        {
            QuickBuy.BuyGlodOrPorwer(PropConst.EncouragePowerId, PropConst.GemIconId);
        }

        /// <summary>
        /// 显示兑换金币窗口
        /// </summary>
        private void ShowBuyGlodWindow()
        {
            QuickBuy.BuyGlodOrPorwer(PropConst.GoldIconId, PropConst.GemIconId);
        }


        /// <summary>
        /// 显示兑换体力窗口
        /// </summary>
        private void ShowBuyPowerWindow()
        {
            QuickBuy.BuyGlodOrPorwer(PropConst.PowerIconId, PropConst.GemIconId);
        }

        private void OnActivitySignInClick(int activityId)
        {
            var req = new SignInReq();
            Debug.LogError("activityId " + activityId);
            req.ActivityId = activityId;
            var dataBytes = NetWorkManager.GetByteData(req);
            NetWorkManager.Instance.Send<SignInRes>(CMD.ACTIVITY_SIGNIN, dataBytes, OnActivitySignInHandler);
        }

        


        private void OnActivitySignInHandler(SignInRes res)
        {
            Debug.Log("OnActivitySignInHandler");
            foreach (var v in res.Awards)
            {
             //   Debug.LogError(v.Num + " " + v.ResourceId + "" + v.Resource);
                switch (v.Resource)
                {
                    case ResourcePB.Card:
                        GlobalData.CardModel.UpdateUserCardsByIdAndNum(v.ResourceId, v.Num);
                        break;
                    case ResourcePB.Item:
                        GlobalData.PropModel.AddProp(v);
                        break;
                    case ResourcePB.Puzzle://更新碎片
                        //策划说不会送碎片   所以不做
                        break;
                    case ResourcePB.Power:
                        GlobalData.PlayerModel.AddPower(v.Num);
                        break;
                    case ResourcePB.Gem:
                        GlobalData.PlayerModel.UpdateUserGem(v.Num);
                        break;
                    case ResourcePB.Gold:
                        GlobalData.PlayerModel.UpdateUserGold(v.Num);
                        break;
                    case ResourcePB.Fans:
                        GlobalData.DepartmentData.UpdateFans(v.ResourceId, v.Num);
                        break;
                    case ResourcePB.Memories:
                        GlobalData.PlayerModel.AddRecollectionEnergy(v.Num);
                        break;
                }
            }
        }

        private void OnDataLoadComplete()
        {            
          PopupManager.ShowPhoneTipsWindow(() => { SendRedPoint(); }); 
        }

        bool isSendNpcMainStageInteract = false;
        private void SendNpcMainStageInteract()//主界面互动加好感度
        {
            if (isSendNpcMainStageInteract)
                return;
            int max = GlobalData.ConfigModel.GetConfigByKey(GameConfigKey.MAIN_STAGE_INTERACT_MAX_CNT);

            int Cnt= GlobalData.NpcModel.GetNpcDialyInteractCnt();
            if (Cnt < 0)
                return;
            if (Cnt >= max)
            {
               view.ShowNumFavorability();
                return;
            }
            isSendNpcMainStageInteract = true;
            NetWorkManager.Instance.Send<NpcMainStageInteractRes>(CMD.NPC_STAGEINTERACT, null, OnNpcMainStageInteractHandler);     
        }

        private void OnNpcMainStageInteractHandler(NpcMainStageInteractRes res)
        {
            GlobalData.NpcModel.UpdateUserNpcPB(res.UserNpcs);
            GlobalData.FavorabilityMainModel.UpdateUserFavorability(res.UserFavorability);
            int n = GlobalData.ConfigModel.GetConfigByKey(GameConfigKey.MAIN_STAGE_INTERACT_ADD_FAVOR);
            view.ShowNumFavorability(n);
            isSendNpcMainStageInteract = false;
        }


        
        private void SendRedPoint()
        {           
            NetWorkManager.Instance.Send<MsgRes>(CMD.MAINPANELVIEW_RED_POINT, null, OnRedPointHandler);
        } 

        private void OnRedPointHandler(MsgRes res)
        {
            //主界面红点消息
            view.HideAllRedPoint();
            GlobalData.DepartmentData.CanGetFriendsPower = false;
            GlobalData.DepartmentData.CanGetSupporterActAward = false;
                                    
            if(res.Msgs!=null)
            {
                for (int i = 0; i < res.Msgs.Count; i++)
                {
                    GlobalData.ActivityModel.SetActivityMissionRedDot(res.Msgs[i].MsgKey,res.Msgs[i].ExtI);                  
                    if (res.Msgs[i].MsgKey == Constants.FRIEND_MSG_KEY)
                    {
                        //应援会内的好友红点要知道状态
                        GlobalData.DepartmentData.CanGetFriendsPower = true;
                    }

                    if (res.Msgs[i].MsgKey==Constants.ENCOURAGE_ACT)
                    {
                        GlobalData.DepartmentData.CanGetSupporterActAward = true;
                    }

                 
                    
                    if (res.Msgs[i].MsgKey == Constants.PHONE_MSG_KEY)
                    { continue; }

                    if (res.Msgs[i].MsgKey==Constants.DAILY_PACKAGE_MSG_KEY)
                    {
                        GlobalData.PlayerModel.PlayerVo.ExtInfo.GotDailyPackageStatus = false;
                    }

                    if (res.Msgs[i].MsgKey == Constants.ACTIVITY_MISSION)
                    {
                        view.SetRedPoint(Constants.ACTIVITY_MSG_KEY);
                        continue;
                    }
                    view.SetRedPoint(res.Msgs[i].MsgKey);
                }
            }
            
            if (GlobalData.DepartmentData.CanLevelUpDepartment())
            {
                view.SetRedPoint(Constants.SUPPORTER_MSG_KEY);
            }

            //手机小红点不走后端
            if (Util.GetIsRedPoint(Constants.REDPOINT_PHONE_BGVIEW_SMS) ||
                         Util.GetIsRedPoint(Constants.REDPOINT_PHONE_BGVIEW_CALL)||
                         Util.GetIsRedPoint(Constants.REDPOINT_PHONE_BGVIEW_FC) ||         
                         Util.GetIsRedPoint(Constants.REDPOINT_PHONE_BGVIEW_WEIBO))
            {
                view.SetRedPoint(Constants.PHONE_MSG_KEY);
            }


            //活动小红点不走运后端
            if (GlobalData.ActivityModel.IsShowRedDot())// &&
               // GuideManager.GetRemoteGuideStep(GuideTypePB.MainGuide)>GuideConst.MainStep_Achievement_EnterRole)        
            {
                view.SetRedPoint(Constants.ACTIVITY_MSG_KEY);
            }

            //首充小红点不走后端
            if (GlobalData.ActivityModel.IsShowFirstRechargeBtn())
            {               
                if (GlobalData.ActivityModel.IsShowFirstRechargeRedDot())
                {
                    
                    view.SetRedPoint(Constants.ACTIVITY_MSG_FIRSTRECHARGE);
                }
            }
            //游客红点设置
            if(GlobalData.PlayerModel.PlayerVo.IsGuset)
            {
                view.SetRedPoint(Constants.USERCENTER_MSG_KEY);
            }
                                                                                          
            if (GlobalData.CardModel.ShowRedPoint)
            {
                view.SetRedPoint(Constants.CARD_MSG_KEY);
            }


            SetShopRetPoint();

        }

        private void SetShopRetPoint()
        {

            if (GlobalData.PlayerModel.PlayerVo.HasGetFreeGemGift)
            {
                view.SetRedPoint(Constants.SHOP_FREEMALL_MSG_KEY);  
            }
            
        }

        private DateTime _lastDate;

        public override void Destroy()
        {
            base.Destroy();
            EventDispatcher.RemoveEvent(EventConst.UpdateUserMoney);
            EventDispatcher.RemoveEvent(EventConst.UserLevelUp);
            EventDispatcher.RemoveEvent(EventConst.MainLineLevelUpdate);
            EventDispatcher.RemoveEvent(EventConst.UpdateEnergy);
            EventDispatcher.RemoveEvent(EventConst.ChangeRole);
            EventDispatcher.RemoveEvent(EventConst.ActivitySignInClick);
            EventDispatcher.RemoveEvent(EventConst.ShowActivityAward);
            EventDispatcher.RemoveEvent(EventConst.OnDataLoadComplete);
            EventDispatcher.RemoveEvent(EventConst.CreateActivityContenet);
            EventDispatcher.RemoveEvent(EventConst.LoveDiaryEditSaveAndGoBackMainModule);
            ClientTimer.Instance.RemoveCountDown("ChangeBackground");
            ClientTimer.Instance.RemoveCountDown(_dailyRefreshtimer);
            EventDispatcher.RemoveEvent(EventConst.UpDataUserName);
            EventDispatcher.RemoveEvent(EventConst.CloseFirstRechargeBtn);
            EventDispatcher.RemoveEvent(EventConst.RefreshActivityImageAndActivityPage); 
            EventDispatcher.RemoveEvent(EventConst.RefreshPoint);
            EventDispatcher.RemoveEvent(EventConst.UpdateExchangeIntegral);
            EventDispatcher.RemoveEvent(EventConst.OnTriggerGiftChange);
            EventDispatcher.RemoveEvent(EventConst.TriggerGiftPaySuccess);
            EventDispatcher.RemoveEvent(EventConst.ShowStoreScore);
          //  EventDispatcher.RemoveEvent(EventConst.ShowConfirmBind);
            EventDispatcher.RemoveEvent(EventConst.SettingUserInfoUpdate);
            EventDispatcher.RemoveEvent(EventConst.GetRealNameAward);
            EventDispatcher.RemoveEvent(EventConst.ChangeTopPower);
            EventDispatcher.RemoveEvent(EventConst.GetPayInfoSuccess);
            EventDispatcher.RemoveEvent(EventConst.UpdateMainViewHeadInfo);
            if (_checkAntiCountDown!=null)
            {
                ClientTimer.Instance.RemoveCountDown(_checkAntiCountDown);
            }

            if (_addictionTimerHandler!=null)
            {
                ClientTimer.Instance.RemoveCountDown(_addictionTimerHandler); 
            }
            
        }

        #region 临时的十日登录奖励

        //        private void SendActivityMsg()
//        {
//            //NetWorkManager.Instance.Send<MissionRuleRes>(CMD.MISSION_RULES, null, OnGetMissionRule, null, true,      
//            //    GlobalData.VersionData.VersionDic[CMD.MISSION_RULES]);
//            NetWorkManager.Instance.Send<MissionRuleRes>(CMD.MISSION_RULES, null, OnGetMissionRule);
//
//        }
//
//        private void OnGetMissionRule(MissionRuleRes res)
//        {
//            missionPbList.Clear();
//            for (int i=0;i<res.MissionRules.Count;i++)
//            {
//                if(res.MissionRules[i].MissionType==MissionTypePB.Sign)
//                {
//                    missionPbList.Add(res.MissionRules[i]);
//                }
//            }
//            NetWorkManager.Instance.Send<ActivityRes>(CMD.ACTIVITY_ACTIVITYLIST, null, OnActivityHandler); 
//        }
//
//        private void OnActivityHandler(ActivityRes res)
//        {                       
//            //todo  获取所有的活动  需求还不明确 后端说先暂时这样应付
//            foreach (var v in res.Activitys)
//            {
//                // Debug.LogError("activityId " + v.ActivityId);
//            //     Debug.LogError("活动名字：" + v.Name);
//                if (v.ActivityId != 1)
//                    continue;
//                long curTime = ClientTimer.Instance.GetCurrentTimeStamp();
//                if (curTime < v.StartTime || curTime > v.EndTime)
//                {
//                    continue;
//                }
//
//                //Debug.Log(curTime - v.StartTime);
//                int diffDay = (int)((curTime - v.StartTime) / (24 * 60 * 60 * 1000) + 1);
//                //var missionPb = GlobalData.MissionModel.GetMissionById(v.ActivityId);
//                //无法识别是哪一个活动的  后端需完善 全部默认十天签到
//               // List<MissionRulePB> missionPbList = GlobalData.MissionModel.GetMissionRulePBListByType(MissionTypePB.Sign);
//                List<MissionRulePB> unFinisedMission = new List<MissionRulePB>();
//                foreach (var missionPb in missionPbList)
//                {
//                    if (missionPb.MissionId > diffDay)
//                    {
//                        continue;
//                    }
//
//                    //表示已完成任务
//                    if (res.UserSigin == null)
//                    {
//                        unFinisedMission.Add(missionPb);
//                    }
//                    else
//                    {
//                        if (res.UserSigin.SignInRecord.Contains(missionPb.MissionId))
//                        {
//                            //todo
//                        }
//                        else//未完成任务
//                        {
//                            unFinisedMission.Add(missionPb);
//                        }
//                    }
//                }
//
//                if (unFinisedMission.Count > 0)
//                {
//                    var ActivityWind = PopupManager.ShowWindow<ActivityAwardWindow>("GameMain/Prefabs/ActivityAward/ActivityAwardWindow");
//                    ActivityWind.SetData(unFinisedMission, v.ActivityId);
//                }
//            }
//        }

        #endregion

    }
}