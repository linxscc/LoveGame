using Assets.Scripts.Common;
using Assets.Scripts.Framework.GalaSports.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace game.main
{
    public class VisitSupporterPanel : ReturnablePanel
    {

        public override void Init(IModule module)
        {
            base.Init(module);

            IView viewScript = InstantiateView<VisitSupporterView>("VisitBattle/Prefabs/Panels/Supporter");

            var control = new VisitSupporterController();
            control.view = (VisitSupporterView)viewScript;

            RegisterController(control);
            RegisterView(viewScript);

            control.Start();

        }
    }
}
