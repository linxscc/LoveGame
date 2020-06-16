using System.Collections.Generic;
using Assets.Scripts.Module.Pay.Agent;
using Assets.Scripts.Module.Sdk;
using Common;
using DataModel;
using GalaSDKBase;
using UnityEngine;

public class SdkHelper
{
    public static bool isInit = false;
    public static AccountAgent AccountAgent;
    public static ShareAgent ShareAgent;
    public static PushAgent PushAgent;
    public static IPayAgent PayAgent;
    public static StatisticsAgent StatisticsAgent;
    public static CustomServiceAgent CustomServiceAgent;

    public const string TYPE_CREATE_ROLE = "createRole";
    public const string TYPE_DATA_CHANGE = "dataChange";
    public const string TYPE_ENTER_SERVER = "enterServer";
    
    public static void Initialize()
    {
        if (isInit) return;
    
        AccountAgent = new AccountAgent();
        ShareAgent = new ShareAgent();
        PushAgent = new PushAgent();
        StatisticsAgent = new StatisticsAgent();
        CustomServiceAgent = new CustomServiceAgent();

        CustomServiceAgent.Init();
        AccountAgent.Init();

        if ((Application.platform == RuntimePlatform.Android &&
             (AppConfig.Instance.channel == "SSEA" || AppConfig.Instance.channel == "STW")))
        {
            PayAgent = new PayAgentGooglePlay();
        }
        else if ((Application.platform == RuntimePlatform.Android && AppConfig.Instance.isChinese == "true") ||
                 AppConfig.Instance.isJailbreak)
        {
            //安卓和越狱iOS
            if (Channel.IsTencent)
            {
                PayAgent = new PayAgentTencent();
            }
            else
            {
                PayAgent = new PayAgent();
            }
        }
        else if (Application.platform == RuntimePlatform.IPhonePlayer && !AppConfig.Instance.isJailbreak)
        {
            //PayAgent = new PayAgentIOS();
            PayAgent = new PayAgentMyIOS();
        }
        else
        {
            PayAgent = new PayAgentEditor();
        }

        string starWay = GalaSDKBaseFunction.GetAPPStartWay();
        if (starWay.Contains("lianouneedlog") && starWay.Contains("true"))
        {
            AppConfig.Instance.paySandbox = true;

            string idfa = GalaSDKBaseFunction.GetDeviceId();
            GUIUtility.systemCopyBuffer = idfa;
        }
        else
        {
            AppConfig.Instance.paySandbox = false;
        }

        Debug.LogWarning("AppConfig.Instance.paySandbox=>" + AppConfig.Instance.paySandbox);

        isInit = true;
    }

    public static void InitAccount()
    {
        Dictionary<string, string> settingInfo = new Dictionary<string, string>();
        settingInfo.Add("channelInfo", AppConfig.Instance.channelInfo); //渠道包标识,默认LI
        settingInfo.Add("isChinese", AppConfig.Instance.isChinese); //默认国内//"true"国内,"false"海外,
        settingInfo.Add("needRealName", AppConfig.Instance.SwitchControl.NeedRealName ? "true" : "false"); //实名认证,"true"打开,"false"关闭,
        settingInfo.Add("addictionType",AppConfig.Instance.SwitchControl.AddictionType);
        settingInfo.Add("markVersion",AppConfig.Instance.markVersion);
        GalaAccountManager.Instance.UpdateSetting(settingInfo); //进行自定义设置

        //设置登录方式
        if (AppConfig.Instance.isChinese == "true")//国内
        {                                      
            
            List<GalaAccountManager.GalaLoginChanelType> list = new List<GalaAccountManager.GalaLoginChanelType>
            {
                GalaAccountManager.GalaLoginChanelType.Phone,
                GalaAccountManager.GalaLoginChanelType.GalaAccount,
                GalaAccountManager.GalaLoginChanelType.Guest,
                //GalaAccountManager.GalaLoginChanelType.QQ,
                //GalaAccountManager.GalaLoginChanelType.WX,
            };

        
            if (Application.platform == RuntimePlatform.IPhonePlayer && !AppConfig.Instance.isJailbreak)
            {
                //IOS
                if (AppConfig.Instance.SwitchControl.AppleLogin)
                {
                    list.Insert(0, GalaAccountManager.GalaLoginChanelType.Apple);
                }
            }
            else
            {
                //Android
              //  list.Add(GalaAccountManager.GalaLoginChanelType.QQ);
              //  list.Add(GalaAccountManager.GalaLoginChanelType.WX);
            }
            
            GalaAccountManager.Instance.SettingLoginChanel(list);
              
        }
        else if (AppConfig.Instance.isChinese == "false")
        {
            List<GalaAccountManager.GalaLoginChanelType> list = new List<GalaAccountManager.GalaLoginChanelType>
            {
                GalaAccountManager.GalaLoginChanelType.GalaAccount,
                GalaAccountManager.GalaLoginChanelType.Guest
            };

            if (AppConfig.Instance.SwitchControl.FacebookLogin)
            {
                list.Insert(1, GalaAccountManager.GalaLoginChanelType.FaceBook);
            }

            if (Application.platform == RuntimePlatform.IPhonePlayer && !AppConfig.Instance.isJailbreak)
            {
                //iOS
                GalaAccountManager.Instance.SettingLoginChanel(list);
            }
            else
            {
                //Android
                list.Insert(list.Count - 1, GalaAccountManager.GalaLoginChanelType.Google);
                GalaAccountManager.Instance.SettingLoginChanel(list);
            }

            GalaAccountManager.Instance.SettingLoginMoreChanel(
                new List<GalaAccountManager.GalaLoginChanelType>
                {
                });
        }
        else
        {
        }

        //GalaAccountManager.Instance.SettingLoginMoreChanel(
        //    new List<GalaAccountManager.GalaLoginChanelType> {
        //        GalaAccountManager.GalaLoginChanelType.FaceBook,
        //        GalaAccountManager.GalaLoginChanelType.Twitter,
        //        GalaAccountManager.GalaLoginChanelType.Guest,//1
        //        GalaAccountManager.GalaLoginChanelType.WX,
        //        GalaAccountManager.GalaLoginChanelType.QQ
        //    });
    }

