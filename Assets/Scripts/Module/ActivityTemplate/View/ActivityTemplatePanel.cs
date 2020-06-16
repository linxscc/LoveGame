
using Assets.Scripts.Common;
using Assets.Scripts.Framework.GalaSports.Interfaces;


public class ActivityTemplatePanel : ReturnablePanel
{


      private ActivityTemplateController _controller;
      public override void Init(IModule module)
      {
            base.Init(module);
            
            var viewScript = InstantiateView<ActivityTemplateView>("ActivityTemplate/Prefabs/ActivityTemplateView");
            _controller = new ActivityTemplateController {View = viewScript};
            RegisterView(viewScript);
            RegisterController(_controller);
            RegisterModel<ActivityTemplateModel>();
            _controller.Start();
           
      }


      public override void Show(float delay)
      {
            Main.ChangeMenu(MainMenuDisplayState.ShowTopBar);
            ShowBackBtn();            
      }


      public void OnShow()
      {
           _controller.View.OnShow();
      }
      
      public void IsShowBackBtnAndTopBar(bool isShow)
      {
            if (isShow)
            {
                  ShowBackBtn();
                  Main.ChangeMenu(MainMenuDisplayState.ShowTopBar);
            }
            else
            {
                  HideBackBtn();
                  Main.ChangeMenu(MainMenuDisplayState.HideAll);
            }
      }

      public void DestroyCountDown()
      {
            _controller.View.DestroyCountDown();
      }
      
      
}
