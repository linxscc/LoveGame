
using game.main;
using UnityEngine.UI;

public class TouristWindow : Window
{
		private Button _okBtn;

		private void Awake()
		{
			_okBtn = transform.Find("Bg/OKButton").GetComponent<Button>();
			_okBtn.onClick.AddListener((() =>
			{				
				WindowEvent = WindowEvent.Ok;
				CloseAnimation();
			}));
		}
		
}


