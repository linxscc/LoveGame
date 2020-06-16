using Assets.Scripts.Framework.GalaSports.Core;
using Common;
using DataModel;
using Framework.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OtherDrawView : View
{
    private List<DrawCardResultVo> _drawAwards;
    private DrawCardAnimationEvent _drawCardAnimationEvent;
    private Animator _drawCardA;
    private void Awake()
    {
        _drawAwards = new List<DrawCardResultVo>();
        _drawCardAnimationEvent = transform.Find("07").GetComponent<DrawCardAnimationEvent>();
        _drawCardA = transform.Find("07").GetComponent<Animator>();
        _showIndex = 0;
        GameObject clickObj = transform.Find("Image").gameObject;
        UIEventListener.Get(clickObj).onDown = (data) => { OnDown(data); }; ;

        Button lapiao = transform.Find("07/07-lapiao/Button").GetComponent<Button>();
        lapiao.onClick.AddListener(() =>
        {
            SdkHelper.ShareAgent.ShareDrawCard(_awards);
           // ScreenShotUtil.ScreenShot(ScreenShotType.DrawCard, _awards);
            SetShareTipsShow(false);
        });
    }
    // Use this for initialization
    void Start () {

        _drawCardAnimationEvent.CurDrawCardState = DrawCardAnimationEvent.CURANIMALSTATE.REPEATING;

    }
    void SetShareTipsShow(bool isShowShareTips, int num = -1)
    {
        transform.Find("07/07-lapiao/Button/ShareTips").gameObject.SetActive(isShowShareTips);
        if(num!=-1)
        {
            transform.Find("07/07-lapiao/Button/ShareTips/Text").GetText().text =
               I18NManager.Get("DrawCard_ShareTips",num);
        }
    }
    public void SetData()
    {

    }

    public void IsShowBottomBg(bool isShow)
    {      
       transform.Find("beijing").gameObject.SetActive(isShow); 
    }
    
    private void OnDown(PointerEventData data)
    {
        //Debug.LogError("OnDown 1 "+ _drawCardAnimationEvent.CurDrawCardState);
        switch (_drawCardAnimationEvent.CurDrawCardState)
        {
            case DrawCardAnimationEvent.CURANIMALSTATE.NONE:
                break;
            case DrawCardAnimationEvent.CURANIMALSTATE.PRESS:
                //_pressDownTime = Time.time;
                //_drawCardA.SetBool("IsPress", true);
                //AudioManager.Instance.PlayEffect("draw_star", 1);
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
    private bool _isEnd;
    DrawCardResultVo _awards;
    private int _showIndex;
    private void ShowAwardCard()
    {
        AudioManager.Instance.StopEffect();
        if (_isEnd || _drawAwards.Count == 0)
        {
            AudioManager.Instance.StopDubbing();
            SendMessage(new Message(MessageConst.MODULE_DRAWCARD_GOBACK));
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

    public void SetShowCard(List<DrawCardResultVo> awards, bool isShowLapiao = false)
    {

//        if (GuideManager.GetRemoteGuideStep(GuideTypePB.MainGuide)<GuideConst.MainStep_ChangeRole)
//        {
//            isShowLapiao = false;
//        }
        
        _drawAwards = awards;
        ShowAwardCard();
        transform.Find("07/07-lapiao").gameObject.GetComponent<Image>().enabled = isShowLapiao;
        transform.Find("07/07-lapiao/Button").gameObject.SetActive(isShowLapiao);
        _drawCardAnimationEvent.isCanShowLapiao = isShowLapiao;

        bool isShowShare = !GlobalData.PlayerModel.PlayerVo.IsGetShareAward(ShareTypePB.ShareDraw);
        SetShareTipsShow(isShowShare,10);
    }
}
