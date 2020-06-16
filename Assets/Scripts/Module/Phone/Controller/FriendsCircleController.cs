using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Core.Events;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module.NetWork;
using Com.Proto;
using Common;
using DataModel;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendsCircleController : Controller
{
    public FriendsCircleView View;

    public override void Start()
    {
        base.Start();
        EventDispatcher.AddEventListener<int>(EventConst.PhoneFriendCirclePublishClick, OnClickFriendCirclePublish);
        EventDispatcher.AddEventListener<int,GameObject,List<int>>(EventConst.PhoneFriendCircleReplyClick, OnClickFriendCircleReply);
        EventDispatcher.AddEventListener<int, int>(EventConst.ClickFriendCircleItemReplyClick, OnClickFriendCircleItemReply);
        var data = GlobalData.PhoneData.UserFriendCircleList;
        View.SetData(data);
    }

    private void OnClickFriendCircleReply(int sceneId, GameObject obj,List<int> ids)
    {
        View.ShowReplay(sceneId, obj,ids);
    }
    private void OnClickFriendCircleItemReply(int sceneId, int sceneIdx)
    {
        Debug.Log("OnClickFriendCircleReply  sceneId   " + sceneId + "  sceneIdx  " + sceneIdx);
        var req = new CommentFriendCircleReq();
        req.SceneId = sceneId;
        req.ReplayId = sceneIdx;
        var dataBytes = NetWorkManager.GetByteData(req);
        NetWorkManager.Instance.Send<CommentFriendCircleRes>(CMD.PHONEC_FRIENDCIRCLE_COMMENT, dataBytes, OnCommentFriendCircleHandler);
    }

    private void OnCommentFriendCircleHandler(CommentFriendCircleRes res)
    {
        //Debug.Log("OnCommentFriendCircleHandler " + res.Ret);
        Debug.Log(res.FriendCircle.PubState + "    " + res.FriendCircle.SceneId + "    " + res.FriendCircle.CreateTime);
        GlobalData.PhoneData.UpdateFriendCircleData(res.FriendCircle);
        var data = GlobalData.PhoneData.UserFriendCircleList;
        View.UpdateData(data);
        //View.UpData(data.Find(m => m.SceneId == res.FriendCircle.SceneId));
    }

    private void OnClickFriendCirclePublish(int sceneId)
    {
        Debug.Log("OnClickFriendCirclePublish "+ sceneId);
        var req = new PubFriendCircleReq();
        req.SceneId = sceneId;
        var dataBytes = NetWorkManager.GetByteData(req);
        NetWorkManager.Instance.Send<PubFriendCircleRes>(CMD.PHONEC_FRIENDCIRCLE_PUBLISH, dataBytes, OnPublishFriendCircleHandler);
    }

    private void OnPublishFriendCircleHandler(PubFriendCircleRes res)
    {
        Debug.Log("OnPublishFriendCircleHandler " + res.Ret);
        Debug.Log(res.FriendCircle.PubState +"    "+ res.FriendCircle.SceneId+"    " + res.FriendCircle.CreateTime);
        GlobalData.PhoneData.UpdateFriendCircleData(res.FriendCircle);
        var data = GlobalData.PhoneData.UserFriendCircleList;
        View.AddData(data);
    }

    public void Hide()
    {
        View.Hide();
    }

    public void Show()
    {
        View.Show();
    }

    public override void Destroy()
    {
        base.Destroy();
        EventDispatcher.RemoveEventListener<int>(EventConst.PhoneFriendCirclePublishClick, OnClickFriendCirclePublish);
        EventDispatcher.RemoveEventListener<int,GameObject,List<int>>(EventConst.PhoneFriendCircleReplyClick, OnClickFriendCircleReply);
        EventDispatcher.RemoveEventListener<int, int>(EventConst.ClickFriendCircleItemReplyClick, OnClickFriendCircleItemReply);
    }
}
