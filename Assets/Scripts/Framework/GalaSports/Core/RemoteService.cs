using System;
using Assets.Scripts.Framework.GalaSports.Service;
using DataModel;
using Google.Protobuf;

namespace Assets.Scripts.Framework.GalaSports.Core
{
    public abstract class RemoteService<T> : Service<T> where T : IMessage<T>, new()
    {
        /// <summary>
        /// 网络连接失败自动重试
        /// </summary>
        protected bool autoRetry = false;
        
        protected byte[] ReqBytes;

        private string _cmd;

        public string Cmd => _cmd;

        protected bool Cache = false;
        
        protected string Version = "0";
        
        protected bool IgnoreResponse = false;
        
        protected string ServerUrl = null;
        
        public object CustomerData = null;

        protected Action<T> SuccessCallback;
        
        protected Action<HttpErrorVo> ErrorCallback;
        
        protected int httpTimeout = -1;
        
        protected byte[] requstBytes;

        public RemoteService<T> Request(byte[] bytes)
        {
            requstBytes = bytes;
            return this;
        }        
        
        /// <summary>
        /// 设置成功和失败回调
        /// </summary>
        /// <param name="successCallback"></param>
        /// <param name="errorCallback"></param>
        public RemoteService<T> SetCallback(Action<T> successCallback, Action<HttpErrorVo> errorCallback = null)
        {
            SuccessCallback = successCallback;
            ErrorCallback = errorCallback;

            return this;
        }

        protected void InitData(string cmd, byte[] reqBytes = null, bool cache = false)
        {
            _cmd = cmd;
            ReqBytes = reqBytes;
            Cache = cache;
            if(Cache)
                Version = GlobalData.VersionData.VersionDic[Cmd];
        }
        
        protected override void DoExecute()
        {
            HttpMessage<T> httpMessage = NetWorkManager.Instance.Send<T>(Cmd, ReqBytes, OnSuccess, CustomerData,
                OnError, Cache, Version, IgnoreResponse, ServerUrl);
            //默认Cache=true的时候AutoRetry也为true，这里只会设置cache为false的情况
            if(autoRetry)
                httpMessage.AutoRetry = autoRetry;
        }

        protected virtual void OnSuccess(T res)
        {
            if(IsDispose)
                return;
            
            _data = res;
            ProcessData();
            OnLoadData();
        }

        protected abstract void ProcessData();
        
        protected virtual void OnError(HttpErrorVo errorVo)
        {
            ErrorCallback?.Invoke(errorVo);
        }

        protected override void OnLoadData()
        {
            SuccessCallback?.Invoke(_data);
            OnFinish?.Invoke(true, this);
        }
    }
}