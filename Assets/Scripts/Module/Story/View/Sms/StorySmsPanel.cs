using Assets.Scripts.Common;
using Assets.Scripts.Framework.GalaSports.Interfaces;

namespace game.main
{
    public class StorySmsPanel : ReturnablePanel
    {
        private StorySmsController _control;

        public override void Init(IModule module)
        {
            base.Init(module);

            var viewScript = InstantiateView<StorySmsView>("Story/Prefabs/SmsView");
        
            _control = new StorySmsController();
            _control.View = (StorySmsView) viewScript;
        
            RegisterController(_control);
        }

        public override void Show(float delay)
        {
            base.Show(delay);
            _control.View.gameObject.Show();
        }
        
        public void Show(bool continueAutoPlay)
        {
            Show(0);
            _control.View.SetAutoPlay(continueAutoPlay);
        }

        public override void Hide()
        {
            base.Hide();
            _control.View.gameObject.Hide();
        }

        public void LoadJson(string id, bool switchover)
        {
            _control.LoadJson(id, switchover);
        }
    }
}