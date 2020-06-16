using System;
using Com.Proto;
using DataModel;
using Google.Protobuf.Collections;
using UnityEngine;

namespace Utils
{
    public class RewardUtil
    {
        public static void AddReward(AwardPB[] awardPbs)
        {
            for (int i = 0; i < awardPbs.Length; i++)
            {
                AddReward(awardPbs[i]);
            }
        }
        
        public static void AddReward(RepeatedField<AwardPB> awardPbs)
        {
            for (int i = 0; i < awardPbs.Count; i++)
            {
                AddReward(awardPbs[i]);
            }
        }

        public static void AddReward(AwardPB award)
        {
            switch(award.Resource)
            {
                case ResourcePB.Card:
                    GlobalData.CardModel.UpdateUserCardsByIdAndNum(award.ResourceId, award.Num);
                    break;
                case ResourcePB.Item:
                    GlobalData.PropModel.AddProp(award);
                    break;
                case ResourcePB.Puzzle://更新碎片
                    GlobalData.CardModel.AddUserPuzzle(award);
                    break;
                case ResourcePB.Power:
                    GlobalData.PlayerModel.AddPower(award.Num);
                    break;
                case ResourcePB.Gem:
                    GlobalData.PlayerModel.UpdateUserGem(award.Num);
                    break;
                case ResourcePB.Gold:
                    GlobalData.PlayerModel.UpdateUserGold(award.Num);
                    break;
                case ResourcePB.Fans:
                    GlobalData.DepartmentData.UpdateFans(award.ResourceId, award.Num);
                    break;
                case ResourcePB.Element:
                    //获取到元素的奖励
                    GlobalData.DiaryElementModel.UpdateElement(award.ResourceId, award.Num);
                    break;
                case ResourcePB.Exp:
                    GlobalData.PlayerModel.AddExp(award.Num);
                    break;
                case ResourcePB.EncouragePower:
                    break;
                case ResourcePB.Favorability:
                    break;
                case ResourcePB.Memories:
                    GlobalData.PlayerModel.AddRecollectionEnergy(award.Num);
                    break;
                case ResourcePB.Signature:
                    GlobalData.CardModel.AddUserSignature(award.ResourceId);    
                    break;
                default:
                    Debug.LogError("此奖励类型没有加入到数据缓存中，请立即添加类型:" + award.Resource);
                    break;
            }
        }
    }
}