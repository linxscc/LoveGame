using System.Collections;
using System.Collections.Generic;
using Com.Proto;
using Google.Protobuf.Collections;
using UnityEngine;

public class AccumulativeRechargeVO
{

    public int GearId;//档位Id
    public int ActivityId;//活动Id
    public int GearAmound;//档位金额
    public int CurAmount;
    public RepeatedField<AwardPB> Awards;//奖励
    public int Weight;
    
    


    public AccumulativeRechargeVO(ActivityAccumulativeRechargeRulePB pb,RepeatedField<int> ReceiveIds,int rechargeNum)
    {
        GearId = pb.GearId;
        ActivityId = pb.ActivityId;
        GearAmound = pb.GearAmount;
        Awards = pb.Awards;
        CurAmount = rechargeNum;

        if (ReceiveIds.Contains(GearId)&&rechargeNum>=GearAmound)
        {
            //已经领取
            Weight = 0;
        }
        else if(rechargeNum<GearAmound)
        {
            //不可领取
            Weight = 1;
        }
        else
        {
            //可领取
            Weight = 2;
        }


    }




}
