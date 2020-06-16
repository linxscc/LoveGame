using System;
using System.Collections.Generic;
using System.IO;
using Componets;
using Google.Protobuf;
using UnityEngine;
using Com.Proto.Server;
using Google.Protobuf.Collections;

namespace Assets.Scripts.Framework.GalaSports.Service
{
    public class NetWorkManager : MonoBehaviour
    {
        public const int SERVER_HIDDEN = 0;
        public const int SERVER_CLOSE = 1;
        public const int SERVER_MAINTAIN = 2;
        public const int SERVER_NEW = 3;
        public const int SERVER_RECOMMEND = 4;
        public const int SERVER_HOT = 5;

        public static string Serverurl;


        public List<string> serverIds = new List<string>();
        private Dictionary<string, GameServerInfoPB> _serverDatas = new Dictionary<string, GameServerInfoPB>();

        public static string CookieStr;
        private static NetWorkManager _instance;

        public MonoObject MonoGameObjectScript;

        private Dictionary<string, string> _headers;
        public static Dictionary<string, string> Headers;

        public Action<HttpErrorVo> GlobalNetErrorHandler;
        /// <summary>
        /// Http超时
        /// </summary>
        public static int HttpTimeOut = 15;

        private List<IHttpMessage> _messageList = new List<IHttpMessage>();

        void Awake()
        {
            _instance = this;
            MonoGameObjectScript = transform.gameObject.AddComponent<MonoObject>();
            InitHead();
        }

        /// <summary>
        /// 单例属性访问器
        /// </summary>
        public static NetWorkManager Instance
        {
            get { return _instance; }
        }

        public void SetServer(string url)
        {
            Debug.Log("SetServer" + url);
            Serverurl = url;
        }

        public GameServerInfoPB SetServerDatas(RepeatedField<GameServerInfoPB> list)
        {
            serverIds.Clear();
            _serverDatas.Clear();
            for (int i = 0; i < list.Count; ++i)
            {
                Debug.LogWarning("server:  id: " + list[i].ServerId + "  ip: " + list[i].Addr + "  name:" + list[i].Name + "  order:" + list[i].Sort + " status:" + list[i].Status);
                if (list[i].Status == SERVER_HIDDEN) continue;
                if (!_serverDatas.ContainsKey(list[i].ServerId))
                {
                    serverIds.Add(list[i].ServerId);
                    _serverDatas.Add(list[i].ServerId, list[i]);
                }
            }

            serverIds.Sort((a, b)=>
            {
                GameServerInfoPB date_a = GetGameServerData(a);
                GameServerInfoPB date_b = GetGameServerData(b);
                return date_b.Sort.CompareTo(date_a.Sort);
            });

            GameServerInfoPB result = null;
            if (serverIds.Count > 0)
                result = GetGameServerData(serverIds[0]);
            return result;

            //serverIds.Add(list[0].ServerId + "_1");
            //_serverDatas.Add(list[0].ServerId + "_1", list[0]);
        }

        public GameServerInfoPB GetGameServerData(string serverId)
        {
            if (_serverDatas.ContainsKey(serverId))
                return _serverDatas[serverId];
            return null;
        }

        public void ChooseServer(string serverId)
        {
            GameServerInfoPB data = GetGameServerData(serverId);
            if (data == null) return;
            AppConfig.Instance.logicServer = "http://" + data.Addr + ":" + data.Port + "/";
            NetWorkManager.Instance.SetServer(AppConfig.Instance.logicServer);
            AppConfig.Instance.serverId = data.ServerId;
            AppConfig.Instance.serverName = data.Name;

            Debug.LogWarning("ChooseServer: serverName" + AppConfig.Instance.serverName);
        }

        public static byte[] GetByteData<T>(T msg) where T : IMessage<T>
        {
            byte[] buffer = null;
            using (MemoryStream m = new MemoryStream())
            {
                msg.WriteTo(m);
                m.Position = 0;
                int length = (int) m.Length;
                buffer = new byte[length];
                m.Read(buffer, 0, length);
            }

            if (buffer.Length == 0)
                return null;

            return buffer;
        }

        private void SendMsg()
        {
            IHttpMessage msg = _messageList[0];
            _messageList.Remove(msg);
            msg.RequestHandler();
        }

        public void Send(IHttpMessage msg)
        {
            if (Serverurl != null)
            {
                _messageList.Add(msg);
                SendMsg();
            }
            else
            {
                Debug.LogError("NetWorkManager.SERVER 为空！");
            }
        }

        public HttpMessage<T> Send<T>(string cmd, byte[] data, Action<T> succCallback, object customerData,
            Action<HttpErrorVo> errCallback = null, bool cache = false, string version = "", bool ignoreResponse = false,
            string serverUrl = null, int httpTimeout = -1) where T : IMessage<T>, new()
        {
            HttpMessage<T> msg = null;
            if (Serverurl != null || serverUrl != null)
            {
                msg = new HttpMessage<T>(cmd, data, succCallback, customerData, delegate(HttpErrorVo vo)
                {
                    if (errCallback != null)
                        errCallback(vo);

                    GlobalNetErrorHandler(vo);
                    
                    LoadingOverlay.Instance.Hide();
                }, cache, version, ignoreResponse, serverUrl, httpTimeout);
                _messageList.Add(msg);
                SendMsg();
            }
            else
            {
                if (errCallback != null)
                {
                    HttpErrorVo vo = new HttpErrorVo
                    {
                        Cmd = cmd,
                        CustomData = customerData,
                        ErrorCode = 999,
                        ErrorString = "SERVER NULL"
                    };
                    errCallback(vo);
                    Debug.Log("NetWorkManager.SERVER 为空！");
                }
            }
            return msg;
        }

        public HttpMessage<T> Send<T>(string cmd, byte[] data, Action<T> succCallback, Action<HttpErrorVo> errCallback = null,
            bool cache = false, string version = "", bool ignoreResponse = false, string serverUrl = null, int httpTimeout = -1)
            where T : IMessage<T>, new()
        {
            return Send(cmd, data, succCallback, null, errCallback, cache, version, ignoreResponse, serverUrl);
        }

        

       

        public static void InitHead()
        {
            Headers = new Dictionary<string, string>();
            Headers["Content-Type"] = "application/x-protobuf";
            Headers["Accept"] = "application/x-protobuf";
            if (CookieStr != null)
            {
                Headers.Add("Cookie", CookieStr);
            }
            else
            {
                Debug.LogWarning("InitHead Headers.Remove('Cookie');");
                Headers.Remove("Cookie");
            }
        }

        public void Clean()
        {
            _messageList?.Clear();
        }
    }
}