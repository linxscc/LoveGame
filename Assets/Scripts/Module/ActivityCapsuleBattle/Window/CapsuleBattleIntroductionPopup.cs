using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core.Events;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module.NetWork;
using Com.Proto;
using Common;
using DataModel;
using game.main;
using Google.Protobuf.Collections;
using Module.Battle.Data;
using UnityEngine;
using UnityEngine.UI;

public class CapsuleBattleIntroductionPopup : Window
{
    private Text _levelNameTxt; //关卡名
    private Text _supportScoreTxt; //应援热度
    private Text _levelDescTxt; //关卡描述
    private Text _playNumTxt; //今日剩余可玩次数

    private Transform _starParent; //星星父物体
    private Transform _dropPropsParent; //掉落道具父物体

    private Button _playOnceBtn;
    private Button _playTimesBtn;
    private Button _startWorkBtn;
    private Button _addPlayNumBtn;


    private CapsuleLevelVo _levelVo;
    private RepeatedField<LevelBuyRulePB> _levelBuyRules;


    private int _moreNum;
    private int _playMax;

    private Action<int, CapsuleLevelVo> _callBack;

    private ActivityVo _curActivityVo;
     

    private void Awake()
    {
        InitMax();
        _levelNameTxt = transform.GetText("Title/Text");
        _supportScoreTxt = transform.GetText("ScoreTxt");
        _levelDescTxt = transform.GetText("DescBg/IntroductionTxt");
        _playNumTxt = transform.GetText("Tips/Text");

        _starParent = transform.Find("StarContainer");
        _dropPropsParent = transform.Find("DropProps");

        var btnContainer = transform.Find("BtnContainer");
        _playOnceBtn = btnContainer.GetButton("PlayOnceBtn");
        _playTimesBtn = btnContainer.GetButton("PlayTimesBtn");
        _startWorkBtn = btnContainer.GetButton("StartWorkBtn");
        _addPlayNumBtn = transform.GetButton("Tips/AddBtn");
        _addPlayNumBtn.onClick.AddListener(AddPlayNum);
        _startWorkBtn.onClick.AddListener(() =>
        {
            
            OnStartBattle(0);
        });
        _playOnceBtn.onClick.AddListener(() =>
        {
            
            OnStartBattle(1);
        });
        _playTimesBtn.onClick.AddListener(() =>
        {
                      
            int challengeTimes = _levelVo.CurPlayNum;
            if (challengeTimes > _playMax)
                challengeTimes = _playMax;
            //最大上限是10
            OnStartBattle(challengeTimes);
        });


       

    }

    private void InitMax()
    {
      //  _moreNum = GlobalData.ConfigModel.GetConfigByKey(GameConfigKey.ACTIVITY_CAPSULE_COPY_SECOND_BUY_COUNT);       
        _playMax = 10;
    }
    
    private void AddPlayNum()
    {
        var win = PopupManager.ShowWindow<CapsuleBattleBuyPlayNumWindow>("ActivityCapsuleBattle/Prefabs/CapsuleBattleBuyPlayNumWindow");
        win.SetData(_levelVo,_curActivityVo);

        win.WindowActionCallback = evt =>
        {
            if (evt == WindowEvent.Ok)//免费的
            {
                Debug.LogError("免费的");
                SendBuyReq( GlobalData.ConfigModel.GetConfigByKey(GameConfigKey.ACTIVITY_CAPSULE_COPY_DAILY_FREE_COUNT));
                //SendBuyReq( 0);
            }
            else if(evt == WindowEvent.Yes) //10次的
            {
                Debug.LogError("10次的");
                SendBuyReq(_moreNum);    
            }
            else if(evt == WindowEvent.Cancel)//1次的
            {
                Debug.LogError("1次的");
                SendBuyReq(1);
            }
        };
    }


    private void SendBuyReq(int buyCount)
    {
        
        BuyActivityLevelCountReq req = new BuyActivityLevelCountReq
        {
            ActivityId = _levelVo.ActivityId,
            LevelId = _levelVo.LevelId,
            BuyCount = buyCount
        };
        
        byte [] date = NetWorkManager.GetByteData(req);
        NetWorkManager.Instance.Send<BuyActivityLevelCountRes>(CMD.ACTIVITYSTENCILC_BUYCOUNT, date, GetBuyRes);
    }

    private void GetBuyRes(BuyActivityLevelCountRes res)
    {
       GlobalData.CapsuleLevelModel.UpdateUserActivityLevelInfo(res.UserActivityLevelInfo);
      
       if (res.UserMoneyPB!=null)
       {
           GlobalData.PlayerModel.UpdateUserMoney(res.UserMoneyPB); 
       }
     
       RefreshWindow(GlobalData.CapsuleLevelModel.GetLevelInfo(_levelVo.LevelId));
    }


