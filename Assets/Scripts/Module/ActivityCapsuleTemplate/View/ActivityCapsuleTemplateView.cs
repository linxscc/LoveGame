using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module;
using Assets.Scripts.Module.Framework.Utils;
using Assets.Scripts.Framework.GalaSports.Core.Events;
using Assets.Scripts.Module.Guide;
using Com.Proto;
using DataModel;
using DG.Tweening;
using game.main;
using game.tools;
using GalaAccount.Scripts.Framework.Utils;
using GalaAccountSystem;
using QFramework;
using UnityEngine;
using UnityEngine.UI;
using Common;

public class ActivityCapsuleTemplateView : View
{
    private GameObject _uiEffectObj;
    private Button _btnStory;
    private GameObject _storyRedPoint;
    private Button _btnLottery;
    private GameObject _lotteryRedPoint;

    private Text _timeText;

    private Button _costBtn;
    private RawImage _costIcon;
    private Text _costNumText;

    private long _endTimeStamp;
    private TimerHandler _countDown;


    private Transform _copyListTra;

    private Button[] _copyBtns;


    private void Awake()
    {
        _uiEffectObj = transform.Find("BG/Effect").gameObject;
        Transform bottom = transform.Find("Bottom");
        _btnStory = transform.Find("Top/StoryBtn").GetComponent<Button>();
        _btnStory.onClick.AddListener(OnBtnStoryClick);
        _btnLottery = bottom.transform.Find("LotteryBtn").GetComponent<Button>();
        _btnLottery.onClick.AddListener(OnBtnLotteryClick);

        _storyRedPoint = _btnStory.transform.Find("RedPoint").gameObject;
        _lotteryRedPoint = _btnLottery.transform.Find("RedPoint").gameObject;

        _timeText = transform.Find("Top/ActivityTimeBg/Text").GetComponent<Text>();
        _costBtn = bottom.transform.Find("HaveNum").GetComponent<Button>();
        _costIcon = bottom.transform.Find("HaveNum/Icon").GetComponent<RawImage>();
        _costNumText = bottom.transform.Find("HaveNum/Num").GetComponent<Text>();


        _copyListTra = transform.Find("Bottom/CopyList");
        
//        _copyListTra.GetButton("1").onClick.AddListener(() =>
//        {
//            SendMessage(new Message(MessageConst.CMD_CAPSULEBATTLE_CHOOSE_LEVEL));
//        });

        _costBtn.onClick.AddListener(OnBtnCostClick);

//        _copyBtns = new Button[4];
//        for (int i = 0; i < _copyBtns.Length; ++i) {
//            int index = i;
//            _copyBtns[i] = bottom.transform.Find("CopyList/" + (i+1)).GetComponent<Button>();
//            _copyBtns[i].onClick.AddListener(()=>
//            {
//                OnBtnCopyClick(index);
//            });
//        }

    }

