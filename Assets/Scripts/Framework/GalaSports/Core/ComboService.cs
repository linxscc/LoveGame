using System;
using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Service;
using DataModel;
using Google.Protobuf;

namespace Framework.GalaSports.Core
{
    /// <summary>
    /// 2个Service组合使用，异步发送
    /// </summary>
    /// <typeparam name="T">最终返回值</typeparam>
    /// <typeparam name="U">第一个接口的返回值</typeparam>
    /// <typeparam name="V">第二个接口的返回值</typeparam>
    public abstract class ComboService<T, U, V> : Service<T> where U : IMessage<U>, new() where V : IMessage<V>, new()
    {
        protected List<string> cmdList;
        protected List<byte[]> reqByteList;
        protected List<bool> cacheList;

        protected Action<T> SuccessCallback;
        protected Action<List<HttpErrorVo>> ErrorCallback;

        private V _resV;
        private U _resU;
        private List<HttpErrorVo> _httpErrorList;

        private int _callbackCount;
        
        protected int httpTimeoutU = -1;
        protected int httpTimeoutV = -1;

        protected ComboService()
        {
            _callbackCount = 0;
            cmdList = new List<string>();
            reqByteList = new List<byte[]>();
            cacheList = new List<bool>();
            _httpErrorList = new List<HttpErrorVo>();
        }

        /// <summary>
        /// 设置成功和失败回调
        /// </summary>
        /// <param name="successCallback"></param>
        /// <param name="errorCallback"></param>
        public ComboService<T, U, V> SetCallback(Action<T> successCallback,
            Action<List<HttpErrorVo>> errorCallback = null)
        {
            SuccessCallback = successCallback;
            ErrorCallback = errorCallback;

            return this;
        }

        protected void AddServiceData(string cmd, byte[] reqBytes = null, bool cache = false)
        {
            if (cmdList.Count > 2)
            {
                throw new Exception("只支持2个接口");
            }

            cmdList.Add(cmd);
            reqByteList.Add(reqBytes);
            cacheList.Add(cache);
        }

        protected abstract void ProcessData(U resU, V resV);

        protected override void DoExecute()
        {
            for (int i = 0; i < cmdList.Count; i++)
            {
                string version = "";
                if (cacheList[i])
                {
                    version = GlobalData.VersionData.VersionDic[cmdList[i]];
                }

                if (i == 0)
                {
                    HttpMessage<U> httpMessage = NetWorkManager.Instance.Send<U>(cmdList[i], reqByteList[i], OnSuccessU,
                        OnErrorU, cacheList[i], version, false, null, httpTimeoutU);
                }
                else
                {
                    HttpMessage<V> httpMessage =  NetWorkManager.Instance.Send<V>(cmdList[i], reqByteList[i], OnSuccessV, OnErrorV, cacheList[i],
                        version, false, null, httpTimeoutU);
                }
            }
        }

        private void OnSuccessV(V res)
        {
            _callbackCount++;
            _resV = res;
            OnComplete();
        }
        private void OnSuccessU(U res)
        {
            _callbackCount++;
            _resU = res;
            OnComplete();
        }
        private void OnComplete()
        {
            if (_resU != null && _resV != null && IsDispose == false)
            {
                ProcessData(_resU, _resV);
                OnLoadData();
            }
        }


        private void OnErrorV(HttpErrorVo errorVo)
        {
            _callbackCount++;
            _httpErrorList.Add(errorVo);
            OnError();
        }

        private void OnErrorU(HttpErrorVo errorVo)
        {
            _callbackCount++;
            _httpErrorList.Add(errorVo);
            OnError();
        }

        private void OnError()
        {
            if (_callbackCount >= cmdList.Count)
            {
                ErrorCallback?.Invoke(_httpErrorList);
            }
        }

        protected override void OnLoadData()
        {
            SuccessCallback?.Invoke(_data);
            OnFinish?.Invoke(true, this);
        }

        public override void Dispose()
        {
            base.Dispose();

            SuccessCallback = null;
            ErrorCallback = null;
        }
    }
}