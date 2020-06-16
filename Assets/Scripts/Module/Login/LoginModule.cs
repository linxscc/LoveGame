using Assets.Scripts.Framework.GalaSports.Core;
using Common;
using DataModel;
using Module.Login.View;

public class LoginModule : ModuleBase
{
    private LoginPanel _panel;
    private AnnouncementPanel _announcementPanel;
    private StopRunningAnnouncementPanel _stopRunningAnnouncement;

    LoginCallbackType _isCallbackTypeSwitch = LoginCallbackType.None;

    public override void Init()
    {
        GuideManager.Setup();
        I18NManager.LoadGalaSDKLanguageConfig((I18NManager.LanguageType)AppConfig.Instance.language);
        _panel = new LoginPanel();
        _panel.SetData(_isCallbackTypeSwitch);
        _panel.Init(this);

        _panel.Show();
 
        if (AppConfig.Instance.needChooseServer == false)
            return;

        if (GlobalData.NoticeData.IsStopService())
        {
            _stopRunningAnnouncement = new StopRunningAnnouncementPanel();
            _stopRunningAnnouncement.Init(this);
            _stopRunningAnnouncement.Show(0.5f);
        }
        else
        {
            var notice = GlobalData.NoticeData.GetNoticeInfo();
            if (notice != null)
            {
                _announcementPanel = new AnnouncementPanel();
                _announcementPanel.Init(this);
                _announcementPanel.Show(0.5f);
            }
        }



    }

    public override void SetData(params object[] paramsObjects)
    {
        _isCallbackTypeSwitch = LoginCallbackType.None;
        if (paramsObjects.Length > 0)
        {
            _isCallbackTypeSwitch = (LoginCallbackType)paramsObjects[0];
        }
    }
}