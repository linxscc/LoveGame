using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module;
using Com.Proto;
using DataModel;
using DG.Tweening;
using game.main;
using game.tools;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SleepAudioPlayView : View
{
    private Slider _progressSlider;
    private Button _btnPause;
    private Button _btnPlay;
    private Text _curTimeText;
    private Text _totalTimeText;


    private void Awake()
    {
        _progressSlider = transform.Find("ProgressSlider").GetComponent<Slider>();
        _btnPause = transform.Find("BtnPause").GetComponent<Button>();
        _btnPlay = transform.Find("BtnPlay").GetComponent<Button>();
        _curTimeText = transform.Find("curTime").GetComponent<Text>();
        _totalTimeText = transform.Find("totalTime").GetComponent<Text>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
