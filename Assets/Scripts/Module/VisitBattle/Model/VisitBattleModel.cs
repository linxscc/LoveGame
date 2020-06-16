using System;
using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core;
using Com.Proto;
using DataModel;
using game.main;
using Google.Protobuf.Collections;
using UnityEngine;
public enum FunsState
{
    None,
    Move,
    Wave,
    Idel,
}

namespace Module.VisitBattle.Data
{
    public class VisitBattleModel : Model
    {
        public VisitLevelVo LevelVo;

        public List<VisitBattleUserCardVo> UserCardList;

        public int SelectedCount = 0;

        public bool IsShowingAnimation = false;
        public bool IsGetBattleResult = false;

        public List<ChallengeCardNumRulePB> CardNumRules { get; set; }

        public Message TempMessage;

        public Dictionary<int, int> ItemsDict;
        public Dictionary<int, int> FansDict;

        public int Power;
        public int Transmission;
        public int Resource;
        public int Financial;
        public int Active;
        public int Support;
        private int _addtional;

        public void InitSupporterValue()
        {
            foreach (var vo in GlobalData.DepartmentData.MyDepartments)
            {
                if (vo.UserDepartmentPb.DepartmentType == DepartmentTypePB.Support)
                {
                    Support = vo.RulePb.Power;
                    _addtional = Support / 4;
                    break;
                }
            }

            foreach (var vo in GlobalData.DepartmentData.MyDepartments)
            {
                switch (vo.UserDepartmentPb.DepartmentType)
                {
                    case DepartmentTypePB.Active:
                        Active = vo.RulePb.Power + _addtional;
                        Power += Active;
                        break;
                    case DepartmentTypePB.Financial:
                        Financial = vo.RulePb.Power + _addtional;
                        Power += Financial;
                        break;
                    case DepartmentTypePB.Resource:
                        Resource = vo.RulePb.Power + _addtional;
                        Power += Resource;
                        break;
                    case DepartmentTypePB.Transmission:
                        Transmission = vo.RulePb.Power + _addtional;
                        Power += Transmission;
                        break;
                }
            }
        }

        public override void OnMessage(Message message)
        {
            base.OnMessage(message);
            string name = message.Name;
            object[] body = message.Params;
            switch (name)
            {
                case MessageConst.CMD_VISITBATTLE_ITEM_DATA:
                    ItemsDict = (Dictionary<int, int>)body[0];
                    FansDict = (Dictionary<int, int>)body[1];
                    break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="AbilityType">AbilityPB, 默认不筛选</param>
        /// <returns></returns>
        public List<VisitBattleUserCardVo> FilterCard(int AbilityType = -1)
        {
            float time;
            //            if (AbilityType == -1)
            //            {
            //                time = Time.realtimeSinceStartup;
            //                UserCardList.Sort();
            //                Debug.LogError("排序时间1：" + (Time.realtimeSinceStartup - time)*1000);
            //                return UserCardList;
            //            }

            AbilityPB pb = (AbilityPB)AbilityType;

            time = Time.realtimeSinceStartup;
            UserCardList.Sort((a, b) =>
            {
                int x = 0;
                int y = 0;
                switch (pb)
                {
                    case AbilityPB.Singing:
                        x = a.UserCardVo.Singing;
                        y = b.UserCardVo.Singing;
                        break;
                    case AbilityPB.Dancing:
                        x = a.UserCardVo.Dancing;
                        y = b.UserCardVo.Dancing;
                        break;
                    case AbilityPB.Composing:
                        x = a.UserCardVo.Original;
                        y = b.UserCardVo.Original;
                        break;
                    case AbilityPB.Popularity:
                        x = a.UserCardVo.Popularity;
                        y = b.UserCardVo.Popularity;
                        break;
                    case AbilityPB.Charm:
                        x = a.UserCardVo.Glamour;
                        y = b.UserCardVo.Glamour;
                        break;
                    case AbilityPB.Perseverance:
                        x = a.UserCardVo.Willpower;
                        y = b.UserCardVo.Willpower;
                        break;
                    default:
                        x = a.UserCardVo.TotalAblity();
                        y = b.UserCardVo.TotalAblity();
                        break;
                }

                if (y.CompareTo(x) != 0)
                    return y.CompareTo(x);

                return a.UserCardVo.CardId.CompareTo(b.UserCardVo.CardId);
            });
            //  Debug.LogError("排序时间2：" + (Time.realtimeSinceStartup - time) * 1000);
            return UserCardList;
        }

        public void InitCardList(List<UserCardVo> list,PlayerPB NpcId)
        {
            SelectedCount = 0;

            UserCardList = new List<VisitBattleUserCardVo>();
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].CardVo.Player != NpcId)//非关卡的卡牌不能探班
                    continue;
                VisitBattleUserCardVo vo = new VisitBattleUserCardVo();
                vo.UserCardVo = list[i];
                UserCardList.Add(vo);
            }
        }

        public bool IsCardPositionOpen(int index)
        {
            return GlobalData.PlayerModel.PlayerVo.Level >= CardNumRules[index].LevelMin;
        }

        /// <summary>
        ///  获得当前等级对应的卡牌开放数量
        /// </summary>
        /// <returns></returns>
        public int CardOpenNum()
        {
            int level = GlobalData.PlayerModel.PlayerVo.Level;

            for (int i = CardNumRules.Count - 1; i >= 0; i--)
            {
                if (level >= CardNumRules[i].LevelMin)
                {
                    return CardNumRules[i].OpenNum;
                }
            }

            return 1;
        }

        public void Reset()
        {
            Power = 0;

            IsGetBattleResult = false;
        }


    }
}