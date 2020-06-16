
using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Interfaces;
using Assets.Scripts.Module;
using Com.Proto;
using DataModel;
using Google.Protobuf.Collections;
using UnityEngine;

namespace Common
{
    public class GuideManager
    {
        private static GuideModule _guideModule;

        private static Dictionary<ModulePB, List<FunctionEntryPB>> _openDict =
            new Dictionary<ModulePB, List<FunctionEntryPB>>();

        private static RepeatedField<FunctionEntryPB> _funtionEntrys;

        private static Dictionary<string, IModule> _moduleDict;
        private static Dictionary<GuideTypePB, int> _remoteGuideDict;

        private static bool _isStepup = false;

        public static TempState TempState;


       

        /// <summary>
        /// 引导类型
        /// 默认是true，强制引导
        ///      false,是非强制引导
        /// </summary>
        public static bool GuideType = true;


        public static void Setup()
        {
            if (_isStepup)
                return;

            _isStepup = true;
            IModule module = new GuideModule();
            module.ModuleName = ModuleConfig.MODULE_GUIDE;
            
            module.Parent = Main.GuideCanvas.gameObject;
            module.LoadAssets();
            module.Init();
            _guideModule = (GuideModule) module;
        }

        public static void RegisterModule(IModule module)
        {
            if (_moduleDict == null)
                _moduleDict = new Dictionary<string, IModule>();       
            _moduleDict.Add(module.ModuleName, module);

            module.MessageHandler = _guideModule.HandleModuleMessage;

            OpenGuide(module);
        }

        public static void UnregisterModule(IModule module)
        {
            if (_moduleDict != null && _moduleDict.ContainsKey(module.ModuleName))
            {              
                _moduleDict.Remove(module.ModuleName);
                
                module.MessageHandler = null;
            }
        }



        public static bool IsPass1_9()
        {
            return GlobalData.LevelModel.FindLevel("1-9").IsPass;
        }

        public static bool IsPass4_12()
        {
            return GlobalData.LevelModel.FindLevel("4-12").IsPass;
        }
        
        
        public static void OpenGuide(IModule module)
        {
            if (GetStepState(module.ModuleName) == GuideStae.Close)
                return;

            switch (module.ModuleName)
            {
                case ModuleConfig.MODULE_GAME_MAIN:
                    _guideModule.ShowMainPanelGuide();
                    break;

                case ModuleConfig.MODULE_BATTLE:
                    _guideModule.ShowBattleGuide();
                    break;

                case ModuleConfig.MODULE_GAME_PLAY:
                    _guideModule.ShowGameplayGuide();
                    break;

                case ModuleConfig.MODULE_MAIN_LINE:
                    _guideModule.ShowMainLineGuide();
                    break;

                case ModuleConfig.MODULE_SUPPORTER:
                    _guideModule.ShowSupporterModule();
                    break;
                case ModuleConfig.MODULE_PHONE:
                    _guideModule.ShowPhoneModule();
                    break;
                case ModuleConfig.MODULE_SUPPORTERACTIVITY:
                    _guideModule.ShowSupporterActModule();
                    break;

                case ModuleConfig.MODULE_CARD:
                    _guideModule.ShowCardGuide();
                    break;
                case ModuleConfig.MODULE_LOVE:
                    _guideModule.ShowLoveGuide();
                    break;
                case ModuleConfig.MODULE_LOVEAPPOINTMENT:
                    _guideModule.ShowLoveAppointmentGuide();
                    break;
                case ModuleConfig.MODULE_LOVEDIARY:
                    _guideModule.ShowLoveDiaryModule();
                    break;

                case ModuleConfig.MODULE_RECOLLECTION:
                    _guideModule.ShowRecollectionModule();
                    break;

                case ModuleConfig.MODULE_FAVORABILITYMAIN:
                    _guideModule.ShowFavorabilityGuide();
                    break;
                case ModuleConfig.MODULE_DRAWCARD:
                    _guideModule.ShowDrawCardGuide();
                    break;
                case ModuleConfig.MODULE_ACHIEVEMENT:
                    _guideModule.ShowAchievementGuide();
                    break;
                case ModuleConfig.MODULE_VISIT:
                    _guideModule.ShowVisitModule();
                    break;
                case ModuleConfig.MODULE_ACTIVITY:
                    _guideModule.ShowActivityGuide();
                    break;
                case ModuleConfig.MODULE_STORY:
                    _guideModule.ShowStoryGuide();
                    break;
                case ModuleConfig.MODULE_TAKEPHOTOSGAME:
                    _guideModule.ShowTakePhotosGameModule();
                    break;
                case ModuleConfig.MODULE_COAXSLEEP:
                    _guideModule.ShowCoaxSleepGuide();
                    break;
                case ModuleConfig.MODULE_MISSION:
                    _guideModule.ShowTaskGuide();
                    break;
            }
        }
        
