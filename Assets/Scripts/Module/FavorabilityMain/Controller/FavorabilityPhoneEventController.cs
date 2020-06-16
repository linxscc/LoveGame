using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module;
using Assets.Scripts.Module.NetWork;
using Com.Proto;
using DataModel;
using game.main;


public class FavorabilityPhoneEventController : Controller
{

	public FavorabilityPhoneEventView _favorabilityPhoneEventView;  

	public override void Start()
	{
		//此处拉用户好感度数据。

		SetView();

	}


	public void SetView()
	{
		_favorabilityPhoneEventView.SetData(GlobalData.FavorabilityMainModel.CurrentRoleVo);
	}

	
   

	/// <summary>
	/// 处理View消息
	/// </summary>
	/// <param name="message"></param>
	public override void OnMessage(Message message)
	{
		string name = message.Name;
		object[] body = message.Params;
		switch (name)
		{
			case MessageConst.MODULE_DISIPOSITION_SHOW_CLOTHPANEL:
				//跳转到换衣服窗口
				ModuleManager.Instance.EnterModule(ModuleConfig.MODULE_RELOADING, false);
				//ModuleManager.Instance.EnterModule(ModuleConfig.MODULE_LOVEAPPOINTMENT);
				break;
			case MessageConst.CMD_FACORABLILITY_JUMPTOCRADS:
				//跳转到星缘界面，
				ModuleManager.Instance.EnterModule(ModuleConfig.MODULE_CARD,false,false, (UserCardVo)message.Body);
				break;
	        
		}
	}  
}
