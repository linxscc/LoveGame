

namespace game.main
{
    public class GameConfigKey
    {

        /** 生涯 普通关卡消耗体力 */
        public const string CAREER_POWER_NOMAL = "CAREER_POWER_NOMAL";
        /** 生涯 精英关卡消耗体力 */
        public const string CAREER_POWER_ELITE = "CAREER_POWER_ELITE";

        /** 抽卡道具id 金币抽卡消耗 */
        public const string DRAW_GOLD = "DRAW_GOLD";
        /** 抽卡道具id 钻石抽卡消耗 */
        public const string DRAW_GEM = "DRAW_GEM";
        /** 抽卡道具id 金币10连抽卡消耗 */
        public const string DRAW_GOLD_10 = "DRAW_GOLD_10";
        /** 抽卡道具id 钻石10连抽卡消耗 */
        public const string DRAW_GEM_10 = "DRAW_GEM_10";
        /** 抽卡道具id 用道具金币10连抽卡消耗 */
        public const string DRAW_GOLD_ITEM_10 = "DRAW_GOLD_ITEM_10";
        /** 抽卡道具id 用道具钻石10连抽卡消耗 */
        public const string DRAW_GEM_ITEM_10 = "DRAW_GEM_ITEM_10";
        /** 抽卡道具id 用于金币抽卡，1张券抵扣一次 */
        public const string DRAW_GOLD_ITEM_ID = "DRAW_GOLD_ITEM_ID";
        /** 抽卡道具id 用于宝石抽卡，1张券抵扣一次 */
        public const string DRAW_GEM_ITEM_ID = "DRAW_GEM_ITEM_ID";
        /** 人物对应进化道具id 唐弋辰 */
        public const string EVO_ITEM_ID_TANG_YI_CHEN = "EVO_ITEM_ID_TANG_YI_CHEN";
        /** 人物对应进化道具id 秦予哲 */
        public const string EVO_ITEM_ID_QIN_YU_ZHE = "EVO_ITEM_ID_QIN_YU_ZHE";
        /** 人物对应进化道具id 言季 */
        public const string EVO_ITEM_ID_YAN_JI = "EVO_ITEM_ID_YAN_JI";
        /** 人物对应进化道具id 迟郁 */
        public const string EVO_ITEM_ID_CHI_YU = "EVO_ITEM_ID_CHI_YU";

        /** 升级道具 活跃 */
        public const string DEPARTMENT_ACTIVE_UPGRADE_ITEM_ID = "DEPARTMENT_ACTIVE_UPGRADE_ITEM_ID";
        /** 升级道具 财力 */
        public const string DEPARTMENT_FINANCIAL_UPGRADE_ITEM_ID = "DEPARTMENT_FINANCIAL_UPGRADE_ITEM_ID";
        /** 升级道具 资源 */
        public const string DEPARTMENT_RESOURCE_UPGRADE_ITEM_ID = "DEPARTMENT_RESOURCE_UPGRADE_ITEM_ID";
        /** 升级道具 传播力 */
        public const string DEPARTMENT_TRANSMISSION_UPGRADE_ITEM_ID = "DEPARTMENT_TRANSMISSION_UPGRADE_ITEM_ID";

