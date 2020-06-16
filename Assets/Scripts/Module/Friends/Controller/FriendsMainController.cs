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


public class FriendsMainController : Controller
{

    public FriendsMainView FriendsMainView;
    private FriendModel _friendModel;
    
    public override void Start()
    {
        LoadingOverlay.Instance.Show();
        NetWorkManager.Instance.Send<UserFriendsRes>(CMD.FRIEND_FRIENDS,null,GetMainInfo);
        EventDispatcher.AddEventListener<int>(EventConst.FriendDelete,DeleteFriendReq);
        EventDispatcher.AddEventListener<int>(EventConst.FriendSendPower,SendPowerReq);
        EventDispatcher.AddEventListener<int>(EventConst.FriendGetPower,GetPowerReq);
        
    }

    private void GetMainInfo(UserFriendsRes res)
    {
        LoadingOverlay.Instance.Hide();
        _friendModel = GetData<FriendModel>();
        _friendModel.Init(res);
        GetApplyRes();
        FriendsMainView.SetData(_friendModel); 
    }

    public void RefreshInfo()
    {
        LoadingOverlay.Instance.Show();
        NetWorkManager.Instance.Send<UserFriendsRes>(CMD.FRIEND_FRIENDS,null, res =>
        {
            LoadingOverlay.Instance.Hide();
            _friendModel.RefreshFriendList(res.UserFriends);
            FriendsMainView.SetData(_friendModel);
            GetApplyRes();
        });
    }
    
    //拉到推荐信息的时候就要顺便拉到好友申请信息了
    private void GetApplyRes()
    {
        LoadingOverlay.Instance.Show();
        NetWorkManager.Instance.Send<ApplysRes>(CMD.FRIEND_APPLY,null,GetApplyRes);
    }

