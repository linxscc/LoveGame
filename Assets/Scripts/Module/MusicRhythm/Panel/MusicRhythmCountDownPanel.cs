using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Interfaces;

public class MusicRhythmCountDownPanel : Panel
{
    MusicRhythmCountDownController _MusicRhythmCountDownController;
    public override void Init(IModule module)
    {
        base.Init(module);
        var viewScript = InstantiateView<MusicRhythmCountDownView>("MusicRhythm/Prefabs/MusicRhythmCountDownView");
        _MusicRhythmCountDownController = new MusicRhythmCountDownController();
        RegisterController(_MusicRhythmCountDownController);
        _MusicRhythmCountDownController.view = (MusicRhythmCountDownView)viewScript;
       // _MusicRhythmCountDownController.Start();
    }


   public void StartCountDown()
    {
        _MusicRhythmCountDownController.Start();
    }

    public override void Show(float delay)
    {
        Main.ChangeMenu(MainMenuDisplayState.HideAll);
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
