using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Interfaces;
using game.main;

public class FinalEstimateCommentPanel : Panel
{
    private FinalEstimateCommentController controller;

    private string path = "Battle/FinalEstimate/FinalEstimateComment"; 

    public override void Init(IModule module)
    {
        base.Init(module);

        FinalEstimateCommentWindow win = InstantiateWindow<FinalEstimateCommentWindow>(path);

        controller = new FinalEstimateCommentController();       
        RegisterController(controller);
        
        controller.Window = win;
        
        controller.Start();
    }
}
