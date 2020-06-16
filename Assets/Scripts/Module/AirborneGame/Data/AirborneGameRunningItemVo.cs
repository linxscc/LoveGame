using Com.Proto;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirborneGameRunningItemVo : IComparable<AirborneGameRunningItemVo>
{

    public int ResourceId;
    public ResourcePB Resource;
    public int Count;
    public float TriggerTime;
    public ItemTypeEnum Itemtype;
    public int Speed;

    public bool IsTrigger;

    public int CompareTo(AirborneGameRunningItemVo other)
    {
        return TriggerTime.CompareTo(other.TriggerTime);
    }
}
