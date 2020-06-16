using Assets.Scripts.Common;
using Assets.Scripts.Framework.GalaSports.Core.Events;
using Assets.Scripts.Framework.GalaSports.Interfaces;
using Common;
using game.main;

public class RecollectionPanel : ReturnablePanel 
{
	private RecollectionController _controller;

	public override void Init(IModule module)
	{
	    base.Init(module);

	    IView view = InstantiateView<RecollectionView>("Recollection/Prefabs/RecollectionView");
		RegisterView(view);
		
		_controller = new RecollectionController();
		_controller.View = (RecollectionView)view;
		
		RegisterController(_controller);
		
		EventDispatcher.AddEventListener<UserCardVo>(EventConst.RecollectionCardClick, OnCardSelected);
	}

    public override void Hide()
    {
        
    }

    public override void Show(float delay)
    {
        ShowBackBtn();
        Main.ChangeMenu(MainMenuDisplayState.ShowRecollectionTopBar);
    }

	private void OnCardSelected(UserCardVo vo)
	{
		_controller.SelectedCard(vo);
	}

	public override void Destroy()
	{
		base.Destroy();
		EventDispatcher.RemoveEvent(EventConst.RecollectionCardClick);
	}
}
