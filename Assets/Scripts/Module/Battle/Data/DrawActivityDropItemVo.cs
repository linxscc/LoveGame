using System.Collections.Generic;
using Com.Proto;
using DataModel;

namespace Module.Battle.Data
{
    public class DrawActivityDropItemVo
    {
        /// <summary>
        /// 活动ID
        /// </summary>
        public int ActivityId;

        /// <summary>
        /// 每日上限
        /// </summary>
        public int LimitNum;
        
        /// <summary>
        /// 今日掉落总数
        /// </summary>
        public int TotalNum;

        /// <summary>
        /// 和显示的RewardItem的index对应
        /// </summary>
        public int DisplayIndex;

        public DrawActivityDropItemVo(DroppingItemPB pb, List<RewardVo> rewardList, HolidayModulePB dropType)
        {
            ActivityId = pb.ActivityId;
            LimitNum = GlobalData.ActivityModel.Limit(ActivityId, dropType);
            
            //单个的时候跟当前数量一致，多个的时候是使用最后一个的数量
            TotalNum = pb.CurrentNum;
            
            for (int i = 0; i < rewardList.Count; i++)
            {
                if (pb.ResourceId == rewardList[i].Id)
                {
                    DisplayIndex = i;
                    break;;
                }
            }
        }
    }
}