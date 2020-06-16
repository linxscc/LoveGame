using Assets.Scripts.Framework.GalaSports.Core.Events;
using Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class MusicRhythmItem : MonoBehaviour {
    
    public float longDistance;
    
    float _speed;

    GameObject _short;
    GameObject _long;
    RectTransform _rect;
    Animator _animator;


    private void Awake()
    {
        _speed = 600f;
        _long = transform.Find("Long").gameObject;
        _short = transform.Find("Short").gameObject;
        _rect = GetComponent<RectTransform>();
        _animator = transform.GetComponent<Animator>();
    }


    public bool EqualTick(Tick tick)
    {
        return _tick.Equals(tick);
    }

    private float GetRectY()
    {
        return _rect.localPosition.y;
    }

    // Use this for initialization

    void Start () {
	}

    public void OnUpdate(float delay)
    {
        DoMoving(delay);
        CheckPositon();
    }

    void DoMoving(float delay)
    {
        float dis = _speed * delay;
        transform.localPosition = new Vector3(
              transform.localPosition.x,
              transform.localPosition.y - dis,
              transform.localPosition.z
              );

        transform.GetComponent<CanvasGroup>().alpha = (3000-transform.localPosition.y)/2000;
    }

    public void OnWayDown(int way)
    {
        if(_tick.Way!=way)
        {
            return;
        }
        MusicRhythmClickCallbackType clickType = GetClickCallbackType(0);

        if (_tick.TickType==1)//short
        {
            EventDispatcher.TriggerEvent(EventConst.MusicRhythmItemShortValidClick, _tick, clickType);
        }
        else
        {
            if (clickType!=MusicRhythmClickCallbackType.None)
            {
                isLongDown = true;
                EventDispatcher.TriggerEvent(EventConst.MusicRhythmItemLongValidDownClick, _tick, clickType);
            }
        }
    }

    bool isLongDown = false;

    public void OnWayUp(int way)
    {
        if (_tick.Way != way)
        {
            return;
        }

        if (_tick.TickType == 2 && isLongDown == true)//short
        {
            isLongDown = false;
            var clickType = GetClickCallbackType(longDistance, true);

            EventDispatcher.TriggerEvent(EventConst.MusicRhythmItemLongValidUpClick, _tick, clickType);
            
           // EventDispatcher.TriggerEvent<Tick>(EventConst.MusicRhythmItemLongValidUpClick, _tick);
        }
    }

    float _length;
    Tick _tick;

    bool isFinishedUsed = false;

    public void SetData(Tick tick)
    {
        isLongDown = false;
        isFinishedUsed = false;
        _length = 0;
        _tick = tick;
        bool isShort = _tick.TickType == 1;
        _short.SetActive(isShort);
        _long.SetActive(!isShort);
        if (!isShort)
            _length = tick.Duration* (_speed/100);
        GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, _length);
        if(!isShort)
        {
            longDistance = _length + 100;
        }
    }

    /// <summary>
    /// 当点击时候检测响应
    /// </summary>
    public void ClickCheck()
    {
        
    }


    /// <summary>
    /// 需求short的为0点判断  long判断开始点和结尾点是否在有效范围
    /// </summary>
    /// <param name="range"></param>
    /// <param name="offset"></param>
    /// <returns></returns>
    private bool isValidClick(float offset)
    {
        float y = GetRectY();
        //if (y > range * 0.5|| y + offset < -0.5 * range)
        //    return false;
        //需求short的为0点判断  long判断开始点和结尾点是否在有效范围
        float distance = Mathf.Abs((y + offset));
        return distance <= _badRange;
    }

    private MusicRhythmClickCallbackType GetClickCallbackType(float offset, bool isShow = false)
    {
        float y = GetRectY();
        //到达完美点的距离
        float distans = Mathf.Abs((y + offset));
        if (isShow)
        {
            Debug.Log("distans:"+ distans);
        }
        
        if (distans <= _perfectRange)
        {
            return MusicRhythmClickCallbackType.Perfect;
        }
        else if (distans <= _goodRange)
        {
            return MusicRhythmClickCallbackType.Good;
        }
        else if (distans <= _badRange) 
        {
            return MusicRhythmClickCallbackType.Bad;
        }
        else if (distans <= _missRange)
        {
            return MusicRhythmClickCallbackType.Miss;
        }
        else if (distans < _length)
        {
            return MusicRhythmClickCallbackType.Miss;
        }
        else
        {
            return MusicRhythmClickCallbackType.None;
        }
    }

    float _perfectRange = 50;
    float _goodRange = 100;
    float _badRange = 150;
    float _missRange = 200;


    private bool CheckLongIsRangeDown(float offset)
    {
        float y = GetRectY();

       // Debug.LogError("CheckLongIsRangeDown");
        float distans = y + offset + _badRange;
        if (distans < 0) 
        {
            return false;
        }

        return true ;
    }


    private void CheckPositon()
    {
        //if(isLongDown)
        //{
        //  //  Debug.LogError("CheckPositon " + isLongDown);

        //}
        if (isLongDown&& !CheckLongIsRangeDown(_length))
        {
            isLongDown = false;
            var clickType = GetClickCallbackType(longDistance, true);
            EventDispatcher.TriggerEvent<Tick, MusicRhythmClickCallbackType>(EventConst.MusicRhythmItemLongValidUpClick, _tick, clickType);
            isFinishedUsed = true;
        }
        else if(!isFinishedUsed&& !CheckLongIsRangeDown(_length))
        {
            isFinishedUsed = true;
            var clickType = GetClickCallbackType(_length);
            if(_tick.TickType==1)
            {
                EventDispatcher.TriggerEvent<Tick, MusicRhythmClickCallbackType>(EventConst.MusicRhythmItemShortValidClick, _tick, clickType);
            }
            else if(isLongDown)
            {
                EventDispatcher.TriggerEvent<Tick, MusicRhythmClickCallbackType>(EventConst.MusicRhythmItemLongValidUpClick, _tick, clickType);
            }  
        }

        float MaxPos = -1500;
        if (_tick.TickType==1)
        {
            MaxPos = -1500;
        }
        else
        {
            MaxPos = -1500 - _tick.Duration * 6;
        }

        if (transform.localPosition.y < MaxPos) 
        {
            EventDispatcher.TriggerEvent<Tick>(EventConst.MusicRhythmItemFinishedUse, _tick);
        }
    }
}
