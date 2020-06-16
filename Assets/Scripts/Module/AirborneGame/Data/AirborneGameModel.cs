using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Framework.GalaSports.Core;
using Com.Proto;
using System;

namespace DataModel
{
    public enum AirborneGameOverType
    {
        OverTime,//时间到
        Durian,//榴莲
    }


    public class AirborneGameModel : Model
    {
        public AirborneGameInfo GameInfo;
        public AirborneGameRunningInfo RunningInfo;
        private Dictionary<int, Dictionary<ItemTypeEnum, List<AirborneGameRunningItemVo>>> _gameJumpItemDir;// group-type-jumpitem;

        public void InitData(GameJumpInfosRes res)
        {
            //计算量级
            int level = GlobalData.PlayerModel.PlayerVo.Level;
            GameInfo = new AirborneGameInfo(res, level);
            RunningInfo = new AirborneGameRunningInfo();
            RunningInfo.MaxTime = GameInfo.MaxGameTime;
        }

        public void InitRuningData(MyGameJumpStartRes res)
        {
            //需要按照ItemTypeEnum初始化时间轴
            _gameJumpItemDir = new Dictionary<int, Dictionary<ItemTypeEnum, List<AirborneGameRunningItemVo>>>();
            foreach (var v in res.Items)
            {
                GameJumpItemRulePB rulepb = GameInfo.GetGameJumpItemRulePB(v.ReourceId, v.ItemType);
                if (!_gameJumpItemDir.ContainsKey(v.Group))
                {
                    _gameJumpItemDir[v.Group] = new Dictionary<ItemTypeEnum, List<AirborneGameRunningItemVo>>();
                }
                Dictionary<ItemTypeEnum, List<AirborneGameRunningItemVo>> group = _gameJumpItemDir[v.Group];
                if (!group.ContainsKey(v.ItemType))
                {
                    group[v.ItemType] = new List<AirborneGameRunningItemVo>();
                }
                group[v.ItemType].AddRange(CreateItmeTimeLine(v));
            }


            foreach (var v in _gameJumpItemDir)
            {
                foreach (var v1 in v.Value)
                {
                    GameJumpItemRulePB rulepb = GameInfo.GetGameJumpItemRulePB(v1.Value[1].ResourceId,v1.Value[1].Itemtype);
                    Debug.LogError("SetItmeTimeLine     group   " + v.Key  + "  AppearTypeEnum  " + rulepb .AppearInfo.AppearType + "  ItemType  " + v1.Key + "   ItemType   " + rulepb.ItemType);
                    Shuffle(v1.Value);
                    SetItmeTimeLine(v1.Value, rulepb);
                    RunningInfo.AddRunningItemsByGroupId(v.Key, v1.Value);
                }
            }

            RunningInfo.SortRunningItems();
       
            //GenerateRunningInfo();
            
        }



        /// <summary>
        ///洗牌
        /// </summary>
        /// <param name="list"></param>
        private void Shuffle(List<AirborneGameRunningItemVo> list)
        {
            int count = list.Count;
            if (count <= 1)
                return;
            for(int i=count-1;i>0;i--)
            {
                int rand = UnityEngine.Random.Range(0, i);
                AirborneGameRunningItemVo temp = list[i];
                list[i] = list[rand];
                list[rand] = temp;

            }
        }

       /// <summary>
       /// 设置时间轴
       /// </summary>
       /// <param name="list"></param>
       /// <param name="rulePb"></param>
        private void SetItmeTimeLine(List<AirborneGameRunningItemVo> list, GameJumpItemRulePB rulePb)
        {
            switch (rulePb.AppearInfo.AppearType)
            {
                case AppearTypeEnum.FixedNum:
                    SetAppearFixedNum(list, rulePb.AppearInfo);
                    break;
                case AppearTypeEnum.Frequency:
                    SetAppearFrequency(list, rulePb.AppearInfo);
                    break;
                default:
                    break;
            }
        }
        private List<AirborneGameRunningItemVo> CreateItmeTimeLine(GameJumpItemPB jumpItem)
        {
            GameJumpItemRulePB itemPb = GameInfo.GetGameJumpItemRulePB(jumpItem.ReourceId,jumpItem.ItemType);
            GameJumpLevelRulePB levelPb = GameInfo.GetGameJumpLevelRule(jumpItem.ItemType);
            GameJumpAppearInfoPB AppearInto = itemPb.AppearInfo;
            int leftCount = jumpItem.Count;

            List<AirborneGameRunningItemVo> list = new List<AirborneGameRunningItemVo>();
            while (leftCount > 0)
            {
                AirborneGameRunningItemVo vo = new AirborneGameRunningItemVo();
                vo.Speed = itemPb.Speed;
                vo.Itemtype = itemPb.ItemType;
                vo.ResourceId = itemPb.ReourceId;
                vo.Resource = (ResourcePB)itemPb.Reource;
                if (leftCount >= levelPb.Num)
                {
                    vo.Count = levelPb.Num;
                }
                else
                {
                    Debug.LogError(" leftCount  is not enough ");
                    vo.Count = leftCount;
                }
                leftCount = leftCount - vo.Count;
                list.Add(vo);
            }
            return list;
        }


        private void SetAppearFrequency(List<AirborneGameRunningItemVo> list, GameJumpAppearInfoPB appearInto)
        {

            float timeline = 0;
            float rate = appearInto.Rate;
            Debug.LogError("TriggerTime rate " + rate);
            foreach (var v in list)
            {
                float rand = UnityEngine.Random.Range(0, rate);
                v.TriggerTime = timeline + rand;
                Debug.LogError("TriggerTime rate " + rate + "   timeline   " + timeline + "  rand  " + rand + "   TriggerTime    " + v.TriggerTime);
                timeline = timeline + rate;
            }
            return;
        }
        private void SetAppearFixedNum(List<AirborneGameRunningItemVo> list, GameJumpAppearInfoPB appearInto)
        {
            int num = appearInto.Num;
            float interval = appearInto.Interval;
            float range = appearInto.Time;//时间范围  前多少秒

            float timeline = 0;
            int count = list.Count;
            for (int i = 0; i < count; i++)
            {
                float rand = UnityEngine.Random.Range(timeline, range - (count - 1 - i) * interval);
                timeline = rand;
                list[i].TriggerTime = timeline;
                Debug.LogError("TriggerTime num " + num + " count " + count + "   range   " + range + " timeline  " + timeline +
                        "   rand   " + rand +
                        "  interval  " + interval + "   TriggerTime    " + list[i].TriggerTime);
                timeline = timeline + interval;
            }

        }

    }
}