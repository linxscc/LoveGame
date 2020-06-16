using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Core.Events;
using Assets.Scripts.Module.Framework.Utils;
using Com.Proto;
using Common;
using Google.Protobuf.Collections;

namespace DataModel
{
    public class RandomEventModel : Model
    {
        public Dictionary<long, long> ClickBuyDict;
        
        public int EnergyUsed;

        public int RecollectionTimes;

        public int RecollectionResetTimes;

        public int GoldDrawCardTimes;

        public int GemDrawCardTimes;

        private RepeatedField<TriggerGiftRulePB> _triggerGiftsRule;
        private RepeatedField<RmbMallRulePB> _rmbMallRules;
        private MapField<int, int> _triggerMap;

        public bool SsrGet = false;

        private RepeatedField<UserTriggerGiftPB> _oldGift;
        private List<UserTriggerGiftPB> _newGifts;

        public List<TriggerGiftVo> GiftList;
        
        /// <summary>
        /// 触发的礼包数量，免费和他的收费礼包算同一个
        /// </summary>
        public int GiftCount;

        public RandomEventModel()
        {
            ClickBuyDict = new Dictionary<long, long>();
        }

        public void AddEnergy(int count)
        {
            EnergyUsed += count;
        }

        public void AddRecollectionTimes(int count)
        {
            RecollectionTimes += count;
        }

        public void AddRecollectionResetTimes(int count)
        {
            RecollectionResetTimes += count;
        }

        public void AddDrawCardTimes(DrawTypePB drawType)
        {
            switch (drawType)
            {
                case DrawTypePB.ByGem:
                    GemDrawCardTimes += 1;
                    break;
                case DrawTypePB.ByGem10:
                    GemDrawCardTimes += 10;
                    break;
                case DrawTypePB.ByGold:
                    GoldDrawCardTimes += 1;
                    break;
                case DrawTypePB.ByGold10:
                    GoldDrawCardTimes += 10;
                    break;
            }
        }

        public void InitRule(RepeatedField<RmbMallRulePB> rmbMallRules,
            RepeatedField<TriggerGiftRulePB> triggerGiftRule)
        {
            _rmbMallRules = rmbMallRules;
            _triggerGiftsRule = triggerGiftRule;
        }

        public void InitData(RepeatedField<UserTriggerGiftDataPB> userTriggerGiftData,
            RepeatedField<UserTriggerGiftPB> userTriggerGiftPb, bool showNewGiftWindow)
        {
            SsrGet = false;

            EnergyUsed = 0;
            RecollectionTimes = 0;
            RecollectionResetTimes = 0;
            GoldDrawCardTimes = 0;
            GemDrawCardTimes = 0;

            //找出新触发的礼包
            FilterNewGift(_oldGift, userTriggerGiftPb, showNewGiftWindow);

            _oldGift = userTriggerGiftPb;

            DateTime today = DateUtil.GetDataTime(ClientTimer.Instance.GetCurrentTimeStamp());

            if (today.Hour < 6)
            {
                today = today.AddDays(-1);
            }

            //找到今天的触发情况
            UserTriggerGiftDataPB currentTrigger = null;
            foreach (var pb in userTriggerGiftData)
            {
                DateTime targetDay = DateUtil.GetDataTime(pb.RecordDay);
                if (targetDay.Year == today.Year
                    && targetDay.Month == today.Month
                    && targetDay.Day == today.Day)
                {
                    currentTrigger = pb;
                    _triggerMap = pb.TriggerNum;
                    break;
                }
            }

            if (currentTrigger != null)
            {
                foreach (var trigger in currentTrigger.TriggerGoal)
                {
                    TriggerGoalTypePB type = (TriggerGoalTypePB) trigger.Key;

                    switch (type)
                    {
                        case TriggerGoalTypePB.DrawCredit:
                            break;
                        case TriggerGoalTypePB.ExpendPower:
                            EnergyUsed = trigger.Value;
                            break;
                        case TriggerGoalTypePB.CurrentOwer:
                            break;
                        case TriggerGoalTypePB.CardMemoriesNum:
                            RecollectionTimes = trigger.Value;
                            break;
                        case TriggerGoalTypePB.ResetCardMemoriesNum:
                            RecollectionResetTimes = trigger.Value;
                            break;
                        case TriggerGoalTypePB.StarCardDrawNum:
                            GemDrawCardTimes = trigger.Value;
                            break;
                        case TriggerGoalTypePB.GemCredit:
                            break;
                        case TriggerGoalTypePB.GoldDrawNum:
                            GoldDrawCardTimes = trigger.Value;
                            break;
                        case TriggerGoalTypePB.GoldNum:
                            break;
                        case TriggerGoalTypePB.DepartmentLevel:
                            break;
                    }
                }
            }
            
           
        }

