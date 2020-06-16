using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core;
using Common;
using DG.Tweening;
using game.main;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class CoaxSleepAniView : View
{
    private Transform _content;
    private Button _skipBtn;
    private RawImage _contentImg;

    private List<RawImage> _images;
    private int _maxIndex;
    private int _curIndex=0;
    private List<float> _times =new List<float>{4.0f,3.0f,4.0f,4.0f};
    private Slider _slider;
    
    private AudioClip _audioClip;

    private float _starTime;
    private bool _isStar;
    
    private void Awake()
    {
       
        _content = transform.Find("Content");
        _contentImg = _content.GetRawImage();
        _skipBtn = transform.GetButton("SkipBtn");
       
        _images =new List<RawImage>();

        
        for (int i = 0; i < _content.childCount; i++)
        {
            var img = _content.GetChild(i).GetRawImage();
            _images.Add(img);
        }

        _maxIndex = _images.Count - 1;
        _skipBtn.onClick.AddListener(OnSkipBtn);
        _slider = transform.Find("Slider").GetComponent<Slider>();
    }

    private void Update()
    {
        if (_isStar)
        {
            _starTime += Time.deltaTime;
            _slider.value = _starTime / _audioClip.length;
        }
    }

    //点击跳过
    private void OnSkipBtn()
    {
        SendMessage(new Message(MessageConst.CMD_COAXSLEEP_PLAY_OVER));
    }
    
    public void StarAni()
    {
        var color = _contentImg.color;
        Tween contentImgAlpha = _contentImg.DOColor(new Color(color.r, color.g, color.b, 1), 0.3f);
        contentImgAlpha.onComplete = () =>
        {
            new AssetLoader().LoadAudio(AssetLoader.GetBackgrounMusicById("coaxsleepanibg"),
                (clip, loader) => { _audioClip = clip; });
            _isStar = true;
            AudioManager.Instance.PlayBackgroundMusic(_audioClip);
            _slider.gameObject.Show();
            _skipBtn.gameObject.Show();
            SetCardAni();
        };      
    }

    private void SetCardAni()
    {
        if (_curIndex>_maxIndex)
        {
            SendMessage(new Message(MessageConst.CMD_COAXSLEEP_PLAY_OVER));
            return;
        }
        var color = _images[_curIndex].color;
        Tween curImgAlpha = _images[_curIndex].DOColor(new Color(color.r,color.g,color.b,1),_times[_curIndex] );
        curImgAlpha.onComplete = () =>
        {
            _curIndex++;
            SetCardAni();
        };
    }
       
    
}