       public static void OpenGuide(GuideEnumType guideType)
        {
            if (GetStepState(guideType.ToString()) == GuideStae.Close)
                return;
            switch (guideType)
            {
                case GuideEnumType.VISIT_BLESS:
                    if (GetStepState(ModuleConfig.MODULE_VISIT) != GuideStae.Close)
                        return;
                    _guideModule.ShowVisitBlessModule();
                    break;
            }

        }

        private static Dictionary<GuideTypePB, List<AwardPB>> _guideAwards;
        public static void InitGuideRule(RepeatedField<GuideRulePB> guideRules)
        {
            
           _guideAwards =new Dictionary<GuideTypePB, List<AwardPB>>();
            
            foreach (var t in guideRules)
            {
                if ( t.GuideType== GuideTypePB.MainGuide && t.Awards.Count>0 )
                {               
                    _guideAwards.Add(t.GuideType,t.Awards.ToList());                  
                }
                else if(t.GuideType== GuideTypePB.CardMemoriesGuide && t.Awards.Count>0)
                {
                    _guideAwards.Add(t.GuideType,t.Awards.ToList()); 
                }
            }
        }

        /// <summary>
        /// 通关统计
        /// </summary>
        /// <param name="level"></param>
        public static void SetPassLevelStep(string level)
        {
            int step = 0;
            switch (level)
            {
               case "1-1":
                   step = GuideConst.MainLineStep_Stroy1_1_Over;
                  break;                
              case  "1-2" :
                  step = GuideConst.MainLineStep_Stroy1_2_Over;
                  break;
              case "1-3":
                  step = GuideConst.MainLineStep_Stroy1_3_Over;
                  break;
              case "1-4":
                  step = GuideConst.MainLineStep_Battle1_4_Over;
                  break;
              case  "1-5":
                  step = GuideConst.MainLineStep_Stroy1_5_Over;
                  break;
              case "1-6":
                  step = GuideConst.MainLineStep_Stroy1_6_Over;
                  break;
              case "1-7":
                  step = GuideConst.MainLineStep_Stroy1_7_Over;
                  break;
              case "1-8":
                  step = GuideConst.MainLineStep_Battle1_8_Over;
                  break;
              case "1-9":
                  step = GuideConst.MainLineStep_Stroy1_9_Over;
                  break;           
              case "2-1":
                  step = GuideConst.MainLineStep_2_1_Over;
                  break;
              case "2-2":
                  step = GuideConst.MainLineStep_2_2_Over;
                  break;
              case "2-3":
                  step = GuideConst.MainLineStep_2_3_Over;
                  break;
              case "2-4":
                  step = GuideConst.MainLineStep_2_4_Over;
                  break;   
               case "2-5":
                   step = GuideConst.MainLineStep_2_5_Over;
                   break;
               case "2-6":
                   step = GuideConst.MainLineStep_2_6_Over;
                   break;
               case "2-7":
                   step = GuideConst.MainLineStep_2_7_Over;
                   break;
               case "2-8":
                   step = GuideConst.MainLineStep_2_8_Over;
                   break;
               case "2-9":
                   step = GuideConst.MainLineStep_2_9_Over;
                   break;
               case "2-10":
                   step = GuideConst.MainLineStep_2_10_Over;
                   break;
               case "2-11":
                   step = GuideConst.MainLineStep_2_11_Over;
                   break;
               case "2-12":
                   step = GuideConst.MainLineStep_2_12_Over;
                   break;
               case "3-1":
                   step = GuideConst.MainLineStep_3_1_Over;
                   break;
               case "3-2":
                   step = GuideConst.MainLineStep_3_2_Over;
                   break;
            }
            
            if (step!=0)
            {
                SetStatisticsRemoteGuideStep(step);   
            }
        }
        