        private void FilterNewGift(RepeatedField<UserTriggerGiftPB> oldTriggerGiftPb,
            RepeatedField<UserTriggerGiftPB> newTriggerGiftPb, bool showNewGiftWindow)
        {
            GiftCount = 0;
            _newGifts = new List<UserTriggerGiftPB>();
            foreach (var giftPb in newTriggerGiftPb)
            {
                bool hasGift = false;
                if (_oldGift != null)
                {
                    foreach (var oldPb in _oldGift)
                    {
                        if (giftPb.Id == oldPb.Id)
                        {
                            hasGift = true;
                            break;
                        }
                    }
                }

                if (hasGift == false)
                    _newGifts.Add(giftPb);
            }

            _oldGift = newTriggerGiftPb;

            GiftList = new List<TriggerGiftVo>();

            foreach (var giftPb in newTriggerGiftPb)
            {
                GiftCount++;
                
                RmbMallRulePB pb = GetMallPb(giftPb.MallId);
                
                bool hasFree = false;
                foreach (var rule in _triggerGiftsRule)
                {
                    if (rule.MallId == giftPb.MallId && giftPb.FreeType == 0 && rule.FreeAward != null && rule.FreeAward.Count > 0)
                    {
                        TriggerGiftVo vo1 = new TriggerGiftVo();
                        vo1.Id = giftPb.Id;
                        vo1.IsFree = true;
                        vo1.MallId = giftPb.MallId;
                        vo1.FreeType = giftPb.FreeType;
                        vo1.MaturityTime = giftPb.MaturityTime;
                        vo1.Awards = rule.FreeAward;
                        vo1.Rule = pb;
                        GiftList.Add(vo1);
                        hasFree = true;
                        break;
                    }
                }

                if (giftPb.FreeType == 1 || hasFree == false)
                {
                    //免费礼包领取之后才会显示收费礼包
                    TriggerGiftVo vo = new TriggerGiftVo();
                    vo.Id = giftPb.Id;
                    vo.MallId = giftPb.MallId;
                    vo.FreeType = giftPb.FreeType;
                    vo.MaturityTime = giftPb.MaturityTime;
                    vo.IsFree = false;
                    
                    vo.Awards = pb.Award;
                    vo.Rule = pb;
                
                    GiftList.Add(vo);
                }
            }

            
            GiftList.Sort();

            if (showNewGiftWindow)
            {
                RandomEventManager.ShowNewTriggerGift();
            }
            
            EventDispatcher.TriggerEvent(EventConst.OnTriggerGiftChange);
        }

        private RmbMallRulePB GetMallPb(int mallId)
        {
            foreach (var pb in _rmbMallRules)
            {
                if (pb.MallId == mallId)
                {
                    return pb;
                }
            }

            return null;
        }


