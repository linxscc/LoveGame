using Assets.Scripts.Common;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Interfaces;
using Assets.Scripts.Module;

namespace game.main
{
    public class ShopMainPanel : Panel
    {


        private ShopMainView _shopMainView;
        private ShopMainController _shopMainController;
        public int JumpTarget=0;
        


        public override void Init(IModule module)
        {
            SetComplexPanel();
            base.Init(module);

            _shopMainView =  InstantiateWindow<ShopMainView>("Shop/Prefab/ShopView");
            _shopMainController = new ShopMainController();
            RegisterController(_shopMainController);
            _shopMainController.View = _shopMainView;
            _shopMainController.TargetPage = JumpTarget;
           
            _shopMainController.Start();

        }

        public override void Show(float delay)
        {
            base.Show(0);
           Main.ChangeMenu(MainMenuDisplayState.ShowTopBar);
//            ShowBackBtn();
            _shopMainView.Show();
        }

        public void GoBack()
        {
//            ModuleManager.Instance.Remove(ModuleConfig.MODULE_SHOP);//可以解决右上角弹窗的问题！
//            RegisterModel<ShopModel>();
            ModuleManager.Instance.GoBack();
        }


    }
}