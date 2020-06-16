using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Module.NetWork;
using Com.Proto;
using DataModel;
using Framework.GalaSports.Core;

namespace Assets.Scripts.Services
{
	public class ShopService : ComboService<ShopModel, MallRuleRes, MallInfoRes>
	{
		protected override void OnExecute()
		{
			httpTimeoutU = 100;
			AddServiceData(CMD.MALL_RULE);
			AddServiceData(CMD.MALL_USERINFO);
			
		}

		protected override void ProcessData(MallRuleRes mallRuleRes,MallInfoRes mallInfoRes)
		{
			_data=new ShopModel();
			_data.InitRule(mallRuleRes);
			_data.InitUserMallInfo(mallInfoRes);
			SetModel(_data);
			GlobalData.PlayerModel.PlayerVo.HasGetFreeGemGift = _data.HasFreeGemMall();
		}
	}
}