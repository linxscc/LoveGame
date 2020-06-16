using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module;
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


public class AttainmentTips : MonoBehaviour
{
    private Transform _tipsWindow;
    private Text _titleTxt;
    private RawImage _propImg;
    private Text _propName;
    private Text _propNum;
    private RectTransform _rect;

    private static AttainmentTips _instance;
    private Queue<MissionRulePB> _attainmentAwardQueue = new Queue<MissionRulePB>();
    private bool _isPlaying = false;

    private float _waitTime = 3.0f;
    private GameObject _onClick;

    private Sequence _tween;

    private TweenState _state = TweenState.Move1State;
    private enum  TweenState
    {
       Move1State,
       Move2State,
       WaitState,
    }
    
    
    
    private void Awake()
    {
        _tipsWindow = transform.Find("TipsWindow");
        _titleTxt = _tipsWindow.GetText("Text");     
        _propImg = _tipsWindow.GetRawImage("Award/PropImage");
        _propName = _tipsWindow.GetText("Award/PropNameText");
        _propNum = _tipsWindow.GetText("Award/PropNum");
        _rect = _tipsWindow.GetRectTransform();

        _onClick = transform.Find("OnClick").gameObject;

        PointerClickListener.Get(_onClick).onClick = go => { OnClickSpeedUp(); };
    }



    /// <summary>
    /// 点击加速
    /// </summary>
    private void OnClickSpeedUp()
    {
       _tween.Kill();


       var move2 = _rect.DOAnchorPosX(_rect.GetWidth(), 0.2f);
       switch (_state)
       {
           case TweenState.Move1State:
               var move1 = _rect.DOAnchorPosX(0f, 0.2f);
               _tween = DOTween.Sequence()
                   .Append(move1)
                   .Append(move2);
               break;
           case TweenState.Move2State:             
           case TweenState.WaitState:
               _tween =  DOTween.Sequence()
                   .Append(move2);
               break;         
       }
 
       _tween.OnComplete(_instance.TweenOver);//动画完成的回调

    }
    
    private void Start()
    {
        _instance = this;
    }

    public static void ShowWindow(MissionRulePB pb, float duration = 0.5f)
    {
       _instance._attainmentAwardQueue.Enqueue(pb);

       if (!_instance._isPlaying)
       {
           _instance.ShowWindowAni(duration);
       }
    }
  

    private void ShowWindowAni(float duration=0.5f )
    {
        
        _onClick.Show();
        _tipsWindow.Show();
        float waitTime = _waitTime;
        _isPlaying = true;
        
        var pb = _instance._attainmentAwardQueue.Dequeue();
        var list = pb.Award;
        _titleTxt.text = I18NManager.Get("Common_TipWindowTxt",pb.MissionDesc);
        PointerClickListener.Get(_tipsWindow.gameObject).onClick = null;
        PointerClickListener.Get(_tipsWindow.gameObject).onClick =go =>
        {
            OnClickSpeedUp();
            ModuleManager.Instance.EnterModule(ModuleConfig.MODULE_STAR_ACTIVITY,true,false,pb.Extra.Days
        );  };
            
       
        
        foreach (var t in list)
        {
            RewardVo rewardVo=new RewardVo(t);
            _propImg.texture = ResourceManager.Load<Texture>(rewardVo.IconPath);
            _propNum.text = rewardVo.Num.ToString();
        }
       
        
        
        var move1 = _rect.DOAnchorPosX(0f, duration);
      
        var move2 = _rect.DOAnchorPosX(_rect.GetWidth(), duration);

        _tween = DOTween.Sequence()
            .Append(move1).AppendCallback(() => { _state = TweenState.WaitState; })
            .AppendInterval(waitTime).AppendCallback(() => { _state = TweenState.Move2State; })
            .Append(move2).AppendCallback(() => { _state = TweenState.Move1State; });
        
        _tween.OnComplete(_instance.TweenOver);//动画完成的回调
    }
  
    private void TweenOver()
    {
       
        _instance.transform.DOKill();
        _isPlaying = false;
        if (_instance._attainmentAwardQueue.Count!=0)
        {
            ShowWindowAni();
        }
        else
        {
           _onClick.Hide();
           _tipsWindow.Hide();
        }
			
    } 
    
}
