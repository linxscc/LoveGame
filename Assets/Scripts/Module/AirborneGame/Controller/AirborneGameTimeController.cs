using Assets.Scripts.Framework.GalaSports.Core;
using DataModel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirborneGameTimeController : AirborneGameUpdateController
{
    //public AirborneGameView CurAirborneGameView;
    public AirborneGameTimeView GameTimeView;
    AirborneGameRunningInfo _runningInfo;
    float runTime = 0;
    float maxTime = 0;
    public override void Start()
    {
        //GameTimeView.StepCallback = Update;
        _runningInfo = GetData<AirborneGameModel>().RunningInfo;
        GameTimeView.SetData(_runningInfo.MaxTime);
    }

    public override void Update(float delay)
    {
        maxTime = _runningInfo.MaxTime;
        runTime += delay;
        float per = (maxTime - runTime) / maxTime;
        if (per < 0)
        { per = 0; }
        GameTimeView.SetProgress(per, maxTime - runTime);
    }
}
