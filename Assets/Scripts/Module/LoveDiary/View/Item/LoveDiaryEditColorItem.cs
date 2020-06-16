using Assets.Scripts.Framework.GalaSports.Core.Events;
using Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LoveDiaryEditColorItem : MonoBehaviour, IPointerClickHandler
{
    RawImage _color;
    
    void Awake()
    {
        _color = transform.Find("RawImage").GetComponent<RawImage>(); 
    }

    public void SetData(Color color)
    {
       _color.color = color;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("OnPointerClick");
        EventDispatcher.TriggerEvent<Color>(EventConst.LoveDiaryEditColorItemClick, _color.color);
    }
}
