using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using Common;
using Framework.Utils;
using game.main;
using Google.Protobuf;
using UnityEngine;
using UnityEngine.Networking;
using Debug = UnityEngine.Debug;

namespace Assets.Scripts.Framework.GalaSports.Service
{
    public class HttpMessage<T> : IHttpMessage where T : IMessage<T>, new()
    {
        private int MaxRetryTimes = 6;
        private Action<T> _succCallback;
        private Action<HttpErrorVo> _errCallback;
        private string _cmd;
        private byte[] _data;
        private bool _cache;
        private string _version;
        private bool _ignoreResponse;

        /// <summary>
        /// Http超时
        /// </summary>
        private int HttpTimeOut = -1;

        public object CustomerData;

        private int _retryTimes = 0;
        private string _serverUrl;

        public bool AutoRetry = false;

        public float retryInterval = 2;

        public string GetCmd()
        {
            return _cmd;
        }

        public HttpMessage(string cmd, byte[] data, Action<T> succCallback, object customerData,
            Action<HttpErrorVo> errCallback = null, bool cache = false, string version = "",
            bool ignoreResponse = false,
            string serverUrl = null, int httpTimeout = -1)
        {
            _succCallback = succCallback;
            _errCallback = errCallback;
            _cmd = cmd;
            _data = data;
            _cache = cache;
            _version = version;
            _ignoreResponse = ignoreResponse;
            CustomerData = customerData;
            _retryTimes = 0;
            _serverUrl = serverUrl;

            HttpTimeOut = httpTimeout;

            //缓存的rule数据自动重试
            AutoRetry = cache;
        }

        public void RequestHandler()
        {
            MonoObject monoGameObjectScript = NetWorkManager.Instance.MonoGameObjectScript;
            string url;
            if (_serverUrl != null)
            {
                url = _serverUrl;
            }
            else
            {
                url = NetWorkManager.Serverurl;
            }

            monoGameObjectScript.Coroutine(Request(url, _cmd, _data, _succCallback, _errCallback,
                _cache, _version, _ignoreResponse));
        }