        /**每日向同一名好友领取的体力*/
        public const string FRIEND_DAILY_GET_POWER_ID = "FRIEND_DAILY_GET_POWER";
        /**每日可领取体力上限*/
        public const string FRIEND_DAILY_MAX_POWER_ID = "FRIEND_DAILY_MAX_POWER";
        /**超过7天未登录的统一展示“7天前” 这里就是7*/
        public const string FRIEND_LOG_DAY_MAX_ID = "FRIEND_LOG_DAY_MAX";
        /**好友上限*/
        public const string FRIEND_MAX_NUM_ID = "FRIEND_MAX_NUM";
        /**推荐刷新间隔，单位秒 传60*/
        public const string FRIEND_SEARCH_INTERVAL_ID = "FRIEND_SEARCH_INTERVAL";
        /**好友申请列表上限*/
        public const string FRIEND_APPLY_MAX_NUM = "FRIEND_APPLY_MAX_NUM";
        /**好友推荐卡牌收集率低的数量*/
        public const string FRIEND_LOWER_COMMENT_NUM = "FRIEND_LOWER_COMMENT_NUM";
        /**好友推荐卡牌收集率高的数量*/
        public const string FRIEND_HIGHER_COMMENT_NUM = "FRIEND_HIGHER_COMMENT_NUM";
        /**推荐最近几天登录的玩家*/
        public const string FRIEND_COMMNET_DAYS = "FRIEND_COMMNET_DAYS";

        /**球队名字符长度下限*/
        public const string USER_NAME_LENGE_MIN = "USER_NAME_LENGE_MIN";
        /**球队名字符长度上限*/
        public const string USER_NAME_LENGE_MAX = "USER_NAME_LENGE_MAX";

        /**恢复一点用户体力时间(分钟)*/
        public const string RESTORE_INFO_POWER_ONE_TIME = "RESTORE_INFO_POWER_ONE_TIME";
        /**恢复一点应援活动体力时间(分钟)*/
        public const string RESTORE_ENCOURAGE_ACT_POWER_ONE_TIME = "RESTORE_ENCOURAGE_ACT_POWER_ONE_TIME";
        /**恢复应援活动的体力最大值*/
        public const string RESTORE_ENCOURAGE_ACT_POWER_MAX_SIZE = "RESTORE_ENCOURAGE_ACT_POWER_MAX_SIZE";
        /**应援活动立即完成消耗上限*/
        public const string ENCOURAGE_ACT_GEM_MAX = "ENCOURAGE_ACT_GEM_MAX";
        /**应援活动立即完成系数X%*/
        public const string ENCOURAGE_ACT_GEM_RADIO = "ENCOURAGE_ACT_GEM_RADIO";
        /**金币抽卡免费时间(分钟)*/
        public const string GOLD_DRAW_FREE_REFRESH_TIME = "GOLD_DRAW_FREE_REFRESH_TIME";
        /**宝石抽卡免费时间(分钟)*/
        public const string GEM_DRAW_FREE_REFRESH_TIME = "GEM_DRAW_FREE_REFRESH_TIME";

        /**每日重置各个活动数据的时间点*/
        public const string DAILY_RESET_HOUR = "DAILY_RESET_HOUR";

        public const string DRAW_TOTAL_NUM = "DRAW_TOTAL_NUM";

        /**恢复一点星缘回忆体力时间(分钟)*/
        public const string RESTORE_MEMORIES_POWER_ONE_TIME = "RESTORE_MEMORIES_POWER_ONE_TIME";
        /**星缘回忆一次购买体力值*/
        public const string RESTORE_MEMORIES_POWER_BUY_VALUE = "RESTORE_MEMORIES_POWER_BUY_VALUE";
        /**星缘回忆体力最大值*/   //这个是3。
        public const string MEMORIES_CHALLENGE_NUM_MAX = "MEMORIES_CHALLENGE_NUM_MAX";
        
        /**星缘回忆体力最大值*/
        public const string  RESTORE_MEMORIES_POWER_MAX_SIZE = "RESTORE_MEMORIES_POWER_MAX_SIZE";
        
        /**星缘回忆卡牌挑战消耗体力值*/
        public const string MEMORIES_CHALLENGE_CONSUME_POEWR = "MEMORIES_CHALLENGE_CONSUME_POEWR";
        /**星缘回忆卡牌挑战消耗金币值*/
        public const string MEMORIES_CHALLENGE_CONSUME_GOLD = "MEMORIES_CHALLENGE_CONSUME_GOLD";

        /**星缘回忆重置道具Id*/
        public const string RESTORE_MEMORIES_RESET_ITEM_ID = "RESTORE_MEMORIES_RESET_ITEM_ID";