        public bool CheckTrigger(params int[] mallIds)
        {
            foreach (var mallId in mallIds)
            {
                if (CheckTrigger(mallId))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 按照礼包ID去检查触发
        /// </summary>
        /// <param name="mallId">礼包ID</param>
        /// <returns></returns>
        public bool CheckTrigger(int mallId)
        {
            TriggerGiftRulePB rule = null;
            foreach (var rulePb in _triggerGiftsRule)
            {
                if (rulePb.MallId == mallId)
                {
                    rule = rulePb;
                    break;
                }
            }

            if (rule == null)
                return false;

            if (_triggerMap != null && _triggerMap.ContainsKey(mallId))
            {
                if (_triggerMap[mallId] >= rule.TriggerLimit)
                    return false;
            }

            if (mallId == 7007 || mallId == 7008 || mallId == 7009)
                return CheckTrigger7007_7009(mallId);

            if (mallId >= 7010)
                return true;

            
            List<bool> orList = new List<bool>();

            //rule.TriggerGoalLists 数组元素满足逻辑与
            foreach (var goal in rule.TriggerGoalLists)
            {
                List<bool> andList = new List<bool>();
                //goal.TriggerGoal 数组元素满足逻辑或
                foreach (var trigger in goal.TriggerGoal)
                {
                    bool isGreater = trigger.LssOrGtr == 0;
                    bool result2 = false;

                    switch (trigger.TriggerGoalType)
                    {
                        case TriggerGoalTypePB.DrawCredit:
                            break;
                        case TriggerGoalTypePB.ExpendPower:
                            result2 = CompareValue(isGreater, EnergyUsed, trigger.Finish);
                            break;
                        case TriggerGoalTypePB.CurrentOwer:
                            result2 = CompareValue(isGreater, GlobalData.PlayerModel.PlayerVo.Energy, trigger.Finish);
                            break;
                        case TriggerGoalTypePB.CardMemoriesNum:
                            result2 = CompareValue(isGreater, RecollectionTimes, trigger.Finish);
                            break;
                        case TriggerGoalTypePB.ResetCardMemoriesNum:
                            result2 = CompareValue(isGreater, RecollectionResetTimes, trigger.Finish);
                            break;
                        case TriggerGoalTypePB.StarCardDrawNum:
                            result2 = CompareValue(isGreater, GemDrawCardTimes, trigger.Finish);
                            break;
                        case TriggerGoalTypePB.GemCredit:
                            result2 = CompareValue(isGreater,
                                GlobalData.PropModel.GetUserProp(PropConst.DrawCardByGem).Num, trigger.Finish);
                            break;
                        case TriggerGoalTypePB.GoldDrawNum:
                            result2 = CompareValue(isGreater, GoldDrawCardTimes, trigger.Finish);
                            break;
                        case TriggerGoalTypePB.GoldNum:
                            result2 = CompareValue(isGreater, (int) GlobalData.PlayerModel.PlayerVo.Gold,
                                trigger.Finish);
                            break;
                        case TriggerGoalTypePB.DepartmentLevel:
                            result2 = CompareValue(isGreater, (int) GlobalData.PlayerModel.PlayerVo.Level,
                                trigger.Finish);
                            break;
                    }

                    andList.Add(result2);
                }

                bool result1 = true;
                foreach (var andVal in andList)
                {
                    if (andVal == false)
                    {
                        result1 = false;
                        break;
                    }
                }

                orList.Add(result1);
            }

            bool result = false;
            foreach (var orVal in orList)
            {
                if (orVal)
                {
                    result = true;
                    break;
                }
            }

            return result;
        }

        private bool CheckTrigger7007_7009(int mallId)
        {
            int level = GlobalData.PlayerModel.PlayerVo.Level;
            int[] arr = new[] {6, 15, 21, 26};
            List<int> giftIds = new List<int>() {7007, 7008, 7009};

            int index = giftIds.IndexOf(mallId);
            return level >= arr[index] && level < arr[index + 1];
        }

        private bool CompareValue(bool isGreater, int value, int finishValue)
        {
            return isGreater ? value > finishValue : value < finishValue;
        }

        public int GetNewGiftIndex()
        {
            if (_newGifts == null || _newGifts.Count == 0)
                return -1;

            for (int i = 0; i < GiftList.Count; i++)
            {
                if (GiftList[i].Id == _newGifts[0].Id)
                {
                    return i;
                }
            }

            return -1;
        }

        public void UpdateDate(UserTriggerGiftPB userTriggerGift)
        {
            if(_oldGift == null)
                return;
            
            RepeatedField<UserTriggerGiftPB> triggerGiftList = new RepeatedField<UserTriggerGiftPB>();
            foreach (var giftPb in _oldGift)
            {
                if (giftPb.Id == userTriggerGift.Id)
                {
                    triggerGiftList.Add(userTriggerGift);
                }
                else
                {
                    triggerGiftList.Add(giftPb);
                }
            }
            
            FilterNewGift(_oldGift, triggerGiftList, false);
        }

        public void Delete(RepeatedField<long> arr)
        {
            if(_oldGift == null)
                return;
            
            RepeatedField<UserTriggerGiftPB> triggerGiftList = new RepeatedField<UserTriggerGiftPB>();
            
            foreach (var giftPb in _oldGift)
            {
                bool isAdd = true;
                foreach (var id in arr)
                {
                    if (giftPb.Id == id)
                    {
                        isAdd = false;
                        break;
                    }
                }
                if(isAdd)
                    triggerGiftList.Add(giftPb);
            }
            
            FilterNewGift(_oldGift, triggerGiftList, false);
           
        }

        public void FilterTimeOut()
        {
            long time = ClientTimer.Instance.GetCurrentTimeStamp();
            for (int i = GiftList.Count - 1; i >= 0; i--)
            {
                if(GiftList[i].MaturityTime <= time)
                    GiftList.RemoveAt(i);
            }
        }
    }
}