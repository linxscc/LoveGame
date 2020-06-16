using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module;
using Assets.Scripts.Module.Framework.Utils;
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

public class ActivityTemplateView : View
{

    private Transform _topTra;
    private Transform _rolesTra;
    private Transform _bottomTra;

    private Transform _roleContent;
    private Transform _tabs;

    private Text _accumulativeTxt;
    private RawImage _accumulativeImg;
    
    private Text _activityTimeTxt;
    private RawImage _activityBgImg;
    private Button _leftBtn;
    private Button _rightBtn;

    private Text _haveNumTxt;
    private RawImage _haveNumIconImg;
	private long _endTimeStamp;
	
	private GameObject _lotteryBtnRedDot;

    private Button _previewBtn; //奖励预览Btn
    private Button _getBtn;     //获取Btn
    private Button _lotteryBtn; //抽奖Btn
    private Button _ruleTipsBtn;

    private RectTransform _progressBar;
    private Transform _tabBarTran;

    private TimerHandler _countDown;
    private int _curIndex;
    private float _speed = 3.0f;
    private int _maxRoleNums;

    private Sequence _sequence;
    private Coroutine _roleBgAniCoroutine;

    private void Awake()
    {
        _topTra = transform.Find("Top");
        _rolesTra = transform.Find("Roles");
        _rolesTra.gameObject.Hide();
        _bottomTra = transform.Find("Bottom");

        _roleContent = _rolesTra.Find("RoleContent");
        _tabs = _rolesTra.Find("Tabs");

        _activityTimeTxt = _topTra.GetText("ActivityTimeBg/Text");


        _activityBgImg = transform.GetRawImage("BG");
        _haveNumTxt = _bottomTra.GetText("HaveNum/Num");
        _haveNumIconImg = _bottomTra.GetRawImage("HaveNum/Icon");

        _previewBtn = _bottomTra.GetButton("PreviewBtn");
        _previewBtn.transform.GetText("Text").text = I18NManager.Get("ActivityTemplate_ActivityTemplatePreviewAwards");

        _getBtn = _bottomTra.GetButton("GetBtn");
        _getBtn.transform.GetText("Text").text = I18NManager.Get("ActivityTemplate_ActivityTemplateGetBtn");

        _lotteryBtn = _bottomTra.GetButton("LotteryBtn");
        //_lotteryBtn.transform.GetText("Text").text = I18NManager.Get("ActivityTemplate_ActivityTemplateLotteryBtn");
        _ruleTipsBtn = _bottomTra.transform.GetButton("BtnRuleTips");
        _ruleTipsBtn.transform.GetText("Text").text = I18NManager.Get("ActivityTemplate_ActivityTemplateRuleTips");

        _lotteryBtnRedDot = _lotteryBtn.transform.Find("Red").gameObject;

        _progressBar = _bottomTra.GetRectTransform("Bottom1/ProgressBar/Mask");
        _tabBarTran = _bottomTra.Find("Bottom1/TabBar");

        _accumulativeTxt = _bottomTra.GetText("Bottom1/Accumulative/Num");
        _accumulativeImg = _bottomTra.GetRawImage("Bottom1/Accumulative/Icon");

        _leftBtn = transform.Find("LeftBtn").GetComponent<Button>();
        _rightBtn = transform.Find("RightBtn").GetComponent<Button>();

        _previewBtn.onClick.AddListener(PreviewBtn);
        _getBtn.onClick.AddListener(GetBtn);
        _lotteryBtn.onClick.AddListener(LotteryBtn);
        _leftBtn.onClick.AddListener(OnBtnLeftClick);
        _rightBtn.onClick.AddListener(OnBtnRightClick);
        _ruleTipsBtn.onClick.AddListener(OnBtnRuleTipsClick);

    }


    public void SetUIData(List<int> playerList, int curPlayerId)
    {
        _activityBgImg.texture = ResourceManager.Load<Texture>("ActivityTemplate/Activity_Template_Bg_" + curPlayerId);
        if (playerList.Count == 0) return;
        if (playerList.Count > 1)
        {
            _lotteryBtn.transform.GetText("Text").text = string.Format(I18NManager.Get("ActivityTemplate_ActivityTemplateLotteryPlayerBtn"), GlobalData.FavorabilityMainModel.GetPlayerName((PlayerPB)curPlayerId));
            _rolesTra.gameObject.Show();
            for (int i = 0; i < playerList.Count; ++i)
            {
                var dot = InstantiatePrefab("ActivityTemplate/Prefabs/Dot", _tabs);
                dot.name = playerList[i].ToString();
                if (curPlayerId == playerList[i])
                {
                    dot.transform.Find("Red").gameObject.Show();
                }
            }
            _leftBtn.gameObject.Show();
            _rightBtn.gameObject.Show();
        }
        else
        {
            _lotteryBtn.transform.GetText("Text").text = I18NManager.Get("ActivityTemplate_ActivityTemplateLotteryBtn");
            _leftBtn.gameObject.Hide();
            _rightBtn.gameObject.Hide();
        }
    }

