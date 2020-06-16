using Com.Proto;

namespace DataModel
{
    public class UserPropVo
    {
        public PlayerPB Only;

        public int Power;

        public string Name;

        public int ItemId;

        public int Num;

        /// <summary>
        /// 卡牌升级道具提供的经验
        /// </summary>
        public int Exp;

        public UserPropVo(UserItemPB pb)
        {
            Num = pb.Num;
            ItemId = pb.ItemId;

            ItemPB itemPb = GlobalData.PropModel.GetPropBase(ItemId);
            if (itemPb != null)
            {
                Name = itemPb.Name;
                Power = itemPb.Power;
                Only = itemPb.Only;
                Exp = itemPb.Exp;
            }
        }

        public UserPropVo(int itemId)
        {
            ItemId = itemId;

            ItemPB itemPb = GlobalData.PropModel.GetPropBase(ItemId);
            if (itemPb != null)
            {
                Name = itemPb.Name;
                Power = itemPb.Power;
                Only = itemPb.Only;
                Exp = itemPb.Exp;
            }
        }

        //这种调用他妈的有问题！
        public string GetTexturePath()
        {
            // Debug.LogError(ItemId);
            return "Prop/" + ItemId;
        }
    }
}