using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Core.Events;
using Common;
using DataModel;
using game.main;
using Module.Login;

public class GameMainModule : ModuleBase
{
    private MainPanel _mainMenuPanel;

    public override void Init()
    {
        _mainMenuPanel = new MainPanel();
        _mainMenuPanel.Init(this);
        
        RegisterModel<PlayerModel>();
        
        EventDispatcher.AddEventListener<MainMenuDisplayState>(EventConst.MainMenuDisplayChange, OnMainMenuDisplayChange);
        
        GuideManager.RegisterModule(this);
    }


    public override void OnShow(float delay)
    {      
        base.OnShow(delay);
        SendMessage(new Message(MessageConst.CMD_MAIN_CHANGE_DISPLAY, MainMenuDisplayState.ShowAll));

        Main.EnableBackKey = true;
        
        AssetManager.Instance.UnloadSingleFileBundleLater();
        AssetManager.Instance.LogMessage();

        AssetLoader.UnloadAllAudio();
        
        _mainMenuPanel.OnShow();
        
        GuideManager.OpenGuide(this);
    }

    private void OnMainMenuDisplayChange(MainMenuDisplayState state)
    {
        SendMessage(new Message(MessageConst.CMD_MAIN_CHANGE_DISPLAY, state));
    }
}