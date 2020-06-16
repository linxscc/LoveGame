#region 模块信息
// **********************************************************************
// Copyright (C) 2018 The 望尘体育科技
//
// 文件名(File Name):             Constants.cs
// 作者(Author):                  张晓宇
// 创建时间(CreateTime):           2018/2/11 16:54:1
// 修改者列表(modifier):
// 模块描述(Module description):
// **********************************************************************
#endregion
using UnityEngine;
using System.Collections;

namespace game.main
{

	public class Constants {

        //Prefab 路径

         
        /*防沉迷相关*/
        public const string CertificationWindow = "Prefabs/AntiAddictionSystem/CertificationWindow";    //实名认证窗口
        public const string SystemMessagesWindow = "Prefabs/AntiAddictionSystem/SystemMessagesWindow";  //系统消息窗口

        public const string RuleExplainWindow = "Prefabs/RuleExplainWindow";
        
        /// <summary>
        /// 确认窗口，有2个按钮
        /// </summary>
	    public const string ConfirmWindowPath = "Prefabs/ConfirmWindow";

	    /// <summary>
	    /// 提示窗，只有确定按钮
	    /// </summary>
	    public const string AlertWindowPath = "Prefabs/AlertWindow";
		public const string DownloadTipsWindowPath = "Prefabs/DownloadTipsWindow";


        public const string BuyWindowPath = "Prefabs/BuyWindow";     //购买窗口的预设物路径

        public const string StarcardWindowPath = "Prefabs/BuyItemWindow";  //购买星卡窗口的预设物路径
        
        
        public const string DownloadingWindowPath = "Prefabs/DownloadingWindow";
		public const string ConfirmChooseWindowPath = "Prefabs/ConfirmChooseWindow";
		public const string ConfirmNoChooseWindowPath = "Prefabs/ConfirmNoChooseWindow";

        public const string RetryWindowPath = "Prefabs/RetryWindow";
        public const string IconSelectWindowPath = "Prefabs/IconSelectWindow";
        //小红点 0表示false，1表示true
        public const string REDPOINT_PHONE_BGVIEW_SMS = "REDPOINT_PHONE_BGVIEW_SMS";
        public const int REDPOINT_PHONE_BGVIEW_SMS_DEFAULT = 0;
        public const string REDPOINT_PHONE_BGVIEW_CALL = "REDPOINT_PHONE_BGVIEW_CALL";
        public const int REDPOINT_PHONE_BGVIEW_CALL_DEFAULT = 0;
        public const string REDPOINT_PHONE_BGVIEW_FC = "REDPOINT_PHONE_BGVIEW_FC";
        public const int REDPOINT_PHONE_BGVIEW_FC_DEFAULT = 0;
        public const string REDPOINT_PHONE_BGVIEW_WEIBO = "REDPOINT_PHONE_BGVIEW_WEIBO";
        public const int REDPOINT_PHONE_BGVIEW_WEIBO_DEFAULT = 0;
        public const string REDPOINT_LOVE_BTN_LOVEAPPOINT = "REDPOINT_LOVE_BTN_LOVEAPPOINT";
		public const string ENCOURAGE_ACT = "ENCOURAGE_ACT_MSG_KEY";//应援活动
        public const string REDPOINT_YUYINSHOUCANG = "REDPOINT_YUYINSHOUCANG";//语音收藏


        public const string CARD_MSG_KEY = "CARD_MSG_KEY";    //星缘
        public const string PHONE_MSG_KEY = "PHONE_MSG_KEY";    //手机
        public const string LOVE_MSG_KEY = "LOVE_MSG_KEY";      //约会
        public const string DRAW_MSG_KEY = "DRAW_MSG_KEY";      //抽卡
        public const string SUPPORTER_MSG_KEY = "SUPPORTER_MSG_KEY"; 
        public const string MISSION_MSG_KEY = "MISSION_MSG_KEY"; //日常和周长任务
		public const string MISSION_STAR_ROAD_KEY = "MISSION_STAR_ROAD_KEY"; //星路历程
		public const string MISSION_STARRY_COVENANT = "MISSION_STARRY_COVENANT"; //星动之约
		public const string MISSION_CHI_YU_BIRTHDAY = "MISSION_CHI_YU_BIRTHDAY";//迟郁的生日
		
        public const string MAIL_MSG_KEY = "MAIL_MSG_KEY";   //邮件
        public const string ACTIVITY_MSG_KEY = "ACTIVITY_MSG_KEY";  //活动
		public const string ACTIVITY_MSG_FIRSTRECHARGE = "ACTIVITY_MSG_FIRSTRECHARGE";    //活动首充
		public const string FRIEND_MSG_KEY = "FRIEND_MSG_KEY"; //好友
        public const string USERCENTER_MSG_KEY = "USERCENTER_MSG_KEY";
		public const string DAILY_PACKAGE_MSG_KEY = "DAILY_PACKAGE_MSG_KEY";//每日免费礼包小红点
		public const string SHOP_FREEMALL_MSG_KEY = "SHOP_FREEMALL_MSG_KEY";//每日免费礼包小红点
        public const string ACTIVITY_ACTIVITY_MISSION = "ACTIVITY_ACTIVITY_MISSION";//抽卡活动模版
        public const string CHINACURRENCY = "CNY";//中国内陆货币符号


		public const string ACTIVITY_DRAW_TEMPLATE = "ACTIVITY_DRAW_TEMPLATE";  //抽奖模板
        public const string ACTIVITY_CAPSULE_TEMPLATE = "ACTIVITY_CAPSULE_TEMPLATE";  //扭蛋抽奖模板
        public const string ACTIVITY_MUSIC = "ACTIVITY_MUSIC";//音游模板
        public const string ACTIVITY_MISSION = "ACTIVITY_MISSION";  /**抽卡活动模版(限时十连)*/
	}
}
