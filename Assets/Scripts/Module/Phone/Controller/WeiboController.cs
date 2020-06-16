using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module.NetWork;
using Com.Proto;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
using Assets.Scripts.Framework.GalaSports.Core.Events;
using System;
using DataModel;

public class WeiboController : Controller
{
    public WeiboView View;

    public override void Start()
    {
        base.Start();
        EventDispatcher.AddEventListener<int>(EventConst.PhoneWeiboItemLikeClick, OnClickWeiboLickItem);
        EventDispatcher.AddEventListener<int>(EventConst.PhoneWeiboItemPublishClick, OnPublishWeibo);
        var data = GlobalData.PhoneData.UserWeiboList;
        View.SetData(data);
    }

    private void OnPublishWeibo(int sceneID)
    {
        var req = new PubMicroBlogReq();
        req.SceneId = sceneID;
        Debug.Log("OnPublishWeibo" + sceneID);
        var dataBytes = NetWorkManager.GetByteData(req);
        NetWorkManager.Instance.Send<PubMicroBlogRes>(CMD.PHONEC_WEIBO_PUBLISH, dataBytes, OnPublishWeiboHandler);
    }

    private void OnPublishWeiboHandler(PubMicroBlogRes obj)
    {
        GlobalData.PhoneData.UpdateWeiboData(obj.MicroBlog);
        var data = GlobalData.PhoneData.UserWeiboList;
        View.SetData(data);
    }

    private void OnClickWeiboLickItem(int sceneID)
    {
        var req = new LikeMicroBlogReq();
        req.SceneId = sceneID;
        Debug.Log("OnClickWeiboLickItem" + sceneID);
        var dataBytes = NetWorkManager.GetByteData(req);
        NetWorkManager.Instance.Send<LikeMicroBlogRes>(CMD.PHONEC_WEIBO_LIKE, dataBytes, OnLikeWeiboHandler);
    }

    private void OnLikeWeiboHandler(LikeMicroBlogRes obj)
    {
        Debug.Log(" OnLikeWeiboHandler  " + obj.Ret+ "   obj.MicroBlog.Like   "+ obj.MicroBlog.Like);
        GlobalData.PhoneData.UpdateWeiboData(obj.MicroBlog);
        //var data = GlobalData.PhoneData.UserWeiboList;
        //View.SetData(data);
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
        EventDispatcher.RemoveEvent(EventConst.PhoneWeiboItemLikeClick );
        EventDispatcher.RemoveEvent(EventConst.PhoneWeiboItemPublishClick);
        base.Destroy();
    }

  
}
