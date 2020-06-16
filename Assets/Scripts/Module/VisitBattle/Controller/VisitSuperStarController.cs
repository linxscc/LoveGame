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
using Module.VisitBattle.Data;

public class VisitSuperStarController : Controller
{
    public VisitSuperStarView View;
    private VisitBattleModel _model;


    private bool _isCardLock=false;
    public override void Init()
    {
        _model = GetData<VisitBattleModel>();
        _model.InitCardList(GlobalData.CardModel.UserCardList, _model.LevelVo.NpcId);
        View.SetData(_model.LevelVo, _model);

        if(_model.LevelVo.LevelExtra.CardId != 0)
        {
            var vo= _model.UserCardList.Find((m) => { return m.UserCardVo.CardId == _model.LevelVo.LevelExtra.CardId; });
            if (vo!=null)
            {
                OnSmallHeroCardClick(vo);
                _isCardLock = true;
            }
        }

        EventDispatcher.AddEventListener<VisitBattleUserCardVo>(EventConst.SmallHeroCardClick, OnSmallHeroCardClick);
    }

    private void OnSmallHeroCardClick(VisitBattleUserCardVo vo)
    {

        if (_isCardLock && _model.LevelVo.LevelExtra.CardId==vo.UserCardVo.CardId)
        {
            FlowText.ShowMessage(I18NManager.Get("Visit_Hint3"));
            return;
        }
        int cardOpenNum = _model.CardOpenNum();
        if (_model.SelectedCount >= cardOpenNum)
        {
            if (vo.IsUsed == false)               
                  FlowText.ShowMessage(I18NManager.Get("Visit_Hint4",cardOpenNum));
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
            case MessageConst.CMD_VISITBATTLE_REMOVE_HERO_CARD:
                VisitBattleUserCardVo vo = (VisitBattleUserCardVo)bodys[0];
                if(_isCardLock && _model.LevelVo.LevelExtra.CardId==vo.UserCardVo.CardId)
                {
                    FlowText.ShowMessage(I18NManager.Get("Visit_Hint3"));
                    return;
                }
                _model.SelectedCount--;       
                vo.IsUsed = false;
                View.RemoveCard(vo);
                break;
            case MessageConst.CMD_VISITBATTLE_SUPERSTAR_CONFIRM_ERROR1:
                FlowText.ShowMessage(I18NManager.Get("SupporterActivity_Hint1"));
                break;
            case MessageConst.CMD_VISITBATTLE_SUPERSTAR_QUICKSELECT:
                FlowText.ShowMessage("CMD_VISITBATTLE_SUPERSTAR_QUICKSELECT");
                break;
            case MessageConst.CMD_VISITBATTLE_SUPERSTAR_CONFIRM:
                var req = new VisitingChallengeReq();
                req.LevelId = _model.LevelVo.LevelId;

                List<VisitBattleUserCardVo> cards = (List<VisitBattleUserCardVo>)bodys[1];
                foreach (var vo1 in cards)
                {
                    req.CardIds.Add(vo1.UserCardVo.CardId);
                }

                VisitBattleModel model = GetData<VisitBattleModel>();
                req.Fans.Add(model.FansDict);
                req.Items.Add(model.ItemsDict);

                var data = NetWorkManager.GetByteData(req);
                NetWorkManager.Instance.Send<VisitingChallengeRes>(CMD.VISITINGC_CHALLENGE, data, OnGetChallenge, OnChallengeError);
                break;
        }
    }

    private void OnChallengeError(HttpErrorVo vo)
    {
        SendMessage(new Message(MessageConst.CMD_VISITBATTLE_SHOW_FAIL));
    }

    private void OnGetChallenge(VisitingChallengeRes res)
    {
        GlobalData.PropModel.UpdateProps(res.UserItem);

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
            SendMessage(new Message(MessageConst.CMD_VISITBATTLE_RESULT_DATA, Message.MessageReciverType.MODEL, res.GameResult));
            if (res.GameResult.Star > 0)
            {   
                SendMessage(new Message(MessageConst.CMD_VISITBATTLE_SHOW_BATTLE_WIN_ANIMATION));
                GlobalData.PlayerModel.UpdateUserPower(res.UserPower);
            }
            else
            {
                SendMessage(new Message(MessageConst.CMD_VISITBATTLE_SHOW_FAIL));
            }
        }
        else
        {
            SendMessage(new Message(MessageConst.CMD_VISITBATTLE_SHOW_FAIL));
        }
        SendMessage(new Message(MessageConst.CMD_VISITBATTLE_SET_POWER, Message.MessageReciverType.DEFAULT, res.GameResult.Cap));
    }

    public override void Destroy()
    {
        base.Destroy();
        EventDispatcher.RemoveEvent(EventConst.SmallHeroCardClick);
    }
}
