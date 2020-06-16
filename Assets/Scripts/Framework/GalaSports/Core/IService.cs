using System;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Interfaces;

namespace Framework.GalaSports.Service
{
    public interface IService
    {
        void SetModule(IModule module);
        
        void Execute();
        
        /// <summary>
        /// 是否成功，Service实例
        /// </summary>
        Action<bool,IService> OnFinish { get; set; }

        bool IsDispose { get; set; }

        object GetData();
        
        T GetModel<T>();

        void Dispose();
    }
}