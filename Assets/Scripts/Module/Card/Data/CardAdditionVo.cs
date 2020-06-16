using System;
using UnityEngine;

namespace game.main
{
    public class CardAdditionVo
    {
        public int SingingAdditon;
        public int DancingAdditon;
        public int OriginalAdditon;
        public int PopularityAdditon;
        public int GlamourAdditon;
        public int WillpowerAdditon;


        public void AssignPower(int pbPower,CardVo vo)
        {           
            SingingAdditon = (int) Math.Ceiling(pbPower * vo.GrowthRateS);
            DancingAdditon = (int) Math.Ceiling(pbPower * vo.GrowthRateD);
            OriginalAdditon = (int) Math.Ceiling(pbPower * vo.GrowthRateC);
            PopularityAdditon = (int) Math.Ceiling(pbPower * vo.GrowthRatePo);
            GlamourAdditon = (int) Math.Ceiling(pbPower * vo.GrowthRateCh);
            WillpowerAdditon = (int) Math.Ceiling(pbPower * vo.GrowthRatePe);
        }
    }
}