using DataModel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmsTalkInfo
{
    public int TalkId;//对话ID
    public int NpcId;//说话的Npc
    public string MusicID;//音频ID
    public string MusicLen;//音频时长
    private string _talkContent;
    public string TalkContent//对话内容
    {
        set{
            _talkContent = value;
        }
        get
        {
            return Util.RegPlayerNameString(_talkContent, GlobalData.PlayerModel.PlayerVo.UserName); 
        }
    }
    public List<int> Selects;
    public List<string> SelectsContent;

    public SmsTalkInfo()
    {
        TalkId = 0;
        NpcId = 0;
        MusicID = "";
        MusicLen = "";
        TalkContent = "";
        Selects = new List<int>();
        SelectsContent = new List<string>();
    }
}

public class SmsSceneInfo
{
    public string SceneId;//情景ID
    public string SceneName;//情景名
    public string CardName;//卡牌明
    public string BackgroundId;//背景图ID
    public int NpcId;

    public SmsSceneInfo()
    {
         SceneId = "";
         BackgroundId = "";
         NpcId = 0;
         SceneName = "";
         CardName = "";
    }
}

public class SmsInfo
{
    public SmsSceneInfo smsSceneInfo;
    public List<SmsTalkInfo> smsTalkInfos;
    public SmsInfo()
    {
        smsSceneInfo = new SmsSceneInfo();
        smsTalkInfos = new List<SmsTalkInfo>();
    }
}
