using Assets.Scripts.Framework.GalaSports.Core;
using UnityEngine.UI;

public class AnnouncementView : View
{
    private Button _black;

    private Text _title;
    private Text _content;
    private Button _okBtn;
    private Text _top;

    private void Awake()
    {
        _black = transform.GetButton("Blank");
        _title = transform.GetText("Content/Bg/Title");
        _content = transform.GetText("Content/Bg/InfoBg/Text");

        _okBtn = transform.GetButton("Content/Button");

        _top = transform.GetText("Content/Top/Text");

        _black.onClick.AddListener(HideAnnouncementView);

        _okBtn.onClick.AddListener(HideAnnouncementView);
    }

    private void HideAnnouncementView()
    {
        Hide();
    }

    public void SetData(string title, string content)
    {
        content = Util.GetNoBreakingString(content);
        _top.text = I18NManager.Get("Login_Announcement1");
        _title.text = "";
        _content.text = "";
        _title.text = title;
        _content.text = content;
    }
}