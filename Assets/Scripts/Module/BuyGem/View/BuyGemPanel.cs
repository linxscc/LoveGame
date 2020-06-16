using Assets.Scripts.Common;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Interfaces;
using Assets.Scripts.Module;
using UnityEngine;

namespace game.main
{
	public class BuyGemPanel : ReturnablePanel
	{


		private BuyGemView _buyGemView;
		private BuyGemController _buygemController;

        


		public override void Init(IModule module)
		{
			base.Init(module);

			_buyGemView = (BuyGemView) InstantiateView<BuyGemView>("Shop/Prefab/BuyGemView");
			_buygemController = new BuyGemController();
			RegisterController(_buygemController);
			_buygemController.View = _buyGemView;
            
           
			_buygemController.Start();

		}

		public override void Show(float delay)
		{
			base.Show(0);
			Main.ChangeMenu(MainMenuDisplayState.ShowTopBar);
		}



		public override void Destroy()
		{
			Unregister();
			Module.Destroy(_showObject);
		}
	}
}