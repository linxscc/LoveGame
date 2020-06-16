using Assets.Scripts.Framework.GalaSports.Interfaces;
using Assets.Scripts.Framework.GalaSports.Service;
using game.main;
using UnityEngine;

namespace Assets.Scripts.Framework.GalaSports.Core
{
    public class View : MonoBehaviour, IView
    {
        /// <summary>
        /// 来自所属模块的名字，不能在Awake获得，要在Start使用
        /// </summary>
        public string ModuleName => _container.ModuleName;
        
        protected IModule _container;

        public void ConnectModule(IView view)
        {
            view.Container = _container;
        }

        protected T GetWindow<T>(string resUrl) where T : Window
        {
            GameObject window = InstantiatePrefab(resUrl);
            var win = window.AddScriptComponent<T>();
            win.Container = _container;
            win.AssetName = resUrl;

            return win;
        }
        
        protected T ShowWindow<T>(string resUrl) where T : Window
        {
            T win = GetWindow<T>(resUrl);
            PopupManager.Popup(win.gameObject);

            return win;
        }
        
        protected GameObject InstantiatePrefab(string resUrl, Transform parent = null)
        {
            GameObject go = Instantiate(ResourceManager.Load<GameObject>("module/" + resUrl), parent, false);
            IView view = go.GetComponent<IView>();
            if (view != null)
            {
                view.Container = _container;
            }
            return go;
        }

        protected GameObject GetPrefab(string resUrl)
        {
            GameObject go = ResourceManager.Load<GameObject>("module/" + resUrl);
            IView view = go.GetComponent<IView>();
            if (view != null)
            {
                view.Container = _container;
            }
            return go;
        }
        
        protected GameObject GetPrefab<T>(string resUrl) where T : Component
        {
            GameObject go = ResourceManager.Load<GameObject>("module/" + resUrl);
            IView view = go.AddComponent<T>() as IView;
            if (view != null)
            {
                view.Container = _container;
            }
            return go;
        }
        

        public virtual IModule Container
        {
            set
            {
                _container = value;
            }
        }
        public virtual void SendMessage(Message message)
        {
            _container.SendViewMessage(message);
        }

        public virtual void Show(float delay = 0)
        {
            gameObject.Show();
        }

        public virtual void Hide()
        {
            gameObject.Hide();
        }
    }
}
