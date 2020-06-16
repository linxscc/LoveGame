using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdatableTimer : MonoBehaviour {

    public bool IsPlaying
    {
        set;
        get;
    }

    public float RuningTime
    {
        get;
    }

    float _runningTime;
    private void Reset()
    {
        _runningTime = 0;
    }

    public void Init(Action<float> update)
    {
        IsPlaying = false;
        _runningTime = 0;
        _update = update;
    }

    Action<float> _update;
	void Update () {

        if (IsPlaying == false) 
        {
            return;
        }
        float delta = Time.deltaTime;
        _runningTime =+ delta;
        _update?.Invoke(delta);

    }
}
