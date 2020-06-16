using Assets.Scripts.Common;
using Assets.Scripts.Framework.GalaSports.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LovePanel : ReturnablePanel
{
    LoveController _loveController;
    public override void Init(IModule module)
    {
        base.Init(module);
        var viewScript = InstantiateView<LoveView>("Love/Prefabs/LoveView");
        _loveController = new LoveController();
        RegisterController(_loveController);
        _loveController.CurLoveView = (LoveView)viewScript;
    
        _loveController.Start();
    }
    public override void Show(float delay)
    {
        base.Show(delay);
        Main.ChangeMenu(MainMenuDisplayState.HideAll);
        ShowBackBtn();
        _loveController.CurLoveView.UpdateView();
    }
    public override void Hide()
    {
        base.Hide();
    }
}
