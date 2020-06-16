using System;
using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Service;
using Google.Protobuf;
using UnityEngine;

namespace Assets.Scripts.Service
{
    public class LoaderMax
    {
        private string Name;
        public int Max;
        private List<IHttpMessage> _messageList = new List<IHttpMessage>();
        private Action<int,int> _onProgressHandler;
        private Action _onLoadCompleteHandler;
        private Action<HttpErrorVo> _onErrorHandler;
        public LoaderMax(string name, Action<int,int> onProgressHanlder, Action onCompleteHandler,
            Action<HttpErrorVo> onErrorHandler)
        {
            Name = name;
            _onProgressHandler = onProgressHanlder;
            _onLoadCompleteHandler = onCompleteHandler;
            _onErrorHandler = onErrorHandler;
        }

        public void Append<T>(string cmd, byte[] data, Action<T> onCompleteHandler, bool cache = false, string version = "") where T : IMessage<T>, new()
        {
            HttpMessage<T> msg = new HttpMessage<T>(cmd, data, delegate (T res)
            {
                onCompleteHandler(res);
                OnComplete(cmd);
            }, "", OnError, cache, version);
            _messageList.Add(msg);
        }

        public void Load()
        {
            Max = _messageList.Count;
            LoadNext();
        }

        private void LoadNext()
        {
            if (_messageList.Count > 0)
            {
                IHttpMessage msg = _messageList[0];
                _messageList.RemoveAt(0);
                NetWorkManager.Instance.Send(msg);
            }
        }

        private void OnComplete(string cmd)
        {
            _onProgressHandler(Max - _messageList.Count,Max);
            Debug.Log("OnComplete  ===========" + (Max - _messageList.Count));
            if (_messageList.Count == 0)
            {
                _onLoadCompleteHandler();
            }
            else
            {
                LoadNext();
            }
        }

        private void OnError(HttpErrorVo vo)
        {
            _onErrorHandler(vo);
        }
    }
}
