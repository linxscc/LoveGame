using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Framework.GalaSports.Core;
using Com.Proto;
using DG.Tweening.Plugins.Options;
using game.main;
using UnityEngine;
using Newtonsoft.Json;

namespace DataModel
{
    public class CardModel : Model
    {
        private CardRuleRes _cardRuleRes;
        private CardRes _cardRes;


        public List<UserCardVo> UserCardList;
        public UserCardVo CurCardVo;
        public PlayerPB CurPlayerPb = PlayerPB.None;

        private Dictionary<int, CardPB> _cardBaseDataDict;

        private Dictionary<int, CardPB> _openBaseCardDict; // Base开放卡
        
        public Dictionary<int, CardPB> CardBaseDataDict
        {
            get { return _cardBaseDataDict; }
        }

        public Dictionary<int, CardPB> OpenBaseCardDict
        {
            get { return _openBaseCardDict; } 
        }
        
        

        public int _curRoleId;

        private List<CardLevelRulePB> _cardLevelRuleList;
        private List<CardStarRulePB> _cardStarRuleList;
        private List<CardStarUpRulePB> _cardUpgradeStarRuleList;
        private List<CardEvoRulePB> _cardEvolutionRuleList;
        private List<CardResolveRulePB> _cardResolveRuleList;

        private List<CardLevelRulePB> _cardLevelRuleSSRList;
        private List<CardLevelRulePB> _cardLevelRuleSRList;
        private List<CardLevelRulePB> _cardLevelRuleRList;
        private List<SignatureRulePB> _signatureRuleList;//这个做成BASE比较好
        private List<UserSignaturePB> _userSignatureList;

        public int TotalCards
        {
            get { return _cardBaseDataDict.Count; }
        }

        public int OpenBaseCards
        {
            get { return _openBaseCardDict.Count; }
        }
        
        

        public List<ResolveCardVo> ResolveCardList
        {
            get
            {
                List<UserCardVo> cards = UserCardList.FindAll(match => { return match.Num > 1; });
                List<ResolveCardVo> list = new List<ResolveCardVo>();
                for (int i = 0; i < cards.Count; i++)
                {
                    ResolveCardVo vo = new ResolveCardVo(cards[i]);
                    list.Add(vo);
                }

                return list;
            }
        }

        public bool ShowRedPoint
        {
            get
            {
//                bool isShow = false;
//
//                foreach (var v in UserCardList)
//                {
//                    if (v.ShowCardDetailRedPoint)
//                    {
//                        return true;
//                    }
//                }
                return ShowCardDetailRedPoint||ShowCardPuzzleRedpoint;
            }
        }

        public bool ShowCardDetailRedPoint
        {
            get
            {
                bool isShow = false;

                foreach (var v in UserCardList)
                {
                    if (v.ShowCardDetailRedPoint)
                    {
                        return true;
                    }
                }
                return isShow;
            }  
        }

        //星缘碎片红点
        public bool ShowCardPuzzleRedpoint
        {
            get
            {
                bool isShow = false;

                foreach (var v in CardPuzzleList)
                {
                    if (v.Num>=v.RequireNum)
                    {
                        return true;
                    }
                }
                return isShow;
            }
        }
        
        
        

        public bool ShowLoveRedPoint
        {
            get
            {
                bool isShow = false;
                foreach (var v in UserCardList)
                {
                    if (v.CardVo.Credit!=CreditPB.R&&v.ShowLovePoint)
                    {
                        return true;
                    }
                }
                return isShow;
            }
            
        }
        

        public List<CardPuzzleVo> CardPuzzleList;

        public CardPuzzleVo GetUserPuzzleVo(int cardId)
        {
            foreach (var v in CardPuzzleList)
            {
                if (v.CardId == cardId)
                {
                    return v;
                }
            }
            return null;
        }

        public void AddUserPuzzle(AwardPB award)
        {
            CardPuzzleVo vo = GetUserPuzzleVo(award.ResourceId);
            if(vo == null) 
            {
                vo = new CardPuzzleVo(award.ResourceId, award.Num);
                CardPuzzleList.Add(vo);
            }
            else
            {
                vo.Num += award.Num;
            }
        }

