using Assets.Scripts.Framework.GalaSports.Core;
using DataModel;

public class AnnouncementController : Controller
{

    public AnnouncementView View;





    public override void Start()
    {
        var notice = GlobalData.NoticeData.GetNoticeInfo();
        var title = notice.Title;
        var content = notice.Content;
        View.SetData(title,content);
    }

    public override void OnMessage(Message message)
    {
        string name = message.Name;
        object[] body = message.Params;
        switch (name)
        {
            case MessageConst.CMD_LOGIN_ANNOUNCEMENT:
                View.Show();
                break;
            
        }
    }


    public override void Destroy()
    {
        base.Destroy();
    }
}
