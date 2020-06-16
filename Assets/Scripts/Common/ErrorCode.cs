namespace Common
{
    public class ErrorCode
    {
        /// <summary>
        /// 账号未激活
        /// </summary>
        public const int USER_NOT_CARD_CODE = -2;

        /// <summary>
        /// 用户账号在异处登录"
        /// </summary>
        public const int NOT_USER_OTHER_LOGIN = -103;
        
        /// <summary>
        /// 没有用户登录
        /// </summary>
        public const int NOT_USER_LOGIN = -102;
        
        /// <summary>
        /// 没有渠道登陆
        /// </summary>
        public const int NOT_CHANNEL_LOGIN = -101;
        
        /// <summary>
        /// 账号已被封号
        /// </summary>
        public const int BAN_CONDITION = -106;


        /// <summary>
        /// 后端错误码（请求支付服务器游客无法充值）
        /// </summary>
        public const int SERVER_TOURIST_NOT_RECHARGE = 120016;


        /// <summary>
        /// 后端错误码(请求支付服务器已达充值限额)
        /// </summary>
        public const int SERVER_RECHARGE_UPPERLIMIT = 120017;


        /// <summary>
        /// 后端错误码(请求支付服务器不开放充值)
        /// </summary>
        public const int SERVER_NOT_OPPEN_RECHARGE = 120018;
        
        //platformCode
        
        /// <summary>
        /// 平台错误码（游客  无法充值）
        /// </summary>
        public const int PLATFORMCODE_TOURIST_NOT_RECHARGE = 100013;


        /// <summary>
        /// 平台错误码（已达到充值上限）
        /// </summary>
        public const int PLATFORMCODE_RECHARGE_UPPERLIMIT = 100014;


        /// <summary>
        /// 平台错误码（未成年8岁以下 无法充值）
        /// </summary>
        public const int PLATFORMCODE_NOT_OPPEN_RECHARGE = 100015;

    }
}