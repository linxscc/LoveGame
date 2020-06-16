using System;
using System.Collections.Generic;
//using Assets.Scripts.Module.Phone.Data;
using Com.Proto;
using DataModel;
using UnityEngine;

/// <summary>
/// 一个对话场景
/// </summary>
public class MySmsOrCallVo : IComparable<MySmsOrCallVo>
{

    public int UserId;
    public int NpcId { set; get; } //跟哪个npc对话
    public int SceneId; //1000--9999 短信      10000-19999电话   20000-29999朋友圈   30000-39999微博
    public long CreateTime;
    public long FinishTime;
    public bool IsReaded;
    //  public string SceneName;
    public List<int> selectIds;
    public List<int> listenIds;
    public bool IsLocal;//是否是本地引导
    public SmsInfo SmsRuleInfo
    {
        get
        {
            if(smsRuleInfo==null)
            {
                smsRuleInfo = PhoneData.LoadPhoneSmsRuleById(SceneId);
            }
            return smsRuleInfo;
        }
    }
    private SmsInfo smsRuleInfo;
    //public MsgOrCallVo CurrentTalk; //对话ID
    public SmsTalkInfo FirstTalkInfo
    {
        get
        {
            SmsTalkInfo startInfo = SmsRuleInfo.smsTalkInfos[0];
            for (int i=1;i< SmsRuleInfo.smsTalkInfos.Count;i++)
            {
                if (SmsRuleInfo.smsTalkInfos[i].TalkId< startInfo.TalkId)
                {
                    startInfo = SmsRuleInfo.smsTalkInfos[i];
                }
            }
            return startInfo;
         //   return SmsRuleInfo.smsTalkInfos.Find((m) => { return m.TalkId == 101 || m.TalkId == 1; });
        }
    }
    public SmsTalkInfo CurTalkInfo
    {
        get
        {
            //   Debug.LogError(SceneId);
            Debug.LogError(selectIds[selectIds.Count - 1]);
            return SmsRuleInfo.smsTalkInfos.Find((m) => { return m.TalkId == selectIds[selectIds.Count - 1]; });
        }
    }

    //  public int BgId; //背景图ID
    //  public string MusicId; //音频ID
    //   public List<MsgOrCallVo> MsgList;
    //   public List<MsgOrCallRulePB> RulesList;


    public bool IsPlayerTrigger
    {
        get
        {
            return FirstTalkInfo.NpcId == 0;
            //return SmsRuleInfo.smsSceneInfo.NpcId == 0;
        }
    }

    public string Sender
    {
        get
        {
            if(NpcId==0)
            {
                return I18NManager.Get("Phone_Hint1", SceneId);               
            }
            Debug.LogError(NpcId);

            if(NpcId<5)
            {
                return GlobalData.NpcModel.GetNpcById(NpcId).NpcName;
            }
            else
            {
                return GlobalData.PhoneData.GetNpcInfoByNpcId(NpcId).NpcName;
            }
        }
    }

    public int CompareTo(MySmsOrCallVo other)
    {
        return CreateTime.CompareTo(other.CreateTime);
    }


}