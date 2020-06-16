using Com.Proto;

namespace game.main
{
	public class MallItemVo
	{
		public int MallId;
		public int ItemId;
		public MoneyTypePB MoneyTypePb;
		public string ItemDesc;
		public string UseDesc;
		public int Num;
		public int Price;
		public int MaxNum;

		public MallItemVo(MallItemRulePB mallItemRulePb)
		{
			MallId = mallItemRulePb.MallId;//商店
			ItemId = mallItemRulePb.ItemId;     //道具Id
			MoneyTypePb = mallItemRulePb.MoneyType;// 货币类型
			ItemDesc = mallItemRulePb.IteamDes;//道具描述           
			UseDesc = mallItemRulePb.UseDes;//使用描述
			Num = mallItemRulePb.Num;//道具数量
			Price = mallItemRulePb.Price;//价格
			MaxNum = mallItemRulePb.MaxNum;//每天购买道具次数上限


		}


	}
	
	

}


