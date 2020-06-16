using Assets.Scripts.Common;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Interfaces;
using System;
using System.Collections.Generic;
using game.main;
using UnityEngine;

public class MailPanel : ReturnablePanel
{
    private MailController _mailController;
    string path = "Mail/Prefabs/MailWinodw";                       
    public override void Init(IModule module)
    {
        base.Init(module);

        //var viewScript = InstantiateView<MailView>(path);
        var viewScript =PopupManager.ShowWindow<MailWinodw>(path, module);
        
        _mailController = new MailController();
        _mailController.Winodw = (MailWinodw)viewScript;

        RegisterView(viewScript);
        RegisterController(_mailController);
        

        _mailController.Start();
    }

    public override void Show(float delay)
    {
       // _mailController.Winodw.Show(delay);
      //  Main.ChangeMenu(MainMenuDisplayState.HideAll);
      //  ShowBackBtn();
    }

    public override void Hide()
    {
       // _mailController.MailView.Hide();
    }
   
    public override void Destroy()
    {
        base.Destroy();
    }

}
