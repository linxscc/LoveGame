using System.Runtime.InteropServices;

namespace Assets.Scripts.Module
{
    namespace NetWork
    {
        public class CMD
        {
            /*****************************************选服************************************************************/
            public const string GAMESERVERC_GAMESERVER = "gameServerC/gameServer"; //游戏版本服务器
            
            /*****************************************通用***********************************************************/
            public const string USERC_GEMEXCHANGE = "userC/gemExchange"; //购买金币或体力

            public const string USERC_USERRULE = "userC/UserRule"; //用户一些规则

            public const string USERC_USERMODIFYNAME = "userC/userModifyName";//修改用户名

            public const string USERC_REPLACEUSERAVATARORBOX = "userC/replaceUserAvatarOrBox"; // 玩家更换头像头像框
            
            public const string USERC_EXCHANGECODE = "userC/exchangeCode";   //兑换码

            public const string USERC_ANTIADDICTION = "userC/antiAddiction";//防沉迷
            /*****************************************新手引导***********************************************************/
            public const string USERC_GUIDE = "userC/guide";  //设置引导步骤到服务器
            
            
            /****************************************************登陆************************************************/
            public const string LOGINC_LOGIN = "loginC/login"; //游戏服务器登陆
            public const string CREATE_USER = "userC/createUser"; //创建角色
            public const string UPDATE_USER_BIRTHDAY = "userC/updateUserBirthday"; //更新玩家生日
            public const string USER_LOGIN = "userC/userLogin"; //玩家登陆
            public const string GAME_CONFIG = "gameConfigC/rules"; //配置       
            public const string USERC_ACTIVATIONACCOUNT = "userC/activationAccount"; //激活码     

            /****************************************************应援会************************************************/
            public const string DEPARTMENTC_MY_DEPARTMENT = "departmentC/myDepartments"; //应援会
            public const string DEPARTMENTC_DEPARTMENT_RULE = "departmentC/departmentRules"; //应援会规则
            public const string DEPARTMENTC_UPGRADEDEPARTMENTS = "departmentC/upgradeDepartments"; //升级应援会
            public const string DEPARTMENTC_RECEIVEAWARD = "departmentC/departmentAward";//领取奖励！
            

            /****************************************************好友************************************************/
            public const string FRIEND_RULES = "friendC/rules"; //获取好友规则
            public const string FRIEND_FRIENDS = "friendC/friends"; //获取用户好友信息
            public const string FRIEND_GETPOWER = "friendC/getPower";//领取体力
            public const string FRIEND_SENDPOWER = "friendC/sendPower";//送体力
            public const string FRIEND_GETALLPOWER = "friendC/getPowerAll";//一键领取
            public const string FRIEND_SENDALLPOWER = "friendC/sendAllPower";//一键赠送
            public const string FRIEND_DELFRIEND = "friendC/del";//删除好友
            public const string FRIEND_COMMENTS = "friendC/comments";//获取好友推荐信息
            public const string FRIEND_DOAPPLY = "friendC/doApply";//发起好友申请
            public const string FRIEND_APPLY = "friendC/applys";//获取好友申请信息
            public const string FRIEND_IGNORE = "friendC/ignore";//忽略好友申请
            public const string FRIEND_AGREE = "friendC/agree";//同意好友申请
            public const string FRIEND_SEARCH = "friendC/search";//查找好友
            

            /****************************************************卡牌************************************************/
            public const string CARDC_CARDRULE = "cardC/cardRule"; //卡规则
            public const string CARDC_CARDS = "cardC/cards"; //卡基础信息
            public const string CARDC_MYCARD = "cardC/myCard"; //玩家的卡
            public const string CARDC_MYPUZZLE = "cardC/myPuzzle"; //卡碎片
            public const string CARDC_CHOOSEEVO = "cardC/chooseEvo";
            public const string CARDC_MYSIGNASTURES = "cardC/mySignatures";//用户签名道具列表

            /****************************************************抽卡************************************************/
            public const string DRAWC_DRAWINFO = "drawC/drawInfo"; //获取抽卡信息
            public const string DRAWC_DRAW = "drawC/draw"; //抽卡
            public const string DRAWC_DRAW_PROBS = "drawC/drawProbs"; //卡池信息

            /****************************************************道具************************************************/
            public const string ITEMC_ITEMS = "itemC/items"; //道具基础信息
            public const string ITEMC_MYITEM = "itemC/myItem"; //玩家的道具
            public const string ITEMC_BUYITEM = "itemC/buyItem";  //购买商品
 

