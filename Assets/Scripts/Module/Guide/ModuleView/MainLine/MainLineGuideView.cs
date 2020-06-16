using System;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Core.Events;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module.NetWork;
using Com.Proto;
using Common;
using DataModel;
using DG.Tweening;
using game.main;
using game.tools;
using GalaAccount.Scripts.Framework.Utils;
using GalaAccountSystem;
using Google.Protobuf.Collections;
using QFramework;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
namespace Assets.Scripts.Module.Guide.ModuleView
{
    public class MainLineGuideView : View
    {

        private Transform _calendarOnClick;   //章节点击
        private Transform _battleIntroductionPopup;   //战斗确认窗口
        private Transform _backArrow;   //返回按钮
        private Transform _bottomNpc;    //底部张轩
        private RectTransform _calendar2;
        private Transform _topNpc;       //顶部张轩
        private Text _bottomNpcTxt;
        private Text _topNpcTxt;
        private Transform _award;
        private readonly float _samllScale = 0f;

        private void Awake()
        {
            _calendarOnClick = transform.Find("Calendar");
            _battleIntroductionPopup = transform.Find("BattleIntroductionPopupStep");
            _backArrow = transform.Find("BackArrow");
            _bottomNpc = transform.Find("BottomGuideView");
            _calendar2 = transform.GetRectTransform("Calendar2");
            _topNpc = transform.Find("TopGuideView");

            _bottomNpcTxt = _bottomNpc.GetText("DialogFrame/Text");
            _topNpcTxt = _topNpc.GetText("DialogFrame/Text");
            _award = transform.Find("Award");
            _award.localScale =new Vector3(_samllScale,_samllScale,1f);
            
            if ((float) Screen.height / Screen.width < 1.4f)
            {
                Vector2 vector = new Vector2(-64, _calendar2.anchoredPosition.y);

                _calendarOnClick.GetRectTransform().anchoredPosition = vector;
                _calendar2.anchoredPosition = vector;
            }
        }

        
       public void HandleStep()
       {
           var curStage = GuideManager.CurStage();
           
           Debug.LogError("主线当前阶段===>"+curStage);

           if (GuideManager.TempState == TempState.Level3_3_Fail)
           {
               GoToNextGuideStage("星路中会提供了大量经验道具、钻石和星卡");
               gameObject.Show();
               return;
           }
           
           if (curStage == GuideStage.MainLine1_1Level_1_3Level_Stage)
           {
               Level1_1ToLevel1_3Stage();
           }
           else if(curStage <= GuideStage.MainLine1_4Level_2_3Level_Stage)
           {
               Level1_4ToLevel2_3Stage();
           }
           else if(curStage == GuideStage.MainStep_MainStory2_4_Fail)
           {
               Level2_4Fail();
           }
           else if (curStage == GuideStage.MainStep_MainStory2_4_Start)
           {
               Level2_4ToLevel2_9Stage();
           }
           else if(curStage == GuideStage.FavorabilityShowRoleStage)
           {
               Level2_9End();
           }
           else if (curStage == GuideStage.MainStep_MainStory2_10_Start)
           {
               Level2_10ToLevel2_11Stage();
           }
           else if (curStage == GuideStage.Over)
           {
               gameObject.Hide();
               StarFunctionGuide();  
           }
           HideMask(GuideManager.GuideType);
           
           // switch (curStage)
           // {             
           //     case GuideStage.MainLine1_1Level_1_3Level_Stage:
           //         
           //         break;              
           //     case GuideStage.MainLine1_4Level_2_3Level_Stage:
           //         Level1_4ToLevel2_3Stage();
           //         break;  
           //     case GuideStage.PhoneSmsStage:                 
           //          Level1_5End();
           //          break;
           //     case GuideStage.MainLine1_6Level_1_7Level_Stage:
           //         Level1_6ToLevel1_7Stage();
           //         break;     
           //     case GuideStage.DrawCardStage:
           //         Level1_7End();
           //         break;
           //     case GuideStage.MainLine1_8Level_1_9Level_Stage:
           //         Level1_8ToLevel1_9Stage();
           //         break;
           //     case GuideStage.FavorabilityShowRoleStage:
           //         Level_9End();
           //         break;
           //     // case GuideStage.MainLine1_9Level_Over_Stage:
           //     //     Level1_9OverStage();
           //     //     break;
           //     // case GuideStage.LoveDiaryStage:
           //     //     GuideManager.GuideType = true;
           //     //     Level2_12End();
           //     //     break;
           //     case GuideStage.Over:
           //
           //         gameObject.Hide();
           //         
           //         StarFunctionGuide();  
           //         
           //         break;                            
           // }
           
           
      }

