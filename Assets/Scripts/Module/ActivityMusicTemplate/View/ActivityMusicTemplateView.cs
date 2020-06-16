using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module.Framework.Utils;
using Com.Proto;
using DataModel;
using game.main;
using UnityEngine;
using UnityEngine.UI;

public class ActivityMusicTemplateView : View
{
    private Button _storyBtn;
    private Button _exchangeShopBtn;
    private Button _taskBtn;
    private Text _time;
    
    private Button _ruleBtn;
    
    private RawImage _bottomIcon;    
    private long _endTimeStamp;
    private TimerHandler _countDown;
    private string _title;
    private string _ruleDesc;
    private string _iconPath;
    private RawImage _exchangeIcon;
    private Text _exchangeNum;

    private int _exchangeItemId;


    private ActivityPlayRuleVo _ruleVo;
    
    private void Awake()
    {
        _exchangeIcon = transform.GetRawImage("ExchangeShopBtn/NumBg/Icon");
        _exchangeNum = transform.GetText("ExchangeShopBtn/NumBg/Text");
        _bottomIcon = transform.GetRawImage("Bottom/Title/Icon");
        _storyBtn = transform.GetButton("StoryBtn");
        _exchangeShopBtn = transform.GetButton("ExchangeShopBtn");
        _taskBtn = transform.GetButton("TaskBtn");
        
        _storyBtn.onClick.AddListener(OnClickStory);
        _exchangeShopBtn.onClick.AddListener(OnClickExchangeShop);
        _taskBtn.onClick.AddListener(OnClickTask);

        _ruleBtn = transform.GetButton("BG/MiddleBg/RuleBtn");
        _time = transform.GetText("EndTimeBg/Text");
        
        _ruleBtn .onClick.AddListener(OnClickRuleBtn);
        SetConfigInfo();
    }

    private void SetConfigInfo()
    {
        _title = I18NManager.Get("Common_HowToPlay");
        _ruleDesc = I18NManager.Get("ActivityMusicTemplate_PlayRule");
       
    }

    private void Start()
    {
         
    }


    /// <summary>
    /// 点击玩法介绍窗口
    /// </summary>
    private void OnClickRuleBtn()
    {            
        var window = PopupManager.ShowWindow<PopupWindow>("GameMain/Prefabs/PropWindow");
        window.SetPlayRuleData(_ruleVo);    
    }


    public void FirstShowRuleWindow(ActivityPlayRuleVo vo)
    {
        _ruleVo = vo;
        if (!PlayerPrefs.HasKey(vo.Key))
        {
            var window = PopupManager.ShowWindow<PopupWindow>("GameMain/Prefabs/PropWindow");
            window.SetPlayRuleData(vo); 
        }
    }
    
    /// <summary>
    /// 设置按钮入口是否显示
    /// </summary>
    /// <param name="isShowStory">是否显示剧情入口</param>
    /// <param name="isShowExchangeShop">是否显示兑换商店入口</param>
    ///  /// <param name="isShowTask">是否显示兑任务入口</param>
    public void SetEntranceShow(bool isShowStory,bool isShowExchangeShop,bool isShowTask)
    {
        _storyBtn.gameObject.SetActive(isShowStory);
        _exchangeShopBtn.gameObject.SetActive(isShowExchangeShop);
        _taskBtn.gameObject.SetActive(isShowTask);
    }
    
    
    //点击兑换商店Btn
    private void OnClickExchangeShop()
    {
        SendMessage(new Message(MessageConst.CMD_OPEN_ACTIVITYMUSIC_EXCHANGESHOP));
    }

    //点击剧情Btn
    private void OnClickStory()
    {
       SendMessage(new Message(MessageConst.CMD_OPEN_ACTIVITYMUSIC_STORY_WINDOW)); 
    }

    //点击任务Btn
    private void OnClickTask()
    {
        SendMessage(new Message(MessageConst.CMD_OPEN_ACTIVITYMUSIC_TASK_WINDOW));
    }
    
    
    public void SetData(ActivityVo curActivity,List<ActivityLevelRulePB> list)
    {
        _exchangeItemId = curActivity.ActivityExtra.ItemId;
        _endTimeStamp = curActivity.EndTime;
        Debug.LogError("时间-------》"+_endTimeStamp);
        _bottomIcon.texture = ResourceManager.Load<Texture>("Prop/"+_exchangeItemId); 
        _exchangeIcon.texture= ResourceManager.Load<Texture>("Prop/"+_exchangeItemId);
        _exchangeNum.text = GlobalData.PropModel.GetUserProp(_exchangeItemId).Num.ToString();
        
        CreateLevelItem(list);

        SetActivityTime();
    }

    public void SetMusicData(List<ActivityMusicVo> vos)
    {
        CreateMusicLevelItem(vos);
    }


