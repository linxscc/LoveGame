using System;
using Com.Proto;
using UnityEngine;

namespace game.main
{
    /// <summary>
    /// 卡牌基础数据
    /// </summary>
    public class CardVo
    {
        private int _cardId;
        
        public int Singing;
        public int Dancing;
        public int Original;
        public int Popularity;
        public int Glamour;
        public int Willpower;
        
        public CreditPB Credit;

        public string CardName;

        public string TitleName;
        
        public int MaxLevel;

        public int MaxStar;
        
        public PlayerPB Player;

        public EvolutionPB NewViewEvo;

        public int RecollectionDropItemId;

        /// <summary>
        /// 碎片合成卡牌需要的金币
        /// </summary>
        public int GoldCost;

        public float GrowthRateS;
        public float GrowthRateD;
        public float GrowthRateC;
        public float GrowthRatePo;
        public float GrowthRateCh;
        public float GrowthRatePe;


        public int GetMaxEvoTimes()
        {
            switch (MaxLevel)
            {
                case 60:
                    return 1;
                case 80:
                    return 2;

                case 100:
                    return 3;
                default:
                    return 1;
            }
        }

        public void InitData(CardPB pb)
        {
            Singing = pb.Singing;
            Dancing = pb.Dancing;
            Original = pb.Composing;
            Popularity = pb.Popularity;
            Glamour = pb.Charm;
            Willpower = pb.Perseverance;
            _cardId = pb.CardId;

            Credit = pb.Credit;

            if (Credit == CreditPB.Ssr)
            {
                MaxLevel = 100;
                MaxStar = 5;
            }
            else if (Credit == CreditPB.Sr)
            {
                MaxLevel = 80;
                MaxStar = 4;
            }
            else
            {
                MaxLevel = 60;
                MaxStar = 3;
            }

            Player = pb.Player;

            GoldCost = pb.Gold;

            TitleName = pb.CardName;
            
            CardName = SpliceCardName(pb.CardName, pb.Player);

            RecollectionDropItemId = pb.MemoriesItem;

//            Debug.LogError(" "+pb.GrowthRateS+" "+pb.GrowthRateD+" "+pb.GrowthRateC+" "+pb.GrowthRatePo+" "+pb.GrowthRateCh
//                           +" "+pb.GrowthRatePe);
            
            GrowthRateS = pb.GrowthRateS;
            GrowthRateD = pb.GrowthRateD;
            GrowthRateC = pb.GrowthRateC;
            GrowthRatePo = pb.GrowthRatePo;
            GrowthRateCh = pb.GrowthRateCh;
            GrowthRatePe = pb.GrowthRatePe;
            NewViewEvo = (EvolutionPB)pb.NewViewEvo;
//            CardName += "::" + _cardId;
        }

        public static string SpliceCardName(string name, PlayerPB pb)
        {
            string cardName = "";
            switch (pb)
            {
                case PlayerPB.None:
                    cardName = "None";
                    break;
                case PlayerPB.TangYiChen:
                    cardName = I18NManager.Get("Common_Role1");
                    break;
                case PlayerPB.QinYuZhe:
                    cardName = I18NManager.Get("Common_Role2");//"秦予哲";
                    break;
                case PlayerPB.YanJi:
                    cardName = I18NManager.Get("Common_Role3"); //"言季";
                    break;
                case PlayerPB.ChiYu:
                    cardName = I18NManager.Get("Common_Role4");// "迟郁";
                    break;
            }
            return cardName + "·" + name;
        }
        
        public string BigCardPath(bool evo=false) {
             return evo?"Card/Image/EvolutionCard/" + _cardId:"Card/Image/" + _cardId; 
        }
        
        public string MiddleCardPath(bool evo=false) {
             return evo?"Card/Image/MiddleCard/EvolutionMiddleCard/" + _cardId:"Card/Image/MiddleCard/" + _cardId; 
        }
        
        public string SmallCardPath (bool evo=false){
            return evo?"Card/Image/SmallCard/EvolutionSmallCard/" + _cardId:"Card/Image/SmallCard/" + _cardId; 
        }
        
        public string SquareCardPath  (bool evo=false){
             return evo?"Head/EvolutionHead/" + _cardId:"Head/" + _cardId; 
        }
    }
}