using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Module.NetWork;
using Common;
using System.Collections.Generic;

public class PhoneModule : ModuleBase {
    private PhonePanel _phonePanel;

    public override void Init()
    {
        if (data != null)
        {
            _phonePanel = new PhonePanel(PhonePanelType.Tips);
        }
        else
        {
            _phonePanel = new PhonePanel();
        }

        //todo 这里做识别是tips 还是手机按钮
        _phonePanel.Init(this);
        _phonePanel.Show(0.5f);

        GuideManager.RegisterModule(this);
        if (data != null)
        {
           _phonePanel.ShowPhoneCallViewByTips(data);
            data = null;
        }
    }

    public override void OnMessage(Message message)
    {
        string name = message.Name;
        object[] body = message.Params;
        switch (name)
        {
            case MessageConst.CMD_PHONE_SHOW_SMS:
                _phonePanel.ShowSmsView();
                break;           
            case MessageConst.CMD_PHONE_SHOW_TELE:
                _phonePanel.ShowTelephoneView();
                break;
            case MessageConst.CMD_PHONE_CALL_HANGUP:
                //  _phonePanel.OnBackClick();
                //_phoneCallController.Hide();
                //_telephoneController.Show();
                //_telephoneController.Refresh();
                if(_phonePanel.PanelType==PhonePanelType.Normal)
                {
                    _phonePanel.ShowTelephoneView();
                    _phonePanel.ShowBackBtn();
                }
                else
                {
                    ModuleManager.Instance.GoBack();
                }

                break;
            case MessageConst.CMD_PHONE_SMS_SHOWDETAIL:
                List<MySmsOrCallVo> data = new List<MySmsOrCallVo>();
                data = (List<MySmsOrCallVo>)body[0];
                _phonePanel.ShowSmsDetailView(data);
                break;          
            case MessageConst.CMD_PHONE_CALL_SHOWDETAIL:
                MySmsOrCallVo data1 = (MySmsOrCallVo) body[0];
                _phonePanel.ShowPhoneCallView(data1);
                break;
            
            case MessageConst.CMD_PHONE_SHOW_FRIENDS:
                _phonePanel.ShowFriendsCircleView();
                break;

            case MessageConst.CMD_PHONE_SHOW_WEIBO:
                _phonePanel.ShowWeiboView();
                break;

            case MessageConst.CMD_PHONE_GUIDE_BACKBTN:
                _phonePanel.OnBackClick();
                break;
        }
    }
    MySmsOrCallVo data = null;
    public override void SetData(params object[] paramsObjects)
    {
        if (paramsObjects.Length == 0)
            return;
        data = (MySmsOrCallVo)paramsObjects[0];
    
    }
    public override void OnShow(float delay)
    {
        base.OnShow(delay);
        _phonePanel.ShowBackBtn();
    }

    public override void Remove(float delay)
    {
        base.Remove(delay);
        _phonePanel.Destroy();
    }
    
}