    public void SetCurPlayer(int curPlayerId)
    {
        _lotteryBtn.transform.GetText("Text").text = string.Format(I18NManager.Get("ActivityTemplate_ActivityTemplateLotteryPlayerBtn"), GlobalData.FavorabilityMainModel.GetPlayerName((PlayerPB)curPlayerId));

        if (_sequence != null)
            _sequence.Kill();
        Tween alphaAnim1 = _activityBgImg.DOColor(new Color(1, 1, 1, 0), 0.3f);
        alphaAnim1.onComplete = () =>
        {
            _activityBgImg.texture = ResourceManager.Load<Texture>("ActivityTemplate/Activity_Template_Bg_" + curPlayerId);
        };
        Tween alphaAnim2 = _activityBgImg.DOColor(new Color(1, 1, 1, 1), 0.3f);
        _sequence = DOTween.Sequence()
            .Append(alphaAnim1)
            .Append(alphaAnim2);
        foreach (Transform t in _tabs)
        {
            if(t.gameObject.name == curPlayerId.ToString())
            {
                t.Find("Red").gameObject.Show();
            }
            else
            {
                t.Find("Red").gameObject.Hide();
            }
        }
    }


    public void SetUiData(ActivityTemplateUIVo vo)
    {
        _activityBgImg.texture = ResourceManager.Load<Texture>(vo.BgPath);
        if (vo.ImagePaths.Length == 0)
            return;

        Vector2 size = new Vector2(vo.Size[0], vo.Size[1]);
        Vector2 point = new Vector2(vo.Pivot[0], vo.Pivot[1]);


        var isNeedCreateDot = vo.ImagePaths.Length > 1;

        for (int i = 0; i < vo.ImagePaths.Length; i++)
        {
            GameObject go = new GameObject();
            go.AddComponent<RawImage>();
            go.transform.SetParent(_roleContent, false);
            var rawImage = go.transform.GetRawImage();
            var rect = go.transform.GetRectTransform();
            rawImage.texture = ResourceManager.Load<Texture>(vo.ImagePaths[i]);
            rect.SetPivotAndAnchors(point);
            rect.SetSize(size);
            go.name = (i + 1).ToString();
            if (i != 0)
                rawImage.color = new Color(rawImage.color.r, rawImage.color.g, rawImage.color.b, 0);

            if (isNeedCreateDot)
            {
                var dot = InstantiatePrefab("ActivityTemplate/Prefabs/Dot", _tabs);
                dot.name = (i + 1).ToString();
                if (i == 0)
                {
                    dot.transform.Find("Red").gameObject.Show();
                    _curIndex = i;
                }

            }
        }
        _rolesTra.gameObject.Show();

        if (isNeedCreateDot)
        {
            _maxRoleNums = vo.ImagePaths.Length;
            _roleBgAniCoroutine = ClientTimer.Instance.DelayCall(RoleBgAni, 2);
        }

    }


    private void RoleBgAni()
	{
		if (transform==null)
		{
			return;
		}
		var curRole = _roleContent.GetChild(_curIndex).GetRawImage();
		RawImage nextRole;

		nextRole = _curIndex+1==_maxRoleNums ? _roleContent.GetChild(0).GetRawImage() : _roleContent.GetChild(_curIndex+1).GetRawImage();
		
		Tween curRoleAlpha =curRole.DOColor(new Color(curRole.color.r,curRole.color.g,curRole.color.b,0),1 );
		Tween nextRoleAlpha =nextRole.DOColor(new Color(nextRole.color.r,nextRole.color.g,nextRole.color.b,1),1 );
		_sequence = DOTween.Sequence()
			.Join(curRoleAlpha)
			.Join(nextRoleAlpha) ;
		_sequence.onComplete = () =>
		{
			_curIndex++;
			ClientTimer.Instance.DelayCall(RoleBgAni, 2);
			if (_curIndex == _maxRoleNums)
			{
				_curIndex = 0;
			}

			for (int i = 0; i < _tabs.childCount; i++)
			{
				if (i==_curIndex)
				{
					_tabs.GetChild(i).transform.Find("Red").gameObject.Show();
				}
				else
				{
					_tabs.GetChild(i).transform.Find("Red").gameObject.Hide();
				}
			}
		};
	}


	private void OnDestroy()
	{		
		_sequence.Kill();
	}

    private void OnBtnLeftClick()
    {
        SendMessage(new Message(MessageConst.CMD_ACTIVITYTEMPLATE1_ON_LEFT_BTN));
    }

