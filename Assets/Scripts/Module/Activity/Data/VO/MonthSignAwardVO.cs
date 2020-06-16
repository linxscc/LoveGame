using Com.Proto;
using DataModel;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Module.Framework.Utils;
using game.main;
using UnityEngine;





public class MonthSignAwardVO
{
    public int ActivityId;
    public int DayId;
    public int Id;
    public List<RewardVo> Awards = new List<RewardVo>();
    public bool IsShowVipImage=false;
    public EveryDaySignState State = EveryDaySignState.NotGet;

  
    public MonthSignAwardVO(MonthSignRulePB pB,int id)
    {
        ActivityId = GlobalData.ActivityModel.GetActivityVo(ActivityType.ActivityMonthSignin).ActivityId;
        DayId = pB.Date;
        Id = id;
   
        AddAwards(pB.Awards.ToList());
        
        if (pB.VipDouble == 1)
            IsShowVipImage = true;
    }


    private void AddAwards(List<AwardPB> list)
    {
        foreach (var t in list)
        {
            var rewardVo = new RewardVo(t);
            Awards.Add(rewardVo);
        }
    }


  

  


}
