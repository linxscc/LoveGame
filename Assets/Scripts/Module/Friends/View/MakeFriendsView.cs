using System;
using UnityEngine;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Module.Framework.Utils;
using Com.Proto;
using DataModel;
using game.main;
using Google.Protobuf.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MakeFriendsView : View
{

    private Toggle _findfriendtoggle;
    private Toggle _applyfriendtoggle;
    private InputField _inputfriendId;
    private Button _findFriend;
    private Button _refreshBtn;
    private Text _gameId;
    private Transform _findFriendPage;
    private Transform _applyPage;
    private LoopVerticalScrollRect _findfriendList;
    private LoopVerticalScrollRect _applyList;
    private RepeatedField<FriendBaseInfo> _friendcommentlist;
    private RepeatedField<FriendBaseInfo> _friendApplyList;
    private FriendBaseInfo _searchItem;
    private GameObject _redpoint;

    private Text _countDown;

    private int _curstate=0;
    private long _refreshTime = 0;
    private TimerHandler _handler;
    private int refreshMaxTime = 0;

    private void Awake()
    {
        _findfriendtoggle = transform.Find("TopTab/FindToggle").GetComponent<Toggle>();
        _applyfriendtoggle=transform.Find("TopTab/ApplyToggle").GetComponent<Toggle>();
        _findfriendtoggle.onValueChanged.AddListener(ChangePanel);
        _applyfriendtoggle.onValueChanged.AddListener(ChangePanel);
        _redpoint = transform.Find("TopTab/ApplyToggle/Image").gameObject;
        _inputfriendId = transform.Find("FindPage/InputField").GetComponent<InputField>();
        _findFriend = transform.Find("FindPage/FindBtn").GetButton();
        _findFriend.onClick.AddListener(() =>
        {
            if (!String.IsNullOrWhiteSpace(_inputfriendId.text)&&Util.IsOnlyNumber(_inputfriendId.text))
            {
                int userID = int.Parse(_inputfriendId.text);

                if (userID==GlobalData.PlayerModel.PlayerVo.UserId)
                {
                    FlowText.ShowMessage(I18NManager.Get("Friend_CannotAddSelf"));
                    return;
                }
                
                SendMessage(new Message(MessageConst.CMD_FRIENDS_SEARCH,userID,Message.MessageReciverType.CONTROLLER));
            }
            else
            {
                FlowText.ShowMessage(I18NManager.Get("Friend_SearchFromId"));
            }

        });
        
        _refreshBtn = transform.Find("FindPage/RefreshBtn").GetButton();
        _countDown = transform.Find("FindPage/RefreshBtn/Text").GetText();
        _countDown.text = I18NManager.Get("Common_Refresh");
        refreshMaxTime = GlobalData.ConfigModel.GetConfigByKey(GameConfigKey.FRIEND_SEARCH_INTERVAL_ID);
        _handler = ClientTimer.Instance.AddCountDown("UpdateAutoChange", Int64.MaxValue, 1f, UpdateAutoChange, null);
        _refreshBtn.onClick.AddListener(() =>
        {
            //Debug.LogError(refreshMaxTime);
            if (ClientTimer.Instance.GetCurrentTimeStamp()-_refreshTime>=refreshMaxTime*1000)
            {
                //Debug.LogError(ClientTimer.Instance.GetCurrentTimeStamp()-_refreshTime);
                SendMessage(new Message(MessageConst.CMD_FRIENDS_REFRESH));  
            }
            else
            {
                //Debug.LogError(ClientTimer.Instance.GetCurrentTimeStamp()-_refreshTime);
              FlowText.ShowMessage(I18NManager.Get("Friend_CoolDownRefresh"));  
            }
        });
        
        _findFriendPage = transform.Find("FindPage");
        _applyPage = transform.Find("ApplyPage");    
        _gameId = transform.Find("FindPage/FriendsNumTxt").GetText();
        _findfriendList = transform.Find("FindPage/RecommendingList").GetComponent<LoopVerticalScrollRect>();
        _applyList = transform.Find("ApplyPage/ApplyList").GetComponent<LoopVerticalScrollRect>();
        _findfriendList.prefabName = "Friends/Prefabs/FindAndApplyItem";
        _applyList.prefabName = "Friends/Prefabs/FindAndApplyItem";
        _findfriendList.poolSize = _applyList.poolSize = 8;
//        _findfriendList.UpdateCallback = UpdateCommentList;
        _applyList.UpdateCallback = UpdateApplyList;

    }

    private void UpdateAutoChange(int count)
    {
//        Debug.LogError(DateUtil.GetDataTime(_refreshTime)+"  curr"+DateUtil.GetDataTime(ClientTimer.Instance.GetCurrentTimeStamp()));
        if (ClientTimer.Instance.GetCurrentTimeStamp()-_refreshTime>=refreshMaxTime*1000)
        {
            _countDown.text = I18NManager.Get("Common_Refresh");
        }
        else
        {
            _countDown.text = DateUtil.GetTimeFormat(refreshMaxTime*1000-(ClientTimer.Instance.GetCurrentTimeStamp()-_refreshTime));
        }
    }

    private void ChangePanel(bool isOn)
    {
        if (isOn == false)
            return;

        string name = EventSystem.current.currentSelectedGameObject.name;
        Debug.Log("OnTabChange===>" + name);

        switch (name)
        {
            case "FindToggle":
                SetCommentData(_friendcommentlist);
                ChangeViewState(0);
                break;
            case "ApplyToggle":
                SetApplyData(_friendApplyList);
                ChangeViewState(1);
                break;
        }


    }

    private void ChangeViewState(int state)
    {
        _curstate = state;
        _findFriendPage.gameObject.SetActive(state==0); 
        _applyPage.gameObject.SetActive(state==1);
    }

    private void UpdateCommentList(GameObject go, int index)
    {
       go.GetComponent<FindAndApplyItem>().SetData(_friendcommentlist[index],0); 
    }

    private void UpdateApplyList(GameObject go, int index)
    {
        go.GetComponent<FindAndApplyItem>().SetData(_friendApplyList[index],1); 
    }
    
    public void SetData(FriendModel friendModel)
    {
        _gameId.text = I18NManager.Get("Friend_MyGameId",GlobalData.PlayerModel.PlayerVo.UserId);//"我的游戏ID:"+GlobalData.PlayerModel.PlayerVo.UserId;
        _friendcommentlist = friendModel.FriendCommentList;
        _friendApplyList = friendModel.FriendApplyList;
        _refreshTime = friendModel.RefreshTime;
        _redpoint.gameObject.SetActive(friendModel.FriendApplyList.Count>0);
        //之后要判断状态！
        if (_curstate==0)
        {
            SetCommentData(_friendcommentlist);
            ChangeViewState(0); 
        }
        else
        {
            SetApplyData(_friendApplyList);
            ChangeViewState(1); 
        }
        

    }

    private void SetCommentData(RepeatedField<FriendBaseInfo> data)
    {
//        Debug.LogError(data.Count);
        //_findfriendList.RefillCells();
        _findfriendList.UpdateCallback = UpdateCommentList;
        _findfriendList.totalCount = data!=null ? data.Count : 0;
        _findfriendList.RefreshCells();
    }
    
    private void SetApplyData(RepeatedField<FriendBaseInfo> data)
    {
        //_applyList.RefillCells();
        //Debug.LogError(data.Count);
        _applyList.totalCount = data!=null ? data.Count : 0;
        _applyList.RefreshCells();
    }

    public void SetSearchFriendItem(FriendBaseInfo friendBaseInfo)
    {
        _searchItem = friendBaseInfo;
        //_findfriendList.RefillCells();
        _findfriendList.totalCount = 1;
        _findfriendList.UpdateCallback = ShowSearchItem;
        _findfriendList.RefreshCells();
    }

    private void ShowSearchItem(GameObject go, int index)
    {
        go.GetComponent<FindAndApplyItem>().SetData(_searchItem,0); 
    }

    private void OnDestroy()
    {
        ClientTimer.Instance.RemoveCountDown(_handler);
    }
}