            /****************************************************主线关卡************************************************/
            public const string CAREERC_LEVELS = "careerC/levels"; //关卡基础数据
            public const string CAREERC_MYLEVELS = "careerC/myLevels"; //玩家关卡信息

            public const string CAREERC_CHALLENGE = "careerC/challenge"; //战斗

            public const string CAREERC_SWEEPRES = "careerC/sweepRes"; //扫荡
            
            public const string CAREERC_BUY_COUNT = "careerC/buyCount"; //购买扫荡次数


            /****************************************************星缘回忆************************************************/
            public const string CARDMEMORIESC_CARDMEMORIESRULE = "cardMemoriesC/cardMemoriesRule"; //星缘回忆规则
            public const string CARDMEMORIESC_CARDMEMORIESINFO = "cardMemoriesC/cardMemoriesInfo"; //星缘回忆信息
            public const string CARDMEMORIESC_BUYMEMORIESCONSUME = "cardMemoriesC/buyMemoriesConsume"; //星缘回忆购买
            public const string CARDMEMORIESC_CHALLENGECARDMEMORIES = "cardMemoriesC/challengeCardMemories"; //星缘回忆挑战
            public const string CARDMEMORIESC_CARDMEMORIESMISSIONRECEIVE = "cardMemoriesC/cardMemoriesMissionReceive"; //领取星缘回忆任务奖励'
            
            /****************************************************星缘************************************************/
            public const string CARDC_LEVELUP = "cardC/addExp"; //升级
            public const string CARDC_STARUP = "cardC/starUp"; //升星
            public const string CARDC_EVOLUTION = "cardC/evolution"; //进化
            public const string CARDC_RESOLVE = "cardC/resolve"; //分解(回溯)
            public const string CARDC_COMPOUND = "cardC/compound"; //合成
            public const string CARDC_LEVELONE = "cardC/upgrade"; //升一级


            /****************************************************手机************************************************/
            public const string PHONEC_RULES = "phoneC/rules"; //规则
            public const string PHONEC_MSGORCALL = "phoneC/msgOrCall"; //用户已触发的短信/电话信息
            public const string PHONEC_READ_MSGORCALL = "phoneC/readMsgOrCall"; //用户短信/电话读完时记录
            public const string PHONEC_LISTEN_MSGORCALL = "phoneC/listenMsgOrCall"; //用户短信/电话语言听语音时记录 
            public const string PHONEC_FRIENDCIRCLE = "phoneC/FC"; //用户已触发的朋友圈信息
            public const string PHONEC_FRIENDCIRCLE_PUBLISH = "phoneC/pubFC"; //发布朋友圈 
            public const string PHONEC_FRIENDCIRCLE_COMMENT = "phoneC/commentFC"; //评论朋友圈 
            public const string PHONEC_WEIBO = "phoneC/microBlogs"; //用户已触发的微博信息
            public const string PHONEC_WEIBO_LIKE = "phoneC/like"; //点赞微博  phoneC/like
            public const string PHONEC_WEIBO_PUBLISH = "phoneC/pubMicroBlog"; //发布微博  phoneC/pubMicroBlog
            public const string PHONEC_USERDATA_ALL = "phoneC/userPhoneData"; //拉取所有用户电话数据    phoneC/userPhoneData
            public const string PHONEC_TIPS_DATA = "phoneC/userPhoneTipData"; //拉取所有用户需要弹窗的电话数据   phoneC/userPhoneTipData
            public const string PHONEC_ELIMINAT_ESCENE = "phoneC/eliminatEscene";// /phoneC/eliminatEscene', '消除事件弹窗 

            /***************************************************NPC******************************************/
            public const string NPC_RULES = "npcC/npcs"; //规则
            public const string NPC_SETSTATE = "npcC/bgState"; //更换背景
            public const string NPC_GETUSERNPC = "npcC/userNpcs"; //获取玩家NPC数据
            public const string NPC_STAGEINTERACT = "npcC/stageInteract";  //主界面互动加好感度

            /*************************************************日常任务******************************************/
            public const string MISSION_RULES = "missionC/missionRules"; //规则
            public const string MISSION_MYMISSION = "missionC/userMissions"; //获取用户信息
            public const string MISSION_AWARDS = "missionC/awards"; //领取任务奖励
            public const string MISSION_REFRESH = "missionC/refresh"; //隔天刷新任务
            public const string MISSION_ACTIVITYAWARDS = "missionC/activityAwards"; //领取活跃奖励

            /*************************************************登陆奖励******************************************/
            //public const string ACTIVITYC_ACTIVITYLIST = "activityC/activityList"; //活动列表
            //public const string ACTIVITYC_SIGNIN = "activityC/signIn"; //领取十天奖励

