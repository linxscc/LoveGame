using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class VirtualJoystick : MonoBehaviour {

    GameObject _bg;
    RectTransform _joystick;
    RectTransform _rect;
    float R = 100;
    float R2 = 10000;
    Vector2 offSet;

    /// <summary>
    /// 方向向量
    /// </summary>
    public Vector2 Dir
    {
        get
        {
            return (offSet/R);
        }
    }

    private void Awake()
    {
        _bg = transform.Find("bg").gameObject;
        _joystick = transform.GetRectTransform("Joystick");
        _rect = GetComponent<RectTransform>();
        UIEventListener.Get(_bg).onBeginDrag = OnBeginDrag;
        UIEventListener.Get(_bg).onDrag = OnDrag;
        UIEventListener.Get(_bg).onEndDrag = OnEndDrag;
        offSet = new Vector2();
    }
    private void OnBeginDrag(PointerEventData eventData)
    {

    }

    private void OnDrag(PointerEventData eventData)
    {
        GetScreenToLocal(eventData.position);
        SetJoystickPos();
       // Debug.LogError("offset "+offSet);
       // Debug.LogError("Dir " + Dir);
    }
    void SetJoystickPos()
    {
        if (offSet.x * offSet.x + offSet.y * offSet.y <= R2) 
        {
            _joystick.SetPositionOfPivot(offSet);
        }
        else
        {
            float angle= Mathf.Atan2(offSet.y,offSet.x);
            //Debug.LogError(angle);
            offSet = new Vector2(Mathf.Cos(angle) * R, Mathf.Sin(angle) * R);
            _joystick.SetPositionOfPivot(offSet);
        }
    }


    void  GetScreenToLocal(Vector2 mousePosition)
    {
        var worldPos = Camera.main.ScreenToWorldPoint(mousePosition); //屏幕坐标转世界坐标
        offSet = _rect.transform.InverseTransformPoint(worldPos); //世界坐标转本地坐
    }

    private void OnEndDrag(PointerEventData eventData)
    {
        offSet = Vector2.zero;
        _joystick.SetPositionOfPivot(offSet);
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