        /// <summary>
        /// 获取主线引导奖励从规则
        /// </summary>
        /// <returns></returns>
        public static List<AwardPB> GetGuideAwardsToRule(GuideTypePB type)
        {
            return _guideAwards[type];
        }
        
      
        
        
        /// <summary>
        /// 初始化引导信息
        /// </summary>
        /// <param name="userGuideMap">模块引导记录</param>
        public static void InitData(RepeatedField<UserGuidePB> guidePb) //MapField<int, int> userGuideMap
        {
            
           // SetRemoteGuideStep(GuideTypePB.MainGuide,1000);
         if (_remoteGuideDict == null)
            {
                //保证新手引导的时候不被重新初始化
                _remoteGuideDict = new Dictionary<GuideTypePB, int>();
                foreach (var map in guidePb)
                {
                    _remoteGuideDict.Add(map.GuideType, map.GuideId);
                }
            }

            if (_remoteGuideDict.ContainsKey(GuideTypePB.MainGuide))
            {
                Debug.Log("<color='#11ff99'>主线引导步骤======>" + _remoteGuideDict[GuideTypePB.MainGuide] + "</color>");
            }


            foreach (var pb in _funtionEntrys)
            {
                List<FunctionEntryPB> list;
                if (_openDict.ContainsKey(pb.Module) == false)
                {
                    list = new List<FunctionEntryPB>();
                    _openDict.Add(pb.Module, list);
                }
                else
                {
                    list = _openDict[pb.Module];
                }

                list.Add(pb);
            }
         
        }

        /// <summary>
        /// 获取功能开放描述
        /// </summary>
        /// <param name="module"></param>
        /// <param name="funtionId"></param>
        /// <returns></returns>
        public static string GetOpenConditionDesc(ModulePB module, FunctionIDPB funtionId)
        {

           string desc ="";
            List<FunctionEntryPB> list = _openDict[module];
            foreach (var entryPb in list)
            {
                if (funtionId == entryPb.FunctionId)
                {
                    
                    if (entryPb.LevelBottom==0 && entryPb.PlotBottom!=0)     //关卡解锁
                    {
                        desc = I18NManager.Get("Guide_PlotBottomDesc",entryPb.PlotBottom/100,entryPb.PlotBottom%100);                        
                    }
                    else if(entryPb.LevelBottom!=0 && entryPb.PlotBottom==0) //等级解锁
                    {
                        desc = I18NManager.Get("Guide_LevelBottomDesc", entryPb.LevelBottom);                        
                    }
                    else if(entryPb.LevelBottom!=0 && entryPb.PlotBottom!=0) //关卡和等级解锁
                    {                        
                        desc = I18NManager.Get("Guide_LevelBottomAndPlotBottomDesc",entryPb.LevelBottom,entryPb.PlotBottom/100,entryPb.PlotBottom%100);        
                    }
                  
                    break;
                }
            }
            return desc;
        }
        
        
        /// <summary>
        /// 判断功能是否开放
        /// </summary>
        /// <param name="module"></param>
        /// <param name="funtionId"></param>
        /// <returns>默认开放</returns>
        public static bool IsOpen(ModulePB module, FunctionIDPB funtionId)
        {
            int levelBottom = 0;
            int storyBottom = 0;
            List<FunctionEntryPB> list = _openDict[module];
            foreach (var entryPb in list)
            {
                if (funtionId == entryPb.FunctionId)
                {
                    levelBottom = entryPb.LevelBottom;
                    storyBottom = entryPb.PlotBottom;
                    break;
                }
            }

            if (levelBottom != 0)
            {
                int level = GlobalData.PlayerModel.PlayerVo.Level;
                return level >= GetOpenUserLevel(module, funtionId);
            }
            else if (storyBottom != 0)
            {
                LevelVo vo;
                if (GlobalData.LevelModel.LevelDict.TryGetValue(storyBottom, out vo))
                {
                    return vo.IsPass;
                }
            }

            return true;
        }

