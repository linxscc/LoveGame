using Com.Proto;
using Google.Protobuf.Collections;


namespace game.main
{
    public class GameMallVo
    {
        public int MallId;//商品Id
        public MallTypePB MallType;//商品类型
        public MallSortPB MallSortPb;       //商品分类
        public MoneyTypePB MoneyTypePb;     //货币类型
        public string MallName;                      //商品名称
        public string MallDesc;                      //商品描述
        public string GiftImage;                     //礼包图片
        public int Slot;                            //槽位
        public MallLabelPB MallLabelPb;             //商品标签
        public string LabelImage;                   //商品类型展示图片
        public int RealPrice;//真实价格
        public int OriginalPrice;//原价
        public int BuyMax;//周期内购买上限
        public RepeatedField<AwardPB> Award;//商品奖励内容
        public long Starttime;//开始时间
        public long EndTime;//结束时间

        public GameMallVo(GameMallRulePB gamemallRulePb)
        {
            MallId = gamemallRulePb.MallId;
            MallType = gamemallRulePb.MallType;
            MallSortPb = gamemallRulePb.MallSort;
            MoneyTypePb = gamemallRulePb.MoneyType;
            MallName = gamemallRulePb.MallName;
            MallDesc = gamemallRulePb.MallDesc;
            GiftImage = gamemallRulePb.GiftImage;
            Slot = gamemallRulePb.Slot;
            MallLabelPb = gamemallRulePb.MallLabel;
            LabelImage = gamemallRulePb.LabelImage;
            RealPrice = gamemallRulePb.RealPrice;
            OriginalPrice = gamemallRulePb.OriginalPrice;
            BuyMax = gamemallRulePb.BuyMax;
            Award = gamemallRulePb.Award;
            Starttime = gamemallRulePb.StartTime;
            EndTime = gamemallRulePb.EndTime;
        }


    }
    
    public class RmbMallVo
    {
        public int MallId;//商品Id
        public MallSortPB MallSortPb;       //商品分类
        public string MallName;                      //商品名称
        public string MallDesc;                      //商品描述
        public string GiftImage;                     //礼包图片
        public int Slot;                            //槽位
        public MallLabelPB MallLabelPb;             //商品标签
        public string LabelImage;                   //商品类型展示图片
        public int RealPrice;//真实价格
        public int OriginalPrice;//原价
        public int BuyMax;//周期内购买上限
        public int BuyRefreshDay; //商品次数刷新周期
        public RepeatedField<AwardPB> Award;//商品奖励内容
        public long Starttime;//开始时间
        public long EndTime;//结束时间
        public bool Special; //特殊礼包标记

        public RmbMallVo(RmbMallRulePB gamemallRulePb)
        {
            MallId = gamemallRulePb.MallId;
            MallSortPb = gamemallRulePb.MallSort;
            MallName = gamemallRulePb.MallName;
            MallDesc = gamemallRulePb.MallDesc;
            GiftImage = gamemallRulePb.GiftImage;
            Slot = gamemallRulePb.Slot;
            MallLabelPb = gamemallRulePb.MallLabel;
            LabelImage = gamemallRulePb.LabelImage;
            RealPrice = gamemallRulePb.RealPrice;
            OriginalPrice = gamemallRulePb.OriginalPrice;
            Special = gamemallRulePb.Special;
            BuyMax = gamemallRulePb.BuyMax;
            BuyRefreshDay = gamemallRulePb.BuyRefreshDay;
            Award = gamemallRulePb.Award;
            Starttime = gamemallRulePb.StartTime;
            EndTime = gamemallRulePb.EndTime;
        }


    }
	
	

}


