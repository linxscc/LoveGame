using System.Runtime.InteropServices;

namespace Common
{
    /// <summary>
    /// 全局事件(用于模块之间，窗口和模块的交互)
    /// </summary>
    public class EventConst
    {
        //连接服务器成功
        public const string OnConnetToServer = "OnConnetToServer";
        public const string OnChooseServer = "OnChooseServer";

        //Common
        public const string MainMenuDisplayChange = "MainMenuDisplayChange";
        public const string UpdateUserMoney = "UpdateUserMoney";
        public const string UserLevelUp = "UserLevelUp";
        public const string UpdateEnergy = "UpdateEnergy";
        public const string ChangeTopPower = "ChangeTopPower";
        public const string ShowStoreScore = "ShowStoreScore";
        public const string BindIdCard = "BindIdCard";
        public const string ShowConfirmBind = "ShowConfirmBind";

        public const string DailyRefresh6 = "DailyRefresh6";
        public const string ActivityUserDataRefresh = "ActivityUserDataRefresh";

        public const string OnTriggerGiftChange = "OnTriggerGiftChange"; //触发型礼包

        public const string SendBuyRecolletionPowerEvent = "SendBuyRecolletionPowerEvent"; //发送购买交卷的事件
        public const string RefreshActivityImageAndActivityPage = "RefreshActivityImageAndActivityPage";

        public const string MainLineLevelUpdate = "MainLineLevelUpdate"; //主线关卡更新
        public const string SettingUserInfoUpdate = "SettingUserInfoUpdate"; //用户信息变更

        public const string GetRealNameAward = "GetRealNameAward";//获取实名认证奖励
        //道具更新
        public const string PropUpdated = "PropUpdated";

        public const string RefreshPoint = "RefreshPoint";


        //Login
        public const string CheckActiveCode = "CheckActiveCode";

        //CreateUser
        public const string CreateRoleSubmit = "CreateRoleSubmit";
        public const string OnCreateUser = "OnCreateUser";
        public const string CreateUserEnd = "CreateUserEnd";

        //LoginAward
        public const string OnDataLoadComplete = "OnDataLoadComplete";
        public const string ShowActivityAward = "ShowActivityAward";
        public const string ActivitySignInClick = "ActivitySignInClick";

        //ChangeRole
        public const string ChangeRole = "ChangeRole";
        public const string JumpToDisiposition = "JumpToDisiposition";

        //Gameplay
        public const string GameplayGotoCity = "GameplayGotoCity";
        public const string ShowStoryView = "ShowStoryView";


        //CardCollection
        public const string CollectedCardClick = "CollectedCardClick";
        public const string CardPuzzleClick = "CardPuzzleClick";
        public const string CardResolveClick = "CardResolveClick";

        public const string CardResolveSelectedChange = "CardResolveSelectedChange";

        public const string CardEvoConfirm = "CardEvoConfirm";

        //LoveDiary
        public const string LoveDiaryEditItemClick = "LoveDiaryEditItemClick";
        public const string LoveDiaryEditBgItemClick = "LoveDiaryEditBgItemClick";
        public const string LoveDiaryEditColorItemClick = "LoveDiaryEditColorItemClick";
        public const string LoveDiaryEditOkInputText = "LoveDiaryEditOkInputText";
        public const string LoveDiaryEditItemText = "LoveDiaryEditItemText";
        public const string LoveDiaryHideEditText = "LoveDiaryHideEditText";
        public const string LoveDiaryEditDeleteElement = "LoveDiaryEditDeleteElement";
        public const string LoveDiaryEditSaveAndGoBackMainModule = "LoveDiaryEditSaveAndGoBackMainModule";
        public const string LoveDiaryElementModify = "LoveDiaryElementModify";

        //phone
        public const string PhoneSmsItemClick = "PhoneSmsItemClick";
        public const string PhoneCallItemClick = "PhoneCallItemClick";
        public const string PhoneFriendCirclePublishClick = "PhoneFriendCirclePublishClick";
        public const string PhoneFriendCircleReplyClick = "PhoneFriendCircleReplyClick";
        public const string ClickFriendCircleItemReplyClick = "ClickFriendCircleItemReplyClick";
        public const string PhoneWeiboItemLikeClick = "PhoneWeiboItemLikeClick";
        public const string PhoneWeiboItemPublishClick = "PhoneWeiboItemPublishClick";

        //Battle
        public const string SmallHeroCardClick = "SmallHeroCardClick";
        public const string ShowLastBattleWindow = "ShowLastBattleWindow";
        public const string EnterBattle = "EnterBattle";
        public const string BuyLevelCount = "BuyLevelCount";
        public const string UpdateSupporterNum = "UpdateSupporterNum";

        //Appointment
        public const string ChooseAppointmentRole = "ChooseAppointmentRole";
        public const string OpenGate = "OpenGate";
        public const string LoveStoryEnd = "LoveStoryEnd";
        public const string LoveAppointmentGotoCardResolve = "LoveAppointmentGotoCardResolve";


