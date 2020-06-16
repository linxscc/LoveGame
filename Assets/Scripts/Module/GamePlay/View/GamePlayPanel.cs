using Assets.Scripts.Common;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Interfaces;
using Assets.Scripts.Module;

public class GamePlayPanel : ReturnablePanel
{
    private GamePlayController control;
    private GamePlayView viewScript;
    public int TargetPanel=0;

    public override void Init(IModule module)
    {
        base.Init(module);
        viewScript = InstantiateView<GamePlayView>("GamePlay/Prefabs/GamePlayView");

        control = new GamePlayController();
        control.view = (GamePlayView)viewScript;
        control.Start();

        RegisterController(control);
        RegisterView(viewScript);
    }

    public override void OnBackClick()
    {
        switch (TargetPanel)
        {
            case 0:
                viewScript.GoBack();
                break;
            case 1:
                ModuleManager.Instance.GoBack();
                break;
        }
    }

    public override void Show(float delay)
    {
        base.Show(0);
        control.Show(TargetPanel);
      
        ShowBackBtn();
    }

    public void IsShowArrow(bool isShow=false)
    {
        control.view.IsShowArrow(isShow);  
    }

    public void ShowShopping()
    {
        viewScript.ShowGoWindowShopping();
    }
}