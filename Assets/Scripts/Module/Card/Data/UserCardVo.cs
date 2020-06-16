using System;
using System.Collections.Generic;
using Com.Proto;
using DataModel;
using UnityEngine;

namespace game.main
{
    /// <summary>
    /// 玩家卡牌数据
    /// </summary>
    public class UserCardVo : IComparable<UserCardVo>
    {
        public int Level;

        public int Star;

        private int _currentLevelExp;

        //用户使用的卡面
        public EvolutionPB UseEvo;
        
        public int Singing   
        {
            get
            {
                if (Evolution>=EvolutionPB.Evo1)
                {
                    return CardVo.Singing + CurLevelInfo.SingingAdditon+CurStarInfo.SingingAdditon+CurEvolutionInfo.SingingAdditon;
                }
                return CardVo.Singing + CurLevelInfo.SingingAdditon+CurStarInfo.SingingAdditon;
            }
        }
        public int Dancing  
        {
            get
            {
                if (Evolution>=EvolutionPB.Evo1)
                {
                    return CardVo.Dancing + CurLevelInfo.DancingAdditon+CurStarInfo.DancingAdditon+CurEvolutionInfo.DancingAdditon;
                }
                return CardVo.Dancing + CurLevelInfo.DancingAdditon+CurStarInfo.DancingAdditon;
            }
        }
        
        /// <summary>
        /// 原创
        /// </summary>
        public int Original
        {
            get
            {
                if (Evolution>=EvolutionPB.Evo1)
                {
                    return CardVo.Original + CurLevelInfo.OriginalAdditon+CurStarInfo.OriginalAdditon+CurEvolutionInfo.OriginalAdditon;
                }
                return CardVo.Original + CurLevelInfo.OriginalAdditon+CurStarInfo.OriginalAdditon;
            }//+CurEvolutionInfo.OriginalAdditon
        }
        
        /// <summary>
        /// 人气
        /// </summary>
        public int Popularity
        {
            get
            {
                if (Evolution>=EvolutionPB.Evo1)
                {
                    return CardVo.Popularity + CurLevelInfo.PopularityAdditon+CurStarInfo.PopularityAdditon+CurEvolutionInfo.PopularityAdditon;
                }
                return CardVo.Popularity + CurLevelInfo.PopularityAdditon+CurStarInfo.PopularityAdditon;
            }
        }
        
        /// <summary>
        /// 魅力
        /// </summary>
        public int Glamour
        {
            get
            {
                if (Evolution>=EvolutionPB.Evo1)
                {
                    return CardVo.Glamour + CurLevelInfo.GlamourAdditon+CurStarInfo.GlamourAdditon+CurEvolutionInfo.GlamourAdditon;
                }
                return CardVo.Glamour + CurLevelInfo.GlamourAdditon+CurStarInfo.GlamourAdditon;
            }
        }
        
        /// <summary>
        /// 毅力
        /// </summary>
        public int Willpower
        {
            get
            {
                if (Evolution>=EvolutionPB.Evo1)
                {
                    return CardVo.Willpower + CurLevelInfo.WillpowerAdditon+CurStarInfo.WillpowerAdditon+CurEvolutionInfo.WillpowerAdditon;
                }
                return CardVo.Willpower + CurLevelInfo.WillpowerAdditon+CurStarInfo.WillpowerAdditon;
            }
        }

        public int TotalAblity()
        {
            return Singing + Dancing + Original + Popularity + Glamour + Willpower;
        }
        /// <summary>
        /// 总经验
        /// </summary>
        public int Exp;
        
        /// <summary>
        /// 当前等级的经验
        /// </summary>
        public int CurrentLevelExp
        {
            get { return _currentLevelExp; }
            set
            {
                CardLevelRulePB rule = GlobalData.CardModel.GetCardLevelRule(Level-1, CardVo.Credit);
                if (rule != null)
                    _currentLevelExp = value - rule.Exp;
                else
                    _currentLevelExp = value;
            }
        }

