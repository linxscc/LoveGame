using Assets.Scripts.Framework.GalaSports.Core;
using Framework.GalaSports.Service;

namespace Assets.Scripts.Framework.GalaSports.Interfaces
{
    public interface IController
    {
        IModule Container { set; get; }
        Panel Panel { set; get; }
        void OnMessage(Message message);
        void Start();
        void Init();
        void Destroy();

        T GetService<T>() where T : IService, new();
    }
}
