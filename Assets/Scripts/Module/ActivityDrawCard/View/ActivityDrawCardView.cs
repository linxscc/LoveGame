using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Module;
using Assets.Scripts.Module.Framework.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActivityDrawCardView : View
{
    private LoopVerticalScrollRect _loopVerticalScroll;
    List<ActivityDrawCardVo> _activityDrawCardVos;
    Text _leftTime;
    private void Awake()
    {
        _loopVerticalScroll = transform.Find("Bg/ListContent/DailyGift/DailyGiftList").GetComponent<LoopVerticalScrollRect>();
        _loopVerticalScroll.prefabName = "Activity/Prefabs/ActivityDrawCardItem";
        _loopVerticalScroll.poolSize = 6;
        _loopVerticalScroll.UpdateCallback = ListUpdateCallback;
        _leftTime = transform.GetText("Bg/Time/Text");
        transform.GetButton("Bg/GotoDrawCard").onClick.AddListener(() =>
        {
            ModuleManager.Instance.EnterModule(ModuleConfig.MODULE_DRAWCARD,false,true);

        });
    
    }
    private void SetCurTime(long t)
    {
        transform.Find("Bg/CurTime").GetText().text = I18NManager.Get("Activity_DrawCardTenTime", t);
    }
    private void ListUpdateCallback(GameObject obj, int arg2)
    {
        obj.GetComponent<ActivityDrawCardItem>().SetData(_activityDrawCardVos[arg2]);
    }

    public void SetData(List<ActivityDrawCardVo> vos, long leftTime ,long endTime,long curDrawTime)
    {
        _loopVerticalScroll.totalCount = vos.Count;
        _activityDrawCardVos = vos;
        _loopVerticalScroll.RefreshCells();
        _endTimeStamp = endTime;
        var d=  DateUtil.GetDataTime(leftTime);
        //_leftTime.text = I18NManager.Get("Activity_SevenActivityResidueDays", d.Day);
        SetActivityTime();
        SetCurTime(curDrawTime);
    }
    private long _endTimeStamp;
    private TimerHandler _countDown = null;
    private void SetActivityTime()
    {
        var curTimeStamp = ClientTimer.Instance.GetCurrentTimeStamp();
        var surplusDay = DateUtil.GetSurplusDay(curTimeStamp, _endTimeStamp);
        if (surplusDay != 0)
        {
            _leftTime.text = I18NManager.Get("Activity_SevenActivityResidueDays", surplusDay);
        }
        else
        {
            if(_countDown==null)
            {
                _countDown = ClientTimer.Instance.AddCountDown("CountDown", Int64.MaxValue, 1f, CountDown, null);
                CountDown(0);
            }
        }
    }
    private void CountDown(int obj)
    {
        string timeStr = "";
        var curTimeStamp = ClientTimer.Instance.GetCurrentTimeStamp();
        long time = _endTimeStamp - curTimeStamp;

        if (time < 1000)
        {
            SendMessage(new Message(MessageConst.CMD_ACTIVITYTEMPLATE1_ACTIVITY_OVER));
            return;
        }
        else
        {
            long s = (time / 1000) % 60;
            long m = (time / (60 * 1000)) % 60;
            long h = time / (60 * 60 * 1000);
            timeStr = $"{h:D2}:{m:D2}:{s:D2}";
        }
        _leftTime.text = I18NManager.Get("ActivityTemplate_ActivityTemplateTime2", timeStr);
    }

    public void DestroyCountDown()
    {
        if (_countDown != null)
        {
            ClientTimer.Instance.RemoveCountDown(_countDown);
            _countDown = null;
        }
    }
}
