using DataModel;
using Google.Protobuf.Collections;
using UnityEngine;

namespace game.main
{
    public class ResolveCardVo
    {
        public string Name;

        public int Num;
        
        public int SelectedNum;

        public int CardId;

        public MapField<int,int> ResolveItem;

        public CreditPB Credit;

        public PlayerPB Player;

        public string CardPath;

        public ResolveCardVo(UserCardVo vo)
        {
            Num = vo.Num - 1;
            CardId = vo.CardId;
            Credit = vo.CardVo.Credit;
            Name = vo.CardVo.CardName;
            Player = vo.CardVo.Player;
            CardPath = vo.CardVo.MiddleCardPath(vo.UserNeedShowEvoCard()&&vo.Level>60);//注意R卡！vo.UserNeedShowEvoCard()
            //ResolveItem = GlobalData.CardModel.GetCardEvoRule(Credit, Player)?.Resolve;
            ResolveItem = GlobalData.CardModel.GetCardResolveRule(Credit, Player).Resolve;
        }
        
        public int CompareTo(ResolveCardVo other)
        {
            int result = 0;
            if (other.Credit.CompareTo(Credit) != 0)
            {
                result = -other.Credit.CompareTo(Credit);
            }
//            else if (other.Evolution.CompareTo(Evolution) != 0)
//            {
//                result = other.Evolution.CompareTo(Evolution);
//            }
//            else if(other.Player.CompareTo(Player)!=0)
//            {
//                result = -other.Player.CompareTo(Player);
//            }
            else
            {
                result = -other.Num.CompareTo(Num);
            }

            return result;
        }
    }
}