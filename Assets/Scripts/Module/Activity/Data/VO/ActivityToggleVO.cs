using Assets.Scripts.Module.Framework.Utils;
using Com.Proto;
using DataModel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivityToggleVO
{

    public bool IsShowRedDot;

    public ActivityToggleVO(ActivityType type)
    {
        SetRedDotState(type);
    }

    private void SetRedDotState(ActivityType type)
    {
        IsShowRedDot = GlobalData.ActivityModel.GetCurActivityRedDot(type);
    }

    

}
