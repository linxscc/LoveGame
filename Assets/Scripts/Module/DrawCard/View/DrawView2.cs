using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Module.Sdk;
using Common;
using DataModel;
using Framework.Utils;
using game.main;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrawView2 : View
{
    private List<DrawCardResultVo> _drawAwards;
    private List<DrawCardResultVo> _olddrawAwards;
    private DrawCardAnimationAssist _drawCardAnimationAssist;

    private GameObject _clicBg;
    private bool _isEnd;//是否是最后一张
    private int _showIndex;

    private Button _jumpBtn;
    private Button _lapiao;


    public bool isOtherShow;

    private void Awake()
    {
        isOtherShow = false;

        _drawCardAnimationAssist = transform.Find("01").GetComponent<DrawCardAnimationAssist>();

        _jumpBtn = transform.GetButton("UI/ui_03_skip");

        _jumpBtn.onClick.AddListener(() =>
        {
            AudioManager.Instance.StopEffect();
            if (_showIndex == _drawAwards.Count)
            {
                if (isOtherShow)
                {
                    SendMessage(new Message(MessageConst.MODULE_DRAWCARD_GOBACK));
                }
                else
                {
                    SendMessage(new Message(MessageConst.MODULE_DRAWCARD_SHOW_RESULT_PANEL, _drawAwards));
                }

                return;
            }
            _curShowCard = _drawAwards[_showIndex];

            List<DrawCardResultVo> list = new List<DrawCardResultVo>();
            for (; _showIndex < _drawAwards.Count; _showIndex++)
            {
                //if(_drawAwards[_showIndex].IsNew&&( _drawAwards[_showIndex].Credit==CreditPB.Ssr))
                if (_drawAwards[_showIndex].Credit == CreditPB.Ssr && (_drawAwards[_showIndex].Resource == ResourcePB.Card))
                {
                    list.Add(_drawAwards[_showIndex]);
                }
            }
            _drawAwards = list;
            _showIndex = 0;
            _jumpBtn.gameObject.Hide();
            ShowAwardCard();

        });

          _clicBg = transform.Find("ClickBg").gameObject;
        UIEventListener.Get(_clicBg).onClick = (data) => { ClickBg(data); };

        _lapiao = transform.GetButton("UI/ui_02+");
        _lapiao.onClick.AddListener(()=> {

            SdkHelper.ShareAgent.ShareDrawCard(_curShowCard);
           // ScreenShotUtil.ScreenShot(ScreenShotType.DrawCard, _curShowCard);
  
            SetShareTipsShow(false);


        });

    }


    void SetShareTipsShow(bool isShowShareTips, int num = -1)
    {
  
        transform.Find("UI/ui_02+/ShareTips").gameObject.SetActive(isShowShareTips);
        if (num != -1)
        {
            transform.Find("UI/ui_02+/ShareTips/Text").GetText().text =
               I18NManager.Get("DrawCard_ShareTips", num);
        }
    }

    private void ClickBg(GameObject data)
    {
        Debug.LogError("ClickBg.......................");

        if (_drawCardAnimationAssist.isAnimationPlaying)
            return;
        ShowAwardCard();



    }

    public void SetShowCard(List<DrawCardResultVo> awards, bool isShowLapiao = false)
    {
        _olddrawAwards = awards;
        _drawAwards = awards;
        _isEnd = false;
        _showIndex = 0;
        _drawCardAnimationAssist.isCanShowLapiao = isShowLapiao;

        ShowAwardCard();

        bool isShowShare = !GlobalData.PlayerModel.PlayerVo.IsGetShareAward(ShareTypePB.ShareDraw);
        var rule = GlobalData.PlayerModel.ShareRules.Find(pb => pb.ShareType == ShareTypePB.ShareDraw);
        var num = rule.Awards[0].Num;
        SetShareTipsShow(isShowShare, num);

        if (awards.Count == 1)
        {
            _jumpBtn.gameObject.Hide();
        }
        else{
             _jumpBtn.gameObject.Show();
        }


    }

    DrawCardResultVo _curShowCard;
    private void ShowAwardCard()
    {
        // AudioManager.Instance.StopEffect();
        //AudioManager.Instance.StopEffectByClipName("draw_card_enter");
        if (_isEnd || _drawAwards.Count == 0)
        {
            if (isOtherShow)
            {
                SendMessage(new Message(MessageConst.MODULE_DRAWCARD_GOBACK));
            }
            else
            {
                SendMessage(new Message(MessageConst.MODULE_DRAWCARD_SHOW_RESULT_PANEL, _olddrawAwards));
            }
            //AudioManager.Instance.StopDubbing();
            return;
        }

        // Debug.Log("抽到卡" + _drawAwards.Count);

        _curShowCard = _drawAwards[_showIndex];
        // Debug.LogError("ShowAwardCard " + _drawCardAnimationEvent.CurDrawCardState);
        _drawCardAnimationAssist.SetResultVo(_curShowCard);
        Debug.Log("抽到卡Id->" + _curShowCard.CardId+"  卡名  "+ _curShowCard.Name );
        _showIndex++;

        if (_showIndex == _drawAwards.Count)
        {
            Debug.Log("显示完了！");
            _isEnd = true;
        }
    }



}
