using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module.NetWork;
using Com.Proto;
using DataModel;
using game.main;
using UnityEngine;
using UnityEngine.UI;

public class MailModule : ModuleBase
{

    private MailPanel _mailPanel;
   
    public override void Init()
    {

        DelayUnloadAtlas = 0.2f;
        RegisterModel<MailModel>();
        NetWorkManager.Instance.Send<UserMailRes>(CMD.MAIL_USERMAILS, null,  GetMailDatas);
       
    }


    private void GetMailDatas(UserMailRes res)
    {
         GetData<MailModel>().Init(res);        
        _mailPanel = new MailPanel();
        _mailPanel.Init(this);
        _mailPanel.Show(0.5f);
    }



    public override void OnShow(float delay)
    {
        base.OnShow(delay);
    }

    public override void OnMessage(Message message)
    {
       
        string name = message.Name;
        object[] body = message.Params;
        switch (name)
        {
            default:
                break;
        }
    }

  


   
}
