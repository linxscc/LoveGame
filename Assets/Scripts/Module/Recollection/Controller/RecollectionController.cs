using System;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Core.Events;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module.NetWork;
using Assets.Scripts.Module.Recollection.View;
using Com.Proto;
using Common;
using Componets;
using DataModel;
using game.main;
using Google.Protobuf.Collections;
using System.Diagnostics;
using Assets.Scripts.Module.Framework.Utils;
using Assets.Scripts.Services;
using Framework.GalaSports.Service;
using QFramework;
using UnityEngine;
using Debug = UnityEngine.Debug;


public class RecollectionController : Controller
{
    private RecollectionModel _model;
    private HttpMessage<ChallengeCardMemoriesRes> _httpMessage;

    public RecollectionView View { get; set; }



   
  

    public override void Init()
    {
        _model = GetData<RecollectionModel>();
        NetWorkManager.Instance.Send<CardMemoriesRuleRes>(CMD.CARDMEMORIESC_CARDMEMORIESRULE, null, res =>
        {
            _model.InitRule(res);
            NetWorkManager.Instance.Send<CardMemoriesInfoRes>(CMD.CARDMEMORIESC_CARDMEMORIESINFO, null, OnGetInfo);
        }, null, true, GlobalData.VersionData.VersionDic[CMD.CARDMEMORIESC_CARDMEMORIESRULE]);

       
        EventDispatcher.AddEventListener(EventConst.SendBuyRecolletionPowerEvent, HandleBuyRecolletionPowerEvent);
        EventDispatcher.AddEventListener(EventConst.AwardIsEnough,AwardIsEnough);
        EventDispatcher.AddEventListener(EventConst.DailyRefresh6,DailyRefresh6);
       
    }

    private void DailyRefresh6()
    {
        NetWorkManager.Instance.Send<CardMemoriesInfoRes>(CMD.CARDMEMORIESC_CARDMEMORIESINFO, null, OnGetInfo);
    }


    
    
    
    private void AwardIsEnough()
    {
        if (_model.JumpData!=null && GlobalData.PropModel.GetUserProp(_model.JumpData.RequireId).Num >=_model.JumpData.RequireNum)
        {
            FlowText.ShowMessage(I18NManager.Get("MainLine_Hint1")); 
        }
    }

    private void HandleBuyRecolletionPowerEvent()
    {        
        BuyEnergy();
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
            case MessageConst.MODULE_RECOLLECTION_PLAY:
                DoChallenge((UserCardVo) body[1], (int) body[0]);
                break;
            case MessageConst.CMD_RECOLLECTION_BUY_ENERGY:
                //BuyEnergy();
                break;
            case MessageConst.CMD_RECOLLECTION_BUY_COUNT:
               
                BuyCount((int) body[0],true);
                break;
            case MessageConst.CMD_RECOLLECTION_SHOW_REWARD:
                //ShowReward();
                break;
            case MessageConst.MODULE_CARD_SHOW_SELECTED_CARD:
                var vo = (UserCardVo)body[0];          
                SelectedCard(vo);
                break;
             case MessageConst.CMD_RECOLLECTION_SHOW_BUY_DESC:
                 View.ShowBuyDesc(_model.GetResetGem(),_model.GetResetLastGem()); 
                 break;

        }
    }

    private void BuyCount(int cardId,bool isInitiative)
    {            
       ConsumeGem(cardId,isInitiative);
    }


    //消耗重置道具
