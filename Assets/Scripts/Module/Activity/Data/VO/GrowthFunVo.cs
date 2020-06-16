using System;
using System.Collections.Generic;
using Com.Proto;
using DataModel;
using Google.Protobuf.Collections;
using UnityEngine;


public class GrowthFunVo:IComparable<GrowthFunVo>
{

    public int Id;
    public int DepartmentLevel;
    public RepeatedField<AwardPB> AwardPbs;
    public int Weight;

    public GrowthFunVo(GrowthFundRulePB pb,RepeatedField<int> awardlist)
    {
        Id = pb.Id;
        DepartmentLevel = pb.DepartmentLevel;
        AwardPbs = pb.Awards;
//        Debug.LogError(pb.DepartmentLevel+" "+GlobalData.PlayerModel.PlayerVo.Level+" "+awardlist.Contains(Id));
        if (pb.DepartmentLevel<=GlobalData.PlayerModel.PlayerVo.Level&&awardlist!=null&&awardlist.Contains(Id))
        {
            //已经领取
            Weight = 0;
        }
        else if(pb.DepartmentLevel>GlobalData.PlayerModel.PlayerVo.Level)//||
        {
            //不能领取  
            Weight = 1;
        }
        else
        {
            //可领取
            Weight = 2;
        }
        

    }
    

    
    /// <summary>
    /// 做成长基金之间的排序
    /// </summary>
    /// <param name="vo"></param>
    /// <returns></returns>
    public int CompareTo(GrowthFunVo other)
    {
        int result = 0;
			
        if (other.Weight.CompareTo(Weight) != 0)
        {
            result = other.Weight.CompareTo(Weight);
        }
        else if (other.Weight.CompareTo(Weight)!=0)
        {
            result = -other.Weight.CompareTo(Weight);
        }


        return result;
    }


}
