using Assets.Scripts.Common;
using Assets.Scripts.Framework.GalaSports.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Framework.GalaSports.Core;

public class VisitLevelPanel : ReturnablePanel
{
    private VisitLevelController _visitLevelController;
    string path = "Visit/Prefabs/VisitLevelView";
    public override void Init(IModule module)
    {
        base.Init(module);
        var viewScript = InstantiateView<VisitLevelView>(path);
        _visitLevelController = new VisitLevelController();
        _visitLevelController.VisitLevelView = (VisitLevelView)viewScript;

        RegisterView(viewScript);
        RegisterController(_visitLevelController);
        _visitLevelController.Start();

    }

    public void Refresh()
    {
        _visitLevelController.Refresh();
    }
    public override void Show(float delay)
    {
        base.Show (delay);
       // Main.ChangeMenu(MainMenuDisplayState.ShowVisitTopBar);
        Main.ChangeMenu(MainMenuDisplayState.ShowVisitTopBar);
        ShowBackBtn();
    }
    public override void Hide()
    {
        base.Hide();
    }

    public void SetData(PlayerPB npcId)
    {

        _visitLevelController.SetData(npcId);
    }
    public override void OnBackClick()
    {
        SendMessage(new Message(MessageConst.MODULE_VISIT_SHOW_VISIT_PANEL, Message.MessageReciverType.DEFAULT));
    }

}
