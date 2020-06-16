
using Assets.Scripts.Framework.GalaSports.Core;


public class ActivityTemplateModule : ModuleBase
{
	private ActivityTemplatePanel _panel;

	public override void Init()
	{
		_panel =new ActivityTemplatePanel();
		_panel.Init(this);
		_panel.Show(0.5f);
	}
	
	
	public override void OnShow(float delay)
	{
		base.OnShow(delay);
		_panel.Show(0);
		_panel.OnShow();
	}
	
	
	public override void OnMessage(Message message)
	{
		string name = message.Name;
		object[] body = message.Params;
		switch (name)
		{
			case MessageConst.CMD_ACTIVITYTEMPLATE1_ACTIVITY_SHOW_TOPBAR_AND_BACKBTN:
				var isShow = (bool) body[0];
				_panel.IsShowBackBtnAndTopBar(isShow);
				break;
		}
	}

	public override void Remove(float delay)
	{
		base.Remove(delay);
	  _panel.DestroyCountDown();	
	}
}
