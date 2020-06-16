using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Core.Events;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module.NetWork;
using Com.Proto;
using Common;
using DataModel;
using Framework.GalaSports.Service;
using game.main;
using Module.Battle.Data;
using UnityEngine;
using UnityEngine.EventSystems;

public class CapsuleSuperStarController : Controller
{
    public CapsuleSuperStarView View;
    private CapsuleBattleModel _model;
    
    public override void Init()
    {
        _model = GetData<CapsuleBattleModel>();
        _model.InitCardList(GlobalData.CardModel.UserCardList);
        View.SetData(_model.LevelVo, _model);

        EventDispatcher.AddEventListener<BattleUserCardVo>(EventConst.SmallHeroCardClick, OnSmallHeroCardClick);
    }
    
    private void OnSmallHeroCardClick(BattleUserCardVo vo)
    {
        int cardOpenNum = _model.CardOpenNum();
        if (_model.SelectedCount >= cardOpenNum)
        {
            if (vo.IsUsed == false)
                FlowText.ShowMessage(I18NManager.Get("SupporterActivity_AtMost",
                    cardOpenNum)); // ("最多选择"+cardOpenNum+"个");
            else
            {
                _model.SelectedCount--;
                vo.IsUsed = false;
                View.RemoveCard(vo);
            }
        }
        else if (_model.SelectedCount < cardOpenNum)
        {
            if (vo.IsUsed == false)
            {
                _model.SelectedCount++;
                vo.IsUsed = true;
                View.AddHeroCard(vo);
            }
            else
            {
                _model.SelectedCount--;
                vo.IsUsed = false;
                View.RemoveCard(vo);
            }
        }
    }
    
    public override void OnMessage(Message message)
    {
        string name = message.Name;
        object[] bodys = message.Params;
        switch (name)
        {
            case MessageConst.CMD_BATTLE_REMOVE_HERO_CARD:
                _model.SelectedCount--;
                BattleUserCardVo vo = (BattleUserCardVo) bodys[0];
                vo.IsUsed = false;
                View.RemoveCard(vo);
                break;
            case MessageConst.CMD_BATTLE_SUPERSTAR_CONFIRM_ERROR1:
                FlowText.ShowMessage(I18NManager.Get("SupporterActivity_Hint1"));
                break;
            case MessageConst.CMD_CAPSULEBATTLE_SUPERSTAR_CONFIRM:
                SendCapsuleBattleChallenge(_model.LevelVo, (List<BattleUserCardVo>) bodys[1]);
                break;
            case MessageConst.GUIDE_BATTLE_SUPERSTAR_CONFIRM:
                View.Confirm();
                break;
        }
    }

    private void SendCapsuleBattleChallenge(CapsuleLevelVo level ,List<BattleUserCardVo> cards)
    {
        int activityId = level.ActivityId;
        int levelId = level.LevelId;
       
        ChallengeActivityLevelReq req =new ChallengeActivityLevelReq();

        req.ActivityId = activityId;
        req.LevelId = levelId;
        
        foreach (var vo1 in cards)
        {
            req.CardIds.Add(vo1.UserCardVo.CardId);
        }
        
        CapsuleBattleModel model = GetData<CapsuleBattleModel>();
        req.Fans.Add(model.FansDict);
        req.Items.Add(model.ItemsDict);
        
        byte[] data = NetWorkManager.GetByteData(req);
        NetWorkManager.Instance.Send<ChallengeActivityLevelRes>(CMD.ACTIVITYSTENCILC_CHALLENGE,data,OnGetCapsuleChallenge,OnCapsuleChallengeError);
        

    }

    private void OnGetCapsuleChallenge(ChallengeActivityLevelRes res)
    {
        SendMessage(new Message(MessageConst.CMD_CAPSULEBATTLE_GET_RES));
        GlobalData.PropModel.UpdateProps(res.UserItem);
        Debug.LogError("战斗结束用户关卡信息回包---》"+res.UserActivityLevelInfo);
        GlobalData.CapsuleLevelModel.UpdateUserActivityLevelInfo(res.UserActivityLevelInfo);
     
        List<int> itemIds =new List<int>();
        for (int i = 0; i < res.UserItem.Count; i++)
        {
            itemIds.Add(res.UserItem[i].ItemId);  
        }
        
        foreach (var pb in res.GameResult.Awards)
        {
            if (pb.Resource != ResourcePB.Item || itemIds.Contains(pb.ResourceId))
                continue;

            Debug.LogError("扭蛋战斗奖励回包===>"+pb);
            GlobalData.PropModel.AddProp(pb);
        }
        
        EventDispatcher.TriggerEvent(EventConst.CapsuleBattleOver);
            
        if (res.Ret == -1)
        {
            SendMessage(new Message(MessageConst.CMD_CAPSULEBATTLE_RESULT_DATA, Message.MessageReciverType.MODEL,
                res.GameResult));

            if (res.GameResult.Star>0)
            {
                SdkHelper.StatisticsAgent.OnMissionCompleted(_model.LevelVo.LevelMark); 
               SendMessage(new Message(MessageConst.CMD_CAPSULEBATTLE_SHOW_BATTLE_WIN_ANIMATION)); 
            }
            else
            {
                SdkHelper.StatisticsAgent.OnMissionFail(_model.LevelVo.LevelMark);
                SendMessage(new Message(MessageConst.CMD_CAPSULEBATTLE_SHOW_FAIL));
            }
        }
        else
        {
            SendMessage(new Message(MessageConst.CMD_CAPSULEBATTLE_SHOW_FAIL));
        }
        
        SendMessage(new Message(MessageConst.CMD_CAPSULEBATTLE_SET_POWER,Message.MessageReciverType.DEFAULT,res.GameResult.Cap));
        
      
    }
    
    private void OnCapsuleChallengeError(HttpErrorVo vo)
    {
        SendMessage(new Message(MessageConst.CMD_CAPSULEBATTLE_SHOW_FAIL)); 
    }
     
    public override void Destroy()
    {
        base.Destroy();
        EventDispatcher.RemoveEvent(EventConst.SmallHeroCardClick);
    }
    
}
