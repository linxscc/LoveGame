using Assets.Scripts.Framework.GalaSports.Service;
using DataModel;
using game.main;
using UnityEngine;
using UnityEngine.UI;

public class RetryWindow : AlertWindow
{


	[SerializeField] private Button _cancelBtn;
        
	public string CancelText
	{
		get
		{
			return _cancelBtn.GetComponentInChildren<Text>().text;
		}
		set
		{
			_cancelBtn.GetComponentInChildren<Text>().text = value;
		}
	}

	protected override void OnInit()
	{
		base.OnInit();
		_cancelBtn.onClick.AddListener(OnCancelBtn);
	}

	protected void OnCancelBtn()
	{
		WindowEvent = WindowEvent.Cancel;
		Close();
	}


	protected override void OnClickOutside(GameObject go)
	{
		
	}
}
