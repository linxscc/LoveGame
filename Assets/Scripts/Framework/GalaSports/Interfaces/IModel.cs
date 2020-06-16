using Assets.Scripts.Framework.GalaSports.Core;

namespace Assets.Scripts.Framework.GalaSports.Interfaces
{
    public interface IModel
    {
        void OnMessage(Message message);
        void Destroy();
    }
}
