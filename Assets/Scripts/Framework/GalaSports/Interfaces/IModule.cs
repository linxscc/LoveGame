using Assets.Scripts.Framework.GalaSports.Core;
using System;
using Framework.GalaSports.Service;
using UnityEngine;

namespace Assets.Scripts.Framework.GalaSports.Interfaces
{
    public interface IModule
    {
        GameObject Parent { get; set; }
        Camera ViewCamera { get; set; }
        string ModuleName { get; set; }
        void LoadAssets();
        void LoadAssetsAsync(Action finish);
        void UnloadAssets();
        void Init();
        void SendMessage(Message message);
        void SendViewMessage(Message message);
        void SendControllerMessage(Message message);
        void OnMessage(Message message);
        void RegisterPanel(Panel panel);
        void UnregisterPanel(Panel panel);
        void RegisterView(IView view);
        T RegisterModel<T>() where T : IModel;
        T RegisterModel<T>(IModel model) where T : IModel;
        void UnregisterModel(string name);
        void UnregisterView(IView view);
        void RegisterController(IController ctrl);
        void UnregisterController(IController ctrl);
        GameObject InstantiateView(string resUrl);
        void Destroy(GameObject obj);
        void OnHide();
        void SetData(params object[] paramObjects);
        void OnShow(float delay);
        void Remove(float delay);
        T GetData<T>();
        U GetService<U>() where U : IService, new();
        
        Action<Message> MessageHandler { get; set; }
    }
}
