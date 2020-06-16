using Assets.Scripts.Common;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Interfaces;

public class SongChoosePanel : ReturnablePanel
{
    private SongChooseController _controller;

    private string Path = "TrainingRoom/Prefabs/SongChooseView";
    
    public override void Init(IModule module)
    {
        base.Init(module);
        var viewScript = InstantiateView<SongChooseView>(Path);        
        _controller =new SongChooseController();
        _controller.View = viewScript;		
        RegisterView(viewScript);
        RegisterController(_controller);		
        _controller.Start();       
    }

    public override void OnBackClick()
    {       
      Destroy();
      SendMessage(new Message(MessageConst.MODULE_TRAININGROOM_SHOW_BACKBTN));
    }
}
