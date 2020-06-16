using System;
using UnityEngine;
using UnityEngine.UI;

namespace game.main
{
	public class DownloadingWindow : Window
	{

		[SerializeField] private Button _cancleBtn;
		[SerializeField] private Text _contenText;
		[SerializeField] private ProgressBar _progressBar;
		[SerializeField]private Text _progressText;
		
		public string Content
		{
			get { return _contenText.text; }
			set
			{
//				Debug.LogError(value);
				_contenText.text = value;
			}
		}


		public string Progress
		{
			get { return _progressText.text; }
			set { _progressText.text = value; }
		}
        
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

		public void SetProgress(int progress)
		{
			_progressBar.Progress = progress;
		}

		public void HindCancelBtn()
		{
			_cancleBtn.gameObject.Hide();
		}

		protected override void OnInit()
		{
			base.OnInit();

			CancelText =  I18NManager.Get("Common_Cancel2");
			_cancleBtn.onClick.AddListener(OnCancelBtn);
		}

		public void InitDownloadingInfo()
		{
			
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
