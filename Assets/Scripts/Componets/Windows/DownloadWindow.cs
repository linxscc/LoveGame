using System;
using UnityEngine;
using UnityEngine.UI;

namespace game.main
{
	public class DownloadWindow : Window
	{
		[SerializeField] private Button _okBtn;
		[SerializeField] private Text _titleText;
		[SerializeField] private Button _cancleBtn;
		[SerializeField] private LoopVerticalScrollRect _loopVerticalScrollRect;
		public string CancelText
		{
			get
			{
				return _cancleBtn.GetComponentInChildren<Text>().text;
			}
			set
			{
				_cancleBtn.GetComponentInChildren<Text>().text = value;
			}
		}
		
		public string Title
		{
			get { return _titleText.text; }
			set { _titleText.text = value; }
		}

		public string OkText
		{
			get
			{
				return _okBtn.GetComponentInChildren<Text>().text;
			}
			set
			{
				_okBtn.GetComponentInChildren<Text>().text = value;
			}
		}

		protected override void OnInit()
		{
			base.OnInit();

			_titleText.text = "";

			_okBtn.onClick.AddListener(OnOkBtn);
			_cancleBtn.onClick.AddListener(OnCancelBtn);
		}

		public void InitDownloadInfo()
		{
			
		}

		protected virtual void OnOkBtn()
		{
			WindowEvent = WindowEvent.Ok;
			CloseAnimation();
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
}
