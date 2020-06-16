using System.Collections.Generic;
using Common;
using DataModel;
using game.main;
using GalaAccountSystem;
using GalaCSCSystem;

namespace Assets.Scripts.Module.Sdk
{
    public class CustomServiceAgent
    {
        public void Init()
        {
            Dictionary<string, string> settingInfo = new Dictionary<string, string>();
            settingInfo.Add("channel", AppConfig.Instance.channelInfo);//渠道,string
            settingInfo.Add("gameType", AppConfig.Instance.gameType+"");//必填//游戏id//足球0，篮球1，新足球2，恋偶3
            settingInfo.Add("isLandscape", "false");//横竖 屏,默认"true"横屏,"false"竖屏
            settingInfo.Add("loadingType", "0");//加在类型,1球类加载,默认菊花
            GalaCSCManager.Instance.UpdateSetting(settingInfo);//进行自定义设置
            
            //竖
            GalaCSCBaseView.Instance.LoadDefultSetting(GalaCSCBaseView.GalaAccountBaseSetting.TitleColor, "9083E7FF");//(所有界面通用)导航栏颜色21B6F6FF
            GalaCSCBaseView.Instance.LoadDefultSetting(GalaCSCBaseView.GalaAccountBaseSetting.TitleCharColor, "FFFFFFFF");//(所有界面通用)导航栏字体颜色
            //横
            //GalaCSCBaseView.Instance.LoadDefultSetting(GalaCSCBaseView.GalaAccountBaseSetting.TitleColor, "FFFFFFFF");//(所有界面通用)导航栏颜色21B6F6FF
            //GalaCSCBaseView.Instance.LoadDefultSetting(GalaCSCBaseView.GalaAccountBaseSetting.TitleCharColor, "21B6F6FF");//(所有界面通用)导航栏字体颜色


            GalaCSCBaseView.Instance.LoadDefultSetting(GalaCSCBaseView.GalaAccountBaseSetting.MainImageColor, "9083E7FF");//(所有界面通用)主题颜色21B6F6FF
            GalaCSCBaseView.Instance.LoadDefultSetting(GalaCSCBaseView.GalaAccountBaseSetting.MainFontColor, "FFFFFFFF");//(所有界面通用)主题颜色FFFFFFFF

            GalaCSCBaseView.Instance.LoadDefultSetting(GalaCSCBaseView.GalaAccountBaseSetting.BtnColor, "9083E7FF");//(所有界面通用)按钮颜色
            GalaCSCBaseView.Instance.LoadDefultSetting(GalaCSCBaseView.GalaAccountBaseSetting.BtnCharColor, "FFFFFFFF");//(所有界面通用)按钮字体颜色
            GalaCSCBaseView.Instance.LoadDefultSetting(GalaCSCBaseView.GalaAccountBaseSetting.BackGroundColor, "E6E6E6FF");//(所有界面通用)背景颜色

            GalaCSCBaseView.Instance.LoadDefultSetting(GalaCSCBaseView.GalaAccountBaseSetting.InputCharColor, "707070FF");//(所有界面通用)输入框字体颜色
            GalaCSCBaseView.Instance.LoadDefultSetting(GalaCSCBaseView.GalaAccountBaseSetting.InputTipsColor, "ADADAFFF");//(所有界面通用)输入框提示颜色
            GalaCSCBaseView.Instance.LoadDefultSetting(GalaCSCBaseView.GalaAccountBaseSetting.TipsColor, "FFFFFFFF");//(所有界面通用)提示字体颜色

            GalaCSCBaseView.Instance.LoadDefultSetting(GalaCSCBaseView.GalaAccountBaseSetting.BarSelectColor, "FFFFFFFF");//(所有界面通用)提示字体颜色
            GalaCSCBaseView.Instance.LoadDefultSetting(GalaCSCBaseView.GalaAccountBaseSetting.BarDefaultColor, "FFFFFFFF");//(所有界面通用)提示字体颜色
            
            GalaCSCBaseView.Instance.LoadDefultSetting(GalaCSCBaseView.GalaAccountBaseSetting.LinkColor, "00FF00FF");//(所有界面通用)链接字体颜色
            
            List<string> loginList = new List<string>();
            loginList.Add(ClientData.ErrorCodeDict[-100]);
            loginList.Add(ClientData.ErrorCodeDict[-101]);
            loginList.Add(ClientData.ErrorCodeDict[-102]);
            loginList.Add(ClientData.ErrorCodeDict[-103]);
            loginList.Add(ClientData.ErrorCodeDict[-104]);
            loginList.Add(ClientData.ErrorCodeDict[-105]);
            loginList.Add(ClientData.ErrorCodeDict[-106]);
            loginList.Add(ClientData.ErrorCodeDict[1000]);
            loginList.Add(ClientData.ErrorCodeDict[1001]);
            loginList.Add(ClientData.ErrorCodeDict[10001]);
            loginList.Add(ClientData.ErrorCodeDict[10002]);
            loginList.Add(ClientData.ErrorCodeDict[10003]);
            loginList.Add(ClientData.ErrorCodeDict[10005]);
            loginList.Add(ClientData.ErrorCodeDict[10006]);

            GalaCSCManager.Instance.LoadErrorInfo(loginList);//设置登录错误码信息（登陆问题供用户选择dataTest：List<string>）

            //            GalaCSCManager.Instance.LoadActiveInfo(loginList);//设置活动信息（活动问题供用户选择dataTest：List<string>）

            List<string> serviceInfos = new List<string>();
       
            serviceInfos.Add(I18NManager.Get("Login_MainServerList"));
            GalaCSCManager.Instance.LoadServiceInfo(serviceInfos);//设置登录服务器信息（登陆问题供用户选择dataTest：List<string>）


            //List<string> GameInfos = new List<string>();
            //GameInfos.Add("西瓜服");
            //GameInfos.Add("西瓜服");
            //GameInfos.Add("西瓜服");
            //GalaCSCManager.Instance.LoadGameInfo(GameInfos);//设置比赛信息（比赛问题供用户选择dataTest：List<string>)
            GalaCSCManager.Instance.SetCustomerServiceUrl(AppConfig.Instance.customerServiceServer);
            GalaCSCManager.Instance.Init();
            
            GalaCSCManager.Instance.SetNewReplyCallback(delegate (bool hasreplay)
            {
                if (hasreplay)
                {
                    //有新消息
                    FlowText.ShowMessage("您有新的客服消息");
                }
                else
                {
                    //没新消息
                }
            });//检测是否有新消息,
        }

        public void Show()
        {
            GalaCSCManager.Instance.ShowCustomer();
        }
        
        public void Reset()
        {
            GalaCSCManager.Instance.ClearUserInfo();//清除登录信息（退出登陆后）
        }
        
        
        public void OnLogin()
        {
            Dictionary<string, string> userdata = new Dictionary<string, string>();
            PlayerVo playerVo = GlobalData.PlayerModel.PlayerVo;
            userdata.Add("userAccount", playerVo.AccountId);//string用户账号
            userdata.Add("serverId", AppConfig.Instance.serverId);//string服务器ID
            userdata.Add("userCode", playerVo.UserId + "");//string识别码          
            userdata.Add("teamName", playerVo.UserName);//string球队名
            userdata.Add("vipLevel", playerVo.IsOnVip ? "1" : "0");//string vip等级
            userdata.Add("totalRecharge", "0");//string累计充值
            userdata.Add("teamLevel", playerVo.Level+"");//string
            userdata.Add("regisTime", playerVo.CreateTime+"");//long时间戳
//            userdata.Add("teamOtherInfo", "jsonString");//string额外信息:json字符串
            GalaCSCManager.Instance.LoadUserInfo(userdata);//设置用户信息（登陆成功后）
        }
        
    }
}