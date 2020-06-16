
using Assets.Scripts.Framework.GalaSports.Core;
using DataModel;
using game.main;
using UnityEngine;


public class PlayerBirthdayModule : ModuleBase
{
	private PlayerBirthdayPanel _panel;
		
	public override void Init()
	{			
		_panel =new PlayerBirthdayPanel();
		_panel.Init(this);
		_panel.Show(0.5f);
	}


	public override void LoadAssets()
	{
		AssetManager.Instance.LoadAtlas("UIAtlas_MissionActivity");		
	}

	public override void OnShow(float delay)
	{
		base.OnShow(delay);
		_panel.Show(0);			
	}
	public override void SetData(params object[] paramsObjects)
	{
		if (paramsObjects.Length > 0)
		{
			var day = (int) paramsObjects[0];
			if (day>7)
			{
				day = 7;
			}
			GlobalData.MissionModel.PlayerBirthdayMissionsDay = day;
		}

	}
	public override void OnMessage(Message message)
	{
		string name = message.Name;
		object[] body = message.Params;
		switch (name)
		{
			case MessageConst.CMD_PLAYERBITTHDAY_SHOW_TOPBAR_AND_BACKBTN:
				var isShow = (bool) body[0];
				_panel.IsShowBackBtnAndTopBar(isShow);
				break;
		}
	}
	
	
	public override void Remove(float delay)
	{
		AssetManager.Instance.UnloadAtlas("UIAtlas_MissionActivity");
		ClientTimer.Instance.RemoveCountDown("SetPlayerBirthdayCountDown");
		base.Remove(delay);
	}
}
