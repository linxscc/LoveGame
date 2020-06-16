using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Google.Protobuf;
using UnityEngine;

namespace Assets.Scripts.Framework.GalaSports.Service.Socket
{
    public class ProtobuffMsg<T> where T : IMessage<T>
    {
        private List<Action<T>> handlerList = new List<Action<T>>();
        private T res;

        public void Decode(byte[] data)
        {
            MemoryStream m = new MemoryStream(data);

            //res = Serializer.Deserialize<T>(m);
            try
            {
                PropertyInfo resType = res.GetType().GetProperty("ret");
                if (resType != null)
                {
                    int ret = (int)resType.GetValue(res, null);
                    if (ret != 0)
                    {
                        Debug.LogError("Pvp Server error " + ret);
                    }
                }
            }
            catch (Exception err)
            {
                Debug.Log("err " + err.Message);
            }
        }

        public void DecodeAndEmit(byte[] data)
        {
            Decode(data);
            Emit();
        }

        public byte[] Encode(T data)
        {
            byte[] buffer = null;
            using (MemoryStream m = new MemoryStream())
            {
                data.WriteTo(m);
                m.Position = 0;
                int length = (int)m.Length;
                buffer = new byte[length];
                m.Read(buffer, 0, length);
            }
            return buffer;
        }

        public bool containsHandler(Action<T> handler)
        {
            return handlerList.IndexOf(handler) >= 0;
        }

        public void addPush(Action<T> handler)
        {
            handlerList.Add(handler);
        }

        public void removePush(Action<T> handler)
        {
            handlerList.Remove(handler);
        }

        public void removePush()
        {
            handlerList.Clear();
        }

        public void Emit(T re)
        {
            foreach (var item in handlerList)
            {
                item.Invoke(re);
            }
        }

        public void Emit()
        {
            foreach (var item in handlerList)
            {
                item.Invoke(res);
            }
        }

    }
}
