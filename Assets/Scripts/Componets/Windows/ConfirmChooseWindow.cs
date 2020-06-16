using UnityEngine;
using UnityEngine.UI;

namespace game.main
{
	public class ConfirmChooseWindow : Window
	{
		[SerializeField] private Button _okBtn;
		
		[SerializeField] private Button _cancleBtn;
		[SerializeField] private Text _contenText;
		[SerializeField] private Toggle _neverNotify;

		public int RecordCardId;//不再提醒的记录
		
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
		
		public string Content
		{
			get { return _contenText.text; }
			set { _contenText.text = value; }
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

			
			_contenText.text = "";
			OkText =I18NManager.Get("Common_OK1");
			_okBtn.onClick.AddListener(OnOkBtn);
			_cancleBtn.onClick.AddListener(OnCancelBtn);
			_neverNotify.onValueChanged.AddListener(OnNeverNotify);
		}

		private void OnNeverNotify(bool state)
		{
			//存储到本地是否要提醒。
			PlayerPrefs.SetInt("RecordLoveStory"+RecordCardId,state?0:1);
		}

		public void InitDownloadInfo()
		{
			
		}

		protected override void OnClickOutside(GameObject go)
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

		
		
	}
}
