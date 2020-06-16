using System;
using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Core.Events;
using Common;
using DataModel;
using GalaAccount;
using GalaAccountSystem;
using GalaSDKBase;
using UnityEngine;

namespace Assets.Scripts.Module.Sdk
{
    public class AccountAgent
    {
        public string Token;
        public string AccountId;
        public string SdkAccountId;
       
        public bool IsAutoLogin;

        public bool IsLogin = false;
        public string AppWelcomeIcon;//登录欢迎显示动画
        
        private Action<LoginCallbackType> _accountCenterLoginCallback;
        
//        public bool CanEnterGame = false;


        public void SetLoginCallback(Action<LoginCallbackType> callback)
        {
            _accountCenterLoginCallback = callback;
        }
        
        public void Logout()
        {
            Debug.LogError("7777777777");
            //GalaSDKBaseFunction.SdkLogoutWithType(GalaSDKBaseFunction.GalaSDKType.Channel);
            GalaAccountManager.Instance.Logout(GalaSDKBaseFunction.GalaSDKType.Channel);
            SdkHelper.StatisticsAgent.OnEvent("Logout");
        }

        public void Init()
        {
            Dictionary<string, string> settingInfo = new Dictionary<string, string>();
            settingInfo.Add("gameType", "3");//必填//游戏id//足球0，篮球1，新足球2，恋偶3
            settingInfo.Add("isLandscape", "false");//横竖屏,默认"true"横屏,"false"竖屏
            settingInfo.Add("loadingType", "0");//加在类型,1球类加载,默认菊花
            settingInfo.Add("needWellCome", "false");//欢迎动画,默认"true"展示,"false"关闭
            GalaAccountManager.Instance.UpdateSetting(settingInfo);//进行自定义设置

            GalaAccountBaseView.Instance.LoadDefultSetting(GalaAccountBaseView.GalaAccountBaseSetting.MainFontColor, "F685B8FF");//(所有界面通用)主题颜色
            GalaAccountBaseView.Instance.LoadDefultSetting(GalaAccountBaseView.GalaAccountBaseSetting.TitleColor, "FFD2E6FF");//(所有界面通用)导航栏颜色
            GalaAccountBaseView.Instance.LoadDefultSetting(GalaAccountBaseView.GalaAccountBaseSetting.TitleCharColor, "FFFFFFFF");//(所有界面通用)导航栏字体颜色
            GalaAccountBaseView.Instance.LoadDefultSetting(GalaAccountBaseView.GalaAccountBaseSetting.BtnColor, "F684B7FF");//(所有界面通用)按钮颜色
            GalaAccountBaseView.Instance.LoadDefultSetting(GalaAccountBaseView.GalaAccountBaseSetting.BtnCharColor, "FFFFFFFF");//(所有界面通用)按钮字体颜色
            GalaAccountBaseView.Instance.LoadDefultSetting(GalaAccountBaseView.GalaAccountBaseSetting.BackGroundColor, "FFFFFFFF");//(所有界面通用)背景颜色

            GalaAccountBaseView.Instance.LoadDefultSetting(GalaAccountBaseView.GalaAccountBaseSetting.BackGroundImage, "");//(所有界面通用)背景图片(界面大小:横版窗口:786*501,竖版全屏:自适应,需要背景图适应最大屏幕,很大很大)
            GalaAccountBaseView.Instance.LoadDefultSetting(GalaAccountBaseView.GalaAccountBaseSetting.LeftUpImage, "");//(所有界面通用)左上角背景图片路径
            GalaAccountBaseView.Instance.LoadDefultSetting(GalaAccountBaseView.GalaAccountBaseSetting.LeftDownImage, "");//(所有界面通用)左下角背景图片路径
            GalaAccountBaseView.Instance.LoadDefultSetting(GalaAccountBaseView.GalaAccountBaseSetting.RightUpImage, "");//(所有界面通用)右上角背景图片路径
            GalaAccountBaseView.Instance.LoadDefultSetting(GalaAccountBaseView.GalaAccountBaseSetting.RightDownImage, "");//(所有界面通用)右下角背景图片路径
            GalaAccountBaseView.Instance.LoadDefultSetting(GalaAccountBaseView.GalaAccountBaseSetting.BtnImage, "");//(所有界面通用)所有按钮图片(按钮大小:::横版:506*70,竖版:880:120)
            GalaAccountBaseView.Instance.LoadDefultSetting(GalaAccountBaseView.GalaAccountBaseSetting.BtnBackImage, "");//(所有界面通用)所有按钮背景图片(按钮大小:::横版:506*70,竖版:880:120)

            GalaAccountBaseView.Instance.LoadDefultSetting(GalaAccountBaseView.GalaAccountBaseSetting.VerificationCodeBtnColor, "FEA0C8FF");//(所有界面通用)获取验证码按钮颜色
            GalaAccountBaseView.Instance.LoadDefultSetting(GalaAccountBaseView.GalaAccountBaseSetting.VerificationCodeBtnCharColor, "FFFFFFFF");//(所有界面通用)获取验证码按钮字体颜色
            GalaAccountBaseView.Instance.LoadDefultSetting(GalaAccountBaseView.GalaAccountBaseSetting.InputCharColor, "F685B8FF");//(所有界面通用)输入框字体颜色
            GalaAccountBaseView.Instance.LoadDefultSetting(GalaAccountBaseView.GalaAccountBaseSetting.InputTipsColor, "AAAAAAFF");//(所有界面通用)输入框提示颜色
            GalaAccountBaseView.Instance.LoadDefultSetting(GalaAccountBaseView.GalaAccountBaseSetting.TipsColor, "9BA6ADFF");//(所有界面通用)提示字体颜色
            GalaAccountBaseView.Instance.LoadDefultSetting(GalaAccountBaseView.GalaAccountBaseSetting.RedColor, "B0322AFF");//(所有界面通用)提示字体红色颜色
            GalaAccountBaseView.Instance.LoadDefultSetting(GalaAccountBaseView.GalaAccountBaseSetting.RegisterBtnColor, "48B4FFFF");//(所有界面通用)注册按钮颜色
            GalaAccountBaseView.Instance.LoadDefultSetting(GalaAccountBaseView.GalaAccountBaseSetting.CancelBtnColor, "FFD2E6FF");//(所有界面通用)提交确认按钮颜色
            GalaAccountBaseView.Instance.LoadDefultSetting(GalaAccountBaseView.GalaAccountBaseSetting.SelectColor, "F685B8FF");//(所有界面通用)选中颜色


            //自定义某个界面单独设置
           // GalaAccountBaseView.Instance.LoadViewCustomSetting(GalaAccountBaseView.GalaAllAccountView.MessageView,GalaAccountBaseView.GalaAccountBaseSetting.RightUpImage, "");//新足消息弹框不显示右上角角图
           // GalaAccountBaseView.Instance.LoadViewCustomSetting(GalaAccountBaseView.GalaAllAccountView.RealNameAuthenticationView, GalaAccountBaseView.GalaAccountBaseSetting.RightUpImage, "");//新足实名认证,右上角有关闭按钮,不显示右上角角图

          
            SetGalaActionInit();
            
            GalaAccountManager.Instance.Init();
        }
             
        
        private void SetGalaActionInit()
        {
            GalaAccountManager.Instance. SettingLoginSuccessCallBack(strArray =>
            {          
                Debug.Log("SettingLoginSuccessCallBack");
                AccountId = strArray[0];
                Token = strArray[1];
                AppWelcomeIcon = strArray[2];
              //  SdkAccountId = strArray[2];
              //  IsAutoLogin = strArray[3].ToLower() == "true";
                _accountCenterLoginCallback?.Invoke(LoginCallbackType.Success);
            });

            GalaAccountManager.Instance.SettingLoginOutCallBack(OnLogout);
            
            GalaAccountManager.Instance.SettingLoginFailCallBack(() =>
            {
                //登录失败
                Debug.Log("SettingLoginFailCallBack");
                AccountId = null;
                Token = null;
                _accountCenterLoginCallback?.Invoke(LoginCallbackType.Fail);
            });

            GalaAccountManager.Instance.SettingUserInfoUpdateCallBack((type =>
            {
                Debug.LogError("触发用户信息更改");
                Debug.Log("SettingUserInfoUpdateCallBack");
                if (type == GalaAccountManager.GalaUserInfoChangeType.RealName)
                {
                    Debug.LogError("触发用户实名认证");
                    EventDispatcher.TriggerEvent(EventConst.GetRealNameAward); 
                }
                EventDispatcher.TriggerEvent(EventConst.SettingUserInfoUpdate);
            }));
                         
            GalaAccountManager.Instance.SettingAccountHideCallBack(() =>
            {
                //账号中心隐藏后
                Debug.Log("SettingAccountHideCallBack");
//                if (DataManager.Instance.hasRealName&&!GlobalData.PlayerModel.PlayerVo.Addication)
//                {
//                  //  EventDispatcher.TriggerEvent(EventConst.SettingUserInfoUpdate);
//                }
            });
        }

       public void OnLogout()
       {
           //登出成功
           Debug.Log("SettingLoginOutCallBack");
           AccountId = null;
           Token = null;
           IsLogin = false;
                
           if(ModuleManager.Instance.HasModule(ModuleConfig.MODULE_LOGIN) == false || 
              ModuleManager.Instance.HasModule(ModuleConfig.MODULE_CREATE_USER))
               EventDispatcher.TriggerEvent<bool>(EventConst.ForceToLogin, true);

           _accountCenterLoginCallback?.Invoke(LoginCallbackType.Logout);
       }
    }
}