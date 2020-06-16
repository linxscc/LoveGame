using System;
using System.Collections.Generic;
using Assets.Scripts.Componets;
using Assets.Scripts.Framework.GalaSports.Interfaces;
using game.main;
using UnityEngine;

namespace Assets.Scripts.Framework.GalaSports.Core
{
    public class Panel
    {
        private IModule _module;
        public IModule Module
        {
            get
            {
                return _module;
            }
        }

        private readonly List<IController> _controllers=new List<IController>();
        private readonly List<IView> _views=new List<IView>();
        
        private GameObject _container;
        protected GameObject _showObject;

        private bool _isSimplePanel = true;
        private List<string> _modelList = new List<string>();

        /// <summary>
        /// 设成简单panel，没有生成容器
        /// </summary>
        public void SetComplexPanel()
        {
            if (_container != null)
            {
                throw new Exception("Please Set It before Init");
            }
            _isSimplePanel = false;
        }
        
        public virtual void SendMessage(Message message)
        {
            if (_module == null)
            {
                throw new Exception(this.GetType()+" didn't registered");
            }
            _module.SendMessage(message);
        }

        public virtual void Init(IModule module)
        {
            _module = module;
            _module.RegisterPanel(this);
          
            if (!_isSimplePanel)//复杂panel 创建容器
            {
                var type = this.GetType();
                _container = new GameObject("" + type.Name);
                _container.transform.SetParent(_module.Parent.transform, false);
                
                RectTransform rect = _container.AddComponent<RectTransform>();
                rect.anchorMin = new Vector2(0, 0);
                rect.anchorMax = new Vector2(1, 1);
                rect.offsetMin = Vector2.zero;
                rect.offsetMax = Vector2.zero;
              
                _showObject = _container;
            }
        }

        public T InstantiateWindow<T>(string resUrl) where T: Window
        {
            T win = PopupManager.ShowWindow<T>(resUrl, _module);
            RegisterView(win);

            _showObject = win.gameObject;
            return win;
        }
        /// <summary>
        /// 初始化View
        /// </summary>
        /// <typeparam name="T">View object的控制类</typeparam>
        /// <param name="resUrl">prefab path</param>
        /// <param name="mode">RenderMode</param>
        /// <param name="siblingIndex">显示层级</param>
        /// <returns></returns>
        public T InstantiateView<T>(string resUrl, int siblingIndex=-1) where T: IView
        {
            var gameObj= ModuleManager.Instance.InstantiatePrefab(resUrl);
            if (_container == null)
            {

                gameObj.transform.SetParent(_module.Parent.transform, false);
                if (_showObject != null)
                {
                    var type = this.GetType();
                    
                    _container = new GameObject("" + type.Name);
                    _container.transform.SetParent(_module.Parent.transform, false);
                    _showObject.transform.SetParent(_container.transform, false);
                    gameObj.transform.SetParent(_container.transform, false);
                    _showObject = _container;
                }
                else
                {
                    _showObject = gameObj;
                }
                RectTransform rect = gameObj.GetComponent<RectTransform>();
                rect.anchorMin = new Vector2(0, 0);
                rect.anchorMax = new Vector2(1, 1);
                rect.offsetMin = Vector2.zero;
                rect.offsetMax = Vector2.zero;
            }
            else
            {
                gameObj.transform.SetParent(_container.transform, false);
            }
            
            gameObj.transform.localScale = new Vector3(1, 1, 1);
            if (siblingIndex != -1)
            {
                gameObj.transform.SetSiblingIndex(siblingIndex);
            }
           
            var tType = typeof(T);
            IView vscript = (IView)gameObj.AddComponent(tType);
            RegisterView(vscript);
            return (T)vscript;
        }

        /// <summary>
        /// 注册controller
        /// </summary>
        /// <param name="controller"></param>
        public void RegisterController(IController controller)
        {
            controller.Panel = this;
            Module.RegisterController(controller);
            _controllers.Add(controller);
        }

        public void UnregisterController(IController controller)
        {
            Module.UnregisterController(controller);
            _controllers.Remove(controller);
        }

        public void RegisterView(IView view)
        {
            Module.RegisterView(view);
            _views.Add(view);
        }

        public void UnregisterView(IView view)
        {
            Module.UnregisterView(view);
            _views.Remove(view);
        }
        public void RegisterModel<T>() where T : IModel
        {
            Module.RegisterModel<T>();
            _modelList.Add(typeof(T).ToString());
        }
        
        public void UnregisterModel()
        {
            for (int i = 0; i < _modelList.Count; i++)
            {
                Module.UnregisterModel(_modelList[i]);
            }
        }

        public void SetSiblingIndex(int index)
        {
            if (_showObject != null)
            {
                _showObject.transform.SetSiblingIndex(index);
            }
            else
            {
                throw new Exception("No View Objects");
            }
           
        }
        /// <summary>
        /// 获取已注册数据
        /// </summary>
        /// <typeparam name="T">model type</typeparam>
        /// <returns></returns>
        public T GetData<T>() where T:IModel
        {
            return _module.GetData<T>();
        }
        

        protected void SetToTop()
        {
            if (_showObject != null)
            {
                _showObject.transform.SetAsLastSibling();
            }
            else
            {
              //  throw new Exception("No View Objects");
            }
        }

        public virtual void Show(float delay)
        {
            if (_container != null)
            {
                _container.Show();
            }
            SetToTop();
        }

        public virtual void Hide()
        {
            if (_container != null)
            {
                _container.Hide();
            }
        }

      
        protected void Unregister()
        {
            foreach(var v in _controllers)
            {
                Module.UnregisterController(v);
            }
            _controllers.Clear();
            _views.Clear();
            UnregisterModel();       
        }

        public virtual void Destroy()
        {
            Module.UnregisterPanel(this);
            
            Unregister();
            
            if(_showObject == null)
                return;
            
            Window win = _showObject.GetComponent<Window>();
            if (win != null)
            {
                win.Close();
            }
            else
            {
                Module.Destroy(_showObject);
            }
        }
    }
}
