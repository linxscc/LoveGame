using System.Collections.Generic;
using Com.Proto;
using game.main;

namespace Assets.Scripts.Module.Recollection.Data
{
    public class RecollectionCardDropVo
    {
        public string CardName;

        public int CardId;

        public bool HasCard;
        
        public CreditPB Credit;

        public UserCardVo UserCardVo;

       
        public RecollectionCardDropVo(CardPB pb, UserCardVo userCardVo)
        {
            CardName = CardVo.SpliceCardName(pb.CardName, pb.Player);

            CardId = pb.CardId;

            HasCard = userCardVo != null;

            Credit = pb.Credit;

            UserCardVo = userCardVo;

        
        }
        
        public string MiddleCardPath(bool evo=false) {
            return evo?"Card/Image/MiddleCard/EvolutionMiddleCard/" + CardId : "Card/Image/MiddleCard/" + CardId; 
        }
    }
}