       private void Level2_10ToLevel2_11Stage()
       {
           var levelModel = GlobalData.LevelModel;
           LevelVo level2_10 = levelModel.FindLevel("2-10");
           LevelVo level2_11 = levelModel.FindLevel("2-11");

           if (!level2_10.IsPass)
           {
               OnClickLevel("2-10");
           }
           else if (!level2_11.IsPass)
           {
               OnClickLevel("2-11");
           }
           else if(level2_11.IsPass)
           {
               ShowSweep2_11_Start();
           }
       }

       private void ShowSweep2_11_Start()
       {
           gameObject.Show();
           var clickAreal = _calendarOnClick.Find("OnClick");
           string levelName = "2-11";
           var pos = GlobalData.LevelModel.FindLevel(levelName);

           var isChangeAngle = IsChangeAngle(levelName);
           SetPos(pos.Positon,isChangeAngle);
           clickAreal.gameObject.Show();

           PointerClickListener.Get(clickAreal.gameObject).onClick = go =>
           {
               clickAreal.gameObject.Hide();
               ClickLevel(levelName);
               ShowSweep2_11_Step2();
           };
       }

       private void ShowSweep2_11_Step2()
       {
           _battleIntroductionPopup.gameObject.Show();
           
           var playTimesBtn = _battleIntroductionPopup.Find("BtnContainer/PlayTimesBtn");
           playTimesBtn.gameObject.Show();
           GuideArrow.DoAnimation(playTimesBtn);
           PointerClickListener.Get(playTimesBtn.gameObject)
               .onClick = go =>
           {
               playTimesBtn.gameObject.Hide();
                   
               GuideManager.SetStatisticsRemoteGuideStep(GuideConst.MainLineStep_OnClick_Battle2_11_Sweep);
               LevelVo vo = GlobalData.LevelModel.FindLevel("2-11");
               EventDispatcher.TriggerEvent(EventConst.EnterBattle, 5, vo);
               PopupManager.CloseLastWindow();
               
               GuideManager.SetRemoteGuideStep(GuideTypePB.MainGuide, GuideConst.MainLineStep_OnClick_Battle2_11_Sweep);
               //防止网络异常先模拟数据
               UserGuidePB userGuide = new UserGuidePB()
               {
                   GuideId = GuideConst.MainStep_MainStory2_4_Fail,
                   GuideType = GuideTypePB.MainGuide
               };
               GuideManager.UpdateRemoteGuide(userGuide);

               ClientTimer.Instance.DelayCall(() =>
               {
                   SetNpcHint(true, I18NManager.Get("Guide_MainLine_Sweep"));

                   PointerClickListener.Get(transform.gameObject).onClick = o =>
                   {
                       PopupManager.CloseAllWindow();
                       ModuleManager.Instance.DestroyAllModuleBackToCommon();
                   };
               }, 1.6f);
           };
       }


       /// <summary>
       /// 开始功能引导
       /// </summary>
       private void StarFunctionGuide()
       {
           var isPass4_12 = GuideManager.IsPass4_12();
           var coaxSleepGuide = GuideManager.CurFunctionGuide(GuideTypePB.LoveGuideCoaxSleep);

           if (isPass4_12 && coaxSleepGuide == FunctionGuideStage.Function_CoaxSleep_OneStage)
           {
               Level4_12End();
           }
       }


       private void Level4_12End()
       {
           GuideManager.GuideType = true;
           GoToNextGuideStage(I18NManager.Get("Guide_MainLine4_12End"));
           gameObject.Show();
           
       }
     
       private void HideMask(bool guideType)
        {
            Debug.LogError("Cur GuideType===>"+guideType);
            transform.GetComponent<Empty4Raycast>().enabled = guideType;
        }


