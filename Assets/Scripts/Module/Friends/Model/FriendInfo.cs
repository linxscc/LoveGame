using System;
using Com.Proto;

public class FriendInfo:IComparable<FriendInfo>
{
    public int UserId; //用户名
    public string UserName;//好友名
    public string SupporterName;//应援团名
    public int Level;//等级
    public int UserHead;//头像
    public long LastLandingTime;//上一次登陆时间
    public int CollectedCardPercent;//星缘收集
    public int IsGetPower;//是否收取能量
    public int IsGivePower;//是否赠送能量
    public int IsBeGivenPower; //是否被赠送
    public int FriendPro; //排序权重

    public FriendInfo(FriendDetailPB pb)
    {
        UserId = pb.UserId;
        UserName = pb.UserName;
        UserHead = pb.UserHead;
        LastLandingTime = pb.LastLoginTime;
        CollectedCardPercent = pb.CardNum;
        Level = pb.DepartmentLevel;
        IsGetPower = pb.PowerGetState;
        IsGivePower = pb.PowerSendState;
        IsBeGivenPower = pb.PowerBeGivenState;
        UpdateFriendPro();
    }

    public void UpdateFriendPro()
    {
        if (IsBeGivenPower==1&&IsGetPower==0&&IsGivePower==0)
        {
            FriendPro = 0;
        }
        else if(IsBeGivenPower==1&&IsGetPower==0&&IsGivePower==1)
        {
            FriendPro = 1;
        }       
        else if(IsBeGivenPower==1&&IsGetPower==1&&IsGivePower==0)
        {
            FriendPro = 2; 
        }
        else if(IsBeGivenPower==0&&IsGetPower==0&&IsGivePower==0)
        {
            FriendPro = 3; 
        }
        else if(IsBeGivenPower==0&&IsGetPower==0&&IsGivePower==1)
        {
            FriendPro = 4; 
        }
        else if(IsBeGivenPower==1&&IsGetPower==1&&IsGivePower==1)
        {
            FriendPro = 5;  
        }
        
        
    }


    public int CompareTo(FriendInfo other)
    {
        int result = 0;
//        if(other.IsBeGivenPower.CompareTo(IsBeGivenPower)!=0)
//        {
//            if (other.IsGetPower.CompareTo(IsGetPower)!=0)
//            {
//                result = -other.IsGetPower.CompareTo(IsGetPower);
//            }
//            else
//            {
//                result = other.IsBeGivenPower.CompareTo(IsBeGivenPower);  
//            }
//
//        }
//        else if(other.IsGivePower.CompareTo(IsGivePower)!=0)
//        {
//            result = other.IsGivePower.CompareTo(IsGivePower);
//        }
        if(other.FriendPro.CompareTo(FriendPro)!=0)
        {
            result = -other.FriendPro.CompareTo(FriendPro);
        }
        else if (other.LastLandingTime.CompareTo(LastLandingTime)!=0)
        {
            result = other.LastLandingTime.CompareTo(LastLandingTime);
        }
        else if(other.UserId.CompareTo(UserId)!=0)
        {
            result = other.UserId.CompareTo(UserId);
        }

        return result;
    }
}