    private void OnBtnRightClick()
    {
        SendMessage(new Message(MessageConst.CMD_ACTIVITYTEMPLATE1_ON_RIGHT_BTN));
    }

    private void OnBtnRuleTipsClick()
    {
        PopupManager.ShowCommonRuleWindow(I18NManager.Get("ActivityTemplate_ActivityTemplateRule_Content"), I18NManager.Get("ActivityTemplate_ActivityTemplateRule_Tittle"));
    }

    //抽奖
    private void LotteryBtn()
	{
		SendMessage(new Message(MessageConst.CMD_ACTIVITYTEMPLATE1_ON_LOTTERBTN));
	}

	

	private void GetBtn()
	{
		SendMessage(new Message(MessageConst.CMD_ACTIVITYTEMPLATE1_ON_GETBTN));
	}

	//预览
	private void PreviewBtn()
	{
		SendMessage(new Message(MessageConst.CMD_ACTIVITYTEMPLATE1_ON_PREVIEWBTN));
	}


	public void SetData(ActivityTemplateModel model)
	{
		_accumulativeImg.texture = ResourceManager.Load<Texture>("Prop/" + model.ConsumeItemId);
		_haveNumIconImg.texture= ResourceManager.Load<Texture>("Prop/" + model.ConsumeItemId);
		
		_accumulativeImg.gameObject.Show();
		_haveNumIconImg.gameObject.Show();
		_haveNumTxt.text = I18NManager.Get("ActivityTemplate_ActivityTemplateHaveNum",model.ActivityItemNum());
		_endTimeStamp = model.EndTimeStamp;		
		_accumulativeTxt.text = "x "+model.GetUserActivityHolidayInfo().DrawCount;
		//_progressBar.sizeDelta =new Vector2(model.Progress(),_progressBar.sizeDelta.y);

		_lotteryBtnRedDot.SetActive(model.ActivityItemNum()>=model.Price*model.PlayMoreNum);
		
		SetActivityTime();
		SetProgress(model);
	}


	

