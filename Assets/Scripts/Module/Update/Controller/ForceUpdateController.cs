using Assets.Scripts.Framework.GalaSports.Core;
using DataModel;
using game.main;
using UnityEngine;

public class ForceUpdateController : Controller
{
    public UpdateView View;

    public override void OnMessage(Message message)
    {
        string name = message.Name;
        object[] body = message.Params;
        switch (name)
        {
          case MessageConst.CMD_UPDATE_SHOW_FORCE_UPDATE:

              RAMInadequateWinodw win =
                  PopupManager.ShowWindow<RAMInadequateWinodw>("Update/Prefabs/RAMInadequateWinodw");

              win.SetDate(GlobalData.NoticeData.GetForceUpdateNotice().Content, (string)body[0]);
              
              if (Application.platform==RuntimePlatform.Android)  
              {
                                       //走安卓更新
              }
              else if (Application.platform==RuntimePlatform.IPhonePlayer)
              {
                  //走苹果更新
              }
              break;
        }
    }


    public override void Destroy()
    {
        base.Destroy();
    }
}
