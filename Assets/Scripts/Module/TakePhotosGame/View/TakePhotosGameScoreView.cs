using Assets.Scripts.Framework.GalaSports.Core;
using Com.Proto;
using Common;
using DG.Tweening;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using game.main;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static Assets.Scripts.Framework.GalaSports.Core.Message;

public class TakePhotosGameScoreView : View
{
    GameObject _score;
    GameObject _accuracy;
    bool _isFinished;
    private void Awake()
    {
        _score = transform.Find("Score").gameObject;
        _accuracy = transform.Find("Accuracy").gameObject;
        _isFinished = false;
        UIEventListener.Get(gameObject).onClick = (p) => {
          
        };
    }

    void DoNext()
    {

        if (_step == 0)
        {
            SetStep(1);
            return;
        }

        if (_isFinished)
        {
            SendMessage(new Message(MessageConst.MODULE_TAKEPHOTOSGAME_GOTO_RESULT_PANEL));
        }
        else
        {
            SendMessage(new Message(MessageConst.CMD_TAKEPHOTOSGAME_SCORE_VIEW_ONCLICK, MessageReciverType.CONTROLLER));
        }
        Hide();

    }
    private void StartGame()
    {

    }

    int _step = 0;
    void SetStep(int s)
    {
        _step = s;
        if(s==0)
        {
            _score.Hide();
            _accuracy.Show();
            AudioManager.Instance.PlayEffect("levelup");
        }
        else
        {
            AudioManager.Instance.PlayEffect("levelup");
            _score.Show();
            _accuracy.Hide();
        }

    }
    public void SetData(TakePhotoGameStateVo vo, TakePhotosGameShowState state, bool isFinished = false)
    {
        SetStep(0);
        _isFinished = isFinished;

        SetAccuracy(vo.accuracy, state);
        SetScore(vo.score);
        ClientTimer.Instance.DelayCall(() => { DoNext(); }, 1.5f);
        ClientTimer.Instance.DelayCall(() => { DoNext(); }, 3f);
    }
    private void SetScore(int score)
    {
        var nums = GetEveryNumString(score);
        int idx = 0;
        for(int i=0;i<5;i++)
        {
            Image img = transform.GetImage("Score/Num/Image" + i.ToString());
            if(idx>=nums.Count)
            {
                img.gameObject.Hide();
                continue;
            }
            img.gameObject.Show();
            string path = "UIAtlas_TakePhotosGame_Score" + nums[idx];
            img.sprite = AssetManager.Instance.GetSpriteAtlas(path);
            img.SetNativeSize();
            idx++;
        }
    }

    List<int> GetEveryNumString(int score)
    {
        List<int> nums = new List<int>();
        int s = score;
        while (s > 0) 
        {
            int n = s % 10;
            nums.Add(n);
            s = s / 10;
        }
        if (nums.Count == 0)
        {
            nums.Add(0);
        }
        nums.Reverse();
        return nums;
    }

    private void SetAccuracy(float accc, TakePhotosGameShowState state)
    {

        Image accuracy = transform.Find("Accuracy/Image").GetImage();
        string r_path = "UIAtlas_TakePhotosGame_Score" + state.ToString();
        accuracy.sprite = AssetManager.Instance.GetSpriteAtlas(r_path);

        int acc = (int)accc;
        var nums = GetEveryNumString(acc);

        int showIdx = 0;
        for (int i=0;i<4;i++)
        {
            Image img = transform.GetImage("Accuracy/Num/Image" + i.ToString());
            if(showIdx> nums.Count)
            {
                img.gameObject.Hide();
                continue;
            }
            img.gameObject.Show();
            string path = "";
            if (showIdx < nums.Count) 
            {
                path = "UIAtlas_TakePhotosGame_Score" + nums[showIdx];
            }
            else
            {
                path = "UIAtlas_TakePhotosGame_Percent";
            }
            
            img.sprite = AssetManager.Instance.GetSpriteAtlas(path);
            img.SetNativeSize();
            showIdx++;
        }

    }


}
