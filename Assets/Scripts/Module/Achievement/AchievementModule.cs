using Assets.Scripts.Framework.GalaSports.Core;
using Common;
using game.main;

public class AchievementModule : ModuleBase
{
	private AchievementChoosePanel _achievementChoosePanel;
	private AchiementListPanel _achiementListPanel;



	public override void Init()
	{
		GuideManager.RegisterModule(this);
		
		_achievementChoosePanel=new AchievementChoosePanel();
		_achievementChoosePanel.Init(this);
		RegisterModel<MissionModel>();
		_achievementChoosePanel.Show(0);
	}

	public override void OnShow(float delay)
	{
		base.OnShow(delay);
		_achiementListPanel?.Show(0);
		_achiementListPanel?.RefreshMission();

	}
    
	public override void OnMessage(Message message)
	{
		string name = message.Name;
		object[] body = message.Params;
		switch (name)
		{
             case MessageConst.CMD_CHOOSEROLE:
	             if (_achiementListPanel==null)
	             {
		             _achiementListPanel=new AchiementListPanel();
		             _achiementListPanel.Init(this);
	             }
	             _achievementChoosePanel.Hide();
	             _achiementListPanel.Show(0);
	             if (message.Body==null)
	             {
		             _achiementListPanel.SetRoleItem(0); 
	             }
	             else
	             {
		             _achiementListPanel.SetRoleItem((int)message.Body); 
	             }


	             break;
             case MessageConst.CMD_ACHIEVEMENTBACK:
	             _achiementListPanel.Hide();
	             _achievementChoosePanel.Show(0);
	             _achievementChoosePanel.OnShowChooseView();
	             break;
		}
	}

    
	public void Destroy()
	{
	}
} 