        private int _needExp = -1;
        public int NeedExp
        {
            get
            {
                CardLevelRulePB prevLevel = GlobalData.CardModel.GetCardLevelRule(Level-1, CardVo.Credit);
                CardLevelRulePB currentLevel = GlobalData.CardModel.GetCardLevelRule(Level, CardVo.Credit);
                if (prevLevel == null)
                {
                    _needExp = currentLevel.Exp;
                }
                else
                {
                    _needExp = currentLevel.Exp - prevLevel.Exp;
                }
                return _needExp;
            }
            set { NeedExp = value; }

        }

        //用户当前的进化阶段
        public EvolutionPB Evolution;

        public int MaxStars
        {
            get
            {
                return GlobalData.CardModel.MaxStars(CardVo.Credit);
            }
        } 

        /// <summary>
        /// 卡牌数量
        /// </summary>
        public int Num;

        public int CardId;

        public CardVo CardVo;

        /// <summary>
        /// 回忆次数
        /// </summary>
        public int RecollectionCount; 

        public CardStarUpRulePB CardStarUpRulePb
        {
            get { return GlobalData.CardModel.GetCardStarUpRule(CardId, (StarPB) Star); }
        }

        
        
        /// <summary>
        /// 升星金币花费
        /// </summary>
        public int UpStarCost {

            get
            {
                CardStarUpRulePB pb = GlobalData.CardModel.GetCardStarUpRule(CardId, (StarPB) Star);
                if (pb == null)
                {
                    return 0;
                }
                return pb.Gold;
            }
        }

        private CardAdditionVo _curStarInfo;
        private CardAdditionVo _curevoInfo;
        private CardAdditionVo _additionEvoInfo;
        private CardAdditionVo _curLevelInfo;
        
        
        private CardAdditionVo _nextStarInfo;
        private CardAdditionVo _evolutionInfo;
        private CardAdditionVo _nextLevelInfo;

        private List<UpgradeStarRequireVo> _upgradeStarRequire;

        public int EvolutionRequireLevel => GlobalData.CardModel.MaxLevel(CardVo.Credit);

        public SignatureRulePB SignatureCard
        {
            get { return GlobalData.CardModel.GetCardSignature(CardId); }
        }

        /// <summary>
        /// 升星需要的等级
        /// </summary>
        public int UpgradeRequireLevel
        {
            get
            {
                CardStarRulePB rule = GlobalData.CardModel.GetCardStarRule(CardVo.Credit, Star);
                if (rule == null)
                    return 0;
                return rule.MaxLevel;
            }
        }


        //是否展示进化后的背景图
        public bool UserNeedShowEvoCard()
        {

            //此处很有可能出现BUG    
            //BUG,请看切换卡面的请求逻辑
            return UseEvo >= EvolutionPB.Evo1 && Evolution >= EvolutionPB.Evo2;
        }
        
        
        public int EvolutionRequirePropId()
        {
            switch (CardVo.Player)
            {
                case PlayerPB.TangYiChen:
                    return PropConst.CardEvolutionPropTang;
                    break;
                case PlayerPB.QinYuZhe:
                    return PropConst.CardEvolutionPropQin;
                    break;
                case PlayerPB.YanJi:
                    return PropConst.CardEvolutionPropYan;
                    break;
                case PlayerPB.ChiYu:
                    return PropConst.CardEvolutionPropChi;
                    break;
            }
            return -1;
        }


        /// <summary>
        /// 获取升星需要的道具
        /// </summary>
        public List<UpgradeStarRequireVo> GetUpgradeStarProps
        {
            get
            {
                _upgradeStarRequire = new List<UpgradeStarRequireVo>();
                CardStarUpRulePB rule = GlobalData.CardModel.GetCardStarUpRule(CardId, (StarPB) Star);

                if (rule == null)
                    return null;

                foreach (KeyValuePair<int, int> pair in rule.Consume)
                {
                    UpgradeStarRequireVo vo = new UpgradeStarRequireVo();
                    vo.PropId = pair.Key;
                    vo.NeedNum = pair.Value;
                    var propbase = GlobalData.PropModel.GetPropBase(vo.PropId);
                    if (propbase!=null)
                    {
                        vo.PropName = propbase.Name;
                    }
                    else
                    {
                        Debug.Log("no prop?"+vo.PropId);
                        return null;
                    }
                    

                    UserPropVo userProp = GlobalData.PropModel.GetUserProp(vo.PropId);
                    vo.CurrentNum = 0;
                    if (userProp != null)
                        vo.CurrentNum = userProp.Num;
                    _upgradeStarRequire.Add(vo);
                }


                return _upgradeStarRequire;
            }
        }


