using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Core.Events;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module.NetWork;
using Com.Proto;
using Common;
using Componets;
using DataModel;
using game.main;
using UnityEngine;
using Utils;
public class ActivityTenDaysSigninController : Controller
{

    public ActivityTenDaysSigninView View;

    public override void Init()
    {


    }

    public override void Start()
    {

    }

    public override void OnMessage(Message message)
    {
        string name = message.Name;
        object[] body = message.Params;
        switch (name)
        {

        }
    }

    public override void Destroy()
    {
        base.Destroy();
    }
}

