using Assets.Scripts.Framework.GalaSports.Core;
using DataModel;
public class StopRunningAnnouncementController : Controller
{

	public StopRunningAnnouncementView View;
	
	public override void Start()
	{
		var notice = GlobalData.NoticeData.GetNoticeInfo();
		var title = notice.Title;
		var content = notice.Content;
		View.SetData(title,content);
		
	}
	
	
}
