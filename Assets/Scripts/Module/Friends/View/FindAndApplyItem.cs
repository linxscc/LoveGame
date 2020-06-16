using Assets.Scripts.Framework.GalaSports.Core.Events;
using Assets.Scripts.Module.Framework.Utils;
using Com.Proto;
using Common;
using Google.Protobuf.WellKnownTypes;
using UnityEngine;
using UnityEngine.UI;

public class FindAndApplyItem : MonoBehaviour
{
    private Text _level;
    private Text _playersupportername;
    private Text _playername;
    private RawImage _headIcon;

    private Text _collectedNum;

    //private Text _ranking;
    private Text _lastloginTime;
    private Button _ignoreBtn;
    private Button _agreeBtn;
    private Button _applybtn;
    private Transform _recommenttran;
    private Transform _applytran;

    private int friendId = 0;

    private void Awake()
    {
        _level = transform.Find("FriendInfoView/LevelTxt").GetText();
        _playersupportername = transform.Find("FriendInfoView/SupporterNameTxt").GetText();
        _playername = transform.Find("FriendInfoView/NameTxt").GetText();
        _headIcon = transform.Find("FriendInfoView/HeadIconMask/HeadIcon").GetRawImage();
        _collectedNum = transform.Find("CollectedCardTxt").GetText();
        //_ranking = transform.Find("UserInfo/Ranking/RankingTxt").GetText();
        _ignoreBtn = transform.Find("AgreePage/ingore").GetButton();
        _agreeBtn = transform.Find("AgreePage/Agree").GetButton();
        _applybtn = transform.Find("RecommentPage/ApplyBtn").GetButton();
        _lastloginTime = transform.Find("RecommentPage/LastLandingTxt").GetText();
        _recommenttran = transform.Find("RecommentPage");
        _applytran = transform.Find("AgreePage");

        _ignoreBtn.onClick.AddListener(() => { EventDispatcher.TriggerEvent(EventConst.FriendIgnore, friendId); });

        _agreeBtn.onClick.AddListener(() => { EventDispatcher.TriggerEvent(EventConst.FriendAgree, friendId); });

        _applybtn.onClick.AddListener(() => { EventDispatcher.TriggerEvent(EventConst.FriendDoApply, friendId); });
    }


    public void SetData(FriendBaseInfo info, int state)
    {
        _recommenttran.gameObject.SetActive(state == 0);
        _applytran.gameObject.SetActive(state == 1);

        friendId = info.UserId;
        _level.text = info.DepartmentLevel.ToString();
        _playersupportername.text = info.UserName;
        _playername.text = info.UserName;
        //_headIcon.texture = null; //info.UserHead
        _collectedNum.text = I18NManager.Get("Friend_CardCollecter2", info.CardNum);//"星缘收集:"+info.CardNum;
        //Debug.LogError(info.LastLoginTime);
//        Debug.LogError(DateUtil.GetDataTime(info.LastLoginTime));
        _lastloginTime.text = DateUtil.GetTimeFormatMinute(ClientTimer.Instance.GetCurrentTimeStamp()-info.LastLoginTime)+I18NManager.Get("Friend_LoginBefore");
    }
}