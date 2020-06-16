using Assets.Scripts.Common;
using Assets.Scripts.Framework.GalaSports.Interfaces;

namespace game.main
{
    public class SupporterPanel : ReturnablePanel
    {
        public override void Init(IModule module)
        {
            base.Init(module);

            IView viewScript = InstantiateView<SupporterView>("Battle/Prefabs/Panels/Supporter");

            var control = new SupporterController();
            control.view = (SupporterView)viewScript;

            RegisterController(control);
            RegisterView(viewScript);
            
            control.Start();

        }
    }
}
