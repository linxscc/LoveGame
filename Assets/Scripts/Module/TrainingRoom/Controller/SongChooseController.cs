using System;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Core.Events;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module;
using Assets.Scripts.Module.NetWork;
using Assets.Scripts.Module.TrainingRoom.Service;
using Com.Proto;
using Common;
using Componets;
using DataModel;
using game.main;

public class SongChooseController : Controller
{
    private int _activityId;
    private int _diffType;

    public SongChooseView View;

    public override void Start()
    {
        EventDispatcher.AddEventListener<int>(EventConst.ChangeAbility, OpenRefreshWindow);
        EventDispatcher.AddEventListener<UserMusicGameVO>(EventConst.GotoChooseCard, GotoChooseCard);
        // EventDispatcher.AddEventListener<UserMusicGameVO, int>(EventConst.StartPlay, StartPlay);
        NetWorkManager.Instance.Send<Concert>(CMD.MUSICGAMEC_CONCERT, null, GetUserMusicGameContent);
    }

    private void GotoChooseCard(UserMusicGameVO vo)
    {
        GlobalData.TrainingRoomModel.CurMusicGame = vo;
        SendMessage(new Message(MessageConst.MODULE_TRAININGROOM_GOTO_CHOOSECARD_PANEL));
    }


    public override void OnMessage(Message message)
    {
        var name = message.Name;
        var body = message.Params;
        switch (name)
        {
            case MessageConst.CMD_TRAININGROOM_CREATE_CHOOSE_CARD:
                CreateChooseCards();
                break;
            case MessageConst.CMD_TRAININGROOM_ONCLICK_UNFOLD_BTN:
                var activityId = Convert.ToInt32(message.Body);
                View.SetChildrenUnfold(activityId);
                break;
            case MessageConst.MODULE_TRAININGROOM_ENTRY_GAME:

                _diffType = (int) body[0];

                if (_diffType < 0)
                {
                    EnterPractice();
                    return;
                }

                var vo = GlobalData.TrainingRoomModel.CurMusicGame;
                _activityId = vo.ActivityId;
                var req = new PlayingMusicReq
                {
                    ActivityId = vo.ActivityId,
                    DiffType = _diffType,
                    MusicId = GlobalData.TrainingRoomModel.GetTodayMusicInfo().MusicId
                };

                foreach (var t in GlobalData.TrainingRoomModel.ChooseCards)
                    req.CardIds.Add(t.UserCardVo.CardId);

                GetService<StartMusicGameService>().Request(req).SetCallback(OnStartPlayMusic).Execute();
                break;
        }
    }

    private void EnterPractice()
    {
        var music = GlobalData.TrainingRoomModel.GetTodayMusicInfo();
        // var music = GlobalData.TrainingRoomModel.GetMusicInfoPbById(2003);
        var vo = new ActivityMusicVo(null)
        {
            MusicId = music.MusicId,
            ActivityId = _activityId,
            MusicName = music.MusicName,
            Diff = (MusicGameDiffTypePB) (-_diffType-1),
            GameScoreRule =
                GlobalData.TrainingRoomModel.GetMusicGameScorePB(music.MusicId, (MusicGameDiffTypePB) (-_diffType-1)),
            IsOpen = true,
            MusicGameType = MusicGameType.TrainingRoomPractice
        };
        ModuleManager.Instance.EnterModule(ModuleConfig.MODULE_MUSICRHYTHM, true, false, vo);
    }

    private void OnStartPlayMusic(PlayingMusicRes obj)
    {
        var music = GlobalData.TrainingRoomModel.GetTodayMusicInfo();
        // var music = GlobalData.TrainingRoomModel.GetMusicInfoPbById(5002);
        var vo = new ActivityMusicVo(null)
        {
            MusicId = music.MusicId,
            ActivityId = _activityId,
            MusicName = music.MusicName,
            Diff = (MusicGameDiffTypePB) _diffType,
            GameScoreRule =
                GlobalData.TrainingRoomModel.GetMusicGameScorePB(music.MusicId, (MusicGameDiffTypePB) _diffType),
            IsOpen = true,
            MusicGameType = MusicGameType.TrainingRoom
        };
        ModuleManager.Instance.EnterModule(ModuleConfig.MODULE_MUSICRHYTHM, true, false, vo);
    }

    private void CreateChooseCards()
    {
        GlobalData.TrainingRoomModel.UpdateMusicActivityDates();
        View.CreateChooseCards(GlobalData.TrainingRoomModel.CurMusicGame.ActivityId,
            GlobalData.TrainingRoomModel.ChooseCards);
    }


    public override void Destroy()
    {
        EventDispatcher.RemoveEvent(EventConst.ChangeAbility);
        EventDispatcher.RemoveEvent(EventConst.GotoChooseCard);
        // EventDispatcher.RemoveEvent(EventConst.StartPlay);
    }

    private void GetUserMusicGameContent(Concert res)
    {
        GlobalData.TrainingRoomModel.InitUserMusicGameContent(res);
        View.SetData(GlobalData.TrainingRoomModel.GetMusicActivityDates());
    }


    //打开刷新弹窗
    private void OpenRefreshWindow(int activityId)
    {
        var curRefreshNum = GlobalData.TrainingRoomModel.GetRefreshMusicNum();
        
        // if (curRefreshNum <= 0)
        // {
        //     FlowText.ShowMessage("刷新次数不足");
        //     return;
        // }

        RefreshDataPB pb = GlobalData.TrainingRoomModel.GetCurMusicGameRefreshRules(curRefreshNum);
        var curCostGemNum = pb.ResourceNum;
        var playerGemNum = GlobalData.PlayerModel.PlayerVo.Gem;
        var content = "是否消耗" + curCostGemNum + "星钻刷新所有商品?";
        
        PopupManager.ShowConfirmWindow(content).WindowActionCallback = evt =>
        {
            if (evt == WindowEvent.Ok)
            {
                if (curCostGemNum > playerGemNum)
                {
                    FlowText.ShowMessage("星钻不足");
                    return;
                }

                SendChangeAbilityReq(activityId);
            }
        };
    }

    private void SendChangeAbilityReq(int activityId)
    {
        LoadingOverlay.Instance.Show();
        var req = new RefreshMusicGameReq {ActivityId = activityId};
        var data = NetWorkManager.GetByteData(req);
        NetWorkManager.Instance.Send<RefreshMusicRes>(CMD.MUSICGAMEC_REFRECHMUSIC, data, GetChangeAbilityRes,
            GetChangeAbilityResFailed);
    }

    private void GetChangeAbilityRes(RefreshMusicRes res)
    {
        LoadingOverlay.Instance.Hide();
        GlobalData.TrainingRoomModel.UpdateRefreshMusicNum(res.RefreshMusicCount);
        GlobalData.PlayerModel.UpdateUserMoney(res.Money);
        var pb = res.UserMusicGame[res.UserMusicGame.Count - 1];
        var vo = new UserMusicGameVO(pb);
        GlobalData.TrainingRoomModel.UpdateMusicActivityDates(vo);
        View.ChangeAbility(vo);
    }

    private void GetChangeAbilityResFailed(HttpErrorVo vo)
    {
        SendMessage(new Message(MessageConst.MODULE_TRAININGROOM_GET_RES_FAILED));
    }
}