    public static void SetSdkData(string type = TYPE_ENTER_SERVER)
    {
        PlayerVo playerVo = GlobalData.PlayerModel.PlayerVo;
        Dictionary<string, string> json = new Dictionary<string, string>();

        Debug.LogWarning("serverName:"+AppConfig.Instance.serverName);
        json.Add(GalaSDKBaseFunction.UserInfoParamKey.PARAMS_TYPE, type); //角色状态
        json.Add(GalaSDKBaseFunction.UserInfoParamKey.PARAMS_ZONEID, AppConfig.Instance.serverId); //游戏区服ID
        json.Add(GalaSDKBaseFunction.UserInfoParamKey.PARAMS_ZONENAME, AppConfig.Instance.serverName); //游戏区服名称
        json.Add(GalaSDKBaseFunction.UserInfoParamKey.PARAMS_ROLEID, playerVo.UserId + ""); //角色ID
        json.Add(GalaSDKBaseFunction.UserInfoParamKey.PARAMS_ROLENAME, playerVo.UserName); //角色名
        json.Add(GalaSDKBaseFunction.UserInfoParamKey.PARAMS_ROLELEVEL, playerVo.Level.ToString()); //角色等级
        json.Add(GalaSDKBaseFunction.UserInfoParamKey.PARAMS_POWER, "0"); //战力值
        json.Add(GalaSDKBaseFunction.UserInfoParamKey.PARAMS_VIP, playerVo.IsOnVip ? "1" : "0"); //VIP等级
        json.Add(GalaSDKBaseFunction.UserInfoParamKey.PARAMS_BALANCENAME, "CNY"); //货币名称
        json.Add(GalaSDKBaseFunction.UserInfoParamKey.PARAMS_BALANCENUM, "0"); //货币数额
        json.Add(GalaSDKBaseFunction.UserInfoParamKey.PARAMS_ROLECTIME, playerVo.CreateTime + ""); //创角时间
        json.Add(GalaSDKBaseFunction.UserInfoParamKey.PARAMS_CHANNEL, AppConfig.Instance.channelInfo + ""); //渠道类型

        json.Add(GalaSDKBaseFunction.UserInfoParamKey.PARAMS_PARTYID, "0"); //帮派ID
        json.Add(GalaSDKBaseFunction.UserInfoParamKey.PARAMS_PARTYNAME, "无"); //帮派名称
        json.Add(GalaSDKBaseFunction.UserInfoParamKey.PARAMS_PARTYROLEID, "0"); //帮派职位ID
        json.Add(GalaSDKBaseFunction.UserInfoParamKey.PARAMS_PARTYROLENAME, "无"); //帮派职业名称
        json.Add(GalaSDKBaseFunction.UserInfoParamKey.PARAMS_PROFESSIONID, "0"); //玩家职业ID
        json.Add(GalaSDKBaseFunction.UserInfoParamKey.PARAMS_PROFESSION, "无"); //职业名称
        json.Add(GalaSDKBaseFunction.UserInfoParamKey.PARAMS_GENDER, "无"); //玩家姓名

        Debug.Log("SetSdkData--->" + json);

        GalaSDKBaseFunction.UpdateUserInfo(json);

        CustomServiceAgent.OnLogin();

        if ((Application.platform == RuntimePlatform.Android && AppConfig.Instance.isChinese == "true") ||
            AppConfig.Instance.isJailbreak)
        {
            //安卓和越狱iOS
            PayAgent.CheckPayWhenLogin();
        }
        else if(Application.platform == RuntimePlatform.IPhonePlayer)
        {
            //PayAgent.InitPay();
            PayAgent.CheckPayWhenLogin();
            Debug.Log("OnGetRechargeRule===>SdkHelper.PayAgent.InitPay()");
        }

#if UNITY_EDITOR
        PayAgent.CheckPayWhenLogin();
#endif
    }
}