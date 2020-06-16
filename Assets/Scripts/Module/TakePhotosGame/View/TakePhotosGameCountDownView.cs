using Assets.Scripts.Framework.GalaSports.Core;
using Com.Proto;
using Common;
using System;
using System.Collections;
using System.Collections.Generic;
using game.main;
using UnityEngine;
using UnityEngine.UI;

public class TakePhotosGameCountDownView : View
{
    //    Text _txt;
    Image _image;
    Image _number;
    
    RawImage _target;
    Text _content;
    private void Awake()
    {
        //  _txt = transform.Find("Text").GetComponent<Text>();
        _image = transform.Find("Image").GetComponent<Image>();
        _number = transform.Find("Number").GetComponent<Image>();
        _target = transform.GetRawImage("Target");
        _content = transform.Find("TextBg/Text").GetText();

    }

    public void SetPhotoArea(TakePhotosGameRunningInfo runningInfo)
    {
       
        _target.texture = runningInfo.targetTexture;
        _content.text = I18NManager.Get("TakePhotosGame_CountDownContent", runningInfo.GetCurPhotoOrder + 1);
    }

    private float _countDown;
    public void SetData(int countDown)
    {
        string path = "";
        if(countDown==0)
        {
            path = "UIAtlas_TakePhotosGame_StartGame";
        }
        else
        {
            path = "UIAtlas_TakePhotosGame_Number" + countDown.ToString();
        }
        _number.sprite = AssetManager.Instance.GetSpriteAtlas(path);
        _number.SetNativeSize();
        AudioManager.Instance.PlayEffect("dada");
    }
}
