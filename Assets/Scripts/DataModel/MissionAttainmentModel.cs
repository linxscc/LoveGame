using System;
using System.Collections.Generic;
using System.Net.Mime;
using Assets.Scripts.Framework.GalaSports.Core;
using Com.Proto;
using Common;
using game.main;
using UnityEngine;

namespace DataModel
{
    //任务达成Model
    public class MissionAttainmentModel : Model
    {

        
        /// <summary>
        /// 星动之约弹窗类型
        /// </summary>
        public enum  StarActivityPopUpsType
        {
           /// <summary>
           /// 不弹
           /// </summary>
           Default =0,
     
           /// <summary>
           /// 解锁恋爱剧情
           /// </summary>
           LoveDrama=1,
          
           /// <summary>
           /// 星缘升等级
           /// </summary> 
           CardLevelUp=2,
           
           /// <summary>
           /// 星缘升心
           /// </summary>
           CardHeartUp=3,
           
           /// <summary>
           /// 星缘进化
           /// </summary>
           CardEvolutionUp =4,
           
           /// <summary>
           /// 星源回忆
           /// </summary>
           StarRecall =5,
           
        }




   
        /// <summary>
        /// 星缘回忆完成次数
        /// </summary>
        private int _starRecallNum = 0;

        /// <summary>
        /// 恋爱剧情
        /// </summary>
        private int _loveDrama=0;

        private Dictionary<StarActivityPopUpsType, List<MissionRulePB>> _missionRulesRequestDic;
        
        
        public MissionAttainmentModel()
        {
            InitRequestDic();
         
            InitStarRecallNum();
        }


        private void InitRequestDic()
        {
            return;
            if (_missionRulesRequestDic==null)
            {
                 _missionRulesRequestDic =new Dictionary<StarActivityPopUpsType, List<MissionRulePB>>();
                 var rules = GlobalData.MissionModel.GetMissionRuleRes();
                 var isNew = GlobalData.MissionModel.IsNewStarActivity();
                 var typePb = isNew ? MissionTypePB.NewStarryCovenant : MissionTypePB.StarryCovenant;
                 
                 foreach (var mission in rules.MissionRules)
                 {
                     if (mission.MissionType==typePb)
                     {
                         if (mission.PopUps!=0)      
                         {
                             SetKey(mission.MissionId);  //设置要弹的任务标记
                             var type = (StarActivityPopUpsType) mission.PopUps;
                       
                             var isTypeKey = _missionRulesRequestDic.ContainsKey(type);
                             if (!isTypeKey)
                             {
                                 _missionRulesRequestDic[type] = new List<MissionRulePB>();
                             }
                     
                             if (!_missionRulesRequestDic[type].Contains(mission))
                             {
                                 
                                 _missionRulesRequestDic[type].Add(mission);
                             }                      
                         }                     
                     }
                  }                      
            }
     }



        /// <summary>
        /// 触发星缘任务窗口
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="cardData">卡等级，卡心级，卡进化</param>
        public void TriggerPopWindow(StarActivityPopUpsType type,int cardData)
        {
            
            return;
            
           var list = _missionRulesRequestDic[type];
            var starActivityOverStamp = GlobalData.MissionModel.GetStarActivityOverTimeStamp();
            var curTimeStamp = ClientTimer.Instance.GetCurrentTimeStamp();
            var openDay =  GlobalData.MissionModel.GetOpenDay();
                   
            if (curTimeStamp<starActivityOverStamp)
            {
             for (int i = list.Count - 1; i >= 0; i--)
                {
                    var missionId = list[i].MissionId;
                    var limitValue= list[i].Extra.LimitValue;
                    var days = list[i].Extra.Days;

                    if (GetValue(missionId)==0&& cardData>=limitValue&& days<=openDay)               
                    {                   
                        UpdateKey(list[i].MissionId);
                        AttainmentTips.ShowWindow(list[i]); 
                        list.RemoveAt(i);                       
                    }
                }
           }           
        }


        /// <summary>
        /// 战斗触发卡升级弹窗
        /// </summary>
        /// <param name="cardLevels"></param>
        public void BattleTriggerPopWindow(List<int> cardLevels)
        {

            return;
            var isPassLevel = GuideManager.IsPass1_9();
            if (!isPassLevel)
            {
               return; 
            }
            
            foreach (var cardLevel in cardLevels)
            {
                TriggerPopWindow(StarActivityPopUpsType.CardLevelUp, cardLevel);
            }
        }
      



        
        
        private void InitStarRecallNum()
        {
            return;
           var list = _missionRulesRequestDic[StarActivityPopUpsType.StarRecall];
           foreach (var t in list)
            {
                var missionId = t.MissionId;
                var finishNum = GlobalData.MissionModel.GetUserMissionDataByMissionId(missionId).Progress;
                _starRecallNum = (int) finishNum;            
            }          
        }

        public void AddLoveDramaNum(int num)
        {
            return;
            _loveDrama = num;           
            TriggerPopWindow(_loveDrama,StarActivityPopUpsType.LoveDrama);
        }

        public void AddStarRecallNum(int num)
        {
            return;
            _starRecallNum += num;          
            TriggerPopWindow(_starRecallNum,StarActivityPopUpsType.StarRecall);
        }
        
     
        private void TriggerPopWindow(int num,StarActivityPopUpsType type)
        {
            return;
           var list = _missionRulesRequestDic[type];
            var starActivityOverStamp = GlobalData.MissionModel.GetStarActivityOverTimeStamp();
            var curTimeStamp = ClientTimer.Instance.GetCurrentTimeStamp();
            var openDay =  GlobalData.MissionModel.GetOpenDay();
           
            if (curTimeStamp<starActivityOverStamp)
            {

                for (int i = list.Count - 1; i >= 0; i--)
                {
                    var missionId = list[i].MissionId;
                    var limitValue= list[i].Extra.LimitValue;
                    var days = list[i].Extra.Days;
                   if (GetValue(missionId)==0 &&num>=limitValue&& days<=openDay)       
                    {                   
                        UpdateKey(list[i].MissionId);
                        AttainmentTips.ShowWindow(list[i]); 
                        list.RemoveAt(i);                     
                    }

                }
            }  
        }
        

       
        
        

        
        /// <summary>
        /// 标记弹窗Key 0是没弹过的
        /// </summary>
        /// <param name="missionId">任务Id</param>
        private void SetKey(int missionId)
        {

            var version = AppConfig.Instance.version.ToString();
            var playerId = GlobalData.PlayerModel.PlayerVo.UserId.ToString();
            var id = missionId.ToString();

            var key = version + playerId + id;

            var isKey = PlayerPrefs.HasKey(key);
            if (isKey==false)
            {
                PlayerPrefs.SetInt(key,0); 
            }

        }


        /// <summary>
        /// 更新Key  Set为1；就是标记已经弹过了
        /// </summary>
        /// <param name="missionId">任务Id</param>
        private void UpdateKey(int missionId)
        {
            var version = AppConfig.Instance.version.ToString();
            var playerId = GlobalData.PlayerModel.PlayerVo.UserId.ToString();
            var key = version + playerId + missionId;
            PlayerPrefs.SetInt(key,1);
        }

        /// <summary>
        /// 获取标记Value
        /// </summary>
        /// <param name="missionId">任务ID</param>
        /// <returns></returns>
        private int GetValue(int missionId)
        {
            var version = AppConfig.Instance.version.ToString();
            var playerId = GlobalData.PlayerModel.PlayerVo.UserId.ToString();
            var key = version + playerId + missionId;
            return PlayerPrefs.GetInt(key);
        }
        


    }
}