    public void RefreshNum()
    {
        _exchangeNum.text = GlobalData.PropModel.GetUserProp(_exchangeItemId).Num.ToString();
    }

    public void SetRedDot(ActivityStoryModel storyModel,ActivityMissionModel missionModel)
    {
        if (storyModel!=null)
        {
            _storyBtn.transform.Find("RedPoint").gameObject.SetActive(storyModel.IsShowStoryRedDot());  
        }

        if (missionModel!=null)
        {
            _taskBtn.transform.Find("Red").gameObject.SetActive(missionModel.IsShowMissionRedDot());
        }
        
    }

    /// <summary>
    /// 生成关卡Item
    /// </summary>
    private void CreateLevelItem(List<ActivityLevelRulePB> list)
    {
        var content = transform.Find("Bottom/LevelContent");            
        var prefab = GetPrefab("ActivityMusicTemplate/Prefabs/LevelItem");      
        for (int i = 0; i < list.Count; i++)
        {
            var parent = content.GetChild(i).transform;
            var go = Instantiate(prefab, parent, false);
            var levelVoInfo = GlobalData.CapsuleLevelModel.GetLevelInfo(list[i].LevelId);         
            go.name = levelVoInfo.LevelId.ToString();          
            go.GetComponent<LevelItem>().SetData(levelVoInfo,(i+1));   
            go.transform.localPosition= Vector3.zero;
        }
    }



    /// <summary>
    /// 刷新关卡入口
    /// </summary>
    /// <param name="list"></param>
    public void RefreshLevelItem(List<ActivityLevelRulePB> list)
    {
        var content = transform.Find("Bottom/LevelContent");
        for (int i = 0; i < list.Count; i++)
        {
            var parent = content.GetChild(i).transform;
            var go = parent.GetChild(0).gameObject;          
            var levelVoInfo = GlobalData.CapsuleLevelModel.GetLevelInfo(list[i].LevelId);          
            go.GetComponent<LevelItem>().SetData(levelVoInfo,(i+1));     
        } 
    }
    
    /// <summary>
    /// 生成音游关卡Item
    /// </summary>
    private void CreateMusicLevelItem(List<ActivityMusicVo> list )
    {
        var prefab = GetPrefab("ActivityMusicTemplate/Prefabs/MusicLevelItem");
        var content = transform.Find("Bottom/MusicContent");
      
        for (int i = 0; i < list.Count; i++)
        {
            var parent = content.GetChild(i).transform;
            var go = Instantiate(prefab, parent, false);
            go.GetComponent<MusicLevelItem>().SetData(list[i], (i+1));   
            go.transform.localPosition= Vector3.zero;
        }
    }

    /// <summary>
    /// 刷新音游关卡Item
    /// </summary>
    /// <param name="list"></param>
    public void RefreshMusicLevelItem(List<ActivityMusicVo> list)
    {
        var content = transform.Find("Bottom/MusicContent");
        for (int i = 0; i < list.Count; i++)
        {
            var parent = content.GetChild(i).transform;
            var go = parent.GetChild(0).gameObject;                          
            go.GetComponent<MusicLevelItem>().SetData(list[i],(i+1));     
        } 
    }

    /// <summary>
    /// 从任务调音游（相当于点击音游关卡入口）
    /// </summary>
    /// <param name="id"></param>
    public void JumpToMusicLevel(string id)
    {
        string entranceName = id.Replace("music", null); 
        var content = transform.Find("Bottom/MusicContent");
        content.Find(entranceName).GetChild(0).GetComponent<MusicLevelItem>().JumpTo();        
    }
    
    
    #region 倒计时相关
    
    /// <summary>
    /// 设置活动时间
    /// </summary>
    private void SetActivityTime()
    {		
        var curTimeStamp = ClientTimer.Instance.GetCurrentTimeStamp();
        var surplusDay = DateUtil.GetSurplusDay(curTimeStamp, _endTimeStamp);
        if (surplusDay!=0)
        {
            _time.text = I18NManager.Get("ActivityTemplate_ActivityTemplateTime1",surplusDay);	
        }
        else
        {
            _countDown = ClientTimer.Instance.AddCountDown("CountDown",Int64.MaxValue, 1f, CountDown, null);
            CountDown(0);
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
	
        if (time<0)
        {
            SendMessage(new Message(MessageConst.CMD_ACTIVITY_MUSIC_OVER));
            return;
        }
        else
        {
            long s = (time / 1000) % 60;
            long m = (time / (60 * 1000)) % 60;
            long h = time / (60 * 60 * 1000);
            timeStr = $"{h:D2}:{m:D2}:{s:D2}";
        }
        _time.text = I18NManager.Get("ActivityTemplate_ActivityTemplateTime2",timeStr);		
    }

    public void DestroyCountDown()
    {
        if (_countDown!=null)
        {
            ClientTimer.Instance.RemoveCountDown(_countDown);	
        }
    }
    #endregion
    
    
}
