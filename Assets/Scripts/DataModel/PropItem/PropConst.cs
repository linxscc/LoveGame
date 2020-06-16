namespace DataModel
{
    public class PropConst
    {
	    /// <summary>
	    ///     金币图标ID
	    /// </summary>
	    public const int GoldIconId = 10001;

	    /// <summary>
	    ///     钻石图标ID
	    /// </summary>
	    public const int GemIconId = 30001;

	    /// <summary>
	    ///     体力图标ID
	    /// </summary>
	    public const int PowerIconId = 20001;

	    /// <summary>
	    ///     探班行动力图标
	    /// </summary>
	    public const int EncouragePowerId = 40001;

	    /// <summary>
	    ///     星缘回忆图标
	    /// </summary>
	    public const int RecolletionIconId = 50001;

	    /// <summary>
	    ///     星缘碎片
	    /// </summary>
	    public const int PuzzleIconId = 200001;

	    /// <summary>
	    ///     金币抽卡券
	    /// </summary>
	    public const int DrawCardByGold = 400;

	    /// <summary>
	    ///     钻石抽卡券 星卡
	    /// </summary>
	    public const int DrawCardByGem = 401;

        //VIP体验卡
        public const int TasteCardId = 402;

        //
        public const int MonthSignAccumulative = 18;

        /// <summary>
        ///     卡牌升级道具 少女星(小)
        /// </summary>
        public const int CardUpgradePropSmall = 1001;

        /// <summary>
        ///     卡牌升级道具 少女星(大)
        /// </summary>
        public const int CardUpgradePropBig = 1002;

        /// <summary>
        ///     卡牌升级道具 少女星(罐)
        /// </summary>
        public const int CardUpgradePropLarge = 1003;


        /// <summary>
        ///     璀璨皇冠    	星缘进化专属道具（精华）唐弋辰
        /// </summary>
        public const int CardEvolutionPropTang = 10001;

        /// <summary>
        ///     钻石星辰			星缘进化专属道具（精华） 秦予哲
        /// </summary>
        public const int CardEvolutionPropQin = 10002;

        /// <summary>
        ///     苍龙之辉			星缘进化专属道具（精华） 言季
        /// </summary>
        public const int CardEvolutionPropYan = 10003;

        /// <summary>
        ///     时光沙漏			星缘进化专属道具（精华） 迟郁
        /// </summary>
        public const int CardEvolutionPropChi = 10004;

        /// <summary>
        ///     100	应援升级道具（活跃）
        /// </summary>
        public const int SupporterActive = 100;

        /// <summary>
        ///     101	应援升级道具（财力）
        /// </summary>
        public const int SupporterFinancial = 101;

        /// <summary>
        ///     102	应援升级道具（资源）
        /// </summary>
        public const int SupporterResource = 102;

        /// <summary>
        ///     103	应援升级道具（传播）
        /// </summary>
        public const int SupporterTransmission = 103;


        //背景
        public const string DEFAULT_BG_IMAGE_NAME = "bg1"; //默认背景Image名

        /// <summary>
        ///     默认背景图片
        /// </summary>
        public const int DefaultBackgroundId = 12500;


        //    1001~1003	用于星缘升级的道具
        //    1100~1139	用于星缘升星的道具
        //    1201~1210	唐弋辰的专属升星道具
        //    1251~1260	秦予哲的专属升星道具
        //    1301~1310	言季的专属升星道具
        //    1351~1360	迟郁的专属升星道具

        public static string GetTips(int itemId)
        {
            if (itemId >= 1001 && itemId <= 1003)
                //return "用于星缘升级的道具";
                return I18NManager.Get("Common_GetTip1");
            if (itemId >= 1100 && itemId <= 1139)
                //return "用于星缘升心的道具";
                return I18NManager.Get("Common_GetTip2");
            if (itemId >= 1201 && itemId <= 1210)
                //return "唐弋辰的专属升心道具";
                return I18NManager.Get("Common_GetTip3");
            if (itemId >= 1251 && itemId <= 1260)
                //return "秦予哲的专属升心道具";
                return I18NManager.Get("Common_GetTip4");
            if (itemId >= 1301 && itemId <= 1310)
                //return "言季的专属升心道具";
                return I18NManager.Get("Common_GetTip5");
            if (itemId >= 1351 && itemId <= 1360)
                //return "迟郁的专属升心道具";
                return I18NManager.Get("Common_GetTip6");

            return null;
        }
    }
}