//    private void ConsumeResetItem(int cardId)
//    {
//        byte[] data = NetWorkManager.GetByteData(new BuyMemoriesConsumeReq
//        {
//            CardId = cardId,
//            Type = MemoriesConsumeTypePB.MemoriesReselatNum
//        });
//        NetWorkManager.Instance.Send<BuyMemoriesConsumeRes>(CMD.CARDMEMORIESC_BUYMEMORIESCONSUME, data,
//            OnResetCountSuccess);
//    }


    //消耗钻石
    private void ConsumeGem(int cardId,bool isInitiative)
    {              
        int resetGem = _model.GetResetGem();      
        if (resetGem != -1)
        {          
            MemoriesReselatNumWindow win = PopupManager.ShowWindow<MemoriesReselatNumWindow>("Recollection/Prefabs/MemoriesReselatNumWindow");
            var mayBuyNum = _model.ReselatMax - _model.ResetTimes;
            win.SetData(mayBuyNum,resetGem,isInitiative);
            win.WindowActionCallback = evt =>
            {
                if (evt ==WindowEvent.Ok)
                {
                   byte[] data = NetWorkManager.GetByteData(new BuyMemoriesConsumeReq
                  {
                      CardId = cardId,
                      Type = MemoriesConsumeTypePB.MemoriesReselatNum
                   });
                 NetWorkManager.Instance.Send<BuyMemoriesConsumeRes>(CMD.CARDMEMORIESC_BUYMEMORIESCONSUME, data,
                    OnResetCountSuccess); 
                }
            };
        }
        else
        {
            MemoriesReselatNumWindow win = PopupManager.ShowWindow<MemoriesReselatNumWindow>("Recollection/Prefabs/MemoriesReselatNumWindow");
            var mayBuyNum =  _model.ReselatMax - _model.ResetTimes;
            var costGem = _model.GetResetLastGem();
            win.SetData(mayBuyNum,costGem,isInitiative);
            win.WindowActionCallback = evt =>
            {
                if (evt == WindowEvent.Ok)
                {
                    FlowText.ShowMessage(I18NManager.Get("Recollection_BuyNumReachMax"));                        
                }
            };
        } 
    }

    //购买星缘回忆体力
    private void BuyEnergy()
    {
        int energyGem = _model.GetEnergyGem();       
        if (energyGem != -1)
        {

            PopupManager.ShowBuyWindow(PropConst.RecolletionIconId, PropConst.GemIconId, energyGem).WindowActionCallback = evt =>
            {
                if (evt != WindowEvent.Ok)
                    return;

                byte[] data = NetWorkManager.GetByteData(new BuyMemoriesConsumeReq
                {
                    Type = MemoriesConsumeTypePB.MemoriesBuyPower
                });
                NetWorkManager.Instance.Send<BuyMemoriesConsumeRes>(CMD.CARDMEMORIESC_BUYMEMORIESCONSUME, data,
                    OnBuyEnergySuccess);
            };

           
        }
        else
        {
            int lastGem = _model.GetEnergyLastGem();
            
            PopupManager.ShowBuyWindow(PropConst.RecolletionIconId, PropConst.GemIconId, lastGem).WindowActionCallback = evt =>
            {
                if (evt != WindowEvent.Ok)
                    return;
                FlowText.ShowMessage(I18NManager.Get("Recollection_Hint7"));              
            };

           
        }
    }

    private void DoChallenge(UserCardVo userCardVo, int challengeNum)
    {
               
        if (userCardVo.RecollectionCount==0)  //算该卡的冲洗次数
        {
             BuyCount(userCardVo.CardId,false);
             return;
        }
        
        var playerGold = GlobalData.PlayerModel.PlayerVo.Gold;          
        var costGold = GlobalData.ConfigModel.GetConfigByKey(GameConfigKey.MEMORIES_CHALLENGE_CONSUME_GOLD)* challengeNum; 
        
        var playerRecollectionEnergy = GlobalData.PlayerModel.PlayerVo.RecollectionEnergy;   
        var costRecollectionEnergy = GlobalData.ConfigModel.GetConfigByKey(GameConfigKey.MEMORIES_CHALLENGE_CONSUME_POEWR)* challengeNum;


        if (playerRecollectionEnergy>=costRecollectionEnergy && playerGold>=costGold )//体力够，金币够 发请求
        {          
            SendChallengeCardMemoriesReq(userCardVo, challengeNum); 
        }
        else if (playerRecollectionEnergy<costRecollectionEnergy && playerGold>=costGold)//体力不够，金币够
        {
            HandleBuyRecolletionPowerEvent();
        }
        else if(playerRecollectionEnergy>=costRecollectionEnergy && playerGold<costGold)//体力够，金币不够
        {
            FlowText.ShowMessage(I18NManager.Get("Recollection_GoldInsufficient"));
        }
        else if(playerRecollectionEnergy< costRecollectionEnergy && playerGold<costGold) //体力不够，金币不够
        {
            //优先体力
            HandleBuyRecolletionPowerEvent();
        }

    }
    
    private void SendChallengeCardMemoriesReq(UserCardVo userCardVo, int challengeNum)
    {
        
        //为了防止网络慢，前后端数据没同步上，添加Loading界面
        LoadingOverlay.Instance.Show();
        
        //发送请求
        byte[] data = NetWorkManager.GetByteData(new ChallengeCardMemoriesReq
        {
            CardId = userCardVo.CardId,
            ChallengeNum = challengeNum
        });
        //处理响应
        _httpMessage = NetWorkManager.Instance.Send<ChallengeCardMemoriesRes>(CMD.CARDMEMORIESC_CHALLENGECARDMEMORIES,
            data, OnChallengeSuccess, challengeNum);
    }


    
    
    private void OnChallengeSuccess(ChallengeCardMemoriesRes res)
    {
        LoadingOverlay.Instance.Hide();

        int challengeNum = (int) _httpMessage.CustomerData;
        
        GlobalData.RandomEventModel.AddRecollectionTimes(challengeNum);

        View.SetMask(true);
        GlobalData.PropModel.AddProps(res.Award);
        View.MaskAni(1.0f,0.2f,(() =>
        {
            View.SetMask(false);
            RecollentionRewardGetWindow win =   PopupManager.ShowWindow<RecollentionRewardGetWindow>("Recollection/Prefabs/RecollentionRewardGetWindow");
            win.SetData(res.Award,challengeNum);

            win.WindowActionCallback = evt =>
            {
                if (evt== WindowEvent.ClickOutsideToClose)
                {
                    TriggerStarActivityMission(challengeNum);
                    if (GlobalData.RandomEventModel.CheckTrigger(7004))
                        new TriggerService().ShowNewGiftWindow().Execute();
                }
            };
            ClientTimer.Instance.DelayCall(() => {View.ShowMaskBlack();  }, 0.6f);      
            
            
        }));
      


        GlobalData.PlayerModel.UpdateUserPower(res.UserPower);
        GlobalData.PlayerModel.UpdateUserMoney(res.UserMoney);

        GlobalData.CardModel.UpdateUserCards(new[] {res.UserCard});
       


        View.UpdateUserCard(GlobalData.CardModel.GetUserCardById(res.UserCard.CardId));

        _model.UpdateInfo(res.UserCardMemoriesInfo);

     
        NetWorkManager.Instance.Send<CardMemoriesInfoRes>(CMD.CARDMEMORIESC_CARDMEMORIESINFO, null, OnGetInfo);
        
        
      
    }


    private void TriggerStarActivityMission(int num)
    {
        var isMission = GlobalData.MissionModel.IsHaveStarActivityMission;
        if (isMission)
        {
            GlobalData.MissionModel.MissionAttainmentModel.AddStarRecallNum(num);
        }
    }
    
    private void OnGetInfo(CardMemoriesInfoRes res)
    {
        
       
        GlobalData.PlayerModel.UpdateUserPower(res.UserPower);

        _model.Init(res);

      

//        View.SetCost(GlobalData.ConfigModel.GetConfigByKey(GameConfigKey.MEMORIES_CHALLENGE_CONSUME_POEWR),
//            GlobalData.ConfigModel.GetConfigByKey(GameConfigKey.MEMORIES_CHALLENGE_CONSUME_GOLD));
//
//        View.SetRewardBtn(_model.Mission.Status);

        NetWorkManager.Instance.Send<MyCardRes>(CMD.CARDC_MYCARD, null, OnCardRefresh);
    }

    private void OnCardRefresh(MyCardRes res)
    {
        GlobalData.CardModel.InitMyCards(res);
        if (View.UserCardVo != null)
        {
            UserCardVo vo = GlobalData.CardModel.GetUserCardById(View.UserCardVo.CardId);
            View.SelectedCard(vo);
        }
        else if (_model.JumpData != null )
        {
            
            UserCardVo vo = GlobalData.CardModel.GetUserCardById((int) _model.JumpData.PostData);
            View.SelectedCard(vo);
      
        }
        
    }
    
    
