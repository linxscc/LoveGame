
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module.NetWork;
using Com.Proto;
using DataModel;

public class TrainingRoomController : Controller
{
	public TrainingRoomView View;

	public override void Start()
	{				
		ClientData.LoadItemDescData(null);
		ClientData.LoadSpecialItemDescData(null);
		
		View.SetData(GlobalData.TrainingRoomModel.GetTodayMusicInfo(),GlobalData.TrainingRoomModel.GetTomorrowMusicInfo());
	}
}
