using System.Collections.Generic;
using Assets.Scripts.Common;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Interfaces;
using Module.Battle.Data;
using UnityEngine;

namespace game.main
{
    public class BattlePanel : ReturnablePanel
    {
        private BattleController controller;

        public override void Init(IModule module)
        {
            base.Init(module);

            BattleView viewScript = (BattleView) InstantiateView<BattleView>("Battle/Prefabs/BattleView");
            RegisterView(viewScript);
            
            controller = new BattleController();
            controller.view = viewScript;
            RegisterController(controller);
            
            HideBackBtn();
        }

        public override void Show(float delay)
        {
            controller.Start();
        }



        public void Restart()
        {
            controller.Start();
            controller.view.ResetView();
        }



        public void GetFansInfo(Queue<string> fansInfo)
        {
            controller.view.GetFansInfo(fansInfo);
        }


        public void GetRolesId(Queue<int> roleIds)
        {
            controller.view.GetRolesId(roleIds);
        }
        
        public void GetPower(int power)
        {
            controller.view.GetPower(power); 
        }
    }
}