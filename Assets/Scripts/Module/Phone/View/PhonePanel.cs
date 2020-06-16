using Assets.Scripts.Common;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Interfaces;
using System;
using System.Collections.Generic;
using UnityEngine;

public enum PhonePanelType
{
    Normal,
    Tips
}

public class PhonePanel : ReturnablePanel
{

    public PhonePanel(PhonePanelType t= PhonePanelType.Normal)
    {
        PanelType = t;
    }

    public PhonePanelType PanelType
    {
        get;
        set;
    }

    private PhoneController control;

    private SmsController _smsController;
    private TelephoneController _telephoneController;
	private SmsDetailController _smsDetailController;
    private PhoneCallController _phoneCallController;
    private FriendsCircleController _friendsCircleController;
    private WeiboController _weiboController;

    public override void Init(IModule module)
    {
        SetComplexPanel();
        base.Init(module);

        var viewScript = InstantiateView<PhoneBgView>("Phone/Prefabs/PhoneBgView");

        control = new PhoneController();
        control.BgView = (PhoneBgView) viewScript;
        RegisterController(control);
        //RegisterModel<PhoneData>();弃用 在进入程序是初始化

        if (PanelType == PhonePanelType.Normal)
            control.Start();
    }

    public void ShowSmsView()
    {
        HideAll();
        if (_smsController == null)
        {
            var view = InstantiateView<SmsView>("Phone/Prefabs/SmsView");
            _smsController=new SmsController();
            _smsController.View = (SmsView) view;
            RegisterController(_smsController);
            _smsController.Start();
        }
        else
        {
            _smsController.Show();
        }
        ShowBackBtn();
    }
    
    public void ShowTelephoneView()
    {
        HideAll();
        if (_telephoneController == null)
        {
            var view = InstantiateView<TelephoneView>("Phone/Prefabs/TelephoneView");
            _telephoneController=new TelephoneController();
            _telephoneController.View = (TelephoneView) view;
            RegisterController(_telephoneController);
            _telephoneController.Start();
        }
        else
        {
            _telephoneController.Show();
            _telephoneController.Refresh();
        }
        ShowBackBtn();
    }

    public void ShowPhoneCallViewByTips(MySmsOrCallVo data)
    {
        if(data.SceneId<10000)
        {
            List<MySmsOrCallVo> ldata = new List<MySmsOrCallVo>() { data };
            ShowSmsDetailView(ldata);
        }
        else
        {
            ShowPhoneCallView(data);
        }
    }

    public void ShowPhoneCallView(MySmsOrCallVo data)
    {
        HideAll();
        if (_phoneCallController != null)
        {
            _phoneCallController.SetData(data);
            _phoneCallController.Show();
        }
        else
        {
            var view = InstantiateView<PhoneCallView>("Phone/Prefabs/PhoneCallView");
            _phoneCallController=new PhoneCallController();
            _phoneCallController.View = (PhoneCallView) view;
            RegisterController(_phoneCallController);
            _phoneCallController.Start();
            _phoneCallController.SetData(data);
        }
        HideBackBtn();
    }
    
    public void ShowSmsDetailView(List<MySmsOrCallVo> data)
    {
        HideAll();
        if (_smsDetailController != null)
        {
            _smsDetailController.SetData(data);
            _smsDetailController.Show();
        }
        else
        {
            var view = InstantiateView<SmsDetailView>("Phone/Prefabs/SmsDetailView");
            _smsDetailController=new SmsDetailController();
            _smsDetailController.View = (SmsDetailView) view;
            RegisterController(_smsDetailController);
            _smsDetailController.Start();
            _smsDetailController.SetData(data);
        }
        control.BgView.ShowBottom(false);

        ShowBackBtn();
    }
    /// <summary>
    /// 显示朋友圈
    /// </summary>
    public void ShowFriendsCircleView()
    {
        Debug.Log("ShowFriendsCircleView");
        HideAll();
        if (_friendsCircleController == null)
        {
            var view = InstantiateView<FriendsCircleView>("Phone/Prefabs/FriendsCircleView");
            _friendsCircleController = new FriendsCircleController();
            _friendsCircleController.View = (FriendsCircleView)view;
            RegisterController(_friendsCircleController);
            _friendsCircleController.Start();
        }
        else
        {
            _friendsCircleController.Show();
        }
        ShowBackBtn();
        //todo
    }

    /// <summary>
    /// 显示微博
    /// </summary>
    public void ShowWeiboView()
    {
        Debug.Log("ShowWeiboView");
        HideAll();
        if (_weiboController == null)
        {
            var view = InstantiateView<WeiboView>("Phone/Prefabs/WeiboView");
            _weiboController = new WeiboController();
            _weiboController.View = (WeiboView)view;
            RegisterController(_weiboController);
            _weiboController.Start();
        }
        else
        {
            _weiboController.Show();
        }
        ShowBackBtn();
    }



    private void HideAll()
    {
        if(_smsController!=null)
            _smsController.Hide();
        
        if(_telephoneController!=null)
            _telephoneController.Hide();
        if (_friendsCircleController != null)
            _friendsCircleController.Hide();
        if (_weiboController != null)
            _weiboController.Hide();
        if(_phoneCallController!=null)
            _phoneCallController.Hide();

    }

    public override void Hide()
    {
        base.Hide();
    }

    public override void Show(float delay)
    {
        Main.ChangeMenu(MainMenuDisplayState.HideAll);
        ShowSmsView();
    }

    public override void OnBackClick()
    {
        // base.OnBackClick();

        if(PanelType==PhonePanelType.Tips)
        {
            ModuleManager.Instance.GoBack();
            return;
        }

        if (_smsDetailController != null)
        {
            if (_smsDetailController.IsHide)
            {
                base.OnBackClick();
            }
            else
            {
                _smsDetailController.Hide();
                _smsController.Show();
                _smsController.Refresh();
                control.BgView.ShowBottom(true);
            }
        }else   if (_phoneCallController != null)
        {
            if (_phoneCallController.IsHide)
            {
                base.OnBackClick();
            }
            else
            {
                _phoneCallController.Hide();
                _telephoneController.Show();
                _telephoneController.Refresh();
            }
        }
        else
        {
            base.OnBackClick();
        }
    }
}