        public static int GetOpenUserLevel(ModulePB module, FunctionIDPB funtionId)
        {
            List<FunctionEntryPB> list = _openDict[module];
            foreach (var entryPb in list)
            {
                if (funtionId == entryPb.FunctionId)
                    return entryPb.LevelBottom;
            }
            return -1;
        }
        
        public static string GetOpenMainStory(ModulePB module, FunctionIDPB funtionId)
        {
            List<FunctionEntryPB> list = _openDict[module];
            int levelId = -1;
            foreach (var entryPb in list)
            {
                if (funtionId == entryPb.FunctionId)
                {
                    levelId = entryPb.PlotBottom;
                    break;
                }
            }

            LevelVo vo;
            if (GlobalData.LevelModel.LevelDict.TryGetValue(levelId, out vo))
            {
                return vo.LevelMark;
            }
            
            return null;
        }

        public static void SetData(RepeatedField<FunctionEntryPB> funtionEntrys)
        {         
            _funtionEntrys = funtionEntrys;         
        }

        public static GuideStae GetStepState(string moduleName, string step = "none")
        {
            int stepRet = -1;
            stepRet = PlayerPrefs.GetInt("Guide_Step_" + moduleName + "___" + step, -1);
            if (stepRet == -1)
                return GuideStae.None;

            return (GuideStae) stepRet;
        }

        public static void SetStepState(GuideStae state, string moduleName, string step = "none")
        {
            PlayerPrefs.SetInt("Guide_Step_" + moduleName + "___" + step, (int) state);
            PlayerPrefs.Save();
        }

        /// <summary>
        /// 透传消息到其他模块
        /// </summary>
        /// <param name="message"></param>
        public static void UnvarnishedTransmissionMessage(Message message)
        {
            if (_moduleDict == null)
                return;
            List<KeyValuePair<string, IModule>> moduleList = _moduleDict.ToList();

            for (int i = moduleList.Count - 1; i >= 0; i--)
            {
                moduleList[i].Value.SendMessage(message);
            }
        }

        public static int GetRemoteGuideStep(GuideTypePB type)
        {
            if (_remoteGuideDict!=null)
            {
                if (_remoteGuideDict.ContainsKey(type))
                {
                    return _remoteGuideDict[type];
                }  
            }
            

            return -1;
        }

        public static void SetRemoteGuideStep(GuideTypePB guideType, int step)
        {
            _guideModule.SetRemoteGuideStep(guideType, step);
        }

        
        /// <summary>
        /// 设置新手引导统计点
        /// </summary>
        /// <param name="step"></param>
        public static void SetStatisticsRemoteGuideStep(int step)
        {
            _guideModule.SetStatisticsRemoteGuideStep(step);
        }
        
       
        
        public static void UpdateRemoteGuide(UserGuidePB userGuide)
        {
            if (_remoteGuideDict.ContainsKey(userGuide.GuideType))
            {
                int currentStep = _remoteGuideDict[userGuide.GuideType];
                if (currentStep >= userGuide.GuideId)
                    return;
                
                _remoteGuideDict[userGuide.GuideType] = userGuide.GuideId;
            }
            else
            {
                _remoteGuideDict.Add(userGuide.GuideType, userGuide.GuideId);
            }
        }