        //Task
        public const string RecieveTaskReward = "RecieveTaskReward";
        public const string JumpToSpecialWindow = "JumpToSpecialWindow";
        public const string GoBackToMain = "GoBackToMain";
        public const string BuyGoldSuccess = "BuyGoldSuccess";
        public const string JumpToCMD = "JumpToCMD";

        //Recollection
        public const string RecollectionCardClick = "RecollectionCardClick";
        public const string RecollentionRewardGetWindowClose = "RecollectionRewardWindowClose"; //领取奖励
        public const string MemoriesReselatNumWindowClose = "CardDropPropWindowClose"; //可选则其他卡
        public const string AwardIsEnough = "AwardIsEnough"; //奖励是否足够

        //Favorability
        public const string ReloadingItemClick = "FavorabilityItemClick"; //好感度Item（服装或背景）点击
        public const string FavorabilityGiveGiftsItemClick = "FavorabilityGiveGiftsItemClick"; //好感度赠送礼物道具点击
        public const string OnClickVoiceItem = "OnClickVoiceItem"; //点击语音Item

        //AirborneGame
        public const string AirborneGameItemOnTriggerEnter2D = "AirborneGameItemOnTriggerEnter2D";

        //Activity
        public const string PreviewAward = "PreviewAward";
        public const string GetCardAward = "GetCardAward";
        public const string GetNormalAward = "GetNormalAward";
        public const string CreateActivityContenet = "CreateActivityContenet";
        public const string ActivityTemplatePreviewAward = "ActivityTemplatePreviewAward";
        public const string GetActivityTemplateAward = "GetActivityTemplateAward";


        public const string GetEverydayPowerAward = "GetEverydayPowerAward";
        public const string OpenEverydayPowerAwardRetroactiveWindow = "OpenEverydayPowerAwardRetroactiveWindow";
        public const string SendRetroactiveEverydayPowerAwardReq = "SendRetroactiveEverydayPowerAwardReq";
        public const string UpDataUserName = "UpDataUserName";
        public const string UpDataSetPanelName = "UpDataSetPanelName";
        public const string MonthSigin = "MonthSigin"; //月签
        public const string MonthRetroactive = "MonthRetroactive"; //补签

        public const string UpDataUserHead = "UpDataUserHead";


        public const string GetGrowthFundAward = "GetGrowthFundAward";
        public const string CloseFirstRechargeBtn = "CloseFirstRechargeBtn"; //首冲完成
        public const string ReceiveCumulativeAward = "ReceiveCumulativeAward"; //领取累积充值奖励

        //Visit
        public const string VisitSelectItemWeatherClick = "VisitSelectItemWeatherClick";
        public const string VisitSelectItemVisitClick = "VisitSelectItemVisitClick";
        public const string VisitLevelItemClick = "VisitSelectItemClick";
        public const string VisitLevelItemExtraClick = "VisitLevelItemExtraClick";
        public const string VisitLevelItemGotoWeather = "VisitLevelItemGotoWeather";
        public const string VisitLevelResetLevelTime = "VisitLevelResetLevelTime";

        public const string VisitFirsetLevelItem = "VisitFirsetLevelItem";

        //VisitBattle
        public const string EnterVisitBattle = "EnterVisitBattle";
        public const string ShowLastVisitBattleWindow = "ShowLastVisitBattleWindow";
        public const string UpdateVisitSupporterNum = "UpdateVisitSupporterNum";

        //Main 回到登陆界面
        public static string ForceToLogin = "ForceToLogin";
        public static string ForceToLoginUserCenter = "ForceToLoginUserCenter";

        //Mail
        public const string MailItemOnClick = "MailItemOnClick";
        public const string MailPastDue = "MailPastDue";
        public const string DeleteReadMail = "DeleteReadMail";
        public const string GetOneMailAwardSucceed = "GetOneMailAwardSucceed";

        public const string DeleteOneMail = "DeleteOneMail";

        //Shop 购买礼包
        public const string BuyMallGift = "BuyMallGift";
        public const string BuyRmbMallGift = "BuyRmbMallGift";
        public const string BuyGemMall = "BuyGemMall";
        public const string BuyMallItem = "BuyMallItem";
        public const string BuyMallBatchItem = "BuyMallBatchItem";
        public const string BuyGoldMallItem = "BuyGoldMallItem";
        public const string RefreshGoodsItem = "RefreshGoodsItem";
        public const string GetPayInfoSuccess = "GetPayInfoSuccess";
        public const string PayforGift = "PayforGift";
        public const string PayforSpecial = "PayforSpecial";
        public const string PayforSpecialGift = "PayforSpecialGift";
        public const string PayforDaily = "PayforDaily";
        public const string TriggerGiftPaySuccess = "TriggerGiftPaySuccess";
        public const string UpdateShopTopBar = "UpdateShopTopBar";

        public const string ActivityGetDrawCardAward = "ActivityGetDrawCardAward";

        //Guide 新手引导
        public const string GuideToLoveStoryGoBack = "GuideToLoveStoryGoBack";

