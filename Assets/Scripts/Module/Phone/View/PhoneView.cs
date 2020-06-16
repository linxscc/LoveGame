using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Module.Framework.Utils;
using UnityEngine.UI;

public class PhoneView : View
{
    private void Awake()
    {
        transform.Find("FunctionBar/Sms/Label").GetComponent<Text>().color = ColorUtil.HexToColor("DA7DED");
        transform.Find("FunctionBar/Sms").GetComponent<Toggle>().onValueChanged.AddListener(delegate(bool b)
        {
            transform.Find("FunctionBar/Sms/Label").GetComponent<Text>().color =
                b ? ColorUtil.HexToColor("DA7DED") : ColorUtil.HexToColor("808080");
            if(b)
                SendMessage(new Message(MessageConst.CMD_PHONE_SHOW_SMS));
        });
        
        transform.Find("FunctionBar/Telephone").GetComponent<Toggle>().onValueChanged.AddListener(delegate(bool b)
        {
            transform.Find("FunctionBar/Telephone/Label").GetComponent<Text>().color =
                b ? ColorUtil.HexToColor("DA7DED") : ColorUtil.HexToColor("808080");
            if(b)
                SendMessage(new Message(MessageConst.CMD_PHONE_SHOW_TELE));
        });
        
        transform.Find("FunctionBar/Friends").GetComponent<Toggle>().onValueChanged.AddListener(delegate(bool b)
        {
            transform.Find("FunctionBar/Friends/Label").GetComponent<Text>().color =
                b ? ColorUtil.HexToColor("DA7DED") : ColorUtil.HexToColor("808080");
            
            if(b)
                SendMessage(new Message(MessageConst.CMD_PHONE_SHOW_FRIENDS));
        });
        
        transform.Find("FunctionBar/Weibo").GetComponent<Toggle>().onValueChanged.AddListener(delegate(bool b)
        {
            transform.Find("FunctionBar/Weibo/Label").GetComponent<Text>().color =
                b ? ColorUtil.HexToColor("DA7DED") : ColorUtil.HexToColor("808080");
            
            if(b)
                SendMessage(new Message(MessageConst.CMD_PHONE_SHOW_WEIBO));
        });
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