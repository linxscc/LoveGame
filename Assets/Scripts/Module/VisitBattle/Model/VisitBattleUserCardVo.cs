using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using game.main;
using System;

namespace Module.VisitBattle.Data
{
    public class VisitBattleUserCardVo : IComparable<VisitBattleUserCardVo>
    {
        public UserCardVo UserCardVo;

        public bool IsUsed;

        public int CompareTo(VisitBattleUserCardVo other)
        {
            return this.UserCardVo.CompareTo(other.UserCardVo);
        }

    }
}