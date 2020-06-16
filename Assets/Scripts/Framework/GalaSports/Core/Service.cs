using System;
using Assets.Scripts.Framework.GalaSports.Interfaces;
using Framework.GalaSports.Service;

namespace Assets.Scripts.Framework.GalaSports.Core
{
    public abstract class Service<T> : IService
    {
        private IModule _module;
        
        public void SetModule(IModule module)
        {
            _module = module;
        }

        public void Execute()
        {
            OnExecute();
            DoExecute();
        }

        /// <summary>
        /// Service完成
        /// </summary>
        public Action<bool, IService> OnFinish { get; set; }

        public bool IsDispose { get; set; }

        protected T _data;

        public virtual object GetData()
        {
            return _data;
        }

        public U GetModel<U>()
        {
            return _module.GetData<U>();
        }

        /// <summary>
        /// 给模块注册model并且赋值
        /// </summary>
        /// <param name="model"></param>
        /// <typeparam name="V">IModel</typeparam>
        /// <returns></returns>
        public V SetModel<V>(V model) where V : IModel
        {
            if (_module == null)
                return default(V);
            
            return _module.RegisterModel<V>(model);
        }

        /// <summary>
        /// 配置参数，做执行前的准备
        /// </summary>
        protected abstract void OnExecute();

        /// <summary>
        /// 默认执行
        /// </summary>
        protected abstract void DoExecute();

        /// <summary>
        ///  完成时调用
        /// </summary>
        protected abstract void OnLoadData();

        public virtual void Dispose()
        {
            IsDispose = true;
            _module = null;
            _data = default(T);
        }
    }
}