using System;
using System.Collections.Generic;
using game.main;

namespace Module.Battle.Data
{
    public class BattleUserCardVo : IComparable<BattleUserCardVo>
    {
        public UserCardVo UserCardVo;

        public bool IsUsed;

        public int CompareTo(BattleUserCardVo other)
        {
            return this.UserCardVo.CompareTo(other.UserCardVo);
        }
    }
}