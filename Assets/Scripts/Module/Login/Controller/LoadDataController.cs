using System;
using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Core.Events;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module.MainLine.Services;
using Assets.Scripts.Module.NetWork;
using Assets.Scripts.Module.Task.Service;
using Assets.Scripts.Service;
using Assets.Scripts.Services;
using Com.Proto;
using Common;
using DataModel;
using Framework.GalaSports.Service;
using game.main;
using UnityEngine;

public class LoadDataController : Controller
{
    private int _loadingProgress = 0;
    
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
            case MessageConst.CMD_LOGIN_LOAD_DATA:
                LoadingProgress.Instance.Show();
                _loadingProgress = 0;
                LoadingProgress.Instance.SetPercent(_loadingProgress);
                
                ClientData.LoadExpressiongData(null);
                
                new ServiceQueue(OnServiceFinish)
                    .Append(new GameConfigService())
                    .Append(new DepartmentService().SetCallback(o =>
                    {
                        GlobalData.PlayerModel.InitData();
                    }))
                    .Append(new CardBaseDataService())
                    .Append(new CardBaseRuleService())
                    .Append(new LevelService())
                    .Append(new MissionService())
                    .Append(new LoveAppointmentService())
                    .Append(new RechargeRuleService())
                    .Append(new ShopService())
                    .Start();

                break;
            case MessageConst.CMD_LOGIN_RESTORE_LOADING_UI:
                LoadingProgress.Instance.Show();
                LoadingProgress.Instance.SetPercent(_loadingProgress);
                break;
        }
    }

    private void StartLoadData()
    {

        LoaderMax loaderMax = new LoaderMax("LoginData", OnProgressHanlder, OnCompleteHandler, OnErroHandler);
        
        


        //道具基础信息
        loaderMax.Append<ItemsRes>(CMD.ITEMC_ITEMS, null, OnGetPropBaseData, true,
            GlobalData.VersionData.VersionDic[CMD.ITEMC_ITEMS]);

        //关卡基础信息
        loaderMax.Append<LevelRes>(CMD.CAREERC_LEVELS, null, OnGetLevelBaseData, true,
            GlobalData.VersionData.VersionDic[CMD.CAREERC_LEVELS]);

        //玩家规则
        loaderMax.Append<UserRuleRes>(CMD.USERC_USERRULE, null, OnGetUserRule, true,
            GlobalData.VersionData.VersionDic[CMD.USERC_USERRULE]);

        //玩家道具信息
        loaderMax.Append<MyItemRes>(CMD.ITEMC_MYITEM, null, OnGetMyProps);

        //玩家卡牌信息
        loaderMax.Append<MyCardRes>(CMD.CARDC_MYCARD, null, OnGetMyCard);

        //NPC
        loaderMax.Append<NpcRes>(CMD.NPC_RULES, null, OnGetNpc, true, GlobalData.VersionData.VersionDic[CMD.NPC_RULES]);
        //玩家NPC信息
        loaderMax.Append<UserNpcRes>(CMD.NPC_GETUSERNPC, null, OnGetMyNpc);

        loaderMax.Append<CardMemoriesRuleRes>(CMD.CARDMEMORIESC_CARDMEMORIESRULE, null, OnGetRecollectionRule, true,
            GlobalData.VersionData.VersionDic[CMD.CARDMEMORIESC_CARDMEMORIESRULE]);

        //音友相关数据
        loaderMax.Append<Rules>(CMD.MUSICGAMEC_RULES, null, OnGetMusicGameRule, true,
            GlobalData.VersionData.VersionDic[CMD.MUSICGAMEC_RULES]);

        //恋爱日记素材规则
        loaderMax.Append<ElementRes>(CMD.DIARYC_ELEMENTS_RULES, null, OnGetElement, true,
            GlobalData.VersionData.VersionDic[CMD.DIARYC_ELEMENTS_RULES]);
        //玩家恋爱日记素材
        loaderMax.Append<MyElementRes>(CMD.DIARYC_ELEMENTS_USER, null, OnGetMyElement);

        //如果因为手机数据缓存没清导致后置请求加载不了，请通知姗姗清数据
        loaderMax.Append<UserPhoneDataRes>(CMD.PHONEC_USERDATA_ALL,
            NetWorkManager.GetByteData(new UserPhoneDataReq() {IsOpenModule = 0}), OnGetMyPhoneData);

     

        //拉取好感度规则
        loaderMax.Append<FavorabilityRuleRes>(CMD.FAVORABILITY_RULE,null,OnGetFavorabilityRule,true,GlobalData.VersionData.VersionDic[CMD.FAVORABILITY_RULE]);

        //获取好感度信息
        loaderMax.Append<FavorabilityInfoRes>(CMD.FAVORABILITY_INFO, null, OnGetFavorabilityInfo);


        //获取用户签名道具列表
        loaderMax.Append<MySignatureRes>(CMD.CARDC_MYSIGNASTURES, null, OnGetSignatures);
        //获取用户碎片列表
        loaderMax.Append<MyPuzzleRes>(CMD.CARDC_MYPUZZLE, null, OnGetMyPuzzle);

        loaderMax.Append<ActivityRuleRes>(CMD.ACTIVITY_RULES, null, OnGetActivityRule, true,
            GlobalData.VersionData.VersionDic[CMD.ACTIVITY_RULES]);
        loaderMax.Append<ActivityRuleListRes>(CMD.ACTIVITY_ACTIVITYRULELIST, null, OnGetActivityMissionRule, true,
            GlobalData.VersionData.VersionDic[CMD.ACTIVITY_ACTIVITYRULELIST]);

        loaderMax.Append<Rules>(CMD.MUSICGAMEC_RULES, null, OnGetMusicGameRule, true, GlobalData.VersionData.VersionDic[CMD.MUSICGAMEC_RULES]);


        loaderMax.Append<MallRuleRes>(CMD.MALL_RULE,null,OnGetMallRuleCall);
        //如果走完星路历程在拉
        if (GlobalData.LevelModel.FindLevel("1-3").IsPass)
        {
            loaderMax.Append<ActivityRes>(CMD.ACTIVITY_ACTIVITYLIST, null, OnGetActivityRes);
           // loaderMax.Append<ActivityListRes>(CMD.ACTIVITY_ACTIVITYLISTS2, null, OnGetActivityListRes);
        }
        
        loaderMax.Load();
             
    }

    private void OnGetMusicGameRule(Rules obj)
    {


    }

    private void OnGetMyPuzzle(MyPuzzleRes res)
    {
       var  puzzleList = new List<CardPuzzleVo>();
        for (int i = 0; i < res.UserPuzzles.Count; i++)
        {
            CardPuzzleVo vo = new CardPuzzleVo(res.UserPuzzles[i]);
            puzzleList.Add(vo);
        }
        puzzleList.Sort();
        GlobalData.CardModel.CardPuzzleList = puzzleList;
    }



    private void OnGetActivityRule(ActivityRuleRes res)
    {
        GlobalData.ActivityModel.GetAllActivityRuleRes(res);
    }
    private void OnGetActivityMissionRule(ActivityRuleListRes res)
    {
        GlobalData.ActivityModel.InitAllActivityTemplateRuleRes(res);
    }

    private void OnGetActivityRes(ActivityRes res)
    {
        GlobalData.ActivityModel.GetAllActivityRes(res);
    }

