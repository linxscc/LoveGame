using Assets.Scripts.Framework.GalaSports.Core.Events;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module.Framework.Utils;
using Assets.Scripts.Module.NetWork;
using Com.Proto;
using Common;
using DataModel;
using game.main;
using game.tools;
using Google.Protobuf.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Utils;

public class MailReadWindow : Window
{
    private Text _titleText;
    private Text _senderText;
    private Text _createTimeText;
    private Text _validityText;    //有效期


    private Transform _noAttachment;
    private Transform _yesAttachment;
    
    private Button _getBtn;
    private Button _closeBtn;
    private Button _deleteBtn;
 
   

    private UserMailVO _data;

    private void Awake()
    {
        _titleText = transform.Find("Bg/Top/TitleText").GetComponent<Text>();
        _senderText = transform.Find("Bg/Top/SenderText").GetComponent<Text>();
        _createTimeText = transform.Find("Bg/Top/CreateTimeText").GetComponent<Text>();
        _validityText = transform.Find("Bg/Top/ValidityText").GetComponent<Text>();


        _noAttachment = transform.Find("Bg/Middle/NoAttachment");
        _yesAttachment = transform.Find("Bg/Middle/YesAttachment");
        
        
        _getBtn = transform.Find("Bg/Bottom/GetBtn").GetComponent<Button>();
        _closeBtn = transform.Find("Bg/Bottom/CloseBtn").GetComponent<Button>();
        _getBtn.onClick.AddListener(GetBtn);
        _closeBtn.onClick.AddListener(CloseBtn);

        _deleteBtn = transform.GetButton("Bg/Bottom/DeleteBtn");
        _deleteBtn.onClick.AddListener(DeleteBtn);

    }

    private void DeleteBtn()
    {

        var curTimeStamp = ClientTimer.Instance.GetCurrentTimeStamp();
        if (curTimeStamp<_data.OutDateTime)
        {
            //触发删除指定邮件
            ClearOneReq req =new ClearOneReq
            {
                Id = _data.Id
            };
            byte [] data = NetWorkManager.GetByteData(req);
            NetWorkManager.Instance.Send<ClearOneRes>(CMD.MAIL_CLEARONE, data, res =>
            {
                var id = res.UserMail.Id;
                EventDispatcher.TriggerEvent(EventConst.DeleteOneMail,id);
                Close();
            });
        }
        else
        {
            FlowText.ShowMessage(I18NManager.Get("Mail_MailOverdue"));// ("该邮件已过期");
            EventDispatcher.TriggerEvent(EventConst.MailPastDue, _data.Id);
            Close(); 
        }
        
    
    }


    public void SetData(UserMailVO vO)
    {
        _data = vO;

        _titleText.text = vO.Title;
        _senderText.text = vO.SenderName;
        _createTimeText.text = vO.CreateTimeStr;
        _validityText.text = vO.OutDataTimeStr;
     
        switch (vO.IsHaveAttachment)
        {
            case IsHaveAttachment.Yes:

               
                _yesAttachment.gameObject.SetActive(true);
                
                _yesAttachment.GetText("Content/Text").text= vO.Content;
                
                CreateMailAwardItem(vO.Awards);
                                                   
                if (vO.ReadStatus==0)//有附件，没领取
                {
                    _getBtn.gameObject.SetActive(true);               
                    _deleteBtn.gameObject.SetActive(false); 
                    _closeBtn.gameObject.SetActive(true);
                }
                else if(vO.ReadStatus==1)//有附件，领取了
                {
                    _getBtn.gameObject.SetActive(false);               
                    _deleteBtn.gameObject.SetActive(true); 
                    _closeBtn.gameObject.SetActive(true);
                    
                    Transform parent = _yesAttachment.Find("Award/Awards/Content");
                    for (int i = 0; i < parent.childCount; i++)
                    {
                        parent.GetChild(i).Find("Mask").gameObject.SetActive(true);
                    }
                }
                
                break;
            case IsHaveAttachment.No:
                
               _noAttachment.gameObject.SetActive(true);

               _noAttachment.GetText("Content").text = vO.Content;
               _deleteBtn.gameObject.SetActive(true); 
                _getBtn.gameObject.SetActive(false);
                _closeBtn.gameObject.SetActive(true);            
                break;           
        }
    }


    
    
    

    private void CreateMailAwardItem(List<MailAwardVO> Awards)
    {
        var mailAwardItme = GetPrefab("Mail/Prefabs/MailAwardItem");

        Transform parent = _yesAttachment.Find("Award/Awards/Content");
        
        for (int i = 0; i < Awards.Count; i++)
        {
            var go = Instantiate(mailAwardItme, parent, false) as GameObject;
            go.name = i.ToString();
            go.transform.localScale = Vector3.one;
            go.GetComponent<MailAwardItem>().SetData(Awards[i]);        
        }
    }

  

    private void GetBtn()
    {
        long curTimeStamp = ClientTimer.Instance.GetCurrentTimeStamp();
        if (curTimeStamp<_data.OutDateTime)
        {
            _getBtn.gameObject.SetActive(false);
            _closeBtn.gameObject.SetActive(true);
            SendMailGottenReq();
         
        }
        else
        {
            FlowText.ShowMessage(I18NManager.Get("Mail_MailOverdue"));// ("该邮件已过期");
            EventDispatcher.TriggerEvent(EventConst.MailPastDue, _data.Id);
            Close();
        }      
    }

    private void CloseBtn()
    {
        Close();        
    }


    private void SendMailGottenReq()
    {
        
        Transform parent = _yesAttachment.Find("Award/Awards/Content");
        
        MailGottenReq req = new MailGottenReq
        {
            Id = _data.Id,        
        };

        byte [] data = NetWorkManager.GetByteData(req);
        NetWorkManager.Instance.Send<MailGottenRes>(CMD.MAIL_GOTTEN, data, res => {
            if (res.UserMail.Awards.Count!=0)
            {
                for (int i = 0; i < res.UserMail.Awards.Count; i++)
                {
                    RewardUtil.AddReward(res.UserMail.Awards[i]);
                }
                FlowText.ShowMessage(I18NManager.Get("Mail_GetAward"));//("奖励已领取");
                for (int i = 0; i < parent.childCount; i++)
                {
                    parent.GetChild(i).Find("Mask").gameObject.SetActive(true);
                }

                StatisticsMailGetGemNums(res.UserMail.Awards.ToList());
            }

            _data.ReadStatus = res.UserMail.MailStatus;
            HideBtn();
            EventDispatcher.TriggerEvent(EventConst.GetOneMailAwardSucceed,_data);
            
          
        });
    }

    private void HideBtn()
    {
        _getBtn.gameObject.Hide();
        _deleteBtn.gameObject.Show();
    }
    
    /// <summary>
    /// 统计邮件获得钻石的数量
    /// </summary>
    /// <param name="list"></param> 
    private void StatisticsMailGetGemNums(List<AwardPB> list)
    {
        int num = 0;
        string reason = "邮件";
        foreach (var t in list)
        {
            if (t.Resource == ResourcePB.Gem)
            {
                num +=t.Num;
            } 
        }
        
        if (num!=0)
        {
            SdkHelper.StatisticsAgent.OnReward(num, reason);
        }        
    }
}
