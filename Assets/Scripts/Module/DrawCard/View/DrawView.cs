using System.Collections;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Service;
using Com.Proto;
using DataModel;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
using game.main;
using game.tools;
using Google.Protobuf.Collections;
using System;
using System.Collections.Generic;
using Common;
using static Assets.Scripts.Framework.GalaSports.Core.Message;
using Framework.Utils;

public class DrawView : View
{
    private Button _bgBtn;
    private Image _pressImage;
    private Button _pressBtn;
    private Text _hintTxt;

    public bool IsFrist = true;
    private CanvasGroup _cg;

    //鼠标点击事件
    private float _starTime;
    private float _durationTime;

    private bool IsDown = false;
    private bool IsStop = false;
    private int _showIndex;
    private List<DrawCardResultVo> _drawAwards;
    private List<DrawCardResultVo> _olddrawAwards;
    // private RawImage _cardDisplay;
    private bool _isEnd;

    private Animator _drawCardA;
    private float _pressDownTime;
    private float _delay = 2.0f;
    private bool _isSending;//是否发送消息中

    private DrawCardAnimationEvent _drawCardAnimationEvent;

    private Button _jumpBtn;
    private GameObject _jumpObj;
    private Transform _clickTips;

    bool _isShowShareTips;

    void SetShareTipsShow(bool isShowShareTips, int num = -1)
    {
        transform.Find("07/07-lapiao/Button/ShareTips").gameObject.SetActive(isShowShareTips);
        if (num != -1)
        {
            transform.Find("07/07-lapiao/Button/ShareTips/Text").GetText().text =
               I18NManager.Get("DrawCard_ShareTips", num);
        }
    }

    private void Awake()
    {
        _isSending = false;
        _pressDownTime = 0;
        _clickTips = transform.Find("chouka/Tips");
        _drawCardA = transform.Find("07").GetComponent<Animator>();

        _drawCardAnimationEvent = transform.Find("07").GetComponent<DrawCardAnimationEvent>();
        //  PointerClickListener.Get(gameObject).onClick = (go) => { ShowAwardCard(); };
        GameObject clickObj = transform.Find("Image").gameObject;
        UIEventListener.Get(clickObj).onDown = (data) => { OnDown(data); }; ;
        UIEventListener.Get(clickObj).onUp = (data) => { OnUp(data); };
        //  UIEventListener.Get(gameObject).onDown = (data) => { OnDown(data); }; ;
        // UIEventListener.Get(gameObject).onUp = (data) => { OnUp(data); };

        _jumpObj = transform.Find("07/Jump").gameObject; ;
        _jumpBtn = transform.Find("07/Jump/JumpBtn").GetComponent<Button>();
        _jumpBtn.onClick.AddListener(() =>
        {
            AudioManager.Instance.StopEffect();

            _awards = _drawAwards[_showIndex];
            if (_showIndex == _drawAwards.Count) 
            {
                SendMessage(new Message(MessageConst.MODULE_DRAWCARD_SHOW_RESULT_PANEL, _drawAwards));
                return;
            }

            List<DrawCardResultVo> list = new List<DrawCardResultVo>();
            for(; _showIndex< _drawAwards.Count; _showIndex++)
            {
                //if(_drawAwards[_showIndex].IsNew&&( _drawAwards[_showIndex].Credit==CreditPB.Ssr))
                if (_drawAwards[_showIndex].Credit == CreditPB.Ssr && (_drawAwards[_showIndex].Resource == ResourcePB.Card)) 
                {
                    list.Add(_drawAwards[_showIndex]);
                }
            }
            _drawAwards = list;
            _showIndex = 0;
            _jumpObj.SetActive(false);
            ShowAwardCard();

        });
        Button lapiao = transform.Find("07/07-lapiao/Button").GetComponent<Button>();
        lapiao.onClick.AddListener(() =>
        {
            SdkHelper.ShareAgent.ShareDrawCard(_awards);
           // ScreenShotUtil.ScreenShot(ScreenShotType.DrawCard, _awards);
            SetShareTipsShow(false);
        });
           
        ClientTimer.Instance.DelayCall(   
            () => {
                _clickTips.gameObject.Show();
                TipsTween();
            }, 0.5f);
    }

    private void Start()
    {
        AudioManager.Instance.StopEffect();

        AudioManager.Instance.PlayEffect("draw_card_enter", 1,true);
    }

    private void TipsTween()
    {
        Text text = _clickTips.Find("Text").GetComponent<Text>();
        Tweener alpha1 = text.DOFade(0.25f, 1.0f);
        alpha1.SetAutoKill(false);
        alpha1.SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
    }

