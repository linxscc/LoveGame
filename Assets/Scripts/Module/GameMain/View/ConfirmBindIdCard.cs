using Assets.Scripts.Framework.GalaSports.Core.Events;
using Common;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace game.main
{
	public class ConfirmBindIdCardWindow : Window
	{
		private Button _closeBtn;
		private Text _contenText;
		private Button _okBtn;
		private Text _titleText;

		public string Title
		{
			get { return _titleText.text; }
			set { _titleText.text = value; }
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

		protected void Awake()
		{

			_titleText = transform.Find("Title/Text").GetComponent<Text>();
//			_contenText.text = "";
			_contenText = transform.Find("contentText").GetComponent<Text>();
			_okBtn = transform.Find("OkBtn").GetComponent<Button>();
			_closeBtn = transform.Find("CancleBtn").GetComponent<Button>();

			_okBtn.onClick.AddListener(OnOkBtn);
			_closeBtn.onClick.AddListener(() =>
			{
				Close();//PopupManager.Instance.
			});
		}

		public void SetData(string content, bool enableOk=true,bool needtoquit=false)
		{
			//_titleText.text = title;
			_contenText.text = content;
			_okBtn.enabled = enableOk;
			_okBtn.image.color = enableOk ? Color.white : Color.grey;
			if (!enableOk)
			{
				OkText = "已认证身份";
				_closeBtn.GetComponentInChildren<Text>().text = "退出";
				_closeBtn.onClick.RemoveAllListeners();
				_closeBtn.onClick.AddListener(QuitGame);
			}

			if (needtoquit)
			{
				_closeBtn.GetComponentInChildren<Text>().text = "退出";
				_closeBtn.onClick.RemoveAllListeners();
				_closeBtn.onClick.AddListener(QuitGame);
			}
			
		}

		private void QuitGame()
		{
//			Application.Quit();
			Close();
		}

		private void OnOkBtn()
		{
			//EventDispatcher.TriggerEvent(EventConst.ShowConfirmBind);
		}

	}
}
