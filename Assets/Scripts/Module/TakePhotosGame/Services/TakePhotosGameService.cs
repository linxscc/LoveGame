using Assets.Scripts.Framework.GalaSports.Core.Events;
using Assets.Scripts.Module.NetWork;
using Com.Proto;
using Common;
using DataModel;
using Framework.GalaSports.Core;

namespace Assets.Scripts.Module.TakePhotosGame.Service
{
	public class TakePhotosGameService : ComboService<TakePhotosGameModel, TakePhotoRules, GetUserTakePhotoInfoRes>
	{
		protected override void OnExecute()
		{
			httpTimeoutU = 100;
			AddServiceData(CMD.TAKEPHOTOC_RULES, null, true);
            AddServiceData(CMD.TAKEPHOTOC_GETINFO);
		}

		protected override void ProcessData(TakePhotoRules resU, GetUserTakePhotoInfoRes resV)
		{
            _data = GetModel<TakePhotosGameModel>();
            var data= _data;
            data.InitRule(resU);
            data.InitCount(resV);

        }
	}
}