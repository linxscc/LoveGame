using Com.Proto;
using DataModel;

namespace Assets.Scripts.Module.Pay.Agent
{
    public class PayHelper
    {
        public static void SetGlobal(CheckOrderRess resList)
        {
            if (resList.UserExtraInfo != null)
            {
                GlobalData.PlayerModel.PlayerVo.ExtInfo = resList.UserExtraInfo;
            }
            if (resList.FirstRecharges != null)
            {
                GlobalData.PlayerModel.PlayerVo.FirstRecharges = resList.FirstRecharges;
            }

            if (resList.UserMonthCard != null)
            {
                GlobalData.PlayerModel.PlayerVo.UserMonthCard = resList.UserMonthCard;
            }
        }
    }
}