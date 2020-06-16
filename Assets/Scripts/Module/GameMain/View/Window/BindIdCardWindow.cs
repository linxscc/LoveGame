using System;
using Assets.Scripts.Framework.GalaSports.Core.Events;
using Common;
using game.main;
using UnityEngine.UI;

namespace game.main
{
	public class BindIdCardWindow : Window
	{
		private InputField _userNameInput;
		private InputField _passwordInput;
		private Button cancleBtn;

		private void Awake()
		{
			_userNameInput = transform.Find("UserNameInput").GetComponent<InputField>();
			_passwordInput = transform.Find("PasswordInput").GetComponent<InputField>();
            
			Button okBtn = transform.Find("OkBtn").GetComponent<Button>();
			okBtn.onClick.AddListener(OnOkBtnClick);
			
			cancleBtn= transform.Find("CancleBtn").GetComponent<Button>();
			cancleBtn.onClick.AddListener(OnCancleClick);
		}

		private void OnCancleClick()
		{
			//当时间超过多少的时候就要直接退出游戏了！
//			PopupManager.Instance.Close();
			Close();
		}

		private void OnOkBtnClick()
		{
			if (_passwordInput.text.Length < 18)
			{
				FlowText.ShowMessage("身份证号码不得小于18位");
				return;
			}
			EventDispatcher.TriggerEvent(EventConst.BindIdCard,_userNameInput.text, _passwordInput.text);
//			PopupManager.Instance.Close();
		}

		public void SetData(string cancleTxt,Action action)
		{
			if (action!=null)
			{
				cancleBtn.GetComponentInChildren<Text>().text = cancleTxt;
				cancleBtn.onClick.RemoveAllListeners();
				cancleBtn.onClick.AddListener(() =>
				{
					action();
				});

			}
			
		}
		
	}
}