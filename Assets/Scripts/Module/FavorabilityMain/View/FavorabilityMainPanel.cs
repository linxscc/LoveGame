using Assets.Scripts.Common;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Core.Events;
using Assets.Scripts.Framework.GalaSports.Interfaces;
using Common;
using game.main;


public class  FavorabilityMainPanel : ReturnablePanel
{
    private FavorabilityMainController _favorabilityMainController;
    public bool IsCardJumpTo = false;
    public override void Init(IModule module)
    {
        base.Init(module);
        var viewSprite = InstantiateView<FavorabilityMainView>("FavorabilityMain/Prefabs/FavorabilityMainView");
        _favorabilityMainController=new FavorabilityMainController();
        _favorabilityMainController.FavorabilityMainView = viewSprite;
        RegisterController(_favorabilityMainController);
        _favorabilityMainController.Start();       
    }

    public override void OnBackClick()
    {
        if (IsCardJumpTo)
        {
            SendMessage(new Message(MessageConst.CMD_FACORABLILITY_GOBACK));
        }
        else
        {
            SendMessage(new Message(MessageConst.CMD_FACORABLILITY_BACKTOFAVORABILITY));
            AudioManager.Instance.StopDubbing();    
        }
    }



    public override void Show(float delay)
    {
        base.Show(0);
        Main.ChangeMenu(MainMenuDisplayState.HideAll);
        ShowBackBtn();
        _favorabilityMainController.SetView();
    }
    
   

    public void ShowView()
    {
        _favorabilityMainController.FavorabilityMainView.Show();
    }
 
    public void OnShowMainBtn()
    {
        _favorabilityMainController.OnShowMainBtn();
    }


    public void JumpTo(string name)
    {
      _favorabilityMainController.JumpTo(name);
    }
    
}