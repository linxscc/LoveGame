using Assets.Scripts.Module.Pay.Data;
using Com.Proto;

namespace DataModel
{
    public class GlobalData
    {
        /// <summary>
        /// 应援会数据
        /// </summary>
        public static MyDepartmentData DepartmentData;

        public static DepartmentRuleRes DepartmentRule;

        /// <summary>
        /// 版本信息
        /// </summary>
        public static VersionData VersionData;

        /// <summary>
        /// 配置信息
        /// </summary>
        public static ConfigModel ConfigModel;

        /// <summary>
        /// 玩家信息
        /// </summary>
        public static PlayerModel PlayerModel;

        /// <summary>
        /// 卡牌信息
        /// </summary>
        public static CardModel CardModel;

        /// <summary>
        /// 道具信息
        /// </summary>
        public static PropModel PropModel;


        /// <summary>
        /// NPC信息
        /// </summary>
        public static NpcModel NpcModel;

        /// <summary>
        /// 手机模块
        /// </summary>
        public static PhoneData PhoneData;


        /// <summary>
        /// 好感度信息
        /// </summary>
        public static FavorabilityMainModel FavorabilityMainModel;

        /// <summary>
        /// 日记元素模块
        /// </summary>
        public static DiaryElementModel DiaryElementModel;


        /// <summary>
        /// 活动模块
        /// </summary>
        public static ActivityModel ActivityModel;

        /// <summary>
        /// 支付
        /// </summary>
        public static PayModel PayModel;
        
        
        /// <summary>
        /// 事件（触发式礼包）
        /// </summary>
        public static RandomEventModel RandomEventModel;


        /// <summary>
        /// 练习室
        /// </summary>
        public static TrainingRoomModel TrainingRoomModel;


        /// <summary>
        /// 公告
        /// </summary>
        public static NoticeData NoticeData;
        
        public static LevelModel LevelModel;


        public static AppointmentModel AppointmentData;

        /// <summary>
        /// 任务
        /// </summary>
        public static MissionModel MissionModel;

        public static CapsuleLevelModel CapsuleLevelModel;
       
        
        public static void InitData()
        {
            VersionData = new VersionData();

            ConfigModel = new ConfigModel();

            PlayerModel = new PlayerModel();

            CardModel = new CardModel();

            PropModel = new PropModel();

            NpcModel = new NpcModel();

            PhoneData = new PhoneData();

            FavorabilityMainModel = new FavorabilityMainModel();

            DiaryElementModel = new DiaryElementModel();

            ActivityModel = new ActivityModel();

            PayModel = new PayModel();

            RandomEventModel = new RandomEventModel();
            
            NoticeData = new NoticeData();  
            
            LevelModel = new LevelModel();
            
            AppointmentData=new AppointmentModel();
            
            TrainingRoomModel =new TrainingRoomModel();


            MissionModel = new MissionModel();

            CapsuleLevelModel =new CapsuleLevelModel();
        }
    }
}