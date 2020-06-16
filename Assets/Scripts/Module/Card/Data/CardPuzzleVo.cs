using System;
using Com.Proto;
using DataModel;

namespace game.main
{
    public class CardPuzzleVo : IComparable<CardPuzzleVo>
    {
        public int Num;

        public int CardId;

        public string Name;

        public CreditPB Credit;
        
        public int RequireNum;

        public PlayerPB Player;

        public string CardPath;

        public CardVo CardVo;
        
//        public string SmallCardPath (bool evo=false){
//            return evo?"Card/Image/SmallCard/EvolutionSmallCard/" + _cardId:"Card/Image/SmallCard/" + _cardId; 
//        }

        public CardPuzzleVo(int Id,int count)
        {
            CardId = Id;
            Num = count;
            InitData();
        }

        public CardPuzzleVo(UserPuzzlePB pb)
        {
            CardId = pb.CardId;
            Num = pb.Num;
            InitData();
        }

        private void InitData()
        {
            CardPB cardPb = GlobalData.CardModel.GetCardBase(CardId);
            Name = cardPb.CardName;
            Credit = cardPb.Credit;
            RequireNum = cardPb.Puzzle;
            Player = cardPb.Player;
            Name = CardVo.SpliceCardName(Name, cardPb.Player);
            CardVo = new CardVo();
            CardVo.InitData(cardPb);
            var vo = GlobalData.CardModel.GetUserCardById(CardId);
            CardPath = vo != null ? CardVo.SmallCardPath(vo.UserNeedShowEvoCard() && vo.Level > 60) : CardVo.SmallCardPath();
        }

        public int CompareTo(CardPuzzleVo other)
        {
            int result = 0;
            
            int sum = Num - RequireNum;
            int otherSum = other.Num - other.RequireNum;
            


            
            if(sum <0&&otherSum>=0)
            {
                result = 1;
            }
            else if (sum >=0&&otherSum<0)
            {
                result = -1;
            }
            else if (other.Credit.CompareTo(Credit) != 0)
            {
                result = -other.Credit.CompareTo(Credit);
            }
//            else if(other.Player.CompareTo(Player)!=0)
//            {
//                result = -other.Player.CompareTo(Player);
//            }
            else if(other.Num.CompareTo(Num)!=0)
            {
                result= other.Num.CompareTo(Num);
            }



            return result;
        }
    }
}