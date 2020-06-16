using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core;
using Com.Proto;
using DataModel;
using UnityEngine;

public class GrowthCapitalModel : Model
{

    public List<GrowthFunVo> GrowthFunVos;

    public GrowthCapitalModel()
    {
        GrowthFunVos=new List<GrowthFunVo>();
        var growthrule = GlobalData.ActivityModel.BaseActivityRule.GrowthFundRules;
        var usergrowthdata = GlobalData.ActivityModel.AllActivityInfo.UserGrowthFund?.AwardStates;
        foreach (var v in growthrule)
        {
            GrowthFunVo growthvo=new GrowthFunVo(v,usergrowthdata);  
            GrowthFunVos.Add(growthvo); 
        }
        
        
    }

    public void UpdateGrowthData(UserGrowthFundPB pb)
    {

        GlobalData.ActivityModel.AllActivityInfo.UserGrowthFund = pb;
        if (pb.AwardStates!=null)
        {

                foreach (var vo in GrowthFunVos)
                {
                    if (pb.AwardStates.Contains(vo.Id))
                    {
                        //如果存在列表中，则表示已经领奖
                        vo.Weight = 0;
                    }
                }
                
        }
        
        
    }


}