//    private void ShowReward()
//    {
//        if (_model.Mission.Status == MissionStatusPB.StatusUnclaimed) //未领取
//        {
//            LoadingOverlay.Instance.Show();
//
//            byte[] data = NetWorkManager.GetByteData(new CardMemoriesMissionReceiveReq
//            {
//                MissionId = _model.Mission.MissionId
//            });
//            NetWorkManager.Instance.Send<CardMemoriesMissionReceiveRes>(CMD.CARDMEMORIESC_CARDMEMORIESMISSIONRECEIVE,
//                data,
//                res =>
//                {
//                    LoadingOverlay.Instance.Hide();
//
//                    RecolletionMissionWindow win =
//                        PopupManager.ShowWindow<RecolletionMissionWindow>(
//                            "Recollection/Prefabs/RecolletionMissionWindow");
//                    
//                    win.ShowReward(res.Award);
//                    win. MaskColor = new Color(0, 0, 0, 0.6f);
//                    UpdataAward(res.Award);
//                    View.SetRewardBtn(MissionStatusPB.StatusBeRewardedWith);
//                    
//                    
//                });
//        }
//        else  //领取或者未完成
//        {
//            RecolletionMissionWindow win =
//                PopupManager.ShowWindow<RecolletionMissionWindow>("Recollection/Prefabs/RecolletionMissionWindow");
//            win.MaskColor = new Color(0, 0, 0, 0.6f);
//            win.SetData(_model.GetCurrentMission(), _model.Mission);
//        }
//    }

    /// <summary>
    /// 更新奖励道具
    /// </summary>
