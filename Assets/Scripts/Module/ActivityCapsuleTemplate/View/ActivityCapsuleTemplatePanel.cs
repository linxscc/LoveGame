using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Common;
using Assets.Scripts.Framework.GalaSports.Interfaces;

public class ActivityCapsuleTemplatePanel : ReturnablePanel
{

    private ActivityCapsuleTemplateController _controller;
    public override void Init(IModule module)
    {
        base.Init(module);

        var viewScript = InstantiateView<ActivityCapsuleTemplateView>("ActivityCapsuleTemplate/Prefabs/ActivityCapsuleTemplateView");
        _controller = new ActivityCapsuleTemplateController { View = viewScript };
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
        //_controller.View.Show();
        Main.ChangeMenu(MainMenuDisplayState.ShowTopBar);
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

    public override void Hide()
    {
        _controller.View.Hide();
        base.Hide();
    }

    public void DestroyCountDown()
    {
        _controller.View.DestroyCountDown();
    }
}
