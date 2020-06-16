using Assets.Scripts.Framework.GalaSports.Core;
using game.main;
using System;
using UnityEngine;
using UnityEngine.UI;

public class UpdateView : View 
{
	private Text _text;
	 
    private GameObject _announcement;
    private ProgressBar _progressBar;
    private Button _okBtn;
    private RectTransform _smallStar;
    private RectTransform _bigStar;
    private RectTransform _starRectTra;
    private RectTransform _maskRect;


    private void Awake()
	{
		_text = transform.GetText("Text");
		_progressBar = transform.Find("ProgressBar").GetComponent<ProgressBar>();
		_progressBar.gameObject.Hide();
		
        _announcement = transform.Find("Announcement").gameObject;
        _announcement.Hide();
        
        _okBtn = _announcement.transform.GetButton("Bg/Bottom/OkBtn");

        Text versionText = transform.GetText("VersionText");
        versionText.text = "V" + AppConfig.Instance.versionName;
        
        _smallStar = transform.Find("ProgressBar/Star/smallStar").GetComponent<RectTransform>();
        _bigStar = transform.Find("ProgressBar/Star/bigStar").GetComponent<RectTransform>();
        
        _starRectTra = transform.Find("ProgressBar/Star").GetComponent<RectTransform>();
        _maskRect = transform.Find("ProgressBar/Mask").GetComponent<RectTransform>();
	}

    private void Update()
    {
	    if (gameObject.activeSelf)
	    {
		    _smallStar.Rotate(-Vector3.forward*Time.deltaTime *500.0f);
		    _bigStar.Rotate(-Vector3.forward *Time.deltaTime *500.0f);
            
		    _starRectTra.localPosition = new Vector3(_maskRect.GetWidth(), _starRectTra.localPosition.y, 0);
	    }
    }
    
	public void SetText(string text)
	{
		_text.text = text;
	}

	public void SetProgress(int progress)
	{
		if(_progressBar == null || _progressBar.gameObject == null)
			return;
		
		_progressBar.gameObject.SetActive(true);
		_progressBar.Progress = progress;
	}

	public void ShowRetry(string msg)
	{
		PopupManager.ShowRetryWindow(msg).WindowActionCallback = evt =>
		{
			if (evt == WindowEvent.Ok)
			{
				SendMessage(new Message(MessageConst.CMD_UPDATE_RETRY));    
			}

			if (evt ==WindowEvent.Cancel)
			{
				Application.Quit();
			}
		};
	}

    //显示热更公告
    public void ShowAnnouncement(string content)
    {
	    ClientTimer.Instance.DelayCall(()=>
	    {
		    _announcement.Show();
	    }, 0.5f);
        
        content = Util.GetNoBreakingString(content);
        _announcement.transform.GetText("Bg/Content/Text").text = content;

        _okBtn.onClick.AddListener(() =>
        {
	        ShowUpdateBtn(false);
	        SendMessage(new Message(MessageConst.CMD_UPDATE_START_DOWNLOAD_HOTFIX_FILE));
        });
    }


    public void ShowUpdateBtn(bool isShow)
    {
	    _okBtn.gameObject.SetActive(isShow);
    }
}


