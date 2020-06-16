using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Core.Events;
using Common;
using GalaSDKBase;
using Module.Login;
using UnityEngine;

namespace game.main
{
    public class UserInfoController : Controller
    {
        public UserInfoView View;

        public override void OnMessage(Message message) //主菜单消息发送
        {
            string name = message.Name;
            object[] body = message.Params;
            bool isStopPlayDubbing = true;

            switch (name)
            {
                case MessageConst.CMD_MAIN_SHOW_USER_INFO:
                    ShowLogout();
                    break;
            }
        }

        private void ShowLogout()
        {
            PopupManager.ShowConfirmWindow(I18NManager.Get("GameMain_SetPanelHint3"), I18NManager.Get("Common_Logout1")).WindowActionCallback = evt =>
            {
                if(evt == WindowEvent.Ok)
                {
                    GalaAccountManager.Instance.Logout(Channel.LoginType());
                    EventDispatcher.TriggerEvent<bool>(EventConst.ForceToLogin,true);
                }
            };
        }
    }
}