//    private void OnGetActivityListRes(ActivityListRes res)
//    {
//        GlobalData.ActivityModel.GetActivityListRes(res);
//    }
    
    private void OnGetMyElement(MyElementRes res)
    {
        GlobalData.DiaryElementModel.InitMyElement(res);
    }

    private void OnGetElement(ElementRes res)
    {
        GlobalData.DiaryElementModel.InitElementRule(res);
    }


    private void OnGetMyPhoneData(UserPhoneDataRes res)
    {
        GlobalData.PhoneData.InitData(res);
    }

    private void OnGetRecollectionRule(CardMemoriesRuleRes res)
    {
        //只做加载
    }

    private void OnGetUserRule(UserRuleRes res)
    {
        GlobalData.PlayerModel.InitRule(res);
    }

    private void OnErroHandler(HttpErrorVo obj)
    {
        Debug.LogError("===Fail===" + obj);
    }

    private void OnCompleteHandler()
    {
        LoadingProgress.Instance.SetPercent(100);

        SendMessage(new Message(MessageConst.CMD_LOGIN_LOAD_DATAFINISHED));
        
        //数据加载完成后显示活动
        EventDispatcher.TriggerEvent(EventConst.OnDataLoadComplete);

        //触发主界面生成活动滚动播放
        EventDispatcher.TriggerEvent(EventConst.CreateActivityContenet);
        

        
        ClientTimer.Instance.DelayCall(()=> 
        {
            LoadingProgress.Instance.Hide();
        }, 1f);

        Debug.Log("OnCompleteHandler");
    }

    private void OnGetMallRuleCall(MallRuleRes res)
    {
        GlobalData.PayModel.AddOn(res.RmbMallRules);
        
        GlobalData.RandomEventModel.InitRule(res.RmbMallRules, res.TriggerGifts);
        new TriggerService().Execute();
    }

    private void OnServiceFinish(List<IService> failList)
    {
        StartLoadData();
    }

    private void OnProgressHanlder(int obj,int max)
    {
        Debug.Log("progress:" + obj);

        _loadingProgress = obj * 100 / max;
        LoadingProgress.Instance.SetPercent(_loadingProgress);
    }

    private void OnGetNpc(NpcRes res)
    {
        GlobalData.NpcModel.InitList(res);
    }

    private void OnGetMyNpc(UserNpcRes res)
    {
        GlobalData.NpcModel.InitUserNpc(res);
    }

    private void OnGetMyProps(MyItemRes res)
    {
        GlobalData.PropModel.InitMyProps(res);
    }
    
    private void OnGetSignatures(MySignatureRes res)
    {
        GlobalData.CardModel.InitSignatures(res);
    }

    private void OnGetMyCard(MyCardRes res)
    {
        GlobalData.CardModel.InitMyCards(res);
    }

    private void OnGetLevelBaseData(LevelRes res)
    {
    }

    private void OnGetPropBaseData(ItemsRes res)
    {
        GlobalData.PropModel.InitPropBase(res);
    }

    private void OnGetFavorabilityRule(FavorabilityRuleRes res)
    {
        GlobalData.FavorabilityMainModel.Init(res);
    }
    private void OnGetFavorabilityInfo(FavorabilityInfoRes res)
    {
        GlobalData.FavorabilityMainModel.GetUserFavorabilityData(res.UserFavorability);
        GlobalData.FavorabilityMainModel.UserFavorabilityInfoPb = res.UserFavorabilityInfo;
    }

}