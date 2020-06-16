using System;
using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Module;
using Assets.Scripts.Module.Task.Service;
using Com.Proto;
using Common;
using DataModel;
using UnityEngine;

namespace Module.Guide.ModuleView.Story
{
    public class StoryGuideController : Controller
    {
        public override void OnMessage(Message message)
        {
            string name = message.Name;
            object[] body = message.Params;
            switch (name)
            {
                case MessageConst.TO_GUIDE_BATTLE_RESULT:
                    ChallengeRes res = (ChallengeRes) body[0];
                    LevelVo level = (LevelVo) body[1];
                    HandelBattleResult(res, level);
                    break;
            }
        }

        
        
        private void HandelBattleResult(ChallengeRes res, LevelVo level)
        {
            if (res != null && level != null)
            {
                GuideManager.SetPassLevelStep(level.LevelName);
            }
            
            if(level == null)
                return;
            
            if (level.LevelId==103)
            {               
                GuideManager.SetStatisticsRemoteGuideStep(GuideConst.MainLineStep_Stroy1_3_Over_Get_First_Card);                  
                  var list= GuideManager.GetGuideAwardsToRule(GuideTypePB.MainGuide);
                  List<AwardPB> temp =new  List<AwardPB>();
                
                  foreach (var t in list)
                  {
                      if (t.Resource== ResourcePB.Card)
                      {
                           temp.Add(t);
                          break;
                      }
                  }

                  Action finish = () =>
                  {       
                    
                      SendMessage(new Message(MessageConst.CMD_MIANLINE_GUIDE_SHOW_TOPBAR_AND_BACKBTN, Message.MessageReciverType.UnvarnishedTransmission,true));                   
                  };

                  ModuleManager.Instance.EnterModule(ModuleConfig.MODULE_DRAWCARD,
                      false,false,"DrawCard_CardShow",temp,finish,false);
                 
                  ClientTimer.Instance.DelayCall(() =>
                  { 
                      ModuleManager.Instance.Remove(ModuleConfig.MODULE_STORY);
                      SendMessage(new Message(MessageConst.CMD_MIANLINE_GUIDE_SHOW_TOPBAR_AND_BACKBTN, Message.MessageReciverType.UnvarnishedTransmission,false)); 
                  }, 0.2f);
            }
            else if(level.LevelId == 109 && level.IsPass == false)
            {
                //第一次通关1-9
                GuideManager.TempState = TempState.Pass_Level1_9;
            }
        }
    }
}