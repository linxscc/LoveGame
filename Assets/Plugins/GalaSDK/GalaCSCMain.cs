using UnityEngine;
using System.Collections.Generic;
using GalaCSCSystem;
using GalaAccountSystem;



namespace GalaAccount.Scripts
{
    public class GalaCSCMain : MonoBehaviour
    {
        public static int StageHeight = 750;
        public static int StageWidth = 1334;
        public static int StageScale = 1;
        // Use this for initialization
        void Start()
        {
            Dictionary<string, string> settingInfo = new Dictionary<string, string>();
            settingInfo.Add("channel", "channel_test");//渠道,string
            settingInfo.Add("gameType", "2");//必填//游戏id//足球0，篮球1，新足球2，恋偶3
            settingInfo.Add("isLandscape", "false");//横竖 屏,默认"true"横屏,"false"竖屏
            settingInfo.Add("loadingType", "0");//加在类型,1球类加载,默认菊花
            GalaCSCManager.Instance.UpdateSetting(settingInfo);//进行自定义设置
            //竖
            GalaCSCBaseView.Instance.LoadDefultSetting(GalaCSCBaseView.GalaAccountBaseSetting.TitleColor, "9083E7FF");//(所有界面通用)导航栏颜色21B6F6FF
            GalaCSCBaseView.Instance.LoadDefultSetting(GalaCSCBaseView.GalaAccountBaseSetting.TitleCharColor, "FFFFFFFF");//(所有界面通用)导航栏字体颜色
            //横
            //GalaCSCBaseView.Instance.LoadDefultSetting(GalaCSCBaseView.GalaAccountBaseSetting.TitleColor, "FFFFFFFF");//(所有界面通用)导航栏颜色21B6F6FF
            //GalaCSCBaseView.Instance.LoadDefultSetting(GalaCSCBaseView.GalaAccountBaseSetting.TitleCharColor, "21B6F6FF");//(所有界面通用)导航栏字体颜色


            GalaCSCBaseView.Instance.LoadDefultSetting(GalaCSCBaseView.GalaAccountBaseSetting.MainImageColor, "21B6F6FF");//(所有界面通用)主题颜色21B6F6FF
            GalaCSCBaseView.Instance.LoadDefultSetting(GalaCSCBaseView.GalaAccountBaseSetting.MainFontColor, "FFFFFFFF");//(所有界面通用)主题颜色FFFFFFFF

            GalaCSCBaseView.Instance.LoadDefultSetting(GalaCSCBaseView.GalaAccountBaseSetting.BtnColor, "21B6F6FF");//(所有界面通用)按钮颜色
            GalaCSCBaseView.Instance.LoadDefultSetting(GalaCSCBaseView.GalaAccountBaseSetting.BtnCharColor, "FFFFFFFF");//(所有界面通用)按钮字体颜色
            GalaCSCBaseView.Instance.LoadDefultSetting(GalaCSCBaseView.GalaAccountBaseSetting.BackGroundColor, "E6E6E6FF");//(所有界面通用)背景颜色

            GalaCSCBaseView.Instance.LoadDefultSetting(GalaCSCBaseView.GalaAccountBaseSetting.InputCharColor, "707070FF");//(所有界面通用)输入框字体颜色
            GalaCSCBaseView.Instance.LoadDefultSetting(GalaCSCBaseView.GalaAccountBaseSetting.InputTipsColor, "ADADAFFF");//(所有界面通用)输入框提示颜色
            GalaCSCBaseView.Instance.LoadDefultSetting(GalaCSCBaseView.GalaAccountBaseSetting.TipsColor, "FFFFFFFF");//(所有界面通用)提示字体颜色

            GalaCSCBaseView.Instance.LoadDefultSetting(GalaCSCBaseView.GalaAccountBaseSetting.BarSelectColor, "FFFFFFFF");//(所有界面通用)提示字体颜色
            GalaCSCBaseView.Instance.LoadDefultSetting(GalaCSCBaseView.GalaAccountBaseSetting.BarDefaultColor, "FFFFFFFF");//(所有界面通用)提示字体颜色
            
            GalaCSCBaseView.Instance.LoadDefultSetting(GalaCSCBaseView.GalaAccountBaseSetting.LinkColor, "00FF00FF");//(所有界面通用)链接字体颜色


//            GalaSDKLanguageService.Instance.SetLanguageType(GalaSDKLanguageService.LanguageType.CN);//设置语言


            List<string> dataTest = new List<string>();
            dataTest.Add(GalaSDKLanguageService.Instance.GetStringByKey("GALA_ACCOUNT_ERROR_100000"));
            dataTest.Add(GalaSDKLanguageService.Instance.GetStringByKey("GALA_ACCOUNT_ERROR_100001"));
            dataTest.Add(GalaSDKLanguageService.Instance.GetStringByKey("GALA_ACCOUNT_ERROR_100003"));
            dataTest.Add(GalaSDKLanguageService.Instance.GetStringByKey("GALA_ACCOUNT_ERROR_100006"));
            dataTest.Add(GalaSDKLanguageService.Instance.GetStringByKey("GALA_ACCOUNT_ERROR_100007"));
            dataTest.Add(GalaSDKLanguageService.Instance.GetStringByKey("GALA_ACCOUNT_ERROR_100008"));

            GalaCSCManager.Instance.LoadErrorInfo(dataTest);//设置登录错误码信息（登陆问题供用户选择dataTest：List<string>）

            GalaCSCManager.Instance.LoadActiveInfo(dataTest);//设置活动信息（活动问题供用户选择dataTest：List<string>）

            GalaCSCManager.Instance.LoadServiceInfo(dataTest);//设置登录服务器信息（登陆问题供用户选择dataTest：List<string>）

            GalaCSCManager.Instance.LoadGameInfo(dataTest);//设置比赛信息（比赛问题供用户选择dataTest：List<string>)

            GalaCSCManager.Instance.Init();//
            Dictionary<string, string> userdata = new Dictionary<string, string>();

            userdata.Add("userAccount", "userAccount");//string用户账号
            userdata.Add("serverId", "serverId");//string服务器ID
            userdata.Add("userCode", "1");//string识别码          
            userdata.Add("teamName", "teamName");//string球队名
            userdata.Add("vipLevel", "1");//string vip等级
            userdata.Add("totalRecharge", "1");//string累计充值
            userdata.Add("teamLevel", "1");//string
            userdata.Add("regisTime", "1231231");//long时间戳
            userdata.Add("teamOtherInfo", "jsonString");//string额外信息:json字符串
            GalaCSCManager.Instance.LoadUserInfo(userdata);//设置用户信息（登陆成功后）

            GalaCSCManager.Instance.ClearUserInfo();//清除登录信息（退出登陆后）

            GalaCSCManager.Instance.SetNewReplyCallback(delegate (bool hasreplay)
            {
                if (hasreplay)
                {
                    //有新消息
                }
                else
                {
                    //没新消息
                }
            });//检测是否有新消息,

            //GalaCSCManager.Instance.ShowCustomer();//显示客服中心
        }

    }
}