        /**触摸版娘好感度经验*/
        public const string FAVORABILITY_TOUCH_ROLE_EXP = "FAVORABILITY_TOUCH_ROLE_EXP";
        /**触摸版娘好感度次数上限*/
        public const string FAVORABILITY_TOUCH_ROLE_NUM_MAX = "FAVORABILITY_TOUCH_ROLE_NUM_MAX";


        /**主界面交互，每次互动增加好感度*/
        public static string MAIN_STAGE_INTERACT_ADD_FAVOR = "MAIN_STAGE_INTERACT_ADD_FAVOR";
        /**主界面交互，每日互动次数上限*/
        public static string MAIN_STAGE_INTERACT_MAX_CNT = "MAIN_STAGE_INTERACT_MAX_CNT";
        /**每天第一次保存日记增加的好感度*/
        public static string DIARY_ADD_FAVOR = "DIARY_ADD_FAVOR";
        
        /**升级应援会等级获得体力*/
        public static string UPGRADE_DEPARTMENT_POWER_NUM = "UPGRADE_DEPARTMENT_POWER_NUM";

        /**最多可以设置闹钟的个数*/
        public static string CLOCK_TOTAL_COUNT = "CLOCK_TOTAL_COUNT";

        /**恋爱日记文本长度上限*/
        public static string DIARY_MAX_TEXT_LEN = "DIARY_MAX_TEXT_LEN";
        /**恋爱日记模板个数上限*/
        public static string DIARY_MAX_TEMPLATE_COUNT = "DIARY_MAX_TEMPLATE_COUNT";
        /**恋爱日记标签个数上限*/
        public static string DIARY_MAX_LABEL_COUNT = "DIARY_MAX_LABEL_COUNT";
        /**恋爱日记贴纸个数上限*/
        public static string DIARY_MAX_IMAG_COUNT = "DIARY_MAX_IMAG_COUNT";
        /**恋爱日记文本框个数上限*/
        public static string DIARY_MAX_TEXT_COUNT = "DIARY_MAX_TEXT_COUNT";
        /**恋爱日记拍立得个数上限*/
        public static string DIARY_MAX_RACKET_COUNT = "DIARY_MAX_RACKET_COUNT";
        /**恋爱日记背景个数上限*/
        public static string DIARY_MAX_BG_COUNT = "DIARY_MAX_BG_COUNT";
        
        /**商城月卡天数*/
        public static string MONTH_CARD_DAY_NUM = "MONTH_CARD_DAY_NUM";

        /**商城月卡特权每天领取宝石*/
        public static string MONTH_CARD_GEM_NUM  = "MONTH_CARD_GEM_NUM";
        
        /**商城月卡特权关卡经验增加数值*/
        public static string MONTH_CARD_LEVEL_EXP_NUM   = "MONTH_CARD_LEVEL_EXP_NUM";

        /**商城月卡特权体力上限数值*/
        public static string MONTH_CARD_POWER_NUM   = "MONTH_CARD_POWER_NUM";

        
        /**唐弋辰好感度换装默认服装*/
        public static string TANG_YI_CHEN_CLOTHES = "TANG_YI_CHEN_CLOTHES";
        
        /**秦予哲好感度换装默认服装*/
        public static string  QIN_YU_ZHE_CLOTHES = "QIN_YU_ZHE_CLOTHES";
        
        /**言季好感度换装默认服装*/
        public static string  YAN_JI_CLOTHES = "YAN_JI_CLOTHES";
        
        /**迟郁好感度换装默认服装*/
        public static string  CHI_YU_CLOTHES = "CHI_YU_CLOTHES";
        
        
        /*星动之约活动天数*/
        public static string SEVEN_DAYS_CARNIVAL_OPEN_DAYS = "SEVEN_DAYS_CARNIVAL_OPEN_DAYS";
        /*探班单个关卡次数上限*/
        public static string VISITING_LEVEL_MAX_TIMES = "VISITING_LEVEL_MAX_TIMES";