        public void InitBaseData(CardRes cardRes)
        {
            _cardRes = cardRes;

            _cardBaseDataDict = new Dictionary<int, CardPB>();
            _openBaseCardDict =new  Dictionary<int, CardPB>();
            foreach (var pb in _cardRes.Cards)
            {
                
              _cardBaseDataDict[pb.CardId] = pb;
                if (pb.Used==0)    //0是开放，1是不开放
                {
                    _openBaseCardDict[pb.CardId] = pb;  
                }
            }


            
            _signatureRuleList = cardRes.SignatureRules.ToList();
        }

        public void InitBaseRule(CardRuleRes cardRuleRes)
        {
            _cardRuleRes = cardRuleRes;

            _cardLevelRuleList = _cardRuleRes.CardLevelRules.ToList();
            _cardStarRuleList = _cardRuleRes.CardStarRules.ToList();
            _cardUpgradeStarRuleList = _cardRuleRes.CardStarUpRules.ToList();
            _cardEvolutionRuleList = _cardRuleRes.CardEvoRules.ToList();
            _cardResolveRuleList = _cardRuleRes.CardResolveRules.ToList();


            _cardLevelRuleSSRList = new List<CardLevelRulePB>();
            _cardLevelRuleSRList = new List<CardLevelRulePB>();
            _cardLevelRuleRList = new List<CardLevelRulePB>();

            for (int i = 0; i < _cardLevelRuleList.Count; i++)
            {
                CardLevelRulePB pb = _cardLevelRuleList[i];
                if (pb.Credit == CreditPB.Ssr)
                {
                    _cardLevelRuleSSRList.Add(pb);
                }
                else if (pb.Credit == CreditPB.Sr)
                {
                    _cardLevelRuleSRList.Add(pb);
                }
                else
                {
                    _cardLevelRuleRList.Add(pb);
                }
            }
        }

        public void InitMyCards(MyCardRes res)
        {
            UserCardList = new List<UserCardVo>();
            for (int i = 0; i < res.UserCards.Count; i++)
            {
                UserCardVo vo = ParseUserCardVo(res.UserCards[i]);
               // Debug.LogError(vo.CardId+":"+vo.RecollectionCount);
                UserCardList.Add(vo);
            }

            UserCardList.Sort();
        }

        public void InitSignatures(MySignatureRes res)
        {
            _userSignatureList = res.UserSignatures.ToList();
        }

        public void AddUserSignature(int signatureId)
        {
            UserSignaturePB signaturePb=new UserSignaturePB();
            signaturePb.SignatureId = signatureId;
            signaturePb.Num = 1;
            _userSignatureList.Add(signaturePb);

        }

        public SignatureRulePB GetCardSignature(int cardid)
        {
            foreach (var v in _userSignatureList)
            {
                if (v.SignatureId==cardid)
                {
                    foreach (var a in _signatureRuleList)
                    {
                        if (a.SignatureId==cardid)
                        {
                            return a; 
                        }
                    }

                }   
            }

            return null;
        }
        
        public CardPB GetCardBase(int cardId)
        {
            if (!_cardBaseDataDict.ContainsKey(cardId))
            {
                Debug.LogError("cardBaseDataDict don't have CardId = " + cardId);
                return null;
            }

            return _cardBaseDataDict[cardId];
        }

        /// <summary>
        /// 卡牌升级规则
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public CardLevelRulePB GetCardLevelRule(int level, CreditPB credit)
        {
            List<CardLevelRulePB> list = _cardLevelRuleRList;
            if (credit == CreditPB.Ssr)
            {
                list = _cardLevelRuleSSRList;
            }

            if (credit == CreditPB.Sr)
            {
                list = _cardLevelRuleSRList;
            }

            if (list.Count <= level)
                return null;
            if (level < 0)
                return null;
            return list[level];
        }

        /// <summary>
        /// 最高能达到的等级
        /// </summary>
        /// <param name="credit">SSR,SR,R</param>
        /// <returns></returns>
        public int MaxLevel(CreditPB credit)
        {
            if (credit == CreditPB.Ssr)
            {
                return _cardLevelRuleSSRList.Count - 1;
            }

            if (credit == CreditPB.Sr)
            {
                return _cardLevelRuleSRList.Count - 1;
            }

            return _cardLevelRuleRList.Count - 1;
        }

