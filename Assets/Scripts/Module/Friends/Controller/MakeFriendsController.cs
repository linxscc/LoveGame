using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Core.Events;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module.NetWork;
using Com.Proto;
using Common;
using Componets;
using DataModel;
using game.main;
using UnityEngine;

public class MakeFriendsController : Controller
{

	public MakeFriendsView MakeFriendsView;
	private FriendModel _friendModel;
    
	public override void Start()
	{
       //一开始只是先获取！     
		var buffer = NetWorkManager.GetByteData(new CommentsReq(){Refresh = 0});
		NetWorkManager.Instance.Send<CommentsRes>(CMD.FRIEND_COMMENTS,buffer,GetCommentInfo);
		EventDispatcher.AddEventListener<int>(EventConst.FriendDoApply,DoApplyReq);
		EventDispatcher.AddEventListener<int>(EventConst.FriendIgnore, IgnoreApplyReq);
		EventDispatcher.AddEventListener<int>(EventConst.FriendAgree,AgreeReq);
	}

	private void GetCommentInfo(CommentsRes res)
	{
		_friendModel = GetData<FriendModel>();
		_friendModel.InitCommentList(res);
		//GetApplyRes();
		MakeFriendsView.SetData(_friendModel);
	}

	//拉到推荐信息的时候就要顺便拉到好友申请信息了
//	private void GetApplyRes()
//	{
//		LoadingOverlay.Instance.Show();
//		NetWorkManager.Instance.Send<ApplysRes>(CMD.FRIEND_APPLY,null,GetApplyRes);
//	}
//
//	private void GetApplyRes(ApplysRes res)
//	{
//		_friendModel.InitApplyList(res);
//		MakeFriendsView.SetData(_friendModel);
//		LoadingOverlay.Instance.Hide();
//	}

	/// <summary>
	/// 处理View消息
	/// </summary>
	/// <param name="message"></param>
	public override void OnMessage(Message message)
	{
		string name = message.Name;
		object[] body = message.Params;
		switch (name)
		{
//            case MessageConst.CMD_FRIENDS_DOAPPLY:
//	            DoApplyReq((int)body[0]);
//	            break;
//			case MessageConst.CMD_FRIENDS_AGREE:
//				AgreeReq((int)body[0]);
//				break;
			case MessageConst.CMD_FRIENDS_SEARCH:
				SearchReq((int) message.Body);
				break;
			case MessageConst.CMD_FRIENDS_REFRESH:
				RefreshRecommentList();
				break;

		}
	}

	private void RefreshRecommentList()
	{
		LoadingOverlay.Instance.Show();
		var buffer = NetWorkManager.GetByteData(new CommentsReq(){Refresh = 1});
		NetWorkManager.Instance.Send<CommentsRes>(CMD.FRIEND_COMMENTS,buffer,RefreshCommentCallBack);  //这样是不正确的
	}

	private void RefreshCommentCallBack(CommentsRes res)
	{
		LoadingOverlay.Instance.Hide();
		FlowText.ShowMessage(I18NManager.Get("Shop_RefreshTips"));
		_friendModel.RefreshTime = res.RefreshTime;
		_friendModel.UpdateCommentList(res.FriendBases);
		MakeFriendsView.SetData(_friendModel);
	}
	
	//发起好友申请
	private void DoApplyReq(int friendId)
	{
		LoadingOverlay.Instance.Show();
		Debug.LogError(friendId);
		var buffer = NetWorkManager.GetByteData(new DoApplyReq(){FriendId = friendId});
		NetWorkManager.Instance.Send<DoApplyRes>(CMD.FRIEND_DOAPPLY,buffer,GetApplyResCallback); 
	}

	private void GetApplyResCallback(DoApplyRes res)
	{
        Debug.LogError(""+res.FriendId);
		LoadingOverlay.Instance.Hide();
		FlowText.ShowMessage(I18NManager.Get("Friend_ApplySuccess"));
		//删掉一个item，并且刷新申请列表
		_friendModel.UpdateCommentList(res.FriendId);
		MakeFriendsView.SetData(_friendModel);
		
	}

	private void IgnoreApplyReq(int friendId)
	{
		LoadingOverlay.Instance.Show();
		var buffer = NetWorkManager.GetByteData(new IgnoreReq(){FriendId = friendId});
		NetWorkManager.Instance.Send<IgnoreRes>(CMD.FRIEND_IGNORE,buffer,IgnoreResCallback); 
		
	}

	private void IgnoreResCallback(IgnoreRes res)
	{
		LoadingOverlay.Instance.Hide();
		_friendModel.UpdateApplyList(res.FriendId);
		MakeFriendsView.SetData(_friendModel);
	}

	
	private void AgreeReq(int friendId)
	{
		LoadingOverlay.Instance.Show();
		var buffer = NetWorkManager.GetByteData(new AgreeReq(){FriendId = friendId});
		NetWorkManager.Instance.Send<AgreeRes>(CMD.FRIEND_AGREE,buffer,AgreeResCallback); 
	}

	private void AgreeResCallback(AgreeRes res)
	{
		LoadingOverlay.Instance.Hide();
		FlowText.ShowMessage(I18NManager.Get("Friend_AddFriendSuccess"));
		//刷新好友主界面！
		_friendModel.AddFriend(res.UserFriend);
		_friendModel.UpdateApplyList(res.UserFriend.UserId);
		MakeFriendsView.SetData(_friendModel);
	}

	private void SearchReq(int friendId)
	{
		LoadingOverlay.Instance.Show();
		var buffer = NetWorkManager.GetByteData(new SearchReq(){FriendId = friendId});
		NetWorkManager.Instance.Send<SearchRes>(CMD.FRIEND_SEARCH,buffer,SearchResCallback); 
	}

	private void SearchResCallback(SearchRes res)
	{
		LoadingOverlay.Instance.Hide();
		_friendModel.SearchFriendInfo = res.FriendBase;
		Debug.LogError(res.FriendBase);
		if (res.FriendBase!=null)
		{
			MakeFriendsView.SetSearchFriendItem(_friendModel.SearchFriendInfo);
		}
		else
		{
			FlowText.ShowMessage(I18NManager.Get("Friend_NoThisFriend"));
		}

	}

	public override void Destroy()
	{
		base.Destroy();
		//一堆事件啊！
		EventDispatcher.RemoveEventListener<int>(EventConst.FriendDoApply,DoApplyReq);
		EventDispatcher.RemoveEventListener<int>(EventConst.FriendIgnore, IgnoreApplyReq);
		EventDispatcher.RemoveEventListener<int>(EventConst.FriendAgree,AgreeReq);
		
	}
}
