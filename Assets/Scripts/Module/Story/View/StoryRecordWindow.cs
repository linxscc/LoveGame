using game.main;
using UnityEngine;
using UnityEngine.UI;

public class StoryRecordWindow : Window 
{
	private Text _text;

	private void Awake()
	{
		transform.Find("CloseBtn").GetComponent<Button>().onClick.AddListener(Close);
		_text = transform.Find("Scroll View/Viewport/Text").GetComponent<Text>();
	}

	public void SetData(string dialog)
	{
		dialog = Util.GetNoBreakingString(dialog);
		_text.text = dialog;
	}
}


