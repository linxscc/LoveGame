using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Module.Supporter.Data;
using game.main;
using Module.Supporter.Data;

public class FansController : Controller {
	
	public FansView View;
	public FansModel _model;

	public override void Start()
	{
		List<FansVo> fansList = GetData<SupporterModel>().FansList;
		View.SetData(fansList);
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
	        
        }
    }

	

	
	public void GoBack()
	{
		SendMessage(new Message(MessageConst.MODULE_SUPPOTER_BACK_SUPPOTER));
	}
}
