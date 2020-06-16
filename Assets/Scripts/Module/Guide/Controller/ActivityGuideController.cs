using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core;
using Module.Guide.ModuleView.Activity;
using UnityEngine;


    public class ActivityGuideController : Controller
    {
        public ActivityGuideView View;


        public override void Start()
        {
            View.Step1();
        }
    }

