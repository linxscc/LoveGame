using Assets.Scripts.Framework.GalaSports.Core;
using game.main;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AirborneGameTimeView : View {

    //public Action StepCallback;
    private Text _text;
    string I18text = "";
    float maxLengeh = 600f;
    float minLengeh = 0f;
    Transform _bar;
    RectTransform _rect;

    private void Awake()
    {
        // _progressBar = transform.Find("ProgressBar").GetComponent<ProgressBar>();
        _bar = transform.Find("PowerBar/ProgressBar/Bar");
        _rect = _bar.GetRectTransform();
        _text = transform.GetText("PowerBar/Text");
    }

    private void Start()
    {
        I18text = I18NManager.Get("AirborneGame_LeftTimeOver");
        SetPercent(1);
    }

    public void SetData(float maxTime)
    {
        _text.text = maxTime.ToString("f2") + I18text;
    }

    public void SetProgress(float per,float leftTime)
    {
        // _progressBar.ProgressFloat = per;
        SetPercent(per);
        if (leftTime < 0)
            leftTime = 0;
        _text.text = leftTime.ToString("f2") + I18text;
    }

    private void SetPercent(float per)
    {

        _rect.SetSize(new Vector2(per * maxLengeh, _rect.GetSize().y));
    }

	// Update is called once per frame
	//void Update () {
 //         StepCallback?.Invoke();
 //   }


}
