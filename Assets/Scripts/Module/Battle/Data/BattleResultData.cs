﻿using System;
using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core;
using Com.Proto;
using DataModel;
using Google.Protobuf.Collections;

namespace Module.Battle.Data
{
    public class BattleResultData:Model
    {
        public int Star;
        public int Cap;
        public int Exp;
        public int CardExp;
        public long CreateTime;
        public List<RewardVo> RewardList;
        public Dictionary<int, DrawActivityDropItemVo> DrawActivityDropItemDict;
        
        public RepeatedField<UserCardPB> UserCards;

        public RepeatedField<LevelCommentRulePB> Comments;

        public override void OnMessage(Message message)
        {
            base.OnMessage(message);
            string name = message.Name;
            object[] body = message.Params;
            switch (name)
            {
                case MessageConst.CMD_BATTLE_RESULT_DATA:
                    SetData((GameResultPB) body[0]);
                    break;
            }
        }

        public List<LevelCommentRulePB> GetRandomComments(int star)
        {
            int length = 4;
            
            List<LevelCommentRulePB> list = new List<LevelCommentRulePB>();
            foreach (var comment in Comments)
            {
                if (comment.GroupId == star)
                {
                    list.Add(comment);
                }
            }

            var random = new Random();
            for (int i = 0; i < list.Count; i++)
            {
                if(list.Count > length)
                {
                    var next = random.Next(0, list.Count);
                    list.RemoveAt(next);
                }
                else
                {
                    break;
                }
            }

            List<LevelCommentRulePB> list2 = new List<LevelCommentRulePB>();
            for (int i = 0; i < length; i++)
            {
                var next = random.Next(0, list.Count);
                LevelCommentRulePB item = list[next];
                list.Remove(item);
                list2.Add(item);
            }
            
            return list2;
        }

        private void SetData(GameResultPB pb)
        {
            if (pb != null)
            {
                Star = pb.Star;
                Cap = pb.Cap;
                Exp = pb.Exp;
                CardExp = pb.CardExp;
                CreateTime = pb.CreateTime;
                RewardList = new List<RewardVo>();
                for (int i = 0; i < pb.Awards.Count; i++)
                {
                    //道具在外部已经更新了
                    RewardVo vo = new RewardVo(pb.Awards[i], pb.Awards[i].Resource != ResourcePB.Item);
                    RewardList.Add(vo);
                }
                DrawActivityDropItemDict = new Dictionary<int, DrawActivityDropItemVo>();
                if (pb.DroppingItem != null)
                {
                    for (int i = 0; i < pb.DroppingItem.Count; i++)
                    {
                        DrawActivityDropItemVo vo = new DrawActivityDropItemVo(pb.DroppingItem[i], RewardList, HolidayModulePB.ActivityCareer);
                        DrawActivityDropItemDict.Add(vo.DisplayIndex, vo);
                    }
                }
                
                UserCards = pb.UserCards;
            }
            else
            {
                Star = 0;
                Cap = 0;
                Exp = 0;
                CardExp = 0;
                CreateTime = 0;
                RewardList = null;
                UserCards = null;
            }
        }
    }
}