            /*************************************************活动******************************************/
            public const string ACTIVITY_RULES = "activityC/activityRules";   //活动规则
            public const string ACTIVITY_ACTIVITYRULELIST = "activityStencilC/activityRuleList";   //活动任务规则
            public const string ACTIVITY_GAINACTIVITYMISSIONAWARDS = "activityStencilC/gainActivityMissionAwards";   //领取活动任务奖励
            public const string ACTIVITY_GAINSEVENDAYAWARD = "activityC/gainSevenDayAward";  //获取七天签到奖励
            public const string ACTIVITY_ACTIVITYLIST = "activityC/activityList"; //活动列表
            public const string ACTIVITY_ACTIVITYLISTS2 = "activityStencilC/activityLists";//活动列表2（放的是：活动任务，活动兑换商店的用户信息）
            public const string ACTIVITY_SIGNIN = "activityC/signIn"; //领取十天奖励
            public const string ACTIVITY_GET_POWER = "activityC/getPower";  //每日体力领取奖励
            public const string ACTIVITY_BUY_GET_POWER = "activityC/buyGetPower";//每日体力补领奖励
            public const string ACTIVITY_MONTH_SING_REWARD = "activityC/monthSignReward"; // 月签到
            public const string ACTIVITY_MONTH_SING_BUY = "activityC/monthSignBuy";// 月签到补签
            public const string ACTIVITY_MONTH_SING_EXTRA = "activityC/monthSignExtra"; // 月签到领取累计奖励
            public const string ACTIVITY_RECEIVE_FIRSTPRIZE = "activityC/receiveFirstPrize"; //领取首冲奖励
            public const string ACTIVITY_GETGROWTHFUND = "activityC/growthFundAward";//领取成长基金奖励
            public const string ACTIVITY_GETACCUMULATIVERECHARGE = "activityC/receiveActivityAccumulativeRechargeAwards";//累计充值领取奖励
            public const string ACTIVITY_ACTIVITYEXCHANGEMALL = "activityStencilC/activityExchangeMall";

            public const string ACTIVITY_ACTIVITYMUSICINFO = "activityC/activityMusicInfo";
         
            //活动兑换商城
            /*************************************************小红点******************************************/
            public const string MAINPANELVIEW_RED_POINT = "msgC/msgs"; //主场景界面小红点


    
            /*************************************************恋爱约会******************************************/
            public const string APPOINTMENT_RULES = "appointmentC/rules"; //规则
            public const string APPOINTMENT_USERAPPOINTMENTS = "appointmentC/userAppointments"; //用户约会数据
            public const string APPOINTMENT_ACTIVE = "appointmentC/active"; //激活约会
            public const string APPOINTMENT_OPENGATE = "appointmentC/openGate"; //解锁约会关卡
            public const string APPOINTMENT_PASSGATE = "appointmentC/passGate"; //通关关卡
            public const string APPOINTMENT_PHOTOCLEAR = "appointmentC/photoClear"; //拍立得交互完成
            public const string APPOINTMENT_PHOTONICKUP = "appointmentC/photoNickUp"; //拍立得钉起来

            /*************************************************恋爱日记******************************************/
            public const string DIARYC_ELEMENTS_RULES = "elementC/elements"; //素材规则 
            public const string DIARYC_ELEMENTS_USER = "elementC/userElements";//用户解锁的素材列表 
            public const string DIARYC_ELEMENTS_BUY = "elementC/buyElement";//购买素材 

            public const string DIARYC_USER_DIARYSTATE = "diaryC/userDiaryState";//用户日记状态列表（按月拉取）
            public const string DIARYC_USER_DIARYDETAIL = "diaryC/userDiaryDetail";//用户日记具体信息
            public const string DIARYC_SAVEDIARY = "diaryC/saveDiary";//保存日记 

            /*************************************************探班******************************************/
            public const string VISITINGC_RULE = "visitingC/rules";//规则
            public const string VISITINGC_MYVISITINGS = "visitingC/myVisitings";//用户数据
            public const string VISITINGC_BLESS = "visitingC/bless";//祈福
            public const string VISITINGC_CHALLENGE = "visitingC/challenge"; //战斗
            public const string VISITINGC_SWEEPRES = "visitingC/sweepRes"; //扫荡      
            public const string VISITINGC_GETFIRSTPASSAWARDS = "visitingC/getFirstPassAwards";//领取首通奖励
            public const string VISITINGC_RESETLEVEL = "visitingC/resetLevel";//重置挑战次数

