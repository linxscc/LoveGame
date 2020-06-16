using System;
using System.Collections;
using System.Collections.Generic;
using Common;
using game.main;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SetVolumeWindow : Window
{

    private Transform _content;
    private Slider[] _sliders;

    private float[] _tempValue;
    private string[] _tempKey; 
    private void Awake()
    {
        _content = transform.Find("Content");
        _sliders =_content.GetComponentsInChildren<Slider>();
        
        var bgMusicVolum = AudioManager.Instance.BgMusicVolum;
        var dubbingVolum = AudioManager.Instance.DubbingVolum;
        var effectVolume =AudioManager.Instance.EffectVolume;
      
         _tempValue = new float[3] {bgMusicVolum ,dubbingVolum,effectVolume};
         _tempKey = new string[3] { "BgMusicVolum", "DubbingVolum" , "EffectVolume" };
        
        for (int i = 0; i < _sliders.Length; i++)
        {
            if (!PlayerPrefs.HasKey("SetAudioSize"))
            {
                _sliders[i].value = _tempValue[i];
                _sliders[i].transform.Find("Handle Slide Area/Handle/Text").GetComponent<Text>().text = _tempValue[i] * 100 + "%";
            }
            else
            {
                _sliders[i].value = PlayerPrefs.GetFloat(_tempKey[i]);
                _sliders[i].transform.Find("Handle Slide Area/Handle/Text").GetComponent<Text>().text = (_sliders[i].value*100) + "%";
            }
            _sliders[i].onValueChanged.AddListener(SliderEvent);        
        }
        
    }

    private void SliderEvent(float value)
    {
        PlayerPrefs.SetString("SetAudioSize", "Modify");
        string name = EventSystem.current.currentSelectedGameObject.name;
        var go = EventSystem.current.currentSelectedGameObject;
        var text = go.transform.Find("Handle Slide Area/Handle/Text").GetComponent<Text>();
        value = (float)Math.Round((double)value, 2);
        text.text =(value * 100) + "%";
        switch (name)
        {         
           case "Slider1":
              AudioManager.Instance.SetAudioSize(AudioManager.AudioTypes.Bgm, value);
              PlayerPrefs.SetFloat("BgMusicVolum", value);
           break;
           case "Slider2":
               AudioManager.Instance.SetAudioSize(AudioManager.AudioTypes.Dubbing, value);
               PlayerPrefs.SetFloat("DubbingVolum", value);
           break;
           case "Slider3":
               AudioManager.Instance.SetAudioSize(AudioManager.AudioTypes.Effect, value);
               PlayerPrefs.SetFloat("EffectVolume", value);
           break;  
        }
    }


    protected override void OnClickOutside(GameObject go)
    {
        for (int i = 0; i < _sliders.Length; i++)
        {
            if (!PlayerPrefs.HasKey(_tempKey[i]))
            {
                PlayerPrefs.SetFloat(_tempKey[i], _sliders[i].value);
            }
        }   
        base.OnClickOutside(go);
    }
}