//    private void UpdataAward(RepeatedField<AwardPB> awardPbs)
//    {
//        
//        for (int i = 0; i < awardPbs.Count; i++)
//        {
//            switch (awardPbs[i].Resource)
//            {
//                case ResourcePB.Item:
//                    GlobalData.PropModel.AddProp(awardPbs[i]);
//                    break;
//                case ResourcePB.Power:
//                    GlobalData.PlayerModel.AddPower(awardPbs[i].Num);
//                    break;
//                case ResourcePB.Gem:
//                    GlobalData.PlayerModel.UpdateUserGem(awardPbs[i].Num);
//                    break;
//            }
//        }
//
//     
//    }




    private void OnBuyEnergySuccess(BuyMemoriesConsumeRes res)
    {
        SdkHelper.StatisticsAgent.OnPurchase("星缘回忆碎片购买", _model.GetEnergyGem());
      
        
        GlobalData.PlayerModel.UpdateUserPower(res.UserPower);
        GlobalData.PlayerModel.UpdateUserMoney(res.UserMoney);

        FlowText.ShowMessage(I18NManager.Get("Common_BuySucceed"));

        _model.UpdateInfo(res.UserCardMemoriesInfo);

        
    }
    

    private void OnResetCountSuccess(BuyMemoriesConsumeRes res)
    {
        GlobalData.RandomEventModel.AddRecollectionResetTimes(1);

        if (GlobalData.RandomEventModel.CheckTrigger(7004))
            new TriggerService().ShowNewGiftWindow().Execute();
      
        SdkHelper.StatisticsAgent.OnPurchase("星缘回忆次数重置",_model.GetResetGem());
      
        GlobalData.CardModel.UpdateUserCards(new[] {res.UserCard});
        GlobalData.PlayerModel.UpdateUserMoney(res.UserMoney);
        Debug.LogError(res.UserItem);
        GlobalData.PropModel.UpdateProps(new UserItemPB[] { res.UserItem });
       
        
        _model.UpdateInfo(res.UserCardMemoriesInfo);

       
        View.UpdateUserCard(GlobalData.CardModel.GetUserCardById(res.UserCard.CardId));
        View.ShowBuyDesc(_model.GetResetGem(),_model.GetResetLastGem());

//        if (_isCostResetItem)
//        {
//            FlowText.ShowMessage(I18NManager.Get("Recollection_Hint18"));
//        }
    }

 
    
 

    public void SelectedCard(UserCardVo vo)
    {
        SendMessage(new Message(MessageConst.MODULE_RECOLLECTION_CARD_SELECTED, Message.MessageReciverType.DEFAULT,
            vo));
        View.SelectedCard(vo);
    }

  

    public override void Destroy()
    {
      
        EventDispatcher.RemoveEvent(EventConst.SendBuyRecolletionPowerEvent);
        EventDispatcher.RemoveEvent(EventConst.AwardIsEnough);
        EventDispatcher.RemoveEvent(EventConst.DailyRefresh6);
    }
}