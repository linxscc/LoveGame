using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Common;
using Assets.Scripts.Framework.GalaSports.Interfaces;
using Assets.Scripts.Framework.GalaSports.Core;
using Com.Proto;
using DataModel;
using UnityEngine;
using game.main;
using UnityEngine.U2D;
using System;

public class DrawPanel : ReturnablePanel
{
    private DrawController _control;
    private DrawController2 _control2;

    public override void Init(IModule module)
    {
        //AssetManager.Instance.LoadAtlas("uiatlas_drawcard2");

        base.Init(module);

        //InitDraw1();
        InitDraw2();
        HideBackBtn();    
    }

    private void InitDraw1()
    {
        IView viewScript = InstantiateView<DrawView>("DrawCard/Prefabs/DrawView");
        _control = new DrawController();
        _control.DrawView = (DrawView)viewScript;
        _control.Start();
        RegisterController(_control);
    }
    private void InitDraw2()
    {
        IView viewScript = InstantiateView<DrawView2>("DrawCard/Prefabs/DrawView2");
        _control2 = new DrawController2();
        _control2.DrawView = (DrawView2)viewScript;
        _control2.Start();
        RegisterController(_control2);
    }



    public override void OnBackClick()
    {
        base.OnBackClick();
    }

    /// <summary>
    /// 新动画
    /// </summary>
    /// <param name="list"></param>
    public void Show2(List<DrawCardResultVo> list)
    {
        base.Show(0);
        _control2.SetData(list);
        Main.ChangeMenu(MainMenuDisplayState.HideAll);
    }


    public void Show(DrawTypePB res,long gemRefershTime)
    {
        base.Show(0);
        _control.DrawType = res;
        _control.GemRefreshTime = gemRefershTime;
        //_control.DrawView.Show(res, GlobalData.CardModel);
        _control.DrawView.Show();
        Main.ChangeMenu(MainMenuDisplayState.HideAll);
    }
    public override void Hide()
    {
         _control2.DrawView.Hide();
        base.Hide();
    }
    public override void Destroy()
    {
        UnregisterController(_control2);
        base.Destroy();
        //_control.Destroy();
        //_control = null;
    }

}