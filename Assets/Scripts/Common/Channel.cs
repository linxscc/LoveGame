using GalaSDKBase;

namespace Common
{
    public class Channel
    {
        public const string Official = "Official";

        public const string GooglePlay = "GooglePlay";

        /// <summary>
        /// 国内安卓官网
        /// </summary>
        public const string ChannelInfo_CN_ANDROID = "CN_ANDROID";
        public const string ChannelInfo_CN_IOS = "CN_IOS";
        
        public const string ChannelInfo_LI = "LI";
        
        public const string ChannelInfo_TENCENT = "TENCENT";


        public static bool IsTencent => AppConfig.Instance.channelInfo == ChannelInfo_TENCENT;

        public static bool CheckShowPayChooser()
        {
            return AppConfig.Instance.channelInfo == ChannelInfo_CN_ANDROID;
        }

        public static GalaSDKBaseFunction.GalaSDKType LoginType()
        {
            GalaSDKBaseFunction.GalaSDKType type;
            if (AppConfig.Instance.channelInfo == ChannelInfo_CN_ANDROID ||
                //AppConfig.Instance.channelInfo == ChannelInfo_CN_IOS ||    //IOS登錄改成萌友
                AppConfig.Instance.channelInfo == ChannelInfo_LI)
            {
                if (UnityEngine.Application.platform == UnityEngine.RuntimePlatform.IPhonePlayer)  //IOS内網channelinfo是LI
                {
                    type = GalaSDKBaseFunction.GalaSDKType.Channel;
                }
                else
                {
                    type = GalaSDKBaseFunction.GalaSDKType.Gala;
                }
            }
            else
            {
                type = GalaSDKBaseFunction.GalaSDKType.Channel;
            }
            return type;
        }
    }
}