using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Framework.GalaSports.Service.Socket
{
    /// <summary>
    /// 调用者(Unity安全调用者)
    /// </summary>
    public class UnityInvoke : IDisposable
    {

        /// <summary>
        /// GameObject
        /// </summary>
        private class UnityInvokeScript : MonoBehaviour
        {
            private List<Action<int>> functions = new List<Action<int>>();
            private List<int> functionsParams = new List<int>();
            private object syncRoot = new object();

            public void Push(Action<int> method, int param)
            {
                lock (syncRoot)
                {
                    functions.Add(method);
                    functionsParams.Add(param);
                }

            }
            void Update()
            {
                List<Action<int>> list = null;
                List<int> listParams = null;
                lock (syncRoot)
                {
                    list = functions;
                    listParams = functionsParams;
                    functions = new List<Action<int>>();
                    functionsParams = new List<int>();
                }
                var length = list.Count;
                for (int i = 0; i < length; i++)
                {
                    list[i].Invoke(listParams[i]);
                }
            }
        }

        private UnityInvokeScript script;
        private bool isDisposed;

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="name"></param>
        public UnityInvoke(string name = "UnityInvoke", Transform parent = null)
        {
            var gameObject = new GameObject(name);
            script = gameObject.AddComponent<UnityInvokeScript>();
            if (parent != null)
            {

                gameObject.transform.SetParent(parent.transform);
            }
            else
            {
                gameObject.transform.SetParent(Camera.main.transform.parent);
                // gameObject.transform.SetParent(AppDelegate.ScriptObject.transform);
            }

            isDisposed = false;
        }

        /// <summary>
        /// 安全调用
        /// </summary>
        /// <param name="action"></param>
        ///<param name="param"></param>
        public void Call(Action<int> action, int param)
        {
            script.Push(action, param);
        }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected void Dispose(bool disposing)
        {
            if (!isDisposed)
            {
                if (disposing)
                {
                    if (script != null)
                    {
                        UnityEngine.Object.Destroy(script.gameObject);
                        script = null;
                    }
                }
                isDisposed = true;
            }
        }
        ~UnityInvoke()
        {
            Dispose(false);
        }
    }

}