using Assets.Scripts.Framework.GalaSports.Core;
using game.main;
using UnityEngine;
using UnityEngine.UI;
using GalaSDKBase;

public class StopRunningAnnouncementView : View {

	private Text _title;
	private Text _content;
	private Button _okBtn;
	private Text _top;

	private void Awake()
	{
		_title = transform.GetText("Content/Bg/Title");
		_content = transform.GetText("Content/Bg/InfoBg/Text");
		_okBtn = transform.GetButton("Content/Button");
		_top = transform.GetText("Content/Top/Text");
		
		_okBtn.onClick.AddListener(Quit);
	}

	private void Quit()
	{
        string starWay = GalaSDKBaseFunction.GetAPPStartWay();
        if (starWay.Contains("lianouneedlog") && starWay.Contains("true"))
        {
			Hide();
		}
		else
        {
	        PopupManager.ShowAlertWindow(I18NManager.Get("Login_Announcement3"));
        }
	}

	public void SetData(string title,string content)
	{
		content = Util.GetNoBreakingString(content);
		_top.text = I18NManager.Get("Login_Announcement2");
		_title.text = "";
		_content.text = "";
		_title.text = title;
		
		_content.text = content;
	}
}
