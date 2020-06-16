using Assets.Scripts.Common;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Interfaces;
using game.main;
using UnityEngine;
using UnityEngine.UI;

public class FullScreenCardPanel : ReturnablePanel {
	private FullScreenCardView view;

	public override void Init(IModule module)
	{
		base.Init(module);

		view = (FullScreenCardView)InstantiateView<FullScreenCardView>("Card/Prefabs/CardDetail/FullScreenCardView");
	}

    public override void Hide()
    {
        
    }

    public override void Show(float delay)
    {
        Main.ChangeMenu(MainMenuDisplayState.HideAll);
	    HideBackBtn();
    }

	public void SetTexture(RawImage texture,RawImage signature)
	{
		
		view.SetTexture(texture,signature);
	}
}