    private void RefreshWindow(CapsuleLevelVo vo)
    {
        _levelVo = vo;
        
        _playNumTxt.text = I18NManager.Get("ActivityCapsuleBattle_TodayResiduePlayNum", vo.CurPlayNum);
        SetBtnState(vo);
    }

    public void Init(CapsuleLevelVo vo,ActivityVo curActivity,Action<int,CapsuleLevelVo> callBack)
    {

        _moreNum = curActivity.ActivityExtra.SecondBuyCount;
        
        _callBack = callBack;
        _levelVo = vo;

        _curActivityVo = curActivity;
        
        _levelDescTxt.text = vo.LevelDescription;
        _supportScoreTxt.text = I18NManager.Get("MainLine_BattleIntroductionPopupScore",vo.Score);
        _levelNameTxt.text = vo.LevelName;


        
        
        _playNumTxt.text = I18NManager.Get("ActivityCapsuleBattle_TodayResiduePlayNum", vo.CurPlayNum);

       
        SetCurStarLevel(vo.CurrentStar);
        HandleDrops(vo.DropsList);
        SetBtnState(vo);
    }

    
    
    private void SetBtnState(CapsuleLevelVo vo)
    {
        if (vo.IsPass&&vo.CurPlayNum>0)
        {
            Debug.LogError("显示");
            _playOnceBtn.gameObject.Show();
            _playTimesBtn.gameObject.Show();

            if (vo.CurPlayNum>_playMax)
            {
                _playTimesBtn.transform.GetText("Text").text = I18NManager.Get("MainLine_BattleIntroductionPopupPlayTimes",_playMax);
            }
            else
            {
                _playTimesBtn.transform.GetText("Text").text = I18NManager.Get("MainLine_BattleIntroductionPopupPlayTimes",vo.CurPlayNum); 
            }
            
        }
        else if(vo.IsPass&&vo.CurPlayNum<=0)
        {
            Debug.LogError("隐藏");
            _playOnceBtn.gameObject.Hide();
            _playTimesBtn.gameObject.Hide(); 
        }

        SetAddBtnState(vo);
    }
    
    
    private void SetAddBtnState(CapsuleLevelVo vo)
    {
        if (vo.IsPass)
        {
          _addPlayNumBtn.gameObject.Show();  
        }
        else
        {
            _addPlayNumBtn.gameObject.Hide();     
        }
    }
    private void OnStartBattle(int num)
    {
        
        if (_levelVo.CurPlayNum<=0)
        {
            FlowText.ShowMessage("今日剩余次数不足");
            return;
        }
       
        Close();
        ClientTimer.Instance.DelayCall(()=>
        {

            _callBack(num, _levelVo);
            
           // EventDispatcher.TriggerEvent(EventConst.EnterCapsuleBattle, num, _levelVo);
        }, 0.2f);
    }
  
    private void HandleDrops(List<DropVo> dropsList)
    {
        if (_dropPropsParent.childCount > 0)
        {
            for (int i = _dropPropsParent.childCount - 1; i >= 0; i--)
            {
                DestroyImmediate(_dropPropsParent.GetChild(i));
            }
        }
        
        var prefab = GetPrefab("ActivityCapsuleBattle/Prefabs/CapsuleDropItem");

        for (int i = 0; i < dropsList.Count; i++)
        {
            var item = Instantiate(prefab, _dropPropsParent, false);
            item.GetComponent<CapsuleDropItem>().SetData(dropsList[i]);    
            item.GetComponent<ItemShowEffect>().OnShowEffect(i * 0.2f);
        }
    }

    /// <summary>
    /// 设置星星级别
    /// </summary>
    /// <param name="curStar">当前星级</param>
    private void SetCurStarLevel(int curStar)
    {
        Image star1 = _starParent.GetImage("Star1");
        Image star2 = _starParent.GetImage("Star2");
        Image star3 = _starParent.GetImage("Star3");

        Color lightColor = Color.white;
        Color grayColor = new Color(160 / 255.0f, 160 / 255.0f, 160 / 255.0f);
        
        switch (curStar)
        {
             case 0:
                 star1.color = grayColor;
                 star2.color = grayColor;
                 star3.color = grayColor;
                 break;
             case 1:
                 star1.color = lightColor;
                 star2.color = grayColor;
                 star3.color = grayColor;
                 break;
             case 2:
                 star1.color = lightColor;
                 star2.color = lightColor;
                 star3.color = grayColor;
                 _playOnceBtn.gameObject.SetActive(true);
                 _playTimesBtn.gameObject.SetActive(true);
                 break;
             default:
                 _playOnceBtn.gameObject.SetActive(true);
                 _playTimesBtn.gameObject.SetActive(true);
                 break;
        }
    }
    
}
