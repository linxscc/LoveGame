using Assets.Scripts.Framework.GalaSports.Core.Events;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module.Framework.Utils;
using Assets.Scripts.Module.NetWork;
using Com.Proto;
using Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using game.main;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UserMailItem : MonoBehaviour, IPointerClickHandler
{

    private Text _titleText;
    private Text _senderText;
    private Text _sendTimeText;
    private Text _validityText;  //有效期
    private UserMailVO _data;
    private void Awake()
    {

        _titleText = transform.Find("Bg/TitleText").GetComponent<Text>();
        _senderText = transform.Find("Bg/SenderText").GetComponent<Text>();
        _sendTimeText = transform.Find("Bg/SendTimeText").GetComponent<Text>();
        _validityText = transform.Find("Bg/ValidityText").GetComponent<Text>();
    }

 



    public void SetData(UserMailVO vO)
    {
        _data = vO;
        _titleText.text = vO.Title;
        _senderText.text = vO.SenderName;
        _sendTimeText.text = vO.CreateTimeStr;
        _validityText.text = vO.OutDataTimeStr;
        SetReadStatus(vO.ReadStatus);
    }

    private void SetReadStatus(int readStatus)    
    {
       // readStatus 0是未读 ，1是已读
       var bg = transform.GetImage("Bg");
       var icon = transform.GetImage("Bg/Icon");

       if (readStatus==0)
       {
           bg.sprite =  AssetManager.Instance.GetSpriteAtlas("UIAtlas_Mail_NoReadBg");
           icon.sprite =AssetManager.Instance.GetSpriteAtlas("UIAtlas_Mail_NoReadIcon");
       }
       else if(readStatus==1)
       {
           bg.sprite =  AssetManager.Instance.GetSpriteAtlas("UIAtlas_Mail_ReadBg");
           icon.sprite =AssetManager.Instance.GetSpriteAtlas("UIAtlas_Mail_ReadIcon");   
       }
    }

    public void OnPointerClick(PointerEventData eventData)
    {

        var curTimeStamp = ClientTimer.Instance.GetCurrentTimeStamp();
        if (curTimeStamp<_data.OutDateTime)
        {
            SendMailReadReq();  
        }
        else
        {
            FlowText.ShowMessage(I18NManager.Get("Mail_MailOverdue"));
            EventDispatcher.TriggerEvent(EventConst.MailPastDue, _data.Id);
        }
        
        
       
    }

    
    
    private void SendMailReadReq()
    {
        MailReadReq req = new MailReadReq
        {
            Id = _data.Id,
        };
        byte[] data = NetWorkManager.GetByteData(req);
        NetWorkManager.Instance.Send<MailReadRes>(CMD.MAIL_READ, data, res => {
            _data.Awards.Clear();
            _data.Content = "";
                                  
            _data.AddUserMailAwardData(res.UserMail.Awards.ToList());
            _data.Content =  Util.GetNoBreakingString(res.UserMail.Content) ;//邮件处理换行空格

            Debug.LogError("读邮件回包奖励集合长度："+ res.UserMail.Awards.Count);

            _data.ReadStatus = res.UserMail.MailStatus;
            SetReadStatus(res.UserMail.MailStatus);
            
            EventDispatcher.TriggerEvent<UserMailVO>(EventConst.MailItemOnClick,_data);            
        });
    }

}
