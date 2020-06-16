using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Interfaces;
using Assets.Scripts.Module.CreateUser;

public class CreateUserPanel : Panel {
	
	private CreateUserWindow view;

	public override void Init(IModule module)
	{
		base.Init(module);
		
//		view = InstantiateView<CreateUserView>("CreateUser/Prefabs/CreateUserView");

		view = InstantiateWindow<CreateUserWindow>("CreateUser/Prefabs/CreateUserWindow");
	}

//	public void EnterStory(bool isSuccess)
//	{
//		view.EnterStory(isSuccess);
//	}
}
