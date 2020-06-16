using Assets.Scripts.Common;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Interfaces;
using Common;
using DataModel;

public class MainLinePanel : ReturnablePanel
{
    private MainLineController controller;

    public override void Init(IModule module)
    {
        base.Init(module);

        MainLineView view = InstantiateView<MainLineView>("MainLine/Prefabs/MainLineView");
        controller = new MainLineController();
        controller.View = view;
        
        RegisterController(controller);

        controller.Start();
    }

    public override void Show(float delay)
    {
//        if (GlobalData.LevelModel.FindLevel("1-6").IsPass == false)
//            HideBackBtn();
        
        Main.ChangeMenu(MainMenuDisplayState.ShowTopBar);
    }

    public void OnShow()
    {
        controller.ResetData();

//        if (GlobalData.LevelModel.FindLevel("1-6").IsPass)
            ShowBackBtn();

        Main.ChangeMenu(MainMenuDisplayState.ShowTopBar);
        
        
    }


    public void GuideIsShowBackBtnAndTopBar(bool isShow)
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
    
}