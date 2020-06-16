using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Interfaces;
using Common;
using DataModel;
using game.main;
using UnityEngine;

namespace Module.Login
{
    public class MainPanel : Panel
    {
        private MainPanelController _mainControl;
        private UserInfoController _userInfoController;

        public override void Init(IModule module)
        {
            base.Init(module);

            var viewScript = InstantiateView<MainPanelView>("GameMain/Prefabs/MainPanel");
          
            _mainControl = new MainPanelController();
            _mainControl.view = viewScript;
            
            _userInfoController = new UserInfoController();
            _userInfoController.View = viewScript.UserInfoView;

            RegisterController(_mainControl);
            RegisterController(_userInfoController);
            
            RegisterModel<ActivityPopupWindowModel>();
           
            _mainControl.Start();

//            if (GuideManager.GetRemoteGuideStep(GuideTypePB.MainGuide) < GuideConst.MainStep_MainStory1_6_Fail)
//            {
//                viewScript.ShowAll(false);
//            }
        }

        public override void Hide()
        {
           
            ClientTimer.Instance.DelayCall(Destroy, 0.4f);
        }

        public override void Show(float delay = 0.1f)
        {
            
            
        }

        public void OnShow()
        {
            
            
            _mainControl.view.OnShow();
        }
    }
}