    // Use this for initialization
    void Start () {
        EventDispatcher.AddEventListener(EventConst.ActivityCapsuleTemplateEnterStory, OnEnterStory);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetData(ActivityCapsuleTemplateModel model)
    {
        //Debug.LogWarning("view  setData:" + model.GainAllCapsuleItem());
        if (model.costItem != null)
        {
            _costIcon.texture = PropUtils.GetPropIcon(model.costItem.ResourceId);
            _costIcon.color = Color.white;
            int num = PropUtils.GetUserPropNum(model.costItem.ResourceId);
            _costNumText.text = num.ToString();

            bool capsuleState = false;
            if (num >= model.costItem.Num && !model.GainAllCapsuleItem())
            {
                capsuleState = true;
            }
            _lotteryRedPoint.SetActive(capsuleState);
        }
        _endTimeStamp = model.EndTimeStamp;
        SetActivityTime();

        _storyRedPoint.SetActive(model.HaveCanReadStory());

        _uiEffectObj.SetActive(true);
    }

    public void RefreshNum(ActivityCapsuleTemplateModel model)
    {
        if (model.costItem != null)
        {
            _costIcon.texture = PropUtils.GetPropIcon(model.costItem.ResourceId);
            _costIcon.color = Color.white;
            int num = PropUtils.GetUserPropNum(model.costItem.ResourceId);
            //Debug.LogError("num====?"+num);
            _costNumText.text = num.ToString();

            bool capsuleState = false;
            if(num >= model.costItem.Num && !model.GainAllCapsuleItem())
            {
                capsuleState = true;
            }
            _lotteryRedPoint.SetActive(capsuleState);
        }
        _uiEffectObj.SetActive(true);
    }
    
    
    /// <summary>
	/// 设置活动时间
	/// </summary>
	private void SetActivityTime()
    {
        var curTimeStamp = ClientTimer.Instance.GetCurrentTimeStamp();
        var surplusDay = DateUtil.GetSurplusDay(curTimeStamp, _endTimeStamp);
        if (surplusDay != 0)
        {
            _timeText.text = I18NManager.Get("ActivityTemplate_ActivityTemplateTime1", surplusDay);
        }
        else
        {
            _countDown = ClientTimer.Instance.AddCountDown("CountDown", Int64.MaxValue, 1f, CountDown, null);
            CountDown(0);
        }

    }


    public void CreateLevelEntranceItem(List<ActivityLevelRulePB> rulePbs)
    {

        for (int i = _copyListTra.childCount - 1; i >= 0; i--)
        {                    
            DestroyImmediate(_copyListTra.GetChild(i).gameObject);
        }
        
        var prefab = GetPrefab("ActivityCapsuleTemplate/Prefabs/LevelEntranceItem");
        
        for (int i = 0; i < rulePbs.Count; i++)
        {
            var levelVoInfo = GlobalData.CapsuleLevelModel.GetLevelInfo(rulePbs[i].LevelId);
            var item = Instantiate(prefab, _copyListTra, false);
            item.GetComponent<LevelEntranceItem>().SetData(levelVoInfo,(i+1));          
        }           
    }
    
    /// <summary>
	/// 不足一天进入倒计时
	/// </summary>
	private void CountDown(int obj)
    {
        string timeStr = "";
        var curTimeStamp = ClientTimer.Instance.GetCurrentTimeStamp();
        long time = _endTimeStamp - curTimeStamp;
        //if (_countDown != null)
        //{
        //    ClientTimer.Instance.RemoveCountDown(_countDown);
        //}
        if (time < 1000)
        {
            SendMessage(new Message(MessageConst.MODULE_ACTIVITY_CAPSULE_TEMPLATE_ACTIVITY_OVER));
            return;
        }
        else
        {
            long s = (time / 1000) % 60;
            long m = (time / (60 * 1000)) % 60;
            long h = time / (60 * 60 * 1000);
            timeStr = $"{h:D2}:{m:D2}:{s:D2}";
        }
        _timeText.text = I18NManager.Get("ActivityTemplate_ActivityTemplateTime2", timeStr);
    }

    private void OnEnterStory()
    {
        _uiEffectObj.gameObject.SetActive(false);
    }

    private void OnBtnCostClick()
    {
        FlowText.ShowMessage(I18NManager.Get("ActivityCapsuleTemplate_drawCostTips"));
    }

    private void OnBtnStoryClick()
    {
        SendMessage(new Message(MessageConst.CMD_ACTIVITY_CAPSULE_TEMPLATE_OPEN_STORY_WINDOW));
    }

    private void OnBtnLotteryClick()
    {
        _uiEffectObj.SetActive(false);
        SendMessage(new Message(MessageConst.MODULE_ACTIVITY_CAPSULE_TEMPLATE_OPEN_DRAW_PANEL));
    }

    private void OnBtnCopyClick(int i)
    {
        SendMessage(new Message(MessageConst.MODULE_ACTIVITY_CAPSULE_TEMPLATE_COPY_BTN, Message.MessageReciverType.DEFAULT, i));
    }

    public void OnShow()
    {
        
    }


    public void DestroyCountDown()
    {
        EventDispatcher.RemoveEvent(EventConst.ActivityCapsuleTemplateEnterStory);
        if (_countDown != null)
        {
            ClientTimer.Instance.RemoveCountDown(_countDown);
        }
    }
}
