using Assets.Scripts.Module.Framework.Utils;
using Com.Proto;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum IsHaveAttachment
{
    Yes,
    No
}

public class UserMailVO
{
    public int Id;
    public string Title;
    public string Content;
    public List<MailAwardVO> Awards =new List<MailAwardVO>();
    public int SenderId;
    public string SenderName;
    public long CreateTime;
    public long OutDateTime;
    public IsHaveAttachment IsHaveAttachment;
    public string CreateTimeStr;
    public string OutDataTimeStr;
    public int ReadStatus; //邮件阅读状态
    
    public UserMailVO(UserMailPB pB)
    {
        Id = pB.Id;
        Title = pB.Title;
       
      
        SenderId = pB.SenderId;
        SenderName = pB.Sender;
        CreateTime = pB.CreateTime;
        OutDateTime = pB.OutDateTime;
        UserMailIsHaveAttachment(pB.HasAttachment);
        ReadStatus = pB.MailStatus;

         CreateTimeStr = GetCreateDate(CreateTime);
        GetValidityTime();
    }

    private void UserMailIsHaveAttachment(int t)
    {
        if (t==0)   //没有附件
        {
            IsHaveAttachment = IsHaveAttachment.No;
        }
        else if (t==1)   //有附件
        {
            IsHaveAttachment = IsHaveAttachment.Yes;
        }
    }


    public  void AddUserMailAwardData(List<AwardPB> pBs)
    {
        bool contain = false;     
        foreach (var v in pBs)
        {
            foreach (var u in Awards)
            {
                if (u.Reward.Id== v.ResourceId)
                {
                    u.Reward.Num += v.Num;
                    contain = true;
                    break;
                }

                contain = false;
            }

            if (!contain)
            {
                var item = new MailAwardVO(v);
                  Awards.Add(item);
            }
        }
      
    }

    private string GetCreateDate(long createTime)
    {
        var t = DateUtil.GetDataTime(createTime);
        return t.Year + "." + t.Month + "." + t.Day;
    }

    private void GetValidityTime()
    {
        long curTimeStamp = ClientTimer.Instance.GetCurrentTimeStamp();
        long endTimeStamp = OutDateTime;
        long difference = endTimeStamp - curTimeStamp;

        var day = difference / 86400000;
        var hout = (difference / 3600000) - day * 24;

        if (day == 0)
        {
            if (hout==0)
            {
                
                OutDataTimeStr = I18NManager.Get("Mail_ValidTime1");
            }
            else
            {
                //OutDataTimeStr = $"<size=30><color=#fe4c4c>有效期：{hout.ToString()}小时</color></size>";
                OutDataTimeStr = I18NManager.Get("Mail_ValidTime2", hout.ToString());
            }
           
        }
        else
        {
            OutDataTimeStr = I18NManager.Get("Mail_ValidTime3", day.ToString(), hout.ToString());
            //OutDataTimeStr = $"<size=30><color=#fe4c4c>有效期：{day.ToString()}天{hout.ToString()}小时</color></size>";
        }

    }
}