        private bool showUpgradeStarRedpoint ;

        public bool ShowUpgradeStarRedpoint
        {

            get
            {
                showUpgradeStarRedpoint = true;
                List<UpgradeStarRequireVo> upgradeStarProps = GetUpgradeStarProps;

                if (upgradeStarProps!=null)
                {
                    foreach (var v in upgradeStarProps)
                    {
                        if (v.CurrentNum >= v.NeedNum) continue;
                        showUpgradeStarRedpoint = false; 
                        break;
                    }
                }
                else
                {
//                    Debug.LogError(CardId);
                    showUpgradeStarRedpoint = false;
                }

                return showUpgradeStarRedpoint&&Level==UpgradeRequireLevel;
            }
        }

        public bool ShowEvolutionRedPoint
        {
            get
            {
                bool isshow = false;
                
                //首先要拿到当前等级能进化的阶段
                int maxevoTimes = CardVo.GetMaxEvoTimes();
                CardEvoRulePB fitStagepb = null;
                for (int i = 1; i < maxevoTimes+1; i++)
                {
                    var cardevorulepb = GlobalData.CardModel.GetCardEvoRule(CardVo.Credit, CardVo.Player,(EvolutionPB)(i));
                    if (Evolution==cardevorulepb.Evo-1&&Star>=(int)cardevorulepb.StarNeed)
                    {
                        fitStagepb = cardevorulepb;
                        break;
                    } 
                    
                }
                int reduceCardNum = 0;
                if (fitStagepb==null)
                {
                    return false;

                }
                
                if (Num-1>=fitStagepb.UseCardNum)
                {
                    reduceCardNum = fitStagepb.UseCardNum;
                }
                else
                {
                    reduceCardNum = Num - 1;
                }
                var costReduceNum = GlobalData.CardModel.GetCardResolveRule(CardVo.Credit, CardVo.Player).EvoResolve[10000 + (int) CardVo.Player];
                
                bool isCrystalSatisfy = true;
                bool isPropSatisfy = true;
                foreach (var v in fitStagepb.Consume)
                {
                    var propitem = GlobalData.PropModel.GetUserProp(v.Key);
                    if (propitem.ItemId>=10001&&propitem.ItemId<=10004)
                    {
                        isCrystalSatisfy = propitem.Num >= v.Value - costReduceNum * reduceCardNum;
                        if (!isCrystalSatisfy)
                        {
                             break;
                        }                       
                    }
                    else
                    {
                        isPropSatisfy = propitem.Num >= v.Value;
                        if (!isPropSatisfy)
                        {
                            break;
                        }
                    }                   
                }
                isshow = isCrystalSatisfy && isPropSatisfy;
                //同时满足道具和结晶的条件
                return isshow;
            }
        }

        public bool ShowCardDetailRedPoint
        {
            get { return ShowUpgradeStarRedpoint || ShowEvolutionRedPoint; }//||ShowLovePoint
        }

        public UserAppointmentVo CardAppointmentVo
        {
            get { return GlobalData.AppointmentData.GetCardAppointmentVo(CardId); } 
           
        }

        public AppointmentRuleVo CardAppointmentRuleVo
        {
            get { return GlobalData.AppointmentData.GetCardAppointmentRuleVo(CardId); }
        }

        public bool ShowLovePoint
        {
            get
            {
                var appointmentvo = GlobalData.AppointmentData.GetCardAppointmentVo(CardId);
                if (appointmentvo==null)
                {
                     //Debug.LogError(appointmentvo);
                    if (CardVo.Credit!=CreditPB.R)
                    {
                        return true;
                    }
                    
                    return false;
                }
                
                return GlobalData.AppointmentData.NeedSetRedPoint(appointmentvo, CardId);
            }
        }
        

