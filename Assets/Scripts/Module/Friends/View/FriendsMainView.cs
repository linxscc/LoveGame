using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Framework.GalaSports.Core;
using UnityEngine.UI;
using System;
using DataModel;
using game.main;

public class FriendsMainView : View
{
    private Button _makeFriendsBtn;//结识好友
    private Button _quickGetPower;//一键领取
    private Button _quickSendPower;//一键赠送
    private LoopVerticalScrollRect _friendsList;
    private Text _friendsNum;
    private Text _level;
    private Text _playersupportername;
    private Text _playername;
    private RawImage _headIcon;
    private Text _collectedNum;
    private Text _getpowerNum;
    private Text _ranking;
    private GameObject _nofriendtips;
    private GameObject _makefriendredpoint;
    

    private List<FriendInfo> _friensData;
    
    
    private void Awake()
    {
        _friendsNum = transform.Find("FriendsNumBg/FriendsNumTxt").GetText();
        _level = transform.Find("UserInfo/FriendInfoView/LevelTxt").GetText();
        _playersupportername = transform.Find("UserInfo/FriendInfoView/SupporterNameTxt").GetText();
        _playername = transform.Find("UserInfo/FriendInfoView/NameTxt").GetText();
        _headIcon = transform.Find("UserInfo/FriendInfoView/HeadIconMask/HeadIcon").GetRawImage();
        _collectedNum = transform.Find("UserInfo/CollectedCardTxt").GetText();
        _makeFriendsBtn = transform.Find("MakeFriendsBtn").GetComponent<Button>();
        _ranking = transform.Find("UserInfo/Ranking/RankingTxt").GetText();
        _getpowerNum = transform.Find("UserInfo/GetPowerNum").GetText();
        _nofriendtips = transform.Find("NoFriendTips").gameObject;
        _makefriendredpoint=transform.Find("MakeFriendsBtn/Image").gameObject;
        _makeFriendsBtn.onClick.AddListener(() => 
        {  
            SendMessage(new Message(MessageConst.MODULE_FRIENDS_GOTO_MAKE_FRIENDS));
        });
        _quickGetPower = transform.Find("UserInfo/GetPowersBtn").GetComponent<Button>();
        _quickGetPower.onClick.AddListener(() =>
        {
            SendMessage(new Message(MessageConst.CMD_FRIENDS_GRTPOWER));
        });

        _quickSendPower = transform.Find("UserInfo/SendPowerBtn").GetButton();
        _quickSendPower.onClick.AddListener(() =>
        {
            
            SendMessage(new Message(MessageConst.CMD_FRIENDS_GIVEAllPOWER));
        });

        _friendsList = transform.Find("FriendsList").GetComponent<LoopVerticalScrollRect>();
        _friendsList.prefabName = "Friends/Prefabs/FriendItem";
        _friendsList.poolSize = 8;
        _friendsList.UpdateCallback = ListUpdataCallback;//这种监听居然初始化操作就可以了？！
    }

    public void SetData(FriendModel friendModel)
    {
        _friensData = friendModel.FriendMainInfoList;
        _friensData.Sort();
        _nofriendtips.SetActive(_friensData.Count==0);
        SetFriendsData(_friensData);
        SetBottomInfo();
        _getpowerNum.text = I18NManager.Get("Friend_PowerCollected", friendModel.DailyPower
            , GlobalData.ConfigModel.GetConfigByKey(GameConfigKey.FRIEND_DAILY_MAX_POWER_ID)); //"体力收集:" + friendModel.DailyPower + "/" + GlobalData.ConfigModel.GetConfigByKey(GameConfigKey.FRIEND_DAILY_MAX_POWER_ID);
    }

    public void SetRedPoint(FriendModel friendModel)
    {
//        Debug.LogError(friendModel.FriendApplyList.Count);
        _makefriendredpoint.gameObject.SetActive(friendModel.FriendApplyList.Count>0);
    }
    
    /// <summary>
    /// 这个应该是个Action，点击了Item后就显示出来！
    /// </summary>
    private void SetBottomInfo()
    {
        _friendsNum.text = I18NManager.Get("Friend_FriendsNum", _friensData.Count,
            GlobalData.ConfigModel.GetConfigByKey(GameConfigKey.FRIEND_MAX_NUM_ID));
        //"好友数量:"+_friensData.Count+"/"+ GlobalData.ConfigModel.GetConfigByKey(GameConfigKey.FRIEND_MAX_NUM_ID);
        _level.text = GlobalData.PlayerModel.PlayerVo.Level.ToString();
        _playersupportername.text = GlobalData.PlayerModel.PlayerVo.UserName;
        _playername.text =  GlobalData.PlayerModel.PlayerVo.UserName;
        //_headIcon.texture = null;
        _collectedNum.text = I18NManager.Get("Friend_CardCollecter2", GlobalData.CardModel.UserCardList.Count);//"星缘收集:"+GlobalData.CardModel.UserCardList.Count;

        _ranking.text = "";
    }
    
    public void SetFriendsData(List<FriendInfo> data)
    {
        //_friendsList.RefillCells();
        _friensData = data;
        _friendsList.totalCount = data.Count;
        _friendsList.RefreshCells();
    }

    private void ListUpdataCallback(GameObject obj, int index)
    {
        obj.GetComponent<FriendItem>().SetData(_friensData[index]);
    }
}
