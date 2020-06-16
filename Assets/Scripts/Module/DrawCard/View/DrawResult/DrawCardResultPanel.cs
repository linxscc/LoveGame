using Assets.Scripts.Common;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Interfaces;
using Assets.Scripts.Module.DrawCard;
using Com.Proto;
using Google.Protobuf.Collections;
using System.Collections.Generic;

public class DrawCardResultPanel : ReturnablePanel 
{
	private DrawCardResultController control;

	public override void Init(IModule module)
	{
	    base.Init(module);

		IView viewScript = InstantiateView<DrawCardResultView>("DrawCard/Prefabs/DrawResultView");

		control = new DrawCardResultController();
		control.View = (DrawCardResultView)viewScript;
		RegisterController(control);
	}

	public override void OnBackClick()
	{
        //base.OnBackClick();
        control.GoBack();
	}
	public void SetData(List<DrawCardResultVo> awardPbs)
	{
        control.SetData(awardPbs);

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
