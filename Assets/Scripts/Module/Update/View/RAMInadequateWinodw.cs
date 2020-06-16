using game.main;
using UnityEngine;
using UnityEngine.UI;

public class RAMInadequateWinodw : Window
{
    private Button _okBtn;
    private Text _content;
    private string _url;

    private void Awake()
    {
        _okBtn = transform.GetButton("OkBtn");
        _content = transform.Find("Text").GetComponent<TextI18N>();
        _okBtn.onClick.AddListener(OpenUrl); //确定按钮就是退出游戏
    }

    private void OpenUrl()
    {
        Application.OpenURL(_url);
    }

    public void SetDate(string content, string url)
    {
        content = Util.GetNoBreakingString(content);  
        
        _content.text = content;
        _url = url;
    }
}
