using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Core.Events;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module;
using Assets.Scripts.Module.MainLine.Services;
using Assets.Scripts.Module.NetWork;
using Assets.Scripts.Services;
using Com.Proto;
using Common;
using Componets;
using DataModel;
using game.main;
using Google.Protobuf.Collections;
using UnityEngine;

public class MainLineController : Controller
{
	public MainLineView View;
	
	private LevelModel _model;
	private LevelVo _lastPlayLevel;
	private BattleIntroductionPopup _battleIntroductionPopup;

	public override void Init()
	{
		EventDispatcher.AddEventListener<int, LevelVo>(EventConst.EnterBattle, OnEnterBattle);
		EventDispatcher.AddEventListener<LevelVo>(EventConst.BuyLevelCount, BuyLevelCount);
	}

	public override void Start()
	{
		_model = GetData<LevelModel>();
		GetLevelData();
	}

	public void ResetData()
	{
		GetService<LevelService>().SetCallback(OnGetMyLevelData).Execute();
	}
	
	private void GetLevelData()
	{
		LoadingOverlay.Instance.Show();
		
		GetService<LevelService>().SetCallback(OnGetMyLevelData).Execute();
	}

	private void OnGetMyLevelData(LevelModel model)
	{
		LoadingOverlay.Instance.Hide();
        
		View.EnterChapter(model);

		SendMessage(new Message(MessageConst.TO_GUIDE_MAINLINE_GETDATA));
		
		if (model.DoJump)
		{
			ClientTimer.Instance.DelayCall(
				() =>
				{
					SendMessage(new Message(MessageConst.MODULE_MAINLINE_SHOW_BATTLE_VIEW,
						Message.MessageReciverType.DEFAULT, model.ActiveLevel));
				}, 0.5f);
		}
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
	        case MessageConst.MODULE_MAINLINE_SHOW_STORY_VIEW: //显示剧情模块
		        LevelVo levelInfo1 = (LevelVo) body[0];
		        ModuleManager.Instance.EnterModule(ModuleConfig.MODULE_STORY, false, false, levelInfo1);
		        ClientData.CustomerSelectedLevel = levelInfo1;
		        NoCoerceHideGudieView();
		        break;
	        case MessageConst.MODULE_MAINLINE_SHOW_BATTLE_VIEW: //显示战斗介绍窗口
		        LevelVo level = (LevelVo) body[0];
		        ShowBattleWindow(level);
		        break;
	        case MessageConst.MODULE_MAIN_NEXTCHAPTER:
		        View.ShowNextChapter();
		        break;
        }
    }


	/// <summary>
	/// 非强制状态下隐藏MainLineGudieView
	/// </summary>
	private void NoCoerceHideGudieView()
	{
	   	SendMessage(new Message(MessageConst.TO_GUIDE_HIDE_MAINLIENGUIDEVIEW));
	}
	
	private void ShowBattleWindow(LevelVo level)
    {
//	    LevelVo level1_6 = _model.FindLevel("1-6");
//	    if (level.IsPass && level1_6.IsPass == false)
//	    {
//		    FlowText.ShowMessage(I18NManager.Get("Guide_Battle6", level1_6.LevelMark));
//		    return;
//	    }
	   
	    if (level.IsPass &&  !GuideManager.IsPass1_9())
	    {
		    FlowText.ShowMessage(I18NManager.Get("Guide_Battle6", "1-9"));
            return;  
	    }


	    _battleIntroductionPopup = PopupManager.ShowWindow<BattleIntroductionPopup>("MainLine/Prefabs/BattleIntroductionPopup");
	    _battleIntroductionPopup.Init(level, _model.LevelBuyRules);
	    _battleIntroductionPopup.MaskColor = new Color(0,0,0,0.5f);
        ClientData.CustomerSelectedLevel = level;
    }

    private void OnEnterBattle(int num, LevelVo level)
    {
        _lastPlayLevel = level;
        if (num == 0)
        {
            Main.ChangeMenu(MainMenuDisplayState.HideAll);
            ModuleManager.Instance.EnterModule(ModuleConfig.MODULE_BATTLE, false, true, level,
                GetData<LevelModel>().CommentRule, GetData<LevelModel>().CardNumRules);
        }
        else
        {
            byte[] data = NetWorkManager.GetByteData(new SweepReq()
            {
                LevelId = level.LevelId,
                Num = num
            });
            LoadingOverlay.Instance.Show();
            NetWorkManager.Instance.Send<SweepRes>(CMD.CAREERC_SWEEPRES, data, OnSweep);
        }
    }

    private void OnSweep(SweepRes res)
    {
        _model.UpdateMyLevel(res.UserLevel);
        
        LoadingOverlay.Instance.Hide();

        EventDispatcher.RemoveEvent(EventConst.ShowLastBattleWindow);
        EventDispatcher.AddEventListener(EventConst.ShowLastBattleWindow, OnSweepEnd);

        RepeatedField<GameResultPB> result = res.GameResult;
        BattleSweepWindow win = PopupManager.ShowWindow<BattleSweepWindow>("MainLine/Prefabs/BattleSweepWindow");
        win.SetData(res.GameResult, ClientData.CustomerSelectedLevel, result[0].Exp);
	    win.MaskColor = new Color(0,0,0,0.5f);

        if (_model.JumpData != null && GlobalData.PropModel.GetUserProp(_model.JumpData.RequireId).Num >=
            _model.JumpData.RequireNum)
        {
            FlowText.ShowMessage(I18NManager.Get("MainLine_Hint1"));
        }

        for (int i = 0; i < result.Count; i++)
        {
            GameResultPB pb = result[i];
            GlobalData.PlayerModel.AddExp(pb.Exp);
        }
	    
	    GlobalData.PlayerModel.UpdateUserPower(res.UserPower);

	    GlobalData.RandomEventModel.AddEnergy(ClientData.CustomerSelectedLevel.CostEnergy * result.Count);
	    
	    if (GlobalData.RandomEventModel.CheckTrigger(7002, 7003))
		    new TriggerService().ShowNewGiftWindow().Execute();
    }

    private void OnSweepEnd()
    {
        if (_lastPlayLevel != null)
            ShowBattleWindow(_lastPlayLevel);
    }
	
	private void BuyLevelCount(LevelVo levelVo)
	{
		LoadingOverlay.Instance.Show();
		BuyCountReq req = new BuyCountReq
		{
			LevelId = levelVo.LevelId
		};
		byte[] data = NetWorkManager.GetByteData(req);
		NetWorkManager.Instance.Send<BuyCountRes>(CMD.CAREERC_BUY_COUNT, data, res =>
		{
			LoadingOverlay.Instance.Hide();
			GlobalData.PlayerModel.UpdateUserMoney(res.UserMoney);
			_model.UpdateMyLevel(res.UserLevel);
			_battleIntroductionPopup.Refresh();			
            FlowText.ShowMessage(I18NManager.Get("MainLine_Hint2"));

        });
	}

	public override void Destroy()
	{
		base.Destroy();
		EventDispatcher.RemoveEvent(EventConst.EnterBattle);
		EventDispatcher.RemoveEvent(EventConst.ShowLastBattleWindow);
		EventDispatcher.RemoveEvent(EventConst.BuyLevelCount);
	}
}
