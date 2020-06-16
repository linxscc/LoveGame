using Assets.Scripts.Framework.GalaSports.Core;
using System.Collections;
using System.Collections.Generic;
using game.main;
using UnityEngine;
using UnityEngine.UI;

public class AirborneGameCountDownView : View
{
//    Text _txt;
    Image _image;
    private void Awake()
    {
      //  _txt = transform.Find("Text").GetComponent<Text>();
        _image = transform.Find("Image").GetComponent<Image>();


    }


    private float _countDown;
    public void SetData(int countDown)
    {
       // _countDown = 3;
       // _txt.text = countDown.ToString();
        _image.sprite = AssetManager.Instance.GetSpriteAtlas("UIAtlas_AirborneGame_CountDown"+ countDown.ToString());
        _image.SetNativeSize();
    }


}
