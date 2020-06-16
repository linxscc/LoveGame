using Assets.Scripts.Framework.GalaSports.Core;
using Com.Proto;
using DataModel;
using game.main;
using Google.Protobuf.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public enum UserMailState
{
    NoMail,         //没有邮件
    HaveAttachment, //有附件
    NoAttachment,   //没附件 
}

public class MailModel : Model
{
    public List<UserMailVO> UserMails;   //用户邮件集合
    public UserMailState UserMailState;   //用户邮件状态
  
    public void Init(UserMailRes res)
    {
      
        UserMails =new List<UserMailVO>();
        foreach (var t in res.UserMails)
        {
            var item = new UserMailVO(t);         
            UserMails.Add(item);
        }
        
        Debug.LogError("UserMails.Count===>"+UserMails.Count);
       
        SetUserMailState();
        SetSort();
    }


    public void SetUserMailState()
    {
        if (UserMails.Count == 0) { UserMailState = UserMailState.NoMail; } 
        else
        {
            foreach (var t in UserMails)
            {
                if (t.IsHaveAttachment == IsHaveAttachment.Yes)
                {
                    UserMailState = UserMailState.HaveAttachment;
                    break;
                }
                else
                {
                    UserMailState = UserMailState.NoAttachment;
                }
            }
        }
    }


    /// <summary>
    /// 更新邮件阅读状态
    /// </summary>
    public void UpdateMailReadState(int id,int readStatus)
    {
        foreach (var t in UserMails)
        {
            if (t.Id==id)
            {
                t.ReadStatus = readStatus;
                break;
            }
        }

        SetSort();
    }


    public void DeleteUserMails(int id)
    {
        for (int i = 0; i < UserMails.Count; i++)
        {
            if (UserMails[i].Id==id)
            {              
               UserMails.Remove(UserMails[i]);
               break;  
            } 
        }

        SetSort();
        SetUserMailState();
        
    }


    private void SetSort()
    {
        ReadSort();
        NoReadCreateTimeSort();
       
    }

    //读的排序。（未读放前面，已读放后面）
    private void ReadSort()
    {
        UserMails.Sort((UserMailVO x, UserMailVO y) =>x.ReadStatus.CompareTo(y.ReadStatus));
        
    }

    //未读按创建时间排序 （创建时间越早放到后面）
    private void NoReadCreateTimeSort()
    {
        for (int i = 0; i < UserMails.Count; i++)
        {          
            for (int j = i+1; j < UserMails.Count; j++)
            {     
                if (UserMails[i].ReadStatus!=1 && UserMails[j].ReadStatus!=1)
                {              
                 if (UserMails[i].CreateTime<UserMails[j].CreateTime)
                 {
                      var temp = UserMails[i];
                      UserMails[i] = UserMails[j];
                      UserMails[j] = temp;
                 }
                }
                
            }
        }      
    }

    
    
    
    public bool IsReadMail()
    {
        bool isRead = false;
        foreach (var t in UserMails)
        {
            if (t.ReadStatus == 1)
            {
                isRead = true;
                break;
            }
        }

        return isRead;

    }
    
}