        public int MaxStars(CreditPB credit)
        {
            int sum = 0;
            for (int i = 0; i < _cardStarRuleList.Count; i++)
            {
                CardStarRulePB pb = _cardStarRuleList[i];
                if (pb.Credit == credit)
                    sum++;
            }

            return sum - 1;
        }

        /// <summary>
        /// 卡牌升星规则
        /// </summary>
        /// <returns></returns>
        public CardStarRulePB GetCardStarRule(CreditPB credit, int star)
        {
            for (int i = 0; i < _cardStarRuleList.Count; i++)
            {
                CardStarRulePB pb = _cardStarRuleList[i];

                if (pb.Credit == credit && pb.Star == (StarPB) star)
                {
                    //Debug.LogError("pb.Credit "+pb.Credit+" "+pb.Power+" pb.Star  "+pb.Star );
                    return pb;
                }
            }

            return null;
        }

        /// <summary>
        /// 卡牌进化规则
        /// </summary>
        /// <param name="credit"></param>
        /// <returns></returns>
        public CardEvoRulePB GetCardEvoRule(CreditPB credit)
        {
            for (int i = 0; i < _cardEvolutionRuleList.Count; i++)
            {
                CardEvoRulePB pb = _cardEvolutionRuleList[i];
                if (pb.Credit == credit)
                    return pb;
            }

            return null;
        }

        //进化时需要用到的数据
        public CardEvoRulePB GetCardEvoRule(CreditPB credit, PlayerPB playerPb, EvolutionPB evolutionPb)
        {
            for (int i = 0; i < _cardEvolutionRuleList.Count; i++)
            {
                CardEvoRulePB pb = _cardEvolutionRuleList[i];
                if (pb.Credit == credit && pb.Player == playerPb && pb.Evo == evolutionPb)
                    return pb;
            }

            return null;
        }

        public int CardEvoPowerUp(CardEvoRulePB cardEvoRulePb)
        {
            int power = 0;
            switch (cardEvoRulePb.Evo)
            {
                case EvolutionPB.Evo1:
                    return GetCardEvoRule(cardEvoRulePb.Credit, cardEvoRulePb.Player, EvolutionPB.Evo1).Power;
                    break;
                case EvolutionPB.Evo2:
                    return GetCardEvoRule(cardEvoRulePb.Credit, cardEvoRulePb.Player, EvolutionPB.Evo2).Power
                           -GetCardEvoRule(cardEvoRulePb.Credit, cardEvoRulePb.Player, EvolutionPB.Evo1).Power;
                    break;
                case EvolutionPB.Evo3:
                    return GetCardEvoRule(cardEvoRulePb.Credit, cardEvoRulePb.Player, EvolutionPB.Evo3).Power
                           -GetCardEvoRule(cardEvoRulePb.Credit, cardEvoRulePb.Player, EvolutionPB.Evo2).Power;
                    break;
                   
            }
            return power;

        }


//        public CardEvoRulePB GetCardEvoRule(CreditPB credit, PlayerPB playerPb)
//        {
//            for (int i = 0; i < _cardEvolutionRuleList.Count; i++)
//            {
//                CardEvoRulePB pb = _cardEvolutionRuleList[i];
//                if (pb.Credit == credit && pb.Player == playerPb)
//                    return pb;
//            }
//
//            return null;
//        }

        public CardResolveRulePB GetCardResolveRule(CreditPB creditPb, PlayerPB playerPb)
        {
            for (int i = 0; i < _cardResolveRuleList.Count; i++)
            {
                CardResolveRulePB pb = _cardResolveRuleList[i];
                if (pb.Credit == creditPb && pb.Player == playerPb)
                {
                    return pb;
                }
            }

            return null;
        }


