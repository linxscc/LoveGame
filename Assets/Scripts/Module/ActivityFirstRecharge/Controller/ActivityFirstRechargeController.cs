using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Core.Events;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module;
using Assets.Scripts.Module.NetWork;
using Com.Proto;
using Com.Proto.Server;
using Common;
using Componets;
using DataModel;
using game.main;
using UnityEngine;
using Utils;

public class ActivityFirstRechargeController : Controller
{   
    public ActivityFirstRechargeView View;
       
    public override void Start()
    {
        
        View.SetData(
            GetData<FirstRechargeModel>().IsRecharge(),
            GetData<FirstRechargeModel>().GetFixedAwards(),
            GetData<FirstRechargeModel>().GetOptionalAwards());                
    }

    public override void OnMessage(Message message)
    {
        string name = message.Name;
        object[] body = message.Params;
        switch (name)
        {
            case MessageConst.CMD_ACTIVITY_FIRST_RECHARGE_BTN:              
                ModuleManager.Instance.EnterModule(ModuleConfig.MODULE_SHOP,false,false,5);
                break;
            case MessageConst.CMD_ACTIVITY_FIRSTRECHARGE_GET_BTN:
                var vo = (FirstRechargeVO)message.Body;
                SendReceiveFirstPrizeReq(vo);
               // TestMethod();
                break;            
        }
    }
  
    /// <summary>
    /// 发生领取首充奖励请求
    /// </summary>
    /// <param name="vO"></param>
    private void SendReceiveFirstPrizeReq(FirstRechargeVO vO)
    {       
        Debug.Log("SendGroup===>"+vO.Group);
        ReceiveFirstPrizeReq req = new ReceiveFirstPrizeReq {Group = vO.Group}; 
        byte[] data = NetWorkManager.GetByteData(req);
        NetWorkManager.Instance.Send<ReceiveFirstPrizeRes>(CMD.ACTIVITY_RECEIVE_FIRSTPRIZE, data, res =>
        {     
              GlobalData.PlayerModel.UpdataUserExtra(res.UserExtraInfo);
              //添加首冲回包奖励
              RewardUtil.AddReward(res.Awards);                                        
              View.ShowGetCardAnimation(res.Awards.ToList());
        });       
    }

//    private void TestMethod()
//    {
//        AwardPB pb = new AwardPB {Resource = ResourcePB.Card, ResourceId = 1108, Num = 1};
//        AwardPB pb1 = new AwardPB {Resource = ResourcePB.Item, ResourceId = 1002, Num = 50};
//        AwardPB pb2 = new AwardPB {Resource = ResourcePB.Memories, Num = 60};
//        AwardPB pb3 = new AwardPB {Resource = ResourcePB.Power,  Num = 60};
//
//        List<AwardPB> list = new List<AwardPB>
//        {
//            pb,
//            pb1,
//            pb2,
//            pb3        
//        };
//
//        View.ShowGetCardAnimation(list);
//    }
}
