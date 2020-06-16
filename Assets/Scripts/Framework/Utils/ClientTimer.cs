using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

public class TimerHandler
{

    public string Name { get; private set; }
    public Action<int> StepFunction { get; private set; }
    public Action FinishFunction { get; private set; }
    public float Interval { get; private set; }
    public long Finish { get; private set; }
    public float IntervalTime { get; private set; }

    public TimerHandler(string name, long finish, float interval, Action<int> stepFunction, Action finishFunction)
    {
        Name = name;
        StepFunction = stepFunction;
        FinishFunction = finishFunction;
        Interval = interval;
        Finish = finish;
        IntervalTime = 0;
    }

    public bool TryExecute(float delta)
    {
        IntervalTime += delta;
        long currentTimeStamp = ClientTimer.Instance.GetCurrentTimeStamp();
        if (IntervalTime >= Interval)
        {
            int millSecond = (int)(Finish - currentTimeStamp);
            IntervalTime -= Interval;
            if (StepFunction != null)
                StepFunction(millSecond);
        }
        if (currentTimeStamp >= Finish)
        {
            if (FinishFunction != null)
                FinishFunction();
            return true;
        }
        return false;
    }
}

public class ClientTimer : MonoBehaviour
{
    private long _serverTime;
    /// <summary>
    /// 设置服务器时间的本地时刻
    /// </summary>
    private long _serverTimeStart;

    List<TimerHandler> timers = new List<TimerHandler>();

    private static ClientTimer instance;
    public static ClientTimer Instance
    {
        get { return instance; }
    }
    void Awake()
    {
        instance = this;
        InitTimeStamp(0);
    }

    List<TimerHandler> removeHandler = new List<TimerHandler>();//修改主要是解决bug 调用RemoveCountDown当用foreach遍历Collection时，如果对Collection有Add或者Remove操作时，会发生以下运行时错误：
//"Collection was modified; enumeration operation may not execute."：

    void Update()
    {
        if (timers == null || timers.Count == 0) return;
        for(int i= timers.Count-1; i>=0;i--)
        {
            if (timers[i].TryExecute(Time.deltaTime))
            {
                timers.Remove(timers[i]);
            }
        }
    }

    private TimerHandler GetTimer(string name)
    {
        List<TimerHandler> timers = instance.timers;
        foreach (var timer in timers)
        {
            if (timer.Name == name)
                return timer;
        }
        return null;
    }
    
    private long GetTimeStamp()
    {
        TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
        return Convert.ToInt64(ts.TotalMilliseconds);
    }

    public void InitTimeStamp(long serverTime)
    {
        _serverTime = serverTime;
        _serverTimeStart = GetTimeStamp();
    }

    /// <summary>
    /// 当前服务器时间戳
    /// </summary>
    /// <returns></returns>
    public long GetCurrentTimeStamp()
    {
        long time = _serverTime + GetTimeStamp() - _serverTimeStart;
        return time;
    }

    public TimerHandler AddCountDown(string name, long finish, float interval, Action<int> stepCallback, Action finishCallback)
    {
        if (finish < GetCurrentTimeStamp())
        {
            return null;
        }

        var action = new TimerHandler(name, finish, interval, stepCallback, finishCallback);

        TimerHandler timer = GetTimer(name);
        if (timer != null)
        {
            timers.Remove(timer);
        }

        timers.Add(action);
        
        return action;
    }


    public long GetCurHour(long lefttime)
    {
        //Debug.LogError("获得小时！"+lefttime / (60 * 60 * 1000));
        return lefttime / (60 * 60 * 1000) + 1;
    }

    public void RemoveCountDown(TimerHandler handler)
    {
        var timer = GetTimer(handler.Name);
        if (timer != null)
        {
            timers.Remove(timer);
        }
    }

    public void RemoveCountDown(string name)
    {
        var timer = GetTimer(name);
        if (timer != null)
        {
            timers.Remove(timer);
        }
    }

    public void Cleanup()
    {
        timers = new List<TimerHandler>();
        CancelInvoke("socketInvoker");
        CancelInvoke("socketInvoker2");
    }


    private Action _invokeCall;
    private Action _invokeCall2;


    /// <summary>
    /// 
    /// </summary>
    /// <param name="invokeCall"></param>
    /// <param name="time"></param>
    public void AddSocketInvokeRepeating(Action invokeCall, int time)
    {
        _invokeCall = invokeCall;
        CancelInvoke("socketInvoker");
        InvokeRepeating("socketInvoker", time, time);
    }

    public void AddSocketInvokeRepeating2(Action invokeCall, int time)
    {
        _invokeCall2 = invokeCall;
        CancelInvoke("socketInvoker2");
        InvokeRepeating("socketInvoker2", time, time);
    }

    public void CancelInvokeRepeationg()
    {
        CancelInvoke("socketInvoker");
    }
    public void CancelInvokeRepeationg2()
    {
        CancelInvoke("socketInvoker2");
    }

    private void socketInvoker()
    {
        if (_invokeCall != null)
        {
            _invokeCall.Invoke();
        }
    }
    private void socketInvoker2()
    {
        if (_invokeCall2 != null)
        {
            _invokeCall2.Invoke();
        }
    }
    public Coroutine DelayCall<T>(Action<T> callback, float time, T data)
    {
        return StartCoroutine(DoDelayCall(time, callback, data));
    }
    public Coroutine DelayCall(Action callback, float time)
    {
         return StartCoroutine(DoDelayCall(time, callback));
    }

    public void CancelDelayCall(Coroutine coroutine)
    {
        if(coroutine != null)
            StopCoroutine(coroutine);
    }

    IEnumerator DoDelayCall<T>(float delay, Action<T> callback, T data)
    {
        yield return new WaitForSeconds(delay);
        if (this != null && callback != null)
            callback(data);
    }
    IEnumerator DoDelayCall(float delay, Action callback)
    {
        yield return new WaitForSeconds(delay);
        if (this != null && callback != null)
        {
            try
            {
                callback();
            }
            catch (Exception ex)
            {
                Debug.LogWarning(ex + " callback:" + callback.ToString());
            }
        }

    }
}