        //Achievement
        public const string RecieveAchievementReward = "RecieveAchievementReward";

        public const string UpdataSupporterFansViewName = "UpdataSupporterFansViewName";
        public const string JumpToAchievementCMD = "JumpToAchievementCMD";

        //Friend
        public const string FriendDoApply = "FriendDoApply";
        public const string FriendAgree = "FriendAgree";
        public const string FriendIgnore = "FriendIgnore";
        public const string FriendDelete = "FriendDelete";
        public const string FriendSendPower = "FriendSendPower";

        public const string FriendGetPower = "FriendGetPower";

        //音友
        public const string MusicRhythmItemFinishedUse = "MusicRhythmItemFinishedUse";
        public const string MusicRhythmItemShortValidClick = "MusicRhythmItemShortValidClick";
        public const string MusicRhythmItemLongValidDownClick = "MusicRhythmItemLongValidDownClick";

        public const string MusicRhythmItemLongValidUpClick = "MusicRhythmItemLongValidUpClick";

        //练习室
        public const string UpdateExchangeIntegral = "UpdateExchangeIntegral";
        public const string BuyExchangeItem = "BuyExchangeItem";


        public const string ChangeAbility = "ChangeAbility"; //更换
        public const string GotoChooseCard = "GotoChooseCards"; //进入选卡界面
        // public const string StartPlay = "StartPlay"; //开始音游游戏

        public const string OkChooseCard = "OkChoose"; //确认选择
        public const string CancelChooseCard = "CancelChooseCard"; //取消选择


        //星活动
        public const string StarActivityGetAward = "StarActivityGetAward"; //星活动领取任务奖励
        public const string StarActivityGotoTask = "StarActivityGotoTask"; //星活动前往任务


        //迟郁的生日
        public const string PlayerBirthdayGetAward = "PlayerBirthdayGetAward"; //迟郁的生日活动领取任务奖励
        public const string PlayerBirthdayGotoTask = "PlayerBirthdayGotoTask"; //迟郁的生日前往任务


        public const string ActivityTemplateOnPlay = "ActivityTemplateOnPlay"; //活动模板（国庆活动抽几次）
        public const string ActivityTemplateCardAniOver = "ActivityTemplateCardAniOver"; //活动模板（国庆活动翻卡动画结束）
        public const string ActivityTemplateJumpTo = "ActivityTemplateJumpTo"; //活动模板（国庆活动任务跳转）

        public const string ActivityCapsuleTemplateSendUserInfo = "ActivityCapsuleTemplateSendUserInfo"; //扭蛋活动请求用户信息

        public const string
            ActivityCapsuleTemplateRefreshUserInfo = "ActivityCapsuleTemplateRefreshUserInfo"; //扭蛋活动刷新用户信息

        public const string ActivityCapsuleTemplateWatchStory = "ActivityCapsuleTemplateWatchStory"; //扭蛋活动查看剧情
        public const string ActivityCapsuleTemplateWatchOverStory = "ActivityCapsuleTemplateWatchOverStory"; //扭蛋活动查看完剧情
        public const string ActivityCapsuleTemplateEnterStory = "ActivityCapsuleTemplateEnterStory"; //扭蛋活动进入剧情
        public const string ActivityCapsuleTemplateDraw = "ActivityCapsuleTemplateDraw"; //扭蛋活动扭蛋
        public const string ActivityCapsuleTemplateDrawAnimOver = "ActivityCapsuleTemplateDrawAnimOver"; //扭蛋活动扭蛋动画完成

        public const string EnterCapsuleBattle = "EnterCapsuleBattle"; //进入扭蛋战斗
        public const string ShowLastCapsuleBattleWindow = "ShowLastCapsuleBattleWindow";
        public const string CapsuleBattleOver = "CapsuleBattleOver";
        public const string CapsuleBattleUpdateSupporterNum = "CapsuleBattleUpdateSupporterNum";
        public const string OnClickCapsuleBattleEntrance = "OnClickCapsuleBattleEntrance";

        //SetPanel

        public const string OnClickHead = "OnClickHead";
        public const string OnClickHeadFrame = "OnClickHeadFrame";
        public const string UpdateMainViewHeadInfo = "UpdateMainViewHeadInfo";
        
        //音游活动
        public const string OnClickExchangeBuyBtn = "OnClickExchangeBuyBtn";
        public const string OnClickExchangeItem = "OnClickExchangeItem";
        public const string GetActivityMusicTaskAward = "GetActivityMusicTaskAward";
        public const string ActivityMusicTaskGoto = "ActivityMusicTaskGoto";
        public const string OnClickMusicCapsuleBattleEntrance = "OnClickMusicCapsuleBattleEntrance";

        public const string WatchActivityStoryOver = "WatchActivityStoryOver";
        
        
        //哄睡
        public const string OnClickCoaxSleepPlay = "OnClickCoaxSleepPlay";   //点击播放按钮
        public const string OnClickUnlockToGem = "OnClickUnlockToGem";       //点击通过钻石解锁

        
    }
}