using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Com.Proto;
using Google.Protobuf.Collections;

namespace Assets.Scripts.DataModel
{
    public class MyCards
    {
        public List<UserCardPB> UserCards;

        public MyCards(RepeatedField<UserCardPB> resCards)
        {
            UserCards=new List<UserCardPB>();
            foreach (var card in resCards)
            {
                UserCards.Add(card);
            }
        }
    }
}
