using UnityEngine;
using UnityEngine.UI;

namespace game.main
{
	public class DownloadTipsWindow : Window
	{
		[SerializeField] private Text _contenText;
		[SerializeField] private Button _okBtn;
		[SerializeField] private Text _titleText;

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

		public void SetCardShow(bool isreceive)
		{
			transform.Find("GameBg/contentText/CardIcon").gameObject.SetActive(isreceive);
			
		}

		protected override void OnInit()
		{
			base.OnInit();

			_titleText.text = "";
			_contenText.text = "";

			_okBtn.onClick.AddListener(OnOkBtn);
		}

		protected virtual void OnOkBtn()
		{
			WindowEvent = WindowEvent.Ok;
			CloseAnimation();
		}

	}
}