    public void SetShowCard(List<DrawCardResultVo> awards,bool isShowLapiao=false)
    {
        _olddrawAwards = awards;
        _drawAwards = awards;
        ShowAwardCard();

        transform.Find("07/07-lapiao").gameObject.GetComponent<Image>().enabled = isShowLapiao;
        transform.Find("07/07-lapiao/Button").gameObject.SetActive(isShowLapiao);
        _drawCardAnimationEvent.isCanShowLapiao = isShowLapiao;

        bool isShowShare = !GlobalData.PlayerModel.PlayerVo.IsGetShareAward(ShareTypePB.ShareDraw);
        var rule = GlobalData.PlayerModel.ShareRules.Find((pb => pb.ShareType == ShareTypePB.ShareDraw));
        var num = rule.Awards[0].Num;
       
        SetShareTipsShow(isShowShare,num);
        if (awards.Count==1)//单抽不能跳过
        {
            return;
        }

        _jumpObj.Show();


        //ClientTimer.Instance.DelayCall(
        //    () => { _jumpBtn.gameObject.Show(); }, 1.2f);
    }

    private void Update()
    {
        if (Time.time - _pressDownTime < _delay || _pressDownTime == 0)
        {
            return;
        }
        if (!_isSending)        //send drawcard to server
        {
            //  Debug.LogError("Update");
            _isSending = true;
            SendMessage(new Message(MessageConst.CMD_DRAWCARD_DRAWCARD));
        }
    }

    private void OnUp(PointerEventData data)
    {
        if (!_isSending)
        {
            _pressDownTime = 0;
            if (_drawCardAnimationEvent.CurDrawCardState == DrawCardAnimationEvent.CURANIMALSTATE.PRESS)
            {
                _drawCardA.SetBool("IsPress", false);
   
                AudioManager.Instance.StopEffectByClipName("draw_star");
                
            }

        }
    }

    private void OnDown(PointerEventData data)
    {
        //Debug.LogError("OnDown 1 "+ _drawCardAnimationEvent.CurDrawCardState);
        switch (_drawCardAnimationEvent.CurDrawCardState)
        {
            case DrawCardAnimationEvent.CURANIMALSTATE.NONE:
                break;
            case DrawCardAnimationEvent.CURANIMALSTATE.PRESS:
                _pressDownTime = Time.time;
                _drawCardA.SetBool("IsPress", true);
                AudioManager.Instance.PlayEffect("draw_star",1);
                break;
            case DrawCardAnimationEvent.CURANIMALSTATE.RESULATING:
                break;
            case DrawCardAnimationEvent.CURANIMALSTATE.RESULATED:
                //can change to repeating state
                ShowAwardCard();
                break;
            case DrawCardAnimationEvent.CURANIMALSTATE.REPEATING:
                break;
            case DrawCardAnimationEvent.CURANIMALSTATE.REPEATED:
                //can change to next repeate state
                AnimatorStateInfo stateInfo = _drawCardA.GetCurrentAnimatorStateInfo(0);
                if (stateInfo.IsName("Base Layer.RepeatResult"))//状态快速切换 一帧内多次切换一些状态 bug解决方法
                {
                    //   Debug.LogError("Base Layer.RepeatResult");
                    return;
                }

                // Debug.LogError("OnDown 2" + _drawCardAnimationEvent.CurDrawCardState);
                ShowAwardCard();
                break;
        }
    }

    public override void Hide()
    {
        gameObject.Hide();
    }

    //  public void Show(DrawRes res, CardModel cardModel)
    public void Show()
    {
        _isEnd = false;
        gameObject.Show();
        _showIndex = 0;
    }


    DrawCardResultVo _awards;
    private void ShowAwardCard()
    {
       // AudioManager.Instance.StopEffect();
        AudioManager.Instance.StopEffectByClipName("draw_card_enter");
        if (_isEnd|| _drawAwards.Count==0)
        {
            AudioManager.Instance.StopDubbing();
            SendMessage(new Message(MessageConst.MODULE_DRAWCARD_SHOW_RESULT_PANEL, _olddrawAwards));
            return;
        }

       // Debug.Log("抽到卡" + _drawAwards.Count);

        _awards = _drawAwards[_showIndex];
        // Debug.LogError("ShowAwardCard " + _drawCardAnimationEvent.CurDrawCardState);
        _drawCardAnimationEvent.SetResultVo(_awards);
        Debug.Log("抽到卡->" + _awards.CardId);
        _showIndex++;

        if (_showIndex == _drawAwards.Count)
        {
            Debug.Log("显示完了！");
            _isEnd = true;
        }
    }

    private void OnDestroy()
    {
    }
}