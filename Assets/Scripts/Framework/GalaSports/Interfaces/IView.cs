using Assets.Scripts.Framework.GalaSports.Core;
using Framework.GalaSports.Service;

namespace Assets.Scripts.Framework.GalaSports.Interfaces
{
    public interface IView
    {
        IModule Container { set; }
        void SendMessage(Message message);
        void Show(float delay = 0);
        void Hide();

        void ConnectModule(IView view);
    }
}
