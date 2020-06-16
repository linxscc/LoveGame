using System;
using Assets.Scripts.Module.Framework.Utils;
using Com.Proto;
using DataModel;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;




public class EveryDayPowerVO
{
    public int Id;
    public int Gem;
    public int ActivityId;
   
    public List<RewardVo> Awards = new List<RewardVo>();
    public EveryDaySignState State;
    private long _startTimeStamp;
    private long _endTimeStamp;
    public string StarTime;
    public string EndTime;
    public EveryDayPowerVO(ActivityPowerGetRulePB pB)
    {
        Id = pB.Id;
        Gem = pB.Gem;

      
        ActivityId = GlobalData.ActivityModel.GetActivityVo(ActivityType.ActivityEveryDayPower).ActivityId;
      
        AddAwards(pB.Award.ToList());
       
        var starStr = pB.Start.Split(':');
        var endStr = pB.End.Split(':');
        StarTime = starStr[0] +":"+starStr[1];
        EndTime = endStr[0] + ":" + endStr[1];

        SetStarAndEndTimeStamp(starStr, endStr);
        
     
        
       
        

        

        SetState();
      
    }

    private void SetStarAndEndTimeStamp(string[] starStr,string[]endStr)
    {
        
        var curTimeStamp = ClientTimer.Instance.GetCurrentTimeStamp(); 
        var todayDt =  DateUtil.GetDataTime(curTimeStamp);
        var todayZeroDt = new DateTime(todayDt.Year, todayDt.Month, todayDt.Day, 0, 0, 0);
        var todaySixDt = new DateTime(todayDt.Year, todayDt.Month, todayDt.Day, 6, 0, 0);

        var todayZeroTimeStamp = DateUtil.GetNotTimezoneTimeStamp(todayZeroDt);
        var todaySixTimeStamp = DateUtil.GetNotTimezoneTimeStamp(todaySixDt);

        if (curTimeStamp>=todayZeroTimeStamp &&  curTimeStamp<todaySixTimeStamp)      //上一天的开始时间
        {
            var lastDayDt = todayDt.AddDays(-1);
            var lastDayStarDt = new  DateTime(lastDayDt.Year,lastDayDt.Month,lastDayDt.Day,int.Parse(starStr[0]),int.Parse(starStr[1]),0,0);
            _startTimeStamp = DateUtil.GetNotTimezoneTimeStamp(lastDayStarDt);
            
            var lastDayEndDt =new  DateTime(lastDayDt.Year,lastDayDt.Month,lastDayDt.Day,int.Parse(endStr[0]),int.Parse(endStr[1]),0,0);
            _endTimeStamp =DateUtil.GetNotTimezoneTimeStamp(lastDayEndDt);
        }
        else
        {
            var todayStarDt = new DateTime(todayDt.Year,todayDt.Month,todayDt.Day,int.Parse(starStr[0]),int.Parse(starStr[1]),0,0);
            _startTimeStamp = DateUtil.GetNotTimezoneTimeStamp(todayStarDt);
            
            var todayDayEndDt =new DateTime(todayDt.Year,todayDt.Month,todayDt.Day,int.Parse(endStr[0]),int.Parse(endStr[1]),0,0);
            _endTimeStamp =DateUtil.GetNotTimezoneTimeStamp(todayDayEndDt); 
        }
        
        Debug.LogError("_startTimeStamp===>"+_startTimeStamp+"=========_endTimeStamp===>"+_endTimeStamp);
    }
    
    
    private void AddAwards(List<AwardPB> pBs)
    {
        for (int i = 0; i < pBs.Count; i++)
        {
            var vo = new RewardVo(pBs[i]);
            Awards.Add(vo);
        }
    }

  

    private void SetState()
    {

        var curTimeStamp = ClientTimer.Instance.GetCurrentTimeStamp();
        var userPowerGetInfoList = GlobalData.ActivityModel.GetUserPowerGottenIds();
      

        if (userPowerGetInfoList.Count==0)
        {
            if (curTimeStamp<_startTimeStamp)
            {
                State= EveryDaySignState.NotGet;
            }
            else if (_startTimeStamp <= curTimeStamp && curTimeStamp<=_endTimeStamp)
            {
                State = EveryDaySignState.CurMayGet;
            }
            else if (curTimeStamp> _endTimeStamp)
            {
                State = EveryDaySignState.Retroactive;
            }
        }
        else
        {
            for (int i = 0; i < userPowerGetInfoList.Count; i++)
            {
                if (userPowerGetInfoList[i]==Id)
                {
                    State = EveryDaySignState.AlreadyGet;
                    break;
                }
                else
                {
                    if (curTimeStamp < _startTimeStamp)
                    {
                        State = EveryDaySignState.NotGet;
                    }
                    else if (_startTimeStamp <= curTimeStamp && curTimeStamp <= _endTimeStamp)
                    {
                        State = EveryDaySignState.CurMayGet;
                    }
                    else if (curTimeStamp > _endTimeStamp)
                    {
                        State = EveryDaySignState.Retroactive;
                    }
                }

            }
        }


    }
   

}