        private void SetNpcHint(bool isBottomNpc, string msg)
        {
            if (isBottomNpc)
            {
                _bottomNpc.gameObject.Show();
                _bottomNpcTxt.text = msg;
            }
            else
            {
                _topNpc.gameObject.Show();
                _topNpcTxt.text = msg;
            }
        }


        private void Level1_1ToLevel1_3Stage()
        {
            LevelVo level1_1 = GlobalData.LevelModel.FindLevel("1-1");
            LevelVo level1_2 = GlobalData.LevelModel.FindLevel("1-2");
            LevelVo level1_3 = GlobalData.LevelModel.FindLevel("1-3");

            if (!level1_1.IsPass)
            {
               
               SetNpcHint(true, I18NManager.Get("Guide_MainLine1_1_Star"));
               OnClickLevel("1-1");
            }
            else if(!level1_2.IsPass)
            {
                
                 GuideManager.SetPassLevelStep("1-1");
                SetNpcHint(true, I18NManager.Get("Guide_MainLine1_2_Star"));
                OnClickLevel("1-2"); 
            }
            else if(!level1_3.IsPass)
            {
                 GuideManager.SetPassLevelStep("1-2");
                OnClickLevel("1-3");        
            }
            else if(level1_3.IsPass)
            {
                 GuideManager.SetPassLevelStep("1-3");               
                Level1_3End();
            }                       
        }


        private void Level1_4ToLevel2_3Stage()
        {
            var levelModel = GlobalData.LevelModel;
            LevelVo level1_4 = levelModel.FindLevel("1-4");
            LevelVo level1_5 = levelModel.FindLevel("1-5");
            LevelVo level1_6 = levelModel.FindLevel("1-6");
            LevelVo level1_7 = levelModel.FindLevel("1-7");
            LevelVo level1_8 = levelModel.FindLevel("1-8");
            LevelVo level1_9 = levelModel.FindLevel("1-9");
            LevelVo level2_1 = levelModel.FindLevel("2-1");
            LevelVo level2_2 = levelModel.FindLevel("2-2");
            LevelVo level2_3 = levelModel.FindLevel("2-3");
            LevelVo level2_4 = levelModel.FindLevel("2-4");
            
            if (!level1_4.IsPass)
            {
                Click1_4("1-4");                
            }
            else if(!level1_5.IsPass)
            {
                SetNpcHint(true, I18NManager.Get("Guide_MainLine1_5_Star"));
                 GuideManager.SetPassLevelStep("1-4");
                OnClickLevel("1-5");
            }
            else if(!level1_6.IsPass)
            {
                 GuideManager.SetPassLevelStep("1-5");    
                OnClickLevel("1-6");
            }
            else if(!level1_7.IsPass)
            {
                 GuideManager.SetPassLevelStep("1-6");    
                OnClickLevel("1-7");
            }
            else if(!level1_8.IsPass)
            {
                 GuideManager.SetPassLevelStep("1-7");    
                OnClickLevel("1-8");
            }
            else if(!level1_9.IsPass)
            {
                 GuideManager.SetPassLevelStep("1-8");    
                OnClickLevel("1-9");
            }
            else if (level1_9.IsPass && level2_1.IsPass == false)
            {
                //引导点击下一章
                 GuideManager.SetPassLevelStep("1-9");

                var curStage = GuideManager.CurStage();
                if (curStage == GuideStage.MainLineStep_Stroy1_9_Over)
                {
                    Level1_9End();
                }
                else if(levelModel.ActiveLevel.ChapterGroup == 1 && level2_1.IsPass == false)
                {
                    OnClickNextChapter();
                }
                else
                {
                    //开始引导第二章
                    OnClickLevel("2-1");
                }
            }
            else if(level2_1.IsPass && level2_2.IsPass == false)
            {
                OnClickLevel("2-2");
            }
            else if(level2_2.IsPass && level2_3.IsPass == false)
            {
                OnClickLevel("2-3");
            }
            else if(level2_3.IsPass && level2_4.IsPass == false)
            {
                OnClickLevel("2-4");
            }
        }

