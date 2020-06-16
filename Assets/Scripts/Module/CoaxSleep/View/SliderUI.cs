using System.Collections;
using System.Collections.Generic;
using Common;
using UnityEngine;
using UnityEngine.UI;

public enum SliderState
{
    Normal, Down, Up
}
public class SliderUI : Slider
{
    public static SliderState State = SliderState.Normal;
   
    
    
    public override void OnPointerDown(UnityEngine.EventSystems.PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        State = SliderState.Down;
        Debug.LogError("按下");
     
    } 
    
    public override void OnPointerUp(UnityEngine.EventSystems.PointerEventData eventData)
    {
        base.OnPointerUp(eventData);
        State = SliderState.Up;
        Debug.LogError("松开");
        Debug.LogError("松开的时间---》"+AudioManager.Instance.BackgroundAudioSource.time);
    }
    
    public void Reset() {
        State = SliderState.Normal;
    }
}
