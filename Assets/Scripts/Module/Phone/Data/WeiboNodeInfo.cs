using DataModel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WeiboTalkInfo
{
    public int TalkId;
    public int ReplyFromNpcId;//回复者ID
    public string ReplyFromNpcName;//回复者昵称
    public int ReplyToNpcId;//被回复者ID
    public string ReplyToNpcName;//被回复者昵称
    //public string Content;
    public string SelectBtnContent;//选择按钮文案
    public List<int> Selects;

    private string _content;
    public string Content
    {
        set { _content = value; }
        get
        {
            // return Util.GetNoBreakingString(_content);
            return Util.RegPlayerNameString(_content, GlobalData.PlayerModel.PlayerVo.UserName);
        }
    }

    public WeiboTalkInfo()
    {
        TalkId = 0;
        ReplyFromNpcId = 0;//回复者ID
        ReplyFromNpcName = "";//回复者昵称
        ReplyToNpcId = 0;//被回复者ID
        ReplyToNpcName = "";//被回复者昵称
        Content = "";
        SelectBtnContent = "";//选择按钮文案
        Selects = new List<int>();
    }
}

public class WeiboSceneInfo
{
    public string SceneId;//情景ID
    public string SceneName;//情景名
    public string CardName;//卡牌明
    public string BackgroundId;//背景图ID
    public string NpcName;//NpcName
    public string HeadPicId;//头像ID
    private string _titleContent;//文案
    public string TitleContent//文案
    {
        set { _titleContent = value; }
        get
        {
            return Util.RegPlayerNameString(_titleContent, GlobalData.PlayerModel.PlayerVo.UserName);
        }
    }
    public int NpcId;
    public string ReadNum;//阅读数
    public string ConmentNum;//评论数
    public string LikeNum;//点赞数
    public List<int> startSelect;

    public WeiboSceneInfo()
    {
        SceneId = "";
        SceneName = "";
        CardName = "";
        BackgroundId = "";
        NpcName = "";//NpcName
        HeadPicId = "";
        TitleContent = "";
        NpcId = 0;
        ReadNum = "";
        ConmentNum = "";
        LikeNum = "";
        startSelect = new List<int>();
    }
}

public class WeiboInfo  {

    public WeiboSceneInfo weiboSceneInfo;
    public List<WeiboTalkInfo> weiboTalkInfos;
    public WeiboInfo()
    {
        weiboSceneInfo = new WeiboSceneInfo();
        weiboTalkInfos = new List<WeiboTalkInfo>();
    }

}