	private void SetProgress(ActivityTemplateModel model)
	{
        //找出进度条开始的位置
        int showLen = _tabBarTran.childCount;
        int totalLen = (model.activeHolidayAwardRules != null) ? model.activeHolidayAwardRules.Count : 0;
        int myCount = model.GetUserActivityHolidayInfo().DrawCount;

        int startIndex = 0;
        int index = 0;
        int startValue = 0;
        for (int i = 0; i < totalLen; ++i)
        {
            int weight = model.activeHolidayAwardRules[i].Weight;
            if (myCount >= weight)
            {
                bool isGet = model.GetUserActivityHolidayInfo().ActiveProgress.Contains(weight);
                if (!isGet) break;
                index = i + 1;
                if (index > totalLen - 1) break;
                if (index % showLen == 0)
                {
                    startIndex = index;
                    startValue = weight;
                }
            }
        }
        index = startIndex + showLen;
        if (index >= totalLen) index = totalLen - 1;

        float progress = 0f;
        int lastValue = startValue;
        for (int i = startIndex; i <= index; ++i)
        {
            int weight = model.activeHolidayAwardRules[i].Weight;
            if(myCount >= weight)
            {
                if (i == totalLen - 1)
                {
                    progress += ((float)(myCount - lastValue) / (weight - lastValue)) * (1f / 6f);
                }
                else
                {
                    progress += 1f / 6f;
                }
                lastValue = weight;
                //Debug.LogWarning(">= weight:" + weight + " progress:"+progress);
            }
            else
            {
                progress += ((float)(myCount - lastValue) / (weight-lastValue))*(1f / 6f);
                //Debug.LogWarning("< weight:" + weight + " lastValue:" + lastValue + " progress:"+progress);
                break;
            }
        }
        if (progress > 1) progress = 1f;
        if (progress < 0) progress = 0f;

        //Debug.LogWarning("myCount:"+myCount + "  startValue:"+startValue + "  endValue:"+endValue);
        //Debug.LogWarning("progress:" + progress + " index:"+index);
        _progressBar.sizeDelta = new Vector2(progress * 972.92f, _progressBar.sizeDelta.y);
        //Debug.LogWarning("startIndex:"+startIndex);

        for (int i = 0; i < _tabBarTran.childCount; i++)
		{
            int weight = model.Weight(startIndex + i);
            //int weight = model.Weight(i);
			var	frameBg = _tabBarTran.GetChild(i).Find("FrameBg");
			var	icon = frameBg.GetRawImage("Icon");
			var	redDot = frameBg.Find("RedDot");
			var	num = _tabBarTran.GetChild(i).GetText("WeightTxt");
			num.text = weight.ToString();
			var	mask = _tabBarTran.GetChild(i).Find("Mask");
			var iconNum = frameBg.GetText("IconNum");
			var rewards = model.GetActiveAward(weight);
			PointerClickListener.Get(icon.gameObject).onClick = null;			
			bool isGift =rewards.Count>1;

			if (!isGift)
			{
				RewardVo vo= new RewardVo(rewards[0]);
				icon.texture =ResourceManager.Load<Texture>(vo.IconPath);
				icon.gameObject.Show();
				icon.transform.GetChild(0).gameObject.SetActive(vo.Resource== ResourcePB.Puzzle);
				iconNum.text = vo.Num.ToString();
			}
			else
			{
				string path = "Prop/GiftPack/tongyong2";
				icon.texture =ResourceManager.Load<Texture>(path);
				icon.gameObject.Show();
				iconNum.transform.gameObject.Hide();
				icon.transform.GetChild(0).gameObject.Hide();
			}
			
            //是否初始化
			if (model.GetUserActivityHolidayInfo().ActiveProgress==null)
			{
				redDot.gameObject.Hide();
				mask.gameObject.Hide();
				PointerClickListener.Get(icon.gameObject).onClick = go =>
				{
					if (!isGift)
					{
						FlowText.ShowMessage(ClientData.GetItemDescById(rewards[0].ResourceId, rewards[0].Resource).ItemDesc);
					}
					else
					{
						//打开奖励预览窗口
						var  window = PopupManager.ShowWindow<CommonAwardWindow>("GameMain/Prefabs/AwardWindow/CommonAwardWindow");
						window.SetData(rewards.ToList(),true,ModuleConfig.MODULE_ACTIVITYTEMPLATE);
					}
				};
				
			}
			else
			{
				//是否领取过 isGet为true领取过，false未领取
				var isGet = model.GetUserActivityHolidayInfo().ActiveProgress.Contains(weight);
				//是否可以领取 isMayGet为true可以领，false未领取
				var isMayGet =  model.GetUserActivityHolidayInfo().DrawCount>=weight;
				if (isMayGet && !isGet) //可以领，没领过
				{
                    frameBg.gameObject.Show();
                    mask.gameObject.Hide();
                    redDot.gameObject.Show();
					PointerClickListener.Get(icon.gameObject).onClick = go =>
					{										
					   SendMessage(new Message(MessageConst.CMD_ACTIVITYTEMPLATE1_GET_ACTIVE_AWARD,Message.MessageReciverType.CONTROLLER,weight));
					};
				}
				else if (isMayGet && isGet) //可以领，领过了
				{
					frameBg.gameObject.Hide();
					mask.gameObject.Show();	
				}
				else if (!isMayGet) //不可以领
				{
                    frameBg.gameObject.Show();
                    redDot.gameObject.Hide();
					mask.gameObject.Hide();	
					PointerClickListener.Get(icon.gameObject).onClick = go =>
					{
						if (!isGift)
						{
							FlowText.ShowMessage(ClientData.GetItemDescById(rewards[0].ResourceId, rewards[0].Resource).ItemDesc);
						}
						else
						{
							//打开奖励预览窗口
							var  window = PopupManager.ShowWindow<CommonAwardWindow>("GameMain/Prefabs/AwardWindow/CommonAwardWindow");
							window.SetData(rewards.ToList(),true,ModuleConfig.MODULE_ACTIVITYTEMPLATE);
						}
						
					};
				}
				
			}
		}
	}
	
	

	/// <summary>
	/// 设置活动时间
	/// </summary>
	private void SetActivityTime()
	{		
		var curTimeStamp = ClientTimer.Instance.GetCurrentTimeStamp();
		var surplusDay = DateUtil.GetSurplusDay(curTimeStamp, _endTimeStamp);
		if (surplusDay!=0)
		{
			_activityTimeTxt.text = I18NManager.Get("ActivityTemplate_ActivityTemplateTime1",surplusDay);	
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
	
		if (time<1000)
		{
		  SendMessage(new Message(MessageConst.CMD_ACTIVITYTEMPLATE1_ACTIVITY_OVER));
		  return;
		}
		else
		{
			long s = (time / 1000) % 60;
			long m = (time / (60 * 1000)) % 60;
			long h = time / (60 * 60 * 1000);
			timeStr = $"{h:D2}:{m:D2}:{s:D2}";
		}
		_activityTimeTxt.text = I18NManager.Get("ActivityTemplate_ActivityTemplateTime2",timeStr);		
	}
	
	
	
	public void OnShow()
	{
		SendMessage(new Message(MessageConst.CMD_ACTIVITYTEMPLATE1_ON_SHOW_REFRESH));
	}


	public void DestroyCountDown()
	{
		if (_roleBgAniCoroutine != null)
		{
			ClientTimer.Instance.CancelDelayCall(_roleBgAniCoroutine);
		}

		if (_countDown!=null)
		{
			ClientTimer.Instance.RemoveCountDown(_countDown);	
		}
	}
}
