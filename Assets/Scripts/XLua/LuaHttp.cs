using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Service;
using Common;
using Framework.GalaSports.Service;
using Framework.Utils;
using game.main;
using UnityEngine;
using UnityEngine.Networking;

namespace XLua
{
    [LuaCallCSharp]
    public class LuaHttp
    {
        private int MaxRetryTimes = 6;
        private string _cmd;
        private byte[] _data;
        private bool _cache;
        private string _version;

        /// <summary>
        /// Http超时
        /// </summary>
        public int HttpTimeOut = -1;

        public object CustomerData;

        private int _retryTimes = 0;
        private string _serverUrl;

        public bool AutoRetry = false;

        private float retryInterval = 3;
       
        private DelegateBytes _successCallback;
        private string _url;

        public static void OnGlobalError(HttpErrorVo vo)
        {
            NetWorkManager.Instance.GlobalNetErrorHandler?.Invoke(vo);
        }

        public LuaHttp(string cmd, 
            byte[] data, 
            DelegateBytes successCallback, 
            object customerData,
            bool cache = false, 
            string version = "", 
            string serverUrl = null, 
            int httpTimeout = -1)
        {
            _successCallback = successCallback;
            _cmd = cmd;
            _data = data;
            _cache = cache;
            _version = version;
            CustomerData = customerData;
            _retryTimes = 0;
            _serverUrl = serverUrl;
            _cache = cache;

            AutoRetry = cache;

            HttpTimeOut = httpTimeout;
        }

        public void SendRequest()
        {
            MonoObject monoGameObjectScript = NetWorkManager.Instance.MonoGameObjectScript;
            if (_serverUrl != null)
            {
                _url = _serverUrl;
            }
            else
            {
                _url = NetWorkManager.Serverurl;
            }
            monoGameObjectScript.Coroutine(Request());
        }

        private IEnumerator Request()
        {
            bool hasTemp = false;
            var pathName = _cmd.Split('/');
            var path1 = Util.ToUnderlineName(pathName[0]);
            var path2 = Util.ToUnderlineName(pathName[1]);

            //缓存数据
            if (_cache)
            {
                byte[] bytes = null;
                if (_version == AppConfig.Instance.cacheVersion)
                {
                    bytes = new AssetLoader().LoadBytes(AssetLoader.GetProtoCachePath(path1 + "/" + path2 + ".info"));
                    Debug.Log("<color='#0099ff'>Load from ProtoCache:" + path1 + "/" + path2 + ".info</color>");
                }
                else
                {
                    bytes = FileUtil.GetBytesFile(AssetLoader.CachePath + path1, path2 + _version + ".info");
                }

                if (bytes != null)
                {
                    _successCallback(bytes);
                    hasTemp = true;
                    yield break;
                }
            }

            string uri = _url + _cmd;
            Debug.Log("connect==>" + uri);

            UnityWebRequest www = new UnityWebRequest(uri, UnityWebRequest.kHttpVerbPOST)
            {
                uploadHandler = new UploadHandlerRaw(_data), 
                downloadHandler = new DownloadHandlerBuffer(),
                useHttpContinue = false,
                timeout = HttpTimeOut > 0 ? HttpTimeOut : NetWorkManager.HttpTimeOut
            };

            foreach (var header in NetWorkManager.Headers)
            {
                www.SetRequestHeader(header.Key, header.Value);
            }

            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.LogError("<color=#00CAFFFF>" + "www_error:" + www.error + " Uri:" + uri + "</color> ");

                if (_retryTimes < MaxRetryTimes && AutoRetry)
                {
                    _retryTimes++;
                    Debug.LogError(www.error + "==RetryTimes:" + _retryTimes);
                    yield return new WaitForSeconds(retryInterval);
                    SendRequest();
                    yield break;
                }

                Debug.LogError("网络异常，重试" + _retryTimes + "次  " + uri);

                HttpErrorVo vo = new HttpErrorVo
                {
                    ErrorCode = 0,
                    Cmd = _cmd,
                    CustomData = CustomerData,
                    ErrorString = www.error
                };
                
                OnGlobalError(vo);
            }
            else
            {
                var bytes = www.downloadHandler.data;
                if (_cache)
                {
                    FileUtil.SaveBytesFile(AssetLoader.CachePath + path1, path2 + _version + ".info", bytes);
                }

                _successCallback(bytes);

//                if (cache)
//                {
//                    FileUtil.SaveBytesFile(AssetLoader.CachePath + path1, path2 + version + ".info", bytes);
//                }
            }
            www.Dispose();
        }
    }
}