using Assets.Scripts.Common;
using Assets.Scripts.Framework.GalaSports.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Framework.GalaSports.Core;

public class ActivityCapsuleTemplateDrawPanel : ReturnablePanel
{

    private ActivityCapsuleTemplateDrawController _controller;
    public override void Init(IModule module)
    {
        base.Init(module);

        var viewScript = InstantiateView<ActivityCapsuleTemplateDrawView>("ActivityCapsuleTemplate/Prefabs/ActivityCapsuleTemplateDrawView");
        _controller = new ActivityCapsuleTemplateDrawController { View = viewScript };
        RegisterView(viewScript);
        RegisterController(_controller);
        RegisterModel<ActivityCapsuleTemplateModel>();
        _controller.Start();

    }

    public void Refresh()
    {
        _controller.Refresh();
    }


    public override void Show(float delay)
    {
        base.Show(delay);
        _controller.View.Show();
        OnShow();
        Main.ChangeMenu(MainMenuDisplayState.HideAll);
        ShowBackBtn();
    }


    public void OnShow()
    {
        _controller.View.OnShow();
    }

    public void IsShowBackBtnAndTopBar(bool isShow)
    {
        if (isShow)
        {
            ShowBackBtn();
            Main.ChangeMenu(MainMenuDisplayState.ShowTopBar);
        }
        else
        {
            HideBackBtn();
            Main.ChangeMenu(MainMenuDisplayState.HideAll);
        }
    }

    public void DestroyCountDown()
    {
        _controller.View.DestroyCountDown();
    }

    public override void Hide()
    {
        _controller.View.Hide();
        base.Hide();
    }

    public override void OnBackClick()
    {
        SendMessage(new Message(MessageConst.MODULE_ACTIVITY_CAPSULE_TEMPLATE_OPEN_MAIN_PANEL, Message.MessageReciverType.DEFAULT));
    }
}
