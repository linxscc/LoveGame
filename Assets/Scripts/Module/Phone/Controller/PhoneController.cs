using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module.NetWork;
using Com.Proto;
using DataModel;
using game.main;
using System;

public class PhoneController : Controller {
	
	public PhoneBgView BgView;

	public override void Start()
    {
        //GlobalData.PhoneData.InitIsRedPoint();
        SetRedPoint();
    }

    private void SetRedPoint()
    {
        BgView.SetRedPoint();
    }

    /// <summary>
    /// 处理View消息
    /// </summary>
    /// <param name="message"></param>
    public override void OnMessage(Message message)
    {
        string name = message.Name;
        object[] body = message.Params;
        switch (name)
        {
            case MessageConst.CMD_PHONE_SMS_CUR:
                BgView.ShowBottom(true);
                break;          
            //case MessageConst.CMD_PHONE_SMS_HISTORY:
                //BgView.ShowBottom(false);
                //break;
            case MessageConst.CMD_PHONE_SMS_DETAIL:
                BgView.ShowBottom(false);
                break;
            case MessageConst.CMD_PHONE_HIDE_BG_REDPOINT:
                PhoneModeType phoneModeType = (PhoneModeType)message.Body;
                switch(phoneModeType)
                {
                    case PhoneModeType.Sms:
                        Util.SetIsRedPoint(Constants.REDPOINT_PHONE_BGVIEW_SMS, false);
                        break;
                    case PhoneModeType.Call:
                        Util.SetIsRedPoint(Constants.REDPOINT_PHONE_BGVIEW_CALL, false);
                        break;
                    case PhoneModeType.Friend:
                        Util.SetIsRedPoint(Constants.REDPOINT_PHONE_BGVIEW_FC, false);
                        break;
                    case PhoneModeType.Weibo:
                        Util.SetIsRedPoint(Constants.REDPOINT_PHONE_BGVIEW_WEIBO, false);
                        break;
                }
                SetRedPoint();
                break;
        }
    }
}
