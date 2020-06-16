using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Interfaces;

public class AirborneGameCountDownPanel : Panel
{
    AirborneGameCountDownController _airborneGameCountDownController;
    public override void Init(IModule module)
    {
        base.Init(module);
        var viewScript = InstantiateView<AirborneGameCountDownView>("AirborneGame/Prefabs/AirborneGameCountDownView");
        _airborneGameCountDownController = new AirborneGameCountDownController();
        RegisterController(_airborneGameCountDownController);
        _airborneGameCountDownController.View = (AirborneGameCountDownView)viewScript;
        _airborneGameCountDownController.Start();
    }

    public override void Show(float delay)
    {
        base.Show(delay);
    }

    public override void Hide()
    {
        base.Hide();
    }

    public override void Destroy()
    {
        base.Destroy();
    }

}
