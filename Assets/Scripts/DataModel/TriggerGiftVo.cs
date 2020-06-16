using System;
using System.Collections.Generic;
using Com.Proto;
using Google.Protobuf.Collections;

namespace DataModel
{
    /// <summary>
    /// 免费礼包从规则读取，付费礼包从商场读取
    /// </summary>
    public class TriggerGiftVo : IComparable<TriggerGiftVo>
    {
        public long Id;
        public int MallId;
        public int FreeType;
        public long MaturityTime;

        public bool IsFree;

        public RepeatedField<AwardPB> Awards;
        
        public RmbMallRulePB Rule;

        public List<RewardVo> GetRewardList()
        {
            List<RewardVo> list = new List<RewardVo>();

            for (int i = 0; i < Awards.Count; i++)
            {
                list.Add(new RewardVo(Awards[i]));
            }
            return list;
        }

        public ProductVo GetProduct()
        {
            return GlobalData.PayModel.GetProduct(MallId);
        }

        public int CompareTo(TriggerGiftVo other)
        {
            int result = MaturityTime.CompareTo(other.MaturityTime);
            if (result == 0)
            {
                if (IsFree)
                    result = -1;
                else
                    result = 1;
            }

            return result;
        }
    }
}