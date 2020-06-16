using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core;
using Com.Proto;
using DataModel;
using Google.Protobuf.Collections;

public class AccumulativeRechargeModel : Model
{
    public List<AccumulativeRechargeVO> AccumulativeRechargeVos;
    //public RepeatedField<ActivityAccumulativeRechargeRulePB> ActivityAccumulativeRechargeRulePbs;
    public RepeatedField<UserActivityAccumulativeRechargeInfoPB> UserActivityAccumulativeRechargeInfoPbs;

    public AccumulativeRechargeModel()
    {
        AccumulativeRechargeVos = new List<AccumulativeRechargeVO>();
        //var accumulativeRechargeRule = GlobalData.ActivityModel.GetActivityAccumulativeRechargeRulePbs();
        UserActivityAccumulativeRechargeInfoPbs = GlobalData.ActivityModel.AllActivityInfo.UserActivityAccumulativeRechargeInfo;

    }

    public void UpdateActivityAccumlativeRecharge(UserActivityAccumulativeRechargeInfoPB pb)
    {
        if (pb.ReceiveStatus != null)
        {
            foreach (var v in AccumulativeRechargeVos)
            {
                if (pb.ReceiveStatus.Contains(v.GearAmound))
                {
                    v.Weight = 0;
                }
            }
        }
        
        
    }

    /// <summary>
    /// 获取长期充值的累充数据！
    /// </summary>
    public List<AccumulativeRechargeVO> GetLongLastingVo()
    {
        var longlastinguservo = GlobalData.ActivityModel.GetLongLastRechargeInfoBb();
        AccumulativeRechargeVos.Clear();
        if (longlastinguservo==null)
        {
            longlastinguservo=new UserActivityAccumulativeRechargeInfoPB()
            {
//                ActivityId = GlobalData.ActivityModel.GetActivity(ActivityTypePB.ActivityAccumulativeRecharge).ActivityId,
                Amount =  0,
                ReceiveStatus = { 0}
            };    
        }

        int displayNum = 6;
        RepeatedField<ActivityAccumulativeRechargeRulePB> rules = GlobalData.ActivityModel.BaseActivityRule.ActivityAccumulativeRechargeRules;
        for (int i = 0; i < rules.Count; i++)
        {
            AccumulativeRechargeVO uservo = new AccumulativeRechargeVO(rules[i], longlastinguservo.ReceiveStatus, longlastinguservo.Amount); 
            if(i == 6 && uservo.Weight != 1)
            {
                displayNum = 11;
            }

            if (i == 11 && uservo.Weight != 1)
            {
                displayNum = rules.Count - 1;
            }
            
            if(i > displayNum)
                break;
            
            AccumulativeRechargeVos.Add(uservo);
        }
//        foreach (var v in GlobalData.ActivityModel.BaseActivityRule.ActivityAccumulativeRechargeRules)
//        {
//            AccumulativeRechargeVO uservo = new AccumulativeRechargeVO(v, longlastinguservo.ReceiveStatus, longlastinguservo.Amount); 
//            AccumulativeRechargeVos.Add(uservo);
//            
//        }

        return AccumulativeRechargeVos;
    }
}