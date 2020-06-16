using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Core.Events;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module.NetWork;
using Assets.Scripts.Services;
using Com.Proto;
using Common;
using DataModel;
using Framework.GalaSports.Service;
using game.main;
using Module.Battle.Data;
using UnityEngine;

public class SuperStarController : Controller
{
    public SuperStarView View;
    private BattleModel _model;


    public override void Init()
    {
        _model = GetData<BattleModel>();
        _model.InitCardList(GlobalData.CardModel.UserCardList);
        View.SetData(_model.LevelVo, _model);

        EventDispatcher.AddEventListener<BattleUserCardVo>(EventConst.SmallHeroCardClick, OnSmallHeroCardClick);

        SendMessage(new Message(MessageConst.TO_GUIDE_BATTLE_SUPERSTAR_START, Message.MessageReciverType.DEFAULT,
            _model.LevelVo));
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

    /// <summary>
    /// 处理View消息
    /// </summary>
    /// <param name="message"></param>
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
            case MessageConst.CMD_BATTLE_SUPERSTAR_CONFIRM:

                var req = new ChallengeReq();
                req.LevelId = _model.LevelVo.LevelId;

                List<BattleUserCardVo> cards = (List<BattleUserCardVo>) bodys[1];
                foreach (var vo1 in cards)
                {
                    req.CardIds.Add(vo1.UserCardVo.CardId);
                }

                BattleModel model = GetData<BattleModel>();
                req.Fans.Add(model.FansDict);
                req.Items.Add(model.ItemsDict);

                var data = NetWorkManager.GetByteData(req);

                GetService<BattleService>()
                    .Request(data)
                    .SetCallback(OnGetChallenge, OnChallengeError)
                    .Execute();

                break;
            case MessageConst.GUIDE_BATTLE_SUPERSTAR_CONFIRM:
                View.Confirm();
                break;
        }
    }

    private void OnChallengeError(HttpErrorVo vo)
    {
        SendMessage(new Message(MessageConst.CMD_BATTLE_SHOW_FAIL));
    }

    private void OnGetChallenge(ChallengeRes res)
    {
        SendMessage(new Message(MessageConst.CMD_BATTLE_GET_RES));

        SendMessage(new Message(MessageConst.TO_GUIDE_BATTLE_RESULT, Message.MessageReciverType.DEFAULT,
            res, _model.LevelVo));

        GlobalData.PropModel.UpdateProps(res.UserItem);
        
        
        //已经更新完毕的道具，奖励里面不在处理
        List<int> itemIds = new List<int>();
        for (int i = 0; i < res.UserItem.Count; i++)
        {
            itemIds.Add(res.UserItem[i].ItemId);
        }

        foreach (var pb in res.GameResult.Awards)
        {
            if (pb.Resource != ResourcePB.Item || itemIds.Contains(pb.ResourceId))
                continue;

            GlobalData.PropModel.AddProp(pb);
        }

        if (res.Ret == -1)
        {
            SendMessage(new Message(MessageConst.CMD_BATTLE_RESULT_DATA, Message.MessageReciverType.MODEL,
                res.GameResult));
            GlobalData.PlayerModel.UpdateUserPower(res.UserPower);
            if (res.GameResult.Star > 0)
            {
                SdkHelper.StatisticsAgent.OnMissionCompleted(_model.LevelVo.LevelMark);
                SendMessage(new Message(MessageConst.CMD_BATTLE_SHOW_BATTLE_WIN_ANIMATION));
            }
            else
            {
                SdkHelper.StatisticsAgent.OnMissionFail(_model.LevelVo.LevelMark);
                SendMessage(new Message(MessageConst.CMD_BATTLE_SHOW_FAIL));
            }
        }
        else
        {
            SendMessage(new Message(MessageConst.CMD_BATTLE_SHOW_FAIL));
        }

        SendMessage(new Message(MessageConst.CMD_BATTLE_SET_POWER, Message.MessageReciverType.DEFAULT,
            res.GameResult.Cap));
        
        GlobalData.RandomEventModel.AddEnergy(_model.LevelVo.CostEnergy);
    }

    public override void Destroy()
    {
        base.Destroy();
        EventDispatcher.RemoveEvent(EventConst.SmallHeroCardClick);
    }
}