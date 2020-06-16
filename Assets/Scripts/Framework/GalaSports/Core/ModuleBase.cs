using System;
using System.Collections.Generic;
using System.Reflection;
using Assets.Scripts.Framework.GalaSports.Interfaces;
using Common;
using Framework.GalaSports.Service;
using game.main;
using UnityEngine;

namespace Assets.Scripts.Framework.GalaSports.Core
{
    public class ModuleBase : IModule
    {
        private readonly Dictionary<string, IController> _controllerDic = new Dictionary<string, IController>();
        private readonly Dictionary<string, IModel> _modelsDic = new Dictionary<string, IModel>();

        private readonly Dictionary<IService, bool> _serviceDic = new Dictionary<IService, bool>();

        private GameObject _parent;

        public float DelayUnloadAtlas = -1;

        public GameObject Parent
        {
            get { return _container; }
            set
            {
                _parent = value;
                InitContainer();
            }
        }

        public Camera ViewCamera { get; set; }

        public string ModuleName { get; set; }


        /// <summary>
        /// 模块自动卸载相关Atlas的AssetBundle
        /// </summary>
        public bool AutoUnloadAtlas = true;

        private List<Panel> _panelList = new List<Panel>();
        protected Panel _currentPanel;
        private GameObject _container;


        /// <summary>
        /// 让Service和模块产生关联
        /// </summary>
        /// <typeparam name="U"></typeparam>
        /// <returns></returns>
        public U GetService<U>() where U : IService, new()
        {
            U service = new U();
            service.SetModule(this);
            _serviceDic.Add(service, true);
            return service;
        }

        /// <summary>
        /// 外部消息接收器
        /// </summary>
        public Action<Message> MessageHandler { get; set; }

        public ModuleBase()
        {
        }

        private void InitContainer()
        {
            if (_container == null)
            {
                var type = this.GetType();
                _container = new GameObject("" + type.Name);
                RectTransform rect = _container.AddComponent<RectTransform>();
                rect.anchorMin = new Vector2(0, 0);
                rect.anchorMax = new Vector2(1, 1);
                rect.offsetMin = Vector2.zero;
                rect.offsetMax = Vector2.zero;
                rect.offsetMax = new Vector2(0, -ModuleManager.OffY);
                _container.transform.SetParent(_parent.transform, false);
            }
        }

        public virtual void LoadAssets()
        {
            AssetManager.Instance.LoadModuleAtlas(ModuleName);
        }

        public virtual void LoadAssetsAsync(Action finish)
        {
            AssetManager.Instance.LoadModuleAtlasAsync(ModuleName, finish);
        }


        public virtual void Init()
        {
        }

        public virtual void SetData(params object[] paramsObjects)
        {
        }

        public void Destroy(GameObject obj)
        {
            ModuleManager.Instance.DestroyObj(obj);
        }

        public void RegisterPanel(Panel panel)
        {
            if (_panelList == null)
            {
                _panelList = new List<Panel>();
            }

            _panelList.Add(panel);
        }

        public void UnregisterPanel(Panel panel)
        {
            if (_panelList != null)
            {
                _panelList.Remove(panel);
            }
        }

        public GameObject InstantiateView(string resUrl)
        {
            return ModuleManager.Instance.InstantiatePrefab(resUrl);
        }

        public void RegisterController(IController ctrl)
        {
            ctrl.Container = this;
            var cname = ctrl.ToString() + ctrl.GetHashCode();
            if (!_controllerDic.ContainsKey(cname))
            {
                _controllerDic.Add(cname, ctrl);
            }

            ctrl.Init();
        }

        public void UnregisterController(IController ctrl)
        {
            ctrl.Container = null;
            var cname = ctrl.ToString() + ctrl.GetHashCode();
            if (_controllerDic.ContainsKey(cname))
            {
                _controllerDic.Remove(cname);
            }

            ctrl.Destroy();
        }

        public void RegisterView(IView view)
        {
            view.Container = this;
        }

        public void UnregisterView(IView view)
        {
            view.Container = null;
        }

        public T RegisterModel<T>() where T : IModel
        {
            var type = typeof(T);
            var cname = type.ToString();

            IModel model = null;
            if (!_modelsDic.ContainsKey(cname))
            {
                Assembly assembly = Assembly.GetExecutingAssembly(); // 获取当前程序集 
                object obj = assembly.CreateInstance(type.ToString()); // 
                model = (IModel) obj;
                _modelsDic.Add(cname, model);
            }
            else
            {
                model = _modelsDic[cname];
            }

            return (T) model;
        }