        public static void Show()
        {
            _guideModule?.OnShow(0);
        }

        public static void Hide()
        {
            _guideModule?.OnHide();
        }

        public static void Reset()
        {
            TempState = TempState.NONE;
            
            if (_remoteGuideDict != null)
            {
                _remoteGuideDict.Clear();
                _remoteGuideDict = null;
            }
            
            _isStepup = false;

            if (_guideModule != null)
            {
                _guideModule.Remove(0);
                _guideModule = null;
            }

            _openDict = new Dictionary<ModulePB, List<FunctionEntryPB>>();
        }
        
        
        /// <summary>
        /// 获取功能引导阶段
        /// </summary>
        /// <param name="typePb">引导类型</param>
        /// <returns></returns>
        public static FunctionGuideStage CurFunctionGuide(GuideTypePB typePb)
        {
            FunctionGuideStage stage = FunctionGuideStage.Function_DEFAULT;

            switch (typePb)
            {
                case GuideTypePB.MainGuide:
                    break;
                case GuideTypePB.EncourageActGuide:
                    break;
                case GuideTypePB.CardMemoriesGuide:
                    break;
                case GuideTypePB.MainGuideRecord:
                    break;
                case GuideTypePB.LoveGuideCoaxSleep:
                    stage=  CoaxSleepGuideStage();
                    break;               
            }

            return stage;
        }


        /// <summary>
        /// 哄睡引导
        /// </summary>
        /// <returns></returns>
        private static FunctionGuideStage CoaxSleepGuideStage()
        {
            if (GetRemoteGuideStep( GuideTypePB.LoveGuideCoaxSleep)<GuideConst.FunctionGuide_CoaxSleep_OneStage)
            {
                return FunctionGuideStage.Function_CoaxSleep_OneStage;
            }
            else if(GetRemoteGuideStep(GuideTypePB.LoveGuideCoaxSleep)>=GuideConst.FunctionGuide_CoaxSleep_OneStage&&
                    GetRemoteGuideStep(GuideTypePB.LoveGuideCoaxSleep)<GuideConst.FunctionGuide_CoaxSleep_TwoStage )
            {
                return FunctionGuideStage.Function_CoaxSleep_TowStage;
            }
            else
            {
                return FunctionGuideStage.Function_CoaxSleep_End;
            }
        }
        
