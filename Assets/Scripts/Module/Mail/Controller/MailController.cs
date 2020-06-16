using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Core.Events;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module.NetWork;
using Com.Proto;
using Common;
using Componets;
using DataModel;
using game.main;
using UnityEngine;
using Utils;

public class MailController : Controller
{

    public MailWinodw Winodw;
    
    
    private MailReadWindow _mailReadWindow;
    private AKeyToDeleteWindow _aKeyToDeleteWindow;
    private AKeyToGetWindow _aKeyToGetWindow;
    private MailModel _model;
    public override void Init()
    {
        EventDispatcher.AddEventListener<UserMailVO>(EventConst.MailItemOnClick, MailItemOnClick);
        EventDispatcher.AddEventListener<int>(EventConst.MailPastDue, MailPastDue);
        EventDispatcher.AddEventListener(EventConst.DeleteReadMail, DeleteReadMail);
        EventDispatcher.AddEventListener<UserMailVO>(EventConst.GetOneMailAwardSucceed,GetOneMailAwardSucceed);
        EventDispatcher.AddEventListener<int>(EventConst.DeleteOneMail,DeleteOneMail);
    }

    /// <summary>
    /// 删除一封邮件
    /// </summary>
    /// <param name="id"></param>
    private void DeleteOneMail(int id)
    {
       _model.DeleteUserMails(id); 
       Winodw.DestroyOneMailItem(id);
       Winodw.IsHaveUserMail(_model.UserMailState); 
    }

    /// <summary>
    /// 领取一封邮件奖励成功
    /// </summary>
    /// <param name="vo"></param>
    private void GetOneMailAwardSucceed(UserMailVO vo)
    {
        _model.UpdateMailReadState(vo.Id,vo.ReadStatus);
       Winodw.SetData(_model.UserMailState,_model.UserMails);
    }

    public override void Start()
    {
        _model =GetData<MailModel>();       
        Winodw.SetData(_model.UserMailState,_model.UserMails);
               
        ClientData.LoadItemDescData(null);
        ClientData.LoadSpecialItemDescData(null);
    }

    public override void OnMessage(Message message)
    {
        string name = message.Name;
        object[] body = message.Params;
        switch (name)
        {
            case MessageConst.CMD_MAIL_A_KEY_TO_GET:
                AKeyToGet();
                break;
            case MessageConst.CMD_MAIL_A_KEY_TO_DELETE:
                AKeyToDelete();
                break;
        }
    }

    public override void Destroy()
    {
        EventDispatcher.RemoveEvent(EventConst.MailItemOnClick);
        EventDispatcher.RemoveEvent(EventConst.MailPastDue);
        EventDispatcher.RemoveEvent(EventConst.DeleteReadMail);
        EventDispatcher.RemoveEvent(EventConst.GetOneMailAwardSucceed);
        ClientData.Clear();
    }


    private void MailItemOnClick(UserMailVO vO)
    {
        _model.UpdateMailReadState(vO.Id,vO.ReadStatus);
        Winodw.SetData(_model.UserMailState,_model.UserMails);
        //打开邮件阅读窗口
        if (_mailReadWindow==null)
        {
            _mailReadWindow = PopupManager.ShowWindow<MailReadWindow>("Mail/Prefabs/MailReadWindow");
            _mailReadWindow.SetData(vO);
        }
    }

    private void MailPastDue(int id)
    {
        _model.DeleteUserMails(id);
        Winodw.SetData(_model.UserMailState,_model.UserMails);
    }
    

    private void AKeyToGet()
    {
        NetWorkManager.Instance.Send<MailGetAllRes>(CMD.MAIL_GETALL, null, OnGetMailGetAllRes);
    }
   
    private void AKeyToDelete()
    {
        if (!_model.IsReadMail())
        {
            FlowText.ShowMessage(I18NManager.Get("Mail_NoReadMail"));
            return;
        }
        
        if (_aKeyToDeleteWindow==null) //只是打开一键删除确认窗口
        {
            _aKeyToDeleteWindow= PopupManager.ShowWindow<AKeyToDeleteWindow>("Mail/Prefabs/AKeyToDeleteWindow");
        }
    }
 
    private void OnGetMailGetAllRes(MailGetAllRes res)
    {       
        var userMailAllAwards = new List<MailAwardVO>();

         //添加奖励
        foreach (var t in res.Awards)
        {
            var item = new MailAwardVO(t);
            userMailAllAwards.Add(item);
            RewardUtil.AddReward(t);
        }


        foreach (var t in res.UserMails)
        {
            _model.UpdateMailReadState(t.Id,t.MailStatus);
        }
        
        //刷新ui
        Winodw.SetData(_model.UserMailState,_model.UserMails);
        


        if (_aKeyToGetWindow==null)
        {
            _aKeyToGetWindow = PopupManager.ShowWindow<AKeyToGetWindow>("Mail/Prefabs/AKeyToGetWindow");
            _aKeyToGetWindow.SetData(userMailAllAwards.Count, userMailAllAwards);
        }

        StatisticsMailGetGemNums(res.Awards.ToList());

    }


    /// <summary>
    /// 统计邮件获得钻石的数量
    /// </summary>
    /// <param name="list"></param>
    private void StatisticsMailGetGemNums(List<AwardPB> list)
    {
        int num = 0;
        string reason = I18NManager.Get("Mail_Title");
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
    
    

    private void DeleteReadMail()
    {
        NetWorkManager.Instance.Send<MailClearRes>(CMD.MAIL_CLEAR, null, OnGetMailClearRes);
    }


    private void OnGetMailClearRes(MailClearRes res)
    {
        foreach (var t in res.UserMails)
        {
            _model.DeleteUserMails(t.Id);
        }

        Debug.LogError("用户的邮件数量===>"+_model.UserMails.Count);
        Winodw.SetData(_model.UserMailState,_model.UserMails);
        FlowText.ShowMessage(I18NManager.Get("Mail_DeletSucceed"));
    }
}
