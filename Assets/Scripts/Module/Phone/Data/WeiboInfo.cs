using System;
using System.Collections.Generic;
using DataModel;
public class WeiboVo : IComparable<WeiboVo>
{
    public int UserId;
    //public int NpcId;
    public int SceneId; //1000--9999 短信      10000-19999电话   20000-29999朋友圈   30000-39999微博
                        // public string SceneName;
    public bool IsLike;//是否点赞过
    public bool IsComment;//0未评论1已评论

    private bool _isPubState;
    public bool IsPublish//0未发布1已发布
    {
        get
        {
            return WeiboRuleInfo.weiboSceneInfo.NpcId == 0 ? _isPubState : true;
        }
        set { _isPubState = value; }
    }
    public int CommentIndex;//评论选择的索引(按配置来)
    public long CreateTime;//触发时间
                           // public string Blogger ;//博主名
                           //   public int PictureId;
                           //  public string Content;//文案
                           //  public int Reading ;//阅读数
                           //  public int Comments ;//评论数
                           // public int Likes;//点赞数
                           //public List<string> CommentContents;//评论内容

    public WeiboInfo WeiboRuleInfo;
    public int CompareTo(WeiboVo other)
    {

        return -CreateTime.CompareTo(other.CreateTime);
    }


    public static string GetHeadPath(int NpcId)
    {

        string path = "";
        switch (NpcId)
        {
            //case 0:
            //    path = "Head/PlayerHead/PlayerHead";
            //    break;
            //case 1:
            //    path = "Head/1005";
            //    break;
            //case 2:
            //    path = "Head/2008";
            //    break;
            //case 3:
            //    path = "Head/3108";
            //    break;
            //case 4:
            //    path = "Head/4103";
            //    break;
            default:
                path = PhoneData.GetHeadPath(NpcId);
                break;
        }
        return path;
    }



}