        public static GuideStage CurStage()
        {
            int remoteGuideStep = GetRemoteGuideStep(GuideTypePB.MainGuide);
            if (remoteGuideStep==GuideConst.MainStep_Over )
            {
                return GuideStage.Over;
            }           
            else  if (remoteGuideStep<GuideConst.MainLineStep_Stroy1_3_Over)//没通过1-3都是 1-1~1-3引导阶段
            {
                return GuideStage.MainLine1_1Level_1_3Level_Stage;
            }
            else if(GlobalData.ActivityModel.GetActivityVo(ActivityType.ActivitySevenDaySignin)!=null
                    &&!GlobalData.ActivityModel.GetActivityVo(ActivityType.ActivitySevenDaySignin).IsActivityPastDue
                    &&GlobalData.ActivityModel.GetUserSevenDaySigninInfo()?.SignDay==0)
            {
                return GuideStage.SevenDaySigninActivityStage;
            }
            // else if(GlobalData.ActivityModel.GetActivityVo(ActivityType.ActivitySevenDaySignin)!=null
            //         &&!GlobalData.ActivityModel.GetActivityVo(ActivityType.ActivitySevenDaySignin).IsActivityPastDue
            //         &&GlobalData.ActivityModel.GetUserSevenDaySigninInfo()?.SignDay==1)
            // {
            //     return GuideStage.MainLine1_4Level_2_3Level_Stage;
            // }           
            // else if (GlobalData.ActivityModel.GetActivityVo(ActivityType.ActivitySevenDaySignin) == null &&
            //         !GlobalData.LevelModel.FindLevel("1-5").IsPass)
            // {
            //     return GuideStage.MainLine1_4Level_2_3Level_Stage;
            // }
            else if(GlobalData.LevelModel.FindLevel("1-9").IsPass && GlobalData.LevelModel.FindLevel("2-1").IsPass == false)
            {
                var missionModel = GlobalData.MissionModel;
                var dailyMission = missionModel.GetMissionListByType(MissionTypePB.Daily);

                if (remoteGuideStep < GuideConst.MainLineStep_Stroy1_9_Over)
                {
                    UserGuidePB userGuide = new UserGuidePB()
                    {
                        GuideId = GuideConst.MainLineStep_Stroy1_9_Over,
                        GuideType = GuideTypePB.MainGuide
                    };
                    UpdateRemoteGuide(userGuide);
                    SetRemoteGuideStep(GuideTypePB.MainGuide,GuideConst.MainLineStep_Stroy1_9_Over);     
                }
                
                if (missionModel.HasDailyActivityAward(dailyMission) || TempState == TempState.Pass_Level1_9)
                {
                    //没有更新任务的时候用TempState确定引导点
                    return GuideStage.MainLineStep_Stroy1_9_Over;
                }
                else
                {
                    return GuideStage.MainLineStep_Stroy2_1_Start;
                }
            }
            else if (remoteGuideStep < GuideConst.MainStep_MainStory2_4_Fail) 
            {
                return GuideStage.MainLine1_4Level_2_3Level_Stage;
            }
            else if(remoteGuideStep == GuideConst.MainStep_MainStory2_4_Fail)
            {
                if (GlobalData.CardModel.UserCardList.Count > 1)
                {
                    return GuideStage.LoveStoryStage;
                }
                return GuideStage.MainStep_MainStory2_4_Fail;
            }
            // else if (GetRemoteGuideStep(GuideTypePB.MainGuide) < GuideConst.MainLineStep_OnClick_Npc_Sms
            //          && !GlobalData.LevelModel.FindLevel("1-7").IsPass
            //          && GlobalData.PhoneData.GetSelectsLocal(101).Count == 0)
            // {
            //     return GuideStage.PhoneSmsStage;
            // }
            // else if (!GlobalData.LevelModel.FindLevel("1-7").IsPass)
            // {
            //     return GuideStage.MainLine1_6Level_1_7Level_Stage;
            // }
            // else if (GetRemoteGuideStep(GuideTypePB.MainGuide) < GuideConst.MainLineStep_OnClick_GlodDrawCard)
            // {
            //     return GuideStage.DrawCardStage;
            // }
            else if (remoteGuideStep < GuideConst.MainLineStep_OnClick_LoveStroy_1)
            {
                //进入恋爱引导
                return GuideStage.LoveStoryStage;
            }
            else if (remoteGuideStep == GuideConst.MainLineStep_OnClick_LoveStroy_1)
            {
                bool showCardLevelup = false;
                for (int i = 0; i < GlobalData.CardModel.UserCardList.Count; i++)
                {
                    var vo = GlobalData.CardModel.UserCardList[i];
                    if (vo.CardVo.Credit == CreditPB.Sr && vo.Level == 0)
                    {
                        showCardLevelup = true;
                        break;
                    }
                }

                if (showCardLevelup)
                {
                    return GuideStage.CardLevelUpStage;
                }
                
                LevelModel levelModel = GlobalData.LevelModel;
                LevelVo level2_9 = levelModel.FindLevel("2-9");
                if (level2_9.IsPass)
                {
                    return GuideStage.FavorabilityShowRoleStage;
                }
                else
                {
                    //点击过恋爱引导 开始主线引导 2-4
                    return GuideStage.MainStep_MainStory2_4_Start;
                }
            }
            else if (remoteGuideStep == GuideConst.MainLineStep_OnClick_FavorabilityShowMainViewBtn)
            {
                return GuideStage.MainStep_MainStory2_10_Start;
            }
            // else if (!GlobalData.LevelModel.FindLevel("1-8").IsPass && CardLevel() == 0)
            // {
            //     return GuideStage.CardLevelUpStage;
            // }
            // else if (GlobalData.LevelModel.FindLevel("1-9").IsPass
            //          && remoteGuideStep <
            //          GuideConst.MainLineStep_OnClick_FavorabilityShowMainViewBtn
            //          && GlobalData.PlayerModel.PlayerVo.ExtInfo.DownloadReceive == 0)
            // {
            //     return GuideStage.FavorabilityShowRoleStage;
            // }

            else if (GlobalData.PlayerModel.PlayerVo.ExtInfo.DownloadReceive == 0)
            {
                return GuideStage.ExtendDownloadStage;
            }
            // else if (!GlobalData.LevelModel.FindLevel("2-12").IsPass)
            // {
            //     return GuideStage.MainLine1_9Level_Over_Stage;
            // }
            // else if (GlobalData.LevelModel.FindLevel("2-12").IsPass &&
            //          GetRemoteGuideStep(GuideTypePB.MainGuide) < GuideConst.MainStep_Over)
            // {
            //     return GuideStage.LoveDiaryStage;
            // }
            else
            {
                return GuideStage.Over;
            }
        }



