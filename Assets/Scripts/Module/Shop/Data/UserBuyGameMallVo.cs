using Com.Proto;


namespace game.main
{
    public class UserBuyGameMallVo
    {
        public int UserId;
        public int MallId;
        public MallTypePB MallTypePb;
        public int BuyNum;

        public UserBuyGameMallVo(UserBuyGameMallPB userBuyGameMallVo)
        {
            UserId = userBuyGameMallVo.UserId;
            MallId = userBuyGameMallVo.MallId;
            MallTypePb = userBuyGameMallVo.MallType;
            BuyNum = userBuyGameMallVo.BuyNum;
        }

    }

    public class UserBuyRmbMallVo
    {
        public int UserId;
        public int MallId;
        public int BuyNum;
        public long RefreshTime;

        public UserBuyRmbMallVo(UserBuyRmbMallPB userBuyRmbMallPb)
        {
            UserId = userBuyRmbMallPb.UserId;
            MallId = userBuyRmbMallPb.MallId;
            BuyNum = userBuyRmbMallPb.BuyNum;
            RefreshTime = userBuyRmbMallPb.RefreshTime;
        }


    }
	

}


