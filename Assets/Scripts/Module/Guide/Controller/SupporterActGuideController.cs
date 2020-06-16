using Assets.Scripts.Framework.GalaSports.Core;
using Module.Guide.ModuleView.Supporter;
using UnityEngine;

namespace Game.Guide
{
	public class SupporterActGuideController : Controller
	{


		public SupporterActGuideView _SupporterActGuideView;
		
		public override void Start()
		{
        
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
                case MessageConst.MOUDLE_GUIDE_RECEIVE_REMOTE_STEP:
	                SendMessage(new Message(MessageConst.CMD_SUPPORTERACTIVITY_GUIDETOFANSMODULE,Message.MessageReciverType.UnvarnishedTransmission));
	                _SupporterActGuideView.EnterActivity(); 
	                break;
				//还需要一个活动成功后的返回！
				case MessageConst.MOUDLE_GUIDE_SUPPORTERACT_STARTSUCCESS:
					_SupporterActGuideView.StartActSuccess();
					Destroy();
					break;
				
			}
		}
	
	
	}

}

