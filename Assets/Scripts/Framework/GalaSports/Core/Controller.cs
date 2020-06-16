using System;
using Assets.Scripts.Framework.GalaSports.Interfaces;
using Framework.GalaSports.Service;

namespace Assets.Scripts.Framework.GalaSports.Core
{
    public class Controller : IController
    {
        private IModule _container;
        public virtual IModule Container
        {
            set
            {
                _container = value;
            }

            get
            {
                return _container;
            }
        }
        
        private Panel _panel;
        public Panel Panel
        {
            set
            {
                _panel = value;
            }

            get
            {
                return _panel;
            }
        }

        protected Controller()
        {
            //_container = module;
        }
        public virtual void SendMessage(Message message)
        {
            if (_container == null)
            {
                throw new Exception(this.GetType()+" didn't registered");
            }
            _container.SendControllerMessage(message);
        }
        public virtual void OnMessage(Message message)
        {

        }

        /// <summary>
        /// 注册后自动执行
        /// </summary>
        public virtual void Init()
        {
            
        }

        /// <summary>
        /// 需手动启动
        /// </summary>
        public virtual void Start()
        {
            
        }

        public virtual void Destroy()
        {
//            if(_container != null)
//                _container.UnregisterController(this);
            
            _container = null;
            _panel = null;
        }

        public T GetService<T>() where T : IService, new()
        {
            return _container.GetService<T>();
        }

        /// <summary>
        /// 获取已注册数据
        /// </summary>
        /// <typeparam name="T">model type</typeparam>
        /// <returns></returns>
        public T GetData<T>() where T:IModel
        {
            if(_container==null) throw new Exception(this.GetType()+" didn't registered");
            return _container.GetData<T>();
        }
    }
}