        private void Level2_4ToLevel2_9Stage()
        {
            var levelModel = GlobalData.LevelModel;
            LevelVo level2_4 = levelModel.FindLevel("2-4");
            LevelVo level2_5 = levelModel.FindLevel("2-5");
            LevelVo level2_6 = levelModel.FindLevel("2-6");
            LevelVo level2_7 = levelModel.FindLevel("2-7");
            LevelVo level2_8 = levelModel.FindLevel("2-8");
            LevelVo level2_9 = levelModel.FindLevel("2-9");

            if (!level2_4.IsPass)
            {
                OnClickLevel("2-4");
            }
            else if (!level2_5.IsPass)
            {
                OnClickLevel("2-5");
            }
            else if (!level2_6.IsPass)
            {
                OnClickLevel("2-6");
            } 
            else if (!level2_7.IsPass)
            {
                OnClickLevel("2-7");
            }
            else if (!level2_8.IsPass)
            {
                OnClickLevel("2-8");
            }
            else if(!level2_9.IsPass)
            {
                OnClickLevel("2-9");
            }
        }

        private void OnClickNextChapter()
        {
            _calendar2.gameObject.Show();
            Transform btn = _calendar2.Find("NextChapterBtn");
            GuideArrow.DoAnimation(btn);
            PointerClickListener.Get(btn.gameObject).onClick = go =>
            {
                btn.gameObject.Hide();
                OnClickLevel("2-1");
                SendMessage(new Message(MessageConst.MODULE_MAIN_NEXTCHAPTER, Message.MessageReciverType.UnvarnishedTransmission));
            };
        }

        // private void Level1_6ToLevel1_7Stage()
        // {
        //     LevelVo level1_6 = GlobalData.LevelModel.FindLevel("1-6");
        //     LevelVo level1_7 = GlobalData.LevelModel.FindLevel("1-7");
        //
        //     if (!level1_6.IsPass)
        //     {
        //         OnClickLevel("1-6");  
        //     }
        //     else if(!level1_7.IsPass)
        //     {
        //          GuideManager.SetPassLevelStep("1-6");
        //         OnClickLevel("1-7");  
        //     }
        //     else if(level1_7.IsPass)
        //     {
        //          GuideManager.SetPassLevelStep("1-7");
        //     }
        // }
        // private void Level1_8ToLevel1_9Stage()
        // {
        //     LevelVo level1_8 = GlobalData.LevelModel.FindLevel("1-8");
        //     LevelVo level1_9 = GlobalData.LevelModel.FindLevel("1-9");
        //
        //     if (!level1_8.IsPass)
        //     {
        //         OnClickLevel("1-8");      
        //     }
        //     else if(!level1_9.IsPass)
        //     {
        //          GuideManager.SetPassLevelStep("1-8");
        //         OnClickLevel("1-9");  
        //     }
        //     else if(level1_9.IsPass)
        //     {
        //          GuideManager.SetPassLevelStep("1-9");               
        //     }
        // }
        // private void Level1_9OverStage()
        // {
        //     GuideManager.GuideType = false;
        //     SetStatisticalOnClick();
        // }
        // private void SetStatisticalOnClick()
        // {
        //     gameObject.Show();
        //      _calendarOnClick.Find("StatisticalOnClick").gameObject.Show();
        //     LevelVo level2_1 = GlobalData.LevelModel.FindLevel("2-1");
        //     LevelVo level2_2 = GlobalData.LevelModel.FindLevel("2-2");
        //     LevelVo level2_3 = GlobalData.LevelModel.FindLevel("2-3");
        //     LevelVo level2_4 = GlobalData.LevelModel.FindLevel("2-4");
        //     LevelVo level2_5 = GlobalData.LevelModel.FindLevel("2-5");
        //     LevelVo level2_6 = GlobalData.LevelModel.FindLevel("2-6");
        //     LevelVo level2_7 = GlobalData.LevelModel.FindLevel("2-7");
        //     LevelVo level2_8 = GlobalData.LevelModel.FindLevel("2-8");
        //     LevelVo level2_9 = GlobalData.LevelModel.FindLevel("2-9");
        //     LevelVo level2_10 = GlobalData.LevelModel.FindLevel("2-10");
        //     LevelVo level2_11 = GlobalData.LevelModel.FindLevel("2-11");
        //     if (!level2_1.IsPass)
        //     {
        //         StatisticalOnClick("2-1");
        //     }
        //     else if(!level2_2.IsPass)
        //     {
        //          GuideManager.SetPassLevelStep("2-1");
        //         StatisticalOnClick("2-2");
        //     }
        //     else if(!level2_3.IsPass)
        //     {
        //          GuideManager.SetPassLevelStep("2-2");
        //         StatisticalOnClick("2-3");
        //     }
        //     else if(!level2_4.IsPass)
        //     {
        //          GuideManager.SetPassLevelStep("2-3");
        //         StatisticalOnClick("2-4");
        //     }
        //     else if(!level2_5.IsPass)
        //     {
        //          GuideManager.SetPassLevelStep("2-4");
        //         StatisticalOnClick("2-5"); 
        //     }
        //     else if(!level2_6.IsPass)
        //     {
        //          GuideManager.SetPassLevelStep("2-5");
        //         StatisticalOnClick("2-6"); 
        //     }
        //     else if(!level2_7.IsPass)
        //     {
        //          GuideManager.SetPassLevelStep("2-6");
        //         StatisticalOnClick("2-7"); 
        //     }
        //     else if(!level2_8.IsPass)
        //     {
        //          GuideManager.SetPassLevelStep("2-7");
        //         StatisticalOnClick("2-8"); 
        //     }
        //     else if(!level2_9.IsPass)
        //     {
        //          GuideManager.SetPassLevelStep("2-8");
        //         StatisticalOnClick("2-9"); 
        //     }
        //     else if(!level2_10.IsPass)
        //     {
        //          GuideManager.SetPassLevelStep("2-9");
        //         StatisticalOnClick("2-10"); 
        //     }
        //     else if(!level2_11.IsPass)
        //     {
        //          GuideManager.SetPassLevelStep("2-10");
        //         StatisticalOnClick("2-11"); 
        //     }
        //
        // }

