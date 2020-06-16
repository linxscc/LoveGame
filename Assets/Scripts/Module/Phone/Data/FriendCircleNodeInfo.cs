using DataModel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendCircleTalkInfo
{
    public int TalkId;
    public int ReplyFromNpcId;//回复者ID
    public string ReplyFromNpcName;//回复者昵称
    public int ReplyToNpcId;//被回复者ID
    public string ReplyToNpcName;//被回复者昵称
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
    public string SelectBtnContent;//选择按钮文案
    public List<int> Selects;

    public FriendCircleTalkInfo()
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

public class FriendCircleSceneInfo
{
    public string SceneId;//情景ID
    public string SceneName;//情景名
    public string CardName;//卡牌名
    public string BackgroundId;//背景图ID
    public string NpcName;//NpcName
    public int CardNpcId;
    public string TitleContent//标题文案
    {
        set
        {
            _titleContent = value;
        }
        get {
            return Util.RegPlayerNameString(_titleContent, GlobalData.PlayerModel.PlayerVo.UserName);
        }
    }
    private string _titleContent;//标题文案
    public int NpcId;
    public List<int> startSelect;

    public FriendCircleSceneInfo()
    {
        SceneId = "";//情景ID
        SceneName = "";//情景名
        CardName = "";//卡牌名
        BackgroundId = "";//背景图ID
        NpcName = "";//NpcName
        TitleContent = "";
        NpcId = 0;
        CardNpcId = 0;//表示未配
        startSelect = new List<int>();
    }
}

public class FriendCircleInfo
{
    public FriendCircleSceneInfo friendCircleSceneInfo;
    public List<FriendCircleTalkInfo> friendCircleTalkInfos;
    public FriendCircleInfo()
    {
        friendCircleSceneInfo = new FriendCircleSceneInfo();
        friendCircleTalkInfos = new List<FriendCircleTalkInfo>();
    }
}