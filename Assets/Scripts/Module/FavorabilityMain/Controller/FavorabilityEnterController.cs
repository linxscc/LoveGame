using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Core.Events;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module;
using Assets.Scripts.Module.NetWork;
using Com.Proto;
using Common;
using DataModel;
using game.main;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FavorabilityEnterController : Controller
{
    public FavorabilityEnterView _favorabilityEnterView;

    public override void Start()
    {
        _favorabilityEnterView.SetView();

    }




    /// <summary>
    /// 处理View消息
    /// </summary>
    /// <param name="message"></param>
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
        
    }
}