        private IEnumerator Request(string serverUrl, string cmd, byte[] data, Action<T> succCallback,
            Action<HttpErrorVo> errCallback, bool cache, string version, bool ignoreResponse)
        {
            bool hasTemp = false;
            var pathName = cmd.Split('/');
            var path1 = Util.ToUnderlineName(pathName[0]);
            var path2 = Util.ToUnderlineName(pathName[1]);

            //缓存数据
            if (cache)
            {
                byte[] dataFile = null;
                if (version == AppConfig.Instance.cacheVersion)
                {
                    dataFile = new AssetLoader().LoadBytes(
                        AssetLoader.GetProtoCachePath(path1 + "/" + path2 + ".info"));
                    Debug.Log("<color='#9900ff'>Load from ProtoCache:" + path1 + "/" + path2 + ".info</color>");
                }
                else
                {
                    dataFile = FileUtil.GetBytesFile(AssetLoader.CachePath + path1, path2 + version + ".info");
                }

                if (dataFile != null)
                {
                    Stopwatch sw = Stopwatch.StartNew();
                    Loom.RunAsync(() =>
                    {
                        MemoryStream m = new MemoryStream(dataFile);
                        MessageParser<T> parser = new MessageParser<T>(() => new T());
                        var res = parser.ParseFrom(m);

                        var field = res.Descriptor.FindFieldByName("ret");
                        int ret = (int) field.Accessor.GetValue(res);
                        Debug.Log("线程处理缓存数据时间1：" + sw.ElapsedMilliseconds);
                        Loom.QueueOnMainThread(() =>
                        {
                            sw.Stop();
                            Debug.Log("线程处理缓存数据时间2：" + sw.ElapsedMilliseconds);
                            if (ret == 0 || ret == -1)
                            {
                                succCallback(res);
                            }
                        });
                    });

                    hasTemp = true;
                }
            }

            if (!hasTemp)
            {
                var uri = serverUrl + cmd;
                Debug.Log("connect==>" + uri);

                UnityWebRequest www = new UnityWebRequest(uri, UnityWebRequest.kHttpVerbPOST);

                www.uploadHandler = new UploadHandlerRaw(data);
                www.downloadHandler = new DownloadHandlerBuffer();


                StringBuilder sb = new StringBuilder();
                foreach (var header in NetWorkManager.Headers)
                {
                    sb.AppendLine("Key:" + header.Key + " Value:" + header.Value);
                    www.SetRequestHeader(header.Key, header.Value);
                }

                if (AppConfig.Instance.isTestMode)
                {
                    Debug.LogWarning("HttpMsg=> SetRequestHeader" + sb.ToString());
                }

                www.useHttpContinue = false;
                if (HttpTimeOut > 0)
                {
                    www.timeout = HttpTimeOut;
                }
                else
                {
                    www.timeout = NetWorkManager.HttpTimeOut;
                }

                if (www.timeout > 15)
                    Debug.LogWarning("Http Time===>" + www.timeout + " Url=>" + uri);

                string str = www.GetRequestHeader("Cookie");

                yield return www.SendWebRequest();
                if (www.isNetworkError || www.isHttpError)
                {
                    Debug.LogError("<color=#00CAFFFF>" + "www_error:" + www.error + " Uri:" + uri + "</color> ");

                    if (_retryTimes < MaxRetryTimes && AutoRetry)
                    {
                        _retryTimes++;
                        Debug.LogError(www.error + "==RetryTimes:" + _retryTimes);
                        yield return new WaitForSeconds(retryInterval);
                        RequestHandler();
                        yield break;
                    }
                    else
                    {
                        Debug.LogError("网络异常，重试" + _retryTimes + "次  " + uri);
                    }

                    HttpErrorVo vo = new HttpErrorVo
                    {
                        ErrorCode = 0,
                        Cmd = _cmd,
                        CustomData = CustomerData,
                        ErrorString = www.error
                    };
                    errCallback?.Invoke(vo);

                    www.Dispose();

                    yield break;
                }
                else
                {
                    var bytes = www.downloadHandler.data;

                    MemoryStream m = new MemoryStream(bytes);
                    MessageParser<T> parser = new MessageParser<T>(() => new T());
                    var res = parser.ParseFrom(m);
                    var field = res.Descriptor.FindFieldByName("ret");
                    int ret = (int) field.Accessor.GetValue(res);

                    if (ret == 0 || ret == -1 || ret == ErrorCode.USER_NOT_CARD_CODE ||
                        ret ==ErrorCode.SERVER_TOURIST_NOT_RECHARGE ||
                        ret ==ErrorCode.SERVER_RECHARGE_UPPERLIMIT||
                        ret== ErrorCode.SERVER_NOT_OPPEN_RECHARGE)
                    {
                        if (NetWorkManager.CookieStr == null)
                        {
                            Dictionary<string, string> responseHeaders = null;
                            try
                            {
                                responseHeaders = www.GetResponseHeaders();
                            }
                            catch (Exception e)
                            {
                                Debug.LogError(e.Message);
                            }

                            if (responseHeaders != null && responseHeaders.ContainsKey("SET-COOKIE"))
                            {
                                var cookie = responseHeaders["SET-COOKIE"];
                                if (NetWorkManager.CookieStr != null && cookie != NetWorkManager.CookieStr ||
                                    NetWorkManager.CookieStr == null)
                                {
                                    NetWorkManager.CookieStr = responseHeaders["SET-COOKIE"];
                                    NetWorkManager.InitHead();
                                }
                            }
                        }

                        if (cache)
                        {
                            FileUtil.SaveBytesFile(AssetLoader.CachePath + path1, path2 + version + ".info", bytes);
                        }

                        succCallback(res);
                    }
                    else
                    {
                        Debug.Log("<color=#00CAFFFF>" + "Error code:::::::" + ret + "        >>> " + uri + "</color> ");

                        HttpErrorVo vo = new HttpErrorVo
                        {
                            ErrorCode = ret,
                            Cmd = _cmd,
                            CustomData = CustomerData,
                        };
                        errCallback?.Invoke(vo);
                    }
                }

                www.Dispose();
            }
        }
    }
}