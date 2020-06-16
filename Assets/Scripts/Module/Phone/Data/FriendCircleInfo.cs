using Com.Proto;
using DataModel;
using game.main;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

public class FriendCircleVo : IComparable<FriendCircleVo>
{
    public int UserId;//
    public int SceneId;//情景ID
    public bool PublishState;//0未发送，1已发送，只对玩家主动发的朋友圈有意义
    public long CreateTime; //触发时间
    public long PublishTime; //发布时间，只对玩家主动发的朋友圈有意义
    public List<int> SelectIds;//已经评论列表

    public int curOperateSelectID;//当前操作Id  用来决定显示评论
    public long curOperateTime;//当前操作时间，用来判断是否显示他们评论

    //  public List<FriendCircleRulePB> friendCircleRules;
    private FriendCircleInfo _friendCircleRuleInfo = null;
    public FriendCircleInfo FriendCircleRuleInfo
    {
        get
        {
            if (_friendCircleRuleInfo == null)
            {
                string text = new AssetLoader().LoadTextSync(AssetLoader.GetPhoneDataPath(SceneId.ToString()));
                _friendCircleRuleInfo = JsonConvert.DeserializeObject<FriendCircleInfo>(text);
            }
            return _friendCircleRuleInfo;
        }
        set
        {
            _friendCircleRuleInfo = value;
        }
    }

    public int CompareTo(FriendCircleVo other)
    {
        long otherTime = other.FriendCircleRuleInfo.friendCircleSceneInfo.NpcId == 0 ? other.PublishTime : other.CreateTime;
        long curTime = FriendCircleRuleInfo.friendCircleSceneInfo.NpcId == 0 ? PublishTime : CreateTime;
        return -curTime.CompareTo(otherTime);
    }

    /// <summary>
    /// 通过replyId获取规则
    /// </summary>
    /// <param name="replyId"></param>
    /// <returns></returns>
    public FriendCircleTalkInfo GetCurSceneFCRuleByReplyId(int replyId)
    {
        FriendCircleTalkInfo res = null;
        res = FriendCircleRuleInfo.friendCircleTalkInfos.Find(match => match.TalkId == replyId);
        return res;
    }

    /// <summary>
    /// 获取相同的评论ID元素
    /// </summary>
    /// <param name="info"></param>
    /// <param name="pb"></param>
    /// <returns></returns>
    public static int GetCommonElement(FriendCircleVo info, FriendCircleRulePB pb)
    {
        int res = -1;
        foreach (var v in info.SelectIds)
        {
            if (pb.SelectIds.Contains(v))
            {
                return v;
            }
        }
        return res;
    }

    /// <summary>
    ///默认规则
    /// </summary>
    //public FriendCircleRulePB DefaultRule
    //{
    //    get
    //    {
    //        return GetCurSceneFCRuleByReplyId(0);
    //    }
    //}
    public string Sender
    {
        get
        {
            if (FriendCircleRuleInfo.friendCircleSceneInfo.NpcId == 0)
            {
                return GlobalData.PlayerModel.PlayerVo.UserName;
            }
            else if (FriendCircleRuleInfo.friendCircleSceneInfo.NpcId < 5)
            {
                return GlobalData.NpcModel.GetNpcById(FriendCircleRuleInfo.friendCircleSceneInfo.NpcId).NpcName;
            }
            else
            {
                return FriendCircleRuleInfo.friendCircleSceneInfo.NpcName;
            }
        }
    }

    public string StateTimeFormat
    {
        get
        {
            string timeFormat = "";
            long curTimeStamp = ClientTimer.Instance.GetCurrentTimeStamp();

            long publishStamp = FriendCircleRuleInfo.friendCircleSceneInfo.NpcId == 0 ? PublishTime : CreateTime;

            long hasGoneTime = curTimeStamp - publishStamp;
            if (hasGoneTime < -1000)
            {
                timeFormat = I18NManager.Get("Friend_Recently");
            }
            else if (hasGoneTime < 1000 * 60 * 60)          //"刚刚";//1个小时内
            {
                timeFormat = I18NManager.Get("Friend_Recently");
            }
            else if (hasGoneTime < 1000 * 60 * 60 * 24)      //"一个小时前";//1个小时前         
            {
                timeFormat = I18NManager.Get("Friend_Hint2");
            }
            else//"一天前";//一天前
            {
                timeFormat = I18NManager.Get("Friend_Hint3");
            }
            timeFormat = "";//这次测试发布全部隐藏
            return timeFormat;
        }
    }

    public string HeadPath
    {
        get
        {
            string path = "";
            switch (FriendCircleRuleInfo.friendCircleSceneInfo.NpcId)
            {
                case 5:
                    if (FriendCircleRuleInfo.friendCircleSceneInfo.NpcName == I18NManager.Get("Common_Role55"))
                    {
                        path = "Head/OtherHead/lxytx";
                    }
                    else if (FriendCircleRuleInfo.friendCircleSceneInfo.NpcName == I18NManager.Get("Common_Role54"))
                    {
                        path = "Head/OtherHead/cyjjrtx";
                    }
                    else if (FriendCircleRuleInfo.friendCircleSceneInfo.NpcName == I18NManager.Get("Common_Role52"))
                    {
                        path = "Head/OtherHead/qyzjjr";
                    }
                    else if (FriendCircleRuleInfo.friendCircleSceneInfo.NpcName == I18NManager.Get("Common_Role53"))
                    {
                        path = "Head/OtherHead/yjjjr";
                    }
                    else
                    {
                        path = "Head/PlayerHead/PlayerHead";
                    }
                    break;
                default:
                    path = PhoneData.GetHeadPath(FriendCircleRuleInfo.friendCircleSceneInfo.NpcId);
                    // path = "Head/PlayerHead/PlayerHead";
                    break;
            }
            return path;
        }
    }


}