        private void Level1_3End()
        {          
            GuideManager.SetRemoteGuideStep(GuideTypePB.MainGuide,GuideConst.MainLineStep_Stroy1_3_Over);                  
            NetWorkManager.Instance.Send<ActivityRes>(CMD.ACTIVITY_ACTIVITYLIST, null,OnGetActivityRes );                    
        }
        
        private void Level1_9End()
        {          
            GuideManager.SetRemoteGuideStep(GuideTypePB.MainGuide,GuideConst.MainLineStep_Stroy1_9_Over);     
            GoToNextGuideStage(I18NManager.Get("Guide_MainLine1_9"));  
        }


        private void Level1_3EndGetAwardWindowAni()
        {                      
            var list= GuideManager.GetGuideAwardsToRule(GuideTypePB.MainGuide);           
            RewardVo vo = null;
            foreach (var t in list)
            {
                if (t.Resource== ResourcePB.Card)
                {
                    vo =new RewardVo(t);                 
                    break;
                }
            }
           
            _award.transform.GetRawImage("GetAward/Awards/1/PropImage").texture = ResourceManager.Load<Texture>(vo.IconPath);
            _award.transform.GetText("GetAward/Awards/1/Name").text = CardName(vo.Id);
            _award.gameObject.Show();
            transform.Find("Mask").gameObject.Show();
            _award.DOScale(Vector3.one,0.2F).SetEase(Ease.OutExpo).OnComplete(() =>
            {
                PointerClickListener.Get(_award.gameObject).onClick = go =>
                {                     
                    _award.DOScale(new Vector3(_samllScale, _samllScale, 1), 0.2F).SetEase(Ease.OutExpo)
                        .OnComplete (() =>
                        {         
                            transform.Find("Mask").gameObject.Hide();
                            GoToNextGuideStage(I18NManager.Get("Guide_MainLineGetFirstAward"));  
                        }) ;
                };
            });  
        }


        private string CardName(int id)
        {
            string name = string.Empty;
          
            var key = "Common_Role"+id / 1000;
            var npcName = I18NManager.Get(key);
            var cardName = GlobalData.CardModel.CardBaseDataDict[id].CardName;
            name = npcName + "•" + cardName;     
            return name;
        }
        
        private void Level1_5End()
        {               
            GoToNextGuideStage(I18NManager.Get("Guide_MainLinePhoneGuide")); 
        }

