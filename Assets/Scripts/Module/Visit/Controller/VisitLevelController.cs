using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Core.Events;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module;
using Assets.Scripts.Module.Download;
using Assets.Scripts.Module.NetWork;
using Com.Proto;
using Common;
using Componets;
using DataModel;
using Framework.Utils;
using game.main;
using Google.Protobuf.Collections;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public class VisitLevelController : Controller
{
    public VisitLevelView VisitLevelView { get; set; }
    VisitVo _curVisitVo;
    List<VisitChapterVo> _visitChapterVoList;
    VisitChapterVo _curVisitChapterVo;
    public override void Init()
    {
        EventDispatcher.AddEventListener<int, int>(EventConst.EnterVisitBattle, OnEnterBattle);
        EventDispatcher.AddEventListener<int>(EventConst.VisitLevelItemClick, OnShowBattleIntroduction);
        EventDispatcher.AddEventListener<VisitLevelVo>(EventConst.VisitLevelItemExtraClick, OnShowLevelFirstPassItemExtra);
        EventDispatcher.AddEventListener(EventConst.VisitLevelItemGotoWeather, OnGotoWeather);
        EventDispatcher.AddEventListener<int>(EventConst.VisitLevelResetLevelTime, ResetLevelTime);
        EventDispatcher.AddEventListener(EventConst.VisitFirsetLevelItem, SelectFirstLevelItem);
    }

    //重置关卡次数
    void ResetLevelTime(int levelId)
    {
        Debug.LogError(levelId);
        VisitingBuyCountReq req = new VisitingBuyCountReq();
        
        req.LevelId = levelId;
        byte[] data = NetWorkManager.GetByteData(req);
        NetWorkManager.Instance.Send<VisitingBuyCountRes>(CMD.VISITINGC_RESETLEVEL, data, OnResetLevelTimeHandler);

    }

    private void OnResetLevelTimeHandler(VisitingBuyCountRes res)
    {
        Debug.LogError("OnResetLevelTimeHandler");

        GlobalData.PlayerModel.UpdateUserMoney(res.UserMoney);
        VisitLevelVo vo = _curVisitChapterVo.GetVisitLevelVoById(res.UserLevel.LevelId);
        //更新UI
        vo.MyVisitLevel.UpdateVisitLevelVo(res.UserLevel);
        //VisitLevelView.UpdateResetLevelTime();

        if (_battleIntroductionPopup != null) 
        {
          //  _battleIntroductionPopup.Init(vo, _curVisitVo);
            _battleIntroductionPopup.Refresh();
        }

    }

    public override void Start()
    {
        base.Start();
    }

    public override void OnMessage(Message message)
    {
        string name = message.Name;
        object[] body = message.Params;
        switch (name)
        {
            case MessageConst.CMD_VISIT_LEVEL_FIRSTAWARDGET_CLICK:
                GetVisitingFirstPassAwardsReq req = new GetVisitingFirstPassAwardsReq();
                req.LevelId = (int)body[0];
                byte[] data = NetWorkManager.GetByteData(req);
                NetWorkManager.Instance.Send<GetVisitingFirstPassAwardsRes>(CMD.VISITINGC_GETFIRSTPASSAWARDS, data, OnGetFirstPassAwardHandler);
                break;
            default:
                break;
        }
    }

    private List<MapPos> LoadJson(string mapId)
    {
        string text = new AssetLoader().LoadTextSync(AssetLoader.GetVisitLevelMapDataPath(mapId));
        List<MapPos> mapPos = JsonConvert.DeserializeObject<List<MapPos>>(text);
        return mapPos;
    }



    private void OnGetFirstPassAwardHandler(GetVisitingFirstPassAwardsRes res)
    {
        Debug.Log("OnGetFirstPassAwardHandler");
        if(visitFirstPassAwardWindow!=null)
        {
            visitFirstPassAwardWindow.Close();
            visitFirstPassAwardWindow = null;
        }

        VisitLevelVo vo = _curVisitChapterVo.GetVisitLevelVoById(res.UserLevel.LevelId);
        vo.MyVisitLevel.UpdateVisitLevelVo(res.UserLevel);
        VisitLevelView.UpdateFirstPassAward();
        RewardUtil.AddReward(res.Awards);
    }

    private void OnEnterBattle(int num, int levelid)
    {
        Debug.Log("OnEnterBattle--------num:"+num);

        VisitLevelVo vo = _curVisitChapterVo.GetVisitLevelVoById(levelid);
        if (vo.LevelType == LevelTypePB.Story)
        {
            ModuleManager.Instance.EnterModule(ModuleConfig.MODULE_STORY, false, false, vo);
        }
        else
        {
            if(num==0)
            {
                ModuleManager.Instance.EnterModule(ModuleConfig.MODULE_VISITBATTLE, false, true, vo, GetData<VisitModel>().CommentRule, GetData<VisitModel>().ChallengeCardNumRule);
            }
            else
            {
                byte[] data = NetWorkManager.GetByteData(new VisitingSweepReq()
                {
                    LevelId = levelid,
                    Num = num
                });
                LoadingOverlay.Instance.Show();
                NetWorkManager.Instance.Send<VisitingSweepRes>(CMD.VISITINGC_SWEEPRES, data, OnSweepHandler);                
            }
        }
    }

    private void OnSweepHandler(VisitingSweepRes res)
    {
        LoadingOverlay.Instance.Hide();
        VisitModel _model = GetData<VisitModel>();

        EventDispatcher.RemoveEvent(EventConst.ShowLastVisitBattleWindow);
        EventDispatcher.AddEventListener<int>(EventConst.ShowLastVisitBattleWindow, OnSweepEnd);

        _model.UpdateMyLevel(res.UserLevel);
        int levelId = res.UserLevel.LevelId;
        RepeatedField<GameResultPB> result = res.GameResult;

        VisitBattleSweepWindow win = PopupManager.ShowWindow<VisitBattleSweepWindow>("Visit/Prefabs/VisitBattleSweepWindow");
        win.SetData(res.GameResult, _model.GetVisitLevelVoById(levelId, _model.GetMyLevelByLevelId(levelId).NpcId), result[0].Exp);
        win.MaskColor = new Color(0, 0, 0, 0.5f);

        for (int i = 0; i < result.Count; i++)
        {
            GameResultPB pb = result[i];
            GlobalData.PlayerModel.AddExp(pb.Exp);
        }

        GetData<VisitModel>().UpdateMyWeather(res.UserWeather);
        GlobalData.PlayerModel.UpdateUserPower(res.UserPower);

        VisitLevelView.SetLeftTime();
    }

    private void OnSweepEnd(int levelId)
    {
        OnShowBattleIntroduction(levelId);
    }

    private void SelectFirstLevelItem()
    {
        VisitLevelVo vo = _visitChapterVoList[0].LevelList[0];
        if (!vo.IsCanPass)
        {
            return;
        }
        Debug.Log("SelectFirstLevelItem");
        EventDispatcher.TriggerEvent<int>(EventConst.VisitLevelItemClick, vo.LevelId);
    }


    private void OnGotoWeather()
    {
        SendMessage(new Message(MessageConst.MODULE_VISIT_SHOW_WEATHER_PANEL, Message.MessageReciverType.DEFAULT, _curVisitVo.NpcId));
    }

    VisitFirstPassAwardWindow visitFirstPassAwardWindow = null;
    private void OnShowLevelFirstPassItemExtra(VisitLevelVo vo)
    {
        visitFirstPassAwardWindow = PopupManager.ShowWindow<VisitFirstPassAwardWindow>("Visit/Prefabs/VisitFirstPassAwardWindow", Container);
        visitFirstPassAwardWindow.Init(vo);
    }

    VisitBattleIntroductionPopup _battleIntroductionPopup;
    private void OnShowBattleIntroduction(int arg0)
    {
        Debug.Log("OnShowBattleIntroduction:" + arg0);
        VisitLevelVo vo = _curVisitChapterVo.GetVisitLevelVoById(arg0);

        //检测星缘解锁条件
        if (vo.LevelExtra.CardId != 0 && GlobalData.CardModel.GetUserCardById(vo.LevelExtra.CardId) == null)  //星缘
        {
            VisitLevelPreUnlockCardWindow window = PopupManager.ShowWindow<VisitLevelPreUnlockCardWindow>("Visit/Prefabs/VisitLevelPreUnlockCardWindow", Container);
            window.Init(vo);
            return;
        }
        //检测好感度等级
        if(vo.LevelExtra.Favorability != 0)
        {
            UserFavorabilityVo favorabilityVo = GlobalData.FavorabilityMainModel.GetUserFavorabilityVo((int)vo.NpcId);
            if(favorabilityVo != null && favorabilityVo.Level < vo.LevelExtra.Favorability)
            {
                ConfirmWindow win = PopupManager.ShowConfirmWindow(I18NManager.Get("Visit_Favorability_NoEnough_Tips", vo.LevelExtra.Favorability), null, I18NManager.Get("Common_Goto"));
                win.WindowActionCallback = evt =>
                {
                    if (evt == WindowEvent.Ok)
                    {
                        //var isOpen = GuideManager.IsOpen(ModulePB.Favorability, FunctionIDPB.FavorabilityView);
                        //if (!isOpen)
                        //{
                        //    var msg = GuideManager.GetOpenConditionDesc(ModulePB.Favorability, FunctionIDPB.FavorabilityView);
                        //    FlowText.ShowMessage(msg);
                        //    return;
                        //}
                        
                        ModuleManager.Instance.EnterModule(ModuleConfig.MODULE_FAVORABILITYMAIN, false, false, "SendGift", vo.NpcId);
                    }
                };
                return;
            }
        }

        if (vo.LevelType == LevelTypePB.Story)
        {
            int npcId = (int)_curVisitVo.NpcId;
            bool isShowLoaddown = CacheManager.IsVisitStoryItemLoaddown(npcId);
            if(!isShowLoaddown)
            {
                ModuleManager.Instance.EnterModule(ModuleConfig.MODULE_STORY, false, false, vo);
    
            }
            else
            {
                CacheManager.ClickVisitStoryItem(npcId,()=> {
                    ModuleManager.Instance.EnterModule(ModuleConfig.MODULE_STORY, false, false, vo);
                },()=> {
                    ModuleManager.Instance.EnterModule(ModuleConfig.MODULE_STORY, false, false, vo);
                });
            }
            return;

        }
 
        if (!GlobalData.CardModel.ContainsCardByNpcId(vo.NpcId))
        {
            string name = Util.GetPlayerName(vo.NpcId);
            PopupManager.ShowAlertWindow( I18NManager.Get("Visit_LevelNoConditionCard", name));
            return;
        }


        _battleIntroductionPopup = PopupManager.ShowWindow<VisitBattleIntroductionPopup>("Visit/Prefabs/VisitBattleIntroductionPopup");
 
        _battleIntroductionPopup.Init(vo, _curVisitVo);
        _battleIntroductionPopup.MaskColor = new Color(0, 0, 0, 0.5f);
   //     ClientData.CustomerSelectedLevel = level;
    }

    public void Refresh()
    {
        NetWorkManager.Instance.Send<MyVisitingRes>(CMD.VISITINGC_MYVISITINGS, null, OnMyVisitingHandler);
    }
    private void OnMyVisitingHandler(MyVisitingRes res)
    {
        GetData<VisitModel>().InitMyData(res);
        //_visitModel.Init();
        SetData(_npcId);
    }

    public override void Destroy()
    {
        base.Destroy();
        EventDispatcher.RemoveEvent(EventConst.EnterVisitBattle);
        EventDispatcher.RemoveEvent(EventConst.VisitLevelItemClick);
        EventDispatcher.RemoveEvent(EventConst.ShowLastVisitBattleWindow);
        EventDispatcher.RemoveEvent(EventConst.VisitLevelItemExtraClick);
        EventDispatcher.RemoveEvent(EventConst.VisitLevelItemGotoWeather);
        EventDispatcher.RemoveEvent(EventConst.VisitLevelResetLevelTime);
        EventDispatcher.RemoveEvent(EventConst.VisitFirsetLevelItem);
    }
    PlayerPB _npcId;
    public static PlayerPB curNpcId;

    public void SetData(PlayerPB npcId)
    {
        _npcId = npcId;
        curNpcId = npcId;
        Debug.Log("VisitLevelController SetData NpcId is " + npcId);
        _curVisitVo = GetData<VisitModel>().GetVisitVo(npcId);
        _visitChapterVoList = GetData<VisitModel>().GetVisitChapterVo(npcId);
        _curVisitChapterVo = _visitChapterVoList[0];

        string mapId =_curVisitChapterVo.LevelList[0].ChapterBackdrop;
        //todo 坐标加载
        List<MapPos> mapPos = LoadJson(mapId);
        VisitLevelView.SetData(_curVisitVo, _visitChapterVoList[0], mapPos);
        

        //通关第一关时弹出天气指引
        if (_visitChapterVoList[0].LevelList.Count > 0)
        {
            VisitLevelVo vo = _visitChapterVoList[0].LevelList[0];

            if (vo.IsPass)
            {
                Common.GuideManager.OpenGuide(Common.GuideEnumType.VISIT_BLESS);
            }
        }
    }
}
