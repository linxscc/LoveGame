using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Module.Download;
using Assets.Scripts.Module.Framework.Utils;
using game.main;
using UnityEngine;
using UnityEngine.UI;

public class PhoneBgView : View
{

    GameObject rightDownload;
    private void Awake()
    {
        rightDownload = transform.Find("RightTop").gameObject;
        transform.Find("FunctionBar/Sms/Label").GetComponent<Text>().color = ColorUtil.HexToColor("DA7DED");
        transform.Find("FunctionBar/Sms").GetComponent<Toggle>().onValueChanged.AddListener(delegate(bool b)
        {
            transform.Find("FunctionBar/Sms/Label").GetComponent<Text>().color =
                b ? ColorUtil.HexToColor("DA7DED") : ColorUtil.HexToColor("808080");
            if(b)
            {
                SendMessage(new Message(MessageConst.CMD_PHONE_SHOW_SMS));
                SendMessage(new Message(MessageConst.CMD_PHONE_HIDE_BG_REDPOINT, PhoneModeType.Sms));
                CheckIsShowDownload();
            }
    
        });
        
        transform.Find("FunctionBar/Telephone").GetComponent<Toggle>().onValueChanged.AddListener(delegate(bool b)
        {
            transform.Find("FunctionBar/Telephone/Label").GetComponent<Text>().color =
                b ? ColorUtil.HexToColor("DA7DED") : ColorUtil.HexToColor("808080");
            if(b)
            {
                SendMessage(new Message(MessageConst.CMD_PHONE_SHOW_TELE));
                SendMessage(new Message(MessageConst.CMD_PHONE_HIDE_BG_REDPOINT, PhoneModeType.Call));
                CacheVo vo = CacheManager.CheckPhoneCache();
                CheckIsShowDownload();
            }
      
        });
        
        transform.Find("FunctionBar/Friends").GetComponent<Toggle>().onValueChanged.AddListener(delegate(bool b)
        {
            transform.Find("FunctionBar/Friends/Label").GetComponent<Text>().color =
                b ? ColorUtil.HexToColor("DA7DED") : ColorUtil.HexToColor("808080");

            if (b)
            {
                SendMessage(new Message(MessageConst.CMD_PHONE_SHOW_FRIENDS));
                SendMessage(new Message(MessageConst.CMD_PHONE_HIDE_BG_REDPOINT, PhoneModeType.Friend));
                rightDownload.Hide();
            }
        });
        
        transform.Find("FunctionBar/Weibo").GetComponent<Toggle>().onValueChanged.AddListener(delegate(bool b)
        {
            transform.Find("FunctionBar/Weibo/Label").GetComponent<Text>().color =
                b ? ColorUtil.HexToColor("DA7DED") : ColorUtil.HexToColor("808080");
            if (b)
            {
                SendMessage(new Message(MessageConst.CMD_PHONE_SHOW_WEIBO));
                SendMessage(new Message(MessageConst.CMD_PHONE_HIDE_BG_REDPOINT, PhoneModeType.Weibo));
                rightDownload.Hide();
            }
        });

        transform.Find("RightTop/Download").GetButton().onClick.AddListener(() => {
            PopupManager.ShowWindow<PhoneDownloadTipsWindow>("Prefabs/PhoneDownloadTipsWindow").WindowActionCallback=evt =>
            {
                CheckIsShowDownload();
            };
        });
      
    }


    void CheckIsShowDownload()
    {
        CacheVo vo = CacheManager.CheckPhoneCache();
        if (vo.needDownload)
        {
            rightDownload.Show();
        }
        else
        {
            rightDownload.Hide();
        }
    }

    private void Start()
    {
        SendMessage(new Message(MessageConst.CMD_PHONE_HIDE_BG_REDPOINT, PhoneModeType.Sms));
        CheckIsShowDownload();
    }

    public void SetRedPoint()
    {
        transform.Find("FunctionBar/Sms/RedPoint").gameObject.SetActive(Util.GetIsRedPoint(Constants.REDPOINT_PHONE_BGVIEW_SMS));
        transform.Find("FunctionBar/Telephone/RedPoint").gameObject.SetActive(Util.GetIsRedPoint(Constants.REDPOINT_PHONE_BGVIEW_CALL));
        transform.Find("FunctionBar/Friends/RedPoint").gameObject.SetActive(Util.GetIsRedPoint(Constants.REDPOINT_PHONE_BGVIEW_FC));
        transform.Find("FunctionBar/Weibo/RedPoint").gameObject.SetActive(Util.GetIsRedPoint(Constants.REDPOINT_PHONE_BGVIEW_WEIBO));
    }

    public void ShowBottom(bool B)
    {
        transform.Find("FunctionBar").gameObject.SetActive(B);
    }

    public override void Show(float delay = 0)
    {

        base.Show(delay);
    }
}