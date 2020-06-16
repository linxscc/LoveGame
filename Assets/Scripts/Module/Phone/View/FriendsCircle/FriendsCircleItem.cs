using Assets.Scripts.Framework.GalaSports.Core.Events;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module;
using Com.Proto;
using Common;
using DataModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class FriendsCircleItem : MonoBehaviour
{

    VerticalLayoutGroup _content;
    Text _nameFC;//朋友圈名
    Text _commentTxt;//朋友圈内容
    RawImage _commentBg;//朋友圈图片
    RawImage _headImage;//头像图片
    Text _lastTimeTxt;//上一次登陆事件
    Button _replyBtn;//回复按钮
    Text _otherComment;//评论内容
    Image _otherCommentBg;//评论背景
    bool isSetHeight;

    FriendCircleVo _info;

    private int _travelindex = 0;
    private List<int> _replyIds;
    
    // Use this for initialization
    void Awake()
    {
        isSetHeight = true;
        _headImage = transform.Find("BlogNameTxt/HeadMask/HeadIcon").GetComponent<RawImage>();
        //_content = transform.Find("Content").GetComponent<VerticalLayoutGroup>();
        _nameFC = transform.Find("BlogNameTxt").GetComponent<Text>();
        _commentTxt = transform.Find("Text").GetComponent<Text>();
        _commentBg = transform.Find("BgImg/ImgMask/Image").GetComponent<RawImage>();
        _lastTimeTxt = transform.Find("Bottom/LastTimeTxt").GetComponent<Text>();
        _replyBtn = transform.Find("Bottom/ReplyBtn").GetComponent<Button>();
        _otherComment = transform.Find("CommentImage/ContentTxt").GetComponent<Text>();
        _otherCommentBg = transform.Find("CommentImage").GetComponent<Image>();
        _replyBtn.onClick.AddListener(() =>
        {
            EventDispatcher.TriggerEvent<int, GameObject, List<int>>(EventConst.PhoneFriendCircleReplyClick,
               int.Parse(_info.FriendCircleRuleInfo.friendCircleSceneInfo.SceneId),
               this.gameObject, _replyIds);
        });
        _replyBtn.gameObject.SetActive(false);
    }

    public void SetData(FriendCircleVo info)
    {
        Debug.Log("setData FriendsCircleItem1    " + info.SceneId);
        CancelInvoke("DelayCheckNpcReply");
        Debug.Log("setData FriendsCircleItem2    "+ info.SceneId);
        isSetHeight = false;
        _info = info;
        //Debug.Log("SetData sceneID " + info.SceneId+ " "+info.friendCircleRuleInfo.friendCircleSceneInfo.TitleContent);

        _commentTxt.text = Util.GetNoBreakingString(info.FriendCircleRuleInfo.friendCircleSceneInfo.TitleContent);

        _headImage.texture = ResourceManager.Load<Texture>(info.HeadPath, ModuleConfig.MODULE_PHONE);
        _nameFC.text = info.Sender;

        _lastTimeTxt.text = info.StateTimeFormat;
        _lastTimeTxt.gameObject.SetActive(false);
        string bgId = info.FriendCircleRuleInfo.friendCircleSceneInfo.BackgroundId;
        Debug.Log("bgid    " + bgId);
        if (bgId == "0"|| bgId == "")
        {
            transform.Find("BgImg").gameObject.SetActive(false);
        }
        else
        {
            //  Debug.Log("info.DefaultRule.PicId=1");
            _commentBg.texture = ResourceManager.Load<Texture>("Phone/FCBackGround/" + bgId, ModuleConfig.MODULE_PHONE);
         //   _commentBg.SetNativeSize();
            transform.Find("BgImg").gameObject.SetActive(true);
        }

       var str = SetComment(info);      
        CheckNpcReply(info.curOperateTime);
       // Invoke("AddText",0.1f);
    }


    public List<int> TravelSelects(List<int> hasSelects)
    {
        List<int> selectIds = new List<int>();

        if (hasSelects.Count == 0)
        {
            if((ClientTimer.Instance.GetCurrentTimeStamp() - _info.PublishTime) <10000)
            {
                return selectIds;
            }
            //  FriendCircleTalkInfo curStartTalk = _info.FriendCircleRuleInfo.friendCircleTalkInfos.Find((m) => { return m.TalkId == 1; }); 

            if(_info.FriendCircleRuleInfo.friendCircleSceneInfo.startSelect.Count==0)
            {
                return selectIds;
            }

            int startid = _info.FriendCircleRuleInfo.friendCircleSceneInfo.startSelect[0];
            FriendCircleTalkInfo curStartTalk = _info.FriendCircleRuleInfo.friendCircleTalkInfos.Find((m) => { return m.TalkId == startid; }); 
            
            while (curStartTalk.ReplyFromNpcId!=0)
            {
                selectIds.Add(curStartTalk.TalkId);
                if (curStartTalk.Selects.Count > 0) 
                {
                    curStartTalk = _info.FriendCircleRuleInfo.friendCircleTalkInfos.Find((m) => { return m.TalkId == curStartTalk.Selects[0]; });
                    continue;
                }
                break;          
            }
            return selectIds;
        }
        var startIndex = 1;

        foreach (var v in _info.FriendCircleRuleInfo.friendCircleSceneInfo.startSelect)
        {
            if(hasSelects.Contains(v))
            {
                startIndex = v;
                break;
            }
        }

        var endTalkId = hasSelects[hasSelects.Count - 1];

        FriendCircleTalkInfo curtalk= _info.FriendCircleRuleInfo.friendCircleTalkInfos.Find((m) => { return m.TalkId == startIndex; }); ;
        while (true)
        {
            if(curtalk.TalkId<= endTalkId)
            {
                selectIds.Add(curtalk.TalkId);
                if(curtalk.Selects.Count>0)
                {
                    var nexttalk = _info.FriendCircleRuleInfo.friendCircleTalkInfos.Find((m) => { return m.TalkId == curtalk.Selects[0]; });
                    if (nexttalk.ReplyFromNpcId==0)
                    {
                        foreach (var v in curtalk.Selects)
                        {
                            if (hasSelects.Contains(v))
                            {
                                nexttalk = _info.FriendCircleRuleInfo.friendCircleTalkInfos.Find((m) => { return m.TalkId == v; });
                            }
                        }
                    }
                    curtalk = nexttalk;
                    continue;
                }          
            }
            break;
        }

        //while (curtalk.TalkId < endTalkId)
        //{
        //    int nextTalkId = 0;
        //    if (curtalk.Selects.Count > 1)
        //    {
        //        foreach(var v in curtalk.Selects)
        //        {
        //            if(hasSelects.Contains(v))
        //            {
        //                nextTalkId = v;
        //                break;
        //            }
        //        }
        //    }
        //    else if(curtalk.Selects.Count>0)
        //    {
        //        nextTalkId = curtalk.Selects[0];
        //    }
        //    else
        //    {
        //        break;
        //    }
        //    curtalk = _info.friendCircleRuleInfo.friendCircleTalkInfos.Find((m) => { return m.TalkId == nextTalkId; });
        //}
        //selectIds.Add(endTalkId);

        //检查是否显示后续
        if(endTalkId == _info.curOperateSelectID&& (ClientTimer.Instance.GetCurrentTimeStamp() - _info.curOperateTime) < 10000)
        {

        }
        else
        {
            var curTalk2 = _info.FriendCircleRuleInfo.friendCircleTalkInfos.Find((m) => { return m.TalkId == endTalkId; });
            
            while(true)
            {
                if (curTalk2.Selects.Count > 0) 
                {
                    var nextTalk2 = _info.FriendCircleRuleInfo.friendCircleTalkInfos.Find((m) => { return m.TalkId == curTalk2.Selects[0]; });
                    if (nextTalk2.ReplyFromNpcId == 0)
                    {
                        break;
                    }
                    else
                    {
                        selectIds.Add(curTalk2.Selects[0]);
                        curTalk2 = nextTalk2;
                        continue;
                    }
                }
                break;
            }
        }
        return selectIds;
    }

    private string TravelComment(List<int> Selects)
    {
        string str = "";
        foreach(var  v in Selects)
        {
            var info= _info.FriendCircleRuleInfo.friendCircleTalkInfos.Find((m) => { return m.TalkId == v; });
            var strTemp = "";
            if (info.ReplyFromNpcId < 5)
            {
                strTemp = "<color=#d0a4d9>"+PhoneData.GetNpcNameById(info.ReplyFromNpcId)+ "</color>";
            }
            else
            {
                strTemp = "<color=#d0a4d9>" + info.ReplyFromNpcName + "</color>";
            }

            if(info.ReplyToNpcId>=5)
            {
                strTemp += I18NManager.Get("Friend_Hint4");
                strTemp += "<color=#d0a4d9>"+info.ReplyToNpcName + "</color>";

            }
            else if(info.ReplyToNpcId >= 0)
            {
                strTemp += I18NManager.Get("Friend_Hint4");
                strTemp += "<color=#d0a4d9>" + PhoneData.GetNpcNameById(info.ReplyToNpcId) + "</color>";
            }

            str += (strTemp +":"+ info.Content + "\n");
        }
        return str;
    }

   
    public string SetComment(FriendCircleVo info)
    {    
        string str = "";
        List<int> hasSelectIds = TravelSelects(info.SelectIds);
        str = TravelComment(hasSelectIds);

        _travelindex = 0;
        int selectIndex = 1;
        List<int> comments = new List<int>();
        List<int> selectIds = new List<int>();
        selectIds.AddRange(info.SelectIds);
        bool showReplyBtn = false;

        List<FriendCircleRulePB> lastComments = new List<FriendCircleRulePB>();
        bool hasComment = false;
        _replyIds=new List<int>();

        bool isShowBtn = IsShowReplyBtn(hasSelectIds);
        _replyBtn.gameObject.SetActive(isShowBtn);
        //_replyBtn.gameObject.SetActive(showReplyBtn);
        _replyBtn.transform.parent.gameObject.SetActive(isShowBtn);//隐藏时间
        str = ParsingCommentStr(str);      
        _otherComment.text = str;     
        _otherCommentBg.gameObject.SetActive( str != "");
           
        CheckNpcReply(info.curOperateTime);
        Invoke("AddText",0.1f);
 
        return str;
    }

    private bool IsShowReplyBtn(List<int> hasSelects)
    {
        FriendCircleTalkInfo nextInfo = null;
        if (hasSelects.Count==0)
        {
            if ((ClientTimer.Instance.GetCurrentTimeStamp() - _info.PublishTime) < 10000)
            {
                return false;
            }
            nextInfo = _info.FriendCircleRuleInfo.friendCircleTalkInfos.Find((m) => { return m.TalkId == 1; });
            _replyIds.AddRange(_info.FriendCircleRuleInfo.friendCircleSceneInfo.startSelect);
            return true;
        }
        else
        {
            var info = _info.FriendCircleRuleInfo.friendCircleTalkInfos.Find((m) => { return m.TalkId == hasSelects[hasSelects.Count - 1]; });     
            if(info.Selects.Count==0)
            {
                return false;
            }
            nextInfo = _info.FriendCircleRuleInfo.friendCircleTalkInfos.Find((m) => { return m.TalkId == info.Selects[0]; });
            if (nextInfo.ReplyFromNpcId == 0)
            {
                _replyIds.AddRange(info.Selects);
                return true;
            }
        }
        return false;
    }

    private void CheckNpcReply(long operateTime)
    {
        var clientTime = ClientTimer.Instance.GetCurrentTimeStamp(); 
        var timeLeft = (10000 - (clientTime - operateTime))/1000f;
        if (timeLeft>0)
        {
            Debug.Log("DelayCheckNpcReply:"+timeLeft);
            Invoke("DelayCheckNpcReply",timeLeft+0.2f);
        }
    }
    
    private void DelayCheckNpcReply()
    {
        SetData(_info);
        Invoke("AddText",0.1f);
    }

    private void AddText()
    {
        if(_otherComment!=null&&_otherComment.transform.parent!=null)
            _otherComment.text += " ";
    }
      
    private string ParsingCommentStr(string str)
    {
        string res = "";

        str = str.Replace("#{$heroTang}#", "#{" + I18NManager.Get("Common_Role1") + "}#");
        str = str.Replace("#{$heroYan}#", "#{" + I18NManager.Get("Common_Role3") + "}#");
        str = str.Replace("#{$heroChi}#", "#{" + I18NManager.Get("Common_Role4") + "}#");
        str = str.Replace("#{$heroQin}#", "#{" + I18NManager.Get("Common_Role2") + "}#");
        str = str.Replace("#{$player}#", "#{" + GlobalData.PlayerModel.PlayerVo.UserName + "}#");

//        str = str.Replace("#{$heroTang:}#", "#{" + "唐弋辰:" + "}#");
//        str = str.Replace("#{$heroYan:}#", "#{" + "言季:" + "}#");
//        str = str.Replace("#{$heroChi:}#", "#{" + "迟郁:" + "}#");
//        str = str.Replace("#{$heroQin:}#", "#{" + "秦予哲:" + "}#");
        str = str.Replace("#{$heroTang:}#", "#{" + I18NManager.Get("Common_Hint2") + "}#");
        str = str.Replace("#{$heroYan:}#", "#{" + I18NManager.Get("Common_Hint4") + "}#");
        str = str.Replace("#{$heroChi:}#", "#{" + I18NManager.Get("Common_Hint5") + "}#");
        str = str.Replace("#{$heroQin:}#", "#{" + I18NManager.Get("Common_Hint3") + "}#");
        str = str.Replace("#{$player:}#", "#{" + GlobalData.PlayerModel.PlayerVo.UserName + ":}#");

        string pattern = @"\#\{.*?\}\#";//匹配模式

        Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
        MatchCollection matches = regex.Matches(str);
        //存放匹配结果
        foreach (Match match in matches)
        {
            string value = match.Value.Substring(2, match.Value.Length - 4);
            StringBuilder sb = new StringBuilder();
            sb.Append("<color=#d0a4d9>");
            sb.Append(value);
            sb.Append("</color>");
            str = str.Replace(match.Value, sb.ToString());
        }
        if (str.Length - 1 > 0)
        {
            str = str.Substring(0, str.Length - 1);
        }
        res = str;
        return res;
    }

    public void testSet(string str)
    {
        _otherComment.text = str;
    }


}
