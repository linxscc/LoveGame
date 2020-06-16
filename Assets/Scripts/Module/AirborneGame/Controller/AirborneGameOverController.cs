using Assets.Scripts.Framework.GalaSports.Core;
using DataModel;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirborneGameOverController : Controller
{

    public AirborneGameOverView OverView;
    
    public override void Start()
    {
        base.Start();
    }

    public void SetData(AirborneGameOverType overType)
    {
        OverView.SetData(overType);
    }
}
