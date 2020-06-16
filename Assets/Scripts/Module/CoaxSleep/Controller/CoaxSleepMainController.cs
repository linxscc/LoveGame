using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module.NetWork;
using Com.Proto;
using UnityEngine;


public class CoaxSleepMainController : Controller
{
    public CoaxSleepMainView View;

    
    
    
    public override void Init()
    {
        base.Init();
    }

    public override void Start()
    {
        base.Start();
        GetCoaxSleepRules();
    }

  
    

    public override void Destroy()
    {
        base.Destroy();
    }


    public override void OnMessage(Message message)
    {
        string name = message.Name;
        object[] body = message.Params;
        switch (name)
        {
            
        }
    }
    
    /// <summary>
    /// 获取哄睡规则
    /// </summary>
    private void GetCoaxSleepRules()
    {
        NetWorkManager.Instance.Send<CoaxSleepRulesRes>(CMD.COAXSLEEPC_COAXSLEEPRULES, null, res =>
        {
            GetData<CoaxSleepModel>().InitRule(res);
            GetCoaxSleepUserInfo();
        });
    }

    /// <summary>
    /// 获取哄睡用户信息
    /// </summary>
    private void GetCoaxSleepUserInfo()
    {
        NetWorkManager.Instance.Send<CoaxSleepInfosRes>(CMD.COAXSLEEPC_COXAXSLEEPINFOS, null,
            res => { GetData<CoaxSleepModel>().InitData(res); });
    }
  
}


