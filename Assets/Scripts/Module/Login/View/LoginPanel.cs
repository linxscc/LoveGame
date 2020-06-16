using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Interfaces;
using game.main;

namespace Module.Login.View
{
    public class LoginPanel : Panel
    {
        private LoginControl control;
        
        private LoadDataController loadDataControl;

        LoginCallbackType _isCallbackTypeSwitch = LoginCallbackType.None;
        public override void Init(IModule module)
        {
            base.Init(module);

            var viewScript = InstantiateView<LoginView>("Login/Prefabs/LoginModule");
    
            control = new LoginControl();
            control.view = (LoginView)viewScript;

            loadDataControl = new LoadDataController();

            RegisterController(control);
            RegisterController(loadDataControl);
            control.SetData(_isCallbackTypeSwitch);
            control.Start();
        }
        
        public void SetData(LoginCallbackType isCallbackTypeSwitch)
        {
            _isCallbackTypeSwitch = isCallbackTypeSwitch;
        }

        public override void Hide()
        {
            ClientTimer.Instance.DelayCall(Destroy, 0.4f);
        }

        public override void Show(float delay=0.1f)
        {
           // control.Show();
        }
    }
}