        private void Level2_4Fail()
        {          
            GoToNextGuideStage(I18NManager.Get("Guide_MainLineDrawCard"));
           // PopupManager.CloseLastWindow(); 
        }

        private void Level2_9End()
        {
            GoToNextGuideStage(I18NManager.Get("Guide_MainLine2_9End"));
        }

        private void Level2_12End()
        {
            gameObject.Show();
             GuideManager.SetPassLevelStep("2-12"); 
            GoToNextGuideStage(I18NManager.Get("Guide_MainLineLoveDiary")); 
        }
        
        private void Click1_4(string levelName)
        {                                
            var clickAreal = _calendarOnClick.Find("OnClick");
            clickAreal.gameObject.Show();              
            var pos = GlobalData.LevelModel.FindLevel(levelName);
                     
            SetPos(pos.Positon);
                        
            ClientTimer.Instance.DelayCall(() =>
            {
                PointerClickListener.Get(clickAreal.gameObject).onClick = go =>
                {
                    clickAreal.gameObject.Hide(); 
                    ClickLevel("1-4");
                   
                    Step2();
                };
                 
            }, 0.5f);                          
        }
        
        private void Step2()
        {          
            _battleIntroductionPopup.gameObject.Show();
            SetNpcHint(true,I18NManager.Get("Guide_MainLine2"));
            var startWorkBtn = _battleIntroductionPopup.Find("BtnContainer/StartWorkBtn");
            startWorkBtn.gameObject.Show();
            GuideArrow.DoAnimation(startWorkBtn);
            PointerClickListener.Get(startWorkBtn.gameObject)
                .onClick = go =>
            {
                GuideManager.SetStatisticsRemoteGuideStep(GuideConst.MainLineStep_OnClick_Battle1_4_StarBtn);
                LevelVo vo = GlobalData.LevelModel.FindLevel("1-4");
                EventDispatcher.TriggerEvent(EventConst.EnterBattle, 0, vo);
                PopupManager.CloseLastWindow();
            };
        }
              
        private void OnGetActivityRes(ActivityRes res)
        {
            GlobalData.ActivityModel.GetAllActivityRes(res);
            Level1_3EndGetAwardWindowAni();
        }

        /// <summary>
        /// 引导去下个阶段
        /// </summary>
        /// <param name="hint">提示语</param>
        private void GoToNextGuideStage(string hint)
        {
            
            GuideManager.GuideType = true;
            HideMask( GuideManager.GuideType);
            
            gameObject.Show();

            Transform backArrow = _backArrow;
            backArrow.gameObject.Show();
            
            GuideArrow.DoAnimation(backArrow);

            Transform guideView = _bottomNpc;
            guideView.GetText("DialogFrame/Text").text = hint;
            guideView.gameObject.Show();

            PointerClickListener.Get(backArrow.gameObject).onClick = go =>
            {
                guideView.gameObject.Hide();            
                ModuleManager.Instance.EnterModule(ModuleConfig.MODULE_GAME_PLAY);
            };

        }


        private void StatisticalOnClick(string levelName)
        {
            gameObject.Show();
            var clickAreal = _calendarOnClick.Find("StatisticalOnClick");
            var pos = GlobalData.LevelModel.FindLevel(levelName);
            clickAreal.GetRectTransform().anchoredPosition = pos.Positon;
            PointerClickListener.Get(clickAreal.gameObject).onClick = go =>
            {
                ClickLevel(levelName);
                clickAreal.gameObject.Hide();
                GuideManager.Hide();
            };
        }
        
        
        private void OnClickLevel(string levelName)
        {
            gameObject.Show();
            var clickAreal = _calendarOnClick.Find("OnClick");
            var pos = GlobalData.LevelModel.FindLevel(levelName);

            var isChangeAngle = IsChangeAngle(levelName);
            SetPos(pos.Positon,isChangeAngle);
            clickAreal.gameObject.Show();

            PointerClickListener.Get(clickAreal.gameObject).onClick = go =>
            {
                ClickLevel(levelName);
                clickAreal.gameObject.Hide();
                GuideManager.Hide();
            };
        }