        private static int CardLevel()
        {          
            var list = GlobalData.CardModel.UserCardList;
            foreach (var t in list)
            {
                if (t.CardVo.Credit == CreditPB.Sr)
                {
                    return t.Level;
                }
            }
            return 0;

        }


    }

    public enum TempState
    {
        NONE,
        Level2_4_Fail,
        Pass_Level1_7,
        Pass_Level1_9,
        Pass_Level1_12,
        Pass_Level2_4,
        Level3_3_Fail,
        AchievementOver
    }

    public enum GuideStae
    {
        None,
        Close,
        Open
    }

    /// <summary>
    /// 功能引导阶段
    /// 目前包含（哄睡引导）
    /// </summary>
    public enum FunctionGuideStage
    {
        Function_DEFAULT,              
        Function_CoaxSleep_OneStage,
        Function_CoaxSleep_TowStage,
        Function_CoaxSleep_End,
    }
    
    /// <summary>
    /// 引导阶段
    /// </summary>
    public enum GuideStage
    {
        /// <summary>
        /// 主线引导1~3关阶段  0~90
        /// </summary>
        MainLine1_1Level_1_3Level_Stage =90,
        
        /// <summary>
        /// 七日签到活动引导阶段 110~130
        /// </summary>
        SevenDaySigninActivityStage=130,
        
        MainLineStep_Stroy1_9_Over = 280,
        
        MainLineStep_Stroy2_1_Start = 305,

        MainLine1_4Level_2_3Level_Stage = 360,
        
        MainStep_MainStory2_4_Fail = 400,

        // /// <summary>
        // /// 手机短息引导阶段 210~240
        // /// </summary>
        // PhoneSmsStage=240,
        //
        //
        // /// <summary>
        // /// 主线引导1-6~1-7阶段
        // /// </summary>
        // MainLine1_6Level_1_7Level_Stage=268,
        //
        // /// <summary>
        // /// 抽卡引导阶段270~290
        // /// </summary>
        //DrawCardStage=580,
        
        /// <summary>
        /// 恋爱剧情引导300~330
        /// </summary>
        LoveStoryStage=410,
            
        /// <summary>
        /// 扩展下载阶段
        /// </summary>
        ExtendDownloadStage =430, 
        
        /// <summary>
        /// 星缘引导
        /// </summary>
        CardLevelUpStage=480,
        
        MainStep_MainStory2_4_Start = 500,
        
        MainStep_MainStory2_10_Start = 605,
        
        /// <summary>
        /// 好感度展示主界面引导
        /// </summary>
        FavorabilityShowRoleStage=620,
        
        /// <summary>
        /// 恋爱日记引导阶段
        /// </summary>
        LoveDiaryStage=700,
        
        MainLine2_12_Over_Stage = 800,
        
        Over=1000,
    }



    
    
    
}