    private void GetApplyRes(ApplysRes res)
    {
        _friendModel.InitApplyList(res);
        //刷新申请列表红点！
        FriendsMainView.SetRedPoint(_friendModel); 
        LoadingOverlay.Instance.Hide();
    }
    
    
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
            case MessageConst.CMD_FRIENDS_GRTPOWER:
                //一键领取
                GetAllPowerReq();
                break;
            case MessageConst.CMD_FRIENDS_GIVEAllPOWER:
                SendAllPowerReq();
                break;
            
        }
    }

    
    /// <summary>
    /// 赠送所有体力
    /// </summary>
    private void SendAllPowerReq()
    {
        var userIds = _friendModel.SendAllPowerFriend();
        if (userIds.Count>0)
        {
            Debug.LogError(userIds.Count);
            LoadingOverlay.Instance.Show();
            var buffer = NetWorkManager.GetByteData(new SendAllPowerReq(){FriendIds = { userIds}});
            NetWorkManager.Instance.Send<SendAllPowerRes>(CMD.FRIEND_SENDALLPOWER,buffer,SendAllPowerCallback); 
        }
        else
        {
            FlowText.ShowMessage("暂无好友可赠送");
        }


    }

    private void SendAllPowerCallback(SendAllPowerRes res)
    {
        LoadingOverlay.Instance.Hide();
        //FlowText.ShowMessage(I18NManager.Get("Friend_ReceiveSuccess"));
        _friendModel.UpdateFriendInfo(res.UserFriends);
        GlobalData.PlayerModel.UpdateUserMoney(res.UserMoney);
        FriendsMainView.SetData(_friendModel);
        FlowText.ShowMessage("成功赠送体力");
        if (_friendModel.GetPowerSendFriend().Count==0)
        {
            GlobalData.DepartmentData.CanGetFriendsPower = false;
        }
        
        
    }

    private void GetAllPowerReq()
    {
        //直接从friendModel里面取！
        var userIds = _friendModel.GetPowerSendFriend();
 
        if (userIds.Count>0)
        {
            Debug.LogError(userIds.Count);
            LoadingOverlay.Instance.Show();
            var buffer = NetWorkManager.GetByteData(new GetPowerAllReq(){FriendIds = { userIds}});
            NetWorkManager.Instance.Send<GetPowerAllRes>(CMD.FRIEND_GETALLPOWER,buffer,GetAllPowerResCallback); 
        }
        else
        {
            FlowText.ShowMessage(I18NManager.Get("Friend_NoPowerToGet"));
        }
    }

    private void GetAllPowerResCallback(GetPowerAllRes res)
    {
        LoadingOverlay.Instance.Hide();
        FlowText.ShowMessage(I18NManager.Get("Friend_ReceiveSuccess"));
        _friendModel.UpdateFriendInfo(res.UserFriends);
        _friendModel.DailyPower +=res.UserFriends.Count;//领取了多少体力要加上去
        FriendsMainView.SetData(_friendModel);
        GlobalData.PlayerModel.AddPower(res.UserFriends.Count);
        if (_friendModel.GetPowerSendFriend().Count==0)
        {
            GlobalData.DepartmentData.CanGetFriendsPower = false;
        }
        
    }


    private void GetPowerReq(int friendId)
    {
        var buffer = NetWorkManager.GetByteData(new GetPowerReq(){FriendId = friendId});
        LoadingOverlay.Instance.Show();
        NetWorkManager.Instance.Send<GetPowerRes>(CMD.FRIEND_GETPOWER,buffer,GetPowerResCallback); 
    }

    private void GetPowerResCallback(GetPowerRes res)
    {
        LoadingOverlay.Instance.Hide();
        FlowText.ShowMessage(I18NManager.Get("Friend_ReceiveSuccess"));
        _friendModel.UpdateFriendInfo(res.UserFriend);
        _friendModel.DailyPower = res.DailyPower;
        FriendsMainView.SetData(_friendModel); 
        GlobalData.PlayerModel.AddPower(1);
        if (_friendModel.GetPowerSendFriend().Count==0)
        {
            GlobalData.DepartmentData.CanGetFriendsPower = false;
        }
    }

    private void SendPowerReq(int friendId)
    {
        var buffer = NetWorkManager.GetByteData(new SendPowerReq(){FriendId = friendId});
        LoadingOverlay.Instance.Show();
        NetWorkManager.Instance.Send<SendPowerRes>(CMD.FRIEND_SENDPOWER,buffer,SendPowerResCallback); 
    }    

    private void SendPowerResCallback(SendPowerRes res)
    {
        LoadingOverlay.Instance.Hide();

        _friendModel.UpdateFriendInfo(res.UserFriend);

        if (res.UserMoney==null)
        {
            Debug.LogError("null?");
        }
        else
        {
            if (res.SendPowerCount>=20)
            {
                FlowText.ShowMessage(I18NManager.Get("Friend_Hint5"));
            }
            else
            {
                FlowText.ShowMessage(I18NManager.Get("Friend_SendPowerSuccess"));
                GlobalData.PlayerModel.UpdateUserMoney(res.UserMoney);//更新金钱 
            }

        }
        FriendsMainView.SetData(_friendModel); 
    }

    private void DeleteFriendReq(int friendId)
    {
        PopupManager.ShowConfirmWindow(I18NManager.Get("Friend_SureDeleteFriend"), I18NManager.Get("Common_Hint"), 
            I18NManager.Get("Common_OK1"), I18NManager.Get("Common_Cancel1")).WindowActionCallback = evt =>
        {
            if (evt == WindowEvent.Ok)
            {
                LoadingOverlay.Instance.Show();
                var buffer = NetWorkManager.GetByteData(new DeleteReq(){FriendId = friendId});
                NetWorkManager.Instance.Send<DeleteRes>(CMD.FRIEND_DELFRIEND,buffer,DeleteFriendResCallback);    
            }
        };
        

    }

    private void DeleteFriendResCallback(DeleteRes res)
    {
//        FlowText.ShowMessage("已删除该好友");
        LoadingOverlay.Instance.Hide();
        _friendModel.DeleteFriend(res.UserFriend);   
        FriendsMainView.SetData(_friendModel);  
    }


    public override void Destroy()
    {
        base.Destroy();
        //注意要移除监听器！
        EventDispatcher.RemoveEventListener<int>(EventConst.FriendDelete,DeleteFriendReq);
        EventDispatcher.RemoveEventListener<int>(EventConst.FriendSendPower,SendPowerReq);
        EventDispatcher.RemoveEventListener<int>(EventConst.FriendGetPower,GetPowerReq);
        
    }
}
