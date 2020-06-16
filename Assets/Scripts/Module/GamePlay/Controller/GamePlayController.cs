using Assets.Scripts.Framework.GalaSports.Core;
using Common;

public class GamePlayController : Controller {
    
    public GamePlayView view;

    
    
    
    public override void Start()
    {
        
    }
    /// <summary>
    /// 处理View消息
    /// </summary>
    /// <param name="message"></param>
    public override void OnMessage(Message message)
    {
        string name = message.Name;
        object[] body = message.Params;
        switch (name)
        {
            case EventConst.GameplayGotoCity:
                
                break;;
        }
    }

    public void Show(int target=0)
    {
        view.gameObject.Show();
        view.SetRedPoint();
        switch (target)
        {
            case 0:
                break;
            case 1:
                view.ShowGoWindowShopping();   
                break;
        }
        
        
        Main.ChangeMenu(MainMenuDisplayState.ShowTopBar);
    }

    public void Hide()
    {
        view.gameObject.Hide();
    }
}
