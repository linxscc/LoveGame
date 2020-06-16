using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Interfaces;

public class VisitFinalEstimateCommentPanel : Panel {
	
	public override void Init(IModule module)
    {
        SetComplexPanel();
        base.Init(module);
        var view = InstantiateView<VisitFinalEstimateCommentView>("VisitBattle/FinalEstimate/FinalEstimateComment");
        var controller=new VisitFinalEstimateCommentController();
        controller.View = (VisitFinalEstimateCommentView) view;
        RegisterController(controller);
    }

    public override void Hide()
    {
        
    }

    public override void Show(float delay)
    {
        
    }

    public override void Destroy()
    {
        base.Destroy();
    }
}
