
using Assets.Scripts.Framework.GalaSports.Core.Events;
using Assets.Scripts.Module.Framework.Utils;
using Common;
using game.main;
using Google.Protobuf.WellKnownTypes;
using UnityEngine;
using UnityEngine.UI;

public class FriendItem : MonoBehaviour {

    private Text _level;
    private Text _playersupportername;
    private Text _playername;
    private RawImage _headIcon;
    private Text _collectedNum;
    private Text _ranking;
    private Text _lastloginTime;
    private Button _getpowerBtn;
    private Button _sendpowerBtn;
    private Text _sendstate;
    private Text _getstate;
    private Button _delbtn;
    private int userId;

    private void Awake()
    {
        _level = transform.Find("FriendInfoView/LevelTxt").GetText();
        _playersupportername = transform.Find("FriendInfoView/SupporterNameTxt").GetText();
        _playername = transform.Find("FriendInfoView/NameTxt").GetText();
        _headIcon = transform.Find("FriendInfoView/HeadIconMask/HeadIcon").GetRawImage();
        _collectedNum = transform.Find("CollectedCardTxt").GetText();
        _ranking = transform.Find("Ranking/RankingTxt").GetText();
        _getpowerBtn = transform.Find("GetPowerBtn").GetButton();
        _sendpowerBtn = transform.Find("GivePowerBtn").GetButton();
        _sendstate = transform.Find("GivePowerBtn/Text").GetText();
        _getstate = transform.Find("GetPowerBtn/Text").GetText();
        _delbtn = transform.Find("DelFriendrBtn").GetButton();
        _lastloginTime = transform.Find("LastLandingTxt").GetText();

        _getpowerBtn.onClick.AddListener(() =>
        {
            EventDispatcher.TriggerEvent<int>(EventConst.FriendGetPower,userId); 
        });
        
        _sendpowerBtn.onClick.AddListener(() =>
        {
//            Debug.LogError(userId);
            EventDispatcher.TriggerEvent<int>(EventConst.FriendSendPower,userId);
        });
        
        _delbtn.onClick.AddListener(() =>
        {
            EventDispatcher.TriggerEvent<int>(EventConst.FriendDelete,userId);
        });
        
    }


    public void SetData(FriendInfo info)
    {
        userId = info.UserId;
        _level.text = info.Level.ToString();
        _playersupportername.text = info.SupporterName;
        _playername.text = info.UserName;
        //_headIcon.texture = null; //info.UserHead
        _collectedNum.text = I18NManager.Get("Friend_CardCollecter2", info.CollectedCardPercent);//"星缘收集:"+info.CollectedCardPercent;
        if (ClientTimer.Instance.GetCurrentTimeStamp() - info.LastLandingTime >= 0)
        {
            _lastloginTime.text = DateUtil.GetTimeFormatMinute(ClientTimer.Instance.GetCurrentTimeStamp()-info.LastLandingTime);
        }
        else
        {
            _lastloginTime.text = I18NManager.Get("Friend_Recently");
        }
        

        _getpowerBtn.gameObject.SetActive(info.IsBeGivenPower==1); 
//        Debug.LogError(info.IsBeGivenPower);
        string spName = "";
        switch (info.IsGetPower)
        {
//            case 0:
//                _getpowerBtn.gameObject.SetActive(false); 
//                break;
            case 0:
                spName = "UIAtlas_Friends_Power1";
                _getstate.text = I18NManager.Get("Friend_GetPower");
                _getpowerBtn.enabled = true;
                break;
            case 1:
                spName = "UIAtlas_Friends_Power3";
                _getstate.text = I18NManager.Get("Common_AlreadyGet");
                _getpowerBtn.enabled = false;
                break;
            default:
                Debug.LogError("Serives Error");
                break;
            
        }
        
        _getpowerBtn.image.sprite=AssetManager.Instance.GetSpriteAtlas(spName);

        string spName1 = "";
        
//        Debug.LogError(info.IsGivePower);
        switch (info.IsGivePower)
        {
            case 0:
                spName1 = "UIAtlas_Friends_Power1";
                _sendstate.text = I18NManager.Get("Friend_SendPower");
                _sendpowerBtn.enabled = true;
                break;
            case 1:
                spName1 = "UIAtlas_Friends_Power3";
                _sendstate.text = I18NManager.Get("Friend_HasSend");
                _sendpowerBtn.enabled = false;
                break;
            
        }
        _sendpowerBtn.image.sprite=AssetManager.Instance.GetSpriteAtlas(spName1);
        


    }
}