        public CardAdditionVo CurStarInfo
        {
            get
            {
                //Debug.LogError(Star);
                _curStarInfo = new CardAdditionVo();
                CardStarRulePB pb = GlobalData.CardModel.GetCardStarRule(CardVo.Credit, Star);//
                if (pb == null)
                    return _curStarInfo;
                _curStarInfo.AssignPower(pb.Power,CardVo);
                
                return _curStarInfo;
            }
        }
        
        public CardAdditionVo NextStarInfo
        {
            get
            {
                //Debug.LogError(Star+1);
                _nextStarInfo = new CardAdditionVo();
                CardStarRulePB pb = GlobalData.CardModel.GetCardStarRule(CardVo.Credit, Star+ 1 );//
                if (pb == null)
                    return _nextStarInfo;
                _nextStarInfo.AssignPower(pb.Power,CardVo);
                
                return _nextStarInfo;
            }
        }

        public CardAdditionVo CurLevelInfo
        {
            get
            {
                _curLevelInfo=new CardAdditionVo();
                CardLevelRulePB pb = GlobalData.CardModel.GetCardLevelRule(Level, CardVo.Credit);
                if (pb == null)
                    return _curLevelInfo;
                _curLevelInfo.AssignPower(pb.Power,CardVo);

                return _curLevelInfo;
            }
        }
        
        public CardAdditionVo NextLevelInfo
        {
            get
            {
                _nextLevelInfo = new CardAdditionVo();
                CardLevelRulePB pb = GlobalData.CardModel.GetCardLevelRule(Level + 1, CardVo.Credit);
                if (pb == null)
                    return _nextLevelInfo;
                _nextLevelInfo.AssignPower(pb.Power,CardVo);

                return _nextLevelInfo;
            }
        }

        
        public CardAdditionVo CurEvolutionInfo
        {
            get
            {
                _curevoInfo = new CardAdditionVo();
                CardEvoRulePB pb = GlobalData.CardModel.GetCardEvoRule(CardVo.Credit,CardVo.Player,Evolution);//GlobalData.CardModel.GetCardEvoRule(CardVo.Credit)这个应该是有BUG的！
                if (pb == null)
                    return _curevoInfo;
                _curevoInfo.AssignPower(pb.Power,CardVo);

                return _curevoInfo;
            }
        }

        public CardAdditionVo EvoInfoAddition
        {
            get
            {
                _additionEvoInfo=new CardAdditionVo();
                if ((int)Evolution<CardVo.GetMaxEvoTimes())
                {
                    CardEvoRulePB pb = GlobalData.CardModel.GetCardEvoRule(CardVo.Credit,CardVo.Player,Evolution+1);//GlobalData.CardModel.GetCardEvoRule(CardVo.Credit)这个应该是有BUG的！
                    if (pb == null)
                        return _additionEvoInfo;
                    _additionEvoInfo.AssignPower(pb.Power,CardVo);

                    return _additionEvoInfo;
                }

                return CurEvolutionInfo;

            }
            
        }
        
        
//        public CardAdditionVo EvolutionInfo
//        {
//            get
//            {
//                _evolutionInfo = new CardAdditionVo();
//                CardEvoRulePB pb = GlobalData.CardModel.GetCardEvoRule(CardVo.Credit);
//                if (pb == null)
//                    return _evolutionInfo;
//                _evolutionInfo.AssignPower(pb.Power,CardVo);
//
//                return _evolutionInfo;
//            }
//        }

        public int CompareTo(UserCardVo other)
        {
            int result = 0;
            if (other.CardVo.Credit.CompareTo(CardVo.Credit) != 0)
            {
                result = -other.CardVo.Credit.CompareTo(CardVo.Credit);
            }
//            else if (other.Evolution.CompareTo(Evolution) != 0)
//            {
//                result = other.Evolution.CompareTo(Evolution);
//            }
//            else if(other.EvolutionRequirePropId().CompareTo(EvolutionRequirePropId())!=0)
//            {
//                result = -other.EvolutionRequirePropId().CompareTo(EvolutionRequirePropId());
//            }
            else if (other.Star.CompareTo(Star) != 0)
            {
                result = other.Star.CompareTo(Star);
            }
            else if (other.Level.CompareTo(Level) != 0)
            {
                result = other.Level.CompareTo(Level);
            }      
            return result;
        }
    }
}