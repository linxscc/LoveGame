using System;
using System.Collections.Generic;
using System.Linq;
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
using Framework.GalaSports.Service;
using game.main;
using Module.FavorabilityMain.View;
using Newtonsoft.Json;
using UnityEngine;

public class CardDetailController : Controller
{

    public CardDetailView view;
    private CardModel _cardModel;
    //private readonly Dictionary<int,int> _itemdic=new Dictionary<int, int>();
    private GainPropWindow _gainPropWindow;
    private UserCardVo _preCardVo;
    private EvolutionWindow _evolutionWindow;
    private StarUpPreviewWindow _starUpPreviewWindow;
    private UpgradeStarRequireVo _upgradeStarRequireVo;

    public override void Start()
    {
        _cardModel = GlobalData.CardModel;
        if (_cardModel == null)
        {
            Debug.LogError("_cardModel is null" + _cardModel);
        }
        ClientData.LoadJumpData(null);
        ClientData.LoadPhoneUnlockData(null);
        EventDispatcher.AddEventListener<UserCardVo, int>(EventConst.CardEvoConfirm, Evolution);
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
            case MessageConst.CMD_CARD_UPGRADE_STAR:
                UpgradeStar((UserCardVo)message.Body);
                break;
            case MessageConst.CMD_CARD_EVOLUTION:
                OpenEvoWindow((UserCardVo)body[0]);
                break;
            case MessageConst.CMD_CARD_LOVE:
                //现在就要跳转到恋爱模块

                ModuleManager.Instance.EnterModule(ModuleConfig.MODULE_LOVEAPPOINTMENT, false, true, (AppointmentRuleVo)body[0]);

                break;
            case MessageConst.CMD_CARD_UPGRADE_LEVEL:
                UserCardVo vo;
                vo = (UserCardVo)body[0];
                if (body.Length > 1 && (int)body[2] <= 0)
                {
                    FlowText.ShowMessage(I18NManager.Get("LoveAppointment_Hint5"));
                    DoGiftTrigger(vo.CardVo.Player);
                    return;
                }
                AddExp(vo, (int)body[1], (int)body[2]);
                break;
            case MessageConst.CMD_CARD_UPGRADE_ONELEVEL:
                UpgradeOneLevel((UserCardVo)body[0]);
                break;
            case MessageConst.CMD_CARD_GET_MORE_PROPS:
                ShowObtainWindow((UpgradeStarRequireVo)body[0], (int)body[1]);
                break;
            case MessageConst.CMD_CARD_CHOOSE_EVO:
                ShowChooseEvo((int[])body[0]);
                break;
            case MessageConst.CMD_CARD_SHOPSTARUPPREVIEW:
                //ShowStarUpPreview((UserCardVo)body[0]);

                ShowCardAwardPreview((UserCardVo)body[0]);
                break;

        }
    }

    private string GetSceneName(int sceneId)
    {
        string SceneName = "";
        string text = new AssetLoader().LoadTextSync(AssetLoader.GetPhoneDataPath(sceneId.ToString()));
        if (text == "")
        {
            return SceneName;
        }
        if (sceneId < 10000)
        {
            SceneName = JsonConvert.DeserializeObject<SmsInfo>(text).smsSceneInfo.SceneName;
        }
        else if (sceneId < 20000)
        {
            SceneName = JsonConvert.DeserializeObject<SmsInfo>(text).smsSceneInfo.SceneName;
        }
        else if (sceneId < 30000)
        {
            SceneName = JsonConvert.DeserializeObject<FriendCircleInfo>(text).friendCircleSceneInfo.SceneName;
        }
        else if (sceneId < 40000)
        {
            SceneName = JsonConvert.DeserializeObject<WeiboInfo>(text).weiboSceneInfo.SceneName;
        }
        return SceneName;
    }

    private CardAwardPreType GetCardAwardPreType(int sceneId)
    {
        if (sceneId < 10000)
        {
            return CardAwardPreType.Sms;
        }
        else if (sceneId < 20000)
        {
            return CardAwardPreType.Call;
        }
        else if (sceneId < 30000)
        {
            return CardAwardPreType.FriendCirlce;
        }
        else
        {
            return CardAwardPreType.Weibo;
        }
    }

    private void ShowCardAwardPreview(UserCardVo userCardVo)
    {
        List<CardAwardPreInfo> infos = new List<CardAwardPreInfo>();
        var vo = userCardVo;
        int cardId = userCardVo.CardId;
        int NpcId = (int)userCardVo.CardVo.Player;

        var userFavorabilityVo = GlobalData.FavorabilityMainModel.GetUserFavorabilityVo(NpcId);//用户NPC的好感度信息
        Debug.LogError("CardId " + cardId + "  NpcId " + NpcId);

        PlayerPB playerPB = userCardVo.CardVo.Player;

        var cardPd = GlobalData.CardModel.GetCardBase(cardId);

        for (int i = 0; i < cardPd.GainSceneIds.Count; i++)
        {
            int phonesceneId = cardPd.GainSceneIds[i];
            if(phonesceneId==0)
            {
                continue;
            }
            bool isUnlock = true;

            var userCard = GlobalData.CardModel.GetUserCardById(cardId);
            isUnlock = userCard!= null;

         //   Debug.LogError("phonesceneId" + phonesceneId+ "GetSceneName"+ GetSceneName(phonesceneId));

            CardAwardPreInfo info = CreateCardAwardPreInfo(GetSceneName(phonesceneId), isUnlock, GetCardAwardPreType(phonesceneId));
          
            info.StartTips =  "1心解锁.";
//            Debug.LogError(isUnlock+" "+info.content);
            infos.Add(info);
        }



        int sceneId = 0;
        //解锁手机事件
        for (int i = 0; i < vo.MaxStars; i++)
        {
            var targetStarUpPb = GlobalData.CardModel.GetCardStarUpRule(vo.CardId, (StarPB)i);
            if (targetStarUpPb == null)
            {
                //Debug.LogError("no i" + i);
                continue;
            }
            for (int j = 0; j < targetStarUpPb.SceneIds.Count; j++)
            {
                if (targetStarUpPb.SceneIds[j] == 0) continue;
                sceneId = targetStarUpPb.SceneIds[j];
                bool isUnlock = false;
                SceneUnlockRulePB curscenePb = GlobalData.FavorabilityMainModel.GetUnlockRulePb(sceneId);
                if (curscenePb == null)
                    continue;
                if (curscenePb.FavorabilityLevel <= userFavorabilityVo.Level && (int)targetStarUpPb.Star <= vo.Star)
                {
                    isUnlock = true;
                }
                if (curscenePb != null)
                {
                    //SmsInfo info = JsonConvert.DeserializeObject<SmsInfo>(text);
                    CardAwardPreInfo info = CreateCardAwardPreInfo(GetSceneName(sceneId), isUnlock, GetCardAwardPreType(sceneId));
                    PhoneUnlockInfo phoneUnlock = ClientData.GetPhoneUnlockInfoById(sceneId);
                    if (phoneUnlock!=null)
                    {
                        info.UnlockDescription = phoneUnlock.UnlcokDes;
                        info.StartTips = phoneUnlock.StartTips+".";
                        Debug.LogError(info.content);
                        infos.Add(info); 
                    }
                    
                    
                }
                Debug.Log("Star  level " + i + "  sceneID  " + sceneId + "  isUnlock " + isUnlock);
            }                                                                                              
        }


        //恋爱剧情
        UserAppointmentVo userAppointmentVo = GlobalData.AppointmentData.GetCardAppointmentVo(cardId);
        if (userAppointmentVo != null)
        {
            int appointmentId = userAppointmentVo.AppointmentId;
            AppointmentRuleVo appointmentRuleVo = GlobalData.AppointmentData.GetAppointmentRule(appointmentId);

            string cotent = appointmentRuleVo.Name;

            foreach (var v in appointmentRuleVo.GateInfos)
            {
                //cotent = cotent + v.Gate.ToString();

                bool isUnlock = userAppointmentVo.ActiveGateInfos.Contains(v.Gate);
                if (v.Star == 0 && v.Evo == 0)
                {
                    isUnlock = true;
                }


                string key = "Common_Number" + v.Gate.ToString();
                var info = CreateCardAwardPreInfo(cotent + I18NManager.Get(key), isUnlock, CardAwardPreType.LoveStory);

                if (v.Star == 0&&v.Evo==0)
                {
                    info.StartTips =  "1心解锁.";
                }
                
                
                if (v.Star > 0)
                {
                    info.StartTips = (v.Star + 1) + "心解锁.";
                    info.UnlockDescription = I18NManager.Get("Card_PreviewStarUnlock", (v.Star + 1));

                }

                if (v.Evo > 0)
                {
                    info.StartTips = "进化"+v.Evo + "解锁.";
                    info.UnlockDescription = I18NManager.Get("Card_PreviewEvoUnlock", v.Evo);
                }

                infos.Add(info);
            }
        }

        //日记语音
        List<ElementPB> elementPBs = GlobalData.DiaryElementModel.GetDialogsByCardId(cardId);
        foreach (var elementPB in elementPBs)
        {
            if (elementPB.ElementType == ElementTypePB.Sound)
            {
                string cotent = elementPB.Name;
                bool isUnlock = GlobalData.DiaryElementModel.IsCanUseElement(elementPB.Id);
                var info = CreateCardAwardPreInfo(cotent, isUnlock, CardAwardPreType.LoveDiaryLabelVoice);
                info.StartTips = "进化1解锁.";
                info.UnlockDescription = I18NManager.Get("Card_PreviewEvoUnlock", 1);
                infos.Add(info);
            }
        }

//        var drawcards = ClientData.GetDrawCardExpressionInfos(NpcId, EXPRESSIONTRIGERTYPE.DRAWCARD);
//        //抽卡语音   
//        foreach (var v in drawcards)
//        {
//            string cotent = v.DialogName;
//            bool isUnlock = true;
//            infos.Add(CreateCardAwardPreInfo(cotent, isUnlock, CardAwardPreType.DrawCardVioce));
//        }

        infos.Sort();

        string cardName = userCardVo.CardVo.CardName;
        var win = PopupManager.ShowWindow<CardAwardPreviewWindow>("Card/Prefabs/CardDetail/CardAwardPreviewWindow");
        win.SetData(cardName, infos);

        //string path = "FavorabilityMain/Prefabs/VoiceWindow";
        //var voiceWin = PopupManager.ShowWindow<VoiceWindow>(path);
        //voiceWin.SetData("111", infos);
    }


    private CardAwardPreInfo CreateCardAwardPreInfo(string content, bool isUnlock, CardAwardPreType cardAwardPreType, int priority = 0)
    {
        var info = new CardAwardPreInfo();
        info.content = content;
        info.isUnlock = isUnlock;
        info.priority = priority;
        info.cardAwardPreType = cardAwardPreType;
        return info;
    }



    private void ShowStarUpPreview(UserCardVo userCardVo)
    {
        if (_starUpPreviewWindow == null)
        {
            _starUpPreviewWindow = PopupManager.ShowWindow<StarUpPreviewWindow>("Card/Prefabs/CardDetail/StarUpPreviewWindow");
        }

        if (GlobalData.LevelModel == null)
        {
            LoadingOverlay.Instance.Show();
            GetService<LevelService>().SetCallback(levelmodel =>
            {
                Debug.LogError("no globalData");
                LoadingOverlay.Instance.Hide();
                _starUpPreviewWindow.SetData(userCardVo, levelmodel);
            }).Execute();
        }
        else
        {
            _starUpPreviewWindow.SetData(userCardVo, GlobalData.LevelModel);
        }


    }

    private void OpenEvoWindow(UserCardVo vo)
    {
        if (_evolutionWindow == null)
        {
            _evolutionWindow = PopupManager.ShowWindow<EvolutionWindow>("Card/Prefabs/CardDetail/EvolutionWindow");
        }
        //		Debug.LogError(vo.CardId);
        _evolutionWindow.SetData(vo);
        _evolutionWindow.WindowActionCallback = evt =>
        {
            if (evt == WindowEvent.ClickOutsideToClose)
            {
                //这里要设置一下tooggle
                view.SetToggleShow(view.ToggleIndex);
            }

        };

    }


    //注意new_view_evo
    private void ShowChooseEvo(int[] get)
    {
        //		Debug.LogError((EvolutionPB)get[0]+" "+get[1]);
        var buffer = NetWorkManager.GetByteData(new ChooseEvoReq() { CardId = get[1], Evolution = (EvolutionPB)get[0] });
        LoadingOverlay.Instance.Show();
        NetWorkManager.Instance.Send<ChooseEvoRes>(CMD.CARDC_CHOOSEEVO, buffer, ChooseEvoSuc);
    }

    private void ChooseEvoSuc(ChooseEvoRes res)
    {
        //Debug.LogError("Success"+res.UserCard.UseEvo);
        //刷新卡的最新数据。
        LoadingOverlay.Instance.Hide();
        GlobalData.CardModel.UpdateUserCards(new[] { res.UserCard });
        view.SetCardBg(GlobalData.CardModel.GetUserCardById(res.UserCard.CardId));
        view.SetEvoEffect();
    }

    private void ShowObtainWindow(UpgradeStarRequireVo vo, int cardId)
    {
        _upgradeStarRequireVo = vo;
        if (_gainPropWindow == null)
        {
            _gainPropWindow = PopupManager.ShowWindow<GainPropWindow>("Card/Prefabs/CardDetail/GainPropWindow");
        }

        if (GlobalData.LevelModel == null)
        {
            LoadingOverlay.Instance.Show();
            GetService<LevelService>().SetCallback(levelmodel =>
            {
                Debug.LogError("no globalData");
                LoadingOverlay.Instance.Hide();
                _gainPropWindow.SetData(vo, _cardModel, levelmodel, cardId);
            }).Execute();
        }
        else
        {
            _gainPropWindow.SetData(vo, _cardModel, GlobalData.LevelModel, cardId);
        }

    }

    private void AddExp(UserCardVo vo, int itemid, int itemnum)
    {
        _preCardVo = vo;
        //		Debug.LogError(itemid+" "+itemnum);
        byte[] buffer = NetWorkManager.GetByteData(new AddExpReq()
        {
            CardId = vo.CardId,
            ItemId = itemid,
            ItemNum = itemnum
        });
        LoadingOverlay.Instance.Show();
        NetWorkManager.Instance.Send<AddExpRes>(CMD.CARDC_LEVELUP, buffer, OnLevelup);
    }

    private void UpgradeRep(UserCardVo vo)
    {
        if (vo == null)
        {
            return;
        }

        byte[] buffer = NetWorkManager.GetByteData(new UpgradeReq() { CardId = vo.CardId });
        LoadingOverlay.Instance.Show();
        NetWorkManager.Instance.Send<UpgradeRes>(CMD.CARDC_LEVELONE, buffer, OnLevelUpOne, onLevelUpOneError);
    }

    private void onLevelUpOneError(HttpErrorVo vo)
    {
        SendMessage(new Message(MessageConst.TO_GUIDE_CARD_LEVELUP_RESET));
    }

    private void OnLevelUpOne(UpgradeRes res)
    {
        LoadingOverlay.Instance.Hide();
        //		if (res.UserCard.Level>_preCardVo.Level)
        //		{
        //FlowText.ShowMessage("升级成功");	
        //		}
        GlobalData.PropModel.UpdateProps(res.UserItems);
        UpdateUserCard(res.UserCard, true);

        SendMessage(new Message(MessageConst.TO_GUIDE_CARD_LEVELUP));

        if (GlobalData.MissionModel.IsHaveStarActivityMission)
        {
            GlobalData.MissionModel.MissionAttainmentModel.TriggerPopWindow(MissionAttainmentModel.StarActivityPopUpsType.CardLevelUp, res.UserCard.Level);
        }

    }

    private void OnLevelup(AddExpRes res)
    {
        //		Debug.LogError(res.UserCard);
        LoadingOverlay.Instance.Hide();
        if (res.UserCard.Level > _preCardVo.Level)
        {
            //Debug.LogError("level up");
        }
        GlobalData.PropModel.UpdateProps(new[] { res.UserItem });
        UpdateUserCard(res.UserCard, true);
        if (GlobalData.MissionModel.IsHaveStarActivityMission)
        {
            GlobalData.MissionModel.MissionAttainmentModel.TriggerPopWindow(MissionAttainmentModel.StarActivityPopUpsType.CardLevelUp, res.UserCard.Level);
        }

    }

    private void UpgradeOneLevel(UserCardVo vo)
    {
        if (vo == null)
            return;

        //_itemdic.Clear();

        int needExp = vo.NeedExp - vo.CurrentLevelExp;
        int smallPropExp = GlobalData.PropModel.GetPropBase(PropConst.CardUpgradePropSmall).Exp;
        int bigPropExp = GlobalData.PropModel.GetPropBase(PropConst.CardUpgradePropBig).Exp;
        int largePropExp = GlobalData.PropModel.GetPropBase(PropConst.CardUpgradePropLarge).Exp;
        int smallProCurNum = GlobalData.PropModel.GetUserProp(PropConst.CardUpgradePropSmall).Num;
        int bigPropCurNum = GlobalData.PropModel.GetUserProp(PropConst.CardUpgradePropBig).Num;
        int largePropCurNum = GlobalData.PropModel.GetUserProp(PropConst.CardUpgradePropLarge).Num;

        if (smallProCurNum * smallPropExp + bigPropCurNum * bigPropExp + largePropCurNum * largePropExp < needExp ||
            smallProCurNum * smallPropExp + bigPropCurNum * bigPropExp + largePropCurNum * largePropExp == 0)
        {
            DoGiftTrigger(vo.CardVo.Player);
            GuideManager.Hide();
            //FlowText.ShowMessage(I18NManager.Get("Card_LevelUpOneFail"));
            PopupManager.ShowConfirmWindow(
                I18NManager.Get("Card_PropLacking"),
                null,
                I18NManager.Get("Card_GotoShop"),
                I18NManager.Get("Card_GotoMainLine")
                ).WindowActionCallback = evt =>
            {
                if (evt == WindowEvent.Ok)
                {
                    ModuleManager.Instance.EnterModule(ModuleConfig.MODULE_SHOP, false, true,3);      
                }
                if (evt == WindowEvent.Cancel)
                {
                    ModuleManager.Instance.EnterModule(ModuleConfig.MODULE_MAIN_LINE, false, true);
                }
            };
            return;
        }

        UpgradeRep(vo);
    }

    private void DoGiftTrigger(PlayerPB player)
    {
        for (int i = 7007; i <= 7009; i++)
        {
            if (GlobalData.RandomEventModel.CheckTrigger(i))
            {
                new TriggerService().Request(i).ShowNewGiftWindow().Execute();
                break;
            }
        }
    }

    private void UpdateUserCard(UserCardPB userCard, bool addexp = false, bool starupSuccess = false)
    {
        GlobalData.CardModel.UpdateUserCards(new[] { userCard });
        SendMessage(new Message(MessageConst.CMD_CARD_REFRESH_USER_CARDS));

        view.SetData(GlobalData.CardModel.GetUserCardById(userCard.CardId), GlobalData.PropModel, addexp, starupSuccess);
    }

    public void UpdatePropAfterBattle()
    {
        //		view.UpdatePropNum(GlobalData.PropModel);
        //		view.UpdateStarPropNum(GlobalData.CardModel.CurCardVo);
        view.SetData(GlobalData.CardModel.CurCardVo, GlobalData.PropModel);
        if (_gainPropWindow != null)
        {
            _gainPropWindow.SetData(_upgradeStarRequireVo, _cardModel, GlobalData.LevelModel, GlobalData.CardModel.CurCardVo.CardId);
        }

    }


    private void Evolution(UserCardVo vo, int chooseState)
    {
        //如果不满足满级满星，就不应该发送请求。
        //FlowText.ShowMessage("确认进化");
        Debug.LogError(chooseState);
        byte[] buffer = NetWorkManager.GetByteData(new EvolutionReq() { CardId = vo.CardId, ConsumeType = chooseState });
        LoadingOverlay.Instance.Show();
        NetWorkManager.Instance.Send<EvolutionRes>(CMD.CARDC_EVOLUTION, buffer, OnEvolution);
    }


    private void OnEvolution(EvolutionRes res)
    {
        LoadingOverlay.Instance.Hide();
        UpdateUserCard(res.UserCard);
        GlobalData.PlayerModel.UpdateUserMoney(res.UserMoney);
        GlobalData.CardModel.UpdateUserCards(new[] { res.UserCard });
        //Debug.LogError(res.UserItems.Count);
        GlobalData.PropModel.UpdateProps(res.UserItems);
        FlowText.ShowMessage(I18NManager.Get("Card_EvolutionSuccess"));
        AudioManager.Instance.PlayEffect("evolve", 1);
        //view.ShowEvolution();
        if (_evolutionWindow != null)
        {
            //_evolutionWindow.SetData(GlobalData.CardModel.CurCardVo);
            _evolutionWindow.Close();
        }

        switch (res.UserCard.Evolution)
        {
            case EvolutionPB.Evo0:
                break;
            case EvolutionPB.Evo1:
                //弹出弹窗显示看一看

                //新获得语音写入缓存
                List<ElementPB> elementPBs = GlobalData.DiaryElementModel.GetDialogsByCardId(res.UserCard.CardId);
                foreach (var v in elementPBs)
                {
                    Debug.LogError(" v id => " + v.Id);
                    GlobalData.DiaryElementModel.UpdateElement(v.Id, 1);
                    string storeKey = Constants.REDPOINT_YUYINSHOUCANG + v.Id;
                    Util.SetIsRedPoint(storeKey, true);
                }

                if (elementPBs.Count > 0)
                {
                    EvoJump(1, GlobalData.CardModel.GetUserCardById(res.UserCard.CardId).CardVo.Player);
                }

                break;
            case EvolutionPB.Evo2:
                //这里要播放动画啊！
                view.SetEvoEffect();
                Debug.Log("Can change cardface");
                break;
            case EvolutionPB.Evo3:
                //EvoJump(3, GlobalData.CardModel.GetUserCardById(res.UserCard.CardId).CardVo.Player);
                break;
        }

        if (GlobalData.MissionModel.IsHaveStarActivityMission)
        {
            GlobalData.MissionModel.MissionAttainmentModel.TriggerPopWindow(MissionAttainmentModel.StarActivityPopUpsType.CardEvolutionUp, (int)res.UserCard.Evolution);
        }

        //此刻控制特效出现。
        EventDispatcher.TriggerEvent(EventConst.ShowStoreScore);
        SdkHelper.StatisticsAgent.OnEvent("进化次数", Convert.ToInt32(res.UserCard.Evolution));
    }

    private void EvoJump(int evostage, PlayerPB curplayer)
    {

        var tipsWindow = PopupManager.ShowConfirmWindow(evostage == 1 ? I18NManager.Get("Card_HasSomeWordToSay") : I18NManager.Get("Card_HasSomeWordToSay2"),
            evostage == 1 ? I18NManager.Get("Card_UnlockNewVoice") : I18NManager.Get("Card_UnlockCloth"), evostage == 1 ? I18NManager.Get("Card_ToSee") : I18NManager.Get("Card_ToLook"));
        tipsWindow.CanClickBGMask = false;
        tipsWindow.WindowActionCallback = evt =>
        {
            if (evt == WindowEvent.Ok)
            {
                //跳转到目标窗口！
                if (evostage == 1)
                {
                    Debug.LogError("jump to voice");
                    ModuleManager.Instance.EnterModule(ModuleConfig.MODULE_FAVORABILITYMAIN, false, false, "Voice", curplayer);

                }
                else if (evostage == 3)
                {
                    Debug.LogError("jump to cloth");
                    GlobalData.FavorabilityMainModel.CurrentRoleVo = GlobalData.FavorabilityMainModel.GetUserFavorabilityVo((int)curplayer);
                    ModuleManager.Instance.EnterModule(ModuleConfig.MODULE_RELOADING, false, true);
                }

            }

        };
    }


    private void UpgradeStar(UserCardVo vo)
    {
        byte[] buffer = NetWorkManager.GetByteData(new StarUpReq { CardId = vo.CardId });
        LoadingOverlay.Instance.Show();
        NetWorkManager.Instance.Send<StarUpRes>(CMD.CARDC_STARUP, buffer, OnUpgradeStar);
    }


    private void OnUpgradeStar(StarUpRes res)
    {
        LoadingOverlay.Instance.Hide();

        GlobalData.PlayerModel.UpdateUserMoney(res.UserMoney);
        GlobalData.PropModel.UpdateProps(res.UserItems.ToArray());


        UpdateUserCard(res.UserCard, false, true);
        //		view.SetData(GlobalData.CardModel.GetUserCardById(res.UserCard.CardId),GlobalData.PropModel);
        FlowText.ShowMessage(I18NManager.Get("Card_StarUpSuccess"));
        AudioManager.Instance.PlayEffect("starup", 1);

        if (GlobalData.MissionModel.IsHaveStarActivityMission)
        {
            Debug.LogError("(int)res.UserCard.Star===>" + (int)res.UserCard.Star);
            GlobalData.MissionModel.MissionAttainmentModel.TriggerPopWindow(MissionAttainmentModel.StarActivityPopUpsType.CardHeartUp, (int)res.UserCard.Star);

        }

        //拿到升星的回包后再继续跑！
        EventDispatcher.TriggerEvent(EventConst.ShowStoreScore);

        SdkHelper.StatisticsAgent.OnEvent("升心次数", Convert.ToInt32(res.UserCard.Star));




    }

    public void Show(bool resetView)
    {
        view.transform.SetAsLastSibling();
        view.gameObject.Show();
        //		if(resetView)
        //			view.ShowUpgradeLevel(true);
    }

    public void Hide()
    {
        view.gameObject.Hide();
    }

    public override void Destroy()
    {
        EventDispatcher.RemoveEvent(EventConst.CardEvoConfirm);
    }
}
