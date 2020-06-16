using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core.Events;
using Common;
using game.main;
using UnityEngine;
using UnityEngine.UI;

public class VisitSelectItem : MonoBehaviour {

    Transform _visit;
    Transform _weather;
    VisitVo _visitVo;
    private void Awake()
    {
        _visit=transform.Find("Visit");
        _weather = transform.Find("Weather");

        _visit.Find("Button").GetComponent<Button>().onClick.AddListener(() =>
        {
           // Debug.Log("_visit");
            EventDispatcher.TriggerEvent<PlayerPB>(EventConst.VisitSelectItemVisitClick, _visitVo.NpcId);
        });

        _weather.Find("Button").GetComponent<Button>().onClick.AddListener(() =>
        {
          //  Debug.Log("_weather");
            EventDispatcher.TriggerEvent<PlayerPB>(EventConst.VisitSelectItemWeatherClick, _visitVo.NpcId);
        });
    }

    public void SetData(VisitVo visitVo)
    {
        
        Debug.Log("VisitSelectItem SetData " + visitVo.NpcId);
        _visitVo = visitVo;
        //_weather.Find("Text").GetComponent<Text>().text = _visitVo.CurWeatherName + I18NManager.Get("Visit_Visit_Effect")
        //    + _visitVo.MaxVisitTime;
  
        //_weather.Find("Button/Text").GetComponent<Text>().text = _visitVo.CurWeatherName +" x"
        //    + _visitVo.MaxVisitTime;
        Image img = _weather.Find("Button/Image").GetComponent<Image>();
        img.sprite = AssetManager.Instance.GetSpriteAtlas("UIAtlas_Visit_levelWeather" + _visitVo.CurWeatherPB.WeatherId);
        img.SetNativeSize();
        img.gameObject.Show();

        string text = I18NManager.Get("Visit_BattleIntroductionPopupTips",
        _visitVo.CurWeatherName, visitVo.MaxVisitTime- visitVo.CurVisitTime, visitVo.MaxVisitTime); 

        _weather.Find("Button/Text").GetComponent<Text>().text = text;


    }

}
