using Assets.Scripts.Common;
using Assets.Scripts.Framework.GalaSports.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace game.main
{
    public class VisitBattlePanel : ReturnablePanel
    {
        private VisitBattleController controller;
        public override void Init(IModule module)
        {
            base.Init(module);

            VisitBattleView viewScript = (VisitBattleView)InstantiateView<VisitBattleView>("VisitBattle/Prefabs/VisitBattleView");
            RegisterView(viewScript);

            controller = new VisitBattleController();
            controller.view = viewScript;
            RegisterController(controller);

            HideBackBtn();
        }

        public override void Show(float delay)
        {
            controller.Start();
            Main.ChangeMenu(MainMenuDisplayState.HideAll);
        }

        public void ShowFans(bool moreFans, bool moreGoods)
        {
            controller.view.ShowFans(moreFans, moreGoods);
        }
        public void LoadFans(bool moreFans = true)
        {
            ClientTimer.Instance.DelayCall(() => { controller.view.LoadAnimation(moreFans); }, 0.8f);
            ClientTimer.Instance.DelayCall(() => { controller.view.LoadAnimation2();  }, 1.8f);
        }

        public void Restart()
        {
            controller.Start();
            controller.view.ResetView();
        }

        public void DoNext()
        {
            controller.view.DoNext();
        }

    }
}