        /*生日活动天数*/
        public static string PLAYER_BIRTHDAY_TIME = "PLAYER_BIRTHDAY_TIME";
        
        /*生日活动提前展示活动窗口天数*/
        public static string BEFORE_BIRTHDAY_DAYS = "BEFORE_BIRTHDAY_DAYS";

        /*假期活动道具Id*/
        public static string HOLIDAY_ACTIVITY_ITEM_CONSUME = "HOLIDAY_ACTIVITY_ITEM_CONSUME";
        
        /*月签开始的天数*/
        public static string MONTH_SIGN_RESET_DAY = "MONTH_SIGN_RESET_DAY";

        /*拍照特写练习*/
        public static string TAKE_PHOTO_COUNT = "TAKE_PHOTO_COUNT";
        
        /** 用户新星动之约判定时间*/
        public static  string NEW_STARRY_COVENANT_DECISION_TIME= "NEW_STARRY_COVENANT_DECISION_TIME";
        
        /** 扭蛋活动每日副本第一次购买赠送次数*/
        public static string  ACTIVITY_CAPSULE_COPY_DAILY_FREE_COUNT ="ACTIVITY_CAPSULE_COPY_DAILY_FREE_COUNT";
        
        /** 扭蛋活动购买一次次数消耗宝石数*/
        public static  string  ACTIVITY_CAPSULE_COPY_BUY_COUNT_CONSUME_GEM = "ACTIVITY_CAPSULE_COPY_BUY_COUNT_CONSUME_GEM";
        
        /** 扭蛋活动每日购买挑战次数上限*/
        public static string  ACTIVITY_CAPSULE_COPY_BUY_COUNT_LIMIT = "ACTIVITY_CAPSULE_COPY_BUY_COUNT_LIMIT";
    
        /** 活动抽卡10连活动新老玩家判断时间*/
        public static string NEW_ACTIVITY_DRAW_TEN_CONTINUOUS_TIME = "NEW_ACTIVITY_DRAW_TEN_CONTINUOUS_TIME";
            
        /** 活动抽卡10连活动老玩家允许触发开始时间*/
        public static string OLD_ACTIVITY_DRAW_TEN_CONTINUOUS_START_TIME = "OLD_ACTIVITY_DRAW_TEN_CONTINUOUS_START_TIME";
        /** 活动抽卡10连活动老玩家允许触发结束时间*/
        public static string OLD_ACTIVITY_DRAW_TEN_CONTINUOUS_END_TIME = "OLD_ACTIVITY_DRAW_TEN_CONTINUOUS_END_TIME";
        
        /**扭蛋活动玩多次**/
        public static string ACTIVITY_CAPSULE_COPY_SECOND_BUY_COUNT = "ACTIVITY_CAPSULE_COPY_SECOND_BUY_COUNT";

        /**扭蛋活动玩一次*/
        public static string ACTIVITY_CAPSULE_COPY_FIRST_BUY_COUNT = "ACTIVITY_CAPSULE_COPY_FIRST_BUY_COUNT";

        /**活动星盘结束时间*/
        public static string DRAW_ACTIVITY_END_TIME = "DRAW_ACTIVITY_END_TIME";
        /**活动星盘开始时间*/
        public static string DRAW_ACTIVITY_START_TIME = "DRAW_ACTIVITY_START_TIME";

        /*头像默认ID*/
        public static string AVATAR_DEFAULT_ID = "AVATAR_DEFAULT_ID";

        /*头像框默认ID*/
        public static string AVATAR_DEFAULT_BOX_ID = "AVATAR_DEFAULT_BOX_ID";

        /*首充头像框ID*/
        public static string FIRST_CHARGE_DEFAULT_BOX_ID = "FIRST_CHARGE_DEFAULT_BOX_ID";

    }
}