            /*************************************************应援活动******************************************/
            public const string SUPPORTERACTIVITY_ENCOURAGEACTRULES = "encourageActC/rules";  //规则
            public const string SUPPORTERACTIVITY_MYENCOURAGEACTS = "encourageActC/acts";    //应援活动列表
            public const string SUPPORTERACTIVITY_REFRESH = "encourageActC/refresh";         //刷新应援活动
            public const string SUPPORTERACTIVITY_START = "encourageActC/start";            //应援活动开始
            public const string SUPPORTERACTIVITY_GETAWARD = "encourageActC/getAward";        //领奖
            public const string SUPPORTERACTIVITY_DONEIMMEDIATE = "encourageActC/doneImmediate";    //立即完成
            public const string SUPPORTERACTIVITY_BUYINNER = "encourageActC/buyInter"; //购买参与次数


            /*************************************************好感度******************************************/
            public const string FAVORABILITY_RULE = "favorabilityC/favorabilityRule"; //好感度規則
            public const string FAVORABILITY_INFO = "favorabilityC/favorabilityInfo"; //角色好感度信息
            public const string FAVORABILITY_TOUCHROLE = "favorabilityC/touchRole";   //触摸版娘增加好感度
            public const string FAVORABILITY_UPGRADEFAVORABILITYLEVEL = "favorabilityC/upgradeFavorabilityLevel";           //升级好感度
            public const string FAVORABILITY_DRESSUP = "favorabilityC/dressUp";        //換裝

            /*************************************************支付******************************************/
            public const string RECHARGEC_RECHARGERULE = "rechargeC/rechargeRule"; //产品列表
            public const string RECHARGEC_CREATEORDER = "rechargeC/createOrder"; //创建订单
            public const string RECHARGEC_CHECKORDER = "rechargeC/checkOrder"; //检查订单
            public const string RECHARGEC_CHECKAPPLEORDER = "rechargeC/checkAppleOrder"; //检查苹果订单
            public const string RECHARGEC_CHECKTXBALANCES = "rechargeC/checkTxBalances"; //获取腾讯米大师余额

            /*************************************************邮件******************************************/
            public const string MAIL_USERMAILS = "mailC/userMails";    //获取用户邮件信息
            public const string MAIL_READ = "mailC/read";              //读邮件
            public const string MAIL_CLEAR = "mailC/clear";            //删除所有邮件
            public const string MAIL_GETALL = "mailC/getAll";          //一键领取奖励
            public const string MAIL_GOTTEN = "mailC/gotten";          //有附件的接口
            public const string MAIL_CLEARONE = "mailC/clearOne";       //删除一封邮件
            /*************************************************商城******************************************/
            public const string MALL_RULE = "mallC/mallRule";//商城规则
            public const string MALL_USERINFO = "mallC/mallInfo";//获取用户商城规则
            public const string MALL_REFRESHGOLDMALL = "mallC/refreshGoldMall";//刷新金币商店
            public const string MALL_BUYGAMEGOODS = "mallC/buyGameGoods";         //购买游戏货币商品
            public const string MonthCard_UseTasteCard = "itemC/useVipExperience";    //使用月卡体验卡
            public const string MonthCard_ReveiveDailyGem = "mallC/receiveMonthCardGem";  //领取VIP每日钻石奖励
            public const string USER_GOTDAILYPACKAGE = "userC/gotDailyPackage"; //领取每日礼包
            /*************************************************分享******************************************/
            public const string USERC_SHAREREWARD= "userC/shareReward"; //领取分享奖励 

            /*************************************************空降小游戏******************************************/
            public const string LITTLEGAMEC_GAMEJUMPINFOS = "littleGameC/gameJumpInfos";  //规则和数据一起请求    
            public const string LITTLEGAMEC_GAMEJUMPSTART = "littleGameC/gameJumpStart";   //游戏开始 
            public const string LITTLEGAMEC_GAMEJUMPEND = "littleGameC/gameJumpEnd";   //游戏结束

            /*************************************************拍照小游戏******************************************/
            public const string TAKEPHOTOC_RULES = "takePhotoC/rules"; //规则
            public const string TAKEPHOTOC_GETINFO = "takePhotoC/getInfo"; //用户信息
            public const string TAKEPHOTOC_STARTTAKEPHOTO = "takePhotoC/startTakePhoto"; //用户信息
            public const string TAKEPHOTOC_SCORE = "takePhotoC/score"; //结果
            public const string TAKEPHOTOC_BUYCOUNT = "takePhotoC/buyCount"; //购买次数

            /*******************************************触发式礼包**************************************************/
            public const string MALLC_TRIGGERGIFT = "mallC/triggerGift";                      //触发礼包
            public const string MALLC_RECEIVEFREETRIGGERGIFT = "mallC/receiveFreeTriggerGift"; //领取免费触发礼包道具


