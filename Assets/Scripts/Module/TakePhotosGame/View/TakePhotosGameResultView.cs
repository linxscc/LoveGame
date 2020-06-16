using Assets.Scripts.Framework.GalaSports.Core;
using Com.Proto;
using DataModel;
using DG.Tweening;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TakePhotosGameResultView : View
{
    Button _close;
    RawImage _target;
    RawImage _destination;
    GameObject _award;
    GameObject _score;
    private LoopVerticalScrollRect _awardList;

    Button _right;
    Button _left;
    Text _scoreText;

    Tweener _rightTweener;
    Tweener _leftTweener;
    private void Awake()
    {
        _scoreText = transform.GetText("Score/Content");
        _right = transform.GetButton("Score/Right");
        _left = transform.GetButton("Score/Left");
        _right.onClick.AddListener(() => {
            showIdex ++;
            SetScore(showIdex);
            SetTitle(false);
        });
        _left.onClick.AddListener(() =>
        {
            showIdex--;
            SetScore(showIdex);
            SetTitle(false);
        });
        _award = transform.Find("Award").gameObject;
        _score = transform.Find("Score").gameObject;
        _close = transform.GetButton("Close");
        _close.interactable = false;
        _close.onClick.AddListener(() =>
        {
            Debug.LogError("_close ......");
          //  ModuleManager.Instance.GoBack();
            SendMessage(new Message(MessageConst.MODULE_TAKEPHOTOSGAME_GOTO_INTRODUCTION_PANEL));
            

        });
        _target = transform.Find("Score/Target/Image").GetRawImage();
        _destination = transform.Find("Score/Destination/Image").GetRawImage();


        Toggle award = transform.Find("ToggleGroup/Award").GetComponent<Toggle>() ;
        award.onValueChanged.AddListener((b)=> {
            _score.SetActive(!b);
            _award.SetActive(b);
            SetTitle(true);
        });
        Toggle score = transform.Find("ToggleGroup/Score").GetComponent<Toggle>();
        score.onValueChanged.AddListener((b)=> {
            _score.SetActive(b);
            _award.SetActive(!b);
            SetTitle(false);
        });

        _awardList = transform.Find("Award/Scroll View").GetComponent<LoopVerticalScrollRect>();
        _awardList.prefabName = "TakePhotosGame/Items/TakePhotosGameAwardItem";
        _awardList.poolSize = 6;
        _awardList.totalCount = 0;
        ClientData.LoadItemDescData(null);
        ClientData.LoadSpecialItemDescData(null);
    }

    private void SetTitle(bool isAward = true)
    {
        Text title = transform.GetText("Title/Text");
        if(isAward)
        {
            title.text = I18NManager.Get("TakePhotosGame_TotalPoints", _totalScore);
        }
        else
        {

            string num = I18NManager.Get("Common_Number"+ (showIdex+1).ToString());
            title.text= I18NManager.Get("TakePhotosGame_ResultPhoto", num);
        }
    }

    private void Start()
    {
        _rightTweener = _right.transform.DOBlendableMoveBy(new Vector3(-0.1f, 0, 0), 0.6f);
        // tweener.SetAutoKill(false);
        _rightTweener.SetLoops(-1, LoopType.Yoyo);
        // tweener.SetEase(DG.Tweening.Ease.Linear).SetLoops(-1, LoopType.Yoyo);
        _rightTweener.Pause();
        _rightTweener.Play();
        _leftTweener = _left.transform.DOBlendableMoveBy(new Vector3(0.1f, 0, 0), 0.6f);
        // tweener.SetAutoKill(false);
        _leftTweener.SetLoops(-1, LoopType.Yoyo);
        // tweener.SetEase(DG.Tweening.Ease.Linear).SetLoops(-1, LoopType.Yoyo);
        _leftTweener.Pause();
        _leftTweener.Play();
    }

    List<TakePhotoGamePhotoVo> _photoVos;
    int _totalScore = 0;
    int showIdex = 0;
    public void SetData(TakePhotosGameRunningInfo runningInfo, List<AwardPB> aw)
    {
        _totalScore = runningInfo.GetCurScore();
        _photoVos = runningInfo.GetPhotosVo();
        showIdex = 0;
        SetScore(showIdex);
        _close.interactable = true;
        SetAwardData(aw);
        SetTitle(true);
    }

    List<AwardPB> _winRunningItem;
    private void SetAwardData(List<AwardPB> winRunningItem)
    {
        Debug.Log("ActivityAwardWindow "+ winRunningItem.Count);

        _winRunningItem = winRunningItem;
        _awardList.UpdateCallback = AwardListUpdateCallback;
        _awardList.totalCount = _winRunningItem.Count;
        _awardList.RefreshCells();
        _awardList.RefillCells();

    }

    private void AwardListUpdateCallback(GameObject go, int index)
    {
        go.GetComponent<TakePhotosGameAwardItem>().SetData(_winRunningItem[index]);
    }

    private void SetScore(int idx)
    {
        var vo = _photoVos[idx];
        SetRightAndLeftBtn();
        _target.texture = vo.targetTexture;
        _destination.texture = vo.playerTexture;
        Debug.LogError(vo.GetCurScore());
        _scoreText.text = I18NManager.Get("TakePhotosGame_ResultContent", vo.GetCurScore());
    }

    void SetRightAndLeftBtn()
    {
        if (showIdex == 0)
        {
            _left.gameObject.Hide();
        }
        else
        {
            _left.gameObject.Show();
        }

        if (showIdex ==_photoVos.Count-1)
        {
            _right.gameObject.Hide();
        }
        else
        {
            _right.gameObject.Show();
        }
    }


}