        private bool IsChangeAngle(string levelName)
        {
            
            if (levelName=="1-9")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
 
        /// <summary>
        /// 设置主线统计记录点
        /// </summary>
        /// <param name="step"></param>
        private void SetMainLineStep(int step)
        {
            GuideManager.SetStatisticsRemoteGuideStep(step);
        }

        private void ClickLevel(string level)
        {
            SetOnClickLevelStartStep(level);
            LevelVo vo = GlobalData.LevelModel.FindLevel(level);
          
            string msg;
            if (vo.LevelType == LevelTypePB.Story)
            {
                msg = MessageConst.MODULE_MAINLINE_SHOW_STORY_VIEW;
            }
            else
            {
                msg = MessageConst.MODULE_MAINLINE_SHOW_BATTLE_VIEW;
            }

            _topNpc.gameObject.Hide();
            _bottomNpc.gameObject.Hide();
            
            SendMessage(new Message(msg, Message.MessageReciverType.UnvarnishedTransmission, vo));
        }


        private void SetOnClickLevelStartStep(string level)
        {
            int step=0;
            switch (level)
            {
              case  "1-1":
                  step = GuideConst.MainLineStep_OnClick_Stroy1_1;
                  break;
              case  "1-2":
                  step = GuideConst.MainLineStep_OnClick_Stroy1_2;
                  break;
              case "1-3":
                  step =GuideConst.MainLineStep_OnClick_Stroy1_3;
                  break;
              case "1-4":
                  step =GuideConst.MainLineStep_OnClick_Battle1_4;
                  break;
              case  "1-5":
                  step = GuideConst.MainLineStep_OnClick_Stroy1_5;
                  break;
              case "1-6":
                  step = GuideConst.MainLineStep_OnClick_Stroy1_6;
                  break;
              case "1-7":
                  step = GuideConst.MainLineStep_OnClick_Stroy1_7;
                  break;
              case "1-8":
                  step = GuideConst.MainLineStep_OnClick_Battle1_8;
                  break;
              case "1-9":
                  step = GuideConst.MainLineStep_OnClick_Stroy1_9;
                  break;              
              case "2-1":
                  step = GuideConst.MainLineStep_OnClick_2_1;
                  break;
              case "2-2":
                  step = GuideConst.MainLineStep_OnClick_2_2;
                  break;
              case "2-3":
                  step = GuideConst.MainLineStep_OnClick_2_3;
                  break;
              case "2-4":
                  step = GuideConst.MainLineStep_OnClick_2_4;
                  break;
              case "2-5":
                  step = GuideConst.MainLineStep_OnClick_2_5;
                  break;
              case "2-6":
                  step = GuideConst.MainLineStep_OnClick_2_6;
                  break;
              case "2-7":
                  step = GuideConst.MainLineStep_OnClick_2_7;
                  break;
              case "2-8":
                  step = GuideConst.MainLineStep_OnClick_2_8;
                  break;
              case "2-9":
                  step = GuideConst.MainLineStep_OnClick_2_9;
                  break;
              case "2-10":
                  step = GuideConst.MainLineStep_OnClick_2_10;
                  break;
              case "2-11":
                  step = GuideConst.MainLineStep_OnClick_2_11;
                  break;
              case "2-12":
                  step = GuideConst.MainLineStep_OnClick_2_12;
                  break;
              case "3-1":
                  step = GuideConst.MainLineStep_OnClick_3_1;
                  break;
              case "3-2":
                  step = GuideConst.MainLineStep_OnClick_3_2;
                  break;
              case "3-3":
                  step = GuideConst.MainLineStep_OnClick_3_3;
                  break;
            }

            if (step!=0)
            {
                SetMainLineStep(step);   
            }
        }
        
        private void SetPos(Vector2 clickArea1Pos,bool isChangeAngle=false)
        {
            var clickArea1 = _calendarOnClick.GetRectTransform("OnClick");
            
            clickArea1.anchoredPosition = clickArea1Pos;
            if (isChangeAngle)
            {
                var arrow = clickArea1.Find("Arrow");
                arrow.GetRectTransform().rotation =Quaternion.Euler(new Vector3(0,0,-90));
            }
            GuideArrow.DoAnimation(clickArea1);
        }
            
    }
}