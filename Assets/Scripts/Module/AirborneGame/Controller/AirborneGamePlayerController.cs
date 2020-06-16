using Assets.Scripts.Framework.GalaSports.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirborneGamePlayerController : Controller
{
    public AirborneGameView CurAirborneGameView;

    public override void Start()
    {
        CurAirborneGameView.SetPlay();
    }

}
