using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core;
using UnityEngine;

public class CoaxSleepAniController : Controller
{
    public CoaxSleepAniView View;


   
    public override void Start()
    {
        base.Start();
        View.StarAni();
    }
    


    
}
