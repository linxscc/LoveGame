using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Interfaces;

public class UpdatePanel : Panel
{

    LoginCallbackType _isCallbackTypeSwitch = LoginCallbackType.None;
    public override void Init(IModule module)
    {
        base.Init(module);

        UpdateView view = InstantiateView<UpdateView>("Update/Prefabs/UpdateView");
        
        UpdateController updateController = new UpdateController();
        HotfixController hotfixController = new HotfixController();
        ForceUpdateController forceUpdateController = new ForceUpdateController();

        RegisterController(hotfixController);
        RegisterController(updateController);
        RegisterController(forceUpdateController);

        updateController.View = view;
        hotfixController.View = view;
        forceUpdateController.View = view;
        updateController.SetData(_isCallbackTypeSwitch);
        updateController.Start();
    }


    public void SetData(LoginCallbackType isCallbackTypeSwitch)
    {
        _isCallbackTypeSwitch = isCallbackTypeSwitch;
    }
    public override void Hide()
    {
    }

    public override void Show(float delay)
    {
    }
}