            public const string ACTIVITYC_CHALLENGEACTIVITYMUSIC = "activityC/challengeActivityMusic";//挑战活动音游信息


            /*************************************************练习室******************************************/
            public const string MUSICGAMEC_OPENMUSICGAME = "musicGameC/openMusicGame";//打开音游
            public const string MUSICGAMEC_CONCERT = "musicGameC/concert";            //打开音游活动信息
            public const string MUSICGAMEC_REFRECHMUSIC = "musicGameC/refreshMusic";  //刷新音游活动
            public const string MUSICGAMEC_PALYING = "musicGameC/playing";            //玩音游游戏
            public const string MUSICGAMEC_ENDPLAYING = "musicGameC/endPlaying";      //音游游戏结束请求
            public const string MUSICGAMEC_MALL = "musicGameC/mall";                  //打开商店 商店信息
            public const string MUSICGAMEC_REFRESHMALL = "musicGameC/refreshMall";     //刷新商店
            public const string MUSICGAMEC_SHOPPING = "musicGameC/shopping";           //购买商店物品
            public const string MUSICGAMEC_SENDRANK = "musicGameC/sendRank";           //发送音游排行
            public const string MUSICGAMEC_RULES = "musicGameC/rules";                 //音乐规则
            
            /*************************************************商店评分******************************************/
            public const string STORESCORE_CANCOMMENT = "storeEvaluationC/canComment";//是否可以评论
            public const string STORESCORE_Comment = "storeEvaluationC/comment";//是否可以评论
            
            /*************************************************下载******************************************/
            public const string DOWNLOAD_RECEIVEAWARD = "activityC/receiveDownloadAwards";//首次下载可以领奖

            /*************************************************假期活动******************************************/
           public const string ACTIVITYC_GETHOLIDAYINFO = "activityC/getHolidayInfo";//假日活动用户信息
           public const string ACTIVITYC_GAINACTIVEHOLIDAYAWARD  ="activityC/gainActiveHolidayAward";//假期活动领取活跃奖励
           public const string ACTIVITYC_DRAWAWARDS = "activityC/drawAwards"; //假期活动抽卡
           public const string ACTIVITYC_ACTIVITYENDMAIL = "activityC/activityEndMail";//假期活动结束

            /*************************************************扭蛋活动模板******************************************/

            public const string ACTIVITYC_CAPSULE_RULE = "activityCapsuleC/rules";  //扭蛋活动模板规则
            public const string ACTIVITYC_CAPSULE_INFO = "activityCapsuleC/getActivityInfo";  //扭蛋活动用户信息
            public const string ACTIVITYC_CAPSULE_WATCH_DRAMA = "activityCapsuleC/watchingDrama";  //扭蛋活动看剧情
            public const string ACTIVITYC_CAPSULE_WATCH_OVER_DRAMA = "activityCapsuleC/watchOverDrama";  //扭蛋活动看完剧情
            public const string ACTIVITYC_CAPSULE_DRAW_AWARD = "activityCapsuleC/drawAward";  //扭蛋活动扭蛋

            public const string ACTIVITYSTENCILC_CHALLENGE = "activityStencilC/challenge";//挑战扭蛋副本
            public const string ACTIVITYCAPSULEC_SWEEP = "activityStencilC/sweep";//扫荡 
            public const string ACTIVITYSTENCILC_BUYCOUNT = "activityStencilC/buyCount"; //购买关卡次数
            
            /*************************************************看剧情活动模板******************************************/
            public const string ACTIVITYC_WATCH_STORY = "activityStencilC/watchActivityPlot";  //扭蛋活动看剧情
            
            /*************************************************活动模板任务领取******************************************/
            public const string ACTIVITYC_GET_AWARDS = "activityStencilC/gainActivityMissionAwards";
            
            /*************************************************哄睡******************************************/
            public const string COAXSLEEPC_COAXSLEEPRULES = "coaxSleepC/coaxSleepRules"; //哄睡规则
            public const string COAXSLEEPC_COXAXSLEEPINFOS = "coaxSleepC/coaxSleepInfos";//哄睡玩家信息
            public const string COAXSLEEPC_UNLOCKCOAXSLEEPAUDIO = "coaxSleepC/unlockCoaxSleepAudio";//解锁哄睡音频
            
            /*************************************************实名认证******************************************/
            public const string USERC_RECEIVEUSEREXTRAAWARDS = "userC/receiveUserExtraAwards";  //领取用户奖励
            
        }

    }
}