        public T RegisterModel<T>(IModel model) where T : IModel
        {
            var type = typeof(T);
            var cname = type.ToString();

            if (!_modelsDic.ContainsKey(cname))
            {
                _modelsDic.Add(cname, model);
            }
            else
            {
                _modelsDic[cname] = model;
            }

            return (T) model;
        }

        public void UnregisterModel(IModel model)
        {
            var cname = model.GetType().ToString();
            if (_modelsDic.ContainsKey(cname))
            {
                _modelsDic.Remove(cname);
            }
        }

        public void UnregisterModel(string name)
        {
            if (_modelsDic.ContainsKey(name))
            {
                _modelsDic.Remove(name);
            }
        }

        public virtual void OnMessage(Message message)
        {
        }

        public void SendViewMessage(Message message)
        {
            SendMessage(message);
        }

        public void SendControllerMessage(Message message)
        {
            SendMessage(message);
        }

        public void SendMessage(Message message)
        {
            switch (message.Type)
            {
                case Message.MessageReciverType.CONTROLLER:
                    SendToControllers(message);
                    break;
                case Message.MessageReciverType.MODEL:
                    SendToModels(message);
                    break;
                case Message.MessageReciverType.DEFAULT:
                case Message.MessageReciverType.UnvarnishedTransmission:
                    OnMessage(message);
                    SendToControllers(message);
                    SendToModels(message);
                    break;
            }

            if (message.Type != Message.MessageReciverType.UnvarnishedTransmission)
                MessageHandler?.Invoke(message);
        }

        private void SendToControllers(Message message)
        {
            var buffer = new List<string>(_controllerDic.Keys);
            if (_controllerDic.Count > 0)
            {
                foreach (string keys in buffer)
                {
                    IController value;
                    _controllerDic.TryGetValue(keys, out value);
                    if (value != null)
                    {
                        value.OnMessage(message);
                    }
                }

                /*foreach (var item in _controllerDic)
                {
                item.Value.OnMessage(message);
                }*/
            }
        }

        private void SendToModels(Message message)
        {
            foreach (var model in _modelsDic)
            {
                if (model.Value != null)
                    model.Value.OnMessage(message);
            }
        }


        public T GetData<T>()
        {
            var cname = typeof(T).ToString();
            if (_modelsDic.ContainsKey(cname))
            {
                return (T) _modelsDic[cname];
            }

            return default(T);
        }

        public virtual void OnHide()
        {
            if (_container != null)
            {
                _container.Hide();
            }
        }

        public virtual void OnShow(float delay)
        {
            if (_container != null)
            {
                if (_container.transform != null)
                {
                    _container.transform.SetAsLastSibling();
                }

                _container.Show();
            }
        }

        public virtual void Remove(float delay)
        {
            GuideManager.UnregisterModule(this);

            foreach (var service in _serviceDic)
            {
                service.Key.Dispose();
            }

            _serviceDic.Clear();

            var temp = new List<IController>();

            foreach (var item in _controllerDic)
            {
                temp.Add(item.Value);
            }

            for (int i = temp.Count - 1; i >= 0; i--)
            {
                temp[i].Destroy();
            }

            foreach (var item in _modelsDic)
            {
                item.Value.Destroy();
            }

            if (_panelList != null)
            {
                for (int i = _panelList.Count - 1; i >= 0; i--)
                {
                    _panelList[i].Destroy();
                }

                _panelList.Clear();
            }

            if (_container != null)
                Destroy(_container);

            _controllerDic.Clear();
            _modelsDic.Clear();

            UnloadAssets();

            Resources.UnloadUnusedAssets();
        }


        public virtual void UnloadAssets()
        {
            if (DelayUnloadAtlas > 0)
            {
                ClientTimer.Instance.DelayCall(
                    () =>
                    {
                        AssetManager.Instance.UnloadModuleBundles(ModuleName, AutoUnloadAtlas);
                        AssetManager.Instance.UnloadSingleFileBundle(ModuleName);
                    }, DelayUnloadAtlas);
            }
            else
            {
                AssetManager.Instance.UnloadModuleBundles(ModuleName, AutoUnloadAtlas);
                AssetManager.Instance.UnloadSingleFileBundle(ModuleName);
            }
        }
    }
}