        /// <summary>
        /// 卡牌升星所需道具
        /// </summary>
        /// <param name="cardId"></param>
        /// <returns></returns>
        public CardStarUpRulePB GetCardStarUpRule(int cardId, StarPB star)
        {
            for (int i = 0; i < _cardUpgradeStarRuleList.Count; i++)
            {
                CardStarUpRulePB pb = _cardUpgradeStarRuleList[i];
                if (pb.CardId == cardId && pb.Star == (star + 1))
                    return pb;
            }

            return null;
        }


        public UserCardVo ParseUserCardVo(UserCardPB pb)
        {
            UserCardVo vo = new UserCardVo();
            vo.Level = pb.Level;
            vo.CardId = pb.CardId;
            vo.Evolution = pb.Evolution;
            vo.Star = (int) pb.Star;
            vo.Num = pb.Num;

            vo.CardVo = new CardVo();
            if (_cardBaseDataDict.ContainsKey(pb.CardId))
            {
                vo.CardVo.InitData(_cardBaseDataDict[pb.CardId]);
            }
            else
            {
                Debug.LogError(pb.CardId);
            }

            vo.CurrentLevelExp = pb.Exp;
            vo.UseEvo = pb.UseEvo;
            vo.Exp = pb.Exp;
            vo.RecollectionCount = pb.MemoriesNum;

            return vo;
        }


        public void UpdateUserCards(UserCardPB[] cards)
        {
            for (int i = 0; i < cards.Length; i++)
            {
                UserCardVo vo = GetUserCardById(cards[i].CardId);
                if (vo != null)
                {
                    UserCardPB pb = cards[i];
                    vo.Num = pb.Num;
                    vo.Evolution = pb.Evolution;
                    vo.Level = pb.Level;
                    vo.Star = (int) pb.Star;
                    vo.CurrentLevelExp = pb.Exp;
                    vo.UseEvo = pb.UseEvo;
                    vo.RecollectionCount = pb.MemoriesNum;
                }
                else
                {
                    UserCardList.Add(ParseUserCardVo(cards[i]));
                }
            }

            UserCardList.Sort();
        }

        public void UpdateUserCardsByIdAndNum(int cardId, int Num)
        {
            ////todo 解析协议xys
            UserCardVo vo = GetUserCardById(cardId);
            if (vo != null)
            {
                //   UserCardPB pb = cards[i];
                vo.Num += Num;
            }
            else
            {
                UserCardPB pb = new UserCardPB();

                pb.UserId = GlobalData.PlayerModel.PlayerVo.UserId;
                pb.CardId = cardId;
                pb.Num = Num;
                pb.Level = 0;
                pb.Exp = 0;
                pb.Star = StarPB.Star0;
                pb.Evolution = EvolutionPB.Evo0;

                //vo.Num = 0;
                //vo.Evolution =0;//0
                //vo.Level = 0;//0
                //vo.Star = 0;//0
                //vo.Exp = 0;//0
                UserCardList.Add(ParseUserCardVo(pb));
            }

            UserCardList.Sort();
        }

        public UserCardVo GetUserCardById(int id)
        {
            for (int i = 0; i < UserCardList.Count; i++)
            {
                if (UserCardList[i].CardId == id)
                {
                    return UserCardList[i];
                }
            }

            return null;
        }


//        private List<CardAwardPreInfo> _cardAwardPreInfos;
//        
//        public CardAwardPreInfo GetCardAwardInfo(PlayerPB playerPb,CardVo vo)
//        {
//            if (_cardAwardPreInfos==null)
//            {
//                _cardAwardPreInfos = ExpressoinUtil.GetDialogCollects((int)playerPb);
//            }
//
//            foreach (var v in _cardAwardPreInfos)
//            {
//                if (v.dialogId==)
//                {
//                    
//                    
//                }
//                
//            }
//            
//            
//            
//        }
        
        public bool ContainsCardByNpcId(PlayerPB pb)
        {
            for (int i = 0; i < UserCardList.Count; i++)
            {
                if (UserCardList[i].CardVo.Player == pb)
                {
                    return true;
                }
            }
            return false;
        }

        public JumpData[] GetJumpDataById(int id)
        {
            return ClientData.GetJumpDataById(id);
        }

        public override void Destroy()
        {
            ClientData.Clear();
        }
    }
}